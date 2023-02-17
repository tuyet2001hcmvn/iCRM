using ISD.Constant;
using ISD.Core;
using ISD.ViewModels;
using System.Web.Mvc;
using System.Linq;
using System;
using System.Collections.Generic;
using ISD.Resources;
using System.Data.SqlClient;
using System.Data;
using System.Net;
using System.Data.Entity;

namespace Work.Controllers
{
    public class CalendarController : BaseController
    {
        // GET: Calendar
        #region Index
        public ActionResult Index()
        {
            var Type = ConstWorkFlowCategory.MyCalendar;
            //Kanban
            var kanban = _context.KanbanModel.Where(p => p.KanbanCode == Type).FirstOrDefault();
            if (kanban != null)
            {
                ViewBag.KanbanId = kanban.KanbanId;
            }

            TaskSearchViewModel searchModel = new TaskSearchViewModel();
            ViewBag.Actived = true;
            //Title
            var title = (from p in _context.PageModel
                         where p.Parameter.Contains(Type)
                         select p.PageName).FirstOrDefault();
            ViewBag.Title = title;
            CreateViewBag(Type: Type);

            var estimatedTitle = (from p in _context.PageModel
                                  where p.PageUrl.Contains("EstimatedCalendar")
                                  select p.PageName).FirstOrDefault();
            ViewBag.EstimatedTitle = estimatedTitle;

            return View(searchModel);
        }

        [HttpPost]
        //Lịch của tôi
        public ActionResult _PaggingServerSide(DatatableViewModel model, TaskSearchViewModel searchViewModel, List<Guid> workflowList, List<string> processCodeList)
        {
            try
            {
                searchViewModel.Assignee = CurrentUser.EmployeeCode;

                #region //Receive Date
                if (searchViewModel.ReceiveCommonDate != "Custom")
                {
                    DateTime? fromDate;
                    DateTime? toDate;
                    _unitOfWork.CommonDateRepository.GetDateBy(searchViewModel.ReceiveCommonDate, out fromDate, out toDate);
                    //Tìm kiếm kỳ hiện tại
                    searchViewModel.ReceiveFromDate = fromDate;
                    searchViewModel.ReceiveToDate = toDate;
                }
                #endregion

                #region //Start Date
                if (searchViewModel.StartCommonDate != "Custom")
                {
                    DateTime? fromDate;
                    DateTime? toDate;
                    _unitOfWork.CommonDateRepository.GetDateBy(searchViewModel.StartCommonDate, out fromDate, out toDate);
                    //Tìm kiếm kỳ hiện tại
                    searchViewModel.StartFromDate = fromDate;
                    searchViewModel.StartToDate = toDate;
                }
                #endregion

                #region //Estimate End Date
                if (searchViewModel.EstimateEndCommonDate != "Custom")
                {
                    DateTime? fromDate;
                    DateTime? toDate;
                    _unitOfWork.CommonDateRepository.GetDateBy(searchViewModel.EstimateEndCommonDate, out fromDate, out toDate);
                    //Tìm kiếm kỳ hiện tại
                    searchViewModel.EstimateEndFromDate = fromDate;
                    searchViewModel.EstimateEndToDate = toDate;
                }
                #endregion

                #region //End Date
                if (searchViewModel.EndCommonDate != "Custom")
                {
                    DateTime? fromDate;
                    DateTime? toDate;
                    _unitOfWork.CommonDateRepository.GetDateBy(searchViewModel.EndCommonDate, out fromDate, out toDate);
                    //Tìm kiếm kỳ hiện tại
                    searchViewModel.EndFromDate = fromDate;
                    searchViewModel.EndToDate = toDate;
                }
                #endregion

                int filteredResultsCount = 0;

                searchViewModel.PageSize = null;
                searchViewModel.PageNumber = null;

                //Calendar không có loại GTB
                var res = _unitOfWork.TaskRepository.SearchQueryTaskProc(searchViewModel, out filteredResultsCount, workflowList: workflowList, AccountId: CurrentUser.AccountId, processCodeList: processCodeList);

                return Json(res, JsonRequestBehavior.AllowGet);
            }
            catch //(Exception)
            {
                return Json(null);
            }
        }

        //Lịch thăm hỏi dự kiến
        public ActionResult _PaggingServerSide_GTB(TaskSearchViewModel searchViewModel)
        {
            List<TaskViewModel> res = new List<TaskViewModel>();

            res = _context.Database.SqlQuery<TaskViewModel>("EXEC [dbo].[usp_EstimatedCalendar] @CompanyId, @RolesCode,  @Reporter, @Assignee, @WorkFlowId, @StartFromDate, @StartToDate",
                new SqlParameter("@CompanyId", searchViewModel.CompanyId ?? (object)DBNull.Value),
                new SqlParameter("@RolesCode", searchViewModel.RolesCode ?? (object)DBNull.Value),
                new SqlParameter("@Reporter", searchViewModel.Reporter ?? (object)DBNull.Value),
                new SqlParameter("@Assignee", searchViewModel.Assignee ?? (object)DBNull.Value),
                new SqlParameter("@WorkFlowId", searchViewModel.WorkFlowId ?? (object)DBNull.Value),
                new SqlParameter("@StartFromDate", searchViewModel.StartFromDate ?? (object)DBNull.Value),
                new SqlParameter("@StartToDate", searchViewModel.StartToDate ?? (object)DBNull.Value)).ToList();

            //return Json(res, JsonRequestBehavior.AllowGet);

            var jsonResult = Json(res, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;

            return jsonResult;
        }
        #endregion

        #region CreateViewBag
        private void CreateViewBag(string Type = null)
        {
            //Type: Loại (WorkflowCategoryCode)
            ViewBag.Type = Type;

            //WorkFlow list
            //Mission
            var MissionTitle = _context.WorkFlowCategoryModel.Where(p => p.WorkFlowCategoryCode == ConstWorkFlowCategory.MISSION)
                                       .Select(p => p.WorkFlowCategoryName).FirstOrDefault();
            ViewBag.MissionTitle = MissionTitle;
            var missionLst = _unitOfWork.WorkFlowRepository.GetWorkFlowBy(ConstWorkFlowCategory.MISSION, CurrentUser.CompanyCode);
            ViewBag.MissionList = missionLst;

            //Booking_Visit
            var BookingVisitTitle = _context.WorkFlowCategoryModel.Where(p => p.WorkFlowCategoryCode == ConstWorkFlowCategory.BOOKING_VISIT)
                                                                    .Select(p => p.WorkFlowCategoryName).FirstOrDefault();
            ViewBag.BookingVisitTitle = BookingVisitTitle;

            var bookingVisitLst = _unitOfWork.WorkFlowRepository.GetWorkFlowBy(ConstWorkFlowCategory.BOOKING_VISIT, CurrentUser.CompanyCode);
            var bookingVisitSubtaskLst = _unitOfWork.WorkFlowRepository.GetWorkFlowBy(ConstWorkFlowCategory.SUBTASK_BOOKINGVISIT, CurrentUser.CompanyCode);
            if (bookingVisitSubtaskLst != null && bookingVisitSubtaskLst.Count > 0)
            {
                bookingVisitLst.AddRange(bookingVisitSubtaskLst);
            }
            ViewBag.BookingVisitList = bookingVisitLst;
            //Ticket
            var TicketTitle = _context.WorkFlowCategoryModel.Where(p => p.WorkFlowCategoryCode == ConstWorkFlowCategory.TICKET)
                                      .Select(p => p.WorkFlowCategoryName).FirstOrDefault();
            ViewBag.TicketTitle = TicketTitle;
            var ticketLst = _unitOfWork.WorkFlowRepository.GetWorkFlowBy(ConstWorkFlowCategory.TICKET, CurrentUser.CompanyCode);
            ViewBag.TicketList = ticketLst;

            //THKH
            //var THKHTitle = _context.WorkFlowCategoryModel.Where(p => p.WorkFlowCategoryCode == ConstWorkFlowCategory.THKH)
            //                        .Select(p => p.WorkFlowCategoryName).FirstOrDefault();
            //ViewBag.THKHTitle = THKHTitle;
            var THKHLst = _unitOfWork.WorkFlowRepository.GetWorkFlowBy(ConstWorkFlowCategory.THKH, CurrentUser.CompanyCode);

            //Activities
            var ActivitiesTitle = _context.WorkFlowCategoryModel.Where(p => p.WorkFlowCategoryCode == ConstWorkFlowCategory.ACTIVITIES)
                                          .Select(p => p.WorkFlowCategoryName).FirstOrDefault();
            ViewBag.ActivitiesTitle = ActivitiesTitle;
            var activitiesLst = _unitOfWork.WorkFlowRepository.GetWorkFlowBy(ConstWorkFlowCategory.ACTIVITIES, CurrentUser.CompanyCode);
            //ViewBag.ActivitiesList = activitiesLst;
            THKHLst.AddRange(activitiesLst);
            ViewBag.THKHList = THKHLst;

            //GTB
            var GTBTitle = _context.WorkFlowCategoryModel.Where(p => p.WorkFlowCategoryCode == ConstWorkFlowCategory.GTB)
                                   .Select(p => p.WorkFlowCategoryName).FirstOrDefault();
            ViewBag.GTBTitle = GTBTitle;
            var GTBLst = _unitOfWork.WorkFlowRepository.GetWorkFlowBy(ConstWorkFlowCategory.GTB, CurrentUser.CompanyCode);
            ViewBag.GTBList = GTBLst;

            //Status 
            var statusLst = new List<TaskStatusDropdownList>();
            //Expired
            statusLst.Add(new TaskStatusDropdownList
            {
                StatusCode = ConstTaskStatus.Expired,
                StatusName = LanguageResource.Expired,
                StatusColor = "#dd4b39"
            });
            //Todo
            statusLst.Add(new TaskStatusDropdownList
            {
                StatusCode = ConstTaskStatus.Todo,
                StatusName = LanguageResource.Todo,
                StatusColor = "#fff"
            });
            //Processing
            statusLst.Add(new TaskStatusDropdownList
            {
                StatusCode = ConstTaskStatus.Processing,
                StatusName = LanguageResource.Processing,
                StatusColor = "#0052CC"
            });
            //Completed
            statusLst.Add(new TaskStatusDropdownList
            {
                StatusCode = ConstTaskStatus.CompletedOnTime,
                StatusName = LanguageResource.CompletedOnTime,
                StatusColor = "#398439"
            });
            //Completed
            statusLst.Add(new TaskStatusDropdownList
            {
                StatusCode = ConstTaskStatus.CompletedExpire,
                StatusName = LanguageResource.CompletedExpire,
                StatusColor = "#f39c12"
            });
            ViewBag.TaskProcessCode = statusLst;

            //Employee
            var empLst = _unitOfWork.SalesEmployeeRepository.GetAllForDropdownlist();
            //Reporter
            ViewBag.ReporterList = new SelectList(empLst, "SalesEmployeeCode", "SalesEmployeeName");
            //Assignee
            ViewBag.AssigneeList = new SelectList(empLst, "SalesEmployeeCode", "SalesEmployeeName");

            //ProfileGroupCode: Nhóm khách hàng
            var customerGroupList = _unitOfWork.CatalogRepository.GetCustomerCategory(CurrentUser.CompanyCode);
            ViewBag.ProfileGroupCode = new SelectList(customerGroupList, "CatalogCode", "CatalogText_vi");

            //RolesCode: Phòng ban
            var rolesList = _context.RolesModel.Where(p => p.Actived == true && p.isEmployeeGroup == true).ToList();
            ViewBag.RolesCode = new SelectList(rolesList, "RolesCode", "RolesName");

            //Company
            var compList = _unitOfWork.CompanyRepository.GetAll();
            ViewBag.CompanyId = new SelectList(compList, "CompanyId", "CompanyName");

        }
        public List<WorkFlowViewModel> GetWorkFlowBy(string Type)
        {
            var lst = _unitOfWork.WorkFlowRepository.GetWorkFlowBy(Type, CurrentUser.CompanyCode);
            return lst;
        }
        public ActionResult GetRoleBySaleEmployee(string SalesEmployeeCode)
        {
            var role = (from p in _context.AccountModel
                        from m in p.RolesModel
                        where p.EmployeeCode == SalesEmployeeCode
                        && m.isEmployeeGroup == true
                        select m.RolesName).FirstOrDefault();

            role = role != null ? role : "";
            return Json(role, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region SaveShortNote
        [HttpPost]
        public ActionResult SaveShortNote(Guid? ShortNoteTaskId, string ShortNote)
        {
            return ExecuteContainer(() =>
            {
                var task = _context.TaskModel.Where(p => p.TaskId == ShortNoteTaskId).FirstOrDefault();
                if (task != null)
                {
                    if (!string.IsNullOrEmpty(ShortNote))
                    {
                        task.ShortNote = ShortNote;
                        _context.Entry(task).State = EntityState.Modified;
                        _context.SaveChanges();
                    }
                }

                return Json(new
                {
                    Code = HttpStatusCode.Created,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Edit_Success, LanguageResource.Note.ToLower()),
                });
            });
        }
        #endregion SaveShortNote

        #region CreateNewTask
        public ActionResult _CreateNewTask()
        {
            var result = new List<ISDSelectStringItem>();

            //Mission
            var MissionTitle = _context.WorkFlowCategoryModel.Where(p => p.WorkFlowCategoryCode == ConstWorkFlowCategory.MISSION)
                                       .Select(p => p.WorkFlowCategoryName).FirstOrDefault();
            result.Add(new ISDSelectStringItem()
            {
                id = ConstWorkFlowCategory.MISSION,
                name = MissionTitle,
            });

            //Booking_Visit
            var BookingVisitTitle = _context.WorkFlowCategoryModel.Where(p => p.WorkFlowCategoryCode == ConstWorkFlowCategory.BOOKING_VISIT)
                                                                   .Select(p => p.WorkFlowCategoryName).FirstOrDefault();
            result.Add(new ISDSelectStringItem()
            {
                id = ConstWorkFlowCategory.BOOKING_VISIT,
                name = BookingVisitTitle,
            });

            //Ticket
            var TicketTitle = _context.WorkFlowCategoryModel.Where(p => p.WorkFlowCategoryCode == ConstWorkFlowCategory.TICKET)
                                      .Select(p => p.WorkFlowCategoryName).FirstOrDefault();
            result.Add(new ISDSelectStringItem()
            {
                id = ConstWorkFlowCategory.TICKET,
                name = TicketTitle,
            });

            //THKH
            var THKHTitle = _context.WorkFlowCategoryModel.Where(p => p.WorkFlowCategoryCode == ConstWorkFlowCategory.THKH)
                                    .Select(p => p.WorkFlowCategoryName).FirstOrDefault();
            result.Add(new ISDSelectStringItem()
            {
                id = ConstWorkFlowCategory.THKH,
                name = THKHTitle,
            });

            //Activities
            var ActivitiesTitle = _context.WorkFlowCategoryModel.Where(p => p.WorkFlowCategoryCode == ConstWorkFlowCategory.ACTIVITIES)
                                          .Select(p => p.WorkFlowCategoryName).FirstOrDefault();
            result.Add(new ISDSelectStringItem()
            {
                id = ConstWorkFlowCategory.ACTIVITIES,
                name = ActivitiesTitle,
            });

            //GTB
            var GTBTitle = _context.WorkFlowCategoryModel.Where(p => p.WorkFlowCategoryCode == ConstWorkFlowCategory.GTB)
                                   .Select(p => p.WorkFlowCategoryName).FirstOrDefault();
            ViewBag.GTBTitle = GTBTitle;
            result.Add(new ISDSelectStringItem()
            {
                id = ConstWorkFlowCategory.GTB,
                name = GTBTitle,
            });

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}