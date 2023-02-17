using ISD.Constant;
using ISD.EntityModels;
using ISD.Repositories.Infrastructure.Extensions;
using ISD.ViewModels;
using ISD.ViewModels.Customer;
using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;

namespace ISD.Repositories
{
    public class AppointmentRepository
    {
        EntityDataContext _context;
        private UtilitiesRepository _utilitiesRepository;
        /// <summary>
        /// Khởi tạo Appointment Repository
        /// </summary>
        /// <param name="dataContext">Entity Data Context</param>
        public AppointmentRepository(EntityDataContext dataContext)
        {
            _context = dataContext;
            _context.Database.CommandTimeout = 1800;
            _utilitiesRepository = new UtilitiesRepository();
        }

        #region Function Create
        /// <summary>
        /// Thêm mới Appointment
        /// </summary>
        /// <param name="appointmentViewModel">View Model Appointment</param>
        /// <returns>AppointmentViewModel</returns>
        public AppointmentViewModel Create(AppointmentViewModel appointmentViewModel)
        {
            //Tạo ID
            var taskId = Guid.NewGuid();
            appointmentViewModel.AppointmentId = taskId;
            appointmentViewModel.TaskId = taskId;

            //Create new Appointment
            var appointmentNew = new AppointmentModel();
            appointmentNew.MapAppointment(appointmentViewModel);

            //Create new Task
            var taskNew = new TaskModel();
            taskNew.MapTaskFromAppointmentVM(appointmentViewModel);
            taskNew.CreateBy = appointmentViewModel.CreateBy;
            taskNew.CreateTime = appointmentViewModel.CreateTime;
            taskNew.Actived = appointmentViewModel.Actived;
            taskNew.DateKey = _utilitiesRepository.ConvertDateTimeToInt(appointmentViewModel.VisitDate);

            //Assginee
            var assignee = new TaskAssignModel();
            assignee.TaskAssignId = Guid.NewGuid();
            assignee.TaskId = taskId;
            assignee.SalesEmployeeCode = appointmentViewModel.SaleEmployeeCode;
            assignee.CreateBy = appointmentViewModel.CreateBy;
            assignee.CreateTime = DateTime.Now;

            //Rating
            if (!string.IsNullOrEmpty(appointmentViewModel.Reviews) || !string.IsNullOrEmpty(appointmentViewModel.Ratings))
            {
                var rating = new RatingModel();
                rating.RatingId = Guid.NewGuid();
                rating.RatingTypeCode = ConstCatalogType.CustomerReviews;
                rating.ReferenceId = taskId;
                rating.Ratings = appointmentViewModel.Ratings;
                rating.Reviews = appointmentViewModel.Reviews;

                _context.Entry(rating).State = EntityState.Added;
            }

            _context.Entry(appointmentNew).State = EntityState.Added;
            _context.Entry(taskNew).State = EntityState.Added;
            _context.Entry(assignee).State = EntityState.Added;

            return appointmentViewModel;
        }
        #endregion

        /// <summary>
        /// Cập nhật Appointment
        /// </summary>
        /// <param name="appointmentViewModel">Appointment ViewModel</param>
        public void Update(AppointmentViewModel appointmentViewModel)
        {
            //Update TaskModel
            var taskInDb = _context.TaskModel.FirstOrDefault(p => p.TaskId == appointmentViewModel.AppointmentId);
            //Mapping
            appointmentViewModel.TaskId = appointmentViewModel.AppointmentId;
            taskInDb.MapTaskFromAppointmentVM(appointmentViewModel);
            taskInDb.LastEditBy = appointmentViewModel.LastEditBy;
            taskInDb.LastEditTime = appointmentViewModel.LastEditTime;

            _context.Entry(taskInDb).State = EntityState.Modified;

            //Update AppointmentModel
            var appointmentInDb = _context.AppointmentModel.FirstOrDefault(p => p.AppointmentId == appointmentViewModel.AppointmentId);
            //Mapping
            appointmentInDb.MapAppointment(appointmentViewModel);
            _context.Entry(appointmentInDb).State = EntityState.Modified;

            //Rating
            var rating = _context.RatingModel.Where(p => p.ReferenceId == appointmentViewModel.AppointmentId && p.RatingTypeCode == ConstCatalogType.CustomerReviews).FirstOrDefault();
            if (rating != null)
            {
                rating.Reviews = appointmentViewModel.Reviews;
                rating.Ratings = appointmentViewModel.Ratings;
                _context.Entry(rating).State = EntityState.Modified;
            }
            else
            {
                //Rating
                if (!string.IsNullOrEmpty(appointmentViewModel.Reviews) || !string.IsNullOrEmpty(appointmentViewModel.Ratings))
                {
                    rating = new RatingModel();
                    rating.RatingId = Guid.NewGuid();
                    rating.RatingTypeCode = ConstCatalogType.CustomerReviews;
                    rating.ReferenceId = appointmentViewModel.AppointmentId;
                    rating.Ratings = appointmentViewModel.Ratings;
                    rating.Reviews = appointmentViewModel.Reviews;

                    _context.Entry(rating).State = EntityState.Added;
                }

            }
        }

        #region Function search
        public List<AppointmentViewModel> Search(Guid? profileId)
        {
            var listResult = (from a in _context.AppointmentModel
                              join t in _context.TaskModel on a.AppointmentId equals t.TaskId
                              // join c in _context.CatalogModel on a.CustomerClassCode equals c.CatalogCode
                              // join ca in _context.CatalogModel on a.CategoryCode equals ca.CatalogCode
                              join sr in _context.CatalogModel on a.ShowroomCode equals sr.CatalogCode into srTemp
                              from shr in srTemp.DefaultIfEmpty()
                                  //  join pr in _context.CatalogModel on t.PriorityCode equals pr.CatalogCode
                                  //join cn in _context.CatalogModel on a.ChannelCode equals cn.CatalogCode
                              join p in _context.ProfileModel on t.ProfileId equals p.ProfileId
                              //  join cm in _context.CompanyModel on t.CompanyId equals cm.CompanyId
                              join s in _context.StoreModel on t.StoreId equals s.StoreId
                              //join w in _context.WorkFlowModel on t.WorkFlowId equals w.WorkFlowId
                              join sa in _context.SalesEmployeeModel on a.SaleEmployeeCode equals sa.SalesEmployeeCode into sag
                              from emp in sag.DefaultIfEmpty()
                                  //join pc in _context.ProfileModel on a.PrimaryContactId equals pc.ProfileId
                              join ts in _context.TaskStatusModel on t.TaskStatusId equals ts.TaskStatusId

                              where t.ProfileId == profileId && t.isDeleted != true && (shr.CatalogTypeCode == null || shr.CatalogTypeCode == Constant.ConstCatalogType.CustomerSource)

                              orderby a.VisitDate descending
                              select new AppointmentViewModel
                              {
                                  //1. GUID
                                  AppointmentId = a.AppointmentId,
                                  TaskId = t.TaskId,
                                  TaskCode = t.TaskCode,
                                  //2. Chủ đề
                                  Summary = t.Summary,
                                  //3. Khách hàng
                                  ProfileId = t.ProfileId,
                                  ProfileName = p.ProfileName,
                                  ProfileCode = p.ProfileCode,
                                  Address = p.Address,
                                  //4. Công ty
                                  CompanyId = t.CompanyId,
                                  //CompanyName =cm.CompanyName,
                                  //5. Chi nhánh
                                  StoreId = t.StoreId,
                                  StoreName = s.StoreName,
                                  //6. Liên hệ chính
                                  // PrimaryContactId = a.PrimaryContactId,
                                  // PrimaryContactName = pc.ProfileName,
                                  //7. Thời gian bắt đầu
                                  StartDate = t.StartDate,
                                  //8. Thời gian kết thúc
                                  EndDate = t.EndDate,
                                  //9. Phân loại Khách hàng
                                  CustomerClassCode = a.CustomerClassCode,
                                  //CustomerClassName = c.CatalogText_vi,
                                  //10. Danh mục
                                  CategoryCode = a.CategoryCode,
                                  // CategoryName = ca.CatalogText_vi,
                                  //11. Địa điểm khách ghé
                                  ShowroomCode = a.ShowroomCode,
                                  ShowroomName = shr.CatalogText_vi,
                                  //12. Mức độ
                                  PriorityCode = t.PriorityCode,
                                  //PriorityName = pr.CatalogText_vi,
                                  //13. Nhiệm vụ
                                  WorkFlowId = t.WorkFlowId,
                                  //WorkFlowName = w.WorkFlowName,
                                  //14. Người tiếp khách
                                  SaleEmployeeCode = a.SaleEmployeeCode,
                                  SaleEmployeeName = emp.SalesEmployeeName,
                                  //15. Ghi chú
                                  Description = t.Description,
                                  //16. Khách biết đến ACN qua
                                  ChannelCode = a.ChannelCode,
                                  //ChannelName = cn.CatalogText_vi,
                                  //17. Ngày ghé thăm
                                  VisitDate = a.VisitDate,
                                  //18. Trạng thái
                                  TaskStatusName = ts.TaskStatusName,
                                  //Thời gian tạo
                                  CreateTime = t.CreateTime
                              }).ToList();
            return listResult;
        }
        #endregion

        #region  Search Query
        public IQueryable<AppointmentViewModel> SearchQuery(AppointmentSearchViewModel searchModel)
        {
            IQueryable<AppointmentViewModel> query = (from a in _context.AppointmentModel
                         join t in _context.TaskModel on a.AppointmentId equals t.TaskId
                         //Nguồn khách hàng
                         join sr in _context.CatalogModel on new { CustomerSource = a.ShowroomCode, Type = ConstCatalogType.CustomerSource } equals new { CustomerSource = sr.CatalogCode, Type = sr.CatalogTypeCode } into srTemp
                         from shr in srTemp.DefaultIfEmpty()
                             //Account
                         join p in _context.ProfileModel on t.ProfileId equals p.ProfileId
                         //Phân loại khách hàng: Doanh nghiệp/Tiêu dùng
                         //Profile Type
                         let profileType = _context.ProfileTypeModel.Where(p3 => p3.ProfileId == p.ProfileId && p3.CompanyCode == searchModel.CompanyCode).FirstOrDefault()
                         //CustomerType: Bussiness, Individual Customers, Contact
                         join customerType in _context.CatalogModel on new { CustomerType = profileType.CustomerTypeCode, Type = ConstCatalogType.CustomerType } equals new { CustomerType = customerType.CatalogCode, Type = customerType.CatalogTypeCode } into cGroup
                         from cG in cGroup.DefaultIfEmpty()
                             //Nhóm khách hàng
                         join customerGroup in _context.CatalogModel on new { CustomerGroupCode = p.CustomerGroupCode, Type = ConstCatalogType.CustomerCategory } equals new { CustomerGroupCode = customerGroup.CatalogCode, Type = customerGroup.CatalogTypeCode } into cgGroup
                         from cgG in cgGroup.DefaultIfEmpty()
                             //Chi nhánh
                         join s in _context.StoreModel on t.StoreId equals s.StoreId
                         //Khu vực
                         join sof in _context.CatalogModel on new { Area = s.Area, Type = "SaleOffice" } equals new { Area = sof.CatalogCode, Type = sof.CatalogTypeCode } into sofTmpList
                         from saleOffice in sofTmpList.DefaultIfEmpty()
                             //Nhân viên tiếp khách
                         join sa in _context.SalesEmployeeModel on a.SaleEmployeeCode equals sa.SalesEmployeeCode into sag
                         from emp in sag.DefaultIfEmpty()
                             //Trạng thái xử lý YC của khách hàng
                         join ts in _context.TaskStatusModel on t.TaskStatusId equals ts.TaskStatusId
                         //Ngành nghề
                         join customerCareer in _context.CatalogModel on new { CustomerCareerCode = p.CustomerCareerCode, Type = ConstCatalogType.CustomerCareer } equals new { CustomerCareerCode = customerCareer.CatalogCode, Type = customerCareer.CatalogTypeCode } into careerGroup
                         from crG in careerGroup.DefaultIfEmpty()
                             //Province
                         join pr in _context.ProvinceModel on p.ProvinceId equals pr.ProvinceId into prG
                         from province in prG.DefaultIfEmpty()
                             //District
                         join d in _context.DistrictModel on p.DistrictId equals d.DistrictId into dG
                         from district in dG.DefaultIfEmpty()
                             //Ward
                         join w in _context.WardModel on p.WardId equals w.WardId into wG
                         from ward in wG.DefaultIfEmpty()
                             //Thông tin liên hệ: Primary contact
                             //join pc in _context.ProfileModel on a.PrimaryContactId equals pc.ProfileId into pcG
                             //from primaryContact in pcG.DefaultIfEmpty()
                        let primaryContact = (from profileContactAttribute in _context.ProfileContactAttributeModel
                                            join profileCT in _context.ProfileModel on profileContactAttribute.ProfileId equals profileCT.ProfileId
                                            where profileContactAttribute.ProfileId == a.PrimaryContactId
                                            orderby profileContactAttribute.IsMain == true descending
                                            select new ProfileSearchResultViewModel()
                                            {
                                                ContactName = profileCT.ProfileName,
                                                ContactPhone = profileCT.Phone,
                                                ContactEmail = profileCT.Email,
                                            }).FirstOrDefault()
                         where
                         //Khu vực
                         //(searchModel.SaleOfficeCode == null || searchModel.SaleOfficeCode.Count() == 0 || searchModel.SaleOfficeCode.Contains(saleOffice.CatalogCode))
                         // 9. Search by Độ tuổi
                         (searchModel.Age == null || p.Age == searchModel.Age)
                         //Hỗ trợ join với bảng catalogue => chỉ lấy loại là CustomerSource
                         //&& (a.ShowroomCode == null || shr.CatalogTypeCode == ConstCatalogType.CustomerSource)
                         //Chưa bị xóa
                         && t.isDeleted != true
                         //Phân loại KH
                         //&& (p.CustomerTypeCode == null || cG.CatalogTypeCode == ConstCatalogType.CustomerType)
                         //Nhóm KH
                         //&& (p.CustomerGroupCode == null || cgG.CatalogTypeCode == ConstCatalogType.CustomerCategory)
                         //Ngành nghề
                         //&& ( (bG != null && bG.CustomerCareerCode != null && crG.CatalogTypeCode == ConstCatalogType.CustomerCareer))
                         orderby a.VisitDate descending
                         select new AppointmentViewModel
                         {
                             //1. GUID
                             AppointmentId = a.AppointmentId,
                             TaskId = t.TaskId,
                             TaskCode = t.TaskCode,
                             //2. Chủ đề
                             Summary = t.Summary,
                             //3. Khách hàng
                             ProfileId = t.ProfileId,
                             ProfileName = p.ProfileName,
                             ProfileCode = p.ProfileCode,
                             Address = p.Address
                                        + (ward == null ? "" : ", " + ward.Appellation + " " + ward.WardName)
                                        + (district == null ? "" : ", " + district.Appellation + " " + district.DistrictName)
                                        + (province == null ? "" : ", " + province.ProvinceName),
                             Email = p.Email,
                             //4. Công ty
                             CompanyId = t.CompanyId,
                             //5. Chi nhánh
                             StoreId = t.StoreId,
                             StoreName = s.StoreName,
                             //7. Thời gian bắt đầu
                             //StartDate = t.StartDate,
                             //8. Thời gian kết thúc
                             //EndDate = t.EndDate,
                             //9. Phân loại Khách hàng
                             CustomerClassCode = a.CustomerClassCode,
                             //CustomerTypeCode = p.CustomerTypeCode,
                             CustomerTypeCode = profileType.CustomerTypeCode,
                             CustomerTypeName = cG.CatalogText_vi,
                             //10. Danh mục
                             CategoryCode = a.CategoryCode,
                             //11. Địa điểm khách ghé
                             ShowroomCode = a.ShowroomCode,
                             ShowroomName = shr.CatalogText_vi,
                             //12. Mức độ
                             PriorityCode = t.PriorityCode,
                             //13. Nhiệm vụ
                             WorkFlowId = t.WorkFlowId,
                             //14. Người tiếp khách
                             SaleEmployeeCode = a.SaleEmployeeCode,
                             SaleEmployeeName = emp.SalesEmployeeName,
                             //15. Ghi chú
                             Description = t.Description,
                             //16. Khách biết đến ACN qua
                             ChannelCode = a.ChannelCode,
                             //17. Ngày ghé thăm
                             VisitDate = a.VisitDate,
                             //18. Trạng thái
                             TaskStatusName = ts.TaskStatusName,
                             //19. Số điện thoại
                             Phone = p.Phone,
                             //Nhóm khách hàng
                             CustomerGroupCode = p.CustomerGroupCode,
                             CustomerGroupName = cgG.CatalogText_vi,
                             //Mã số thuế
                             TaxNo = p.TaxNo,
                             //Ngành nghề
                             CustomerCareerCode = p.CustomerCareerCode,
                             CustomerCareerName = crG.CatalogText_vi,
                             //Ngày tiếp nhận
                             ReceiveDate = t.ReceiveDate,
                             //Ngày tạo
                             CreateTime = t.CreateTime,
                             //Gửi SMS cho khách hàng
                             isSentSMS = a.isSentSMS,
                             //Thông tin người liên hệ
                             ContactName = primaryContact != null ? primaryContact.ContactName : "",
                             ContactPhone = primaryContact != null ? primaryContact.ContactPhone : "",
                             ContactEmail = primaryContact != null ? primaryContact.ContactEmail : "",
                         });

            //1. Công ty
            if (searchModel.CompanyId != null)
            {
                query = query.Where(p => searchModel.CompanyId == p.CompanyId);
            }
            if (!string.IsNullOrEmpty(searchModel.CompanyCode))
            {
                Guid companyId = _context.CompanyModel.Where(p => p.CompanyCode == searchModel.CompanyCode).Select(p => p.CompanyId).FirstOrDefault();
                if (companyId != Guid.Empty)
                {
                    query = query.Where(p => p.CompanyId == companyId);
                }
            }
            //2. Chi nhánh
            if (searchModel.StoreId != null && searchModel.StoreId.Count > 0)
            {
                query = query.Where(p => searchModel.StoreId.Contains(p.StoreId));
            }
            if (!string.IsNullOrEmpty(searchModel.SaleOrgCode))
            {
                var store = _context.StoreModel.Where(p => p.SaleOrgCode == searchModel.SaleOrgCode).FirstOrDefault();
                if (store != null)
                {
                    query = query.Where(p => store.StoreId == p.StoreId);
                }
            }
            //3,4.Ngày ghé thăm
            //3. Từ ngày
            if (searchModel.FromDate != null)
            {
                query = query.Where(p => searchModel.FromDate <= p.VisitDate);
            }
            //4. Đến ngày
            if (searchModel.ToDate != null)
            {
                query = query.Where(p => p.VisitDate <= searchModel.ToDate);
            }
            //var b = query.Select(p => p.SaleEmployeeCode).ToList();
            //5. Search by Nguồn khách hàng (địa điểm khách ghé)
            if (searchModel.CustomerSourceCode != null && searchModel.CustomerSourceCode.Count > 0 && !searchModel.CustomerSourceCode.Contains(string.Empty))
            {
                query = query.Where(p => searchModel.CustomerSourceCode.Contains(p.ShowroomCode));
            }
            //6. Search by nhân viên tiếp khách
            if (!string.IsNullOrEmpty(searchModel.SalesEmployeeCode))
            {
                query = query.Where(p => searchModel.SalesEmployeeCode == p.SaleEmployeeCode);
            }
            //7. Loại (Doanh nghiệp | Tiêu dùng)
            if (!string.IsNullOrEmpty(searchModel.CustomerTypeCode))
            {
                query = query.Where(p => searchModel.CustomerTypeCode == p.CustomerTypeCode);
            }
            //8. Phân loại Khách hàng (Cũ | Mới)
            if (!string.IsNullOrEmpty(searchModel.CustomerClassCode))
            {
                query = query.Where(p => searchModel.CustomerClassCode == p.CustomerClassCode);
            }
            //9. Độ tuổi
            //đã tìm ở trên
            //10. Trạng thái xử lý yêu cầu của khách hàng
            if (searchModel.TaskStatusId != null)
            {
                query = query.Where(p => searchModel.TaskStatusId == p.TaskStatusId);
            }
            //11. Search by nhóm khách hàng 
            //if (searchModel.CustomerGroupCode != null && searchModel.CustomerGroupCode.Count() > 0 && !string.IsNullOrEmpty(searchModel.CustomerGroupCode.FirstOrDefault()))
            //{
            //    query = query.Where(p => searchModel.CustomerGroupCode.Contains(p.CustomerGroupCode));
            //}
            //12. Số điện thoại
            if (!string.IsNullOrEmpty(searchModel.Phone))
            {
                query = query.Where(p => p.Phone.Contains(searchModel.Phone));
            }
            //13. Mã số thuế
            if (!string.IsNullOrEmpty(searchModel.TaxNo))
            {
                query = query.Where(p => p.TaxNo.Contains(searchModel.TaxNo));
            }
            //14.Nhóm khách hàng
            if (searchModel.CustomerGroupCode != null && searchModel.CustomerGroupCode.Count() > 0 && !searchModel.CustomerSourceCode.Contains(string.Empty))
            {
                query = query.Where(p => searchModel.CustomerGroupCode.Contains(p.CustomerGroupCode));
            }
            //15.Ngành nghề
            if (!string.IsNullOrEmpty(searchModel.CustomerCareerCode))
            {
                query = query.Where(p => p.CustomerCareerCode == searchModel.CustomerCareerCode);
            }
            //15,16.Ngày ghé thăm
            //15. Từ ngày
            if (searchModel.ReceiveFromDate != null)
            {
                query = query.Where(p => searchModel.ReceiveFromDate <= p.ReceiveDate);
            }
            //16. Đến ngày
            if (searchModel.ReceiveToDate != null)
            {
                query = query.Where(p => p.ReceiveDate <= searchModel.ReceiveToDate);
            }
            //17,18.Ngày kết thúc
            //17. Từ ngày
            if (searchModel.EndFromDate != null)
            {
                query = query.Where(p => searchModel.EndFromDate <= p.EndDate);
            }
            //18. Đến ngày
            if (searchModel.EndToDate != null)
            {
                query = query.Where(p => p.EndDate <= searchModel.EndToDate);
            }

            //17,18.Ngày tạo
            //17. Từ ngày
            if (searchModel.CreateFromDate != null)
            {
                query = query.Where(p => searchModel.CreateFromDate <= p.CreateTime);
            }
            //18. Đến ngày
            if (searchModel.CreateToDate != null)
            {
                query = query.Where(p => p.CreateTime <= searchModel.CreateToDate);
            }
            //Khách hàng
            if (searchModel.ProfileId != null)
            {
                query = query.Where(p => p.ProfileId == searchModel.ProfileId);
            }
            //if (searchModel.SaleOfficeCode != null && searchModel.SaleOfficeCode.Count() > 0)
            //{
            //    query = query.Where(p => searchModel.SaleOfficeCode.Contains(p.));
            //}

            return query;
        }
        #endregion Search Query

        #region Search Query Appointment
        public List<AppointmentViewModel> SearchQueryAppointment(AppointmentSearchViewModel searchModel, string CurrentCompanyCode, out int filteredResultsCount)
        {
            //Parameters for your query
            #region CreateAtCompany
            string CreateAtCompany = string.Empty;
            if (searchModel.CompanyId != null)
            {
                var company = _context.CompanyModel.Where(p => p.CompanyId == searchModel.CompanyId).FirstOrDefault();
                if (company != null)
                {
                    CreateAtCompany = company.CompanyCode;
                }
            }
            #endregion CreateAtCompany

            #region CreateAtSaleOrg
            //Build your record
            var tableCreateAtSaleOrgSchema = new List<SqlMetaData>(1)
                {
                    new SqlMetaData("Code", SqlDbType.NVarChar, 100)
                }.ToArray();

            //And a table as a list of those records
            var tableCreateAtSaleOrg = new List<SqlDataRecord>();
            List<string> saleOrgLst = new List<string>();
            if (searchModel.StoreId != null && searchModel.StoreId.Count > 0)
            {
                foreach (var r in searchModel.StoreId)
                {
                    var tableRow = new SqlDataRecord(tableCreateAtSaleOrgSchema);
                    var store = _context.StoreModel.Where(p => p.StoreId == r).FirstOrDefault();
                    if (store != null)
                    {
                        tableRow.SetString(0, store.SaleOrgCode);
                        if (!saleOrgLst.Contains(store.SaleOrgCode))
                        {
                            saleOrgLst.Add(store.SaleOrgCode);
                            tableCreateAtSaleOrg.Add(tableRow);
                        }
                    }
                }
            }
            else
            {
                var tableRow = new SqlDataRecord(tableCreateAtSaleOrgSchema);
                tableCreateAtSaleOrg.Add(tableRow);
            }
            #endregion

            #region ShowroomCode (CustomerSourceCode)
            //Build your record
            var tableCustomerSourceCodeSchema = new List<SqlMetaData>(1)
                {
                    new SqlMetaData("Code", SqlDbType.NVarChar, 100)
                }.ToArray();

            //And a table as a list of those records
            var tableCustomerSourceCode = new List<SqlDataRecord>();
            List<string> customerSourceCodeLst = new List<string>();
            if (searchModel.CustomerSourceCode != null && searchModel.CustomerSourceCode.Count > 0)
            {
                foreach (var r in searchModel.CustomerSourceCode)
                {
                    var tableRow = new SqlDataRecord(tableCustomerSourceCodeSchema);
                    tableRow.SetString(0, r);
                    if (!customerSourceCodeLst.Contains(r))
                    {
                        customerSourceCodeLst.Add(r);
                        tableCustomerSourceCode.Add(tableRow);
                    }
                }
            }
            else
            {
                var tableRow = new SqlDataRecord(tableCustomerSourceCodeSchema);
                tableCustomerSourceCode.Add(tableRow);
            }
            #endregion

            string sqlQuery = "EXEC [Customer].[usp_SearchAppointment_Dynamic] @TaskCode, @ProfileName, @CompanyId, @CreateAtSaleOrg, @SalesEmployeeCode, @CustomerSourceCode, @CustomerGroupCode, @FromDate, @ToDate, @SaleOfficeCode, @CustomerTypeCode, @Age, @TaskStatusId, @Phone, @TaxNo, @CustomerCareerCode, @CreateFromDate, @CreateToDate, @CurrentCompanyCode, @PageSize, @PageNumber, @FilteredResultsCount OUT";
            var FilteredResultsCountOutParam = new SqlParameter();
            FilteredResultsCountOutParam.ParameterName = "FilteredResultsCount";
            FilteredResultsCountOutParam.SqlDbType = SqlDbType.Int;
            FilteredResultsCountOutParam.Direction = ParameterDirection.Output;

            #region Parameters
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter
                {
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input,
                    ParameterName = "TaskCode",
                    Value = searchModel.TaskCode ?? (object)DBNull.Value,
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input,
                    ParameterName = "ProfileName",
                    Value = searchModel.ProfileName ?? (object)DBNull.Value,
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.UniqueIdentifier,
                    Direction = ParameterDirection.Input,
                    ParameterName = "CompanyId",
                    Value = searchModel.CompanyId ?? (object)DBNull.Value,
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.Structured,
                    Direction = ParameterDirection.Input,
                    ParameterName = "CreateAtSaleOrg",
                    TypeName = "[dbo].[StringList]", //Don't forget this one!
                    Value = tableCreateAtSaleOrg
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input,
                    ParameterName = "SalesEmployeeCode",
                    Value = searchModel.SalesEmployeeCode ?? (object)DBNull.Value,
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.Structured,
                    Direction = ParameterDirection.Input,
                    ParameterName = "CustomerSourceCode",
                    TypeName = "[dbo].[StringList]", //Don't forget this one!
                    Value = tableCustomerSourceCode
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.Structured,
                    Direction = ParameterDirection.Input,
                    ParameterName = "CustomerGroupCode",
                    TypeName = "[dbo].[StringList]", //Don't forget this one!
                    Value = new UtilitiesRepository().ConvertTableFromListString(searchModel.CustomerGroupCode)

                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.DateTime,
                    Direction = ParameterDirection.Input,
                    ParameterName = "FromDate",
                    Value = searchModel.FromDate ?? (object)DBNull.Value
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.DateTime,
                    Direction = ParameterDirection.Input,
                    ParameterName = "ToDate",
                    Value = searchModel.ToDate ?? (object)DBNull.Value
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.Structured,
                    Direction = ParameterDirection.Input,
                    ParameterName = "SaleOfficeCode",
                    TypeName = "[dbo].[StringList]", //Don't forget this one!
                    Value = new UtilitiesRepository().ConvertTableFromListString(searchModel.SaleOfficeCode)
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input,
                    ParameterName = "CustomerTypeCode",
                    Value = searchModel.CustomerTypeCode ?? (object)DBNull.Value
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input,
                    ParameterName = "Age",
                    Value = searchModel.Age ?? (object)DBNull.Value
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.UniqueIdentifier,
                    Direction = ParameterDirection.Input,
                    ParameterName = "TaskStatusId",
                    Value = searchModel.TaskStatusId ?? (object)DBNull.Value,
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input,
                    ParameterName = "Phone",
                    Value = searchModel.Phone ?? (object)DBNull.Value
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input,
                    ParameterName = "TaxNo",
                    Value = searchModel.TaxNo ?? (object)DBNull.Value
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input,
                    ParameterName = "CustomerCareerCode",
                    Value = searchModel.CustomerCareerCode ?? (object)DBNull.Value
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.DateTime,
                    Direction = ParameterDirection.Input,
                    ParameterName = "CreateFromDate",
                    Value = searchModel.CreateFromDate ?? (object)DBNull.Value
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.DateTime,
                    Direction = ParameterDirection.Input,
                    ParameterName = "CreateToDate",
                    Value = searchModel.CreateToDate ?? (object)DBNull.Value
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input,
                    ParameterName = "CurrentCompanyCode",
                    Value = CurrentCompanyCode ?? (object)DBNull.Value,
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Input,
                    ParameterName = "PageSize",
                    Value = searchModel.PageSize,
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Input,
                    ParameterName = "PageNumber",
                    Value = searchModel.PageNumber,
                },
                FilteredResultsCountOutParam
            };
            #endregion

            var result = _context.Database.SqlQuery<AppointmentViewModel>(sqlQuery, parameters.ToArray()).ToList();
            var filteredResultsCountValue = FilteredResultsCountOutParam.Value;
            if (filteredResultsCountValue != null)
            {
                filteredResultsCount = Convert.ToInt32(filteredResultsCountValue);
            }
            else
            {
                filteredResultsCount = 0;
            }
            return result;
        }
        #endregion Search Query Appointment

        /// <summary>
        /// Lấy một appointment theo Id
        /// </summary>
        /// <param name="appointmentId">Guid: AppointmentId</param>
        /// <returns></returns>
        public AppointmentViewModel GetById(Guid appointmentId)
        {
            var appointment = (from a in _context.AppointmentModel
                               join t in _context.TaskModel on a.AppointmentId equals t.TaskId
                               join w in _context.WorkFlowModel on t.WorkFlowId equals w.WorkFlowId
                               join ts in _context.TaskStatusModel on t.TaskStatusId equals ts.TaskStatusId
                               join p in _context.ProfileModel on t.ProfileId equals p.ProfileId
                               //Province
                               join pr in _context.ProvinceModel on p.ProvinceId equals pr.ProvinceId into prG
                               from province in prG.DefaultIfEmpty()
                                   //District
                               join d in _context.DistrictModel on p.DistrictId equals d.DistrictId into dG
                               from district in dG.DefaultIfEmpty()
                                   //CreateBy
                               join create in _context.AccountModel on t.CreateBy equals create.AccountId
                               join cr in _context.SalesEmployeeModel on create.EmployeeCode equals cr.SalesEmployeeCode into crg
                               from cr1 in crg.DefaultIfEmpty()
                                   //EditBy
                               join modify in _context.AccountModel on t.LastEditBy equals modify.AccountId into mg
                               from m in mg.DefaultIfEmpty()
                               join md in _context.SalesEmployeeModel on m.EmployeeCode equals md.SalesEmployeeCode into mdg
                               from md1 in mdg.DefaultIfEmpty()
                                   //Rating
                               join rating in _context.RatingModel on new { ReferenceId = t.TaskId, RatingTypeCode = ConstCatalogType.CustomerReviews } equals new { ReferenceId = rating.ReferenceId.Value, RatingTypeCode = rating.RatingTypeCode } into rg
                               from r in rg.DefaultIfEmpty()

                               where a.AppointmentId == appointmentId && t.isDeleted != true
                               select new AppointmentViewModel
                               {
                                   //1. GUID
                                   AppointmentId = a.AppointmentId,
                                   TaskId = t.TaskId,
                                   TaskCode = t.TaskCode,
                                   //2. Chủ đề
                                   Summary = t.Summary,
                                   //3. Khách hàng
                                   ProfileId = t.ProfileId,
                                   ProfileName = p.ProfileName,
                                   ProfileAddress = p.Address,
                                   ProvinceName = province == null ? "" : ", " + province.ProvinceName,
                                   DistrictName = district == null ? "" : ", " + district.Appellation + " " + district.DistrictName,
                                   Phone = p.SAPPhone ?? p.Phone,
                                   Email = p.Email,
                                   //4. Công ty
                                   CompanyId = t.CompanyId,
                                   //CompanyName =cm.CompanyName,
                                   //5. Chi nhánh
                                   StoreId = t.StoreId,
                                   //6. Liên hệ chính
                                   PrimaryContactId = a.PrimaryContactId,
                                   //7. Thời gian bắt đầu
                                   ReceiveDate = t.ReceiveDate,
                                   StartDate = t.StartDate,
                                   //8. Thời gian kết thúc
                                   EstimateEndDate = t.EstimateEndDate,
                                   EndDate = t.EndDate,
                                   //9. Phân loại Khách hàng
                                   CustomerClassCode = a.CustomerClassCode,
                                   //10. Danh mục
                                   CategoryCode = a.CategoryCode,
                                   //11. Địa điểm khách ghé
                                   ShowroomCode = a.ShowroomCode,
                                   //12. Mức độ
                                   PriorityCode = t.PriorityCode,
                                   //PriorityName = pr.CatalogText_vi,
                                   //13. Nhiệm vụ
                                   WorkFlowId = t.WorkFlowId,
                                   WorkFlowCode = w.WorkFlowCode,
                                   WorkFlowName = w.WorkFlowName,
                                   ProcessCode = ts.ProcessCode,
                                   //14. Người tiếp khách
                                   SaleEmployeeCode = a.SaleEmployeeCode,
                                   //15. Ghi chú
                                   Description = t.Description,
                                   //16. Khách biết đến ACN qua
                                   ChannelCode = a.ChannelCode,
                                   //17. Ngày ghé thăm
                                   VisitDate = a.VisitDate,
                                   //18. Trạng thái
                                   TaskStatusId = t.TaskStatusId,
                                   //19. NV phân công/giám sát
                                   Reporter = t.Reporter,
                                   //Thông tin tạo/ sửa chữa
                                   CreateBy = t.CreateBy,
                                   CreateTime = t.CreateTime,
                                   CreateByName = cr1.SalesEmployeeCode + " | " + cr1.SalesEmployeeName,
                                   LastEditBy = t.LastEditBy,
                                   LastEditTime = t.LastEditTime,
                                   LastEditByName = md1 != null ? md1.SalesEmployeeCode + " | " + md1.SalesEmployeeName : "",
                                   Requirement = a.Requirement,
                                   CreateByFullName = cr1.SalesEmployeeName,
                                   LastEditByFullName = md1.SalesEmployeeName,
                                   //Đề xuất
                                   SaleEmployeeOffer = a.SaleEmployeeOffer,
                                   //Nhận xét
                                   Reviews = r.Reviews,
                                   Ratings = r.Ratings,
                                   //Ghé thăm Cabinet Pro
                                   isVisitCabinetPro = a.isVisitCabinetPro.HasValue ? a.isVisitCabinetPro : false,
                                   VisitCabinetProRequest = a.VisitCabinetProRequest,
                               }).FirstOrDefault();

            if (appointment != null)
            {
                #region Survey
                appointment.surveyList = new TaskRepository(_context).GetSurveyByTask(appointmentId, appointment.WorkFlowId);
                #endregion
                appointment.ProfileAddress = string.Format("{0}{1}{2}", appointment.ProfileAddress, appointment.DistrictName, appointment.ProvinceName);
                //Contact
                //Liên hệ
                //appointment.taskContactList = (from p in _context.TaskContactModel
                //                               join s in _context.ProfileModel on p.ContactId equals s.ProfileId
                //                               where p.TaskId == appointment.TaskId
                //                               orderby p.isMain descending
                //                               select new TaskContactViewModel()
                //                               {
                //                                   ContactId = p.ContactId,
                //                                   ContactName = s.ProfileName
                //                               }).ToList();
                appointment.taskContact = (from p in _context.TaskContactModel
                                           join s in _context.ProfileModel on p.ContactId equals s.ProfileId
                                           where p.TaskId == appointment.TaskId
                                           && p.isMain == true
                                           select new TaskContactViewModel()
                                           {
                                               ContactId = p.ContactId,
                                               ContactName = s.ProfileName
                                           }).FirstOrDefault();
                if (appointment.taskContact != null)
                {
                    var contact = (from p in _context.ProfileModel
                                       //Province
                                   join pr in _context.ProvinceModel on p.ProvinceId equals pr.ProvinceId into prG
                                   from province in prG.DefaultIfEmpty()
                                       //District
                                   join d in _context.DistrictModel on p.DistrictId equals d.DistrictId into dG
                                   from district in dG.DefaultIfEmpty()
                                   where p.ProfileId == appointment.taskContact.ContactId
                                   select new
                                   {
                                       ProfileAddress = p.Address,
                                       ProvinceName = province == null ? "" : ", " + province.ProvinceName,
                                       DistrictName = district == null ? "" : ", " + district.Appellation + " " + district.DistrictName,
                                       Phone = p.Phone,
                                       Email = p.Email
                                   }).FirstOrDefault();
                    if (contact != null)
                    {
                        appointment.ProfileAddress = string.Format("{0}{1}{2}", contact.ProfileAddress, contact.DistrictName, contact.ProvinceName);
                        appointment.ProvinceName = contact.ProvinceName;
                        appointment.DistrictName = contact.DistrictName;
                        appointment.Phone = contact.Phone;
                        appointment.Email = contact.Email;
                    }
                }

                //Assignee
                //Người được phân công
                appointment.taskAssignList = (from p in _context.TaskAssignModel
                                              join s in _context.SalesEmployeeModel on p.SalesEmployeeCode equals s.SalesEmployeeCode
                                              where p.TaskId == appointment.TaskId
                                              select new TaskAssignViewModel()
                                              {
                                                  TaskAssignId = p.TaskAssignId,
                                                  SalesEmployeeCode = p.SalesEmployeeCode,
                                                  TaskAssignTypeCode = p.TaskAssignTypeCode,
                                                  SalesEmployeeName = p.SalesEmployeeCode + " | " + s.SalesEmployeeName
                                              }).ToList();

                TaskRepository _taskRepository = new TaskRepository(_context);
                //Comment
                var commentList = _taskRepository.GetTaskCommentList(appointmentId);
                appointment.taskCommentList = commentList;
                appointment.NumberOfComments = commentList.Count;
                //File Attachment
                var taskFileList = _taskRepository.GetTaskFileList(commentList, appointmentId);
                appointment.taskFileList = taskFileList;
                appointment.NumberOfFiles = taskFileList.Count;

                //Sản phẩm thị hiếu
                var productList = (from p in _context.CustomerTastesModel
                                   where p.AppointmentId == appointmentId
                                   orderby p.CreatedDate
                                   select p.ERPProductCode).ToList();
                appointment.productList = productList;
            }
            return appointment;
        }

        #region Báo cáo số lượng KH ghé thăm từng showroom
        public List<ProfileQuantityAppointmentWithShowRoomReportViewModel> GetProfileQuantityAppointmentWithShowRoomReport(ProfileQuantityAppointmentWithShowRoomReportSearchViewModel searchModel, string CurrentCompanyCode, DateTime? FromDate, DateTime? ToDate)
        {
            #region CreateAtSaleOrg
            //Build your record
            var tableCreateAtSaleOrgSchema = new List<SqlMetaData>(1)
                {
                    new SqlMetaData("Code", SqlDbType.NVarChar, 100)
                }.ToArray();

            //And a table as a list of those records
            var tableCreateAtSaleOrg = new List<SqlDataRecord>();
            List<string> saleOrgLst = new List<string>();
            if (searchModel.CreateAtSaleOrg != null && searchModel.CreateAtSaleOrg.Count > 0)
            {
                foreach (var r in searchModel.CreateAtSaleOrg)
                {
                    var tableRow = new SqlDataRecord(tableCreateAtSaleOrgSchema);

                    tableRow.SetString(0, r);
                    if (!saleOrgLst.Contains(r))
                    {
                        saleOrgLst.Add(r);
                        tableCreateAtSaleOrg.Add(tableRow);
                    }
                }
            }
            else
            {
                var tableRow = new SqlDataRecord(tableCreateAtSaleOrgSchema);
                tableCreateAtSaleOrg.Add(tableRow);
            }
            #endregion

            string sqlQuery = "EXEC [Report].[usp_ProfileQuantityAppointmentWithShowRoomReport] @CreateAtSaleOrg, @FromDate, @ToDate, @CurrentCompanyCode, @CustomerSource";

            #region Parameters
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter
                {
                    SqlDbType = SqlDbType.Structured,
                    Direction = ParameterDirection.Input,
                    ParameterName = "CreateAtSaleOrg",
                    TypeName = "[dbo].[StringList]", //Don't forget this one!
                    Value = tableCreateAtSaleOrg
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.DateTime,
                    Direction = ParameterDirection.Input,
                    ParameterName = "FromDate",
                    Value = FromDate ?? (object)DBNull.Value
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.DateTime,
                    Direction = ParameterDirection.Input,
                    ParameterName = "ToDate",
                    Value = ToDate ?? (object)DBNull.Value
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input,
                    ParameterName = "CurrentCompanyCode",
                    Value = CurrentCompanyCode ?? (object)DBNull.Value,
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input,
                    ParameterName = "@CustomerSource",
                    Value = searchModel.CustomerSource ?? (object)DBNull.Value,
                },
            };
            #endregion

            var result = _context.Database.SqlQuery<ProfileQuantityAppointmentWithShowRoomReportViewModel>(sqlQuery, parameters.ToArray()).ToList();
           
            return result;
        }
        #endregion Báo cáo số lượng KH ghé thăm từng showroom


        #region BÁO CÁO CHI TIẾT PHÂN CẤP KHÁCH HÀNG
        public List<CustomerHierarchyDetailReportViewModel> GetCustomerHierarchyDetail(CustomerHierarchyDetailReportSearchViewModel searchModel, string CurrentCompanyCode)
        {
            string sqlQuery = "EXEC [Report].[usp_CustomerHierarchyDetailReport]";

            #region Parameters
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                //new SqlParameter
                //{
                //    SqlDbType = SqlDbType.Structured,
                //    Direction = ParameterDirection.Input,
                //    ParameterName = "CreateAtSaleOrg",
                //    TypeName = "[dbo].[StringList]", //Don't forget this one!
                //    Value = tableCreateAtSaleOrg
                //},
                //new SqlParameter
                //{
                //    SqlDbType = SqlDbType.DateTime,
                //    Direction = ParameterDirection.Input,
                //    ParameterName = "FromDate",
                //    Value = searchModel.FromDate ?? (object)DBNull.Value
                //},
                //new SqlParameter
                //{
                //    SqlDbType = SqlDbType.DateTime,
                //    Direction = ParameterDirection.Input,
                //    ParameterName = "ToDate",
                //    Value = searchModel.ToDate ?? (object)DBNull.Value
                //},
                //new SqlParameter
                //{
                //    SqlDbType = SqlDbType.NVarChar,
                //    Direction = ParameterDirection.Input,
                //    ParameterName = "CurrentCompanyCode",
                //    Value = CurrentCompanyCode ?? (object)DBNull.Value,
                //},
            };
            #endregion

            var result = _context.Database.SqlQuery<CustomerHierarchyDetailReportViewModel>(sqlQuery, parameters.ToArray()).ToList();

            return result;
        }
        #endregion


        #region Tổng hợp lỗi sản phẩm trong xử lý khiếu nại
        public List<TicketUsualErrorViewModel> GetTicketUsualErrorReport(TaskSearchViewModel searchModel, string CurrentCompanyCode)
        {
            #region UsualErrorCodeList
            //Build your record
            var tableErrorSchema = new List<SqlMetaData>(1)
            {
                new SqlMetaData("Code", SqlDbType.NVarChar, 50)
            }.ToArray();

            //And a table as a list of those records
            var tableError = new List<SqlDataRecord>();
            if (searchModel.UsualErrorCode != null && searchModel.UsualErrorCode.Count > 0)
            {
                foreach (var r in searchModel.UsualErrorCode)
                {
                    var tableRow = new SqlDataRecord(tableErrorSchema);
                    tableRow.SetString(0, r);
                    tableError.Add(tableRow);
                }
            }
            else
            {
                var tableRow = new SqlDataRecord(tableErrorSchema);
                tableError.Add(tableRow);
            }
            #endregion

            #region ProductColorCodeList
            //Build your record
            var tableColorSchema = new List<SqlMetaData>(1)
            {
                new SqlMetaData("Code", SqlDbType.NVarChar, 50)
            }.ToArray();

            //And a table as a list of those records
            var tableColor = new List<SqlDataRecord>();
            if (searchModel.ProductColorCode != null && searchModel.ProductColorCode.Count > 0)
            {
                foreach (var r in searchModel.ProductColorCode)
                {
                    var tableRow = new SqlDataRecord(tableColorSchema);
                    tableRow.SetString(0, r);
                    tableColor.Add(tableRow);
                }
            }
            else
            {
                var tableRow = new SqlDataRecord(tableColorSchema);
                tableColor.Add(tableRow);
            }
            #endregion

            #region ProductCategoryCode
            //Build your record
            var tableCategorySchema = new List<SqlMetaData>(1)
            {
                new SqlMetaData("Code", SqlDbType.NVarChar, 50)
            }.ToArray();

            //And a table as a list of those records
            var tableCategory = new List<SqlDataRecord>();
            if (searchModel.ProductCategoryCode != null && searchModel.ProductCategoryCode.Count > 0)
            {
                foreach (var r in searchModel.ProductCategoryCode)
                {
                    var tableRow = new SqlDataRecord(tableCategorySchema);
                    tableRow.SetString(0, r);
                    tableCategory.Add(tableRow);
                }
            }
            else
            {
                var tableRow = new SqlDataRecord(tableCategorySchema);
                tableCategory.Add(tableRow);
            }
            #endregion

            string sqlQuery = "EXEC [Report].[usp_TicketUsualErrorReport] @ProductCategoryCode, @ProductColorCode, @UsualErrorCode, @CurrentCompanyCode,@FromDate,@ToDate";
            #region Parameters
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter
                {
                    SqlDbType = SqlDbType.Structured,
                    Direction = ParameterDirection.Input,
                    ParameterName = "ProductCategoryCode",
                    TypeName = "[dbo].[StringList]", //Don't forget this one!
                    Value = tableCategory
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.Structured,
                    Direction = ParameterDirection.Input,
                    ParameterName = "ProductColorCode",
                    TypeName = "[dbo].[StringList]", //Don't forget this one!
                    Value = tableColor
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.Structured,
                    Direction = ParameterDirection.Input,
                    ParameterName = "UsualErrorCode",
                    TypeName = "[dbo].[StringList]", //Don't forget this one!
                    Value = tableError
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input,
                    ParameterName = "CurrentCompanyCode",
                   Value = CurrentCompanyCode ?? (object)DBNull.Value
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.DateTime,
                    Direction = ParameterDirection.Input,
                    ParameterName = "FromDate",
                    Value = searchModel.FromDate ?? (object)DBNull.Value
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.DateTime,
                    Direction = ParameterDirection.Input,
                    ParameterName = "ToDate",
                    Value = searchModel.ToDate ?? (object)DBNull.Value
                }
            };
            #endregion

            var result = _context.Database.SqlQuery<TicketUsualErrorViewModel>(sqlQuery, parameters.ToArray()).ToList();

            return result;
        }
        #endregion


        #region Báo cáo nhu cầu khách hàng đến showroom
        public List<CustomerDemandReportViewModel> QueryCustomerDemand(CustomerDemandViewModel searchModel, string CurrentCompanyCode)
        {
            //Parameters for your query
            #region CreateAtCompany
            string CreateAtCompany = string.Empty;
            if (searchModel.CompanyId != null)
            {
                var company = _context.CompanyModel.Where(p => p.CompanyId == searchModel.CompanyId).FirstOrDefault();
                if (company != null)
                {
                    CreateAtCompany = company.CompanyCode;
                }
            }
            #endregion CreateAtCompany

            #region CreateAtSaleOrg
            //Build your record
            var tableCreateAtSaleOrgSchema = new List<SqlMetaData>(1)
                {
                    new SqlMetaData("Code", SqlDbType.NVarChar, 100)
                }.ToArray();

            //And a table as a list of those records
            var tableCreateAtSaleOrg = new List<SqlDataRecord>();
            List<string> saleOrgLst = new List<string>();
            if (searchModel.StoreId != null && searchModel.StoreId.Count > 0)
            {
                foreach (var r in searchModel.StoreId)
                {
                    var tableRow = new SqlDataRecord(tableCreateAtSaleOrgSchema);
                    var store = _context.StoreModel.Where(p => p.StoreId == r).FirstOrDefault();
                    if (store != null)
                    {
                        tableRow.SetString(0, store.SaleOrgCode);
                        if (!saleOrgLst.Contains(store.SaleOrgCode))
                        {
                            saleOrgLst.Add(store.SaleOrgCode);
                            tableCreateAtSaleOrg.Add(tableRow);
                        }
                    }
                }
            }
            else
            {
                var tableRow = new SqlDataRecord(tableCreateAtSaleOrgSchema);
                tableCreateAtSaleOrg.Add(tableRow);
            }
            #endregion

            #region ShowroomCode (CustomerSourceCode)
            //Build your record
            var tableCustomerSourceCodeSchema = new List<SqlMetaData>(1)
                {
                    new SqlMetaData("Code", SqlDbType.NVarChar, 100)
                }.ToArray();

            //And a table as a list of those records
            var tableCustomerSourceCode = new List<SqlDataRecord>();
            List<string> customerSourceCodeLst = new List<string>();
            if (searchModel.CustomerSourceCode != null && searchModel.CustomerSourceCode.Count > 0)
            {
                foreach (var r in searchModel.CustomerSourceCode)
                {
                    var tableRow = new SqlDataRecord(tableCustomerSourceCodeSchema);
                    tableRow.SetString(0, r);
                    if (!customerSourceCodeLst.Contains(r))
                    {
                        customerSourceCodeLst.Add(r);
                        tableCustomerSourceCode.Add(tableRow);
                    }
                }
            }
            else
            {
                var tableRow = new SqlDataRecord(tableCustomerSourceCodeSchema);
                tableCustomerSourceCode.Add(tableRow);
            }
            #endregion

            string sqlQuery = "EXEC [Report].[usp_CustomerDemand]";

            //var FilteredResultsCountOutParam = new SqlParameter();
            //FilteredResultsCountOutParam.ParameterName = "FilteredResultsCount";
            //FilteredResultsCountOutParam.SqlDbType = SqlDbType.Int;
            //FilteredResultsCountOutParam.Direction = ParameterDirection.Output;

            #region Parameters
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                //new SqlParameter
                //{
                //    SqlDbType = SqlDbType.NVarChar,
                //    Direction = ParameterDirection.Input,
                //    ParameterName = "TaskCode",
                //    Value = searchModel.TaskCode ?? (object)DBNull.Value,
                //},
                //new SqlParameter
                //{
                //    SqlDbType = SqlDbType.NVarChar,
                //    Direction = ParameterDirection.Input,
                //    ParameterName = "ProfileName",
                //    Value = searchModel.ProfileName ?? (object)DBNull.Value,
                //},
                //new SqlParameter
                //{
                //    SqlDbType = SqlDbType.UniqueIdentifier,
                //    Direction = ParameterDirection.Input,
                //    ParameterName = "CompanyId",
                //    Value = searchModel.CompanyId ?? (object)DBNull.Value,
                //},
                //new SqlParameter
                //{
                //    SqlDbType = SqlDbType.Structured,
                //    Direction = ParameterDirection.Input,
                //    ParameterName = "CreateAtSaleOrg",
                //    TypeName = "[dbo].[StringList]", //Don't forget this one!
                //    Value = tableCreateAtSaleOrg
                //},
                //new SqlParameter
                //{
                //    SqlDbType = SqlDbType.NVarChar,
                //    Direction = ParameterDirection.Input,
                //    ParameterName = "SalesEmployeeCode",
                //    Value = searchModel.SalesEmployeeCode ?? (object)DBNull.Value,
                //},
                //new SqlParameter
                //{
                //    SqlDbType = SqlDbType.Structured,
                //    Direction = ParameterDirection.Input,
                //    ParameterName = "CustomerSourceCode",
                //    TypeName = "[dbo].[StringList]", //Don't forget this one!
                //    Value = tableCustomerSourceCode
                //},
                //new SqlParameter
                //{
                //    SqlDbType = SqlDbType.NVarChar,
                //    Direction = ParameterDirection.Input,
                //    ParameterName = "CustomerGroupCode",
                //    Value = searchModel.CustomerGroupCode ?? (object)DBNull.Value,
                //},
                //new SqlParameter
                //{
                //    SqlDbType = SqlDbType.DateTime,
                //    Direction = ParameterDirection.Input,
                //    ParameterName = "FromDate",
                //    Value = searchModel.FromDate ?? (object)DBNull.Value
                //},
                //new SqlParameter
                //{
                //    SqlDbType = SqlDbType.DateTime,
                //    Direction = ParameterDirection.Input,
                //    ParameterName = "ToDate",
                //    Value = searchModel.ToDate ?? (object)DBNull.Value
                //},
                //new SqlParameter
                //{
                //    SqlDbType = SqlDbType.NVarChar,
                //    Direction = ParameterDirection.Input,
                //    ParameterName = "SaleOfficeCode",
                //    Value = searchModel.SaleOfficeCode ?? (object)DBNull.Value
                //},
                //new SqlParameter
                //{
                //    SqlDbType = SqlDbType.NVarChar,
                //    Direction = ParameterDirection.Input,
                //    ParameterName = "CustomerTypeCode",
                //    Value = searchModel.CustomerTypeCode ?? (object)DBNull.Value
                //},
                //new SqlParameter
                //{
                //    SqlDbType = SqlDbType.NVarChar,
                //    Direction = ParameterDirection.Input,
                //    ParameterName = "Age",
                //    Value = searchModel.Age ?? (object)DBNull.Value
                //},
                //new SqlParameter
                //{
                //    SqlDbType = SqlDbType.UniqueIdentifier,
                //    Direction = ParameterDirection.Input,
                //    ParameterName = "TaskStatusId",
                //    Value = searchModel.TaskStatusId ?? (object)DBNull.Value,
                //},
                //new SqlParameter
                //{
                //    SqlDbType = SqlDbType.NVarChar,
                //    Direction = ParameterDirection.Input,
                //    ParameterName = "Phone",
                //    Value = searchModel.Phone ?? (object)DBNull.Value
                //},
                //new SqlParameter
                //{
                //    SqlDbType = SqlDbType.NVarChar,
                //    Direction = ParameterDirection.Input,
                //    ParameterName = "TaxNo",
                //    Value = searchModel.TaxNo ?? (object)DBNull.Value
                //},
                //new SqlParameter
                //{
                //    SqlDbType = SqlDbType.NVarChar,
                //    Direction = ParameterDirection.Input,
                //    ParameterName = "CustomerCareerCode",
                //    Value = searchModel.CustomerCareerCode ?? (object)DBNull.Value
                //},
                //new SqlParameter
                //{
                //    SqlDbType = SqlDbType.DateTime,
                //    Direction = ParameterDirection.Input,
                //    ParameterName = "CreateFromDate",
                //    Value = searchModel.CreateFromDate ?? (object)DBNull.Value
                //},
                //new SqlParameter
                //{
                //    SqlDbType = SqlDbType.DateTime,
                //    Direction = ParameterDirection.Input,
                //    ParameterName = "CreateToDate",
                //    Value = searchModel.CreateToDate ?? (object)DBNull.Value
                //},
                //new SqlParameter
                //{
                //    SqlDbType = SqlDbType.NVarChar,
                //    Direction = ParameterDirection.Input,
                //    ParameterName = "CurrentCompanyCode",
                //    Value = CurrentCompanyCode ?? (object)DBNull.Value,
                //},
                //new SqlParameter
                //{
                //    SqlDbType = SqlDbType.Int,
                //    Direction = ParameterDirection.Input,
                //    ParameterName = "PageSize",
                //    Value = searchModel.PageSize,
                //},
                //new SqlParameter
                //{
                //    SqlDbType = SqlDbType.Int,
                //    Direction = ParameterDirection.Input,
                //    ParameterName = "PageNumber",
                //    Value = searchModel.PageNumber,
                //},
                //FilteredResultsCountOutParam
            };
            #endregion

            var result = _context.Database.SqlQuery<CustomerDemandReportViewModel>(sqlQuery, parameters.ToArray()).ToList();
            //var filteredResultsCountValue = FilteredResultsCountOutParam.Value;
            //if (filteredResultsCountValue != null)
            //{
            //    filteredResultsCount = Convert.ToInt32(filteredResultsCountValue);
            //}
            //else
            //{
            //    filteredResultsCount = 0;
            //}
            return result;
        }
        #endregion



        #region Báo cáo tỷ lệ nhóm khách hàng đến showroom
        public List<RateOfAppointmentWithShowRoomReportViewModel> GetRateOfAppointmentWithShowRoomReport(RateOfAppointmentWithShowRoomReportSearchViewModel searchModel, string CurrentCompanyCode)
        {

            #region SalesEmployeeCode
            //Build your record
            var tableSalesEmployeeCodeSchema = new List<SqlMetaData>(1)
                {
                    new SqlMetaData("Code", SqlDbType.NVarChar, 100)
                }.ToArray();

            //And a table as a list of those records
            var tableSalesEmployeeCode = new List<SqlDataRecord>();
            List<string> salesEmployeeCodeLst = new List<string>();
            if (searchModel.SalesEmployeeCode != null && searchModel.SalesEmployeeCode.Count > 0)
            {
                foreach (var r in searchModel.SalesEmployeeCode)
                {
                    var tableRow = new SqlDataRecord(tableSalesEmployeeCodeSchema);

                    tableRow.SetString(0, r);
                    if (!salesEmployeeCodeLst.Contains(r))
                    {
                        salesEmployeeCodeLst.Add(r);
                        tableSalesEmployeeCode.Add(tableRow);
                    }
                }
            }
            else
            {
                var tableRow = new SqlDataRecord(tableSalesEmployeeCodeSchema);
                tableSalesEmployeeCode.Add(tableRow);
            }
            #endregion
            #region RolesCode
            //Build your record
            var tableRolesCodeSchema = new List<SqlMetaData>(1)
                {
                    new SqlMetaData("Code", SqlDbType.NVarChar, 100)
                }.ToArray();

            //And a table as a list of those records
            var tableRolesCode = new List<SqlDataRecord>();
            List<string> rolesCodeLst = new List<string>();
            if (searchModel.RolesCode != null && searchModel.RolesCode.Count > 0)
            {
                foreach (var r in searchModel.RolesCode)
                {
                    var tableRow = new SqlDataRecord(tableRolesCodeSchema);

                    tableRow.SetString(0, r);
                    if (!rolesCodeLst.Contains(r))
                    {
                        rolesCodeLst.Add(r);
                        tableRolesCode.Add(tableRow);
                    }
                }
            }
            else
            {
                var tableRow = new SqlDataRecord(tableRolesCodeSchema);
                tableRolesCode.Add(tableRow);
            }
            #endregion


            #region CreateAtSaleOrg
            //Build your record
            var tableCreateAtSaleOrgSchema = new List<SqlMetaData>(1)
                {
                    new SqlMetaData("Code", SqlDbType.NVarChar, 100)
                }.ToArray();

            //And a table as a list of those records
            var tableCreateAtSaleOrg = new List<SqlDataRecord>();
            List<string> saleOrgLst = new List<string>();
            if (searchModel.CreateAtSaleOrg != null && searchModel.CreateAtSaleOrg.Count > 0)
            {
                foreach (var r in searchModel.CreateAtSaleOrg)
                {
                    var tableRow = new SqlDataRecord(tableCreateAtSaleOrgSchema);

                    tableRow.SetString(0, r);
                    if (!saleOrgLst.Contains(r))
                    {
                        saleOrgLst.Add(r);
                        tableCreateAtSaleOrg.Add(tableRow);
                    }
                }
            }
            else
            {
                var tableRow = new SqlDataRecord(tableCreateAtSaleOrgSchema);
                tableCreateAtSaleOrg.Add(tableRow);
            }
            #endregion

            string sqlQuery = "EXEC [Report].[usp_RateOfAppoinmentWithShowRoomReport] @CreateAtSaleOrg, @FromDate, @ToDate, @CurrentCompanyCode, @SalesEmployeeCode, @RolesCode";

            #region Parameters
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter
                {
                    SqlDbType = SqlDbType.Structured,
                    Direction = ParameterDirection.Input,
                    ParameterName = "CreateAtSaleOrg",
                    TypeName = "[dbo].[StringList]", //Don't forget this one!
                    Value = tableCreateAtSaleOrg
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.DateTime,
                    Direction = ParameterDirection.Input,
                    ParameterName = "FromDate",
                    Value = searchModel.FromDate ?? (object)DBNull.Value
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.DateTime,
                    Direction = ParameterDirection.Input,
                    ParameterName = "ToDate",
                    Value = searchModel.ToDate ?? (object)DBNull.Value
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input,
                    ParameterName = "CurrentCompanyCode",
                    Value = searchModel.CompanyCode  ?? (object)DBNull.Value,
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.Structured,
                    Direction = ParameterDirection.Input,
                    ParameterName = "SalesEmployeeCode",
                    TypeName = "[dbo].[StringList]", //Don't forget this one!
                    Value = tableSalesEmployeeCode,
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.Structured,
                    Direction = ParameterDirection.Input,
                    ParameterName = "RolesCode",
                    TypeName = "[dbo].[StringList]", //Don't forget this one!
                    Value = tableRolesCode,
                },
            };
            #endregion

            var result = _context.Database.SqlQuery<RateOfAppointmentWithShowRoomReportViewModel>(sqlQuery, parameters.ToArray()).ToList();

            return result;
        }
        #endregion Báo cáo số lượng KH ghé thăm từng showroom

        #region BÁO CÁO TỔNG HỢP XUẤT NHẬP TỒN CATALOGUE
        public List<TotalRDCatalogueReportViewModel> GetTotalRDCatalogueReport(TotalRDCatalogueReportSearchViewModel searchModel, string CurrentCompanyCode)
        {
            #region StoreId (chi nhánh)
            //Build your record
            var tableStoreIdSchema = new List<SqlMetaData>(1)
                {
                    new SqlMetaData("Id", SqlDbType.UniqueIdentifier)
                }.ToArray();

            //And a table as a list of those records
            var tableStoreId = new List<SqlDataRecord>();
            List<Guid> StoredIdLst = new List<Guid>();
            if (searchModel.StoreId != null && searchModel.StoreId.Count > 0)
            {
                foreach (var r in searchModel.StoreId)
                {
                    var tableRow = new SqlDataRecord(tableStoreIdSchema);

                    tableRow.SetGuid(0, r);
                    if (!StoredIdLst.Contains(r))
                    {
                        StoredIdLst.Add(r);
                        tableStoreId.Add(tableRow);
                    }
                }
            }
            else
            {
                var tableRow = new SqlDataRecord(tableStoreIdSchema);
                tableStoreId.Add(tableRow);
            }
            #endregion


            #region ProductId (Sản phẩm)
            //Build your record
            var tableProductIdSchema = new List<SqlMetaData>(1)
                {
                    new SqlMetaData("Id", SqlDbType.UniqueIdentifier)
                }.ToArray();

            //And a table as a list of those records
            var tableProductId = new List<SqlDataRecord>();
            List<Guid> ProductIdLst = new List<Guid>();
            if (searchModel.ProductId != null && searchModel.ProductId.Count > 0)
            {
                foreach (var r in searchModel.ProductId)
                {
                    var tableRow = new SqlDataRecord(tableProductIdSchema);

                    tableRow.SetGuid(0, r);
                    if (!ProductIdLst.Contains(r))
                    {
                        ProductIdLst.Add(r);
                        tableProductId.Add(tableRow);
                    }
                }
            }
            else
            {
                var tableRow = new SqlDataRecord(tableProductIdSchema);
                tableProductId.Add(tableRow);
            }
            #endregion


            string sqlQuery = "EXEC [Report].[usp_TotalRDCatalogueReport] @ProductId, @StoreId, @FromDate, @ToDate";

            #region Parameters
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter
                {
                    SqlDbType = SqlDbType.Structured,
                    Direction = ParameterDirection.Input,
                    ParameterName = "ProductId",
                    TypeName = "[dbo].[GuidList]", //Don't forget this one!
                    Value = tableProductId
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.Structured,
                    Direction = ParameterDirection.Input,
                    ParameterName = "StoreId",
                    TypeName = "[dbo].[GuidList]", //Don't forget this one!
                    Value = tableStoreId
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.DateTime,
                    Direction = ParameterDirection.Input,
                    ParameterName = "FromDate",
                    Value = searchModel.FromDate ?? (object)DBNull.Value
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.DateTime,
                    Direction = ParameterDirection.Input,
                    ParameterName = "ToDate",
                    Value = searchModel.ToDate ?? (object)DBNull.Value
                },
            };
            #endregion

            var result = _context.Database.SqlQuery<TotalRDCatalogueReportViewModel>(sqlQuery, parameters.ToArray()).ToList();

            return result;
        }
        #endregion

        #region Search Query Appointment
        public List<AppointmentReportViewModel> QueryReportAppointment(AppointmentSearchViewModel searchModel, string CurrentCompanyCode, out int filteredResultsCount)
        {
            //Parameters for your query
            #region CreateAtCompany
            string CreateAtCompany = string.Empty;
            if (searchModel.CompanyId != null)
            {
                var company = _context.CompanyModel.Where(p => p.CompanyId == searchModel.CompanyId).FirstOrDefault();
                if (company != null)
                {
                    CreateAtCompany = company.CompanyCode;
                }
            }
            #endregion CreateAtCompany

            #region CreateAtSaleOrg
            //Build your record
            var tableCreateAtSaleOrgSchema = new List<SqlMetaData>(1)
                {
                    new SqlMetaData("Code", SqlDbType.NVarChar, 100)
                }.ToArray();

            //And a table as a list of those records
            var tableCreateAtSaleOrg = new List<SqlDataRecord>();
            List<string> saleOrgLst = new List<string>();
            if (searchModel.StoreId != null && searchModel.StoreId.Count > 0)
            {
                foreach (var r in searchModel.StoreId)
                {
                    var tableRow = new SqlDataRecord(tableCreateAtSaleOrgSchema);
                    var store = _context.StoreModel.Where(p => p.StoreId == r).FirstOrDefault();
                    if (store != null)
                    {
                        tableRow.SetString(0, store.SaleOrgCode);
                        if (!saleOrgLst.Contains(store.SaleOrgCode))
                        {
                            saleOrgLst.Add(store.SaleOrgCode);
                            tableCreateAtSaleOrg.Add(tableRow);
                        }
                    }
                }
            }
            else
            {
                var tableRow = new SqlDataRecord(tableCreateAtSaleOrgSchema);
                tableCreateAtSaleOrg.Add(tableRow);
            }
            #endregion

            #region ShowroomCode (CustomerSourceCode)
            //Build your record
            var tableCustomerSourceCodeSchema = new List<SqlMetaData>(1)
                {
                    new SqlMetaData("Code", SqlDbType.NVarChar, 100)
                }.ToArray();

            //And a table as a list of those records
            var tableCustomerSourceCode = new List<SqlDataRecord>();
            List<string> customerSourceCodeLst = new List<string>();
            if (searchModel.CustomerSourceCode != null && searchModel.CustomerSourceCode.Count > 0)
            {
                foreach (var r in searchModel.CustomerSourceCode)
                {
                    var tableRow = new SqlDataRecord(tableCustomerSourceCodeSchema);
                    tableRow.SetString(0, r);
                    if (!customerSourceCodeLst.Contains(r))
                    {
                        customerSourceCodeLst.Add(r);
                        tableCustomerSourceCode.Add(tableRow);
                    }
                }
            }
            else
            {
                var tableRow = new SqlDataRecord(tableCustomerSourceCodeSchema);
                tableCustomerSourceCode.Add(tableRow);
            }
            #endregion

            string sqlQuery = "EXEC [Customer].[usp_SearchAppointment_Dynamic] @TaskCode, @ProfileName, @CompanyId, @CreateAtSaleOrg, @SalesEmployeeCode, @CustomerSourceCode, @CustomerGroupCode, @FromDate, @ToDate, @SaleOfficeCode, @CustomerTypeCode, @Age, @TaskStatusId, @Phone, @TaxNo, @CustomerCareerCode, @CreateFromDate, @CreateToDate, @CurrentCompanyCode, @PageSize, @PageNumber, @FilteredResultsCount OUT";
            var FilteredResultsCountOutParam = new SqlParameter();
            FilteredResultsCountOutParam.ParameterName = "FilteredResultsCount";
            FilteredResultsCountOutParam.SqlDbType = SqlDbType.Int;
            FilteredResultsCountOutParam.Direction = ParameterDirection.Output;

            #region Parameters
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter
                {
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input,
                    ParameterName = "TaskCode",
                    Value = searchModel.TaskCode ?? (object)DBNull.Value,
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input,
                    ParameterName = "ProfileName",
                    Value = searchModel.ProfileName ?? (object)DBNull.Value,
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.UniqueIdentifier,
                    Direction = ParameterDirection.Input,
                    ParameterName = "CompanyId",
                    Value = searchModel.CompanyId ?? (object)DBNull.Value,
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.Structured,
                    Direction = ParameterDirection.Input,
                    ParameterName = "CreateAtSaleOrg",
                    TypeName = "[dbo].[StringList]", //Don't forget this one!
                    Value = tableCreateAtSaleOrg
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input,
                    ParameterName = "SalesEmployeeCode",
                    Value = searchModel.SalesEmployeeCode ?? (object)DBNull.Value,
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.Structured,
                    Direction = ParameterDirection.Input,
                    ParameterName = "CustomerSourceCode",
                    TypeName = "[dbo].[StringList]", //Don't forget this one!
                    Value = tableCustomerSourceCode
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.Structured,
                    Direction = ParameterDirection.Input,
                    ParameterName = "CustomerGroupCode",
                    TypeName = "[dbo].[StringList]", //Don't forget this one!
                    Value = new UtilitiesRepository().ConvertTableFromListString(searchModel.CustomerGroupCode)

                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.DateTime,
                    Direction = ParameterDirection.Input,
                    ParameterName = "FromDate",
                    Value = searchModel.FromDate ?? (object)DBNull.Value
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.DateTime,
                    Direction = ParameterDirection.Input,
                    ParameterName = "ToDate",
                    Value = searchModel.ToDate ?? (object)DBNull.Value
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.Structured,
                    Direction = ParameterDirection.Input,
                    ParameterName = "SaleOfficeCode",  
                    TypeName = "[dbo].[StringList]", //Don't forget this one!
                    Value = new UtilitiesRepository().ConvertTableFromListString(searchModel.SaleOfficeCode)
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input,
                    ParameterName = "CustomerTypeCode",
                    Value = searchModel.CustomerTypeCode ?? (object)DBNull.Value
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input,
                    ParameterName = "Age",
                    Value = searchModel.Age ?? (object)DBNull.Value
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.UniqueIdentifier,
                    Direction = ParameterDirection.Input,
                    ParameterName = "TaskStatusId",
                    Value = searchModel.TaskStatusId ?? (object)DBNull.Value,
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input,
                    ParameterName = "Phone",
                    Value = searchModel.Phone ?? (object)DBNull.Value
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input,
                    ParameterName = "TaxNo",
                    Value = searchModel.TaxNo ?? (object)DBNull.Value
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input,
                    ParameterName = "CustomerCareerCode",
                    Value = searchModel.CustomerCareerCode ?? (object)DBNull.Value
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.DateTime,
                    Direction = ParameterDirection.Input,
                    ParameterName = "CreateFromDate",
                    Value = searchModel.CreateFromDate ?? (object)DBNull.Value
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.DateTime,
                    Direction = ParameterDirection.Input,
                    ParameterName = "CreateToDate",
                    Value = searchModel.CreateToDate ?? (object)DBNull.Value
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input,
                    ParameterName = "CurrentCompanyCode",
                    Value = CurrentCompanyCode ?? (object)DBNull.Value,
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Input,
                    ParameterName = "PageSize",
                    Value = searchModel.PageSize,
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Input,
                    ParameterName = "PageNumber",
                    Value = searchModel.PageNumber,
                },
                FilteredResultsCountOutParam
            };
            #endregion

            var result = _context.Database.SqlQuery<AppointmentReportViewModel>(sqlQuery, parameters.ToArray()).ToList();
            var filteredResultsCountValue = FilteredResultsCountOutParam.Value;
            if (filteredResultsCountValue != null)
            {
                filteredResultsCount = Convert.ToInt32(filteredResultsCountValue);
            }
            else
            {
                filteredResultsCount = 0;
            }
            return result;
        }
        public TaskContactViewModel GetMainContact(Guid? profileId, string currentCompanyCode)
        {
            TaskContactViewModel taskContact = new TaskContactViewModel();

            var contact = (from p in _context.ProfileModel
                               //Province
                           join pr in _context.ProvinceModel on p.ProvinceId equals pr.ProvinceId into prG
                           from province in prG.DefaultIfEmpty()
                               //District
                           join d in _context.DistrictModel on p.DistrictId equals d.DistrictId into dG
                           from district in dG.DefaultIfEmpty()
                               //Ward
                           join w in _context.WardModel on p.WardId equals w.WardId into wG
                           from ward in wG.DefaultIfEmpty()
                           where p.ProfileId == profileId
                           select new
                           {
                               ContactId = p.ProfileId,
                               CustomerTypeCode = p.CustomerTypeCode,
                               ProfileAddress = p.Address,
                               ProvinceName = province == null ? "" : ", " + province.ProvinceName,
                               DistrictName = district == null ? "" : ", " + district.Appellation + " " + district.DistrictName,
                               WardName = ward == null ? "" : ", " + ward.Appellation + " " + ward.WardName,
                               Phone = p.Phone ?? p.SAPPhone,
                               ContactShortName = p.ProfileCode + " | " + (p.ProfileShortName != null ? p.ProfileShortName : p.ProfileName),
                               Email = p.Email,
                           }).FirstOrDefault();

            //Thông tin nhân viên kinh doanh
            var SaleSupervisorList = (from p in _context.PersonInChargeModel
                                      join s in _context.SalesEmployeeModel on p.SalesEmployeeCode equals s.SalesEmployeeCode
                                      join acc in _context.AccountModel on s.SalesEmployeeCode equals acc.EmployeeCode
                                      from r in acc.RolesModel
                                      where p.ProfileId == profileId
                                      && p.CompanyCode == currentCompanyCode
                                      select new
                                      {
                                          SalesEmployeeCode = p.SalesEmployeeCode,
                                          SalesEmployeeName = s.SalesEmployeeName,
                                          DepartmentName = r.RolesName,
                                          isEmployeeGroup = r.isEmployeeGroup
                                      }).ToList();
            //Lấy 1 thông tin NV kinh doanh + phòng ban
            if (SaleSupervisorList != null && SaleSupervisorList.Count > 0)
            {
                var SaleSupervisor = SaleSupervisorList.Where(p => p.isEmployeeGroup == true).FirstOrDefault();
                if (SaleSupervisor == null)
                {
                    SaleSupervisor = SaleSupervisorList.FirstOrDefault();
                }
                taskContact.SalesSupervisorCode = SaleSupervisor.SalesEmployeeCode;
                taskContact.SalesSupervisorName = SaleSupervisor.SalesEmployeeName;
                taskContact.DepartmentName = SaleSupervisor.isEmployeeGroup == true ? SaleSupervisor.DepartmentName : "";
            }

            taskContact.ContactShortName = contact.ContactShortName;
            taskContact.ContactAddress = string.Format("{0}{1}{2}{3}", contact.ProfileAddress, contact.WardName, contact.DistrictName, contact.ProvinceName);
            taskContact.ContactPhone = contact.Phone;
            taskContact.ContactEmail = contact.Email;

            //Thông tin người liên hệ chính (nếu có)
            var mainContactList = (from p in _context.ProfileContactAttributeModel
                                   join pro in _context.ProfileModel on p.ProfileId equals pro.ProfileId
                                   where p.CompanyId == profileId
                                   select new
                                   {
                                       MainContactName = pro.ProfileShortName != null ? pro.ProfileShortName : pro.ProfileName,
                                       MainContactPhone = pro.Phone,
                                       MainContactEmail = pro.Email,
                                       IsMain = p.IsMain
                                   }).ToList();

            //Set thông tin liên hệ chính. nếu không có thông tin liên hệ thì thông tin liên hệ chính là profile đó
            if (mainContactList != null && mainContactList.Count > 0)
            {
                var mainContact = mainContactList.Where(p => p.IsMain == true).FirstOrDefault();
                if (mainContact == null)
                {
                    mainContact = mainContactList.FirstOrDefault();
                }
                taskContact.MainContactName = mainContact.MainContactName;
                taskContact.MainContactPhone = mainContact.MainContactPhone != null ? mainContact.MainContactPhone : taskContact.ContactPhone;
                taskContact.MainContactEmail = mainContact.MainContactEmail != null ? mainContact.MainContactEmail : taskContact.ContactEmail;
            }
            else
            {
                taskContact.MainContactPhone = taskContact.ContactPhone;
                taskContact.MainContactEmail = taskContact.ContactEmail;
            }

            return taskContact;
        }
        #endregion Search Query Appointment



        #region Báo cáo số lượng KH ghé thăm theo NV kinh doanh
        public List<AppointmentWithPersonInChargeReportViewModel> GetQuantityAppointmentWithPersonInChargeReport(AppointmentWithPersonInChargeReportSearchViewModel searchModel, string CurrentCompanyCode, DateTime? FromDate, DateTime? ToDate)
        {
            #region SalesEmployeeCode
            //Build your record
            var tableSalesEmployeeCodeSchema = new List<SqlMetaData>(1)
                {
                    new SqlMetaData("Code", SqlDbType.NVarChar, 100)
                }.ToArray();

            //And a table as a list of those records
            var tableSalesEmployeeCode = new List<SqlDataRecord>();
            List<string> saleEmployeeCodeLst = new List<string>();
            if (searchModel.SalesEmployeeCode != null && searchModel.SalesEmployeeCode.Count > 0)
            {
                foreach (var r in searchModel.SalesEmployeeCode)
                {
                    var tableRow = new SqlDataRecord(tableSalesEmployeeCodeSchema);

                    tableRow.SetString(0, r);
                    if (!saleEmployeeCodeLst.Contains(r))
                    {
                        saleEmployeeCodeLst.Add(r);
                        tableSalesEmployeeCode.Add(tableRow);
                    }
                }
            }
            else
            {
                var tableRow = new SqlDataRecord(tableSalesEmployeeCodeSchema);
                tableSalesEmployeeCode.Add(tableRow);
            }
            #endregion

            #region RolesCode
            //Build your record
            var tableRolesCodeSchema = new List<SqlMetaData>(1)
                {
                    new SqlMetaData("Code", SqlDbType.NVarChar, 100)
                }.ToArray();

            //And a table as a list of those records
            var tableRolesCode = new List<SqlDataRecord>();
            List<string> rolesCodeLst = new List<string>();
            if (searchModel.RolesCode != null && searchModel.RolesCode.Count > 0)
            {
                foreach (var r in searchModel.RolesCode)
                {
                    var tableRow = new SqlDataRecord(tableRolesCodeSchema);

                    tableRow.SetString(0, r);
                    if (!rolesCodeLst.Contains(r))
                    {
                        rolesCodeLst.Add(r);
                        tableRolesCode.Add(tableRow);
                    }
                }
            }
            else
            {
                var tableRow = new SqlDataRecord(tableRolesCodeSchema);
                tableRolesCode.Add(tableRow);
            }
            #endregion

            string sqlQuery = "EXEC [Report].[usp_AppointmentWithPersonInChargeReport] @SalesEmployeeCode, @FromDate, @ToDate, @CurrentCompanyCode, @RolesCode";

            #region Parameters
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter
                {   
                    SqlDbType = SqlDbType.Structured,
                    Direction = ParameterDirection.Input,
                    ParameterName = "SalesEmployeeCode",
                    TypeName = "[dbo].[StringList]", //Don't forget this one!
                    Value = tableSalesEmployeeCode
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.DateTime,
                    Direction = ParameterDirection.Input,
                    ParameterName = "FromDate",
                    Value = FromDate ?? (object)DBNull.Value
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.DateTime,
                    Direction = ParameterDirection.Input,
                    ParameterName = "ToDate",
                    Value = ToDate ?? (object)DBNull.Value
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input,
                    ParameterName = "CurrentCompanyCode",
                    Value = CurrentCompanyCode ?? (object)DBNull.Value,
                }, 
                new SqlParameter
                {
                    SqlDbType = SqlDbType.Structured,
                    Direction = ParameterDirection.Input,
                    ParameterName = "RolesCode",
                    TypeName = "[dbo].[StringList]", //Don't forget this one!
                    Value = tableRolesCode
                },
            };
            #endregion

            var result = _context.Database.SqlQuery<AppointmentWithPersonInChargeReportViewModel>(sqlQuery, parameters.ToArray()).ToList();

            return result;
        }
        #endregion Báo cáo số lượng KH ghé thăm từng showroom

    }
}

