using ISD.Constant;
using ISD.EntityModels;
using ISD.Extensions;
using ISD.Repositories.Infrastructure.Extensions;
using ISD.Resources;
using ISD.ViewModels;
using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ISD.Repositories
{
    public class TaskRepository
    {

        EntityDataContext _context;

        /// <summary>
        /// Khởi tạo Task Repository
        /// </summary>
        /// <param name="db">Entiry Data Context</param>
        public TaskRepository(EntityDataContext db)
        {
            _context = db;
            _context.Database.CommandTimeout = 1800;
        }

        /// <summary>
        /// Tìm kiếm Task
        /// </summary>
        /// <param name="searchModel">Task Search Model</param>
        /// <returns>Danh sách TaskViewModel</returns>
        public List<TaskViewModel> Search(TaskSearchViewModel searchModel)
        {
            var listTask = (from t in _context.TaskModel
                            join p in _context.ProfileModel on t.ProfileId equals p.ProfileId into profileTable
                            from pro in profileTable.DefaultIfEmpty()
                            join c in _context.CatalogModel on t.PriorityCode equals c.CatalogCode
                            join w in _context.WorkFlowModel on t.WorkFlowId equals w.WorkFlowId
                            join ts in _context.TaskStatusModel on t.TaskStatusId equals ts.TaskStatusId
                            // join cm in _context.CompanyModel on t.CompanyId equals cm.CompanyId
                            //  join s in _context.StoreModel on t.StoreId equals s.StoreId
                            where
                            (searchModel.PriorityCode == null || t.PriorityCode.Contains(searchModel.PriorityCode))
                            && (searchModel.Summary == null || t.Summary.Contains(searchModel.Summary))
                            && (searchModel.TaskCode == null || t.TaskCode.ToString() == searchModel.TaskCode || t.SubtaskCode == searchModel.TaskCode)
                            //&& (searchModel.Actived == null || t.Actived == searchModel.Actived)
                            && t.isDeleted != true
                            select new TaskViewModel
                            {
                                TaskId = t.TaskId,
                                TaskCode = t.TaskCode,
                                Summary = t.Summary,
                                ProfileId = t.ProfileId,
                                ProfileName = pro.ProfileName,
                                Description = t.Description,
                                PriorityCode = t.PriorityCode,
                                PriorityText_vi = c.CatalogText_vi,
                                WorkFlowId = t.WorkFlowId,
                                WorkFlowName = w.WorkFlowName,
                                TaskStatusId = t.TaskStatusId,
                                TaskStatusName = ts.TaskStatusName,
                                ReceiveDate = t.ReceiveDate,
                                Actived = t.Actived
                            }).OrderBy(p => p.TaskCode).ToList();
            return listTask;
        }

        /// <summary>
        /// Tìm kiếm Task
        /// </summary>
        /// <param name="searchModel">Task Search Model</param>
        /// <returns>Danh sách TaskViewModel</returns>
        public IQueryable<TaskViewModel> SearchQuery(TaskSearchViewModel searchModel)
        {
            var listTask = (from t in _context.TaskModel
                            join p in _context.ProfileModel on t.ProfileId equals p.ProfileId into profileTable
                            from pro in profileTable.DefaultIfEmpty()
                                //Province
                            join pr in _context.ProvinceModel on pro.ProvinceId equals pr.ProvinceId into prG
                            from province in prG.DefaultIfEmpty()
                                //District
                            join d in _context.DistrictModel on pro.DistrictId equals d.DistrictId into dG
                            from district in dG.DefaultIfEmpty()
                            join c in _context.CatalogModel on t.PriorityCode equals c.CatalogCode
                            join w in _context.WorkFlowModel on t.WorkFlowId equals w.WorkFlowId
                            join ts in _context.TaskStatusModel on t.TaskStatusId equals ts.TaskStatusId
                            join re in _context.SalesEmployeeModel on t.Reporter equals re.SalesEmployeeCode into reg
                            from report in reg.DefaultIfEmpty()
                            where
                            (searchModel.PriorityCode == null || t.PriorityCode.Contains(searchModel.PriorityCode))
                            && (searchModel.Summary == null || t.Summary.Contains(searchModel.Summary))
                            && (searchModel.TaskCode == null || t.TaskCode.ToString() == searchModel.TaskCode || t.SubtaskCode == searchModel.TaskCode)
                            //&& (searchModel.Actived == null || t.Actived == searchModel.Actived)
                            && t.isDeleted != true
                            orderby t.CreateTime descending
                            select new TaskViewModel
                            {
                                TaskId = t.TaskId,
                                TaskCode = t.TaskCode,
                                Summary = t.Summary,
                                ProfileId = t.ProfileId,
                                ProfileCode = pro.ProfileCode.ToString(),
                                ProfileForeignCode = pro.ProfileForeignCode,
                                ProfileName = pro.ProfileName,
                                ProfileAddress = pro.Address,
                                ProvinceName = province == null ? "" : ", " + province.ProvinceName,
                                DistrictName = district == null ? "" : ", " + district.Appellation + " " + district.DistrictName,
                                Description = t.Description,
                                //PriorityCode = t.PriorityCode,
                                PriorityText_vi = c.CatalogText_vi,
                                //WorkFlowId = t.WorkFlowId,
                                //WorkFlowName = w.WorkFlowName,
                                //TaskStatusId = t.TaskStatusId,
                                TaskStatusName = ts.TaskStatusName,
                                //ReceiveDate = t.ReceiveDate,
                                StartDate = t.StartDate,
                                EndDate = t.EndDate,
                                Actived = t.Actived,
                                ReporterName = report.SalesEmployeeName
                            });

            return listTask;
        }

        public string GetWorkflowCategoryCode(Guid? TaskId)
        {
            string type = string.Empty;
            type = (from p in _context.TaskModel
                    join w in _context.WorkFlowModel on p.WorkFlowId equals w.WorkFlowId
                    where p.TaskId == TaskId
                    select w.WorkflowCategoryCode).FirstOrDefault();
            return type;
        }

        public string GetWorkflowCode(Guid? TaskId)
        {
            string code = string.Empty;
            code = (from p in _context.TaskModel
                    join w in _context.WorkFlowModel on p.WorkFlowId equals w.WorkFlowId
                    where p.TaskId == TaskId
                    select w.WorkFlowCode).FirstOrDefault();
            return code;
        }

        #region Search Task Query (not use)
        /// <summary>
        /// Filter Task
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns>IQueryable<TaskViewModel></returns>
        public IQueryable<TaskViewModel> SearchQueryTask(TaskSearchViewModel searchModel, string DomainImageWorkFlow = null, List<Guid> workflowList = null, Guid? AccountId = null)
        {
            //string DomainImageWorkFlow = ConstDomain.Domain  + "/Upload/WorkFlow/";
            //Nếu là MyWork hoặc MyFolow: Tìm tất cả các workflow

            IQueryable<TaskViewModel> query1;
            var currentDate = DateTime.Now.AddDays(1).AddSeconds(-1);
            //Nếu là loại Myflow hoặc MyWork => tìm theo kanban
            if (searchModel.Type == ConstWorkFlowCategory.MyFollow || searchModel.Type == ConstWorkFlowCategory.MyWork)
            {
                searchModel.Type = ConstWorkFlowCategory.ALL;

                query1 = (from t in _context.TaskModel
                              //Profile
                          join p in _context.ProfileModel on t.ProfileId equals p.ProfileId into profileTable
                          from pro in profileTable.DefaultIfEmpty()
                              //Province
                          join pr in _context.ProvinceModel on pro.ProvinceId equals pr.ProvinceId into prG
                          from province in prG.DefaultIfEmpty()
                              //District
                          join d in _context.DistrictModel on pro.DistrictId equals d.DistrictId into dG
                          from district in dG.DefaultIfEmpty()
                              //Contact
                          join tc in _context.TaskContactModel.Where(p => searchModel.ContactId == null || p.ContactId == searchModel.ContactId) on t.TaskId equals tc.TaskId into tcg
                          from taskContact in tcg.DefaultIfEmpty()
                              //Priority
                          join c in _context.CatalogModel on t.PriorityCode equals c.CatalogCode
                          //WorkFlowId
                          join w in _context.WorkFlowModel on t.WorkFlowId equals w.WorkFlowId
                          //TaskStatusId
                          join ts in _context.TaskStatusModel on t.TaskStatusId equals ts.TaskStatusId
                          //Reporter
                          join re in _context.SalesEmployeeModel on t.Reporter equals re.SalesEmployeeCode into reg
                          from report in reg.DefaultIfEmpty()
                              //CreateBy
                          join ac in _context.AccountModel on t.CreateBy equals ac.AccountId into acg
                          from ac1 in acg.DefaultIfEmpty()
                          join se in _context.SalesEmployeeModel on ac1.EmployeeCode equals se.SalesEmployeeCode into seg
                          from se1 in seg.DefaultIfEmpty()
                              //Kanban
                          join m in _context.Kanban_TaskStatus_Mapping on t.TaskStatusId equals m.TaskStatusId
                          join d in _context.KanbanDetailModel on m.KanbanDetailId equals d.KanbanDetailId

                          where
                          //1. Mã yêu cầu
                          (searchModel.TaskCode == null || t.TaskCode.ToString() == searchModel.TaskCode || t.SubtaskCode == searchModel.TaskCode)
                          //2. Yêu cầu
                          && (searchModel.Summary == null || t.Summary.Contains(searchModel.Summary))
                          //3. Type: MyWork, MyFollow, TICKET, ACTIVITIES, GTB
                          && ((searchModel.Type == ConstWorkFlowCategory.ALL) || w.WorkflowCategoryCode == searchModel.Type)
                          //4. Loại
                          && (searchModel.WorkFlowId == null || t.WorkFlowId == searchModel.WorkFlowId)
                          //5.a Trạng thái
                          && (searchModel.TaskStatusCode == null || ts.TaskStatusCode == searchModel.TaskStatusCode)
                          //5.b Nhóm trạng thái: Incomplete, Todo, Processing, Completed
                          //Tìm theo query3
                          //Quá hạn
                          && (searchModel.TaskProcessCode != ConstTaskStatus.Expired || (searchModel.TaskProcessCode == ConstTaskStatus.Expired && t.EstimateEndDate != null && t.EstimateEndDate <= currentDate && ts.ProcessCode != ConstTaskStatus.Completed))
                          //6. Người giao việc: EmployeeCode
                          && (searchModel.Reporter == null || t.Reporter == searchModel.Reporter)
                          //7. NV được phân công
                          //public string Assignee { get; set; }
                          //Tìm theo query2
                          //8. Khách hàng
                          && (searchModel.ProfileId == null || t.ProfileId == searchModel.ProfileId)
                          //9. Liên hệ
                          //&& (searchModel.ContactId == null || taskContact.ContactId == searchModel.ContactId)
                          //10. Người tạo
                          && (searchModel.CreateBy == null || se1.SalesEmployeeCode == searchModel.CreateBy)
                          //11. Mức độ
                          && (searchModel.PriorityCode == null || t.PriorityCode == searchModel.PriorityCode)
                          //12. Ngày tiếp nhận
                          //ReceiveFromDate
                          && (searchModel.ReceiveFromDate == null || searchModel.ReceiveFromDate <= t.ReceiveDate)
                          //ReceiveToDate
                          && (searchModel.ReceiveToDate == null || t.ReceiveDate <= searchModel.ReceiveToDate)
                          //13. Ngày bắt đầu
                          //StartFromDate
                          && (searchModel.StartFromDate == null || searchModel.StartFromDate <= t.StartDate)
                          //StartToDate
                          && (searchModel.StartToDate == null || t.StartDate <= searchModel.StartToDate)
                          //14. Ngày kết thúc dự kiến
                          //EstimateEndFromDate
                          && (searchModel.EstimateEndFromDate == null || searchModel.EstimateEndFromDate <= t.EstimateEndDate)
                          //EstimateEndToDate
                          && (searchModel.EstimateEndToDate == null || t.EstimateEndDate <= searchModel.EstimateEndToDate)
                          //15. Ngày kết thúc thực tế
                          //EndFromDate
                          && (searchModel.EndFromDate == null || searchModel.EndFromDate <= t.EndDate)
                          //EndToDate
                          && (searchModel.EndToDate == null || t.EndDate <= searchModel.EndToDate)
                          //16. Đơn vị thi công
                          && (searchModel.ConstructionUnit == null || t.ConstructionUnit == searchModel.ConstructionUnit)
                          //17. Nhóm công ty
                          && (searchModel.ServiceTechnicalTeamCode == null || searchModel.ServiceTechnicalTeamCode.Contains(t.ServiceTechnicalTeamCode))
                          //18. Lỗi hay gặp
                          && (searchModel.CommonMistakeCode == null || t.CommonMistakeCode == searchModel.CommonMistakeCode)
                          //19. Mã lỗi
                          && (searchModel.ErrorCode == null || t.ErrorCode == searchModel.ErrorCode)
                          //20. Loại mã lỗi
                          && (searchModel.ErrorTypeCode == null || t.ErrorTypeCode == searchModel.ErrorTypeCode)
                          //Kanban
                          && d.KanbanId == searchModel.KanbanId
                          //&& taskContact.isMain == true

                          && t.isDeleted != true
                          orderby t.CreateTime descending
                          select new TaskViewModel
                          {
                              TaskId = t.TaskId,
                              TaskCode = t.TaskCode,
                              Summary = t.Summary,
                              ProfileId = t.ProfileId,
                              ProfileCode = pro == null ? "" : pro.ProfileCode.ToString(),
                              ProfileForeignCode = pro == null ? "" : pro.ProfileForeignCode,
                              ProfileName = pro == null ? "" : pro.ProfileName,
                              ProfileAddress = pro == null ? "" : pro.Address,
                              ProvinceName = province == null ? "" : ", " + province.ProvinceName,
                              DistrictName = district == null ? "" : ", " + district.Appellation + " " + district.DistrictName,
                              Description = t.Description,
                              //PriorityCode = t.PriorityCode,
                              PriorityText_vi = c.CatalogText_vi,
                              WorkFlowId = t.WorkFlowId,
                              WorkFlowName = w.WorkFlowName,
                              WorkFlowCode = w.WorkFlowCode,
                              TaskStatusId = t.TaskStatusId,
                              TaskStatusName = ts.TaskStatusName,
                              ProcessCode = ts.ProcessCode,
                              ProcessCodeIndex = ts.OrderIndex,
                              ReceiveDate = t.ReceiveDate,
                              StartDate = t.StartDate,
                              EstimateEndDate = t.EstimateEndDate,
                              EndDate = t.EndDate,
                              Actived = t.Actived,
                              ReporterName = report == null ? "" : report.SalesEmployeeName,
                              //Kanban
                              id = t.TaskId.ToString(),
                              state = d.OrderIndex.ToString(),
                              label = "<a class='mr-10' href='/Work/Task/Edit/" + t.TaskId + "'><i class='fa fa-eye'></i></a><a class='btn-showTaskPopup' data-id='" + t.TaskId + "'>" + t.Summary + "</a>",
                              tags = w.WorkFlowCode + "." + t.TaskCode,
                              code = ts.ProcessCode,
                              WorkFlowImageUrl = DomainImageWorkFlow + w.ImageUrl,
                              //Maps
                              VisitAddress = t.VisitAddress,
                              lat = t.lat,
                              lng = t.lng,
                              isRequiredCheckin = t.isRequiredCheckin,
                              TaskStatusOrderIndex = ts.OrderIndex,
                              isPrivate = t.isPrivate,
                              Reporter = t.Reporter,
                              //Remind
                              isRemind = t.isRemind,
                              RemindTime = t.RemindTime,
                              RemindCycle = t.RemindCycle,
                              isRemindForReporter = t.isRemindForReporter,
                              isRemindForAssignee = t.isRemindForAssignee,
                          });
            }
            else
            {
                query1 = (from t in _context.TaskModel
                              //Profile
                          join p in _context.ProfileModel on t.ProfileId equals p.ProfileId into profileTable
                          from pro in profileTable.DefaultIfEmpty()
                              //Province
                          join pr in _context.ProvinceModel on pro.ProvinceId equals pr.ProvinceId into prG
                          from province in prG.DefaultIfEmpty()
                              //District
                          join d in _context.DistrictModel on pro.DistrictId equals d.DistrictId into dG
                          from district in dG.DefaultIfEmpty()
                              //Priority
                          join c in _context.CatalogModel on t.PriorityCode equals c.CatalogCode
                          //WorkFlowId
                          join w in _context.WorkFlowModel on t.WorkFlowId equals w.WorkFlowId
                          //TaskStatusId
                          join ts in _context.TaskStatusModel on t.TaskStatusId equals ts.TaskStatusId
                          //Reporter
                          join re in _context.SalesEmployeeModel on t.Reporter equals re.SalesEmployeeCode into reg
                          from report in reg.DefaultIfEmpty()
                              //CreateBy
                          join ac in _context.AccountModel on t.CreateBy equals ac.AccountId into acg
                          from ac1 in acg.DefaultIfEmpty()
                          join se in _context.SalesEmployeeModel on ac1.EmployeeCode equals se.SalesEmployeeCode into seg
                          from se1 in seg.DefaultIfEmpty()
                              //Contact
                          join tc in _context.TaskContactModel.Where(p => searchModel.ContactId == null || p.ContactId == searchModel.ContactId) on t.TaskId equals tc.TaskId into tcg
                          from taskContact in tcg.DefaultIfEmpty()

                          where
                          //1. Mã yêu cầu
                          (searchModel.TaskCode == null || t.TaskCode.ToString() == searchModel.TaskCode || t.SubtaskCode == searchModel.TaskCode)
                          //2. Yêu cầu
                          && (searchModel.Summary == null || t.Summary.Contains(searchModel.Summary))
                          //3. Type: MyWork, MyFollow, TICKET, ACTIVITIES, GTB
                          && ((searchModel.Type == ConstWorkFlowCategory.ALL) || w.WorkflowCategoryCode == searchModel.Type)
                          //4. Loại
                          && (searchModel.WorkFlowId == null || t.WorkFlowId == searchModel.WorkFlowId)
                          //5.a Trạng thái
                          && (searchModel.TaskStatusCode == null || ts.TaskStatusCode == searchModel.TaskStatusCode)
                          //5.b Nhóm trạng thái: Incomplete, Todo, Processing, Completed
                          //Tìm theo query3
                          //Quá hạn
                          && (searchModel.TaskProcessCode != ConstTaskStatus.Expired || (searchModel.TaskProcessCode == ConstTaskStatus.Expired && t.EstimateEndDate != null && t.EstimateEndDate <= currentDate && ts.ProcessCode != ConstTaskStatus.Completed))
                          //6. Người giao việc: EmployeeCode
                          && (searchModel.Reporter == null || t.Reporter == searchModel.Reporter)
                          //7. NV được phân công
                          //public string Assignee { get; set; }
                          //Tìm theo query2
                          //8. Khách hàng
                          && (searchModel.ProfileId == null || t.ProfileId == searchModel.ProfileId)
                          //9. Liên hệ
                          //&& (searchModel.ContactId == null || taskContact == null || taskContact.ContactId == searchModel.ContactId)
                          //10. Người tạo
                          && (searchModel.CreateBy == null || se1.SalesEmployeeCode == searchModel.CreateBy)
                          //11. Mức độ
                          && (searchModel.PriorityCode == null || t.PriorityCode == searchModel.PriorityCode)
                          //12. Ngày tiếp nhận
                          //ReceiveFromDate
                          && (searchModel.ReceiveFromDate == null || searchModel.ReceiveFromDate <= t.ReceiveDate)
                          //ReceiveToDate
                          && (searchModel.ReceiveToDate == null || t.ReceiveDate <= searchModel.ReceiveToDate)
                          //13. Ngày bắt đầu
                          //StartFromDate
                          && (searchModel.StartFromDate == null || searchModel.StartFromDate <= t.StartDate)
                          //StartToDate
                          && (searchModel.StartToDate == null || t.StartDate <= searchModel.StartToDate)
                          //14. Ngày kết thúc dự kiến
                          //EstimateEndFromDate
                          && (searchModel.EstimateEndFromDate == null || searchModel.EstimateEndFromDate <= t.EstimateEndDate)
                          //EstimateEndToDate
                          && (searchModel.EstimateEndToDate == null || t.EstimateEndDate <= searchModel.EstimateEndToDate)
                          //15. Ngày kết thúc thực tế
                          //EndFromDate
                          && (searchModel.EndFromDate == null || searchModel.EndFromDate <= t.EndDate)
                          //EndToDate
                          && (searchModel.EndToDate == null || t.EndDate <= searchModel.EndToDate)
                          //16. Đơn vị thi công
                          && (searchModel.ConstructionUnit == null || t.ConstructionUnit == searchModel.ConstructionUnit)
                          //17. Nhóm công ty
                          && (searchModel.ServiceTechnicalTeamCode == null || searchModel.ServiceTechnicalTeamCode.Contains(t.ServiceTechnicalTeamCode))
                          //18. Lỗi hay gặp
                          && (searchModel.CommonMistakeCode == null || t.CommonMistakeCode == searchModel.CommonMistakeCode)
                          //19. Mã lỗi
                          && (searchModel.ErrorCode == null || t.ErrorCode == searchModel.ErrorCode)
                          //20. Loại mã lỗi
                          && (searchModel.ErrorTypeCode == null || t.ErrorTypeCode == searchModel.ErrorTypeCode)

                          && t.isDeleted != true
                          orderby t.CreateTime descending
                          select new TaskViewModel
                          {
                              TaskId = t.TaskId,
                              TaskCode = t.TaskCode,
                              Summary = t.Summary,
                              ProfileId = t.ProfileId,
                              ProfileCode = pro == null ? "" : pro.ProfileCode.ToString(),
                              ProfileForeignCode = pro == null ? "" : pro.ProfileForeignCode,
                              ProfileName = pro == null ? "" : pro.ProfileName,
                              ProfileAddress = pro == null ? "" : pro.Address,
                              ProvinceName = province == null ? "" : ", " + province.ProvinceName,
                              DistrictName = district == null ? "" : ", " + district.Appellation + " " + district.DistrictName,
                              Description = t.Description,
                              //PriorityCode = t.PriorityCode,
                              PriorityText_vi = c.CatalogText_vi,
                              WorkFlowId = t.WorkFlowId,
                              WorkFlowName = w.WorkFlowName,
                              WorkFlowCode = w.WorkFlowCode,
                              TaskStatusId = t.TaskStatusId,
                              TaskStatusName = ts.TaskStatusName,
                              ProcessCode = ts.ProcessCode,
                              ProcessCodeIndex = ts.OrderIndex,
                              ReceiveDate = t.ReceiveDate,
                              StartDate = t.StartDate,
                              EstimateEndDate = t.EstimateEndDate,
                              EndDate = t.EndDate,
                              Actived = t.Actived,
                              Reporter = t.Reporter,
                              ReporterName = report == null ? "" : report.SalesEmployeeName,
                              WorkFlowImageUrl = DomainImageWorkFlow + w.ImageUrl,
                              tags = w.WorkFlowCode + "." + t.TaskCode,
                              //Maps
                              VisitAddress = t.VisitAddress,
                              lat = t.lat,
                              lng = t.lng,
                              isRequiredCheckin = t.isRequiredCheckin,
                              TaskStatusOrderIndex = ts.OrderIndex,
                              StoreId = t.StoreId,
                              CompanyId = t.CompanyId,
                              isPrivate = t.isPrivate,
                              //Remind
                              isRemind = t.isRemind,
                              RemindTime = t.RemindTime,
                              RemindCycle = t.RemindCycle,
                              isRemindForReporter = t.isRemindForReporter,
                              isRemindForAssignee = t.isRemindForAssignee,
                          });

                #region //Theo loại phân quyền xem công việc (chỉ filter khi không phải là MyWork và MyFollow)
                if (AccountId != null)
                {
                    var account = _context.AccountModel.Where(p => p.AccountId == AccountId.Value).FirstOrDefault();
                    if (account != null)
                    {
                        if (!string.IsNullOrEmpty(account.TaskFilterCode) && account.TaskFilterCode != ConstFilter.TatCa)
                        {
                            //1. Cá Nhân
                            if (account.TaskFilterCode == ConstFilter.CaNhan)
                            {
                                List<Guid> taskIdList = new List<Guid>();
                                //assignee
                                var queryFilter1 = (from assignee in _context.TaskAssignModel
                                                    where (assignee.SalesEmployeeCode == account.EmployeeCode)
                                                    group assignee by assignee.TaskId into g
                                                    select new { TaskId = g.Key }).ToList();

                                if (queryFilter1 != null && queryFilter1.Count > 0)
                                {
                                    taskIdList.AddRange(queryFilter1.Select(p => p.TaskId.Value));
                                }

                                query1 = from master in query1
                                         where taskIdList.Contains(master.TaskId) || master.Reporter == account.EmployeeCode
                                         select master;
                            }

                            //2. Chi Nhánh
                            else if (account.TaskFilterCode == ConstFilter.ChiNhanh)
                            {
                                //Lấy danh sách chi nhánh theo phân quyền
                                var accountInStore = account.StoreModel.ToList();
                                if (accountInStore != null && accountInStore.Count > 0)
                                {
                                    List<Guid> storeLst = new List<Guid>();
                                    storeLst.AddRange(accountInStore.Select(p => p.StoreId));

                                    //Filter task theo chi nhánh
                                    query1 = from master in query1
                                             where storeLst.Contains(master.StoreId)
                                             select master;
                                }

                            }
                            //3. Công Ty
                            else if (account.TaskFilterCode == ConstFilter.CongTy)
                            {
                                //Lấy danh sách chi nhánh theo phân quyền và tìm công ty
                                var accountInStore = account.StoreModel.ToList();
                                if (accountInStore != null && accountInStore.Count > 0)
                                {
                                    //Danh sách cty
                                    var companyLst = accountInStore.Select(p => p.CompanyId).Distinct().ToList();
                                    var companyIdLst = (from c in _context.CompanyModel
                                                        where companyLst.Contains(c.CompanyId)
                                                        select c.CompanyId
                                                      ).ToList();

                                    //Filter task theo chi nhánh
                                    query1 = from master in query1
                                             where companyIdLst.Contains(master.CompanyId)
                                             select master;
                                }

                            }
                        }
                    }

                }
                #endregion //Theo loại phân quyền xem công việc (chỉ filter khi không phải là MyWork và MyFollow)
            }

            //7. NV được phân công
            //public string Assignee { get; set; }
            if (!string.IsNullOrEmpty(searchModel.Assignee))
            {
                var query2 = (from assignee in _context.TaskAssignModel
                              where (assignee.SalesEmployeeCode == searchModel.Assignee)
                              group assignee by assignee.TaskId into g
                              select new { TaskId = g.Key });
                query1 = from master in query1
                         join detail in query2 on master.TaskId equals detail.TaskId
                         select master;
            }
            //5. Trạng thái: Incomplete, Todo, Processing, Completed, Expired
            //public string TaskStatusId { get; set; }
            if (!string.IsNullOrEmpty(searchModel.TaskProcessCode))
            {
                var query3 = (from t in _context.TaskStatusModel
                              where
                              (searchModel.TaskProcessCode == ConstTaskStatus.Expired && t.ProcessCode != ConstTaskStatus.Completed)
                              ||
                              (searchModel.TaskProcessCode == ConstTaskStatus.Incomplete && t.ProcessCode != ConstTaskStatus.Completed)
                              ||
                              (searchModel.TaskProcessCode == ConstTaskStatus.Todo && t.ProcessCode == ConstTaskStatus.Todo)
                              ||
                              (searchModel.TaskProcessCode == ConstTaskStatus.Processing && t.ProcessCode == ConstTaskStatus.Processing)
                              ||
                              ((searchModel.TaskProcessCode == ConstTaskStatus.CompletedOnTime || searchModel.TaskProcessCode == ConstTaskStatus.CompletedExpire) && t.ProcessCode == ConstTaskStatus.Completed)

                              group t by t.TaskStatusId into g
                              select new { TaskStatusId = g.Key });

                query1 = from master in query1
                         join detail in query3 on master.TaskStatusId equals detail.TaskStatusId
                         where
                         //Quá hạn
                         (searchModel.TaskProcessCode == ConstTaskStatus.Expired &&
                                master.EstimateEndDate.HasValue && master.EstimateEndDate < DateTime.Now)
                         ||
                         //Hoàn thành quá hạn
                         (searchModel.TaskProcessCode == ConstTaskStatus.CompletedExpire &&
                                master.ProcessCode == ConstTaskStatus.Completed && master.EstimateEndDate != null &&
                                master.EstimateEndDate.HasValue && master.EstimateEndDate < master.EndDate)
                         ||
                         //Hoàn thành đúng hạn
                         (searchModel.TaskProcessCode == ConstTaskStatus.CompletedOnTime &&
                                master.ProcessCode == ConstTaskStatus.Completed &&
                                (master.EstimateEndDate == null || master.EndDate <= master.EstimateEndDate))
                         ||
                         (searchModel.TaskProcessCode != ConstTaskStatus.Expired &&
                          searchModel.TaskProcessCode != ConstTaskStatus.CompletedExpire &&
                          searchModel.TaskProcessCode != ConstTaskStatus.CompletedOnTime &&
                                (master.EstimateEndDate == null || master.EstimateEndDate >= DateTime.Now))
                         select master;
            }
            //Lọc theo loại công việc (Calendar cho phép chọn nhiều loại công việc)
            if (workflowList != null && workflowList.Count > 0)
            {
                query1 = from p in query1
                         where workflowList.Contains(p.WorkFlowId)
                         select p;
            }

            //Nếu task riêng tư thì chỉ những người theo dõi và người được phân công xem

            if (AccountId != null)
            {
                var account = _context.AccountModel.Where(p => p.AccountId == AccountId).FirstOrDefault();
                List<Guid> taskIdList = new List<Guid>();
                // TODO: dữ liệu phình thì tốc độ chậm
                //assignee
                var queryFilter1 = (from assignee in _context.TaskAssignModel
                                    where (assignee.SalesEmployeeCode == account.EmployeeCode)
                                    group assignee by assignee.TaskId into g
                                    select new { TaskId = g.Key }).ToList();

                if (queryFilter1 != null && queryFilter1.Count > 0)
                {
                    taskIdList.AddRange(queryFilter1.Select(p => p.TaskId.Value));
                }

                query1 = (from p in query1
                          where ((p.isPrivate == null || p.isPrivate == false) || (p.isPrivate == true && (taskIdList.Contains(p.TaskId) || p.Reporter == account.EmployeeCode)))
                          select p);
            }


            return query1;
        }
        #endregion

        /// <summary>
        /// Search task by stored procedure
        /// </summary>
        /// <param name="searchModel"></param>
        /// <param name="filteredResultsCount"></param>
        /// <param name="DomainImageWorkFlow"></param>
        /// <param name="workflowList"></param>
        /// <param name="AccountId"></param>
        /// <param name="processCodeList"></param>
        /// <param name="errorList"></param>
        /// <param name="colorList"></param>
        /// <param name="CurrentCompanyCode"></param>
        /// <param name="IsReport"></param>
        /// <returns>List TaskViewModel</returns>
        public List<TaskViewModel> SearchQueryTaskProc(TaskSearchViewModel searchModel, out int filteredResultsCount, string DomainImageWorkFlow = null, List<Guid> workflowList = null, Guid? AccountId = null, List<string> processCodeList = null, List<string> errorList = null, List<string> colorList = null, string CurrentCompanyCode = null)
        {
            #region Task Process
            var TaskProcessCode_Todo = false;
            var TaskProcessCode_Processing = false;
            var TaskProcessCode_Incomplete = false;
            var TaskProcessCode_CompletedOnTime = false;
            var TaskProcessCode_CompletedExpire = false;
            var TaskProcessCode_Expired = false;

            if (processCodeList != null && processCodeList.Count > 0)
            {
                foreach (var item in processCodeList)
                {
                    //Việc cần làm
                    if (item == ConstTaskStatus.Todo)
                    {
                        TaskProcessCode_Todo = true;
                    }
                    //Đang thực hiện
                    if (item == ConstTaskStatus.Processing)
                    {
                        TaskProcessCode_Processing = true;
                    }
                    //Chưa hoàn thành
                    if (item == ConstTaskStatus.Incomplete)
                    {
                        TaskProcessCode_Incomplete = true;
                    }
                    //Hoàn thành đúng hạn
                    if (item == ConstTaskStatus.CompletedOnTime)
                    {
                        TaskProcessCode_CompletedOnTime = true;
                    }
                    //Hoàn thành quá hạn
                    if (item == ConstTaskStatus.CompletedExpire)
                    {
                        TaskProcessCode_CompletedExpire = true;
                    }
                    //Quá hạn
                    if (item == ConstTaskStatus.Expired)
                    {
                        TaskProcessCode_Expired = true;
                    }
                }
            }
            else
            {
                //Việc cần làm
                if (searchModel.TaskProcessCode == ConstTaskStatus.Todo)
                {
                    TaskProcessCode_Todo = true;
                }
                //Đang thực hiện
                if (searchModel.TaskProcessCode == ConstTaskStatus.Processing)
                {
                    TaskProcessCode_Processing = true;
                }
                //Chưa hoàn thành
                if (searchModel.TaskProcessCode == ConstTaskStatus.Incomplete)
                {
                    TaskProcessCode_Incomplete = true;
                }
                //Hoàn thành đúng hạn
                if (searchModel.TaskProcessCode == ConstTaskStatus.CompletedOnTime)
                {
                    TaskProcessCode_CompletedOnTime = true;
                }
                //Hoàn thành quá hạn
                if (searchModel.TaskProcessCode == ConstTaskStatus.CompletedExpire)
                {
                    TaskProcessCode_CompletedExpire = true;
                }
                //Quá hạn
                if (searchModel.TaskProcessCode == ConstTaskStatus.Expired)
                {
                    TaskProcessCode_Expired = true;
                }
            }
            #endregion

            #region WorkflowList
            //Build your record
            var tableWorkFlowSchema = new List<SqlMetaData>(1)
                {
                    new SqlMetaData("WorkFlowId", SqlDbType.UniqueIdentifier)
                }.ToArray();

            //And a table as a list of those records
            var tableWorkFlow = new List<SqlDataRecord>();
            if (workflowList != null && workflowList.Count > 0)
            {
                foreach (var r in workflowList)
                {
                    var tableRow = new SqlDataRecord(tableWorkFlowSchema);
                    tableRow.SetSqlGuid(0, r);
                    tableWorkFlow.Add(tableRow);
                }
            }
            else
            {
                var tableRow = new SqlDataRecord(tableWorkFlowSchema);
                tableWorkFlow.Add(tableRow);
            }
            #endregion

            #region UsualErrorCodeList
            //Build your record
            var tableErrorSchema = new List<SqlMetaData>(1)
                {
                    new SqlMetaData("Code", SqlDbType.NVarChar, 100)
                }.ToArray();

            //And a table as a list of those records
            var tableError = new List<SqlDataRecord>();
            if (errorList != null && errorList.Count > 0)
            {
                foreach (var r in errorList)
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
                    new SqlMetaData("Code", SqlDbType.NVarChar, 100)
                }.ToArray();

            //And a table as a list of those records
            var tableColor = new List<SqlDataRecord>();
            if (colorList != null && colorList.Count > 0)
            {
                foreach (var r in colorList)
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

            #region ServiceTechnicalTeamCode
            //Build your record
            var tableServiceTechnicalTeamCodeSchema = new List<SqlMetaData>(1)
                {
                    new SqlMetaData("Code", SqlDbType.NVarChar, 100)
                }.ToArray();

            //And a table as a list of those records
            var tableServiceTechnicalTeamCode = new List<SqlDataRecord>();
            List<string> serviceTechnicalTeamCodeLst = new List<string>();
            if (searchModel.ServiceTechnicalTeamCode != null && searchModel.ServiceTechnicalTeamCode.Count > 0)
            {
                foreach (var r in searchModel.ServiceTechnicalTeamCode)
                {
                    var tableRow = new SqlDataRecord(tableServiceTechnicalTeamCodeSchema);
                    tableRow.SetString(0, r);
                    if (!serviceTechnicalTeamCodeLst.Contains(r))
                    {
                        serviceTechnicalTeamCodeLst.Add(r);
                        tableServiceTechnicalTeamCode.Add(tableRow);
                    }
                }
            }
            else
            {
                var tableRow = new SqlDataRecord(tableServiceTechnicalTeamCodeSchema);
                tableServiceTechnicalTeamCode.Add(tableRow);
            }
            #endregion
            #region DepartmentCode
            //Build your record
            var tableDepartmentCodeSchema = new List<SqlMetaData>(1)
                {
                    new SqlMetaData("Code", SqlDbType.NVarChar, 100)
                }.ToArray();

            //And a table as a list of those records
            var tableDepartmentCode = new List<SqlDataRecord>();
            List<string> departmentCodeLst = new List<string>();
            if (searchModel.DepartmentCode != null && searchModel.DepartmentCode.Count > 0)
            {
                foreach (var r in searchModel.DepartmentCode)
                {
                    var tableRow = new SqlDataRecord(tableDepartmentCodeSchema);
                    tableRow.SetString(0, r);
                    if (!departmentCodeLst.Contains(r))
                    {
                        departmentCodeLst.Add(r);
                        tableDepartmentCode.Add(tableRow);
                    }
                }
            }
            else
            {
                var tableRow = new SqlDataRecord(tableDepartmentCodeSchema);
                tableDepartmentCode.Add(tableRow);
            }
            #endregion

            string sqlQuery = "EXEC [Task].[usp_SearchTask] @ServiceTechnicalTeamCode, @TaskCode, @Summary, @WorkFlowId, @TaskStatusCode, @TaskProcessCode, @Reporter, @Assignee, @ProfileId, @CompanyId, @CreateBy, @PriorityCode, @CreatedFromDate, @CreatedToDate, @ReceiveFromDate, @ReceiveToDate, @StartFromDate, @StartToDate, @EstimateEndFromDate, @EstimateEndToDate, @EndFromDate, @EndToDate, @KanbanId, @Type, @CommonMistakeCode, @ErrorCode, @ErrorTypeCode, @ConstructionUnit, @WorkFlowIdList, @AccountId, @TaskProcessCode_Todo, @TaskProcessCode_Processing, @TaskProcessCode_Incomplete, @TaskProcessCode_CompletedOnTime, @TaskProcessCode_CompletedExpire, @TaskProcessCode_Expired, @DomainImageWorkFlow, @ProductCategoryCode, @UsualErrorCodeList, @ProductColorCodeList, @ProfileGroupCode, @SalesSupervisorCode, @DepartmentCode, @VisitTypeCode, @CurrentCompanyCode, @Actived, @IsReport, @PageSize, @PageNumber, @FilteredResultsCount OUT, @CompletedEmployee, @ProvinceId, @DistrictId, @WardId, @ProfileCode, @ProfileName";
            if (searchModel.Type == ConstWorkFlowCategory.GTB)
            {
                sqlQuery += ",@AddressType, @VisitSaleOfficeCode";
            }
            
            var FilteredResultsCountOutParam = new SqlParameter();
            FilteredResultsCountOutParam.ParameterName = "FilteredResultsCount";
            FilteredResultsCountOutParam.SqlDbType = SqlDbType.Int;
            FilteredResultsCountOutParam.Direction = ParameterDirection.Output;

            #region Parameters
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter
                {
                    SqlDbType = SqlDbType.Structured,
                    Direction = ParameterDirection.Input,
                    ParameterName = "ServiceTechnicalTeamCode",
                    TypeName = "[dbo].[StringList]", //Don't forget this one!
                    Value = tableServiceTechnicalTeamCode,
                },
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
                    ParameterName = "Summary",
                    Value = searchModel.Summary ?? (object)DBNull.Value,
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.UniqueIdentifier,
                    Direction = ParameterDirection.Input,
                    ParameterName = "WorkFlowId",
                    Value = searchModel.WorkFlowId ?? (object)DBNull.Value,
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input,
                    ParameterName = "TaskStatusCode",
                    Value = searchModel.TaskStatusCode ?? (object)DBNull.Value
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input,
                    ParameterName = "TaskProcessCode",
                    Value = searchModel.TaskProcessCode ?? (object)DBNull.Value
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input,
                    ParameterName = "Reporter",
                    Value = searchModel.Reporter ?? (object)DBNull.Value
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input,
                    ParameterName = "Assignee",
                    Value = searchModel.Assignee ?? (object)DBNull.Value
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.UniqueIdentifier,
                    Direction = ParameterDirection.Input,
                    ParameterName = "ProfileId",
                    Value = searchModel.ProfileId ?? (object)DBNull.Value
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.UniqueIdentifier,
                    Direction = ParameterDirection.Input,
                    ParameterName = "CompanyId",
                    Value = searchModel.CompanyId ?? (object)DBNull.Value
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input,
                    ParameterName = "CreateBy",
                    Value = searchModel.CreateBy ?? (object)DBNull.Value
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input,
                    ParameterName = "PriorityCode",
                    Value = searchModel.PriorityCode ?? (object)DBNull.Value
                },
                 new SqlParameter
                {
                    SqlDbType = SqlDbType.DateTime,
                    Direction = ParameterDirection.Input,
                    ParameterName = "CreatedFromDate",
                    Value = searchModel.CreatedFromDate ?? (object)DBNull.Value
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.DateTime,
                    Direction = ParameterDirection.Input,
                    ParameterName = "CreatedToDate",
                    Value = searchModel.CreatedToDate ?? (object)DBNull.Value
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.DateTime,
                    Direction = ParameterDirection.Input,
                    ParameterName = "ReceiveFromDate",
                    Value = searchModel.ReceiveFromDate ?? (object)DBNull.Value
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.DateTime,
                    Direction = ParameterDirection.Input,
                    ParameterName = "ReceiveToDate",
                    Value = searchModel.ReceiveToDate ?? (object)DBNull.Value
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.DateTime,
                    Direction = ParameterDirection.Input,
                    ParameterName = "StartFromDate",
                    Value = searchModel.StartFromDate ?? (object)DBNull.Value
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.DateTime,
                    Direction = ParameterDirection.Input,
                    ParameterName = "StartToDate",
                    Value = searchModel.StartToDate ?? (object)DBNull.Value
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.DateTime,
                    Direction = ParameterDirection.Input,
                    ParameterName = "EstimateEndFromDate",
                    Value = searchModel.EstimateEndFromDate ?? (object)DBNull.Value
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.DateTime,
                    Direction = ParameterDirection.Input,
                    ParameterName = "EstimateEndToDate",
                    Value = searchModel.EstimateEndToDate ?? (object)DBNull.Value
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.DateTime,
                    Direction = ParameterDirection.Input,
                    ParameterName = "EndFromDate",
                    Value = searchModel.EndFromDate ?? (object)DBNull.Value
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.DateTime,
                    Direction = ParameterDirection.Input,
                    ParameterName = "EndToDate",
                    Value = searchModel.EndToDate ?? (object)DBNull.Value
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.UniqueIdentifier,
                    Direction = ParameterDirection.Input,
                    ParameterName = "KanbanId",
                    Value = searchModel.KanbanId ?? (object)DBNull.Value
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input,
                    ParameterName = "Type",
                    Value = searchModel.Type ?? (object)DBNull.Value
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input,
                    ParameterName = "CommonMistakeCode",
                    Value = searchModel.CommonMistakeCode ?? (object)DBNull.Value
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input,
                    ParameterName = "ErrorCode",
                    Value = searchModel.ErrorCode ?? (object)DBNull.Value
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input,
                    ParameterName = "ErrorTypeCode",
                    Value = searchModel.ErrorTypeCode ?? (object)DBNull.Value
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input,
                    ParameterName = "ConstructionUnit",
                    Value = searchModel.ConstructionUnit ?? (object)DBNull.Value
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.Structured,
                    Direction = ParameterDirection.Input,
                    ParameterName = "WorkFlowIdList",
                    TypeName = "[dbo].[WorkFlowIdList]", //Don't forget this one!
                    Value = tableWorkFlow
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.UniqueIdentifier,
                    Direction = ParameterDirection.Input,
                    ParameterName = "AccountId",
                    Value = AccountId ?? (object)DBNull.Value
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.Bit,
                    Direction = ParameterDirection.Input,
                    ParameterName = "TaskProcessCode_Todo",
                    Value = TaskProcessCode_Todo
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.Bit,
                    Direction = ParameterDirection.Input,
                    ParameterName = "TaskProcessCode_Processing",
                    Value = TaskProcessCode_Processing
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.Bit,
                    Direction = ParameterDirection.Input,
                    ParameterName = "TaskProcessCode_Incomplete",
                    Value = TaskProcessCode_Incomplete
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.Bit,
                    Direction = ParameterDirection.Input,
                    ParameterName = "TaskProcessCode_CompletedOnTime",
                    Value = TaskProcessCode_CompletedOnTime
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.Bit,
                    Direction = ParameterDirection.Input,
                    ParameterName = "TaskProcessCode_CompletedExpire",
                    Value = TaskProcessCode_CompletedExpire
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.Bit,
                    Direction = ParameterDirection.Input,
                    ParameterName = "TaskProcessCode_Expired",
                    Value = TaskProcessCode_Expired
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input,
                    ParameterName = "DomainImageWorkFlow",
                    Value = DomainImageWorkFlow ?? (object)DBNull.Value
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.Structured,
                    Direction = ParameterDirection.Input,
                    ParameterName = "ProductCategoryCode",
                    TypeName = "[dbo].[StringList]", //Don't forget this one!
                    Value = new UtilitiesRepository().ConvertTableFromListString(searchModel.ProductCategoryCode)
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.Structured,
                    Direction = ParameterDirection.Input,
                    ParameterName = "UsualErrorCodeList",
                    TypeName = "[dbo].[StringList]", //Don't forget this one!
                    Value = tableError
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.Structured,
                    Direction = ParameterDirection.Input,
                    ParameterName = "ProductColorCodeList",
                    TypeName = "[dbo].[StringList]", //Don't forget this one!
                    Value = tableColor
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input,
                    ParameterName = "ProfileGroupCode",
                    Value = searchModel.ProfileGroupCode ?? (object)DBNull.Value
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input,
                    ParameterName = "SalesSupervisorCode",
                    Value = searchModel.SalesSupervisorCode ?? (object)DBNull.Value
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.Structured,
                    Direction = ParameterDirection.Input,
                    ParameterName = "DepartmentCode",
                    TypeName = "[dbo].[StringList]", //Don't forget this one!
                    Value = tableDepartmentCode,
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.Structured,
                    Direction = ParameterDirection.Input,
                    ParameterName = "VisitTypeCode",
                    TypeName = "[dbo].[StringList]", //Don't forget this one!
                    Value = new UtilitiesRepository().ConvertTableFromListString(searchModel.VisitTypeCode)
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
                    SqlDbType = SqlDbType.Bit,
                    Direction = ParameterDirection.Input,
                    ParameterName = "Actived",
                    Value = searchModel.Actived ?? (object)DBNull.Value
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input,
                    ParameterName = "IsReport",
                    Value = false
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Input,
                    ParameterName = "PageSize",
                    Value = searchModel.PageSize ?? (object)DBNull.Value
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Input,
                    ParameterName = "PageNumber",
                    Value = searchModel.PageNumber ?? (object)DBNull.Value
                },
                FilteredResultsCountOutParam,
                new SqlParameter
                {
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input,
                    ParameterName = "CompletedEmployee",
                    Value = searchModel.CompletedEmployee ?? (object)DBNull.Value
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.Structured,
                    Direction = ParameterDirection.Input,
                    ParameterName = "ProvinceId",
                    TypeName = "[dbo].[GuidList]", //Don't forget this one!
                    Value = new UtilitiesRepository().ConvertTableFromListGuid(searchModel.ProvinceId)
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.Structured,
                    Direction = ParameterDirection.Input,
                    ParameterName = "DistrictId",
                    TypeName = "[dbo].[GuidList]", //Don't forget this one!
                    Value = new UtilitiesRepository().ConvertTableFromListGuid(searchModel.DistrictId)
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.Structured,
                    Direction = ParameterDirection.Input,
                    ParameterName = "WardId",
                    TypeName = "[dbo].[GuidList]", //Don't forget this one!
                    Value = new UtilitiesRepository().ConvertTableFromListGuid(searchModel.WardId)
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input,
                    ParameterName = "ProfileCode",
                    Value = searchModel.ProfileCode ?? (object)DBNull.Value
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input,
                    ParameterName = "ProfileName",
                    Value = searchModel.ProfileName ?? (object)DBNull.Value
                },

            };

            if (searchModel.Type == ConstWorkFlowCategory.GTB)
            {
                parameters.Add(new SqlParameter
                {
                    SqlDbType = SqlDbType.Bit,
                    Direction = ParameterDirection.Input,
                    ParameterName = "AddressType",
                    Value = searchModel.AddressType ?? (object)DBNull.Value,
                });
                parameters.Add(new SqlParameter
                {
                    SqlDbType = SqlDbType.Structured,
                    Direction = ParameterDirection.Input,
                    ParameterName = "VisitSaleOfficeCode",
                    TypeName = "[dbo].[StringList]", //Don't forget this one!
                    Value = new UtilitiesRepository().ConvertTableFromListString(searchModel.VisitSaleOfficeCode)
                });
            }
            #endregion
            var result = _context.Database.SqlQuery<TaskViewModel>(sqlQuery, parameters.ToArray()).ToList();
            
            var filteredResultsCountValue = FilteredResultsCountOutParam.Value;
            if (filteredResultsCountValue != null && filteredResultsCountValue != DBNull.Value)
            {
                filteredResultsCount = Convert.ToInt32(filteredResultsCountValue);
            }
            else
            {
                filteredResultsCount = 0;
            }
            return result;
        }

        #region ExportExcelTaskVisit
        public List<TaskExportVisitViewModel> ExportExcelTaskVisit(TaskSearchViewModel searchModel, out int filteredResultsCount, string DomainImageWorkFlow = null, List<Guid> workflowList = null, Guid? AccountId = null, List<string> processCodeList = null, List<string> errorList = null, List<string> colorList = null, string CurrentCompanyCode = null)
        {
            #region Task Process
            var TaskProcessCode_Todo = false;
            var TaskProcessCode_Processing = false;
            var TaskProcessCode_Incomplete = false;
            var TaskProcessCode_CompletedOnTime = false;
            var TaskProcessCode_CompletedExpire = false;
            var TaskProcessCode_Expired = false;

            if (processCodeList != null && processCodeList.Count > 0)
            {
                foreach (var item in processCodeList)
                {
                    //Việc cần làm
                    if (item == ConstTaskStatus.Todo)
                    {
                        TaskProcessCode_Todo = true;
                    }
                    //Đang thực hiện
                    if (item == ConstTaskStatus.Processing)
                    {
                        TaskProcessCode_Processing = true;
                    }
                    //Chưa hoàn thành
                    if (item == ConstTaskStatus.Incomplete)
                    {
                        TaskProcessCode_Incomplete = true;
                    }
                    //Hoàn thành đúng hạn
                    if (item == ConstTaskStatus.CompletedOnTime)
                    {
                        TaskProcessCode_CompletedOnTime = true;
                    }
                    //Hoàn thành quá hạn
                    if (item == ConstTaskStatus.CompletedExpire)
                    {
                        TaskProcessCode_CompletedExpire = true;
                    }
                    //Quá hạn
                    if (item == ConstTaskStatus.Expired)
                    {
                        TaskProcessCode_Expired = true;
                    }
                }
            }
            else
            {
                //Việc cần làm
                if (searchModel.TaskProcessCode == ConstTaskStatus.Todo)
                {
                    TaskProcessCode_Todo = true;
                }
                //Đang thực hiện
                if (searchModel.TaskProcessCode == ConstTaskStatus.Processing)
                {
                    TaskProcessCode_Processing = true;
                }
                //Chưa hoàn thành
                if (searchModel.TaskProcessCode == ConstTaskStatus.Incomplete)
                {
                    TaskProcessCode_Incomplete = true;
                }
                //Hoàn thành đúng hạn
                if (searchModel.TaskProcessCode == ConstTaskStatus.CompletedOnTime)
                {
                    TaskProcessCode_CompletedOnTime = true;
                }
                //Hoàn thành quá hạn
                if (searchModel.TaskProcessCode == ConstTaskStatus.CompletedExpire)
                {
                    TaskProcessCode_CompletedExpire = true;
                }
                //Quá hạn
                if (searchModel.TaskProcessCode == ConstTaskStatus.Expired)
                {
                    TaskProcessCode_Expired = true;
                }
            }
            #endregion

            #region WorkflowList
            //Build your record
            var tableWorkFlowSchema = new List<SqlMetaData>(1)
                {
                    new SqlMetaData("WorkFlowId", SqlDbType.UniqueIdentifier)
                }.ToArray();

            //And a table as a list of those records
            var tableWorkFlow = new List<SqlDataRecord>();
            if (workflowList != null && workflowList.Count > 0)
            {
                foreach (var r in workflowList)
                {
                    var tableRow = new SqlDataRecord(tableWorkFlowSchema);
                    tableRow.SetSqlGuid(0, r);
                    tableWorkFlow.Add(tableRow);
                }
            }
            else
            {
                var tableRow = new SqlDataRecord(tableWorkFlowSchema);
                tableWorkFlow.Add(tableRow);
            }
            #endregion

            #region UsualErrorCodeList
            //Build your record
            var tableErrorSchema = new List<SqlMetaData>(1)
                {
                    new SqlMetaData("Code", SqlDbType.NVarChar, 50)
                }.ToArray();

            //And a table as a list of those records
            var tableError = new List<SqlDataRecord>();
            if (errorList != null && errorList.Count > 0)
            {
                foreach (var r in errorList)
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
                    new SqlMetaData("WorkFlowId", SqlDbType.NVarChar, 50)
                }.ToArray();

            //And a table as a list of those records
            var tableColor = new List<SqlDataRecord>();
            if (colorList != null && colorList.Count > 0)
            {
                foreach (var r in colorList)
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
            #region ServiceTechnicalTeamCode
            //Build your record
            var tableServiceTechnicalTeamCodeSchema = new List<SqlMetaData>(1)
                {
                    new SqlMetaData("Code", SqlDbType.NVarChar, 100)
                }.ToArray();

            //And a table as a list of those records
            var tableServiceTechnicalTeamCode = new List<SqlDataRecord>();
            List<string> serviceTechnicalTeamCodeLst = new List<string>();
            if (searchModel.ServiceTechnicalTeamCode != null && searchModel.ServiceTechnicalTeamCode.Count > 0)
            {
                foreach (var r in searchModel.ServiceTechnicalTeamCode)
                {
                    var tableRow = new SqlDataRecord(tableServiceTechnicalTeamCodeSchema);
                    tableRow.SetString(0, r);
                    if (!serviceTechnicalTeamCodeLst.Contains(r))
                    {
                        serviceTechnicalTeamCodeLst.Add(r);
                        tableServiceTechnicalTeamCode.Add(tableRow);
                    }
                }
            }
            else
            {
                var tableRow = new SqlDataRecord(tableServiceTechnicalTeamCodeSchema);
                tableServiceTechnicalTeamCode.Add(tableRow);
            }
            #endregion

            string sqlQuery = "EXEC [Task].[usp_SearchTask] @ServiceTechnicalTeamCode, @TaskCode, @Summary, @WorkFlowId, @TaskStatusCode, @TaskProcessCode, @Reporter, @Assignee, @ProfileId, @CompanyId, @CreateBy, @PriorityCode, @CreatedFromDate, @CreatedToDate, @ReceiveFromDate, @ReceiveToDate, @StartFromDate, @StartToDate, @EstimateEndFromDate, @EstimateEndToDate, @EndFromDate, @EndToDate, @KanbanId, @Type, @CommonMistakeCode, @ErrorCode, @ErrorTypeCode, @ConstructionUnit, @WorkFlowIdList, @AccountId, @TaskProcessCode_Todo, @TaskProcessCode_Processing, @TaskProcessCode_Incomplete, @TaskProcessCode_CompletedOnTime, @TaskProcessCode_CompletedExpire, @TaskProcessCode_Expired, @DomainImageWorkFlow, @ProductCategoryCode, @UsualErrorCodeList, @ProductColorCodeList, @ProfileGroupCode, @SalesSupervisorCode, @DepartmentCode, @VisitTypeCode, @CurrentCompanyCode, @Actived, @IsReport, @PageSize, @PageNumber, @FilteredResultsCount OUT, @CompletedEmployee, @ProvinceId, @DistrictId, @WardId, @ProfileCode, @ProfileName";
            if (searchModel.Type == ConstWorkFlowCategory.GTB)
            {
                sqlQuery += ",@AddressType, @VisitSaleOfficeCode";
            }

            var FilteredResultsCountOutParam = new SqlParameter();
            FilteredResultsCountOutParam.ParameterName = "FilteredResultsCount";
            FilteredResultsCountOutParam.SqlDbType = SqlDbType.Int;
            FilteredResultsCountOutParam.Direction = ParameterDirection.Output;

            #region Parameters
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter
                {
                    SqlDbType = SqlDbType.Structured,
                    Direction = ParameterDirection.Input,
                    ParameterName = "ServiceTechnicalTeamCode",
                    TypeName = "[dbo].[StringList]", //Don't forget this one!
                    Value = tableServiceTechnicalTeamCode,
                },
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
                    ParameterName = "Summary",
                    Value = searchModel.Summary ?? (object)DBNull.Value,
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.UniqueIdentifier,
                    Direction = ParameterDirection.Input,
                    ParameterName = "WorkFlowId",
                    Value = searchModel.WorkFlowId ?? (object)DBNull.Value,
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input,
                    ParameterName = "TaskStatusCode",
                    Value = searchModel.TaskStatusCode ?? (object)DBNull.Value
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input,
                    ParameterName = "TaskProcessCode",
                    Value = searchModel.TaskProcessCode ?? (object)DBNull.Value
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input,
                    ParameterName = "Reporter",
                    Value = searchModel.Reporter ?? (object)DBNull.Value
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input,
                    ParameterName = "Assignee",
                    Value = searchModel.Assignee ?? (object)DBNull.Value
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.UniqueIdentifier,
                    Direction = ParameterDirection.Input,
                    ParameterName = "ProfileId",
                    Value = searchModel.ProfileId ?? (object)DBNull.Value
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.UniqueIdentifier,
                    Direction = ParameterDirection.Input,
                    ParameterName = "CompanyId",
                    Value = searchModel.CompanyId ?? (object)DBNull.Value
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input,
                    ParameterName = "CreateBy",
                    Value = searchModel.CreateBy ?? (object)DBNull.Value
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input,
                    ParameterName = "PriorityCode",
                    Value = searchModel.PriorityCode ?? (object)DBNull.Value
                },
                 new SqlParameter
                {
                    SqlDbType = SqlDbType.DateTime,
                    Direction = ParameterDirection.Input,
                    ParameterName = "CreatedFromDate",
                    Value = searchModel.CreatedFromDate ?? (object)DBNull.Value
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.DateTime,
                    Direction = ParameterDirection.Input,
                    ParameterName = "CreatedToDate",
                    Value = searchModel.CreatedToDate ?? (object)DBNull.Value
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.DateTime,
                    Direction = ParameterDirection.Input,
                    ParameterName = "ReceiveFromDate",
                    Value = searchModel.ReceiveFromDate ?? (object)DBNull.Value
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.DateTime,
                    Direction = ParameterDirection.Input,
                    ParameterName = "ReceiveToDate",
                    Value = searchModel.ReceiveToDate ?? (object)DBNull.Value
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.DateTime,
                    Direction = ParameterDirection.Input,
                    ParameterName = "StartFromDate",
                    Value = searchModel.StartFromDate ?? (object)DBNull.Value
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.DateTime,
                    Direction = ParameterDirection.Input,
                    ParameterName = "StartToDate",
                    Value = searchModel.StartToDate ?? (object)DBNull.Value
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.DateTime,
                    Direction = ParameterDirection.Input,
                    ParameterName = "EstimateEndFromDate",
                    Value = searchModel.EstimateEndFromDate ?? (object)DBNull.Value
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.DateTime,
                    Direction = ParameterDirection.Input,
                    ParameterName = "EstimateEndToDate",
                    Value = searchModel.EstimateEndToDate ?? (object)DBNull.Value
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.DateTime,
                    Direction = ParameterDirection.Input,
                    ParameterName = "EndFromDate",
                    Value = searchModel.EndFromDate ?? (object)DBNull.Value
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.DateTime,
                    Direction = ParameterDirection.Input,
                    ParameterName = "EndToDate",
                    Value = searchModel.EndToDate ?? (object)DBNull.Value
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.UniqueIdentifier,
                    Direction = ParameterDirection.Input,
                    ParameterName = "KanbanId",
                    Value = searchModel.KanbanId ?? (object)DBNull.Value
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input,
                    ParameterName = "Type",
                    Value = searchModel.Type ?? (object)DBNull.Value
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input,
                    ParameterName = "CommonMistakeCode",
                    Value = searchModel.CommonMistakeCode ?? (object)DBNull.Value
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input,
                    ParameterName = "ErrorCode",
                    Value = searchModel.ErrorCode ?? (object)DBNull.Value
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input,
                    ParameterName = "ErrorTypeCode",
                    Value = searchModel.ErrorTypeCode ?? (object)DBNull.Value
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input,
                    ParameterName = "ConstructionUnit",
                    Value = searchModel.ConstructionUnit ?? (object)DBNull.Value
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.Structured,
                    Direction = ParameterDirection.Input,
                    ParameterName = "WorkFlowIdList",
                    TypeName = "[dbo].[WorkFlowIdList]", //Don't forget this one!
                    Value = tableWorkFlow
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.UniqueIdentifier,
                    Direction = ParameterDirection.Input,
                    ParameterName = "AccountId",
                    Value = AccountId ?? (object)DBNull.Value
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.Bit,
                    Direction = ParameterDirection.Input,
                    ParameterName = "TaskProcessCode_Todo",
                    Value = TaskProcessCode_Todo
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.Bit,
                    Direction = ParameterDirection.Input,
                    ParameterName = "TaskProcessCode_Processing",
                    Value = TaskProcessCode_Processing
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.Bit,
                    Direction = ParameterDirection.Input,
                    ParameterName = "TaskProcessCode_Incomplete",
                    Value = TaskProcessCode_Incomplete
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.Bit,
                    Direction = ParameterDirection.Input,
                    ParameterName = "TaskProcessCode_CompletedOnTime",
                    Value = TaskProcessCode_CompletedOnTime
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.Bit,
                    Direction = ParameterDirection.Input,
                    ParameterName = "TaskProcessCode_CompletedExpire",
                    Value = TaskProcessCode_CompletedExpire
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.Bit,
                    Direction = ParameterDirection.Input,
                    ParameterName = "TaskProcessCode_Expired",
                    Value = TaskProcessCode_Expired
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input,
                    ParameterName = "DomainImageWorkFlow",
                    Value = DomainImageWorkFlow ?? (object)DBNull.Value
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input,
                    ParameterName = "ProductCategoryCode",
                    Value = searchModel.ProductCategoryCode ?? (object)DBNull.Value
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.Structured,
                    Direction = ParameterDirection.Input,
                    ParameterName = "UsualErrorCodeList",
                    TypeName = "[dbo].[StringList]", //Don't forget this one!
                    Value = tableError
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.Structured,
                    Direction = ParameterDirection.Input,
                    ParameterName = "ProductColorCodeList",
                    TypeName = "[dbo].[StringList]", //Don't forget this one!
                    Value = tableColor
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input,
                    ParameterName = "ProfileGroupCode",
                    Value = searchModel.ProfileGroupCode ?? (object)DBNull.Value
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input,
                    ParameterName = "SalesSupervisorCode",
                    Value = searchModel.SalesSupervisorCode ?? (object)DBNull.Value
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input,
                    ParameterName = "DepartmentCode",
                    Value = searchModel.DepartmentCode ?? (object)DBNull.Value
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.Structured,
                    Direction = ParameterDirection.Input,
                    ParameterName = "VisitTypeCode",
                    Value = searchModel.VisitTypeCode ?? (object)DBNull.Value
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
                    SqlDbType = SqlDbType.Bit,
                    Direction = ParameterDirection.Input,
                    ParameterName = "Actived",
                    Value = searchModel.Actived ?? (object)DBNull.Value
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input,
                    ParameterName = "IsReport",
                    Value = false
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Input,
                    ParameterName = "PageSize",
                    Value = searchModel.PageSize ?? (object)DBNull.Value
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Input,
                    ParameterName = "PageNumber",
                    Value = searchModel.PageNumber ?? (object)DBNull.Value
                },
                FilteredResultsCountOutParam,
                new SqlParameter
                {
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input,
                    ParameterName = "CompletedEmployee",
                    Value = searchModel.CompletedEmployee ?? (object)DBNull.Value
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.Structured,
                    Direction = ParameterDirection.Input,
                    ParameterName = "ProvinceId",
                    TypeName = "[dbo].[GuidList]", //Don't forget this one!
                    Value = new UtilitiesRepository().ConvertTableFromListGuid(searchModel.ProvinceId)
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.UniqueIdentifier,
                    Direction = ParameterDirection.Input,
                    ParameterName = "DistrictId",
                    Value = searchModel.DistrictId ?? (object)DBNull.Value
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.UniqueIdentifier,
                    Direction = ParameterDirection.Input,
                    ParameterName = "WardId",
                    Value = searchModel.WardId ?? (object)DBNull.Value
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input,
                    ParameterName = "ProfileCode",
                    Value = searchModel.ProfileCode ?? (object)DBNull.Value
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input,
                    ParameterName = "ProfileName",
                    Value = searchModel.ProfileName ?? (object)DBNull.Value
                },

            };

            if (searchModel.Type == ConstWorkFlowCategory.GTB)
            {
                parameters.Add(new SqlParameter
                {
                    SqlDbType = SqlDbType.Bit,
                    Direction = ParameterDirection.Input,
                    ParameterName = "AddressType",
                    Value = searchModel.AddressType ?? (object)DBNull.Value,
                });
                parameters.Add(new SqlParameter
                {
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input,
                    ParameterName = "VisitSaleOfficeCode",
                    TypeName = "[dbo].[StringList]", //Don't forget this one!
                    Value = new UtilitiesRepository().ConvertTableFromListString(searchModel.VisitSaleOfficeCode),
                });
            }
            #endregion
            var result = _context.Database.SqlQuery<TaskExportVisitViewModel>(sqlQuery, parameters.ToArray()).ToList();

            var filteredResultsCountValue = FilteredResultsCountOutParam.Value;
            if (filteredResultsCountValue != null && filteredResultsCountValue != DBNull.Value)
            {
                filteredResultsCount = Convert.ToInt32(filteredResultsCountValue);
            }
            else
            {
                filteredResultsCount = 0;
            }
            return result;
        }

        #endregion

        /// <summary>
        /// Export excel task by Type
        /// </summary>
        /// <param name="searchModel"></param>
        /// <param name="filteredResultsCount"></param>
        /// <param name="DomainImageWorkFlow"></param>
        /// <param name="workflowList"></param>
        /// <param name="AccountId"></param>
        /// <param name="processCodeList"></param>
        /// <param name="errorList"></param>
        /// <param name="colorList"></param>
        /// <param name="CurrentCompanyCode"></param>
        /// <param name="IsReport"></param>
        /// <returns></returns>
        public List<TaskExcelViewModel> SearchQueryTaskExcelProc(TaskSearchViewModel searchModel, out int filteredResultsCount, string DomainImageWorkFlow = null, List<Guid> workflowList = null, Guid? AccountId = null, List<string> processCodeList = null, List<string> errorList = null, List<string> colorList = null, string CurrentCompanyCode = null, List<string> taskStatusCodeList = null, List<string> assigneeList = null, List<Guid> workFlowId = null, List<string> productCategoryCodeList = null, List<string> profileGroupCodeList = null, List<string> departmentCodeList = null)
        {
            var result = new List<TaskExcelViewModel>();
            #region Task Process
            var TaskProcessCode_Todo = false;
            var TaskProcessCode_Processing = false;
            var TaskProcessCode_Incomplete = false;
            var TaskProcessCode_CompletedOnTime = false;
            var TaskProcessCode_CompletedExpire = false;
            var TaskProcessCode_Expired = false;

            if (processCodeList != null && processCodeList.Count > 0)
            {
                foreach (var item in processCodeList)
                {
                    //Việc cần làm
                    if (item == ConstTaskStatus.Todo)
                    {
                        TaskProcessCode_Todo = true;
                    }
                    //Đang thực hiện
                    if (item == ConstTaskStatus.Processing)
                    {
                        TaskProcessCode_Processing = true;
                    }
                    //Chưa hoàn thành
                    if (item == ConstTaskStatus.Incomplete)
                    {
                        TaskProcessCode_Incomplete = true;
                    }
                    //Hoàn thành đúng hạn
                    if (item == ConstTaskStatus.CompletedOnTime)
                    {
                        TaskProcessCode_CompletedOnTime = true;
                    }
                    //Hoàn thành quá hạn
                    if (item == ConstTaskStatus.CompletedExpire)
                    {
                        TaskProcessCode_CompletedExpire = true;
                    }
                    //Quá hạn
                    if (item == ConstTaskStatus.Expired)
                    {
                        TaskProcessCode_Expired = true;
                    }
                }
            }
            else
            {
                //Việc cần làm
                if (searchModel.TaskProcessCode == ConstTaskStatus.Todo)
                {
                    TaskProcessCode_Todo = true;
                }
                //Đang thực hiện
                if (searchModel.TaskProcessCode == ConstTaskStatus.Processing)
                {
                    TaskProcessCode_Processing = true;
                }
                //Chưa hoàn thành
                if (searchModel.TaskProcessCode == ConstTaskStatus.Incomplete)
                {
                    TaskProcessCode_Incomplete = true;
                }
                //Hoàn thành đúng hạn
                if (searchModel.TaskProcessCode == ConstTaskStatus.CompletedOnTime)
                {
                    TaskProcessCode_CompletedOnTime = true;
                }
                //Hoàn thành quá hạn
                if (searchModel.TaskProcessCode == ConstTaskStatus.CompletedExpire)
                {
                    TaskProcessCode_CompletedExpire = true;
                }
                //Quá hạn
                if (searchModel.TaskProcessCode == ConstTaskStatus.Expired)
                {
                    TaskProcessCode_Expired = true;
                }
            }
            #endregion

            #region WorkflowList
            //Build your record
            var tableWorkFlowSchema = new List<SqlMetaData>(1)
            {
                new SqlMetaData("WorkFlowId", SqlDbType.UniqueIdentifier)
            }.ToArray();

            //And a table as a list of those records
            var tableWorkFlow = new List<SqlDataRecord>();
            if (workflowList != null && workflowList.Count > 0)
            {
                foreach (var r in workflowList)
                {
                    var tableRow = new SqlDataRecord(tableWorkFlowSchema);
                    tableRow.SetSqlGuid(0, r);
                    tableWorkFlow.Add(tableRow);
                }
            }
            else
            {
                var tableRow = new SqlDataRecord(tableWorkFlowSchema);
                tableWorkFlow.Add(tableRow);
            }
            #endregion

            #region UsualErrorCodeList
            //Build your record
            var tableErrorSchema = new List<SqlMetaData>(1)
            {
                new SqlMetaData("Code", SqlDbType.NVarChar, 50)
            }.ToArray();

            //And a table as a list of those records
            var tableError = new List<SqlDataRecord>();
            if (errorList != null && errorList.Count > 0)
            {
                foreach (var r in errorList)
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
            if (colorList != null && colorList.Count > 0)
            {
                foreach (var r in colorList)
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

            #region TaskStatusCodeList - Trạng thái
            //And a table as a list of those records
            var tableTaskStatusCode = new List<SqlDataRecord>();
            if (taskStatusCodeList != null && taskStatusCodeList.Count > 0)
            {
                foreach (var t in taskStatusCodeList)
                {
                    var tableRow = new SqlDataRecord(tableErrorSchema);
                    tableRow.SetString(0, t);
                    tableTaskStatusCode.Add(tableRow);
                }
            }
            else
            {
                var tableRow = new SqlDataRecord(tableErrorSchema);
                tableTaskStatusCode.Add(tableRow);
            }
            #endregion

            #region WorkFlowId - Loại
            //Build your record
            var tableGuidSchema = new List<SqlMetaData>(1)
            {
                new SqlMetaData("Id", SqlDbType.UniqueIdentifier)
            }.ToArray();

            //And a table as a list of those records
            var tableWorkFlowId = new List<SqlDataRecord>();
            if (workFlowId != null && workFlowId.Count > 0)
            {
                foreach (var w in workFlowId)
                {
                    var wid = (Guid)w;
                    var tableRow = new SqlDataRecord(tableGuidSchema);
                    tableRow.SetSqlGuid(0, wid);
                    tableWorkFlowId.Add(tableRow);
                }
            }
            else
            {
                var tableRow = new SqlDataRecord(tableGuidSchema);
                tableWorkFlowId.Add(tableRow);
            }
            #endregion

            #region AssigneeList - NV được phân công
            //And a table as a list of those records
            var tableAssignee = new List<SqlDataRecord>();
            if (assigneeList != null && assigneeList.Count > 0)
            {
                foreach (var a in assigneeList)
                {
                    var tableRow = new SqlDataRecord(tableErrorSchema);
                    tableRow.SetString(0, a);
                    tableAssignee.Add(tableRow);
                }
            }
            else
            {
                var tableRow = new SqlDataRecord(tableErrorSchema);
                tableAssignee.Add(tableRow);
            }
            #endregion

            #region ProductCategoryCodeList - Nhóm vật tư
            //And a table as a list of those records
            var tableProductCategoryCode = new List<SqlDataRecord>();
            if (productCategoryCodeList != null && productCategoryCodeList.Count > 0)
            {
                foreach (var p in productCategoryCodeList)
                {
                    var tableRow = new SqlDataRecord(tableErrorSchema);
                    tableRow.SetString(0, p);
                    tableProductCategoryCode.Add(tableRow);
                }
            }
            else
            {
                var tableRow = new SqlDataRecord(tableErrorSchema);
                tableProductCategoryCode.Add(tableRow);
            }
            #endregion

            #region DepartmentCodeList - Phòng ban
            //And a table as a list of those records
            var tableDepartmentCode = new List<SqlDataRecord>();
            if (departmentCodeList != null && departmentCodeList.Count > 0)
            {
                foreach (var p in departmentCodeList)
                {
                    var tableRow = new SqlDataRecord(tableErrorSchema);
                    tableRow.SetString(0, p);
                    tableDepartmentCode.Add(tableRow);
                }
            }
            else
            {
                var tableRow = new SqlDataRecord(tableErrorSchema);
                tableDepartmentCode.Add(tableRow);
            }
            #endregion

            #region ServiceTechnicalTeamCode - Trung tâm bảo hành
            //Build your record
            var tableServiceTechnicalTeamCodeSchema = new List<SqlMetaData>(1)
                {
                    new SqlMetaData("Code", SqlDbType.NVarChar, 100)
                }.ToArray();

            //And a table as a list of those records
            var tableServiceTechnicalTeamCode = new List<SqlDataRecord>();
            List<string> serviceTechnicalTeamCodeLst = new List<string>();
            if (searchModel.ServiceTechnicalTeamCode != null && searchModel.ServiceTechnicalTeamCode.Count > 0)
            {
                foreach (var r in searchModel.ServiceTechnicalTeamCode)
                {
                    var tableRow = new SqlDataRecord(tableServiceTechnicalTeamCodeSchema);
                    tableRow.SetString(0, r);
                    if (!serviceTechnicalTeamCodeLst.Contains(r))
                    {
                        serviceTechnicalTeamCodeLst.Add(r);
                        tableServiceTechnicalTeamCode.Add(tableRow);
                    }
                }
            }
            else
            {
                var tableRow = new SqlDataRecord(tableServiceTechnicalTeamCodeSchema);
                tableServiceTechnicalTeamCode.Add(tableRow);
            }
            #endregion

            #region ProfileGroupCodeList - Nhóm KH
            //And a table as a list of those records
            var tableProfileGroupCode = new List<SqlDataRecord>();
            if (profileGroupCodeList != null && profileGroupCodeList.Count > 0)
            {
                foreach (var p in profileGroupCodeList)
                {
                    var tableRow = new SqlDataRecord(tableErrorSchema);
                    tableRow.SetString(0, p);
                    tableProfileGroupCode.Add(tableRow);
                }
            }
            else
            {
                var tableRow = new SqlDataRecord(tableErrorSchema);
                tableProfileGroupCode.Add(tableRow);
            }
            #endregion

            #region CategoryId - Nhóm VT
            //Build your record
            var tableCategorySchema = new List<SqlMetaData>(1)
            {
                new SqlMetaData("Id", SqlDbType.UniqueIdentifier)
            }.ToArray();

            //And a table as a list of those records
            var tableCategoryId = new List<SqlDataRecord>();
            if (searchModel.CategoryId != null && searchModel.CategoryId.Count > 0)
            {
                foreach (var w in searchModel.CategoryId)
                {
                    var wid = (Guid)w;
                    var tableRow = new SqlDataRecord(tableCategorySchema);
                    tableRow.SetSqlGuid(0, wid);
                    tableCategoryId.Add(tableRow);
                }
            }
            else
            {
                var tableRow = new SqlDataRecord(tableCategorySchema);
                tableCategoryId.Add(tableRow);
            }
            #endregion

        
            int FilteredResultsCount = 0;
            DataSet ds = new DataSet();
            string constr = ConfigurationManager.ConnectionStrings["cnStr"].ConnectionString;
            string stored = "[Task].[usp_SearchReportTask]";
            if (searchModel.Type == ConstWorkFlowCategory.TICKET)
            {
                stored = "[Task].[usp_SearchReportTask_Ticket]";
            }else if (searchModel.Type == ConstWorkFlowCategory.TICKET_MLC)
            {
                stored = "[Task].[usp_SearchReportTask_TICKET_MLC]";
            }
            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(stored, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 1800;
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        #region Parameters
                        //sda.SelectCommand.Parameters.AddWithValue("@ServiceTechnicalTeamCode", searchModel.ServiceTechnicalTeamCode);
                        var ServiceTechnicalTeamCode = sda.SelectCommand.Parameters.AddWithValue("@ServiceTechnicalTeamCode", tableServiceTechnicalTeamCode);
                        ServiceTechnicalTeamCode.SqlDbType = SqlDbType.Structured;
                        ServiceTechnicalTeamCode.TypeName = "[dbo].[StringList]";
                        sda.SelectCommand.Parameters.AddWithValue("@TaskCode", searchModel.TaskCode ?? (object)DBNull.Value);
                        sda.SelectCommand.Parameters.AddWithValue("@Summary", searchModel.Summary ?? (object)DBNull.Value);
                        var tablework = sda.SelectCommand.Parameters.AddWithValue("@WorkFlowId", tableWorkFlowId);
                        tablework.SqlDbType = SqlDbType.Structured;
                        tablework.TypeName = "[dbo].[GuidList]";
                        var tableStatus = sda.SelectCommand.Parameters.AddWithValue("@TaskStatusCodeList", tableTaskStatusCode);
                        tableStatus.SqlDbType = SqlDbType.Structured;
                        tableStatus.TypeName = "[dbo].[StringList]";
                        sda.SelectCommand.Parameters.AddWithValue("@TaskProcessCode", searchModel.TaskProcessCode ?? (object)DBNull.Value);
                        sda.SelectCommand.Parameters.AddWithValue("@Reporter", searchModel.Reporter ?? (object)DBNull.Value);
                        var tableAssig = sda.SelectCommand.Parameters.AddWithValue("@AssigneeList", tableAssignee);
                        tableAssig.SqlDbType = SqlDbType.Structured;
                        tableAssig.TypeName = "[dbo].[StringList]";
                        sda.SelectCommand.Parameters.AddWithValue("@ProfileId", searchModel.ProfileId ?? (object)DBNull.Value);
                        sda.SelectCommand.Parameters.AddWithValue("@CompanyId", searchModel.CompanyId ?? (object)DBNull.Value);
                        sda.SelectCommand.Parameters.AddWithValue("@CreateBy", searchModel.CreateBy ?? (object)DBNull.Value);
                        sda.SelectCommand.Parameters.AddWithValue("@PriorityCode", searchModel.PriorityCode ?? (object)DBNull.Value);
                        sda.SelectCommand.Parameters.AddWithValue("@CreatedFromDate", searchModel.CreatedFromDate ?? (object)DBNull.Value);
                        sda.SelectCommand.Parameters.AddWithValue("@CreatedToDate", searchModel.CreatedToDate ?? (object)DBNull.Value);
                        sda.SelectCommand.Parameters.AddWithValue("@ReceiveFromDate", searchModel.ReceiveFromDate ?? (object)DBNull.Value);
                        sda.SelectCommand.Parameters.AddWithValue("@ReceiveToDate", searchModel.ReceiveToDate ?? (object)DBNull.Value);
                        sda.SelectCommand.Parameters.AddWithValue("@StartFromDate", searchModel.StartFromDate ?? (object)DBNull.Value);
                        sda.SelectCommand.Parameters.AddWithValue("@StartToDate", searchModel.StartToDate ?? (object)DBNull.Value);
                        sda.SelectCommand.Parameters.AddWithValue("@EstimateEndFromDate", searchModel.EstimateEndFromDate ?? (object)DBNull.Value);
                        sda.SelectCommand.Parameters.AddWithValue("@EstimateEndToDate", searchModel.EstimateEndToDate ?? (object)DBNull.Value);
                        sda.SelectCommand.Parameters.AddWithValue("@EndFromDate", searchModel.EndFromDate ?? (object)DBNull.Value);
                        sda.SelectCommand.Parameters.AddWithValue("@EndToDate", searchModel.EndToDate ?? (object)DBNull.Value);
                        sda.SelectCommand.Parameters.AddWithValue("@KanbanId", searchModel.KanbanId ?? (object)DBNull.Value);
                        sda.SelectCommand.Parameters.AddWithValue("@Type", searchModel.Type ?? (object)DBNull.Value);
                        sda.SelectCommand.Parameters.AddWithValue("@CommonMistakeCode", searchModel.CommonMistakeCode ?? (object)DBNull.Value);
                        sda.SelectCommand.Parameters.AddWithValue("@ErrorCode", searchModel.ErrorCode ?? (object)DBNull.Value);
                        sda.SelectCommand.Parameters.AddWithValue("@ErrorTypeCode", searchModel.ErrorTypeCode ?? (object)DBNull.Value);
                        sda.SelectCommand.Parameters.AddWithValue("@ConstructionUnit", searchModel.ConstructionUnit ?? (object)DBNull.Value);
                        var table = sda.SelectCommand.Parameters.AddWithValue("@WorkFlowIdList", tableWorkFlow);
                        table.SqlDbType = SqlDbType.Structured;
                        table.TypeName = "[dbo].[WorkFlowIdList]";
                        sda.SelectCommand.Parameters.AddWithValue("@AccountId", AccountId ?? (object)DBNull.Value);
                        sda.SelectCommand.Parameters.AddWithValue("@TaskProcessCode_Todo", TaskProcessCode_Todo);
                        sda.SelectCommand.Parameters.AddWithValue("@TaskProcessCode_Processing", TaskProcessCode_Processing);
                        sda.SelectCommand.Parameters.AddWithValue("@TaskProcessCode_Incomplete", TaskProcessCode_Incomplete);
                        sda.SelectCommand.Parameters.AddWithValue("@TaskProcessCode_CompletedOnTime", TaskProcessCode_CompletedOnTime);
                        sda.SelectCommand.Parameters.AddWithValue("@TaskProcessCode_CompletedExpire", TaskProcessCode_CompletedExpire);
                        sda.SelectCommand.Parameters.AddWithValue("@TaskProcessCode_Expired", TaskProcessCode_Expired);
                        sda.SelectCommand.Parameters.AddWithValue("@DomainImageWorkFlow", DomainImageWorkFlow ?? (object)DBNull.Value);
                        var tableProCategory = sda.SelectCommand.Parameters.AddWithValue("@ProductCategoryCodeList", tableProductCategoryCode);
                        tableProCategory.SqlDbType = SqlDbType.Structured;
                        tableProCategory.TypeName = "[dbo].[StringList]";
                        var errorTable = sda.SelectCommand.Parameters.AddWithValue("@UsualErrorCodeList", tableError);
                        errorTable.SqlDbType = SqlDbType.Structured;
                        errorTable.TypeName = "[dbo].[StringList]";
                        var colorTable = sda.SelectCommand.Parameters.AddWithValue("@ProductColorCodeList", tableColor);
                        colorTable.SqlDbType = SqlDbType.Structured;
                        colorTable.TypeName = "[dbo].[StringList]";
                        var tbaleProfileGroup = sda.SelectCommand.Parameters.AddWithValue("@ProfileGroupCodeList", tableProfileGroupCode);
                        tbaleProfileGroup.SqlDbType = SqlDbType.Structured;
                        tbaleProfileGroup.TypeName = "[dbo].[StringList]";
                        sda.SelectCommand.Parameters.AddWithValue("@SalesSupervisorCode", searchModel.SalesSupervisorCode ?? (object)DBNull.Value);
                        var tableDepartment = sda.SelectCommand.Parameters.AddWithValue("@DepartmentCodeList", tableDepartmentCode);
                        tableDepartment.SqlDbType = SqlDbType.Structured;
                        tableDepartment.TypeName = "[dbo].[StringList]";
                        var tableVisitTypeCode = sda.SelectCommand.Parameters.AddWithValue("@VisitTypeCode", new UtilitiesRepository().ConvertTableFromListString(searchModel.VisitTypeCode));
                        tableVisitTypeCode.SqlDbType = SqlDbType.Structured;
                        tableVisitTypeCode.TypeName = "[dbo].[StringList]";
                        sda.SelectCommand.Parameters.AddWithValue("@CurrentCompanyCode", CurrentCompanyCode ?? (object)DBNull.Value);
                        sda.SelectCommand.Parameters.AddWithValue("@IsReport", true);
                        sda.SelectCommand.Parameters.AddWithValue("@Property5", searchModel.Property5 ?? (object)DBNull.Value);
                        sda.SelectCommand.Parameters.AddWithValue("@ERPProductCode", searchModel.ERPProductCode ?? (object)DBNull.Value);
                        sda.SelectCommand.Parameters.AddWithValue("@ERPAccessoryCode", searchModel.ERPAccessoryCode ?? (object)DBNull.Value);
                        sda.SelectCommand.Parameters.AddWithValue("@AccessoryTypeCode", searchModel.AccessoryTypeCode ?? (object)DBNull.Value);
                        sda.SelectCommand.Parameters.AddWithValue("@PageSize", searchModel.PageSize ?? (object)DBNull.Value);
                        sda.SelectCommand.Parameters.AddWithValue("@PageNumber", searchModel.PageNumber ?? (object)DBNull.Value);
                        var output = sda.SelectCommand.Parameters.AddWithValue("@FilteredResultsCount", FilteredResultsCount);
                        output.Direction = ParameterDirection.Output;

                        var tableCategory = sda.SelectCommand.Parameters.AddWithValue("@CategoryId", tableCategoryId);
                        tableCategory.SqlDbType = SqlDbType.Structured;
                        tableCategory.TypeName = "[dbo].[GuidList]";

                        var tableSaleOfficeCode = sda.SelectCommand.Parameters.AddWithValue("@SaleOfficeCode", new UtilitiesRepository().ConvertTableFromListString(searchModel.SaleOfficeCode));
                        tableSaleOfficeCode.SqlDbType = SqlDbType.Structured;
                        tableSaleOfficeCode.TypeName = "[dbo].[StringList]";

                        var tableProvinceId = sda.SelectCommand.Parameters.AddWithValue("@ProvinceId", new UtilitiesRepository().ConvertTableFromListGuid(searchModel.ProvinceId));
                        tableProvinceId.SqlDbType = SqlDbType.Structured;
                        tableProvinceId.TypeName = "[dbo].[GuidList]";

                        var tableDistrictId = sda.SelectCommand.Parameters.AddWithValue("@DistrictId", new UtilitiesRepository().ConvertTableFromListGuid(searchModel.DistrictId));
                        tableDistrictId.SqlDbType = SqlDbType.Structured;
                        tableDistrictId.TypeName = "[dbo].[GuidList]";

                        var tableWardId = sda.SelectCommand.Parameters.AddWithValue("@WardId", new UtilitiesRepository().ConvertTableFromListGuid(searchModel.WardId));
                        tableWardId.SqlDbType = SqlDbType.Structured;
                        tableWardId.TypeName = "[dbo].[GuidList]";
                        #endregion

                        sda.Fill(ds);
                        var dt = ds.Tables[0];

                        if (dt != null && dt.Rows.Count > 0)
                        {
                            foreach (DataRow item in dt.Rows)
                            {
                                TaskExcelViewModel model = new TaskExcelViewModel();
                                if (!string.IsNullOrEmpty(item["ProfileId"].ToString()))
                                {
                                    model.ProfileId = Guid.Parse(item["ProfileId"].ToString());
                                }
                                var Address = item["ProfileAddress"].ToString();
                                if (!string.IsNullOrEmpty(Address))
                                {
                                    if (Address.StartsWith(","))
                                    {
                                        Address = Address.Remove(0, 1).Trim();
                                    }
                                }
                                var Description = item["DetailSummary"].ToString();
                                if (!string.IsNullOrEmpty(Description))
                                {
                                    Description = HttpUtility.HtmlDecode(item["DetailSummary"].ToString());
                                }
                                #region Convert to list
                                if (!string.IsNullOrEmpty(item["ProfileCode"].ToString()))
                                {
                                    model.ProfileCode = Convert.ToInt32(item["ProfileCode"].ToString());
                                    model.TICKET_ProfileCode = Convert.ToInt32(item["ProfileCode"].ToString());
                                    model.GTB_ProfileCode = Convert.ToInt32(item["ProfileCode"].ToString());
                                }
                                //Thăm hỏi KH (THKH)
                                if (searchModel.Type == ConstWorkFlowCategory.THKH)
                                {
                                    model.THKH_VisitTypeName = item["VisitTypeName"].ToString();
                                }
                                //Bảo hành MLC (TICKET_MLC)
                                if (searchModel.Type == ConstWorkFlowCategory.TICKET_MLC)
                                {
                                    model.TICKET_MLC_STT = item["STT"].ToString();
                                    model.TICKET_MLC_WorkFlowName = item["WorkFlowName"].ToString();
                                    model.TaskStatusCode = item["TaskStatusCode"].ToString();
                                    model.TICKET_MLC_TaskStatusName = item["TaskStatusName"].ToString();
                                    model.TICKET_MLC_Summary = item["Summary"].ToString();
                                    model.TICKET_MLC_VisitAddress = item["VisitAddress"].ToString();
                                    model.TICKET_MLC_ServiceTechnicalTeam = item["ServiceTechnicalTeam"].ToString();
                                    model.TICKET_MLC_ReporterName = item["ReporterName"].ToString();
                                    model.TICKET_MLC_AssigneeName = item["AssigneeName"].ToString();
                                    model.TICKET_MLC_RoleInCharge = item["RoleInCharge"].ToString();
                                    model.TICKET_MLC_ProfileForeignCode = item["ProfileForeignCode"].ToString();
                                    model.TICKET_MLC_ProfileName = item["ProfileName"].ToString();
                                    model.TICKET_MLC_ProfileAddress = Address;
                                    model.TICKET_MLC_Phone = item["Phone"].ToString();
                                    model.TICKET_MLC_ProvinceName = item["ProvinceName"].ToString();
                                    model.TICKET_MLC_DistrictName = item["DistrictName"].ToString();
                                    model.TICKET_MLC_WardName = item["WardName"].ToString();
                                    model.TICKET_MLC_Email = item["Email"].ToString();
                                    model.TICKET_MLC_PersonInCharge = item["PersonInCharge"].ToString();
                                    model.TICKET_MLC_ERPProductCode = item["ERPProductCode"].ToString();
                                    model.TICKET_MLC_ProductName = item["ProductName"].ToString();
                                    model.TICKET_MLC_Description = Description;
                                    model.TICKET_MLC_CustomerReviews = item["CustomerReviews"].ToString();
                                    model.TICKET_MLC_Review = item["Review"].ToString();
                                    if (!string.IsNullOrEmpty(item["ReceiveDate"].ToString()))
                                    {
                                        model.TICKET_MLC_ReceiveDate = Convert.ToDateTime(item["ReceiveDate"].ToString());
                                    }
                                    if (!string.IsNullOrEmpty(item["StartDate"].ToString()))
                                    {
                                        model.TICKET_MLC_StartDate = Convert.ToDateTime(item["StartDate"].ToString());
                                    }
                                    if (!string.IsNullOrEmpty(item["EndDate"].ToString()))
                                    {
                                        model.TICKET_MLC_EndDate = Convert.ToDateTime(item["EndDate"].ToString());
                                    }
                                    model.TICKET_MLC_CreateByName = item["CreateByName"].ToString();
                                    if (!string.IsNullOrEmpty(item["CreateTime"].ToString()))
                                    {
                                        model.CreateTime = Convert.ToDateTime(item["CreateTime"].ToString());
                                    }
                                    model.TICKET_MLC_LastEditByName = item["LastEditByName"].ToString();
                                    if (!string.IsNullOrEmpty(item["LastEditTime"].ToString()))
                                    {
                                        model.TICKET_MLC_LastEditTime = Convert.ToDateTime(item["LastEditTime"].ToString());
                                    }
                                    if (!string.IsNullOrEmpty(item["Qty"].ToString()))
                                    {
                                        model.TICKET_MLC_Qty = Convert.ToInt32(item["Qty"].ToString());
                                    }
                                    model.TICKET_MLC_Unit = item["Unit"].ToString();
                                    model.TICKET_MLC_SerialNumber = item["SerialNumber"].ToString();
                                    model.TICKET_MLC_ProductCategoryName = item["ProductCategoryName"].ToString();
                                    model.TICKET_MLC_ErrorName = item["ErrorName"].ToString();
                                    model.TICKET_MLC_ERPAccessoryCode = item["ERPAccessoryCode"].ToString();
                                    model.TICKET_MLC_AccessoryName = item["AccessoryName"].ToString();
                                    model.TICKET_MLC_ErrorTypeName = item["ErrorTypeName"].ToString();
                                    if (!string.IsNullOrEmpty(item["AccessoryQty"].ToString()))
                                    {
                                        model.TICKET_MLC_AccessoryQty = Convert.ToInt32(item["AccessoryQty"].ToString());
                                    }
                                    model.TICKET_MLC_AccessoryCategoryName = item["AccessoryCategoryName"].ToString();
                                    model.TICKET_MLC_SurveyCreateByName = item["SurveyCreateByName"].ToString();
                                    if (!string.IsNullOrEmpty(item["SurveyCreateTime"].ToString()))
                                    {
                                        model.TICKET_MLC_SurveyCreateByTime = Convert.ToDateTime(item["SurveyCreateTime"].ToString());
                                    }
                                    model.TICKET_MLC_SurveyProductQuality = item["SurveyProductQuality"].ToString();
                                    model.TICKET_MLC_SurveyEmployeeProfessional = item["SurveyEmployeeProfessional"].ToString();
                                    model.TICKET_MLC_SurveyServiceBehaviorEmployees = item["SurveyServiceBehaviorEmployees"].ToString();
                                    model.TICKET_MLC_SurveyCustomerComments = item["SurveyCustomerComments"].ToString();
                                }
                                if (searchModel.Type == ConstWorkFlowCategory.THKH)
                                {
                                    if (!string.IsNullOrEmpty(item["StartDate"].ToString()))
                                    {
                                        model.StartDate = Convert.ToDateTime(item["StartDate"].ToString());
                                    }
                                    model.Summary = item["Summary"].ToString();
                                    model.Address = Address;
                                    model.Phone = item["Phone"].ToString();
                                    model.Email = item["Email"].ToString();
                                    model.TaskStatusCode = item["TaskStatusCode"].ToString();
                                    model.TaskStatusName = item["TaskStatusName"].ToString();
                                    model.WorkFlowName = item["WorkFlowName"].ToString();
                                    model.ProfileName = item["ProfileName"].ToString();
                                    model.ProfileShortName = item["ProfileShortName"].ToString();
                                    model.PersonInCharge = item["PersonInCharge"].ToString();
                                    model.RoleInCharge = item["RoleInCharge"].ToString();
                                    model.VisitAddress = item["VisitAddress"].ToString();
                                    model.SaleOfficeName = item["SaleOfficeName"].ToString();
                                    model.AssigneeCode = item["AssigneeCode"].ToString();
                                    model.AssigneeName = item["AssigneeName"].ToString();
                                    model.ReporterName = item["ReporterName"].ToString();
                                    model.Description = Description;
                                    if (!string.IsNullOrEmpty(item["ConstructionDate"].ToString()))
                                    {
                                        model.ConstructionDate = Convert.ToDateTime(item["ConstructionDate"].ToString());
                                    }
                                    model.CustomerReviews = item["CustomerReviews"].ToString();
                                    if (!string.IsNullOrEmpty(item["ReceiveDate"].ToString()))
                                    {
                                        model.ReceiveDate = Convert.ToDateTime(item["ReceiveDate"].ToString());
                                    }
                                    if (!string.IsNullOrEmpty(item["EstimateEndDate"].ToString()))
                                    {
                                        model.EstimateEndDate = Convert.ToDateTime(item["EstimateEndDate"].ToString());
                                    }
                                    if (!string.IsNullOrEmpty(item["CheckInTime"].ToString()))
                                    {
                                        model.CheckInTime = Convert.ToDateTime(item["CheckInTime"].ToString());
                                    }
                                    if (!string.IsNullOrEmpty(item["CheckOutTime"].ToString()))
                                    {
                                        model.CheckOutTime = Convert.ToDateTime(item["CheckOutTime"].ToString());
                                    }
                                    if (!string.IsNullOrEmpty(item["EndDate"].ToString()))
                                    {
                                        model.EndDate = Convert.ToDateTime(item["EndDate"].ToString());
                                    }
                                    model.OrderCode = item["OrderCode"].ToString();
                                    model.WarrantyValue = item["WarrantyValue"].ToString();
                                    model.ERPProductCode = item["ERPProductCode"].ToString();
                                    model.ProductName = item["ProductName"].ToString();
                                    if (!string.IsNullOrEmpty(item["Qty"].ToString()))
                                    {
                                        model.Qty = Convert.ToInt32(item["Qty"].ToString());
                                    }
                                    model.ProductName = item["ProductName"].ToString();
                                    model.ProductCategoryName = item["ProductCategoryName"].ToString();
                                    model.ProductColorCode = item["ProductColorCode"].ToString();
                                    model.UsualErrorName = item["UsualErrorName"].ToString();
                                    model.ErrorName = item["ErrorName"].ToString();
                                    model.ErrorTypeName = item["ErrorTypeName"].ToString();
                                    if (!string.IsNullOrEmpty(item["CreateTime"].ToString()))
                                    {
                                        model.CreateTime = Convert.ToDateTime(item["CreateTime"].ToString());
                                    }
                                    model.ServiceRating = item["ServiceRating"].ToString();
                                    model.ProductRating = item["ProductRating"].ToString();
                                    model.Review = item["Review"].ToString();
                                    model.ServiceTechnicalTeam = item["ServiceTechnicalTeam"].ToString();
                                    model.ERPAccessoryCode = item["ERPAccessoryCode"].ToString();
                                    model.AccessoryName = item["AccessoryName"].ToString();
                                    if (!string.IsNullOrEmpty(item["AccessoryQty"].ToString()))
                                    {
                                        model.AccessoryQty = Convert.ToInt32(item["AccessoryQty"].ToString());
                                    }
                                    model.AccessoryCategoryName = item["AccessoryCategoryName"].ToString();
                                    model.CreateByName = item["CreateByName"].ToString();
                                }
                                //Bảo hành ACC (TICKET)
                                else if (searchModel.Type == ConstWorkFlowCategory.TICKET)
                                {
                                    //Mã SAP
                                    model.TICKET_TaskCode = string.IsNullOrEmpty(item["SubtaskCode"].ToString()) ? item["TaskCode"].ToString() : item["SubtaskCode"].ToString();
                                    model.TICKET_ProfileForeignCode = item["ProfileForeignCode"].ToString();
                                    model.TaskId = Guid.Parse(item["TaskId"].ToString());
                                    if (!string.IsNullOrEmpty(item["ReceiveDate"].ToString()))
                                    {
                                        model.TICKET_ReceiveDate = Convert.ToDateTime(item["ReceiveDate"].ToString());
                                    }
                                    model.TICKET_TaskSourceName = item["TaskSourceName"].ToString();
                                    model.TICKET_CreateByName = item["CreateByName"].ToString();
                                    if (!string.IsNullOrEmpty(item["CreateTime"].ToString()))
                                    {
                                        model.CreateTime = Convert.ToDateTime(item["CreateTime"].ToString());
                                    }
                                    model.TICKET_ProfileName = item["ProfileName"].ToString();
                                    model.TICKET_ProfileShortName = item["ProfileShortName"].ToString();
                                    model.TICKET_Address = !string.IsNullOrEmpty(item["VisitAddress"].ToString()) ? item["VisitAddress"].ToString() : Address;
                                    if (!string.IsNullOrEmpty(item["ConstructionDate"].ToString()))
                                    {
                                        model.TICKET_ConstructionDate = Convert.ToDateTime(item["ConstructionDate"].ToString());
                                    }
                                    model.TICKET_ContactName = item["ContactName"].ToString();
                                    model.TICKET_Phone = item["Phone"].ToString();
                                    model.TICKET_Description = Description;
                                    model.TICKET_ConstructionUnit = item["ConstructionUnitCode"].ToString();
                                    model.TICKET_ConstructionUnitName = item["ConstructionUnitName"].ToString();
                                    model.TICKET_OrderCode = item["OrderCode"].ToString();
                                    //Số chứng từ
                                    model.TICKET_SAPSOProduct = item["SAPSOProduct"].ToString();
                                    if (!string.IsNullOrEmpty(item["StartDate"].ToString()))
                                    {
                                        model.TICKET_StartDate = Convert.ToDateTime(item["StartDate"].ToString());
                                    }
                                    if (!string.IsNullOrEmpty(item["EndDate"].ToString()))
                                    {
                                        model.TICKET_EndDate = Convert.ToDateTime(item["EndDate"].ToString());
                                    }
                                    //Mã nhân viên được phân công
                                    model.TICKET_AssigneeCode = item["AssigneeCode"].ToString();
                                    model.TICKET_AssigneeName = item["AssigneeName"].ToString();
                                    //Kết quả
                                    model.TICKET_CustomerReviews = item["CustomerReviews"].ToString();
                                    //Giá trị đơn hàng
                                    if (!string.IsNullOrEmpty(item["ProductValue"].ToString()))
                                    {
                                        model.TICKET_ProductValue = Convert.ToDecimal(item["ProductValue"].ToString());
                                    }
                                    //Giá trị bảo hành
                                    if (!string.IsNullOrEmpty(item["WarrantyValue"].ToString()))
                                    {
                                        model.TICKET_WarrantyValue = Convert.ToDecimal(item["WarrantyValue"].ToString());
                                    }
                                    //Nhóm vật tư
                                    model.TICKET_ProductCategoryName = item["ProductCategoryName"].ToString();
                                    if (!string.IsNullOrEmpty(item["ProductColorCode"].ToString()))
                                    {
                                        model.TICKET_ProductColorCode = item["ProductColorCode"].ToString();
                                    }
                                    else
                                    {
                                        model.TICKET_ProductColorCode = item["ProductName"].ToString();
                                    }
                                    //Danh sách lỗi
                                    model.TICKET_UsualErrorName = item["UsualErrorName"].ToString();
                                    //Đơn vị tính
                                    model.TICKET_Unit = item["Unit"].ToString();
                                    if (!string.IsNullOrEmpty(item["Qty"].ToString()))
                                    {
                                        model.TICKET_Qty = Convert.ToInt32(item["Qty"].ToString());
                                    }
                                    model.TICKET_PersonInCharge = item["PersonInCharge"].ToString();
                                    model.TICKET_TaskStatusName = item["TaskStatusName"].ToString();
                                    model.TICKET_WorkFlowName = item["WorkFlowName"].ToString();
                                    //Phân cấp sản phẩm
                                    model.TICKET_ProductLevelName = item["ProductLevelName"].ToString();
                                    //Hình thức bảo hành
                                    model.TICKET_ErrorTypeName = item["ErrorTypeName"].ToString();
                                    //Phương thức xử lý
                                    model.TICKET_ErrorName = item["ErrorName"].ToString();
                                    //Phụ kiện
                                    model.TICKET_AccessoryName = item["AccessoryName"].ToString();
                                    //Hình thức bảo hành Phụ kiện
                                    model.TICKET_AccErrorTypeName = item["AccErrorTypeName"].ToString();
                                    //Loại phụ kiện
                                    model.TICKET_AccessoryCategoryName = item["AccessoryCategoryName"].ToString();
                                    // Khu vực
                                    model.TICKET_SaleOfficeName = item["SaleOfficeName"].ToString();

                                    // Thông tin Khảo Sát
                                    model.TICKET_SurveyCreateByName = item["SurveyCreateByName"].ToString();
                                    if (!string.IsNullOrEmpty(item["SurveyCreateTime"].ToString()))
                                    {
                                        model.TICKET_SurveyCreateByTime = Convert.ToDateTime(item["SurveyCreateTime"].ToString());
                                    }
                                    model.TICKET_SurveyProductQuality = item["SurveyProductQuality"].ToString();
                                    model.TICKET_SurveyEmployeeProfessional = item["SurveyEmployeeProfessional"].ToString();
                                    model.TICKET_SurveyServiceBehaviorEmployees = item["SurveyServiceBehaviorEmployees"].ToString();
                                    model.TICKET_SurveyCustomerComments = item["SurveyCustomerComments"].ToString();
                                }
                                else if (searchModel.Type == ConstWorkFlowCategory.GTB)
                                {
                                    //Góc trưng bày
                                    model.GTB_TaskSummary = item["Summary"].ToString();
                                    //Loại
                                    model.GTB_TaskCode = item["TaskCode"].ToString();
                                    model.WorkFlowName = item["WorkFlowName"].ToString();
                                    //Tên ngắn
                                    model.GTB_ProfileShortName = string.IsNullOrEmpty(item["ProfileShortName"].ToString()) ? item["ProfileName"].ToString() : item["ProfileShortName"].ToString();
                                    //Trạng thái
                                    model.GTB_TaskStatusName = item["TaskStatusName"].ToString();
                                    //NV kinh doanh
                                    model.GTB_PersonInCharge = item["PersonInCharge"].ToString();
                                    //Phòng ban
                                    model.GTB_RoleInCharge = item["RoleInCharge"].ToString();
                                    model.GTB_VisitAddress = item["VisitAddress"].ToString();
                                    // Địa chỉ ĐTB
                                    model.DTB_VisitAddress = item["VisitAddress"].ToString();
                                    // Khu vực
                                    model.GTB_SaleOfficeName = item["SaleOfficeName"].ToString();
                                    // Tên liên hệ
                                    model.GTB_ContactName = item["ContactName"].ToString();
                                    // SĐT liên hệ
                                    model.GTB_ContactPhone = item["ContactPhone"].ToString();
                                    //Nhóm KH
                                    model.GTB_CustomerGroupName = item["CustomerGroupName"].ToString();
                                    // Quận/ Huyện
                                    model.GTB_DistrictName = item["DistrictName"].ToString();
                                    // Phường xã
                                    model.GTB_WardName = item["WardName"].ToString();
                                    // Mã SAP
                                    model.GTB_ProfileForeignCode = item["ProfileForeignCode"].ToString();
                                    // Tỉnh/thành phố
                                    model.GTB_ProvinceName = item["ProvinceName"].ToString();
                                    // Ngày chăm sóc gần nhất
                                    if (!string.IsNullOrEmpty(item["NearestDate_THKH"].ToString()))
                                    {
                                        model.GTB_NearestDate_THKH = Convert.ToDateTime(item["NearestDate_THKH"].ToString());
                                    }
                                    //Nhân viên chăm sóc
                                    model.GTB_AssigneeName_THKH = item["AssigneeName_THKH"].ToString();
                                    //Nội dung chăm sóc
                                    model.GTB_Description_THKH = item["Description_THKH"].ToString();
                                    //Ngày chăm sóc dự kiến
                                    if (!string.IsNullOrEmpty(item["RemindDate_THKH"].ToString()))
                                    {
                                        model.GTB_RemindDate_THKH = Convert.ToDateTime(item["RemindDate_THKH"].ToString());
                                    }
                                    //Giá trị GVL
                                    if (!string.IsNullOrEmpty(item["ValueOfShowroom"].ToString()))
                                    {
                                        model.GTB_ValueOfShowroom = Convert.ToDecimal(item["ValueOfShowroom"].ToString());
                                        model.DTB_ValueOfShowroom = Convert.ToDecimal(item["ValueOfShowroom"].ToString());
                                    }
                                    //Ngày lắp
                                    if (!string.IsNullOrEmpty(item["StartDate"].ToString()))
                                    {
                                        model.GTB_StartDate = Convert.ToDateTime(item["StartDate"].ToString());
                                    }
                                    //Số lượng
                                    if (!string.IsNullOrEmpty(item["QuantityCatalogueOfShowroom"].ToString()))
                                    {
                                        model.GTB_QuantityCatalogueOfShowroom = Convert.ToDecimal(item["QuantityCatalogueOfShowroom"].ToString());
                                    }

                                    //Ngày tạo
                                    if (!string.IsNullOrEmpty(item["CreateTime"].ToString()))
                                    {
                                        model.GTB_CreateTime = Convert.ToDateTime(item["CreateTime"].ToString());
                                    }

                                    //Người tạo
                                    model.GTB_CreateByName = item["CreateByName"].ToString();
                                }
                                else if (searchModel.Type == ConstWorkFlowCategory.ACTIVITIES)//Nhiệm vụ
                                {
                                    if (!string.IsNullOrEmpty(item["StartDate"].ToString()))
                                    {
                                        model.ACTI_StartDate = Convert.ToDateTime(item["StartDate"].ToString());
                                    }
                                    if (!string.IsNullOrEmpty(item["EndDate"].ToString()))
                                    {
                                        model.ACTI_StartDate = Convert.ToDateTime(item["EndDate"].ToString());
                                    }
                                    model.ACTI_Summary = item["Summary"].ToString();
                                    model.ACTI_TaskStatusName = item["TaskStatusName"].ToString();
                                    model.ACTI_WorkFlowName = item["WorkFlowName"].ToString();
                                    model.ACTI_Description = Description;
                                    model.ACTI_PersonInCharge = item["PersonInCharge"].ToString();
                                    model.ACTI_RoleInCharge = item["RoleInCharge"].ToString();
                                    if (!string.IsNullOrEmpty(item["ProfileCode"].ToString()))
                                    {
                                        model.ACTI_ProfileCode = Convert.ToInt32(item["ProfileCode"].ToString());
                                    }
                                    model.ACTI_ProfileForeignCode = item["ProfileForeignCode"].ToString();
                                    model.ACTI_ProfileName = item["ProfileName"].ToString();
                                    model.ACTI_ProfileShortName = item["ProfileShortName"].ToString();
                                    model.ACTI_Address = item["ProfileAddress"].ToString();
                                    model.ACTI_ContactName = item["ContactName"].ToString();
                                    model.ACTI_Phone = item["Phone"].ToString();
                                    model.ACTI_Email = item["Email"].ToString();
                                    if (!string.IsNullOrEmpty(item["ReceiveDate"].ToString()))
                                    {
                                        model.ACTI_ReceiveDate = Convert.ToDateTime(item["ReceiveDate"].ToString());
                                    }
                                    model.ACTI_ReporterName = item["ReporterName"].ToString();
                                    model.ACTI_AssigneeCode = item["AssigneeCode"].ToString();
                                    model.ACTI_AssigneeName = item["AssigneeName"].ToString();
                                    model.CreateByName = item["CreateByName"].ToString();
                                    if (!string.IsNullOrEmpty(item["CreateTime"].ToString()))
                                    {
                                        model.CreateTime = Convert.ToDateTime(item["CreateTime"].ToString());
                                    }
                                    model.ACTI_SaleOfficeName = item["SaleOfficeName"].ToString();
                                }
                                else if (searchModel.Type == ConstWorkFlowCategory.MISSION)
                                {
                                    model.MIS_TaskCode = item["TaskCode"].ToString();//1. Mã yêu cầu
                                    model.MIS_Summary = item["Summary"].ToString();//2. Tiêu Đề
                                    model.MIS_TaskStatusName = item["TaskStatusName"].ToString();//3. Trạng Thái
                                    model.MIS_PriorityName = item["PriorityName"].ToString();//4. Mức Độ
                                    model.MIS_Description = item["DetailSummary"].ToString();//5. Mô tả
                                    model.MIS_CreateByName = item["CreateByName"].ToString();//6. NV Giao việc
                                    model.MIS_ReporterName = item["ReporterName"].ToString();//7. NV theo dõi/giám sát
                                    model.MIS_AssigneeName = item["AssigneeName"].ToString();//8. NV được phân công
                                    model.MIS_AssigneePhone = item["AssigneePhone"].ToString();//SĐT NV được phân công
                                    if (!string.IsNullOrEmpty(item["StartDate"].ToString()))
                                    {
                                        model.MIS_StartDate = Convert.ToDateTime(item["StartDate"].ToString());//9. Ngày bắt đầu
                                    }
                                    if (!string.IsNullOrEmpty(item["EstimateEndDate"].ToString()))
                                    {
                                        model.MIS_EstimateEndDate = Convert.ToDateTime(item["EstimateEndDate"].ToString());//10. Ngày đến hạn
                                    }
                                    if (!string.IsNullOrEmpty(item["EndDate"].ToString()))
                                    {
                                        model.MIS_EndDate = Convert.ToDateTime(item["EndDate"].ToString());//11. Ngày kết thúc
                                    }


                                }

                                #endregion
                                result.Add(model);
                            }
                        }
                    }
                }
            }
            filteredResultsCount = FilteredResultsCount;
            return result;
        }

        /// <summary>
        /// Select list remind task in estimated calendar Task
        /// </summary>
        /// <param name="taskIdList"></param>
        /// <param name="StartFromDate"></param>
        /// <param name="StartToDate"></param>
        /// <returns></returns>
        public List<KanbanTaskViewModel> SearchTaskEstimatedCalendar(List<Guid> taskIdList, DateTime? StartFromDate, DateTime? StartToDate)
        {
            string sqlQuery = "EXEC [Task].[usp_SearchTaskEstimatedCalendar] @TaskId, @StartFromDate, @StartToDate";

            #region WorkflowList
            //Build your record
            var tableTaskIdSchema = new List<SqlMetaData>(1)
                {
                    new SqlMetaData("WorkFlowId", SqlDbType.UniqueIdentifier)
                }.ToArray();

            //And a table as a list of those records
            var tableTaskId = new List<SqlDataRecord>();
            List<string> saleOrgLst = new List<string>();
            if (taskIdList != null && taskIdList.Count > 0)
            {
                foreach (var r in taskIdList)
                {
                    var tableRow = new SqlDataRecord(tableTaskIdSchema);
                    tableRow.SetSqlGuid(0, r);
                    tableTaskId.Add(tableRow);
                }
            }
            else
            {
                var tableRow = new SqlDataRecord(tableTaskIdSchema);
                tableTaskId.Add(tableRow);
            }
            #endregion

            #region Parameters
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter
                {
                    SqlDbType = SqlDbType.Structured,
                    Direction = ParameterDirection.Input,
                    ParameterName = "TaskId",
                    TypeName = "[dbo].[WorkFlowIdList]", //Don't forget this one!
                    Value = tableTaskId
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.DateTime,
                    Direction = ParameterDirection.Input,
                    ParameterName = "StartFromDate",
                    Value = StartFromDate ?? (object)DBNull.Value
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.DateTime,
                    Direction = ParameterDirection.Input,
                    ParameterName = "StartToDate",
                    Value = StartToDate ?? (object)DBNull.Value
                }
            };
            #endregion

            var result = _context.Database.SqlQuery<KanbanTaskViewModel>(sqlQuery, parameters.ToArray()).ToList();
            return result;
        }

        /// <summary>
        /// Lấy danh sách subtask
        /// </summary>
        /// <param name="TaskId"></param>
        /// <returns></returns>
        public List<SubtaskViewModel> GetSubtaskList(Guid TaskId)
        {
            List<SubtaskViewModel> taskList = new List<SubtaskViewModel>();
            taskList = (from p in _context.TaskModel
                        join w in _context.WorkFlowModel on p.WorkFlowId equals w.WorkFlowId
                        join ts in _context.TaskStatusModel on p.TaskStatusId equals ts.TaskStatusId
                        join vp in _context.CatalogModel on new { CatalogTypeCode = ConstCatalogType.VisitPlace, CatalogCode = p.VisitPlace } equals new { CatalogTypeCode = vp.CatalogTypeCode, CatalogCode = vp.CatalogCode } into vpG
                        from visitPlace in vpG.DefaultIfEmpty()
                        where p.ParentTaskId == TaskId
                        select new SubtaskViewModel()
                        {
                            TaskId = p.TaskId,
                            TaskCode = p.TaskCode,
                            Summary = p.Summary,
                            PriorityCode = p.PriorityCode,
                            //WorkFlow
                            WorkFlowId = p.WorkFlowId,
                            WorkFlowCode = w.WorkFlowCode,
                            WorkFlowName = w.WorkFlowName,
                            //TaskStatus
                            TaskStatusId = p.TaskStatusId,
                            ProcessCode = ts.ProcessCode,
                            ProcessCodeIndex = ts.OrderIndex,
                            TaskStatusName = ts.TaskStatusName,
                            Actived = p.Actived,
                            SubtaskCode = p.SubtaskCode,
                            //Nơi tham quan
                            VisitPlace = visitPlace.CatalogText_vi,
                            //Thời gian tham quan
                            StartDate = p.StartDate,
                            EstimatedEndDate = p.EstimateEndDate,
                        }).ToList();

            if (taskList != null && taskList.Count > 0)
            {
                foreach (var item in taskList)
                {
                    if (!string.IsNullOrEmpty(item.Summary))
                    {
                        if (item.Summary.Count() > 65)
                        {
                            item.Summary = item.Summary.Substring(0, 65) + "...";
                        }
                    }
                    var taskAssign = (from p in _context.TaskAssignModel
                                      join s in _context.SalesEmployeeModel on p.SalesEmployeeCode equals s.SalesEmployeeCode
                                      where p.TaskId == item.TaskId
                                      select s.SalesEmployeeName).ToList();
                    if (taskAssign != null && taskAssign.Count > 0)
                    {
                        item.AssigneeName = string.Join(", ", taskAssign.ToArray());
                    }

                    //Dùng cho mobile
                    var taskAssignList = (from p in _context.TaskAssignModel
                                          join s in _context.SalesEmployeeModel on p.SalesEmployeeCode equals s.SalesEmployeeCode
                                          where p.TaskId == item.TaskId
                                          select new TaskAssignViewModel()
                                          {
                                              SalesEmployeeName = s.SalesEmployeeName,
                                          }).ToList();
                    if (taskAssignList != null && taskAssignList.Count > 0)
                    {
                        item.taskAssignList = new List<TaskAssignViewModel>();
                        foreach (var itemAssign in taskAssignList)
                        {
                            itemAssign.LogoName = itemAssign.SalesEmployeeName.GetCharacterForLogoName();
                            item.taskAssignList.Add(itemAssign);
                        }

                    }

                    //Thời gian thực tế KH tham quan
                    TaskRepository _taskRepo = new TaskRepository(_context);
                    var processingList = _taskRepo.GetTaskProcessingList(TaskId);
                    if (processingList != null && processingList.Count > 0)
                    {
                        var ThoiGianThucTeBatDauThamQuan = processingList.Where(p => p.ProcessCode == ConstProcess.processing).OrderBy(p => p.LastEditTime).Select(p => p.LastEditTime).FirstOrDefault();
                        if (ThoiGianThucTeBatDauThamQuan.HasValue)
                        {
                            item.ActualStartDate = ThoiGianThucTeBatDauThamQuan;
                        }
                        var ThoiGianThucTeKetThucThamQuan = processingList.Where(p => p.ProcessCode == ConstProcess.completed).OrderByDescending(p => p.LastEditTime).Select(p => p.LastEditTime).FirstOrDefault();
                        if (ThoiGianThucTeKetThucThamQuan.HasValue)
                        {
                            item.ActualEstimatedEndDate = ThoiGianThucTeKetThucThamQuan;
                        }
                    }
                }
            }
            return taskList;
        }

        /// <summary>
        /// Get Task theo TaskId
        /// </summary>
        /// <param name="taskId">Guid: TaskId</param>
        /// <returns>Task View Model</returns>
        #region Get task details
        public TaskViewModel GetTaskById(Guid taskId, string CompanyCode = null)
        {
            var taskInDb = (from p in _context.TaskModel
                            join w in _context.WorkFlowModel on p.WorkFlowId equals w.WorkFlowId
                            join ts in _context.TaskStatusModel on p.TaskStatusId equals ts.TaskStatusId
                            //Profile
                            join profile in _context.ProfileModel on p.ProfileId equals profile.ProfileId into profileTable
                            from pro in profileTable.DefaultIfEmpty()
                                //Province
                            join pr in _context.ProvinceModel on pro.ProvinceId equals pr.ProvinceId into prG
                            from province in prG.DefaultIfEmpty()
                                //District
                            join d in _context.DistrictModel on pro.DistrictId equals d.DistrictId into dG
                            from district in dG.DefaultIfEmpty()
                                //Ward
                            join wd in _context.WardModel on pro.WardId equals wd.WardId into wG
                            from ward in wG.DefaultIfEmpty()
                                //CreateBy
                            join create in _context.AccountModel on p.CreateBy equals create.AccountId
                            join cr in _context.SalesEmployeeModel on create.EmployeeCode equals cr.SalesEmployeeCode into crg
                            from cr1 in crg.DefaultIfEmpty()
                                //EditBy
                            join modify in _context.AccountModel on p.LastEditBy equals modify.AccountId into mg
                            from m in mg.DefaultIfEmpty()
                            join md in _context.SalesEmployeeModel on m.EmployeeCode equals md.SalesEmployeeCode into mdg
                            from md1 in mdg.DefaultIfEmpty()
                                //Report
                            join r in _context.AccountModel on p.Reporter equals r.EmployeeCode into rg
                            from rpt in rg.DefaultIfEmpty()
                                //Priority
                            join pr in _context.CatalogModel on p.PriorityCode equals pr.CatalogCode into prg
                            from priority in prg.DefaultIfEmpty()
                                //ConstructionUnitName
                            join cons in _context.ProfileModel on p.ConstructionUnit equals cons.ProfileId into consTable
                            from construction in consTable.DefaultIfEmpty()

                            let SaleSupervisor = (from pic in _context.PersonInChargeModel
                                                  join s in _context.SalesEmployeeModel on pic.SalesEmployeeCode equals s.SalesEmployeeCode
                                                  join acc in _context.AccountModel on s.SalesEmployeeCode equals acc.EmployeeCode
                                                  from r in acc.RolesModel
                                                  where pic.ProfileId == p.ProfileId && (CompanyCode == null || pic.CompanyCode == CompanyCode)
                                                  select new SalesSupervisorViewModel()
                                                  {
                                                      SalesSupervisorCode = pic.SalesEmployeeCode,
                                                      SalesSupervisorName = s.SalesEmployeeName,
                                                      DepartmentName = r.isEmployeeGroup == true ? r.RolesName : ""
                                                  }).FirstOrDefault()
                            where p.TaskId == taskId
                            select new TaskViewModel
                            {
                                TaskId = p.TaskId,
                                TaskCode = p.TaskCode,
                                Summary = p.Summary,
                                ProfileId = p.ProfileId,
                                ProfileName = pro.ProfileCode + " | " + pro.ProfileName,
                                ProfileAddress = pro.Address,
                                ProvinceName = province == null ? "" : ", " + province.ProvinceName,
                                DistrictName = district == null ? "" : ", " + district.Appellation + " " + district.DistrictName,
                                WardName = ward == null ? "" : ", " + ward.Appellation + " " + ward.WardName,
                                ProfileCode = pro.ProfileCode.ToString(),
                                //Phone = (pro.SAPPhone == "" || pro.SAPPhone == null) ? pro.Phone : pro.SAPPhone,
                                Phone = (pro.Phone == "" || pro.Phone == null) ? pro.SAPPhone : pro.Phone,
                                Email = pro.Email,
                                Text3 = pro.Text3,
                                Description = p.Description,
                                PriorityCode = p.PriorityCode,
                                PriorityName = priority.CatalogText_vi,
                                //WorkFlow
                                WorkFlowId = p.WorkFlowId,
                                WorkFlowCode = w.WorkFlowCode,
                                WorkFlowName = w.WorkFlowName,
                                WorkFlowImageUrl = w.ImageUrl,
                                //TaskStatus
                                TaskStatusId = p.TaskStatusId,
                                ProcessCode = ts.ProcessCode,
                                ProcessCodeIndex = ts.OrderIndex,
                                TaskStatusName = ts.TaskStatusName,
                                ReceiveDate = p.ReceiveDate,
                                StartDate = p.StartDate,
                                EstimateEndDate = p.EstimateEndDate,
                                EndDate = p.EndDate,
                                CompanyId = p.CompanyId,
                                StoreId = p.StoreId,
                                FileUrl = p.FileUrl,
                                CommonMistakeCode = p.CommonMistakeCode,
                                ConstructionUnit = p.ConstructionUnit,
                                ConstructionUnitContact = p.ConstructionUnitContact,
                                ConstructionUnitName = construction.ProfileCode + " | " + construction.ProfileName,
                                Actived = p.Actived,
                                Reporter = p.Reporter,
                                ProductWarrantyId = p.ProductWarrantyId,
                                CreateBy = p.CreateBy,
                                CreateTime = p.CreateTime,
                                CreateByName = cr1.SalesEmployeeCode + " | " + cr1.SalesEmployeeName,
                                CreateByFullName = cr1.SalesEmployeeName,
                                LastEditBy = p.LastEditBy,
                                LastEditTime = p.LastEditTime,
                                LastEditByName = md1 != null ? md1.SalesEmployeeCode + " | " + md1.SalesEmployeeName : "",
                                LastEditByFullName = md1.SalesEmployeeName,
                                ServiceTechnicalTeamCode = p.ServiceTechnicalTeamCode,
                                CustomerReviews = p.CustomerReviews,
                                ErrorTypeCode = p.ErrorTypeCode,
                                ErrorCode = p.ErrorCode,
                                VisitAddress = p.VisitAddress,
                                lat = p.lat,
                                lng = p.lng,
                                isRequiredCheckin = p.isRequiredCheckin,
                                VisitTypeCode = p.VisitTypeCode,
                                VisitSaleOfficeCode = p.VisitSaleOfficeCode,
                                //Mobile
                                tags = "(" + w.WorkFlowName.Trim() + ")" + (!string.IsNullOrEmpty(p.SubtaskCode) ? p.SubtaskCode : p.TaskCode.ToString()),
                                ReporterName = rpt.FullName,
                                //Riêng tư
                                isPrivate = p.isPrivate,
                                //Nhắc nhở
                                isRemind = p.isRemind,
                                RemindTime = p.RemindTime,
                                RemindCycle = p.RemindCycle,
                                isRemindForReporter = p.isRemindForReporter,
                                isRemindForAssignee = p.isRemindForAssignee,
                                RemindStartDate = p.RemindStartDate,
                                //Bảo hành
                                Property1 = p.Property1,
                                Property2 = p.Property2,
                                Property3 = p.Property3,
                                Property4 = p.Property4,
                                Property5 = p.Property5,
                                //Ngày
                                Date1 = p.Date1,
                                Date2 = p.Date2,
                                Date3 = p.Date3,
                                Date4 = p.Date4,
                                Date5 = p.Date5,
                                //Text
                                Text1 = p.Text1,
                                Text2 = p.Text2,
                                //Text3 = p.Text3,
                                Text4 = p.Text4,
                                Text5 = p.Text5,

                                ParentTaskId = p.ParentTaskId,
                                SalesSupervisorCode = p.SalesSupervisorCode,
                                SalesSupervisorName = SaleSupervisor.SalesSupervisorName,
                                DepartmentName = SaleSupervisor.DepartmentName,
                                TaskSourceCode = p.Property4,
                                IsAssignGroup = p.IsAssignGroup,
                                //ĐTB
                                Property6 = p.Property6,
                                SubtaskCode = p.SubtaskCode,
                                //Nơi tham quan
                                VisitPlace = p.VisitPlace,
                                //Bảo hành_kết quả
                                Result = p.CancelReason,
                                //Tỉnh/Thành phố
                                ProvinceId = p.ProvinceId,
                                DistrictId = p.DistrictId,
                                WardId = p.WardId,
                                //Cấu hình Phân công: Loại công việc
                                IsTogether = p.IsTogether
                            }).FirstOrDefault();

            if (taskInDb != null)
            {
                //Get for edit
                if (taskInDb.WorkFlowCode == ConstWorkFlow.GT)
                {
                    AppointmentRepository _appointmentRepository = new AppointmentRepository(_context);
                    var appointmentInDb = _appointmentRepository.GetById(taskInDb.TaskId);
                    if (appointmentInDb != null)
                    {
                        taskInDb.CustomerClassCode = appointmentInDb.CustomerClassCode;
                    }
                }
                //Address
                taskInDb.ProfileAddress = string.Format("{0}{1}{2}{3}", taskInDb.ProfileAddress, taskInDb.WardName, taskInDb.DistrictName, taskInDb.ProvinceName);

                //Assignee
                //NV được phân công
                taskInDb.taskAssignList = (from p in _context.TaskAssignModel
                                           join s in _context.SalesEmployeeModel on p.SalesEmployeeCode equals s.SalesEmployeeCode into sg
                                           from emp in sg.DefaultIfEmpty()
                                           join c in _context.CatalogModel on new { TaskAssignTypeCode = p.TaskAssignTypeCode, Type = ConstCatalogType.TaskAssignType } equals new { TaskAssignTypeCode = c.CatalogCode, Type = c.CatalogTypeCode } into taskAssignTypeTemp
                                           from taskAssignType in taskAssignTypeTemp.DefaultIfEmpty()
                                           join r in _context.RolesModel on p.RolesCode equals r.RolesCode into rTemp
                                           from role in rTemp.DefaultIfEmpty()
                                           where p.TaskId == taskInDb.TaskId
                                           orderby p.CreateTime
                                           select new TaskAssignViewModel()
                                           {
                                               TaskAssignId = p.TaskAssignId,
                                               SalesEmployeeCode = p.SalesEmployeeCode,
                                               TaskAssignTypeCode = p.TaskAssignTypeCode,
                                               RolesCode = p.RolesCode,
                                               RolesName = role.RolesName,
                                               TaskAssignTypeName = taskAssignType.CatalogText_vi,
                                               SalesEmployeeName = emp.SalesEmployeeName,
                                           }).ToList();
                //Phòng ban theo NV được phân công
                if (taskInDb.taskAssignList != null && taskInDb.taskAssignList.Count > 0)
                {
                    if (taskInDb.IsAssignGroup != true || taskInDb.IsTogether == true)
                    {
                        foreach (var item in taskInDb.taskAssignList)
                        {
                            var role = (from acc in _context.AccountModel
                                        from r in acc.RolesModel
                                        where acc.EmployeeCode == item.SalesEmployeeCode
                                        && r.isEmployeeGroup == true
                                        select r.RolesName).FirstOrDefault();
                            item.RoleName = role != null ? role : "";
                        }
                    }
                }
                ////Phân công nhóm - làm chung
                //if (taskInDb.IsTogether == true && taskInDb.IsAssignGroup == true)
                //{
                //    taskInDb.taskAssignGroupList = taskInDb.taskAssignList;
                //}

                //Người theo dõi/giám sát
                taskInDb.taskReporterList = (from p in _context.TaskReporterModel
                                             join s in _context.SalesEmployeeModel on p.SalesEmployeeCode equals s.SalesEmployeeCode into sg
                                             from emp in sg.DefaultIfEmpty()
                                             join c in _context.CatalogModel on new { TaskAssignTypeCode = p.TaskAssignTypeCode, Type = ConstCatalogType.TaskReporterType } equals new { TaskAssignTypeCode = c.CatalogCode, Type = c.CatalogTypeCode } into taskAssignTypeTemp
                                             from taskAssignType in taskAssignTypeTemp.DefaultIfEmpty()
                                             where p.TaskId == taskInDb.TaskId
                                             orderby p.CreateTime
                                             select new TaskReporterViewModel()
                                             {
                                                 TaskReporterId = p.TaskReporterId,
                                                 SalesEmployeeCode = p.SalesEmployeeCode,
                                                 SalesEmployeeName = emp.SalesEmployeeName,
                                                 TaskAssignTypeCode = p.TaskAssignTypeCode,
                                                 TaskAssignTypeName = taskAssignType.CatalogText_vi,
                                             }).ToList();
                //Contact
                //Liên hệ
                taskInDb.taskContact = (from p in _context.TaskContactModel
                                        join s in _context.ProfileModel on p.ContactId equals s.ProfileId
                                        where p.TaskId == taskInDb.TaskId
                                        && p.isMain == true
                                        select new TaskContactViewModel()
                                        {
                                            ContactId = p.ContactId,
                                            ContactName = s.ProfileName
                                        }).FirstOrDefault();

                if (taskInDb.taskContact != null)
                {
                    taskInDb.ContactId = taskInDb.taskContact.ContactId;
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
                                   where p.ProfileId == taskInDb.taskContact.ContactId
                                   select new
                                   {
                                       ProfileAddress = p.Address,
                                       ContactName = p.ProfileCode + " | " + p.ProfileName,
                                       ProvinceName = province == null ? "" : ", " + province.ProvinceName,
                                       DistrictName = district == null ? "" : ", " + district.Appellation + " " + district.DistrictName,
                                       WardName = ward == null ? "" : ", " + ward.Appellation + " " + ward.WardName,
                                       Phone = p.Phone,
                                       Email = p.Email
                                   }).FirstOrDefault();

                    //Hiển thị thông tin khách hàng theo liên hệ (nếu có)
                    if (contact != null)
                    {
                        taskInDb.ContactAddress = string.Format("{0}{1}{2}{3}", contact.ProfileAddress, contact.WardName, contact.DistrictName, contact.ProvinceName);
                        taskInDb.ContactName = contact.ContactName;
                        taskInDb.Phone = contact.Phone;
                        taskInDb.Email = contact.Email;
                    }
                }
                //Product Warranty
                //Phiếu bảo hành
                if (taskInDb.ProductWarrantyId != null)
                {
                    ProductWarrantyRepository _repo = new ProductWarrantyRepository(_context);
                    taskInDb.productWarranty = _repo.GetTaskProductWarrantyById((Guid)taskInDb.ProductWarrantyId);
                }

                //Product
                //Sản phẩm
                var productList = GetTaskProductList(taskId);
                taskInDb.taskProductList = productList;

                //Comment
                var commentList = GetTaskCommentList(taskId);
                taskInDb.taskCommentList = commentList;
                taskInDb.NumberOfComments = commentList.Count;
                //File Attachment
                var taskFileList = GetTaskFileList(commentList, taskId);
                taskInDb.taskFileList = taskFileList;
                taskInDb.NumberOfFiles = taskFileList.Count;

                //Task check in/out
                var checkInOut = _context.CheckInOutModel.Where(p => p.TaskId == taskId).FirstOrDefault();
                if (checkInOut != null)
                {
                    taskInDb.CheckInOutId = checkInOut.CheckInOutId;
                    taskInDb.CheckInBy = checkInOut.CheckInBy;
                    taskInDb.CheckOutBy = checkInOut.CheckOutBy;
                    taskInDb.CheckInLat = checkInOut.CheckInLat;
                    taskInDb.CheckInLng = checkInOut.CheckInLng;
                    taskInDb.CheckOutLat = checkInOut.CheckOutLat;
                    taskInDb.CheckOutLng = checkInOut.CheckOutLng;
                    taskInDb.CheckInTime = checkInOut.CheckInTime;
                    taskInDb.CheckOutTime = checkInOut.CheckOutTime;

                    taskInDb.CheckInByName = (from p in _context.AccountModel
                                              join s in _context.SalesEmployeeModel on p.EmployeeCode equals s.SalesEmployeeCode
                                              where p.AccountId == taskInDb.CheckInBy
                                              select s.SalesEmployeeCode + " | " + s.SalesEmployeeName).FirstOrDefault();
                    taskInDb.CheckOutByName = (from p in _context.AccountModel
                                               join s in _context.SalesEmployeeModel on p.EmployeeCode equals s.SalesEmployeeCode
                                               where p.AccountId == taskInDb.CheckOutBy
                                               select s.SalesEmployeeCode + " | " + s.SalesEmployeeName).FirstOrDefault();
                }
                //Subtask
                taskInDb.subtaskList = GetSubtaskList(taskId);
                if (taskInDb.subtaskList != null && taskInDb.subtaskList.Count > 0)
                {
                    var processCodeList = (from p in taskInDb.subtaskList
                                           join ts in _context.TaskStatusModel on p.TaskStatusId equals ts.TaskStatusId
                                           select ts.ProcessCode.ToLower()).ToList();

                    taskInDb.TodoSubtask = processCodeList.Where(p => p == ConstProcess.todo).Count();
                    taskInDb.ProcessingSubtask = processCodeList.Where(p => p == ConstProcess.processing).Count();
                    taskInDb.CompletedSubtask = processCodeList.Where(p => p == ConstProcess.completed).Count();
                }

                #region Survey
                taskInDb.surveyList = GetSurveyByTask(taskId,taskInDb.WorkFlowId);
                #endregion
            }
            return taskInDb;
        }
        #endregion


        /// <summary>
        ///Survey => Cấu hình hiển thị trong "Cấu hình nhiệm vụ" => Chọn hiển thị "tab_survey"
        ///Chưa có chức năng gắn workFlowId <==> Survey => Phải lưu tay trong DB, [Task].[Survey_Mapping]
        ///Tạo câu hỏi trong tMasterData.SurveyModel , Câu trả lời trong tMasterData.SurveyDetailModel
        ///Mapping Survey với CustomReferences(Hiện tại chỉ dùng cho WorkFlowId)
        /// </summary>
        /// <param name="TaskId"></param>
        /// <param name="WorkFlowId"></param>
        /// <returns></returns>
        public List<SurveyViewModel> GetSurveyByTask(Guid? TaskId, Guid? WorkFlowId)
        {
            var surveyViewModel = new List<SurveyViewModel>();
            //Lấy survey theo workflowId
            surveyViewModel = (from p in _context.Survey_Mapping
                               //Survey
                               join q in _context.SurveyModel on p.SurveyId equals q.SurveyId
                               where p.CustomReferences == WorkFlowId
                               select new SurveyViewModel()
                               {
                                   SurveyId = p.SurveyId,
                                   Question = q.Question,
                                   Type = q.Type,
                                   OrderIndex = p.OrderIndex
                               }).OrderBy(x => x.OrderIndex).ToList();
            if (surveyViewModel != null && surveyViewModel.Count() > 0)
            {
                var survey = (from p in surveyViewModel
                                   join s in _context.TaskSurveyModel on p.SurveyId equals s.SurveyId
                                   join aTemp in _context.AccountModel on s.CreateBy equals aTemp.AccountId into aList
                                   from a in aList.DefaultIfEmpty()
                                   where s.TaskId == TaskId
                                   select new SurveyViewModel()
                                   {
                                       TaskSurveyId = s.TaskSurveyId,
                                       SurveyId = p.SurveyId,
                                       Question = p.Question,
                                       OrderIndex = p.OrderIndex,
                                       Type = p.Type,
                                       CreateByCode = a?.EmployeeCode,
                                       CreateTime = s.CreateTime,
                                   }).OrderBy(x => x.OrderIndex).ToList();
                if (survey != null && survey.Count() > 0)
                {
                    surveyViewModel = survey;
                }
                foreach (var item in surveyViewModel)
                {
                    item.SurveyDetail = _context.SurveyDetailModel.Where(x => x.SurveyId == item.SurveyId)
                                                                  .Select(x => new SurveyDetailViewModel
                                                                  {
                                                                      SurveyId = item.SurveyId,
                                                                      SurveyDetailId = x.SurveyDetailId,
                                                                      AnswerDatetime = x.AnswerDatetime,
                                                                      AnswerText = x.AnswerText,
                                                                      AnswerValue = x.AnswerValue,
                                                                      OrderIndex = x.OrderIndex
                                                                  }).OrderBy(x => x.OrderIndex).ToList();
                    item.SurveyDetailSelected = (from p in _context.TaskSurveyAnswerModel
                                                     //Survey
                                                 join q in _context.SurveyDetailModel on p.SurveyDetailId equals q.SurveyDetailId
                                                 where p.TaskSurveyId == item.TaskSurveyId
                                                 select new SurveyDetailViewModel
                                                 {
                                                     SurveyId = q.SurveyId,
                                                     SurveyDetailId = p.SurveyDetailId,
                                                     AnswerDatetime = p.AnswerDatetime,
                                                     AnswerText = p.AnswerText,
                                                     AnswerValue = p.AnswerValue,
                                                     OrderIndex = q.OrderIndex
                                                 }).OrderBy(x => x.OrderIndex).ToList();
                }
            }
            return surveyViewModel;
        }


        /// <summary>
        /// Lấy thông tin task/appointment hiển thị chi tiết
        /// </summary>
        /// <param name="TaskId"></param>
        /// <returns>TaskViewModel</returns>
        #region Get task details include Appointment (GT)
        public TaskViewModel GetTaskInfo(Guid TaskId, string CompanyCode = null)
        {
            //Type
            var type = GetWorkflowCategoryCode(TaskId);
            //WorkFlowCode
            var WorkFlowCode = GetWorkflowCode(TaskId);

            TaskViewModel task = new TaskViewModel();
            if (WorkFlowCode != ConstWorkFlow.GT)
            {
                task = GetTaskById(TaskId, CompanyCode);
                if (task == null)
                {
                    return null;
                }
            }
            else
            {
                AppointmentRepository _appointmentRepository = new AppointmentRepository(_context);
                var appointmentInDb = _appointmentRepository.GetById(TaskId);
                if (appointmentInDb == null)
                {
                    return null;
                }
                //Convert to task
                task.TaskId = appointmentInDb.TaskId;
                task.TaskCode = appointmentInDb.TaskCode;
                task.Summary = appointmentInDb.Summary;
                task.Requirement = appointmentInDb.Requirement;
                task.PriorityCode = appointmentInDb.PriorityCode;
                task.WorkFlowName = appointmentInDb.WorkFlowName;
                task.ProcessCode = appointmentInDb.ProcessCode;
                task.Description = appointmentInDb.Description;
                task.WorkFlowId = appointmentInDb.WorkFlowId;
                task.WorkFlowCode = appointmentInDb.WorkFlowCode;
                task.TaskStatusId = appointmentInDb.TaskStatusId;
                task.StoreId = appointmentInDb.StoreId;
                task.ProfileId = appointmentInDb.ProfileId;
                task.ProfileName = appointmentInDb.ProfileName;
                task.ProfileAddress = appointmentInDb.ProfileAddress;
                task.DistrictName = appointmentInDb.DistrictName;
                task.ProvinceName = appointmentInDb.ProvinceName;
                task.Phone = appointmentInDb.Phone;
                task.Email = appointmentInDb.Email;
                task.Reporter = appointmentInDb.Reporter;
                task.taskContactList = appointmentInDb.taskContactList;
                task.taskContact = appointmentInDb.taskContact;
                task.ContactId = appointmentInDb.taskContact != null ? appointmentInDb.taskContact.ContactId : null;
                task.ReceiveDate = appointmentInDb.ReceiveDate;
                task.StartDate = appointmentInDb.StartDate;
                task.EstimateEndDate = appointmentInDb.EstimateEndDate;
                task.EndDate = appointmentInDb.EndDate;
                //Create and Modify info
                task.CreateBy = appointmentInDb.CreateBy;
                task.CreateByName = appointmentInDb.CreateByName;
                task.CreateByFullName = appointmentInDb.CreateByFullName;
                task.CreateTime = appointmentInDb.CreateTime;
                task.LastEditBy = appointmentInDb.LastEditBy;
                task.LastEditByName = appointmentInDb.LastEditByName;
                task.LastEditByFullName = appointmentInDb.LastEditByFullName;
                task.LastEditTime = appointmentInDb.LastEditTime;
                //Assignee
                task.taskAssignList = appointmentInDb.taskAssignList;
                //Appointment
                task.CustomerClassCode = appointmentInDb.CustomerClassCode;
                task.CategoryCode = appointmentInDb.CategoryCode;
                task.ShowroomCode = appointmentInDb.ShowroomCode;
                task.SaleEmployeeCode = appointmentInDb.SaleEmployeeCode;
                task.ChannelCode = appointmentInDb.ChannelCode;
                task.VisitDate = appointmentInDb.VisitDate;
                //Comment
                task.taskCommentList = appointmentInDb.taskCommentList;
                task.NumberOfComments = appointmentInDb.taskCommentList.Count;
                //File Attachment
                task.taskFileList = appointmentInDb.taskFileList;
                task.NumberOfFiles = appointmentInDb.taskFileList.Count;
                //Đề xuất
                task.SaleEmployeeOffer = appointmentInDb.SaleEmployeeOffer;
                //Nhận xét
                task.Reviews = appointmentInDb.Reviews;
                //Xếp hạng
                task.Ratings = appointmentInDb.Ratings;
                //Có ghé thăm cabinet pro
                task.isVisitCabinetPro = appointmentInDb.isVisitCabinetPro;

                //Nhu cầu KH khi ghé thăm cabinet pro
                task.VisitCabinetProRequest = appointmentInDb.VisitCabinetProRequest;

                    //Survey
                task.surveyList = appointmentInDb.surveyList;
                var profileB = _context.ProfileBAttributeModel.Where(p => p.ProfileId == task.ProfileId).FirstOrDefault();
                if (profileB != null)
                {
                    task.ContactName = profileB.ContactName;
                }

                #region //Mobile
                var workflow = _context.WorkFlowModel.Where(p => p.WorkFlowId == task.WorkFlowId).FirstOrDefault();
                if (workflow != null)
                {
                    task.tags = "(" + workflow.WorkFlowName.Trim() + ")" + task.TaskCode;
                }
                #endregion //Mobile
            }
            if (task.TaskId == null || task.TaskId == Guid.Empty)
            {
                return null;
            }
            task.Type = type;
            return task;
        }
        #endregion

        /// <summary>
        /// Lấy thông tin subtask
        /// </summary>
        /// <param name="TaskId"></param>
        /// <returns></returns>
        #region Get Subtask Info
        public TaskViewModel GetSubtaskInfo(Guid? TaskId)
        {
            var taskInDb = (from p in _context.TaskModel
                            join profile in _context.ProfileModel on p.ProfileId equals profile.ProfileId into profileTable
                            from pro in profileTable.DefaultIfEmpty()
                                //Province
                            join pr in _context.ProvinceModel on pro.ProvinceId equals pr.ProvinceId into prG
                            from province in prG.DefaultIfEmpty()
                                //District
                            join d in _context.DistrictModel on pro.DistrictId equals d.DistrictId into dG
                            from district in dG.DefaultIfEmpty()
                                //Ward
                            join wd in _context.WardModel on pro.WardId equals wd.WardId into wG
                            from ward in wG.DefaultIfEmpty()
                                //Report
                            join r in _context.AccountModel on p.Reporter equals r.EmployeeCode into rg
                            from rpt in rg.DefaultIfEmpty()
                                //Priority
                            join pr in _context.CatalogModel on p.PriorityCode equals pr.CatalogCode into prg
                            from priority in prg.DefaultIfEmpty()
                                //ConstructionUnitName
                            join cons in _context.ProfileModel on p.ConstructionUnit equals cons.ProfileId into consTable
                            from construction in consTable.DefaultIfEmpty()

                            let SaleSupervisor = (from pic in _context.PersonInChargeModel
                                                  join s in _context.SalesEmployeeModel on pic.SalesEmployeeCode equals s.SalesEmployeeCode
                                                  join acc in _context.AccountModel on s.SalesEmployeeCode equals acc.EmployeeCode
                                                  from r in acc.RolesModel
                                                  where pic.ProfileId == p.ProfileId
                                                  select new SalesSupervisorViewModel()
                                                  {
                                                      SalesSupervisorCode = pic.SalesEmployeeCode,
                                                      SalesSupervisorName = s.SalesEmployeeName,
                                                      DepartmentName = r.isEmployeeGroup == true ? r.RolesName : ""
                                                  }).FirstOrDefault()

                            where p.TaskId == TaskId
                            select new TaskViewModel
                            {
                                TaskId = p.TaskId,
                                TaskCode = p.TaskCode,
                                Summary = p.Summary,
                                ProfileId = p.ProfileId,
                                ProfileName = pro.ProfileName,
                                ProfileAddress = pro.Address,
                                ProvinceName = province == null ? "" : ", " + province.ProvinceName,
                                DistrictName = district == null ? "" : ", " + district.Appellation + " " + district.DistrictName,
                                WardName = ward == null ? "" : ", " + ward.Appellation + " " + ward.WardName,
                                Phone = (pro.SAPPhone == "" || pro.SAPPhone == null) ? pro.Phone : pro.SAPPhone,
                                Email = pro.Email,
                                Description = p.Description,
                                PriorityCode = p.PriorityCode,
                                PriorityName = priority.CatalogText_vi,
                                ReceiveDate = p.ReceiveDate,
                                StartDate = p.StartDate,
                                EstimateEndDate = p.EstimateEndDate,
                                EndDate = p.EndDate,
                                CompanyId = p.CompanyId,
                                StoreId = p.StoreId,
                                FileUrl = p.FileUrl,
                                CommonMistakeCode = p.CommonMistakeCode,
                                ConstructionUnit = p.ConstructionUnit,
                                Actived = p.Actived,
                                Reporter = p.Reporter,
                                ProductWarrantyId = p.ProductWarrantyId,
                                ServiceTechnicalTeamCode = p.ServiceTechnicalTeamCode,
                                CustomerReviews = p.CustomerReviews,
                                ErrorTypeCode = p.ErrorTypeCode,
                                ErrorCode = p.ErrorCode,
                                VisitAddress = p.VisitAddress,
                                lat = p.lat,
                                lng = p.lng,
                                isRequiredCheckin = p.isRequiredCheckin,
                                VisitTypeCode = p.VisitTypeCode,
                                ReporterName = rpt.FullName,
                                //Riêng tư
                                isPrivate = p.isPrivate,
                                //Bảo hành
                                Property1 = p.Property1,
                                Property2 = p.Property2,
                                Property3 = p.Property3,
                                Property4 = p.Property4,
                                Property5 = p.Property5,
                                //Ngày
                                Date1 = p.Date1,
                                Date2 = p.Date2,
                                Date3 = p.Date3,
                                Date4 = p.Date4,
                                Date5 = p.Date5,
                                //Text
                                Text1 = p.Text1,
                                Text2 = p.Text2,
                                Text3 = p.Text3,
                                Text4 = p.Text4,
                                Text5 = p.Text5,
                                ParentTaskId = TaskId,
                                //lấy WorkFlowId để lấy Type mới tạo subtask, khi lấy được Type sẽ change WorkFlowId
                                WorkFlowId = p.WorkFlowId,
                                SalesSupervisorCode = p.SalesSupervisorCode,
                                SalesSupervisorName = SaleSupervisor.SalesSupervisorName,
                                DepartmentName = SaleSupervisor.DepartmentName,
                                TaskSourceCode = p.Property4,
                                IsAssignGroup = p.IsAssignGroup
                            }).FirstOrDefault();

            if (taskInDb != null)
            {
                //Address
                taskInDb.ProfileAddress = string.Format("{0}{1}{2}{3}", taskInDb.ProfileAddress, taskInDb.WardName, taskInDb.DistrictName, taskInDb.ProvinceName);

                //Assignee
                //NV được phân công
                taskInDb.taskAssignList = (from p in _context.TaskAssignModel
                                           join s in _context.SalesEmployeeModel on p.SalesEmployeeCode equals s.SalesEmployeeCode into sg
                                           from emp in sg.DefaultIfEmpty()
                                           join c in _context.CatalogModel on new { TaskAssignTypeCode = p.TaskAssignTypeCode, Type = ConstCatalogType.TaskAssignType } equals new { TaskAssignTypeCode = c.CatalogCode, Type = c.CatalogTypeCode } into taskAssignTypeTemp
                                           from taskAssignType in taskAssignTypeTemp.DefaultIfEmpty()
                                           where p.TaskId == taskInDb.TaskId
                                           orderby p.CreateTime
                                           select new TaskAssignViewModel()
                                           {
                                               TaskAssignId = p.TaskAssignId,
                                               SalesEmployeeCode = p.SalesEmployeeCode,
                                               TaskAssignTypeCode = p.TaskAssignTypeCode,
                                               RolesCode = p.RolesCode,
                                               TaskAssignTypeName = taskAssignType.CatalogText_vi,
                                               SalesEmployeeName = emp.SalesEmployeeName
                                           }).ToList();
                //Phòng ban theo NV được phân công
                if (taskInDb.taskAssignList != null && taskInDb.taskAssignList.Count > 0)
                {
                    if (taskInDb.IsAssignGroup != true)
                    {
                        foreach (var item in taskInDb.taskAssignList)
                        {
                            var role = (from acc in _context.AccountModel
                                        from r in acc.RolesModel
                                        where acc.EmployeeCode == item.SalesEmployeeCode
                                        && r.isEmployeeGroup == true
                                        select r.RolesName).FirstOrDefault();
                            item.RoleName = role != null ? role : "";
                        }
                    }
                }

                //Contact
                //Liên hệ
                taskInDb.taskContact = (from p in _context.TaskContactModel
                                        join s in _context.ProfileModel on p.ContactId equals s.ProfileId
                                        where p.TaskId == taskInDb.TaskId
                                        && p.isMain == true
                                        select new TaskContactViewModel()
                                        {
                                            ContactId = p.ContactId,
                                            ContactName = s.ProfileName
                                        }).FirstOrDefault();

                if (taskInDb.taskContact != null)
                {
                    taskInDb.ContactId = taskInDb.taskContact.ContactId;
                    var contact = (from p in _context.ProfileModel
                                       //Province
                                   join pr in _context.ProvinceModel on p.ProvinceId equals pr.ProvinceId into prG
                                   from province in prG.DefaultIfEmpty()
                                       //District
                                   join d in _context.DistrictModel on p.DistrictId equals d.DistrictId into dG
                                   from district in dG.DefaultIfEmpty()
                                       //Ward
                                   join wd in _context.WardModel on p.WardId equals wd.WardId into wG
                                   from ward in wG.DefaultIfEmpty()
                                   where p.ProfileId == taskInDb.taskContact.ContactId
                                   select new
                                   {
                                       ProfileAddress = p.Address,
                                       ContactName = p.ProfileName,
                                       ProvinceName = province == null ? "" : ", " + province.ProvinceName,
                                       DistrictName = district == null ? "" : ", " + district.Appellation + " " + district.DistrictName,
                                       WardName = ward == null ? "" : ", " + ward.Appellation + " " + ward.WardName,
                                       Phone = p.Phone,
                                       Email = p.Email
                                   }).FirstOrDefault();

                    //Hiển thị thông tin khách hàng theo liên hệ (nếu có)
                    if (contact != null)
                    {
                        taskInDb.ContactAddress = string.Format("{0}{1}{2}{3}", contact.ProfileAddress, contact.WardName, contact.DistrictName, contact.ProvinceName);
                        taskInDb.ContactName = contact.ContactName;
                        taskInDb.Phone = contact.Phone;
                        taskInDb.Email = contact.Email;
                    }
                }
            }
            return taskInDb;
        }
        #endregion

        /// <summary>
        /// Lấy processing của task để hiển thị Progress bar
        /// </summary>
        /// <param name="TaskId"></param>
        /// <returns>List TaskProcessingViewModel</returns>
        public List<TaskProcessingViewModel> GetTaskProcessingList(Guid TaskId)
        {
            var processing = _context.Database.SqlQuery<TaskProcessingViewModel>("EXEC [Task].[msp_TaskProcessing] @TaskId",
                                                                                new SqlParameter("@TaskId", TaskId)).ToList();
            return processing;
        }

        /// <summary>
        /// Return Summary (Tiêu đề) theo cấu hình
        /// </summary>
        /// <param name="WorkFlowId"></param>
        /// <param name="VisitTypeCode"></param>
        /// <param name="ReceiveDate"></param>
        /// <param name="StartDate"></param>
        /// <param name="ProfileId"></param>
        /// <returns></returns>
        public string GetSummary(Guid? WorkFlowId = null, string VisitTypeCode = null, DateTime? ReceiveDate = null, DateTime? StartDate = null, Guid? ProfileId = null, List<TaskAssignViewModel> taskAssignList = null, string EmployeeName = null, string ProfileName = null)
        {
            var ret = string.Empty;
            //Nếu không truyền WorkFlowId => ghé thăm
            if (WorkFlowId == null)
            {
                var workFlow = _context.WorkFlowModel.Where(p => p.WorkFlowCode == ConstWorkFlow.GT).FirstOrDefault();
                if (workFlow != null)
                {
                    WorkFlowId = workFlow.WorkFlowId;
                }
            }
            var config = _context.WorkFlowConfigModel.Where(p => p.WorkFlowId == WorkFlowId && p.FieldCode == "Summary" && p.IsRequired != true)
                                 .Select(p => p.Parameters).FirstOrDefault();
            if (!string.IsNullOrEmpty(config))
            {
                ret = config;
                //Tên ngắn khách hàng
                ProfileRepository _profileRepo = new ProfileRepository(_context);
                string ContactShortName = _profileRepo.GetProfileShortNameBy(ProfileId);
                if (!string.IsNullOrEmpty(ProfileName))
                {
                    ContactShortName = ProfileName;
                }

                //Nhiệm vụ
                WorkFlowRepository _workflowRepo = new WorkFlowRepository(_context);
                string WorkFlowName = string.Empty;
                if (WorkFlowId != null)
                {
                    var workflow = _workflowRepo.GetWorkFlow((Guid)WorkFlowId);
                    if (workflow != null)
                    {
                        WorkFlowName = workflow.WorkFlowName;
                    }
                }
                //Ngày tiếp nhận
                string ReceiveDate_Str = string.Empty;
                if (ReceiveDate != null)
                {
                    ReceiveDate_Str = ReceiveDate.Value.ToString("dd/MM/yyyy");
                }
                //Ngày bắt đầu
                string StartDate_Str = string.Empty;
                if (StartDate != null)
                {
                    StartDate_Str = StartDate.Value.ToString("dd/MM/yyyy");
                }
                //Phân loại chuyến thăm
                var visitType = _context.CatalogModel
                                        .Where(p => p.CatalogTypeCode == ConstCatalogType.VisitType && p.CatalogCode == VisitTypeCode)
                                        .Select(p => p.CatalogText_vi).FirstOrDefault();

                //NV được phân công
                string Assignee = string.Empty;
                if (taskAssignList != null && taskAssignList.Count > 0)
                {
                    var empCodeList = taskAssignList.Select(p => p.SalesEmployeeCode).ToList();
                    var empNameList = (from p in _context.SalesEmployeeModel
                                       where empCodeList.Contains(p.SalesEmployeeCode)
                                       select p.SalesEmployeeName).ToList();
                    Assignee = string.Join(", ", empNameList.ToArray());
                }

                //Current Day
                var CurrentDay = DateTime.Now.Day;
                var CurrentDay_String = CurrentDay.ToString();
                if (CurrentDay < 10)
                {
                    CurrentDay_String = "0" + CurrentDay_String;
                }
                //Current Month
                var CurrentMonth = DateTime.Now.Month;
                var CurrentMonth_String = CurrentMonth.ToString();
                if (CurrentMonth < 10)
                {
                    CurrentMonth_String = "0" + CurrentMonth_String;
                }
                var CurrentDate_String = string.Format("{0}/{1}", CurrentDay_String, CurrentMonth_String);

                if (string.IsNullOrEmpty(EmployeeName) && !string.IsNullOrEmpty(Assignee))
                {
                    EmployeeName = Assignee;
                }

                ret = ret.Replace("[WorkFlowId]", WorkFlowName);
                ret = ret.Replace("[VisitTypeCode]", visitType);
                ret = ret.Replace("[StartDate]", StartDate_Str);
                ret = ret.Replace("[ReceiveDate]", ReceiveDate_Str);
                ret = ret.Replace("[ProfileId]", ContactShortName);
                ret = ret.Replace("[Assignee]", Assignee);
                ret = ret.Replace("[EmployeeName]", EmployeeName);
                ret = ret.Replace("[CurrentDate]", CurrentDate_String);
            }
            return ret;
        }

        /// <summary>
        /// Lấy danh sách sản phẩm của task
        /// </summary>
        /// <param name="TaskId"></param>
        /// <returns></returns>
        public List<TaskProductViewModel> GetTaskProductList(Guid TaskId)
        {
            var productList = (from p in _context.TaskProductModel
                                   //Product
                               join pr in _context.ProductModel on p.ProductId equals pr.ProductId into prg
                               from pr1 in prg.DefaultIfEmpty()
                                   //Product Attribute
                               join attr in _context.ProductAttributeModel on p.ProductId equals attr.ProductId into ag
                               from at in ag.DefaultIfEmpty()
                                   //CreateBy
                               join create in _context.AccountModel on p.CreateBy equals create.AccountId
                               join cr in _context.SalesEmployeeModel on create.EmployeeCode equals cr.SalesEmployeeCode into crg
                               from cr1 in crg.DefaultIfEmpty()
                                   //EditBy
                               join modify in _context.AccountModel on p.LastEditBy equals modify.AccountId into mg
                               from m in mg.DefaultIfEmpty()
                                   //Sales Employee
                               join md in _context.SalesEmployeeModel on m.EmployeeCode equals md.SalesEmployeeCode into mdg
                               from md1 in mdg.DefaultIfEmpty()
                                   //Phân cấp SP (ProductLevelCode)
                               join level in _context.CatalogModel on new
                               {
                                   ProductLevelCode = p.ProductLevelCode,
                                   CatalogTypeCode = ConstCatalogType.ProductLevel
                               } equals new
                               {
                                   ProductLevelCode = level.CatalogCode,
                                   CatalogTypeCode = level.CatalogTypeCode
                               } into lg
                               from cat1 in lg.DefaultIfEmpty()
                                   //Nhóm vật tư (ProductCategoryCode)
                               join category in _context.CatalogModel on new
                               {
                                   ProductLevelCode = p.ProductCategoryCode,
                                   CatalogTypeCode = ConstCatalogType.ProductCategory
                               } equals new
                               {
                                   ProductLevelCode = category.CatalogCode,
                                   CatalogTypeCode = category.CatalogTypeCode
                               } into cg
                               from cat2 in cg.DefaultIfEmpty()
                                   //ĐVT
                               join unit in _context.CatalogModel on new
                               {
                                   Unit = p.Unit,
                                   CatalogTypeCode = ConstCatalogType.Unit
                               } equals new
                               {
                                   Unit = unit.CatalogCode,
                                   CatalogTypeCode = unit.CatalogTypeCode
                               } into u
                               from pu in u.DefaultIfEmpty()
                                   //Phương thức xử lý
                               join err in _context.CatalogModel on new
                               {
                                   ErrorCode = p.ErrorCode,
                                   CatalogTypeCode = ConstCatalogType.Error
                               } equals new
                               {
                                   ErrorCode = err.CatalogCode,
                                   CatalogTypeCode = err.CatalogTypeCode
                               } into erg
                               from error in erg.DefaultIfEmpty()
                               where p.TaskId == TaskId
                               orderby p.CreateTime
                               select new TaskProductViewModel
                               {
                                   TaskProductId = p.TaskProductId,
                                   TaskId = p.TaskId,
                                   ProductId = p.ProductId,
                                   ERPProductCode = pr1.ERPProductCode,
                                   ProductCode = pr1.ProductCode,
                                   ProductName = pr1.ProductName,
                                   Qty = p.Qty,
                                   Unit = p.Unit == null ? at.Unit : p.Unit,
                                   UnitName = p.Unit == null ? at.Unit : pu.CatalogText_vi,
                                   CreateBy = p.CreateBy,
                                   CreateByName = cr1.SalesEmployeeName,
                                   CreateTime = p.CreateTime,
                                   LastEditBy = p.LastEditBy,
                                   LastEditByName = md1.SalesEmployeeName,
                                   LastEditTime = p.LastEditTime,
                                   ErrorCode = p.ErrorCode,
                                   ErrorCodeName = error.CatalogText_vi,
                                   ErrorTypeCode = p.ErrorTypeCode,
                                   ProductCategoryCode = p.ProductCategoryCode,
                                   ProductCategoryName = cat2.CatalogText_vi,
                                   ProductLevelCode = p.ProductLevelCode,
                                   ProductLevelName = cat1.CatalogText_vi,
                                   ProductColorCode = p.ProductColorCode,
                                   SAPSOWarranty = p.SAPSOWarranty,
                                   SAPSOProduct = p.SAPSOProduct,
                                   WarrantyValue = p.WarrantyValue,
                                   ProductValue = p.ProductValue,
                                   DiscountValue = p.DiscountValue,
                                   SerialNumber = p.SerialNumber,
                               }).ToList();

            //Phụ kiện
            if (productList != null && productList.Count > 0)
            {
                foreach (var item in productList)
                {
                    item.Accessory = string.Empty;
                    var taskProductAccessoryList = (from p in _context.TaskProductAccessoryModel
                                                    join pr in _context.ProductModel on p.AccessoryId equals pr.ProductId
                                                    //Loại phụ kiện
                                                    join type in _context.CatalogModel on new
                                                    {
                                                        ProductAccessoryTypeCode = p.ProductAccessoryTypeCode,
                                                        CatalogTypeCode = ConstCatalogType.ProductAccessoryType
                                                    } equals new
                                                    {
                                                        ProductAccessoryTypeCode = type.CatalogCode,
                                                        CatalogTypeCode = type.CatalogTypeCode
                                                    } into tg
                                                    from typeCode in tg.DefaultIfEmpty()
                                                        //Hình thức BH
                                                    join err in _context.CatalogModel on new
                                                    {
                                                        AccErrorTypeCode = p.AccErrorTypeCode,
                                                        CatalogTypeCode = ConstCatalogType.ErrorType
                                                    } equals new
                                                    {
                                                        AccErrorTypeCode = err.CatalogCode,
                                                        CatalogTypeCode = err.CatalogTypeCode
                                                    } into eg
                                                    from error in eg.DefaultIfEmpty()
                                                    where p.TaskProductId == item.TaskProductId
                                                    select new TaskProductViewModel
                                                    {
                                                        TaskProductId = p.TaskProductId,
                                                        ProductId = p.AccessoryId,
                                                        ProductCode = pr.ProductCode,
                                                        ProductName = pr.ERPProductCode + " | " + pr.ProductName,
                                                        ERPProductCode = pr.ERPProductCode,
                                                        //ProductName = pr.ProductName,
                                                        Qty = p.Qty,
                                                        ProductAccessoryTypeCode = p.ProductAccessoryTypeCode,
                                                        ProductAccessoryTypeName = typeCode.CatalogText_vi,
                                                        AccErrorTypeCode = p.AccErrorTypeCode,
                                                        AccErrorTypeName = error.CatalogText_vi
                                                    }).ToList();
                    if (taskProductAccessoryList != null && taskProductAccessoryList.Count > 0)
                    {
                        foreach (var accessory in taskProductAccessoryList)
                        {
                            //item.Accessory += accessory.ProductName + " (SL: " + accessory.Qty + ") - " + accessory.ProductAccessoryTypeName + "<br>";
                            item.AccessoryName += accessory.ProductName + "<br />";
                            item.ProductAccessoryTypeName += accessory.ProductAccessoryTypeName + "<br />";
                            item.AccessoryQuantity += accessory.Qty + "<br />";
                            item.AccErrorTypeName += accessory.AccErrorTypeName + "<br />";
                        }
                    }

                    //các lỗi bảo hành thường gặp
                    item.UsualErrorName = string.Empty;
                    var taskProductUsualErrorList = (from p in _context.TaskProductUsualErrorModel
                                                     join cTemp in _context.CatalogModel on new { UsualErrorCode = p.UsualErrorCode, Type = ConstCatalogType.UsualError } equals new { UsualErrorCode = cTemp.CatalogCode, Type = cTemp.CatalogTypeCode } into list0
                                                     from c in list0.DefaultIfEmpty()
                                                     where p.TaskProductId == item.TaskProductId
                                                     select c.CatalogText_vi).ToList();
                    if (taskProductUsualErrorList != null && taskProductUsualErrorList.Count > 0)
                    {
                        foreach (var taskProductUsualError in taskProductUsualErrorList)
                        {
                            item.UsualErrorName += taskProductUsualError + "<br />";
                        }
                    }
                }
            }

            return productList;
        }

        /// <summary>
        /// Lấy chi tiết sản phẩm bảo hành (Task Product)
        /// </summary>
        /// <param name="TaskProductId"></param>
        /// <returns>TaskProductViewModel</returns>
        public TaskProductViewModel GetTaskProduct(Guid? TaskProductId)
        {
            var taskProduct = (from p in _context.TaskProductModel
                                   //Product
                               join pr in _context.ProductModel on p.ProductId equals pr.ProductId into prg
                               from pr1 in prg.DefaultIfEmpty()
                                   //Product Attribute
                               join attr in _context.ProductAttributeModel on p.ProductId equals attr.ProductId into ag
                               from at in ag.DefaultIfEmpty()
                                   //CreateBy
                               join create in _context.AccountModel on p.CreateBy equals create.AccountId
                               join cr in _context.SalesEmployeeModel on create.EmployeeCode equals cr.SalesEmployeeCode into crg
                               from cr1 in crg.DefaultIfEmpty()
                                   //EditBy
                               join modify in _context.AccountModel on p.LastEditBy equals modify.AccountId into mg
                               from m in mg.DefaultIfEmpty()
                                   //Sales Employee
                               join md in _context.SalesEmployeeModel on m.EmployeeCode equals md.SalesEmployeeCode into mdg
                               from md1 in mdg.DefaultIfEmpty()

                               where p.TaskProductId == TaskProductId
                               select new TaskProductViewModel
                               {
                                   TaskProductId = p.TaskProductId,
                                   TaskId = p.TaskId,
                                   ProductId = p.ProductId,
                                   ERPProductCode = pr1.ERPProductCode,
                                   ProductCode = pr1.ProductCode,
                                   ProductName = pr1 != null ? pr1.ERPProductCode + " | " + pr1.ProductName : "",
                                   Qty = p.Qty,
                                   Unit = p.Unit == null ? at.Unit : p.Unit,
                                   CreateBy = p.CreateBy,
                                   CreateByName = cr1.SalesEmployeeName,
                                   CreateTime = p.CreateTime,
                                   LastEditBy = p.LastEditBy,
                                   LastEditByName = md1.SalesEmployeeName,
                                   LastEditTime = p.LastEditTime,
                                   ErrorCode = p.ErrorCode,
                                   ErrorTypeCode = p.ErrorTypeCode,
                                   ProductCategoryCode = p.ProductCategoryCode,
                                   ProductLevelCode = p.ProductLevelCode,
                                   ProductColorCode = p.ProductColorCode,
                                   WarrantyValue = p.WarrantyValue,
                                   ProductValue = p.ProductValue,
                                   SAPSOProduct = p.SAPSOProduct,
                                   SAPSOWarranty = p.SAPSOWarranty,
                                   DiscountValue = p.DiscountValue
                               }).FirstOrDefault();


            if (taskProduct != null)
            {
                //phụ kiện thay thế
                var taskProductAccessoryList = (from p in _context.TaskProductAccessoryModel
                                                join pr in _context.ProductModel on p.AccessoryId equals pr.ProductId
                                                where p.TaskProductId == taskProduct.TaskProductId
                                                select new TaskProductViewModel
                                                {
                                                    TaskProductId = p.TaskProductId,
                                                    ProductId = p.AccessoryId,
                                                    ProductCode = pr.ProductCode,
                                                    //ProductName = pr.ERPProductCode + " | " + pr.ProductName,
                                                    ProductName = pr.ProductName,
                                                    Qty = p.Qty,
                                                    ProductAccessoryTypeCode = p.ProductAccessoryTypeCode,
                                                    AccErrorTypeCode = p.AccErrorTypeCode
                                                }).ToList();
                if (taskProductAccessoryList != null && taskProductAccessoryList.Count > 0)
                {
                    taskProduct.accessoryList = new List<TaskProductViewModel>();
                    taskProduct.accessoryList = taskProductAccessoryList;
                }

                //các lỗi bảo hành thường gặp
                var taskProductUsualErrorList = (from p in _context.TaskProductUsualErrorModel
                                                 where p.TaskProductId == taskProduct.TaskProductId
                                                 select p.UsualErrorCode).ToList();
                if (taskProductUsualErrorList != null && taskProductUsualErrorList.Count > 0)
                {
                    taskProduct.usualErrorList = new List<string>();
                    taskProduct.usualErrorList = taskProductUsualErrorList;
                }
            }
            return taskProduct;
        }

        /// <summary>
        /// Lấy lỗi thường gặp theo nhóm vật tư và công ty
        /// </summary>
        /// <param name="ProductCategoryCode"></param>
        /// <param name="CompanyCode"></param>
        /// <returns></returns>
        public List<CatalogViewModel> GetUsualErrorByProductCategory(string ProductCategoryCode, string CompanyCode)
        {
            var categoryType = _context.CatalogModel.Where(p => p.CatalogCode == ProductCategoryCode
                                                             && p.CatalogText_en.Contains(CompanyCode))
                                       .Select(p => p.CatalogText_en).FirstOrDefault();
            if (categoryType != null && categoryType.Contains("_"))
            {
                categoryType = categoryType.Substring(categoryType.LastIndexOf('_') + 1);
            }
            var errorList = (from p in _context.CatalogModel
                             where p.CatalogTypeCode == ConstCatalogType.UsualError
                             && p.CatalogText_en == categoryType
                             orderby p.OrderIndex
                             select new CatalogViewModel
                             {
                                 CatalogCode = p.CatalogCode,
                                 CatalogText_en = p.CatalogText_en,
                                 CatalogText_vi = p.CatalogText_vi,
                                 OrderIndex = p.OrderIndex
                             }).ToList();
            return errorList;
        }

        /// <summary>
        /// Lấy thông tin khách hàng theo Task
        /// </summary>
        /// <param name="TaskId"></param>
        /// <returns></returns>
        public ProfileViewModel GetProfileByTask(Guid? TaskId)
        {
            var profile = (from p in _context.TaskModel
                           join pro in _context.ProfileModel on p.ProfileId equals pro.ProfileId
                           where p.TaskId == TaskId
                           select new ProfileViewModel
                           {
                               ProfileId = pro.ProfileId,
                               ProfileName = string.IsNullOrEmpty(pro.ProfileShortName) ? pro.ProfileName : pro.ProfileShortName,
                               ProfileShortName = pro.ProfileShortName,
                               Address = pro.Address
                           }).FirstOrDefault();
            if (profile == null)
            {
                profile = new ProfileViewModel();
            }
            return profile;
        }
        public ProfileViewModel GetProfileByTask(int? TaskCode)
        {
            var profile = (from p in _context.TaskModel
                           join pro in _context.ProfileModel on p.ProfileId equals pro.ProfileId
                           where p.TaskCode == TaskCode
                           select new ProfileViewModel
                           {
                               ProfileId = pro.ProfileId,
                               ProfileName = string.IsNullOrEmpty(pro.ProfileShortName) ? pro.ProfileName : pro.ProfileShortName,
                               ProfileShortName = pro.ProfileShortName,
                               Address = pro.Address
                           }).FirstOrDefault();
            if (profile == null)
            {
                profile = new ProfileViewModel();
            }
            return profile;
        }

        /// <summary>
        /// Lấy 5 công việc mới tạo gần đây
        /// </summary>
        /// <param name="TotalTask"></param>
        /// <returns>danh sách công việc</returns>
        public List<TaskViewModel> Get4TaskRecently(out int TotalTask)
        {
            var listTask = (from t in _context.TaskModel
                            join p in _context.ProfileModel on t.ProfileId equals p.ProfileId
                            // from pro in profileTable.DefaultIfEmpty()
                            // join c in _context.CatalogModel on t.PriorityCode equals c.CatalogCode
                            join w in _context.WorkFlowModel on t.WorkFlowId equals w.WorkFlowId
                            // join ts in _context.TaskStatusModel on t.TaskStatusId equals ts.TaskStatusId
                            where t.Actived == true && t.isDeleted == null
                            orderby t.CreateTime descending
                            select new TaskViewModel
                            {
                                TaskId = t.TaskId,
                                TaskCode = t.TaskCode,
                                Summary = t.Summary,
                                ProfileId = t.ProfileId,
                                ProfileName = p.ProfileName,
                                Description = t.Description,
                                PriorityCode = t.PriorityCode,
                                // PriorityText_vi = c.CatalogText_vi,
                                WorkFlowId = t.WorkFlowId,
                                WorkFlowName = w.WorkFlowName,
                                WorkFlowCode = w.WorkFlowCode,
                                WorkFlowImageUrl = w.ImageUrl,
                                TaskStatusId = t.TaskStatusId,
                                //  TaskStatusName = ts.TaskStatusName,
                                ReceiveDate = t.ReceiveDate,
                                CreateBy = t.CreateBy,
                                CreateTime = t.CreateTime,
                                Actived = t.Actived
                            });
            //Count Number of task
            TotalTask = listTask.Count();
            //Get Task List
            return listTask.Take(4).ToList();
        }

        public List<TaskViewModel> GetAll()
        {
            var listTask = (from t in _context.TaskModel
                            join p in _context.ProfileModel on t.ProfileId equals p.ProfileId into profileTable
                            from pro in profileTable.DefaultIfEmpty()
                            join c in _context.CatalogModel on t.PriorityCode equals c.CatalogCode
                            join w in _context.WorkFlowModel on t.WorkFlowId equals w.WorkFlowId
                            join ts in _context.TaskStatusModel on t.TaskStatusId equals ts.TaskStatusId
                            // join cm in _context.CompanyModel on t.CompanyId equals cm.CompanyId
                            //  join s in _context.StoreModel on t.StoreId equals s.StoreId
                            where t.isDeleted != true
                            orderby t.CreateTime descending
                            select new TaskViewModel
                            {
                                TaskId = t.TaskId,
                                TaskCode = t.TaskCode,
                                Summary = t.Summary,
                                ProfileId = t.ProfileId,
                                ProfileName = pro.ProfileName,
                                Description = t.Description,
                                PriorityCode = t.PriorityCode,
                                PriorityText_vi = c.CatalogText_vi,
                                WorkFlowId = t.WorkFlowId,
                                WorkFlowName = w.WorkFlowName,
                                TaskStatusId = t.TaskStatusId,
                                TaskStatusName = ts.TaskStatusName,
                                ReceiveDate = t.ReceiveDate,
                                Actived = t.Actived,
                                CreateTime = t.CreateTime
                            })
                            .ToList();
            return listTask;
        }

        public TaskModel Create(TaskViewModel taskViewModel)
        {
            var taskNew = new TaskModel();
            taskNew.MapTask(taskViewModel);
            taskNew.CreateBy = taskViewModel.CreateBy;
            taskNew.CreateTime = taskViewModel.CreateTime;

            //Profile Address
            if (!string.IsNullOrEmpty(taskNew.ProfileAddress))
            {
                try
                {
                    //Tìm theo địa chỉ chính (ProfileModel)
                    var profile = new ProfileRepository(_context).GetById(Guid.Parse(taskNew.ProfileAddress));
                    if (profile != null)
                    {
                        profile.Address += (!string.IsNullOrEmpty(profile.WardName) ? ", " + profile.WardName : null);
                        profile.Address += (!string.IsNullOrEmpty(profile.DistrictName) ? ", " + profile.DistrictName : null);
                        profile.Address += (!string.IsNullOrEmpty(profile.ProvinceName) ? ", " + profile.ProvinceName : null);
                        if (!string.IsNullOrEmpty(profile.Address) && profile.Address.StartsWith(","))
                        {
                            taskNew.ProfileAddress = profile.Address.Remove(0, 1).Trim();
                        }
                        else
                        {
                            taskNew.ProfileAddress = profile.Address;
                        }
                    }
                    else
                    {
                        var address = new AddressBookRepository(_context).GetById(Guid.Parse(taskNew.ProfileAddress));
                        if (address != null)
                        {
                            address.Address += (!string.IsNullOrEmpty(address.WardName) ? ", " + address.WardName : null);
                            address.Address += (!string.IsNullOrEmpty(address.DistrictName) ? ", " + address.DistrictName : null);
                            address.Address += (!string.IsNullOrEmpty(address.ProvinceName) ? ", " + address.ProvinceName : null);
                            if (!string.IsNullOrEmpty(address.Address) && address.Address.StartsWith(","))
                            {
                                taskNew.ProfileAddress = address.Address.Remove(0, 1).Trim();
                            }
                            else
                            {
                                taskNew.ProfileAddress = address.Address;
                            }
                        }
                    }
                }
                catch (Exception)
                { }

            }
            //Cập nhật TaskCode cho subtask
            if (taskViewModel.ParentTaskId.HasValue)
            {
                var parentTask = _context.TaskModel.Where(p => p.TaskId == taskViewModel.ParentTaskId).FirstOrDefault();
                if (parentTask != null)
                {
                    int index = 0;
                    var lastSubtask = _context.TaskModel.Where(p => p.ParentTaskId == taskViewModel.ParentTaskId).Count();
                    index = lastSubtask + 1;
                    string subtaskCode = string.Format("{0}-{1}", parentTask.TaskCode, index);
                    taskNew.SubtaskCode = subtaskCode;
                }
            }

            //Ratings
            //Ý kiến khách hàng + đánh giá
            if (!string.IsNullOrEmpty(taskViewModel.CustomerRatings))
            {
                taskNew.Property5 = taskViewModel.CustomerRatings;
                //Nếu dữ liệu cũ có rating thì xóa đi thêm lại
                var existRatingProduct = _context.RatingModel.Where(p => p.ReferenceId == taskNew.TaskId && p.RatingTypeCode == ConstCatalogType.Ticket_CustomerReviews_Product).FirstOrDefault();
                if (existRatingProduct != null)
                {
                    _context.Entry(existRatingProduct).State = EntityState.Deleted;
                }
                var existRatingService = _context.RatingModel.Where(p => p.ReferenceId == taskNew.TaskId && p.RatingTypeCode == ConstCatalogType.Ticket_CustomerReviews_Service).FirstOrDefault();
                if (existRatingService != null)
                {
                    _context.Entry(existRatingService).State = EntityState.Deleted;
                }
                var existsCustomerReviews = _context.RatingModel.Where(p => p.ReferenceId == taskNew.TaskId && p.RatingTypeCode == null && p.Reviews != null).FirstOrDefault();
                if (existsCustomerReviews != null)
                {
                    _context.Entry(existsCustomerReviews).State = EntityState.Deleted;
                }
                //1. Không ý kiến => Property5 = none
                if (taskViewModel.CustomerRatings == "none")
                {

                }
                //2. Đánh giá theo sao => lưu thông tin vào RatingModel
                else if (taskViewModel.CustomerRatings == "rating")
                {
                    //2.1. Về sản phẩm
                    if (!string.IsNullOrEmpty(taskViewModel.Ticket_CustomerReviews_Product))
                    {
                        RatingModel ratingProduct = new RatingModel();
                        ratingProduct.RatingId = Guid.NewGuid();
                        ratingProduct.RatingTypeCode = ConstCatalogType.Ticket_CustomerReviews_Product;
                        ratingProduct.ReferenceId = taskNew.TaskId;
                        ratingProduct.Ratings = taskViewModel.Ticket_CustomerReviews_Product;
                        _context.Entry(ratingProduct).State = EntityState.Added;
                    }

                    //2.2. Về dịch vụ
                    if (!string.IsNullOrEmpty(taskViewModel.Ticket_CustomerReviews_Service))
                    {
                        RatingModel ratingService = new RatingModel();
                        ratingService.RatingId = Guid.NewGuid();
                        ratingService.RatingTypeCode = ConstCatalogType.Ticket_CustomerReviews_Service;
                        ratingService.ReferenceId = taskNew.TaskId;
                        ratingService.Ratings = taskViewModel.Ticket_CustomerReviews_Service;
                        _context.Entry(ratingService).State = EntityState.Added;
                    }

                    //2.3. Ý kiến khác => nhập nội dung => lưu thông tin vào RatingModel (chỉ lưu reviews)
                    if (!string.IsNullOrEmpty(taskViewModel.Property5))
                    {
                        RatingModel customerReviews = new RatingModel();
                        customerReviews.RatingId = Guid.NewGuid();
                        customerReviews.ReferenceId = taskNew.TaskId;
                        customerReviews.Reviews = taskViewModel.Property5;
                        _context.Entry(customerReviews).State = EntityState.Added;
                    }
                }
            }
            //Bảo hành_Kết quả
            if (!string.IsNullOrEmpty(taskViewModel.Result))
            {
                taskNew.CancelReason = taskViewModel.Result;
            }
            _context.TaskModel.Add(taskNew);
            return taskNew;
        }

        #region Check required field config 
        public List<string> CheckTaskRequiredFieldConfig(TaskViewModel taskViewModel)
        {
            List<string> errorMessageRequired = new List<string>();
            var fieldConfig = _context.WorkFlowConfigModel.Where(p => p.WorkFlowId == taskViewModel.WorkFlowId && p.IsRequired == true).ToList();
            if (fieldConfig != null && fieldConfig.Count > 0)
            {
                foreach (var field in fieldConfig)
                {
                    //Tiêu đề
                    if (field.FieldCode == RepositoryPropertyHelper.GetPropertyName<TaskViewModel, string>(p => p.Summary))
                    {
                        if (string.IsNullOrEmpty(taskViewModel.Summary))
                        {
                            errorMessageRequired.Add(string.Format(LanguageResource.Required, RepositoryPropertyHelper.GetDisplayName<TaskViewModel, string>(p => p.Summary)));
                        }
                    }
                    //Yêu cầu
                    else if (field.FieldCode == RepositoryPropertyHelper.GetPropertyName<TaskViewModel, string>(p => p.Requirement))
                    {
                        if (string.IsNullOrEmpty(taskViewModel.Requirement))
                        {
                            errorMessageRequired.Add(string.Format(LanguageResource.Required, RepositoryPropertyHelper.GetDisplayName<TaskViewModel, string>(p => p.Requirement)));
                        }
                    }
                    //Task riêng tư
                    else if (field.FieldCode == RepositoryPropertyHelper.GetPropertyName<TaskViewModel, bool?>(p => p.isPrivate))
                    {
                        if (!taskViewModel.isPrivate.HasValue)
                        {
                            errorMessageRequired.Add(string.Format(LanguageResource.Required, RepositoryPropertyHelper.GetDisplayName<TaskViewModel, bool?>(p => p.isPrivate)));
                        }
                    }
                    //Khách hàng
                    else if (field.FieldCode == RepositoryPropertyHelper.GetPropertyName<TaskViewModel, Guid?>(p => p.ProfileId))
                    {
                        if (!taskViewModel.ProfileId.HasValue)
                        {
                            errorMessageRequired.Add(string.Format(LanguageResource.Required, RepositoryPropertyHelper.GetDisplayName<TaskViewModel, Guid?>(p => p.ProfileId)));
                        }
                    }
                    //Mô tả
                    else if (field.FieldCode == RepositoryPropertyHelper.GetPropertyName<TaskViewModel, string>(p => p.Description))
                    {
                        if (string.IsNullOrEmpty(taskViewModel.Description))
                        {
                            errorMessageRequired.Add(string.Format(LanguageResource.Required, RepositoryPropertyHelper.GetDisplayName<TaskViewModel, string>(p => p.Description)));
                        }
                    }
                    //Mức độ
                    else if (field.FieldCode == RepositoryPropertyHelper.GetPropertyName<TaskViewModel, string>(p => p.PriorityCode))
                    {
                        if (string.IsNullOrEmpty(taskViewModel.PriorityCode))
                        {
                            errorMessageRequired.Add(string.Format(LanguageResource.Required, RepositoryPropertyHelper.GetDisplayName<TaskViewModel, string>(p => p.PriorityCode)));
                        }
                    }
                    //Yêu cầu cần xử lý
                    else if (field.FieldCode == RepositoryPropertyHelper.GetPropertyName<TaskViewModel, string>(p => p.HasRequest))
                    {
                        if (string.IsNullOrEmpty(taskViewModel.HasRequest))
                        {
                            errorMessageRequired.Add(string.Format(LanguageResource.Required, RepositoryPropertyHelper.GetDisplayName<TaskViewModel, string>(p => p.HasRequest)));
                        }
                    }
                    //Phiếu bảo hành
                    else if (field.FieldCode == RepositoryPropertyHelper.GetPropertyName<TaskViewModel, Guid?>(p => p.ProductWarrantyId))
                    {
                        if (!taskViewModel.ProductWarrantyId.HasValue)
                        {
                            errorMessageRequired.Add(string.Format(LanguageResource.Required, RepositoryPropertyHelper.GetDisplayName<TaskViewModel, Guid?>(p => p.ProductWarrantyId)));
                        }
                    }
                    //Phân loại sản phẩm bảo hành	
                    else if (field.FieldCode == RepositoryPropertyHelper.GetPropertyName<TaskViewModel, string>(p => p.Property1))
                    {
                        if (string.IsNullOrEmpty(taskViewModel.Property1))
                        {
                            errorMessageRequired.Add(string.Format(LanguageResource.Required, RepositoryPropertyHelper.GetDisplayName<TaskViewModel, string>(p => p.Property1)));
                        }
                    }
                    //Số chứng từ
                    else if (field.FieldCode == RepositoryPropertyHelper.GetPropertyName<TaskViewModel, string>(p => p.Property2))
                    {
                        if (string.IsNullOrEmpty(taskViewModel.Property2))
                        {
                            errorMessageRequired.Add(string.Format(LanguageResource.Required, RepositoryPropertyHelper.GetDisplayName<TaskViewModel, string>(p => p.Property2)));
                        }
                    }
                    //Giá trị bảo hành 
                    else if (field.FieldCode == RepositoryPropertyHelper.GetPropertyName<TaskViewModel, decimal?>(p => p.Property3))
                    {
                        if (!taskViewModel.Property3.HasValue)
                        {
                            errorMessageRequired.Add(string.Format(LanguageResource.Required, RepositoryPropertyHelper.GetDisplayName<TaskViewModel, decimal?>(p => p.Property3)));
                        }
                    }
                    //Thời gian thi công
                    else if (field.FieldCode == RepositoryPropertyHelper.GetPropertyName<TaskViewModel, DateTime?>(p => p.Date1))
                    {
                        if (!taskViewModel.Date1.HasValue)
                        {
                            errorMessageRequired.Add(string.Format(LanguageResource.Required, RepositoryPropertyHelper.GetDisplayName<TaskViewModel, DateTime?>(p => p.Date1)));
                        }
                    }
                    //Ngày tiếp nhận
                    else if (field.FieldCode == RepositoryPropertyHelper.GetPropertyName<TaskViewModel, DateTime?>(p => p.ReceiveDate))
                    {
                        if (!taskViewModel.ReceiveDate.HasValue)
                        {
                            errorMessageRequired.Add(string.Format(LanguageResource.Required, RepositoryPropertyHelper.GetDisplayName<TaskViewModel, DateTime?>(p => p.ReceiveDate)));
                        }
                    }
                    //Ngày bắt đầu
                    else if (field.FieldCode == RepositoryPropertyHelper.GetPropertyName<TaskViewModel, DateTime?>(p => p.StartDate))
                    {
                        if (!taskViewModel.StartDate.HasValue)
                        {
                            errorMessageRequired.Add(string.Format(LanguageResource.Required, RepositoryPropertyHelper.GetDisplayName<TaskViewModel, DateTime?>(p => p.StartDate)));
                        }
                    }
                    //Ngày đến hạn
                    else if (field.FieldCode == RepositoryPropertyHelper.GetPropertyName<TaskViewModel, DateTime?>(p => p.EstimateEndDate))
                    {
                        if (!taskViewModel.EstimateEndDate.HasValue)
                        {
                            errorMessageRequired.Add(string.Format(LanguageResource.Required, RepositoryPropertyHelper.GetDisplayName<TaskViewModel, DateTime?>(p => p.EstimateEndDate)));
                        }
                    }
                    //Ngày kết thúc
                    else if (field.FieldCode == RepositoryPropertyHelper.GetPropertyName<TaskViewModel, DateTime?>(p => p.EndDate))
                    {
                        if (!taskViewModel.EndDate.HasValue)
                        {
                            errorMessageRequired.Add(string.Format(LanguageResource.Required, RepositoryPropertyHelper.GetDisplayName<TaskViewModel, DateTime?>(p => p.EndDate)));
                        }
                    }
                    //NV theo dõi/giám sát
                    else if (field.FieldCode == RepositoryPropertyHelper.GetPropertyName<TaskViewModel, string>(p => p.Reporter))
                    {
                        //if (string.IsNullOrEmpty(taskViewModel.Reporter))
                        //{
                        //    errorMessageRequired.Add(string.Format(LanguageResource.Required, RepositoryPropertyHelper.GetDisplayName<TaskViewModel, string>(p => p.Reporter)));
                        //}
                        if (taskViewModel.taskReporterList == null || taskViewModel.taskReporterList.Count == 0)
                        {
                            errorMessageRequired.Add(string.Format(LanguageResource.Required, RepositoryPropertyHelper.GetDisplayName<TaskViewModel, string>(p => p.Reporter)));
                        }
                    }
                    //NV được phân công
                    else if (field.FieldCode == RepositoryPropertyHelper.GetPropertyName<TaskViewModel, string>(p => p.Assignee))
                    {
                        if (taskViewModel.taskAssignList == null || taskViewModel.taskAssignList.Count == 0)
                        {
                            errorMessageRequired.Add(string.Format(LanguageResource.Required, RepositoryPropertyHelper.GetDisplayName<TaskViewModel, string>(p => p.Assignee)));
                        }
                    }
                    //Vai trò NV được phân công
                    else if (field.FieldCode == RepositoryPropertyHelper.GetPropertyName<TaskViewModel, string>(p => p.RoleName))
                    {
                        if (string.IsNullOrEmpty(taskViewModel.RoleName))
                        {
                            errorMessageRequired.Add(string.Format(LanguageResource.Required, LanguageResource.PersonInCharge_RoleCode));
                        }
                    }
                    //Kết quả/Ý kiến khách hàng
                    else if (field.FieldCode == RepositoryPropertyHelper.GetPropertyName<TaskViewModel, string>(p => p.CustomerReviews))
                    {
                        if (string.IsNullOrEmpty(taskViewModel.CustomerReviews))
                        {
                            errorMessageRequired.Add(string.Format(LanguageResource.Required, RepositoryPropertyHelper.GetDisplayName<TaskViewModel, string>(p => p.CustomerReviews)));
                        }
                    }
                    //Trung tâm bảo hành
                    else if (field.FieldCode == RepositoryPropertyHelper.GetPropertyName<TaskViewModel, string>(p => p.ServiceTechnicalTeamCode))
                    {
                        if (string.IsNullOrEmpty(taskViewModel.ServiceTechnicalTeamCode))
                        {
                            errorMessageRequired.Add(string.Format(LanguageResource.Required, RepositoryPropertyHelper.GetDisplayName<TaskViewModel, string>(p => p.ServiceTechnicalTeamCode)));
                        }
                    }
                    //Nguồn tiếp nhận
                    else if (field.FieldCode == RepositoryPropertyHelper.GetPropertyName<TaskViewModel, string>(p => p.Property4))
                    {
                        if (string.IsNullOrEmpty(taskViewModel.Property4))
                        {
                            errorMessageRequired.Add(string.Format(LanguageResource.Required, RepositoryPropertyHelper.GetDisplayName<TaskViewModel, string>(p => p.Property4)));
                        }
                    }
                    //Đơn vị thi công
                    else if (field.FieldCode == RepositoryPropertyHelper.GetPropertyName<TaskViewModel, Guid?>(p => p.ConstructionUnit))
                    {
                        if (!taskViewModel.ConstructionUnit.HasValue)
                        {
                            errorMessageRequired.Add(string.Format(LanguageResource.Required, RepositoryPropertyHelper.GetDisplayName<TaskViewModel, Guid?>(p => p.ConstructionUnit)));
                        }
                    }
                    //Ngày ghé thăm
                    else if (field.FieldCode == RepositoryPropertyHelper.GetPropertyName<TaskViewModel, DateTime?>(p => p.VisitDate))
                    {
                        if (!taskViewModel.VisitDate.HasValue)
                        {
                            errorMessageRequired.Add(string.Format(LanguageResource.Required, RepositoryPropertyHelper.GetDisplayName<TaskViewModel, DateTime?>(p => p.VisitDate)));
                        }
                    }
                    //Địa điểm ghé thăm
                    else if (field.FieldCode == RepositoryPropertyHelper.GetPropertyName<TaskViewModel, string>(p => p.VisitAddress))
                    {
                        if (string.IsNullOrEmpty(taskViewModel.VisitAddress))
                        {
                            errorMessageRequired.Add(string.Format(LanguageResource.Required, RepositoryPropertyHelper.GetDisplayName<TaskViewModel, string>(p => p.VisitAddress)));
                        }
                    }
                    //Khu vực
                    else if (field.FieldCode == RepositoryPropertyHelper.GetPropertyName<TaskViewModel, string>(p => p.VisitSaleOfficeCode))
                    {
                        if (string.IsNullOrEmpty(taskViewModel.VisitSaleOfficeCode))
                        {
                            errorMessageRequired.Add(string.Format(LanguageResource.Required, RepositoryPropertyHelper.GetDisplayName<TaskViewModel, string>(p => p.VisitSaleOfficeCode)));
                        }
                    }
                    //Yêu cầu checkin
                    else if (field.FieldCode == RepositoryPropertyHelper.GetPropertyName<TaskViewModel, bool?>(p => p.isRequiredCheckin))
                    {
                        if (!taskViewModel.isRequiredCheckin.HasValue)
                        {
                            errorMessageRequired.Add(string.Format(LanguageResource.Required, RepositoryPropertyHelper.GetDisplayName<TaskViewModel, bool?>(p => p.isRequiredCheckin)));
                        }
                    }
                    //Phân loại chuyến thăm
                    else if (field.FieldCode == RepositoryPropertyHelper.GetPropertyName<TaskViewModel, string>(p => p.VisitTypeCode))
                    {
                        if (string.IsNullOrEmpty(taskViewModel.VisitTypeCode))
                        {
                            errorMessageRequired.Add(string.Format(LanguageResource.Required, RepositoryPropertyHelper.GetDisplayName<TaskViewModel, string>(p => p.VisitTypeCode)));
                        }
                    }
                    //Nguồn khách hàng
                    else if (field.FieldCode == RepositoryPropertyHelper.GetPropertyName<TaskViewModel, string>(p => p.ShowroomCode))
                    {
                        if (string.IsNullOrEmpty(taskViewModel.ShowroomCode))
                        {
                            errorMessageRequired.Add(string.Format(LanguageResource.Required, RepositoryPropertyHelper.GetDisplayName<TaskViewModel, string>(p => p.ShowroomCode)));
                        }
                    }
                    //NV tiếp khách
                    else if (field.FieldCode == RepositoryPropertyHelper.GetPropertyName<TaskViewModel, string>(p => p.SaleEmployeeCode))
                    {
                        if (string.IsNullOrEmpty(taskViewModel.SaleEmployeeCode))
                        {
                            errorMessageRequired.Add(string.Format(LanguageResource.Required, RepositoryPropertyHelper.GetDisplayName<TaskViewModel, string>(p => p.SaleEmployeeCode)));
                        }
                    }
                    //Khách biết đến An Cường qua
                    else if (field.FieldCode == RepositoryPropertyHelper.GetPropertyName<TaskViewModel, string>(p => p.ChannelCode))
                    {
                        if (string.IsNullOrEmpty(taskViewModel.ChannelCode))
                        {
                            errorMessageRequired.Add(string.Format(LanguageResource.Required, RepositoryPropertyHelper.GetDisplayName<TaskViewModel, string>(p => p.ChannelCode)));
                        }
                    }
                    //Phân loại KH
                    else if (field.FieldCode == RepositoryPropertyHelper.GetPropertyName<TaskViewModel, string>(p => p.CustomerClassCode))
                    {
                        if (string.IsNullOrEmpty(taskViewModel.CustomerClassCode))
                        {
                            errorMessageRequired.Add(string.Format(LanguageResource.Required, RepositoryPropertyHelper.GetDisplayName<TaskViewModel, string>(p => p.CustomerClassCode)));
                        }
                    }
                    //Nhắc nhở
                    else if (field.FieldCode == RepositoryPropertyHelper.GetPropertyName<TaskViewModel, bool?>(p => p.isRemind))
                    {
                        if (!taskViewModel.isRemind.HasValue)
                        {
                            errorMessageRequired.Add(string.Format(LanguageResource.Required, RepositoryPropertyHelper.GetDisplayName<TaskViewModel, bool?>(p => p.isRemind)));
                        }
                    }
                    //NV kinh doanh
                    else if (field.FieldCode == RepositoryPropertyHelper.GetPropertyName<TaskViewModel, string>(p => p.SalesSupervisorCode))
                    {
                        if (string.IsNullOrEmpty(taskViewModel.SalesSupervisorCode))
                        {
                            errorMessageRequired.Add(string.Format(LanguageResource.Required, RepositoryPropertyHelper.GetDisplayName<TaskViewModel, string>(p => p.SalesSupervisorCode)));
                        }
                    }


                }
            }
            return errorMessageRequired;
        }
        #endregion Check required field config 

        /// <summary>
        /// Create and update RemindTaskModel
        /// </summary>
        /// <param name="taskViewModel"></param>
        /// <param name="CurrentUser"></param>
        public void UpdateRemindTask(TaskViewModel taskViewModel, Guid? CurrentUser)
        {
            var result = _context.Database.ExecuteSqlCommand("EXEC [Task].[usp_UpdateRemindTask] @TaskId, @RemindTime, @RemindCycle, @RemindStartDate, @AccountId, @isRemind",
                new SqlParameter("@TaskId", taskViewModel.TaskId),
                new SqlParameter("@RemindTime", taskViewModel.RemindTime ?? (object)DBNull.Value),
                new SqlParameter("@RemindCycle", taskViewModel.RemindCycle ?? (object)DBNull.Value),
                new SqlParameter("@RemindStartDate", taskViewModel.RemindStartDate ?? (object)DBNull.Value),
                new SqlParameter("@AccountId", CurrentUser ?? (object)DBNull.Value),
                new SqlParameter("@isRemind", taskViewModel.isRemind ?? (object)DBNull.Value));
        }

        /// <summary>
        /// Tạo subtask nếu là phân công theo nhóm
        /// </summary>
        /// <param name="TaskId"></param>
        public void UpdateGroupAssignTask(Guid TaskId, List<TaskAssignViewModel> taskAssignPersonGroupList = null)
        {
            if (taskAssignPersonGroupList != null && taskAssignPersonGroupList.Count > 0)
            {
                #region taskAssignGroupList
                //Build your record
                var tableTaskAssignGroupSchema = new List<SqlMetaData>(1)
                {
                    new SqlMetaData("Code", SqlDbType.NVarChar, 100)
                }.ToArray();

                //And a table as a list of those records
                var tableTaskAssignGroup = new List<SqlDataRecord>();
                List<string> saleOrgLst = new List<string>();

                foreach (var r in taskAssignPersonGroupList)
                {
                    var tableRow = new SqlDataRecord(tableTaskAssignGroupSchema);
                    if (!string.IsNullOrEmpty(r.SalesEmployeeCode))
                    {
                        tableRow.SetString(0, r.SalesEmployeeCode);
                    }
                    tableTaskAssignGroup.Add(tableRow);
                }
                #endregion

                var result = _context.Database.ExecuteSqlCommand("EXEC [Task].[usp_UpdateGroupAssignTask] @TaskId, @taskAssignGroupList",
                    new SqlParameter("@TaskId", TaskId),
                    new SqlParameter
                    {
                        SqlDbType = SqlDbType.Structured,
                        Direction = ParameterDirection.Input,
                        ParameterName = "taskAssignGroupList",
                        TypeName = "[dbo].[StringList]", //Don't forget this one!
                        Value = tableTaskAssignGroup
                    });
            }
        }

        public void Update(TaskViewModel taskViewModel)
        {
            var taskInDb = _context.TaskModel.FirstOrDefault(p => p.TaskId == taskViewModel.TaskId);
            taskInDb.MapTask(taskViewModel);
            taskInDb.LastEditBy = taskViewModel.LastEditBy;
            taskInDb.LastEditTime = taskViewModel.LastEditTime;
            _context.Entry(taskInDb).State = EntityState.Modified;
        }

        public void CreateTaskAssign(TaskAssignViewModel viewModel)
        {
            TaskAssignModel assign = new TaskAssignModel();
            assign.TaskAssignId = Guid.NewGuid();
            assign.TaskId = viewModel.TaskId;
            assign.SalesEmployeeCode = viewModel.SalesEmployeeCode;
            assign.TaskAssignTypeCode = viewModel.TaskAssignTypeCode;
            assign.RolesCode = viewModel.RolesCode;
            assign.CreateBy = viewModel.CreateBy;
            assign.CreateTime = DateTime.Now;
            _context.Entry(assign).State = EntityState.Added;
        }

        public void UpdateTaskAssign(TaskAssignViewModel viewModel)
        {
            var assign = _context.TaskAssignModel.FirstOrDefault(p => p.TaskAssignId == viewModel.TaskAssignId);
            if (assign != null)
            {
                if (viewModel.SalesEmployeeCode != assign.SalesEmployeeCode || assign.TaskAssignTypeCode != viewModel.TaskAssignTypeCode)
                {
                    assign.SalesEmployeeCode = viewModel.SalesEmployeeCode;
                    assign.TaskAssignTypeCode = viewModel.TaskAssignTypeCode;
                    _context.Entry(assign).State = EntityState.Modified;
                }
            }
        }

        public void CreateTaskReporter(TaskReporterViewModel viewModel)
        {
            TaskReporterModel taskReporter = new TaskReporterModel();
            taskReporter.TaskReporterId = Guid.NewGuid();
            taskReporter.TaskId = viewModel.TaskId;
            taskReporter.SalesEmployeeCode = viewModel.SalesEmployeeCode;
            taskReporter.TaskAssignTypeCode = viewModel.TaskAssignTypeCode;
            taskReporter.CreateBy = viewModel.CreateBy;
            taskReporter.CreateTime = DateTime.Now;
            _context.Entry(taskReporter).State = EntityState.Added;
        }

        public void UpdateTaskReporter(TaskReporterViewModel viewModel)
        {
            var reporter = _context.TaskReporterModel.FirstOrDefault(p => p.TaskReporterId == viewModel.TaskReporterId);
            if (reporter != null)
            {
                if (viewModel.SalesEmployeeCode != reporter.SalesEmployeeCode)
                {
                    reporter.SalesEmployeeCode = viewModel.SalesEmployeeCode;
                    reporter.TaskAssignTypeCode = viewModel.TaskAssignTypeCode;
                    _context.Entry(reporter).State = EntityState.Modified;
                }
            }
        }

        public void CreateTaskReference(TaskReferenceModel model)
        {
            model.TaskReferenceId = Guid.NewGuid();
            model.CreateTime = DateTime.Now;
            _context.Entry(model).State = EntityState.Added;
        }

        public void CreateTaskContact(TaskContactModel model)
        {
            model.TaskContactId = Guid.NewGuid();
            model.CreateTime = DateTime.Now;
            _context.Entry(model).State = EntityState.Added;
        }

        /// <summary>
        /// Lấy danh sách bình luận của task
        /// </summary>
        /// <param name="TaskId"></param>
        /// <returns>list TaskCommentViewModel</returns>
        public List<TaskCommentViewModel> GetTaskCommentList(Guid TaskId)
        {
            var commentList = (from p in _context.TaskCommentModel
                                   //CreateBy
                               join create in _context.AccountModel on p.CreateBy equals create.AccountId
                               join cr in _context.SalesEmployeeModel on create.EmployeeCode equals cr.SalesEmployeeCode into crg
                               from cr1 in crg.DefaultIfEmpty()
                                   //EditBy
                               join modify in _context.AccountModel on p.LastEditBy equals modify.AccountId into mg
                               from m in mg.DefaultIfEmpty()
                               join md in _context.SalesEmployeeModel on m.EmployeeCode equals md.SalesEmployeeCode into mdg
                               from md1 in mdg.DefaultIfEmpty()

                               where p.TaskId == TaskId
                               && (p.Comment != null && p.Comment != "")
                               orderby p.CreateTime
                               select new TaskCommentViewModel
                               {
                                   TaskId = p.TaskId,
                                   TaskCommentId = p.TaskCommentId,
                                   Comment = p.Comment,
                                   ProcessUser = p.ProcessUser,
                                   FromStatusId = p.FromStatusId,
                                   ToStatusId = p.ToStatusId,
                                   CreateBy = p.CreateBy,
                                   CreateByName = cr1.SalesEmployeeName,
                                   CreateTime = p.CreateTime,
                                   LastEditBy = p.LastEditBy,
                                   LastEditByName = md1.SalesEmployeeName,
                                   LastEditTime = p.LastEditTime
                               }).ToList();

            if (commentList != null && commentList.Count > 0)
            {
                foreach (var item in commentList)
                {
                    item.LogoName = item.CreateByName.GetCharacterForLogoName();
                }
            }
            return commentList;
        }

        /// <summary>
        /// Lấy danh sách file đính kèm của task
        /// </summary>
        /// <param name="commentList"></param>
        /// <param name="TaskId"></param>
        /// <returns>list FileAttachmentViewModel</returns>
        public List<FileAttachmentViewModel> GetTaskFileList(List<TaskCommentViewModel> commentList, Guid TaskId)
        {
            List<FileAttachmentViewModel> taskFileList = new List<FileAttachmentViewModel>();
            if (commentList != null && commentList.Count > 0)
            {
                foreach (var item in commentList)
                {
                    var commentFiles = (from p in _context.FileAttachmentModel
                                        join m in _context.Comment_File_Mapping on p.FileAttachmentId equals m.FileAttachmentId
                                        where m.TaskCommentId == item.TaskCommentId
                                        && p.ObjectId == item.TaskCommentId
                                        select new FileAttachmentViewModel
                                        {
                                            FileAttachmentId = p.FileAttachmentId,
                                            ObjectId = p.ObjectId,
                                            FileAttachmentCode = p.FileAttachmentCode,
                                            FileAttachmentName = p.FileAttachmentName,
                                            FileExtention = p.FileExtention,
                                            FileUrl = p.FileUrl,
                                            CreateTime = p.CreateTime
                                        }).ToList();
                    taskFileList.AddRange(commentFiles);
                }
            }
            var taskFiles = (from p in _context.FileAttachmentModel
                             join m in _context.Task_File_Mapping on p.FileAttachmentId equals m.FileAttachmentId
                             where m.TaskId == TaskId
                             && p.ObjectId == TaskId
                             select new FileAttachmentViewModel
                             {
                                 FileAttachmentId = p.FileAttachmentId,
                                 ObjectId = p.ObjectId,
                                 FileAttachmentCode = p.FileAttachmentCode,
                                 FileAttachmentName = p.FileAttachmentName,
                                 FileExtention = p.FileExtention,
                                 FileUrl = p.FileUrl,
                                 CreateTime = p.CreateTime
                             }).ToList();
            taskFileList.AddRange(taskFiles);
            taskFileList = taskFileList.OrderBy(p => p.CreateTime).ToList();

            return taskFileList;
        }

        /// <summary>
        /// Lấy lịch sử cập nhật task
        /// </summary>
        /// <param name="TaskId"></param>
        /// <returns>List<TaskHistoryViewModel></returns>
        public List<TaskHistoryViewModel> GetTaskHistoryList(Guid TaskId)
        {
            var historyList = (from p in _context.ChangeDataLogModel
                               join a in _context.AccountModel on p.LastEditBy equals a.AccountId
                               join s in _context.SalesEmployeeModel on a.EmployeeCode equals s.SalesEmployeeCode
                               where p.PrimaryKey == TaskId
                               select new TaskHistoryViewModel
                               {
                                   FieldName = p.FieldName,
                                   OldData = p.OldData,
                                   NewData = p.NewData,
                                   LastEditBy = p.LastEditBy,
                                   LastEditByName = s.SalesEmployeeName,
                                   LastEditTime = p.LastEditTime
                               }).ToList();
            return historyList;
        }

        /// <summary>
        /// Lưu assignee (người được phân công)
        /// </summary>
        /// <param name="taskAssignList"></param>
        /// <param name="TaskId"></param>
        /// <param name="CurrentUserId"></param>
        /// <returns>List<AssigneeNotificationMessageViewModel></returns>
        #region Save task assignee
        public void SaveTaskAssignee(List<TaskAssignViewModel> taskAssignList, Guid TaskId, Guid? CurrentUserId)
        {
            if (taskAssignList == null)
            {
                taskAssignList = new List<TaskAssignViewModel>();
            }
            //Danh sách assignee từ view
            var assignIdList = taskAssignList.Where(p => p.TaskAssignId != null).Select(p => p.TaskAssignId).ToList();
            //Danh sách assignee từ db
            var existAssignList = _context.TaskAssignModel.Where(p => p.TaskId == TaskId)
                                          .Select(p => p.TaskAssignId).ToList();

            foreach (var item in existAssignList)
            {
                //Nếu có trong db nhưng ko có trong view => Xoá
                if (!assignIdList.Contains(item))
                {
                    var delItem = _context.TaskAssignModel.FirstOrDefault(p => p.TaskAssignId == item);
                    if (delItem != null)
                    {
                        _context.Entry(delItem).State = EntityState.Deleted;
                    }
                }
            }

            if (taskAssignList != null && taskAssignList.Count > 0)
            {
                foreach (var item in taskAssignList)
                {
                    if (item.SalesEmployeeCode != null)
                    {
                        //Nếu có Id => Sửa
                        if (item.TaskAssignId != null && item.TaskAssignId != Guid.Empty)
                        {
                            UpdateTaskAssign(item);

                        }
                        //Nếu không có Id => Thêm
                        else
                        {
                            item.TaskId = TaskId;
                            item.CreateBy = CurrentUserId;
                            CreateTaskAssign(item);
                        }
                    }
                    else
                    {
                        var delItem = _context.TaskAssignModel.FirstOrDefault(p => p.TaskAssignId == item.TaskAssignId);
                        if (delItem != null)
                        {
                            _context.Entry(delItem).State = EntityState.Deleted;
                        }
                    }
                }
            }
        }
        #endregion

        /// <summary>
        /// Lưu nhóm được phân công (nếu thêm mới chưa lưu, lưu ở cập nhật)
        /// Sau khi lưu, thêm mới subtask phân công cho nhân viên trong nhóm
        /// Chỉ tạo subtask khi chọn "Làm riêng"
        /// </summary>
        /// <param name="taskAssignList"></param>
        /// <param name="TaskId"></param>
        /// <param name="CurrentUserId"></param>
        public void SaveTaskAssignGroup(List<TaskAssignViewModel> taskAssignList, Guid TaskId, Guid? CurrentUserId, bool? IsTogether = false)
        {
            if (IsTogether == false || IsTogether == null)
            {
                var existTaskAssign = _context.TaskAssignModel.Where(p => p.TaskId == TaskId).ToList();
                if (existTaskAssign == null || existTaskAssign.Count == 0)
                {
                    if (taskAssignList != null && taskAssignList.Count > 0)
                    {
                        foreach (var item in taskAssignList)
                        {
                            if (!string.IsNullOrEmpty(item.RolesCode))
                            {
                                item.TaskId = TaskId;
                                item.CreateBy = CurrentUserId;
                                CreateTaskAssign(item);

                                //Save task reference
                                TaskReferenceModel referenceAssignee = new TaskReferenceModel();
                                referenceAssignee.TaskId = TaskId;
                                referenceAssignee.CreateBy = CurrentUserId;
                                referenceAssignee.RolesCode = item.RolesCode;
                                referenceAssignee.Type = ConstTaskReference.Group;
                                CreateTaskReference(referenceAssignee);
                            }
                        }
                    }
                }
            }
            else
            {
                if (taskAssignList == null)
                {
                    taskAssignList = new List<TaskAssignViewModel>();
                }
                //Danh sách assignee từ view
                var assignIdList = taskAssignList.Where(p => p.TaskAssignId != null).Select(p => p.TaskAssignId).ToList();
                //Danh sách assignee từ db
                var existAssignList = _context.TaskAssignModel.Where(p => p.TaskId == TaskId)
                                              .Select(p => p.TaskAssignId).ToList();

                foreach (var item in existAssignList)
                {
                    //Nếu có trong db nhưng ko có trong view => Xoá
                    if (!assignIdList.Contains(item))
                    {
                        var delItem = _context.TaskAssignModel.FirstOrDefault(p => p.TaskAssignId == item);
                        if (delItem != null)
                        {
                            _context.Entry(delItem).State = EntityState.Deleted;
                        }
                    }
                }

                if (taskAssignList != null && taskAssignList.Count > 0)
                {
                    foreach (var item in taskAssignList)
                    {
                        if (item.SalesEmployeeCode != null)
                        {
                            //Nếu có Id => Sửa
                            if (item.TaskAssignId != null && item.TaskAssignId != Guid.Empty)
                            {
                                item.TaskAssignTypeCode = ConstTaskReference.Group;
                                UpdateTaskAssign(item);

                            }
                            //Nếu không có Id => Thêm
                            else
                            {
                                item.TaskId = TaskId;
                                item.CreateBy = CurrentUserId;
                                item.TaskAssignTypeCode = ConstTaskReference.Group;
                                CreateTaskAssign(item);
                            }
                        }
                        else
                        {
                            var delItem = _context.TaskAssignModel.FirstOrDefault(p => p.TaskAssignId == item.TaskAssignId);
                            if (delItem != null)
                            {
                                _context.Entry(delItem).State = EntityState.Deleted;
                            }
                        }
                    }
                }
            }

        }

        /// <summary>
        /// Lưu reporter (người theo dõi/giám sát)
        /// </summary>
        /// <param name="taskReporterList"></param>
        /// <param name="TaskId"></param>
        /// <param name="CurrentUserId"></param>
        /// <returns></returns>
        #region Save task reporter
        public void SaveTaskReporter(List<TaskReporterViewModel> taskReporterList, Guid TaskId, Guid? CurrentUserId)
        {
            if (taskReporterList != null && taskReporterList.Count > 0)
            {
                //Danh sách reporter từ view
                var reporterIdList = taskReporterList.Where(p => p.TaskReporterId != null).Select(p => p.TaskReporterId).ToList();
                //Danh sách reporter từ db
                var existReporterList = _context.TaskReporterModel.Where(p => p.TaskId == TaskId)
                                              .Select(p => p.TaskReporterId).ToList();

                foreach (var item in existReporterList)
                {
                    //Nếu có trong db nhưng ko có trong view => Xoá
                    if (!reporterIdList.Contains(item))
                    {
                        var delItem = _context.TaskReporterModel.FirstOrDefault(p => p.TaskReporterId == item);
                        if (delItem != null)
                        {
                            _context.Entry(delItem).State = EntityState.Deleted;
                        }
                    }
                }

                foreach (var item in taskReporterList)
                {
                    if (item.SalesEmployeeCode != null)
                    {
                        //Nếu có Id => Sửa
                        if (item.TaskReporterId != null && item.TaskReporterId != Guid.Empty)
                        {
                            UpdateTaskReporter(item);
                        }
                        //Nếu không có Id => Thêm
                        else
                        {
                            item.TaskId = TaskId;
                            item.CreateBy = CurrentUserId;
                            CreateTaskReporter(item);
                        }
                    }
                    else
                    {
                        var delItem = _context.TaskReporterModel.FirstOrDefault(p => p.TaskReporterId == item.TaskReporterId);
                        if (delItem != null)
                        {
                            _context.Entry(delItem).State = EntityState.Deleted;
                        }
                    }
                }
            }
            else
            {
                //Danh sách reporter từ db
                var existReporterList = _context.TaskReporterModel.Where(p => p.TaskId == TaskId)
                                              .Select(p => p.TaskReporterId).ToList();
                if (existReporterList != null && existReporterList.Count > 0)
                {
                    foreach (var item in existReporterList)
                    {
                        var delItem = _context.TaskReporterModel.FirstOrDefault(p => p.TaskReporterId == item);
                        if (delItem != null)
                        {
                            _context.Entry(delItem).State = EntityState.Deleted;
                        }
                    }
                }
            }
        }
        #endregion

        /// <summary>
        /// Truy xuất số lượng các task theo status
        /// </summary>
        /// <param name="TaskStatus"></param>
        /// <param name="EmployeeCode"></param>
        /// <returns>int</returns>
        public int GetCountOfTask(string TaskStatus, string EmployeeCode)
        {
            #region Count task (not use)
            //switch (TaskStatus)
            //{
            //    //Todo: Cần làm
            //    case string w when w == ConstTaskStatus.Todo:
            //        return (from t in _context.TaskModel
            //                join status in _context.TaskStatusModel on t.TaskStatusId equals status.TaskStatusId
            //                join workflow in _context.WorkFlowModel on t.WorkFlowId equals workflow.WorkFlowId
            //                //assign
            //                join assign in _context.TaskAssignModel on t.TaskId equals assign.TaskId
            //                where
            //                    status.ProcessCode == ConstProcess.todo
            //                    // Của mình
            //                    && (t.Reporter == EmployeeCode || assign.SalesEmployeeCode == EmployeeCode)
            //                select 1).Count();

            //    //Processing: Đang thực hiện
            //    case string w when w == ConstTaskStatus.Processing:
            //        return (from t in _context.TaskModel
            //                join status in _context.TaskStatusModel on t.TaskStatusId equals status.TaskStatusId
            //                join workflow in _context.WorkFlowModel on t.WorkFlowId equals workflow.WorkFlowId
            //                //assign
            //                join assign in _context.TaskAssignModel on t.TaskId equals assign.TaskId
            //                where
            //                    status.ProcessCode == ConstProcess.processing
            //                    // Của mình
            //                    && (t.Reporter == EmployeeCode || assign.SalesEmployeeCode == EmployeeCode)
            //                    // Yêu cầu của KH
            //                    && workflow.WorkflowCategoryCode == ConstWorkFlowCategory.TICKET
            //                select 1).Count();
            //    //Completed: Đã hoàn thành
            //    case string w when w == ConstTaskStatus.Completed:
            //        return (from t in _context.TaskModel
            //                join status in _context.TaskStatusModel on t.TaskStatusId equals status.TaskStatusId
            //                join workflow in _context.WorkFlowModel on t.WorkFlowId equals workflow.WorkFlowId
            //                //assign
            //                join assign in _context.TaskAssignModel on t.TaskId equals assign.TaskId
            //                where
            //                    status.ProcessCode == ConstProcess.completed
            //                    // Của mình
            //                    && (t.Reporter == EmployeeCode || assign.SalesEmployeeCode == EmployeeCode)
            //                    // Yêu cầu của KH
            //                    && workflow.WorkflowCategoryCode == ConstWorkFlowCategory.TICKET
            //                select 1).Count();
            //    case string w when w == ConstTaskStatus.Incomplete:
            //        return (from t in _context.TaskModel
            //                join status in _context.TaskStatusModel on t.TaskStatusId equals status.TaskStatusId
            //                join workflow in _context.WorkFlowModel on t.WorkFlowId equals workflow.WorkFlowId
            //                //assign
            //                join assign in _context.TaskAssignModel on t.TaskId equals assign.TaskId
            //                where
            //                    //!= Completed: Chưa hoàn thành
            //                    status.ProcessCode != ConstProcess.completed
            //                    // Của mình
            //                    && (t.Reporter == EmployeeCode || assign.SalesEmployeeCode == EmployeeCode)
            //                    // Yêu cầu của KH
            //                    && workflow.WorkflowCategoryCode == ConstWorkFlowCategory.TICKET
            //                select 1).Count();
            //    case string w when w == ConstTaskStatus.Unassign:
            //        return (from t in _context.TaskModel
            //                join status in _context.TaskStatusModel on t.TaskStatusId equals status.TaskStatusId
            //                join workflow in _context.WorkFlowModel on t.WorkFlowId equals workflow.WorkFlowId
            //                //assign
            //                join assign in _context.TaskAssignModel on t.TaskId equals assign.TaskId into lst1
            //                from ass in lst1.DefaultIfEmpty()
            //                where
            //                    //!= Completed: Chưa hoàn thành
            //                    status.ProcessCode != ConstProcess.completed
            //                    // Của mình
            //                    && ass == null
            //                    // Yêu cầu của KH
            //                    && workflow.WorkflowCategoryCode == ConstWorkFlowCategory.TICKET
            //                select 1).Count();
            //    case string w when w == ConstTaskStatus.Follow:
            //        return (from t in _context.TaskModel
            //                join status in _context.TaskStatusModel on t.TaskStatusId equals status.TaskStatusId
            //                join workflow in _context.WorkFlowModel on t.WorkFlowId equals workflow.WorkFlowId
            //                where
            //                    //!= Completed: Chưa hoàn thành
            //                    status.ProcessCode != ConstProcess.completed
            //                    // Đang theo dõi
            //                    && t.Reporter == EmployeeCode
            //                    // Yêu cầu của KH
            //                    && workflow.WorkflowCategoryCode == ConstWorkFlowCategory.TICKET
            //                select 1).Count();
            //    //Tất cả
            //    case string w when w == ConstTaskStatus.All:
            //        return (from t in _context.TaskModel
            //                join status in _context.TaskStatusModel on t.TaskStatusId equals status.TaskStatusId
            //                join workflow in _context.WorkFlowModel on t.WorkFlowId equals workflow.WorkFlowId
            //                where workflow.WorkflowCategoryCode == ConstWorkFlowCategory.TICKET
            //                select 1).Count();
            //    default:
            //        break;
            //}
            #endregion

            List<string> processCodeList = new List<string>();

            TaskSearchViewModel searchModel = new TaskSearchViewModel();
            searchModel.TaskProcessCode = TaskStatus;
            if (TaskStatus != ConstTaskStatus.All)
            {
                if (TaskStatus == ConstTaskStatus.Todo || TaskStatus == ConstTaskStatus.Processing || TaskStatus == ConstTaskStatus.Incomplete || TaskStatus == ConstTaskStatus.Completed || TaskStatus == ConstTaskStatus.Expired)
                {
                    searchModel.Assignee = EmployeeCode;
                    //searchModel.Reporter = EmployeeCode;
                }
                if (TaskStatus == ConstTaskStatus.Follow)
                {
                    //searchModel.Reporter = EmployeeCode;
                }
                if (TaskStatus == ConstTaskStatus.Unassign)
                {
                    //searchModel.Reporter = EmployeeCode;
                    searchModel.isUnassign = true;
                }
            }
            else
            {
                processCodeList.Add(ConstTaskStatus.Todo);
                processCodeList.Add(ConstTaskStatus.Processing);
                processCodeList.Add(ConstTaskStatus.Incomplete);
                //processCodeList.Add(ConstTaskStatus.CompletedOnTime);
                //processCodeList.Add(ConstTaskStatus.CompletedExpire);
                processCodeList.Add(ConstTaskStatus.Expired);
            }

            #region Task Process
            var TaskProcessCode_Todo = false;
            var TaskProcessCode_Processing = false;
            var TaskProcessCode_Incomplete = false;
            var TaskProcessCode_CompletedOnTime = false;
            var TaskProcessCode_CompletedExpire = false;
            var TaskProcessCode_Expired = false;

            if (processCodeList != null && processCodeList.Count > 0)
            {
                foreach (var item in processCodeList)
                {
                    //Việc cần làm
                    if (item == ConstTaskStatus.Todo)
                    {
                        TaskProcessCode_Todo = true;
                    }
                    //Đang thực hiện
                    if (item == ConstTaskStatus.Processing)
                    {
                        TaskProcessCode_Processing = true;
                    }
                    //Chưa hoàn thành
                    if (item == ConstTaskStatus.Incomplete)
                    {
                        TaskProcessCode_Incomplete = true;
                    }
                    //Hoàn thành đúng hạn
                    if (item == ConstTaskStatus.CompletedOnTime)
                    {
                        TaskProcessCode_CompletedOnTime = true;
                    }
                    //Hoàn thành quá hạn
                    if (item == ConstTaskStatus.CompletedExpire)
                    {
                        TaskProcessCode_CompletedExpire = true;
                    }
                    //Quá hạn
                    if (item == ConstTaskStatus.Expired)
                    {
                        TaskProcessCode_Expired = true;
                    }
                }
            }
            else
            {
                //Việc cần làm
                if (searchModel.TaskProcessCode == ConstTaskStatus.Todo)
                {
                    TaskProcessCode_Todo = true;
                }
                //Đang thực hiện
                if (searchModel.TaskProcessCode == ConstTaskStatus.Processing)
                {
                    TaskProcessCode_Processing = true;
                }
                //Chưa hoàn thành
                if (searchModel.TaskProcessCode == ConstTaskStatus.Incomplete)
                {
                    TaskProcessCode_Incomplete = true;
                }
                //Hoàn thành đúng hạn
                if (searchModel.TaskProcessCode == ConstTaskStatus.CompletedOnTime)
                {
                    TaskProcessCode_CompletedOnTime = true;
                }
                //Hoàn thành quá hạn
                if (searchModel.TaskProcessCode == ConstTaskStatus.CompletedExpire)
                {
                    TaskProcessCode_CompletedExpire = true;
                }
                //Hoàn thành
                if (searchModel.TaskProcessCode == ConstTaskStatus.Completed)
                {
                    TaskProcessCode_CompletedOnTime = true;
                    TaskProcessCode_CompletedExpire = true;
                }
                //Quá hạn
                if (searchModel.TaskProcessCode == ConstTaskStatus.Expired)
                {
                    TaskProcessCode_Expired = true;
                }
            }
            #endregion

            string sqlQuery = "EXEC [Task].[usp_CountTask] @TaskProcessCode, @Assignee, @TaskProcessCode_Todo, @TaskProcessCode_Processing, @TaskProcessCode_Incomplete, @TaskProcessCode_CompletedOnTime, @TaskProcessCode_CompletedExpire, @TaskProcessCode_Expired, @isUnassign";

            #region Parameters
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                 new SqlParameter
                {
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input,
                    ParameterName = "TaskProcessCode",
                    Value = searchModel.TaskProcessCode ?? (object)DBNull.Value,
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input,
                    ParameterName = "Assignee",
                    Value = searchModel.Assignee ?? (object)DBNull.Value,
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.Bit,
                    Direction = ParameterDirection.Input,
                    ParameterName = "TaskProcessCode_Todo",
                    Value = TaskProcessCode_Todo,
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.Bit,
                    Direction = ParameterDirection.Input,
                    ParameterName = "TaskProcessCode_Processing",
                    Value = TaskProcessCode_Processing,
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.Bit,
                    Direction = ParameterDirection.Input,
                    ParameterName = "TaskProcessCode_Incomplete",
                    Value = TaskProcessCode_Incomplete,
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.Bit,
                    Direction = ParameterDirection.Input,
                    ParameterName = "TaskProcessCode_CompletedOnTime",
                    Value = TaskProcessCode_CompletedOnTime,
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.Bit,
                    Direction = ParameterDirection.Input,
                    ParameterName = "TaskProcessCode_CompletedExpire",
                    Value = TaskProcessCode_CompletedExpire,
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.Bit,
                    Direction = ParameterDirection.Input,
                    ParameterName = "TaskProcessCode_Expired",
                    Value = TaskProcessCode_Expired,
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.Bit,
                    Direction = ParameterDirection.Input,
                    ParameterName = "isUnassign",
                    Value = searchModel.isUnassign ?? (object)DBNull.Value,
                },
            };
            #endregion

            var res = _context.Database.SqlQuery<int>(sqlQuery, parameters.ToArray()).FirstOrDefault();
            return res;
        }

        #region BC số lượng GVL theo thời gian
        /// <summary>
        ///Báo cáo số lượng góc vật liệu theo thời gian
        /// </summary>
        /// <param name="FromDate"></param>
        /// <param name="ToDate"></param>
        /// <returns></returns>
        public List<TaskGTBQuantityReportViewModel> TaskGTBQuantityReport(DateTime? FromDate, DateTime? ToDate)
        {
            string sqlQuery = "EXEC [Task].[usp_TaskGTBQuantityReport] @FromDate, @ToDate";

            #region Parameters
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
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
                }
            };
            #endregion

            var result = _context.Database.SqlQuery<TaskGTBQuantityReportViewModel>(sqlQuery, parameters.ToArray()).ToList();
            return result;
        }
        #endregion

        #region Push Notification
        public void PushNotification(Guid? TaskId, string notificationMessage, string[] deviceLst, string taskCode, List<Guid> accountLst, DateTime? ScheduleTime = null)
        {
            string message = string.Empty;

            var _utilitiesRepository = new UtilitiesRepository();
            Guid notificationId = Guid.NewGuid();
            if (!string.IsNullOrEmpty(notificationMessage) && notificationMessage.Length > 160)
            {
                notificationMessage = notificationMessage.Substring(0, 155) + "...";
            }
            _utilitiesRepository.PushNotification(deviceLst, out message, taskCode, notificationMessage, new { openAction = TaskId, notificationId }, ScheduleTime);

            //save notification in db
            if (string.IsNullOrEmpty(message))
            {
                NotificationModel notif = new NotificationModel();
                notif.NotificationId = notificationId;
                notif.TaskId = TaskId;
                notif.Title = taskCode;
                notif.Description = notificationMessage;
                notif.Detail = notificationMessage;
                notif.CreatedDate = DateTime.Now;

                _context.Entry(notif).State = EntityState.Added;

                if (accountLst != null && accountLst.Count > 0)
                {
                    foreach (var acc in accountLst)
                    {
                        var existNotif = _context.NotificationAccountMappingModel
                                                 .Where(p => p.AccountId == acc && p.NotificationId == notif.NotificationId && p.IsRead == false)
                                                 .FirstOrDefault();
                        if (existNotif != null)
                        {
                            _context.Entry(existNotif).State = EntityState.Deleted;
                        }
                        NotificationAccountMappingModel mapping = new NotificationAccountMappingModel();
                        mapping.AccountId = acc;
                        mapping.NotificationId = notif.NotificationId;
                        mapping.IsRead = false;

                        _context.Entry(mapping).State = EntityState.Added;
                    }
                }
            }

            _context.SaveChanges();
        }
        #endregion Push Notification

        #region Credit
        public DataTable GetUserList()
        {
            //Khởi tạo thư viện và kết nối
            var _sap = new SAPRepository();
            var destination = _sap.GetRfcWithConfig();
            //Định nghĩa hàm cần gọi
            var function = destination.Repository.CreateFunction(ConstantFunctionName.YAC_FM_CRM_LIST_USER);

            //Thông số truyền vào

            //Thực thi
            function.Invoke(destination);

            var datatable = function.GetTable("USER_T").ToDataTable("USER_T");
            return datatable;
        }

        public DataTable GetCreditList(string CompanyCode, string Type)
        {
            //Khởi tạo thư viện và kết nối
            var _sap = new SAPRepository();
            var destination = _sap.GetRfcWithConfig();
            //Định nghĩa hàm cần gọi
            var function = destination.Repository.CreateFunction(ConstantFunctionName.YAC_FM_CRM_GET_CREDITLIST);

            //Thông số truyền vào
            function.SetValue("IM_WERKS", CompanyCode);
            function.SetValue("IM_TYPE", Type);

            //Thực thi
            function.Invoke(destination);

            var datatable = function.GetTable("LIST_T").ToDataTable("LIST_T");
            return datatable;
        }

        public DataTable CheckLogin(CreditAuthenticateViewModel auth)
        {
            //Khởi tạo thư viện và kết nối
            var _sap = new SAPRepository();
            var destination = _sap.GetRfcWithConfig();
            //Định nghĩa hàm cần gọi
            var function = destination.Repository.CreateFunction(ConstantFunctionName.YAC_FM_CRM_CHECK_LOGIN);

            //Thông số truyền vào
            //1. Cấp duyệt từ danh mục 4.Cấp duyệt.  Chuyển về UPPER(…) khi truyền vào hàm
            function.SetValue("IM_CAPDUYET", auth.CapDuyet.ToUpper());
            //2. Mã User login
            function.SetValue("IM_USERNO", auth.UserNo);
            //3. Mật khẩu. Chuyển về UPPER(…) khi truyền vào hàm
            function.SetValue("IM_PASS", auth.Password);

            //Thực thi
            function.Invoke(destination);

            var datatable = function.GetTable("USER_T").ToDataTable("USER_T");
            return datatable;
        }

        public DataTable GetCreditLimit(CreditLimitSearchViewModel searchViewModel)
        {
            //Khởi tạo thư viện và kết nối
            var _sap = new SAPRepository();
            var destination = _sap.GetRfcWithConfig();
            //Định nghĩa hàm cần gọi
            var function = destination.Repository.CreateFunction(ConstantFunctionName.YAC_FM_CRM_GET_CREDITLIMIT);

            //Thông số truyền vào
            //Required
            function.SetValue("IM_WERKS", searchViewModel.CompanyCode);
            function.SetValue("IM_NAM_TRUOC", searchViewModel.NamTruoc);
            function.SetValue("IM_NAM_HIENTAI", searchViewModel.NamHienTai);
            function.SetValue("IM_CAPDUYET", searchViewModel.CapDuyet);
            function.SetValue("IM_DUYET", searchViewModel.Duyet);
            //Optional
            function.SetValue("IM_DEPTNO", searchViewModel.PhongBan);
            function.SetValue("IM_KUNNR", searchViewModel.MaSAPKH);
            function.SetValue("IM_SALEEMP", searchViewModel.NVSale);
            function.SetValue("IM_CNOEMP", searchViewModel.NVCongNo);
            function.SetValue("IM_GETROW", searchViewModel.GetRow);
            function.SetValue("IM_USERNO", searchViewModel.UserNo);

            //Thực thi
            function.Invoke(destination);

            var datatable = function.GetTable("CREDIT_T").ToDataTable("CREDIT_T");
            return datatable;
        }

        public DataTable SetCreditLimit(CreditLimitFormViewModel viewModel)
        {
            //Khởi tạo thư viện và kết nối
            var _sap = new SAPRepository();
            var destination = _sap.GetRfcWithConfig();
            //Định nghĩa hàm cần gọi
            var function = destination.Repository.CreateFunction(ConstantFunctionName.YAC_FM_CRM_SET_CREDITLIMIT);

            //Thông số truyền vào
            //Required
            function.SetValue("IM_CAPDUYET", viewModel.CapDuyet);
            function.SetValue("IM_WERKS", viewModel.MaCongTy);
            function.SetValue("IM_KUNNR", viewModel.MaKhachHang);
            function.SetValue("IM_NAM", viewModel.NamHanMuc);
            function.SetValue("IM_PERIOD", viewModel.KyHanMuc);
            function.SetValue("IM_IDNO", viewModel.NaturalNumber);
            function.SetValue("IM_TYPE", viewModel.FormType);
            function.SetValue("IM_STATUS", viewModel.Status);
            function.SetValue("IM_NOTE", viewModel.Note);
            //Cấp CONGNO lấy field: CNOXEPLOAI ngược lại các cấp khác lấy field: XEPLOAI
            string XepLoai = string.Empty;
            if (viewModel.CapDuyet == "CONGNO")
            {
                XepLoai = viewModel.CNOXepLoai;
            }
            else
            {
                XepLoai = viewModel.SALEXepLoai;
            }
            function.SetValue("IM_XEPLOAI", XepLoai);
            function.SetValue("IM_USERNO", viewModel.UserNo);
            function.SetValue("IM_DANHGIAKH", viewModel.DanhGiaKH);
            function.SetValue("IM_NOTETHOIHAN", viewModel.GhiChuThoiHan2);

            //Thực thi
            function.Invoke(destination);

            var datatable = function.GetTable("THONGBAO_T").ToDataTable("THONGBAO_T");
            //var datatable = new DataTable();
            return datatable;
        }

        public void SendMailCreditLimit(string EmailContent, string Subject, string FromEmail, string ToMail, string EmailTemplateType)
        {
            //GET email account
            //EmailAccountModel emailAccount = _context.EmailAccountModel.Where(s => s.SenderName == "Gỗ An Cường" && s.IsSender == true).FirstOrDefault();
            ////get mail server provider
            //MailServerProviderModel provider = _context.MailServerProviderModel.Where(s => s.Id == emailAccount.ServerProviderId).FirstOrDefault();
            //var emailConfigLst = _context.CatalogModel.Where(p => p.CatalogTypeCode == ConstCatalogType.FeedbackEmailConfig).ToList();
            var emailConfig = new EmailTemplateConfigRepository(_context).GetByType(EmailTemplateType);
            //FromEmail
            //string FromEmail = emailConfig.FromEmail;
            // get email config
            //string FromEmail = ConstEmail.FromEmail;
            //string FromEmailPassword = ConstEmail.FromEmailPassword;
            //string Host = ConstEmail.Host;
            //int Port = 587;
            MailMessage email = new MailMessage();
            email.From = new MailAddress(FromEmail);
            email.Sender = new MailAddress(FromEmail);
            List<string> toEmailList = ToMail.Split(';').ToList();
            foreach (var toEmail in toEmailList.Distinct())
            {
                if (!string.IsNullOrEmpty(toEmail))
                {
                    email.To.Add(new MailAddress(toEmail.Trim()));
                }
            }
            //email.CC.Add(new MailAddress(FromMail.Trim()));
            email.Body = EmailContent;
            email.IsBodyHtml = true;
            email.BodyEncoding = Encoding.UTF8;
            email.Subject = Subject;

            string message = "";
            using (var smtp = new SmtpClient())
            {
                smtp.Host = emailConfig.EmailHost;
                smtp.Port = emailConfig.EmailPort.Value;
                smtp.EnableSsl = emailConfig.EmailEnableSsl ?? false;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential(emailConfig.EmailAccount, emailConfig.EmailPassword);
                try
                {
                    smtp.Send(email);
                }
                catch (SmtpException ex)
                {
                    message = ex.Message;
                    if (ex.InnerException != null)
                    {
                        if (ex.InnerException.InnerException != null)
                        {
                            message = ex.InnerException.InnerException.Message;
                        }
                        else
                        {
                            message = ex.InnerException.Message;
                        }
                    }
                }
                catch (Exception ex)
                {
                    message = ex.Message;
                    if (ex.InnerException != null)
                    {
                        if (ex.InnerException.InnerException != null)
                        {
                            message = ex.InnerException.InnerException.Message;
                        }
                        else
                        {
                            message = ex.InnerException.Message;
                        }
                    }
                }
            }
        }

        public DataTable GetDanhGia(string CompanyCode)
        {
            //Khởi tạo thư viện và kết nối
            var _sap = new SAPRepository();
            var destination = _sap.GetRfcWithConfig();
            //Định nghĩa hàm cần gọi
            var function = destination.Repository.CreateFunction(ConstantFunctionName.YAC_FM_CRM_GET_DANH_GIA);

            //Thông số truyền vào
            //Required
            function.SetValue("IM_WERKS", CompanyCode);

            //Thực thi
            function.Invoke(destination);

            var datatable = function.GetTable("LIST_T").ToDataTable("LIST_T");
            return datatable;
        }
        #endregion

        #region GetNearByAccountBy
        public List<ProfileViewModel> GetNearByAccountBy(Guid? ProfileId, string Address)
        {
            var result = new List<ProfileViewModel>();
            //1. Check dữ liệu KH hiện tại đã có chưa => có thì lấy địa chỉ => tìm trong thăm hỏi có lat, long
            //2. Lấy thông tin cấu hình số khoảng cách để tìm => VD: tìm trong khoảng 2km
            //3. Lấy trước danh sách các thăm hỏi có lat, long
            //4. Tính khoảng cách dựa vào lat long => lọc lại lần nữa danh sách các KH thỏa điều kiện gần KH hiện tại trong vòng 2km

            //=======================================================================================================================

            //1
            var profile = _context.ProfileModel.Where(p => p.ProfileId == ProfileId).FirstOrDefault();
            if (profile != null)
            {
                var task = _context.TaskModel.Where(p => p.ProfileId == ProfileId && p.VisitAddress.Contains(Address)).OrderByDescending(p => p.TaskCode).FirstOrDefault();
                if (task != null)
                {
                    if (!string.IsNullOrEmpty(task.lat) && !string.IsNullOrEmpty(task.lng))
                    {
                        double currentLat = Convert.ToDouble(task.lat);
                        double currentLng = Convert.ToDouble(task.lng);
                        //2
                        var distance = _context.ResourceModel.Where(p => p.ResourceKey == "NearByDistance").Select(p => p.ResourceValue).FirstOrDefault();
                        if (distance != null)
                        {
                            //3 
                            var workFlowTHKH = _context.WorkFlowModel.Where(p => p.WorkflowCategoryCode == ConstWorkFlowCategory.THKH).Select(p => p.WorkFlowId).ToList();
                            var latlongList = _context.TaskModel.Where(p => p.ProfileId != ProfileId &&
                                                                            workFlowTHKH.Contains(p.WorkFlowId) &&
                                                                            (p.lat != null && p.lat != string.Empty && p.lat != "undefined" && p.lat != "null") &&
                                                                            (p.lng != null && p.lng != string.Empty && p.lng != "undefined" && p.lng != "null"))
                                                                .Select(p => new { p.lat, p.lng, p.ProfileId, p.VisitAddress })
                                                                .Distinct()
                                                                .ToList();
                            if (latlongList != null)
                            {
                                //4
                                foreach (var item in latlongList)
                                {
                                    double lat = Convert.ToDouble(item.lat);
                                    double lng = Convert.ToDouble(item.lng);

                                    var distanceTo = DistanceTo(currentLat, currentLng, lat, lng);
                                    if (distanceTo < Convert.ToDouble(distance))
                                    {
                                        var nearByProfile = (from p in _context.ProfileModel
                                                                 //join p1 in _context.ProvinceModel on p.ProvinceId equals p1.ProvinceId into TmpList1
                                                                 //from prov in TmpList1.DefaultIfEmpty()
                                                                 //join p2 in _context.DistrictModel on p.DistrictId equals p2.DistrictId into TmpList2
                                                                 //from dist in TmpList2.DefaultIfEmpty()
                                                                 //join p3 in _context.WardModel on p.WardId equals p3.WardId into TmpList3
                                                                 //from ward in TmpList3.DefaultIfEmpty()
                                                             where p.ProfileId == item.ProfileId
                                                             select new ProfileViewModel()
                                                             {
                                                                 ProfileId = p.ProfileId,
                                                                 ProfileCode = p.ProfileCode,
                                                                 ProfileForeignCode = p.ProfileForeignCode,
                                                                 ProfileName = p.ProfileName,
                                                                 Address = item.VisitAddress,
                                                             }).FirstOrDefault();
                                        if (nearByProfile != null)
                                        {
                                            bool containsItem = result.Any(r => r.ProfileId == nearByProfile.ProfileId && r.Address == nearByProfile.Address);
                                            if (containsItem == false)
                                            {
                                                result.Add(nearByProfile);
                                            }
                                        }

                                    }
                                }
                            }
                        }
                    }
                }
            }

            return result;
        }

        public static double DistanceTo(double lat1, double lon1, double lat2, double lon2, char unit = 'm')
        {
            double rlat1 = Math.PI * lat1 / 180;
            double rlat2 = Math.PI * lat2 / 180;
            double theta = lon1 - lon2;
            double rtheta = Math.PI * theta / 180;
            double dist =
                Math.Sin(rlat1) * Math.Sin(rlat2) + Math.Cos(rlat1) *
                Math.Cos(rlat2) * Math.Cos(rtheta);
            dist = Math.Acos(dist);
            dist = dist * 180 / Math.PI;
            dist = dist * 60 * 1.1515;

            switch (unit)
            {
                case 'm': //Met
                    return dist * 1000 * 1.609344;
                case 'K': //Kilometers -> default
                    return dist * 1.609344;
                case 'N': //Nautical Miles 
                    return dist * 0.8684;
                case 'M': //Miles
                    return dist;
            }

            return dist;
        }
        #endregion
    }
}
