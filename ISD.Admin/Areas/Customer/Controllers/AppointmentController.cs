using ISD.Constant;
using ISD.EntityModels;
using ISD.Extensions;
using ISD.Repositories;
using ISD.Resources;
using ISD.ViewModels;
using ISD.Core;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ISD.Repositories.Excel;
using System.Data.SqlClient;
using System.Data;

namespace Customer.Controllers
{
    public class AppointmentController : BaseController
    {
        #region Index
        [ISDAuthorizationAttribute]
        public ActionResult Index()
        {
            DateTime? fromDate, toDate;
            var CommonDate = "ThisMonth";
            _unitOfWork.CommonDateRepository.GetDateBy(CommonDate, out fromDate, out toDate);
            AppointmentSearchViewModel searchViewModel = new AppointmentSearchViewModel()
            {
                //Ngày ghé thăm
                CommonDate = CommonDate,
                FromDate = fromDate,
                ToDate = toDate,

                //Ngày tiếp nhận
                //CommonReceiveDate = "Custom",

                //Ngày kết thúc
                //CommonEndDate = "Custom",

                //Ngày kết thúc
                CommonCreateDate = "Custom",
            };
            ViewBag.PageId = GetPageId("/Customer/Appointment");
            CreateSearchViewBag(searchViewModel);
            return View(searchViewModel);
        }
        public ActionResult _PaggingServerSide(DatatableViewModel model, AppointmentSearchViewModel searchViewModel)
        {
            //try
            //{
                //Page Size 
                searchViewModel.PageSize = model.length;

                //Page Number
                searchViewModel.PageNumber = model.start / model.length + 1;
                //Nếu không tim theo custom thì lấy fromdate, todate theo common
                var search2 = new AppointmentSearchViewModel()
                {
                    Age = searchViewModel.Age,
                    ToDate = searchViewModel.ToDate,
                    FromDate = searchViewModel.FromDate,
                    CommonDate = searchViewModel.CommonDate,
                    CompanyCode = searchViewModel.CompanyCode,
                    CompanyId = searchViewModel.CompanyId,
                    CustomerClassCode = searchViewModel.CustomerClassCode,
                    CustomerSourceCode = searchViewModel.CustomerSourceCode,
                    CustomerTypeCode = searchViewModel.CustomerTypeCode,
                    SalesEmployeeCode = searchViewModel.SalesEmployeeCode,
                    SaleOfficeCode = searchViewModel.SaleOfficeCode,
                    SaleOrgCode = searchViewModel.SaleOrgCode,
                    StoreId = searchViewModel.StoreId,
                    Summary = searchViewModel.Summary,
                    TaskCode = searchViewModel.TaskCode,
                    TaskStatusId = searchViewModel.TaskStatusId,
                    Phone = searchViewModel.Phone,
                    TaxNo = searchViewModel.TaxNo,
                    CustomerGroupCode = searchViewModel.CustomerGroupCode,
                    CustomerCareerCode = searchViewModel.CustomerCareerCode,
                    ReceiveToDate = searchViewModel.ReceiveToDate,
                    ReceiveFromDate = searchViewModel.ReceiveFromDate,
                    CommonReceiveDate = searchViewModel.CommonReceiveDate,
                    EndToDate = searchViewModel.EndToDate,
                    EndFromDate = searchViewModel.EndFromDate,
                    CommonEndDate = searchViewModel.CommonEndDate,
                    CreateToDate = searchViewModel.CreateToDate,
                    CreateFromDate = searchViewModel.CreateFromDate,
                    CommonCreateDate = searchViewModel.CommonCreateDate,
                    ProfileId = searchViewModel.ProfileId,
                    ProfileName = searchViewModel.ProfileName,
                    PageNumber = searchViewModel.PageNumber,
                    PageSize = searchViewModel.PageSize,
                };

                //Ngày ghé thăm
                if (searchViewModel.CommonDate != "Custom")
                {
                    DateTime? fromDate;
                    DateTime? toDate;
                    DateTime? fromPreviousDay;
                    DateTime? toPreviousDay;

                    _unitOfWork.CommonDateRepository.GetDateBy(searchViewModel.CommonDate, out fromDate, out toDate, out fromPreviousDay, out toPreviousDay);
                    //Tìm kiếm kỳ hiện tại
                    searchViewModel.FromDate = fromDate;
                    searchViewModel.ToDate = toDate;

                    //Tìm kiếm kỳ trước đó
                    search2.FromDate = fromPreviousDay;
                    search2.ToDate = toPreviousDay;
                }
                /*
                //Ngày tiếp nhận
                if (searchViewModel.CommonReceiveDate != "Custom")
                {
                    DateTime? fromDate;
                    DateTime? toDate;
                    DateTime? fromPreviousDay;
                    DateTime? toPreviousDay;


                    _unitOfWork.CommonDateRepository.GetDateBy(searchViewModel.CommonReceiveDate, out fromDate, out toDate, out fromPreviousDay, out toPreviousDay);
                    //Tìm kiếm kỳ hiện tại
                    searchViewModel.ReceiveFromDate = fromDate;
                    searchViewModel.ReceiveToDate = toDate;

                    //Tìm kiếm kỳ trước đó
                    search2.ReceiveFromDate = fromPreviousDay;
                    search2.ReceiveToDate = toPreviousDay;
                }

                //Ngày kết thúc
                if (searchViewModel.CommonEndDate != "Custom")
                {
                    DateTime? fromDate;
                    DateTime? toDate;
                    DateTime? fromPreviousDay;
                    DateTime? toPreviousDay;


                    _unitOfWork.CommonDateRepository.GetDateBy(searchViewModel.CommonEndDate, out fromDate, out toDate, out fromPreviousDay, out toPreviousDay);
                    //Tìm kiếm kỳ hiện tại
                    searchViewModel.EndFromDate = fromDate;
                    searchViewModel.EndToDate = toDate;

                    //Tìm kiếm kỳ trước đó
                    search2.EndFromDate = fromPreviousDay;
                    search2.EndToDate = toPreviousDay;
                }*/

                //Ngày tạo
                if (searchViewModel.CommonCreateDate != "Custom")
                {
                    DateTime? fromDate;
                    DateTime? toDate;
                    DateTime? fromPreviousDay;
                    DateTime? toPreviousDay;


                    _unitOfWork.CommonDateRepository.GetDateBy(searchViewModel.CommonCreateDate, out fromDate, out toDate, out fromPreviousDay, out toPreviousDay);
                    //Tìm kiếm kỳ hiện tại
                    searchViewModel.CreateFromDate = fromDate;
                    searchViewModel.CreateToDate = toDate;

                    //Tìm kiếm kỳ trước đó
                    search2.CreateFromDate = fromPreviousDay;
                    search2.CreateToDate = toPreviousDay;
                }


                // action inside a standard controller
                int filteredResultsCount;
                //10
                int totalResultsCount = model.length;

                //Nếu tài khoản xem theo chi nhánh thì chỉ lấy các chi nhánh được phân quyền
                if (CurrentUser.isViewByStore == true)
                {
                    if (searchViewModel.StoreId == null || searchViewModel.StoreId.Count == 0)
                    {
                        var storeList = _unitOfWork.StoreRepository.GetAllStore(CurrentUser.isViewByStore, CurrentUser.AccountId);
                        if (storeList != null && storeList.Count > 0)
                        {
                            searchViewModel.StoreId = new List<Guid>();
                            searchViewModel.StoreId = storeList.Select(p => p.StoreId).ToList();
                        }
                    }
                }

                var query = _unitOfWork.AppointmentRepository.SearchQueryAppointment(searchViewModel, CurrentUser.CompanyCode, out filteredResultsCount);
                var res = query;
                if (res != null && res.Count() > 0)
                {
                    int i = model.start;
                    foreach (var item in res)
                    {
                        i++;
                        item.STT = i;
                        if (item.Address.StartsWith(", "))
                        {
                            item.Address = item.Address.Remove(0, 2);
                        }
                    }
                }

                #region Số thứ tự, tổng số kết quả
                //var res = CustomSearchRepository.CustomSearchFunc<AppointmentViewModel>(model, out filteredResultsCount, out totalResultsCount, query, "TaskCode", "desc");
                //if (res != null && res.Count > 0)
                //{
                //    int i = model.start;
                //    foreach (var item in res)
                //    {
                //        i++;
                //        item.STT = i;

                //        //Thị hiếu KH
                //        var customerTasteLst = _unitOfWork.CustomerTasteRepository.GetCustomerTastesBy(item.AppointmentId);
                //        if (customerTasteLst != null && customerTasteLst.Count > 0)
                //        {
                //            item.customerTasteLst = string.Join<string>(", ", customerTasteLst.Select(p => p.ProductCode));
                //        }

                //        //Catalogue
                //        var customerCatalogueLst = _unitOfWork.CatalogueRepository.GetCustomerCatalogueBy(item.ProfileId.Value, item.VisitDate.Value);
                //        if (customerCatalogueLst != null && customerCatalogueLst.Count > 0)
                //        {
                //            item.customerCatalogueLst = string.Join<string>(", ", customerCatalogueLst.Select(p => p.customerCatalogueLst));
                //        }
                //    }
                //}
                #endregion

                //Kỳ trước đó
                var PreviousCount = 0;
                if (searchViewModel.CommonDate != "Custom")
                {
                    //PreviousCount = _unitOfWork.AppointmentRepository.SearchQueryAppointment(search2, CurrentUser.CompanyCode, out filteredResultsCount).Count();
                    _unitOfWork.AppointmentRepository.SearchQueryAppointment(search2, CurrentUser.CompanyCode, out PreviousCount);
                }
                //Kỳ này
                decimal KyNay = filteredResultsCount;
                //Kỳ 
                decimal KyTruoc = PreviousCount;
                //Tỷ lệ kỳ này so với kỳ trước
                decimal TyLe = 100;
                if (KyTruoc != 0)
                {
                    TyLe = KyNay / KyTruoc * 100;
                }
                else if (KyTruoc == 0 && KyNay == 0)
                {
                    TyLe = 0;
                }

                return Json(new
                {
                    draw = model.draw,
                    recordsTotal = totalResultsCount,
                    //Tổng số kết quả kỳ này
                    recordsFiltered = filteredResultsCount,
                    //Tổng số kết quả kỳ trước
                    previousCount = PreviousCount,
                    //Tỷ lệ kỳ này so với kỳ trước
                    ratio = TyLe.ToString("0.##"),
                    data = res
                });
            //}
            //catch (Exception)
            //{
            //    return Json(null);
            //}
        }

        public ActionResult _List(Guid? id, bool? isLoadContent = false)
        {
            return ExecuteSearch(() =>
            {
                var listAppointment = _unitOfWork.AppointmentRepository.Search(id);
                if (isLoadContent == true)
                {
                    return PartialView("_ListContent", listAppointment);
                }
                return PartialView(listAppointment);
            });
        }
        #endregion

        #region Create and Edit popup
        public ActionResult _Create(Guid profileId)
        {
            var profile = _unitOfWork.ProfileRepository.GetById(profileId);

            //var store = _context.StoreModel.FirstOrDefault(p => p.SaleOrgCode == CurrentUser.SaleOrg);
            //var ShowroomCode = store.DefaultCustomerSource;

            var EmployeeName = _unitOfWork.SalesEmployeeRepository.GetSaleEmployeeNameBy(CurrentUser.EmployeeCode);
            var appointmentVM = new AppointmentViewModel
            {
                VisitDate = DateTime.Now,
                ProfileId = profileId,
                ProfileName = profile.ProfileName,
                CreateBy = CurrentUser.AccountId,
                CreateTime = DateTime.Now,
                Actived = true,
                Summary = _unitOfWork.TaskRepository.GetSummary(ProfileId: profileId, EmployeeName: EmployeeName),
                isVisitCabinetPro = false,
            };
            var salesEmpCode = CurrentUser.EmployeeCode;
            CreateViewBag(PriorityCode: ConstPriotityCode.NORMAL, SaleEmployeeCode: salesEmpCode, ProfileId: profileId);
            return PartialView("_FormAppointment", appointmentVM);
        }

        public ActionResult _Edit(Guid AppointmentId)
        {
            var appointmentInDb = _unitOfWork.AppointmentRepository.GetById(AppointmentId);
            if (appointmentInDb == null)
            {
                return HttpNotFound();
            }
            CreateViewBag(appointmentInDb.ShowroomCode, appointmentInDb.PriorityCode, appointmentInDb.CustomerClassCode, appointmentInDb.CategoryCode, appointmentInDb.ChannelCode, appointmentInDb.SaleEmployeeCode, appointmentInDb.ProfileId, appointmentInDb.StoreId, Ratings: appointmentInDb.Ratings, TaskStatusId: appointmentInDb.TaskStatusId);
            return PartialView("_FormAppointment", appointmentInDb);
        }

        #region SaveData
        [HttpPost]
        public JsonResult Save(AppointmentViewModel viewModel, List<TaskContactViewModel> contactList, List<string> productList)
        {
            return ExecuteContainer(() =>
            {
                //Create New
                if (viewModel.AppointmentId == Guid.Empty)
                {
                    //KH đã có kế hoạch thăm hỏi trong tương lai và cùng chi nhánh => không được thêm lần nữa
                    //var exist = (from p in _context.TaskModel
                    //             join a in _context.AppointmentModel on p.TaskId equals a.AppointmentId
                    //             where p.ProfileId == viewModel.ProfileId
                    //             && a.VisitDate >= viewModel.VisitDate
                    //             && viewModel.StoreId == p.StoreId
                    //             select p).FirstOrDefault();
                    //if (exist != null)
                    //{
                    //    return Json(new
                    //    {
                    //        Code = HttpStatusCode.NotModified,
                    //        Success = false,
                    //        Data = LanguageResource.Appointment_ExistError
                    //    });
                    //}

                    //Set field
                    var EmployeeName = _unitOfWork.SalesEmployeeRepository.GetSaleEmployeeNameBy(CurrentUser.EmployeeCode);
                    viewModel.Summary = _unitOfWork.TaskRepository.GetSummary(ProfileId: viewModel.ProfileId, EmployeeName: EmployeeName);

                    viewModel.CompanyId = _unitOfWork.StoreRepository.GetCompanyIdByStoreId(viewModel.StoreId);
                    viewModel.WorkFlowId = _unitOfWork.WorkFlowRepository.FindWorkFlowIdByCode(ConstWorkFlow.GT);
                    if (viewModel.TaskStatusId == null || viewModel.TaskStatusId == Guid.Empty)
                    {
                        var taskStatusList = _unitOfWork.TaskStatusRepository.GetTaskStatusByWorkFlow(viewModel.WorkFlowId);
                        var taskStatus = taskStatusList.FirstOrDefault(p => p.TaskStatusCode == ConstWorkFlow.GT && p.WorkFlowId == viewModel.WorkFlowId).TaskStatusId;
                        viewModel.TaskStatusId = (Guid)taskStatus;
                    }

                    viewModel.CreateBy = CurrentUser.AccountId;
                    viewModel.CreateTime = DateTime.Now;
                    viewModel.Actived = true;

                    _unitOfWork.AppointmentRepository.Create(viewModel);
                    //Lưu liên hệ vào bảng TaskContact
                    if (contactList != null && contactList.Count > 0)
                    {
                        int index = 0;
                        foreach (var item in contactList)
                        {
                            if (item.ContactId != null)
                            {
                                TaskContactModel contact = new TaskContactModel();
                                contact.TaskContactId = Guid.NewGuid();
                                contact.TaskId = viewModel.TaskId;
                                contact.ContactId = item.ContactId;
                                contact.isMain = index == 0 ? true : false;
                                contact.CreateBy = CurrentUser.AccountId;
                                _unitOfWork.TaskRepository.CreateTaskContact(contact);
                            }
                            index++;
                        }
                    }
                    //Thị hiếu
                    if (productList != null && productList.Count > 0)
                    {
                        foreach (var item in productList)
                        {
                            CustomerTastesModel customerTaste = new CustomerTastesModel();
                            customerTaste.CustomerTasteId = Guid.NewGuid();
                            customerTaste.ERPProductCode = item;
                            customerTaste.ProfileId = viewModel.ProfileId;
                            customerTaste.AppointmentId = viewModel.AppointmentId;
                            customerTaste.CreatedDate = DateTime.Now;
                            customerTaste.StoreId = _unitOfWork.StoreRepository.GetStoreIdBySaleOrgCode(CurrentUser.SaleOrg);
                            customerTaste.CompanyId = CurrentUser.CompanyId;
                            _context.Entry(customerTaste).State = EntityState.Added;
                        }
                    }
                    _context.SaveChanges();

                    return Json(new
                    {
                        Code = HttpStatusCode.Created,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Create_Success, LanguageResource.Appointment.ToLower())
                    });
                }
                //Edit
                else
                {
                    viewModel.LastEditBy = CurrentUser.AccountId;
                    viewModel.LastEditTime = DateTime.Now;

                    _unitOfWork.AppointmentRepository.Update(viewModel);

                    //Thị hiếu
                    var tasteList = _context.CustomerTastesModel.Where(p => p.AppointmentId == viewModel.AppointmentId).ToList();
                    var delList = (from p in tasteList
                                   where !productList.Contains(p.ERPProductCode)
                                   select p).ToList();
                    _context.CustomerTastesModel.RemoveRange(delList);

                    var existCodeList = tasteList.Select(p => p.ERPProductCode).ToList();
                    var newProductList = (from p in productList
                                          where !existCodeList.Contains(p)
                                          select p).ToList();

                    if (newProductList != null && newProductList.Count > 0)
                    {
                        foreach (var item in newProductList)
                        {
                            CustomerTastesModel customerTaste = new CustomerTastesModel();
                            customerTaste.CustomerTasteId = Guid.NewGuid();
                            customerTaste.ERPProductCode = item;
                            customerTaste.ProfileId = viewModel.ProfileId;
                            customerTaste.AppointmentId = viewModel.AppointmentId;
                            customerTaste.CreatedDate = DateTime.Now;
                            customerTaste.StoreId = _unitOfWork.StoreRepository.GetStoreIdBySaleOrgCode(CurrentUser.SaleOrg);
                            customerTaste.CompanyId = CurrentUser.CompanyId;
                            _context.Entry(customerTaste).State = EntityState.Added;
                        }
                    }
                    _context.SaveChanges();

                    return Json(new
                    {
                        Code = HttpStatusCode.Created,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Edit_Success, LanguageResource.Appointment.ToLower())
                    });
                }
            });
        }
        #endregion
        #endregion

        #region Delete
        [HttpPost]
        [ISDAuthorizationAttribute]
        public ActionResult Delete(Guid id)
        {
            return ExecuteDelete(() =>
            {
                var taskInDb = _context.TaskModel.FirstOrDefault(p => p.TaskId == id);
                if (taskInDb != null)
                {
                    taskInDb.Actived = false;
                    taskInDb.isDeleted = true;
                    taskInDb.DeleteBy = CurrentUser.AccountId;
                    _context.Entry(taskInDb).State = EntityState.Modified;
                    _context.SaveChanges();
                    return Json(new
                    {
                        Code = HttpStatusCode.OK,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Delete_Success, LanguageResource.Appointment.ToLower())
                    });
                }
                else
                {
                    return Json(new
                    {
                        Code = HttpStatusCode.NotFound,
                        Success = false,
                        Data = string.Format(LanguageResource.Alert_NotExist_Delete, LanguageResource.Appointment.ToLower())
                    });
                }


            });
        }
        #endregion

        #region CreateSearchViewBag
        private void CreateSearchViewBag(AppointmentSearchViewModel searchViewModel)
        {
            //Dropdown Company
            var companyList = _unitOfWork.CompanyRepository.GetAll(CurrentUser.isViewByStore, CurrentUser.AccountId);
            ViewBag.CompanyId = new SelectList(companyList, "CompanyId", "CompanyName", searchViewModel.CompanyId);

            //Dropdown Store
            var storeList = _unitOfWork.StoreRepository.GetAllStore(CurrentUser.isViewByStore, CurrentUser.AccountId);
            ViewBag.StoreId = new SelectList(storeList, "StoreId", "StoreName", searchViewModel.StoreId);

            //Địa điểm khách ghé
            var CustomerSourceList = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.CustomerSource);
            ViewBag.CustomerSourceCode = new SelectList(CustomerSourceList, "CatalogCode", "CatalogText_vi");

            //Nhân viên tiếp khách
            var _salesEmployeeRepository = new SalesEmployeeRepository(_context);
            var saleEmployeeList = _salesEmployeeRepository.GetAllForDropdownlist();
            ViewBag.SalesEmployeeCode = new SelectList(saleEmployeeList, "SalesEmployeeCode", "SalesEmployeeName", searchViewModel.SalesEmployeeCode);

            //Danh mục
            var categoryList = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.Appoitment_Category);
            ViewBag.CategoryCode = new SelectList(categoryList, "CatalogCode", "CatalogText_vi");

            //Phân loại khách hàng
            var customerClassList = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.CustomerClass);
            ViewBag.CustomerClassCode = new SelectList(customerClassList, "CatalogCode", "CatalogText_vi");

            //Get list Age (Độ tuổi)
            var ageList = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.Age);
            ViewBag.Age = new SelectList(ageList, "CatalogCode", "CatalogText_vi");
             
            //Get list CustomerType (Tiêu dùng, Doanh nghiệp || Liên hệ)
            var catalogList = _context.CatalogModel.Where(
                p => p.CatalogTypeCode == ConstCatalogType.CustomerType
                && p.Actived == true && p.CatalogCode != ConstCustomerType.Contact).OrderBy(p => p.OrderIndex).ToList();

            ViewBag.CustomerTypeCode = new SelectList(catalogList, "CatalogCode", "CatalogText_vi");

            //Trạng thái xử lý yêu cầu
            var WorkFlowId = _unitOfWork.WorkFlowRepository.FindWorkFlowIdByCode(ConstWorkFlow.GT);
            var taskStatusList = _unitOfWork.TaskStatusRepository.GetTaskStatusByWorkFlow(WorkFlowId);
            ViewBag.TaskStatusId = new SelectList(taskStatusList, "TaskStatusId", "TaskStatusName");

            //CommonDate
            var commonDateList = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.CommonDate);
            ViewBag.CommonDate = new SelectList(commonDateList, "CatalogCode", "CatalogText_vi", searchViewModel.CommonDate);
            ViewBag.CommonCreateDate = new SelectList(commonDateList, "CatalogCode", "CatalogText_vi", searchViewModel.CommonCreateDate);
            //ViewBag.CommonReceiveDate = new SelectList(commonDateList, "CatalogCode", "CatalogText_vi", searchViewModel.CommonReceiveDate);
            //ViewBag.CommonEndDate = new SelectList(commonDateList, "CatalogCode", "CatalogText_vi", searchViewModel.CommonEndDate);

            //Nhóm khách hàng
            var customerGroupList = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.CustomerCategory);
            ViewBag.CustomerGroupCode = new SelectList(customerGroupList, "CatalogCode", "CatalogText_vi");

            //Ngành nghề
            var customerCareerList = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.CustomerCareer);
            ViewBag.CustomerCareerCode = new SelectList(customerCareerList, "CatalogCode", "CatalogText_vi");

            //Ngày nhập thông tin

            //Bắc trung Nam
            var SaleOfficeList = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.SaleOffice);
            ViewBag.SaleOfficeCode = new SelectList(SaleOfficeList, "CatalogCode", "CatalogText_vi");

            var filterList = new List<DropdownlistFilter>();
            filterList.Add(new DropdownlistFilter { FilterCode = ConstCustomerFilter.SaleOfficeCode, FilterName = LanguageResource.Profile_SaleOfficeCode });
            filterList.Add(new DropdownlistFilter { FilterCode = ConstCustomerFilter.CustomerTypeCode, FilterName = LanguageResource.Profile_CustomerTypeCode });
            //filterList.Add(new DropdownlistFilter { FilterCode = ConstCustomerFilter.CustomerSourceCode, FilterName = LanguageResource.Profile_CustomerSourceCode });
            // filterList.Add(new DropdownlistFilter { FilterCode = ConstCustomerFilter.StoreId, FilterName = LanguageResource.MasterData_Store });
            //filterList.Add(new DropdownlistFilter { FilterCode = ConstCustomerFilter.SalesEmployeeCode, FilterName = LanguageResource.Appointment_SaleEmployeeCode });
            filterList.Add(new DropdownlistFilter { FilterCode = ConstCustomerFilter.Age, FilterName = LanguageResource.Profile_Age });
            filterList.Add(new DropdownlistFilter { FilterCode = ConstCustomerFilter.TaskStatusId, FilterName = LanguageResource.TaskStatus });
            filterList.Add(new DropdownlistFilter { FilterCode = ConstCustomerFilter.Phone, FilterName = LanguageResource.Profile_Phone });
            filterList.Add(new DropdownlistFilter { FilterCode = ConstCustomerFilter.TaxNo, FilterName = LanguageResource.Profile_TaxNo });
            filterList.Add(new DropdownlistFilter { FilterCode = ConstCustomerFilter.CustomerGroupCode, FilterName = LanguageResource.Profile_CustomerCategoryCode });
            filterList.Add(new DropdownlistFilter { FilterCode = ConstCustomerFilter.CustomerCareerCode, FilterName = LanguageResource.Profile_CustomerCareerCode });
            filterList.Add(new DropdownlistFilter { FilterCode = ConstCustomerFilter.Create, FilterName = LanguageResource.CreatedDate });
            ViewBag.Filters = filterList;
        }
        #endregion

        #region ViewBag helper
        private void CreateViewBag(string ShowroomCode = "", string PriorityCode = "", string CustomerClassCode = "", string CategoryCode = "", string ChannelCode = "", string SaleEmployeeCode = "", Guid? ProfileId = null, Guid? StoreId = null, string ERPProductCode = null, string Ratings = null, Guid? TaskStatusId = null)
        {
            //ShowRoom
            var showroomList = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.CustomerSource);
            ViewBag.ShowroomCode = new SelectList(showroomList, "CatalogCode", "CatalogText_vi", ShowroomCode);

            //Mức độ
            //var priorityList = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.Priority);
            //ViewBag.PriorityCode = new SelectList(priorityList, "CatalogCode", "CatalogText_vi", PriorityCode);
            //ViewBag.PriorityCodeList = new SelectList(priorityList, "CatalogCode", "CatalogText_vi", PriorityCode);
            ViewBag.PriorityCode = PriorityCode;

            //Phân loại khách hàng
            var customerClassList = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.CustomerClass);
            ViewBag.CustomerClassCode = new SelectList(customerClassList, "CatalogCode", "CatalogText_vi", CustomerClassCode);
            ViewBag.CustomerClassCodeList = new SelectList(customerClassList, "CatalogCode", "CatalogText_vi", CustomerClassCode);

            //Danh mục
            var categoryList = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.Appoitment_Category);
            ViewBag.CategoryCode = new SelectList(categoryList, "CatalogCode", "CatalogText_vi", CategoryCode);
            ViewBag.CategoryCodeList = new SelectList(categoryList, "CatalogCode", "CatalogText_vi", CategoryCode);

            //Khách biết đến ACN qua
            var channelList = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.Appoitment_Channel);
            ViewBag.ChannelCode = new SelectList(channelList, "CatalogCode", "CatalogText_vi", ChannelCode);

            //Nhân viên tiếp khách
            var _salesEmployeeRepository = new SalesEmployeeRepository(_context);
            var saleEmployeeList = _salesEmployeeRepository.GetAllForDropdownlist();
            ViewBag.SaleEmployeeCode = new SelectList(saleEmployeeList, "SalesEmployeeCode", "SalesEmployeeName", SaleEmployeeCode);

            //Chi Nhánh
            var storeList = _unitOfWork.StoreRepository.GetStoreByCompanyPermission(CurrentUser.AccountId, CurrentUser.CompanyId);
            //Nếu không có chi nhánh trong phân quyền => add thêm vào để hiển thị
            if (storeList.Where(p => p.StoreId == StoreId).FirstOrDefault() == null)
            {
                var store = _unitOfWork.StoreRepository.Find(StoreId);
                if (store != null)
                {
                    storeList.Add(store);
                }
            }
            ViewBag.StoreId = new SelectList(storeList, "StoreId", "StoreName", StoreId);

            //Liên hệ chính
            var listPrimaryContact = _unitOfWork.ProfileRepository.GetContactListOfProfile(ProfileId);
            ViewBag.PrimaryContactId = new SelectList(listPrimaryContact, "ProfileContactId", "ProfileContactName");

            //Get list CustomerType (Tiêu dùng, Doanh nghiệp || Liên hệ)
            var catalogList = _context.CatalogModel.Where(
                p => p.CatalogTypeCode == ConstCatalogType.CustomerType
                && p.Actived == true && p.CatalogCode != ConstCustomerType.Contact).OrderBy(p => p.OrderIndex).ToList();

            ViewBag.CustomerTypeCode = new SelectList(catalogList, "CatalogCode", "CatalogText_vi");

            #region //Get list Age (Độ tuổi)
            var ageList = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.Age);
            ViewBag.Age = new SelectList(ageList, "CatalogCode", "CatalogText_vi");
            #endregion

            //Trạng thái xử lý yêu cầu
            var WorkFlowId = _unitOfWork.WorkFlowRepository.FindWorkFlowIdByCode(ConstWorkFlow.GT);
            var taskStatusList = _unitOfWork.TaskStatusRepository.GetTaskStatusByWorkFlow(WorkFlowId);
            ViewBag.TaskStatusId = new SelectList(taskStatusList, "TaskStatusId", "TaskStatusName", TaskStatusId);


            //Liên hệ theo khách hàng doanh nghiệp
            var contacts = _unitOfWork.ProfileRepository.GetContactListOfProfile(ProfileId);
            ViewBag.ContactId = contacts;

            var lst = new List<ProductViewModel>();
            if (StoreId != null)
            {
                var SaleOrg = _unitOfWork.StoreRepository.GetSaleOrgCodeByStoreId(StoreId);
                var CompanyCode = _unitOfWork.StoreRepository.GetCompanyCodeBySaleOrgCode(SaleOrg);

                string sqlQuery = "EXEC [dbo].[usp_GetProduct_ACShopping] @CompanyCode";
                List<SqlParameter> parameters = new List<SqlParameter>()
                {
                    new SqlParameter
                    {
                        SqlDbType = SqlDbType.NVarChar,
                        Direction = ParameterDirection.Input,
                        ParameterName = "CompanyCode",
                        Value = CompanyCode
                    }
                };
                lst = _context.Database.SqlQuery<ProductViewModel>(sqlQuery, parameters.ToArray()).ToList();
            }
            ViewBag.ProductCode = lst;

            #region //Ý kiến khách hàng
            var ratingLst = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.CustomerReviews);
            ViewBag.Ratings = new SelectList(ratingLst, "CatalogCode", "CatalogText_vi", Ratings);
            #endregion
        }

        public ActionResult GetProductBySaleOrg(Guid? StoreId)
        {
            var SaleOrg = _unitOfWork.StoreRepository.GetSaleOrgCodeByStoreId(StoreId);
            var CompanyCode = _unitOfWork.StoreRepository.GetCompanyCodeBySaleOrgCode(SaleOrg);

            string sqlQuery = "EXEC [dbo].[usp_GetProduct_ACShopping] @CompanyCode";
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter
                {
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input,
                    ParameterName = "CompanyCode",
                    Value = CompanyCode
                }
            };
            var lst = _context.Database.SqlQuery<ProductViewModel>(sqlQuery, parameters.ToArray()).ToList();

            var jsonResult = Json(lst, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        #endregion

        //Export
        #region Export to excel
        //const string controllerCode = ConstExcelController.Appointment;
        const int startIndex = 8;

        public ActionResult ExportExcel(AppointmentSearchViewModel searchViewModel)
        {
            if (searchViewModel.CommonDate != "Custom")
            {
                DateTime? fromDate;
                DateTime? toDate;
                DateTime? fromPreviousDay;
                DateTime? toPreviousDay;


                _unitOfWork.CommonDateRepository.GetDateBy(searchViewModel.CommonDate, out fromDate, out toDate, out fromPreviousDay, out toPreviousDay);
                //Tìm kiếm kỳ hiện tại
                searchViewModel.FromDate = fromDate;
                searchViewModel.ToDate = toDate;
            }
            //Get data filter
            //Get data from server
            int resultCount = 0;
            var appointments = _unitOfWork.AppointmentRepository.SearchQueryAppointment(searchViewModel, CurrentUser.CompanyCode, out resultCount);

            return Export(appointments);
        }

        [ISDAuthorizationAttribute]
        public FileContentResult Export(List<AppointmentViewModel> viewModel)
        {
            #region Master
            List<ExcelTemplate> columns = new List<ExcelTemplate>();

            columns.Add(new ExcelTemplate { ColumnName = "TaskCode", isAllowedToEdit = false });
            columns.Add(new ExcelTemplate { ColumnName = "Summary", isAllowedToEdit = false });
            columns.Add(new ExcelTemplate { ColumnName = "ProfileCode", isAllowedToEdit = false });
            columns.Add(new ExcelTemplate { ColumnName = "ProfileName", isAllowedToEdit = false });
            columns.Add(new ExcelTemplate { ColumnName = "Phone", isAllowedToEdit = false });
            columns.Add(new ExcelTemplate { ColumnName = "TaxNo", isAllowedToEdit = false });
            columns.Add(new ExcelTemplate { ColumnName = "Address", isAllowedToEdit = false });
            columns.Add(new ExcelTemplate { ColumnName = "Email", isAllowedToEdit = false });
            columns.Add(new ExcelTemplate { ColumnName = "CustomerTypeName", isAllowedToEdit = false });
            columns.Add(new ExcelTemplate { ColumnName = "SaleEmployeeName", isAllowedToEdit = false });
            //columns.Add(new ExcelTemplate { ColumnName = "StartDate", isAllowedToEdit = false, isDateTime = true });
            //columns.Add(new ExcelTemplate { ColumnName = "EndDate", isAllowedToEdit = false, isDateTime = true });
            columns.Add(new ExcelTemplate { ColumnName = "Description", isAllowedToEdit = false });
            columns.Add(new ExcelTemplate { ColumnName = "ShowroomName", isAllowedToEdit = false });
            columns.Add(new ExcelTemplate { ColumnName = "StoreName", isAllowedToEdit = false });
            columns.Add(new ExcelTemplate { ColumnName = "VisitDate", isAllowedToEdit = false, isDateTime = true });
            columns.Add(new ExcelTemplate { ColumnName = "CreateTime", isAllowedToEdit = false, isDateTimeTime = true });

            #endregion Master

            //Header
            string fileheader = string.Format(LanguageResource.Export_ExcelHeader, LanguageResource.Appointment);
            //List<ExcelHeadingTemplate> heading initialize in BaseController
            //Default:
            //          1. heading[0] is controller code
            //          2. heading[1] is file name
            //          3. headinf[2] is warning (edit)
            heading.Add(new ExcelHeadingTemplate()
            {
                Content = "",//controllerCode,
                RowsToIgnore = 1,
                isWarning = false,
                isCode = true
            });
            heading.Add(new ExcelHeadingTemplate()
            {
                Content = fileheader.ToUpper(),
                RowsToIgnore = 1,
                isWarning = false,
                isCode = false
            });
            //heading.Add(new ExcelHeadingTemplate()
            //{
            //    Content = LanguageResource.Export_ExcelWarning1,
            //    RowsToIgnore = 0,
            //    isWarning = true,
            //    isCode = false
            //});
            //heading.Add(new ExcelHeadingTemplate()
            //{
            //    Content = LanguageResource.Export_ExcelWarning2,
            //    RowsToIgnore = 1,
            //    isWarning = true,
            //    isCode = false
            //});

            //Trạng thái
            //heading.Add(new ExcelHeadingTemplate()
            //{
            //    Content = string.Format(LanguageResource.Export_ExcelWarningActived, LanguageResource.MasterData_District),
            //    RowsToIgnore = 1,
            //    isWarning = true,
            //    isCode = false
            //});

            //Body
            byte[] filecontent = ClassExportExcel.ExportExcel(viewModel, columns, heading, true);
            //File name
            //Insert => THEM_MOI
            //Edit => CAP_NHAT
            //string exportType = LanguageResource.exportType_Insert;
            //if (isEdit == true)
            //{
            //    exportType = LanguageResource.exportType_Edit;
            //}
            string fileNameWithFormat = string.Format("{0}.xlsx", _unitOfWork.UtilitiesRepository.RemoveSign4VietnameseString(fileheader.ToUpper()).Replace(" ", "_"));

            return File(filecontent, ClassExportExcel.ExcelContentType, fileNameWithFormat);
        }
        #endregion Export to excel
    }
}