using GoogleMaps.LocationServices;
using ISD.Constant;
using ISD.EntityModels;
using ISD.Extensions;
using ISD.Repositories;
using ISD.Resources;
using ISD.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using ISD.Core;
using System.Data.SqlClient;

namespace ISD.API.Controllers
{
    public class TaskController : BaseController
    {
        private string GoogleMapAPIKey = WebConfigurationManager.AppSettings["GoogleMapAPIKey"].ToString();
        // GET: Task
        #region Get All Task

        public ActionResult GetAllTask(string KanbanCode, string UserNameCode, string CompanyCode, string TaskProcessCode, string Reporter, int? PageSize, int? PageNumber, string token, string key, Guid? AccountId = null, bool? isCreated = null, bool? isReporter = null, bool? isAssignee = null)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                if (string.IsNullOrEmpty(CompanyCode))
                {
                    var account = _context.AccountModel.Where(p => p.EmployeeCode == UserNameCode).FirstOrDefault();
                    if (account != null)
                    {
                        var store = account.StoreModel.FirstOrDefault();
                        if (store != null)
                        {
                            CompanyCode = _context.CompanyModel.Where(p => p.CompanyId == store.CompanyId).Select(p => p.CompanyCode).FirstOrDefault();
                        }

                    }
                }

                if (KanbanCode == ConstWorkFlowCategory.TICKET && CompanyCode == "2000")
                {
                    KanbanCode = ConstWorkFlowCategory.TICKET_MLC;
                }
                if (KanbanCode == "My_Follow")
                {
                    KanbanCode = "M_BOOKING_VISIT";
                }

                var searchModel = new TaskSearchViewModel();
                if (isCreated == true)
                {
                    searchModel.CreateBy = UserNameCode;
                }
                if (isReporter == true)
                {
                    searchModel.Reporter = UserNameCode;
                }
                if (isAssignee == true)
                {
                    searchModel.Assignee = UserNameCode;
                }

                //Trên mobile mặc định sẽ hiển thị danh sách task theo người đang đăng nhập nếu không truyền các tham số:
                //1.Người tạo
                //2.NV theo dõi
                //3.NV được phân công
                if (string.IsNullOrEmpty(searchModel.CreateBy) && string.IsNullOrEmpty(searchModel.Reporter) && string.IsNullOrEmpty(searchModel.Assignee))
                {
                    if (KanbanCode == ConstWorkFlowCategory.TICKET_MLC)
                    {
                        searchModel.Assignee = UserNameCode;
                    }
                    else if (KanbanCode == ConstWorkFlowCategory.MISSION)
                    {
                        searchModel.CreateBy = UserNameCode;
                        searchModel.Reporter = UserNameCode;
                        searchModel.Assignee = UserNameCode;
                    }
                }

                List<Guid> WorkflowList = _unitOfWork.WorkFlowRepository.GetWorkFlowBy(KanbanCode, CompanyCode)
                                                //.Where(p => p.WorkFlowCode != ConstWorkFlow.GT)
                                                .Select(p => p.WorkFlowId).ToList();

                #region //Start Date
                searchModel.StartCommonDate = "Custom";
                #endregion

                #region //Receive Date
                searchModel.ReceiveCommonDate = "Custom";
                #endregion

                #region //Estimate End Date
                searchModel.EstimateEndCommonDate = "Custom";
                #endregion

                #region //End Date
                searchModel.EndCommonDate = "Custom";
                #endregion

                searchModel.Type = KanbanCode;
                //searchModel.KanbanId = KanbanId;
                searchModel.IsMobile = true;
                searchModel.TaskProcessCode = TaskProcessCode;

                //Trạng thái chưa hoàn thành
                List<string> processCodeList = new List<string>();
                if (KanbanCode == ConstWorkFlowCategory.TICKET_MLC)
                {
                    searchModel.TaskProcessCode = ConstTaskStatus.Incomplete;
                }
                else
                {
                    if (string.IsNullOrEmpty(searchModel.TaskProcessCode))
                    {
                        processCodeList.Add(ConstTaskStatus.Todo);
                        processCodeList.Add(ConstTaskStatus.Processing);
                        processCodeList.Add(ConstTaskStatus.Incomplete);
                        processCodeList.Add(ConstTaskStatus.CompletedOnTime);
                        processCodeList.Add(ConstTaskStatus.CompletedExpire);
                        processCodeList.Add(ConstTaskStatus.Expired);
                    }
                }

                //update: giao việc đổi thành công việc đã giao và search theo CreateBy = CurrentAccount
                //if (searchModel.Type == "MISSON")
                //{
                //    string EmployeeCode = _context.AccountModel.Where(p => p.AccountId == AccountId).Select(p => p.EmployeeCode).FirstOrDefault();
                //    searchModel.CreateBy = EmployeeCode;
                //}

                string DomainImageWorkFlow = ConstDomain.Domain + "/Upload/WorkFlow/";
                int filteredResultsCount = 0;

                //Page Size
                searchModel.PageSize = PageSize;

                //Page Number
                searchModel.PageNumber = PageNumber;


                //var taskList = _unitOfWork.TaskRepository.SearchQueryTask(searchModel, DomainImageWorkFlow, AccountId: AccountId).OrderBy(p => p.TaskStatusOrderIndex).ToList();
                var taskList = _unitOfWork.TaskRepository.SearchQueryTaskProc(searchModel, out filteredResultsCount, DomainImageWorkFlow: DomainImageWorkFlow, AccountId: AccountId, processCodeList: processCodeList, workflowList: WorkflowList, CurrentCompanyCode: CompanyCode);

                //Bảo hành: show ra ngoài các lịch mặc định theo thứ tự của Ngày Bắt Đầu
                //if (KanbanCode == ConstWorkFlowCategory.TICKET_MLC && (string.IsNullOrEmpty(TaskProcessCode) || TaskProcessCode == ConstTaskStatus.Incomplete))
                //{
                //    taskList = taskList.OrderByDescending(p => p.StartDate.HasValue).ThenBy(p => p.StartDate).ToList();
                //}
                //else
                //{
                //    taskList = taskList.OrderByDescending(p => p.TaskCode).ToList();
                //}

                if (taskList != null && taskList.Count > 0)
                {
                    foreach (var item in taskList)
                    {
                        //Assignee
                        var assigneeLst = (from p in _context.TaskAssignModel
                                           join s in _context.SalesEmployeeModel on p.SalesEmployeeCode equals s.SalesEmployeeCode
                                           where p.TaskId == item.TaskId
                                           select new SalesEmployeeViewModel
                                           {
                                               SalesEmployeeCode = s.SalesEmployeeCode,
                                               SalesEmployeeName = s.SalesEmployeeName
                                           }).ToList();
                        if (assigneeLst != null && assigneeLst.Count() > 0)
                        {
                            item.AssigneeName = string.Join(", ", assigneeLst.Select(p => p.SalesEmployeeName).ToArray());
                            //Assignee on kanban
                            var avaLst = new List<string>();
                            foreach (var emp in assigneeLst)
                            {
                                avaLst.Add(emp.SalesEmployeeName.GetCharacterForLogoName());
                            }

                            if (avaLst != null && avaLst.Count > 0)
                            {
                                item.Avatar = string.Join(", ", avaLst);
                            }
                        }

                        //check in/out
                        if (item.isRequiredCheckin == true)
                        {
                            var checkInOutComplete = _context.CheckInOutModel.Where(p => p.TaskId == item.TaskId && p.CheckOutBy != null).FirstOrDefault();
                            if (checkInOutComplete != null)
                            {
                                item.isCheckInOutComplete = true;
                            }
                        }
                    }
                }

                //Check có hiển thị kanban hay không?
                var columns = new List<KanbanColumnViewModel>();
                var kanban = _context.KanbanModel.Where(p => p.KanbanCode == KanbanCode).FirstOrDefault();
                if (kanban != null)
                {
                    if (kanban.Actived == true)
                    {
                        columns = _unitOfWork.KanbanTaskRepository.GetColumnKanban(kanban.KanbanId, HasUnmapped: false, IsMobile: true);
                    }
                    else
                    {
                        return _APISuccess(new { kanbanDetailList = taskList });
                    }
                }

                return _APISuccess(new { columns, kanbanDetailList = taskList });
            });
        }

        #region GetAllTask_bak
        public ActionResult GetAllTask_bak(string KanbanCode, string UserNameCode, string CompanyCode, string TaskProcessCode, string Reporter, int? PageSize, int? PageNumber, string token, string key, Guid? AccountId = null, List<Guid> WorkflowList = null)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                //Load column base on KanbanCode
                Guid KanbanId = Guid.Empty;

                if (string.IsNullOrEmpty(CompanyCode))
                {
                    var account = _context.AccountModel.Where(p => p.EmployeeCode == UserNameCode).FirstOrDefault();
                    if (account != null)
                    {
                        var store = account.StoreModel.FirstOrDefault();
                        if (store != null)
                        {
                            CompanyCode = _context.CompanyModel.Where(p => p.CompanyId == store.CompanyId).Select(p => p.CompanyCode).FirstOrDefault();
                        }

                    }
                }
                if (KanbanCode == "TICKET" && CompanyCode == "2000")
                {
                    KanbanCode = "TICKET_MLC";
                }

                var kanban = _context.KanbanModel.Where(p => p.KanbanCode == KanbanCode).FirstOrDefault();
                if (kanban != null)
                {
                    KanbanId = kanban.KanbanId;
                }

                var searchModel = new TaskSearchViewModel();

                if (!string.IsNullOrEmpty(Reporter) && (KanbanCode != ConstKanbanType.MyWork) && (KanbanCode != ConstKanbanType.MyWork))
                {
                    searchModel.Reporter = Reporter;
                }

                //Giao cho tôi => MyWork: Assignee = UserNameCode
                if (KanbanCode == ConstKanbanType.MyWork)
                {
                    searchModel.Assignee = UserNameCode;
                }
                //Đang theo dõi => MyFollow: Reporter = UserNameCode
                if (KanbanCode == ConstKanbanType.MyFollow)
                {
                    searchModel.Reporter = UserNameCode;
                }

                #region //Start Date
                searchModel.StartCommonDate = "Custom";
                #endregion

                #region //Receive Date
                searchModel.ReceiveCommonDate = "Custom";
                #endregion

                #region //Estimate End Date
                searchModel.EstimateEndCommonDate = "Custom";
                #endregion

                #region //End Date
                searchModel.EndCommonDate = "Custom";
                #endregion

                searchModel.Type = KanbanCode;
                searchModel.KanbanId = KanbanId;
                searchModel.IsMobile = true;
                searchModel.TaskProcessCode = TaskProcessCode;

                //Trạng thái chưa hoàn thành
                List<string> processCodeList = new List<string>();
                //if (KanbanCode != ConstKanbanType.MyWork && KanbanCode != ConstKanbanType.MyFollow && string.IsNullOrEmpty(searchModel.TaskProcessCode))
                //{
                //    //searchModel.TaskProcessCode = ConstTaskStatus.Incomplete;
                //}
                //else
                //{
                //    if (string.IsNullOrEmpty(searchModel.TaskProcessCode))
                //    {
                //        processCodeList.Add(ConstTaskStatus.Todo);
                //        processCodeList.Add(ConstTaskStatus.Processing);
                //        processCodeList.Add(ConstTaskStatus.Incomplete);
                //        processCodeList.Add(ConstTaskStatus.CompletedOnTime);
                //        processCodeList.Add(ConstTaskStatus.CompletedExpire);
                //        processCodeList.Add(ConstTaskStatus.Expired);
                //    }
                //}

                if (string.IsNullOrEmpty(searchModel.TaskProcessCode))
                {
                    processCodeList.Add(ConstTaskStatus.Todo);
                    processCodeList.Add(ConstTaskStatus.Processing);
                    processCodeList.Add(ConstTaskStatus.Incomplete);
                    processCodeList.Add(ConstTaskStatus.CompletedOnTime);
                    processCodeList.Add(ConstTaskStatus.CompletedExpire);
                    processCodeList.Add(ConstTaskStatus.Expired);
                }

                //update: giao việc đổi thành công việc đã giao và search theo CreateBy = CurrentAccount
                if (searchModel.Type == "MISSON")
                {
                    string EmployeeCode = _context.AccountModel.Where(p => p.AccountId == AccountId).Select(p => p.EmployeeCode).FirstOrDefault();
                    searchModel.CreateBy = EmployeeCode;
                }

                string DomainImageWorkFlow = ConstDomain.Domain + "/Upload/WorkFlow/";
                int filteredResultsCount = 0;

                //Page Size
                searchModel.PageSize = PageSize;

                //Page Number
                searchModel.PageNumber = PageNumber;


                //var taskList = _unitOfWork.TaskRepository.SearchQueryTask(searchModel, DomainImageWorkFlow, AccountId: AccountId).OrderBy(p => p.TaskStatusOrderIndex).ToList();
                var taskList = _unitOfWork.TaskRepository.SearchQueryTaskProc(searchModel, out filteredResultsCount, DomainImageWorkFlow: DomainImageWorkFlow, AccountId: AccountId, processCodeList: processCodeList, workflowList: WorkflowList, CurrentCompanyCode: CompanyCode);

                if (taskList != null && taskList.Count > 0)
                {
                    foreach (var item in taskList)
                    {
                        //Assignee
                        var assigneeLst = (from p in _context.TaskAssignModel
                                           join s in _context.SalesEmployeeModel on p.SalesEmployeeCode equals s.SalesEmployeeCode
                                           where p.TaskId == item.TaskId
                                           select new SalesEmployeeViewModel
                                           {
                                               SalesEmployeeCode = s.SalesEmployeeCode,
                                               SalesEmployeeName = s.SalesEmployeeName
                                           }).ToList();
                        if (assigneeLst != null && assigneeLst.Count() > 0)
                        {
                            item.AssigneeName = string.Join(", ", assigneeLst.Select(p => p.SalesEmployeeName).ToArray());
                            //Assignee on kanban
                            var avaLst = new List<string>();
                            foreach (var emp in assigneeLst)
                            {
                                avaLst.Add(emp.SalesEmployeeName.GetCharacterForLogoName());
                            }

                            if (avaLst != null && avaLst.Count > 0)
                            {
                                item.Avatar = string.Join(", ", avaLst);
                            }
                        }

                        //check in/out
                        if (item.isRequiredCheckin == true)
                        {
                            var checkInOutComplete = _context.CheckInOutModel.Where(p => p.TaskId == item.TaskId && p.CheckOutBy != null).FirstOrDefault();
                            if (checkInOutComplete != null)
                            {
                                item.isCheckInOutComplete = true;
                            }
                        }
                    }
                }
                if (kanban.Actived != true)
                {
                    return _APISuccess(new { kanbanDetailList = taskList.OrderByDescending(p => p.TaskCode) });
                }

                var columns = _unitOfWork.KanbanTaskRepository.GetColumnKanban(KanbanId, HasUnmapped: false, IsMobile: true);



                return _APISuccess(new { columns, kanbanDetailList = taskList.OrderByDescending(p => p.TaskCode) });
            });
        }
        #endregion GetAllTask_bak

        #endregion Get All Task

        #region Get Notification
        public ActionResult GetNotificationBy(Guid AccountId, string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                var result = new List<NotificationMobileViewModel>();
                ////Get notification id by AccountId
                //var notificationAccountLst = _context.NotificationAccountMappingModel.Where(p => p.AccountId == AccountId).ToList();
                //if (notificationAccountLst != null && notificationAccountLst.Count > 0)
                //{
                //    foreach (var notification in notificationAccountLst)
                //    {
                //        result.Add(new NotificationMobileViewModel()
                //        {
                //            NotificationId = notification.NotificationId,
                //            IsRead = notification.IsRead,
                //        });
                //    }
                //}

                ////Get notification data
                //if (result != null && result.Count > 0)
                //{
                //    foreach (var item in result)
                //    {
                //        var notification = (from p in _context.NotificationModel
                //                            join t in _context.TaskModel on p.TaskId equals t.TaskId
                //                            where p.NotificationId == item.NotificationId
                //                            select p
                //                            ).FirstOrDefault();
                //        item.TaskId = notification.TaskId;
                //        item.Title = notification.Title;
                //        item.Description = notification.Description;
                //        item.Detail = notification.Detail;
                //        item.CreatedDate = notification.CreatedDate;
                //    }
                //}

                result = (from p in _context.NotificationModel
                          join a in _context.NotificationAccountMappingModel on p.NotificationId equals a.NotificationId
                          join t in _context.TaskModel on p.TaskId equals t.TaskId
                          where a.AccountId == AccountId
                          select new NotificationMobileViewModel()
                          {
                              NotificationId = a.NotificationId,
                              IsRead = a.IsRead,
                              TaskId = p.TaskId,
                              Title = p.Title,
                              Description = p.Description,
                              Detail = p.Detail,
                              CreatedDate = p.CreatedDate,
                          }).OrderByDescending(p => p.CreatedDate).Take(100).ToList();
                return _APISuccess(result);
            });
        }
        #endregion Get Notification

        #region Get Task Detail
        public ActionResult GetTaskDetail(Guid TaskId, string token, string key, Guid? AccountId = null, Guid? NotificationId = null, bool? isRead = null, string CompanyCode = null)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                //Type
                var type = _unitOfWork.TaskRepository.GetWorkflowCategoryCode(TaskId);
                TaskViewModel task = new TaskViewModel();
                var statusLst = new List<ISDSelectGuidItemWithColor>();
                var priorityLst = new List<ISDSelectStringItemWithColor>();
                var issueTypeLst = new List<ISDSelectGuidItemWithColor>();
                var assignLst = new List<ISDSelectStringItemWithIcon>();
                var reporterLst = new List<ISDSelectStringItemWithIcon>();
                var assigneeLst = new List<ISDSelectStringItemWithIcon>();
                var stepButtonLst = new List<StepButtonViewModel>();
                var configList = new List<MobileWorkFlowConfigViewModel>();
                var contactList = new List<ISDSelectGuidItem>();
                var subtaskTypeList = new List<ISDSelectStringItem>();
                var addressList = new List<string>();

                bool? isDisabledSummary = false;
                bool? isSavePermission = false;
                //isCheckOut
                bool isCheckOut = false;
                bool isCheckInOutComplete = false;

                task = _unitOfWork.TaskRepository.GetTaskInfo(TaskId, CompanyCode);
                //task = _unitOfWork.TaskRepository.GetTaskById(TaskId);
                if (task != null)
                {
                    //IsAssignGroup
                    if (!task.IsAssignGroup.HasValue)
                    {
                        task.IsAssignGroup = false;
                    }
                    //Check in out
                    if (task.CheckInOutId != null)
                    {
                        if (task.CheckOutBy == null)
                        {
                            isCheckOut = true;
                        }
                        else
                        {
                            isCheckInOutComplete = true;
                        }
                    }
                    //Description
                    if (!string.IsNullOrEmpty(task.Description))
                    {
                        task.Description = task.Description.Replace("<p>", "").Replace("</p>", "");
                    }
                    if (!string.IsNullOrEmpty(task.WorkFlowImageUrl))
                    {
                        task.WorkFlowImageUrl = ConstDomain.Domain + "/Upload/WorkFlow/" + task.WorkFlowImageUrl;
                    }
                    //Color
                    //Todo
                    if (task.ProcessCode == ConstProcess.todo)
                    {
                        task.TaskStatusBackgroundColor = "#fff";
                        task.TaskStatusColor = "#000";
                    }
                    //Processing
                    else if (task.ProcessCode == ConstProcess.processing)
                    {
                        task.TaskStatusBackgroundColor = "#0052CC";
                        task.TaskStatusColor = "#fff";
                    }
                    //Completed
                    else if (task.ProcessCode == ConstProcess.completed)
                    {
                        task.TaskStatusBackgroundColor = "#398439";
                        task.TaskStatusColor = "#fff";
                    }
                    //Task Status list
                    #region Task Status
                    //List task status theo workflow
                    statusLst = (from p in _context.TaskStatusModel
                                 where p.WorkFlowId == task.WorkFlowId
                                 orderby p.OrderIndex
                                 select new ISDSelectGuidItemWithColor()
                                 {
                                     id = p.TaskStatusId,
                                     name = p.TaskStatusName,
                                     color = p.ProcessCode == ConstProcess.todo ? "#000" : "#fff",
                                     bgColor = p.ProcessCode == ConstProcess.todo ? "#fff" : (p.ProcessCode == ConstProcess.processing ? "#0052CC" : "#398439"),
                                     iconType = "MaterialCommunityIcons",
                                     iconName = "progress-check"
                                 }).ToList();

                    //List issue type
                    issueTypeLst = (from p in _context.WorkFlowModel
                                    orderby p.OrderIndex
                                    select new ISDSelectGuidItemWithColor()
                                    {
                                        id = p.WorkFlowId,
                                        name = p.WorkFlowName,
                                        color = ConstDomain.Domain + "/Upload/WorkFlow/" + p.ImageUrl,
                                    }).ToList();

                    //List employee
                    if (task.Type.Contains(ConstWorkFlowCategory.BOOKING_VISIT) || task.Type.Contains(ConstWorkFlowCategory.SUBTASK_BOOKINGVISIT))
                    {
                        reporterLst = _unitOfWork.SalesEmployeeRepository.GetSalesEmployeeByRoles(ConstRoleCode.BAOVE).Select(p => new ISDSelectStringItemWithIcon()
                        {
                            id = p.SalesEmployeeCode,
                            name = p.SalesEmployeeName,
                            icon = p.SalesEmployeeName.GetCharacterForLogoName()
                        }).ToList();
                    }
                    else
                    {
                        reporterLst = _unitOfWork.SalesEmployeeRepository.GetAllForDropdownlist().Select(p => new ISDSelectStringItemWithIcon()
                        {
                            id = p.SalesEmployeeCode,
                            name = p.SalesEmployeeName,
                            icon = p.SalesEmployeeName.GetCharacterForLogoName()
                        }).ToList();
                    }


                    assigneeLst = _unitOfWork.SalesEmployeeRepository.GetAllForDropdownlist().Select(p => new ISDSelectStringItemWithIcon()
                    {
                        id = p.SalesEmployeeCode,
                        name = p.SalesEmployeeName + " | " + p.RolesName,
                        icon = p.SalesEmployeeName.GetCharacterForLogoName(),
                        description = p.RolesName,
                    }).ToList();

                    //List priority
                    priorityLst = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.Priority).OrderByDescending(p => p.OrderIndex).Select(p => new ISDSelectStringItemWithColor()
                    {
                        id = p.CatalogCode,
                        name = p.CatalogText_vi,
                        color = p.CatalogCode == ConstPriotityCode.LOW ? "#008000" : (p.CatalogCode == ConstPriotityCode.NORMAL ? "#FFA500" : (p.CatalogCode == ConstPriotityCode.HIGH ? "#FF4500" : "#FF0000")),
                        iconType = "AntDesign",
                        iconName = p.CatalogCode == ConstPriotityCode.LOW ? "arrowdown" : "arrowup",
                    }).ToList();
                    #endregion

                    //Assignee list
                    #region Assignee
                    if (task.taskAssignList != null && task.taskAssignList.Count() > 0)
                    {
                        foreach (var assignee in task.taskAssignList)
                        {
                            if (!string.IsNullOrEmpty(assignee.SalesEmployeeName))
                            {
                                assignee.LogoName = assignee.SalesEmployeeName.GetCharacterForLogoName();
                            }
                        }
                    }
                    #endregion Assignee

                    //Reporter list
                    if (!string.IsNullOrEmpty(task.ReporterName))
                    {
                        task.ReporterLogoName = task.ReporterName.GetCharacterForLogoName();
                    }
                    if (task.taskReporterList != null && task.taskReporterList.Count() > 0)
                    {
                        foreach (var reporter in task.taskReporterList)
                        {
                            reporter.Reporter = reporter.SalesEmployeeCode;
                            if (!string.IsNullOrEmpty(reporter.SalesEmployeeName))
                            {
                                reporter.ReporterName = reporter.SalesEmployeeName;
                                reporter.ReporterLogoName = reporter.SalesEmployeeName.GetCharacterForLogoName();
                            }
                        }
                    }

                    //Load comments of task
                    #region Comments
                    var commentList = _unitOfWork.TaskRepository.GetTaskCommentList(TaskId);
                    task.taskCommentList = commentList;
                    task.NumberOfComments = commentList.Count;
                    #endregion

                    //Load files attachment upload
                    #region File Attachment
                    var taskFileList = _unitOfWork.TaskRepository.GetTaskFileList(commentList, TaskId);
                    if (taskFileList != null && taskFileList.Count > 0)
                    {
                        foreach (var file in taskFileList)
                        {
                            file.FileUrl = ConstDomain.Domain + "/Upload/Document/" + file.FileUrl;
                        }
                    }
                    task.taskFileList = taskFileList;
                    task.NumberOfFiles = taskFileList.Count;
                    #endregion

                    //Load history
                    #region History
                    var historyList = _unitOfWork.TaskRepository.GetTaskHistoryList(TaskId);
                    task.taskHistoryList = historyList;
                    #endregion

                    //Step button
                    #region Step button
                    var statusTransitionLst = (from p in _context.StatusTransitionModel
                                               where p.WorkFlowId == task.WorkFlowId && p.FromStatusId == task.TaskStatusId
                                               select p).ToList();

                    //check if current account is reporter or assignee
                    var currentAccount = _context.AccountModel.Where(p => p.AccountId == AccountId).FirstOrDefault();
                    if (currentAccount != null)
                    {
                        //reporter
                        if (currentAccount.EmployeeCode == task.Reporter)
                        {
                            var reporterStatusTransitionLst = statusTransitionLst.Where(p => p.isReporterPermission == true)
                                                                                .Select(p => new StepButtonViewModel()
                                                                                {
                                                                                    TransitionName = p.TransitionName,
                                                                                    TransitionStatusId = p.ToStatusId,
                                                                                    Color = p.Color,
                                                                                    isRequiredComment = p.isRequiredComment,
                                                                                }).ToList();
                            if (reporterStatusTransitionLst != null && reporterStatusTransitionLst.Count > 0)
                            {
                                stepButtonLst.AddRange(reporterStatusTransitionLst);
                            }
                        }
                        //assignee
                        if (task.taskAssignList != null && task.taskAssignList.Count > 0 && task.taskAssignList.Select(p => p.SalesEmployeeCode).Contains(currentAccount.EmployeeCode))
                        {
                            var asigneeStatusTransitionLst = statusTransitionLst.Where(p => p.isAssigneePermission == true)
                                                                                .Select(p => new StepButtonViewModel()
                                                                                {
                                                                                    TransitionName = p.TransitionName,
                                                                                    TransitionStatusId = p.ToStatusId,
                                                                                    Color = p.Color,
                                                                                    isRequiredComment = p.isRequiredComment,
                                                                                }).ToList();
                            if (asigneeStatusTransitionLst != null && asigneeStatusTransitionLst.Count > 0)
                            {
                                stepButtonLst.AddRange(asigneeStatusTransitionLst);
                            }
                        }
                    }

                    if (stepButtonLst != null && stepButtonLst.Count > 0)
                    {
                        stepButtonLst = stepButtonLst.GroupBy(m => new { m.TransitionName, m.TransitionStatusId, m.Color, m.isRequiredComment })
                                                    .Select(group => group.First())  // instead of First you can also apply your logic here what you want to take, for example an OrderBy
                                                    .ToList();
                    }

                    #endregion Step button

                    //Work flow config
                    //Task config field
                    //configList = (from config in _context.WorkFlowConfigModel
                    //              join field in _context.WorkFlowFieldModel on config.FieldCode equals field.FieldCode
                    //              where config.WorkFlowId == task.WorkFlowId
                    //              select new MobileWorkFlowConfigViewModel()
                    //              {
                    //                  FieldCode = config.FieldCode,
                    //                  FieldName = string.IsNullOrEmpty(config.Note) ? field.FieldName : config.Note,
                    //                  OrderIndex = field.OrderIndex,
                    //                  IsRequired = config.IsRequired,
                    //              }).OrderBy(p => p.OrderIndex).ToList();

                    configList = _context.WorkFlowConfigModel.Where(p => p.WorkFlowId == task.WorkFlowId && p.FieldCode != "ServiceTechnicalTeamCode")
                                                          .Select(p => new MobileWorkFlowConfigViewModel()
                                                          {
                                                              FieldCode = p.FieldCode,
                                                              FieldName = p.Note,
                                                              IsRequired = p.IsRequired,
                                                              HideWhenAdd = p.HideWhenAdd,
                                                              AddDefaultValue = p.AddDefaultValue,
                                                              HideWhenEdit = p.HideWhenEdit,
                                                              EditDefaultValue = p.EditDefaultValue,
                                                          }).ToList();
                    foreach (var item in configList)
                    {
                        if (string.IsNullOrEmpty(item.FieldName))
                        {
                            item.FieldName = PropertyHelper.GetDisplayNameByString<TaskViewModel>(item.FieldCode);
                        }
                    }

                    //Liên hệ
                    if (task.ProfileId != null)
                    {
                        contactList = _unitOfWork.ProfileRepository.GetContactListOfProfile(task.ProfileId)
                                                                    .Select(p => new ISDSelectGuidItem()
                                                                    {
                                                                        id = p.ProfileContactId,
                                                                        name = p.ProfileContactName,
                                                                    }).ToList();
                    }

                    //Ẩn trường tiêu đề
                    isDisabledSummary = _context.WorkFlowModel.Where(p => p.WorkFlowId == task.WorkFlowId).Select(p => p.IsDisabledSummary).FirstOrDefault();

                    //Quyền được cập nhập công việc: Chỉ có nhân viên được phân công mới được cập nhật
                    string currentEmployeeCode = _unitOfWork.SalesEmployeeRepository.GetSaleEmployeeCodeBy(AccountId);
                    if (task.taskAssignList.Select(p => p.SalesEmployeeCode).Contains(currentEmployeeCode) || task.Reporter == currentEmployeeCode)
                    {
                        isSavePermission = true;
                    }

                    //return type list of subtask based on WorkFlowId
                    subtaskTypeList = _unitOfWork.WorkFlowRepository.GetTypeByParentWorkFlow(task.WorkFlowId)
                                                                 .Select(p => new ISDSelectStringItem()
                                                                 {
                                                                     id = p.WorkFlowCategoryCode,
                                                                     name = p.WorkFlowCategoryName,
                                                                 }).ToList();

                    //Địa chỉ khách hàng
                    var profileAddressList = _unitOfWork.AddressBookRepository.GetAll(task.ProfileId);
                    if (profileAddressList != null && profileAddressList.Count > 0)
                    {
                        foreach (var item in profileAddressList)
                        {
                            item.Address += item.WardName + item.DistrictName + item.ProvinceName;
                            addressList.Add(item.Address);
                        }
                    }

                    //Ý kiến khách hàng(ratings)
                    task.CustomerRatings = task.Property5;
                    if (task.Property5 == "none")
                    {
                        task.Property5 = null;
                    }
                    else if (task.Property5 == "rating")
                    {
                        //task.Property5 = null;
                        task.Ticket_CustomerReviews_Product = _context.RatingModel.Where(p => p.ReferenceId == task.TaskId && p.RatingTypeCode == ConstCatalogType.Ticket_CustomerReviews_Product).Select(p => p.Ratings).FirstOrDefault();
                        task.Ticket_CustomerReviews_Service = _context.RatingModel.Where(p => p.ReferenceId == task.TaskId && p.RatingTypeCode == ConstCatalogType.Ticket_CustomerReviews_Service).Select(p => p.Ratings).FirstOrDefault();
                        task.Property5 = _context.RatingModel.Where(p => p.ReferenceId == task.TaskId && p.RatingTypeCode == null).Select(p => p.Reviews).FirstOrDefault();
                    }
                    //else if (task.Property5 == "other")
                    //{
                    //    task.Property5 = _context.RatingModel.Where(p => p.ReferenceId == task.TaskId && p.RatingTypeCode == null).Select(p => p.Reviews).FirstOrDefault();
                    //}
                }

                //set isRead when user click notification
                if (isRead == true)
                {
                    var currentNotif = _context.NotificationAccountMappingModel.Where(p => p.AccountId == AccountId.Value && p.NotificationId == NotificationId.Value).FirstOrDefault();
                    if (currentNotif != null)
                    {
                        currentNotif.IsRead = true;
                        _context.Entry(currentNotif).State = EntityState.Modified;
                        _context.SaveChanges();
                    }
                }

                //Chi nhánh
                var storeLst = _unitOfWork.StoreRepository.GetAllStore()
                                                   .Select(p => new ISDSelectGuidItem()
                                                   {
                                                       id = p.StoreId,
                                                       name = p.StoreName,
                                                   }).ToList();

                //Phân loại chuyến thăm
                var visitTypeLst = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.VisitType)
                                         .Select(p => new ISDSelectStringItem()
                                         {
                                             id = p.CatalogCode,
                                             name = p.CatalogText_vi,
                                         }).ToList();

                //Vai trò
                string taskAssignType = ConstCatalogType.TaskAssignType;

                var workFlowConfig = _context.WorkFlowConfigModel.Where(p => p.WorkFlowId == task.WorkFlowId && p.FieldCode == "RoleName").FirstOrDefault();
                if (workFlowConfig != null)
                {
                    taskAssignType = workFlowConfig.Parameters;
                }

                var taskAssignTypeLst = _unitOfWork.CatalogRepository.GetBy(taskAssignType)
                                                   .Select(p => new ISDSelectStringItem
                                                   {
                                                       id = p.CatalogCode,
                                                       name = p.CatalogText_vi,
                                                   }).ToList();

                var taskReporterTypeLst = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.TaskReporterType)
                                                  .Select(p => new ISDSelectStringItem
                                                  {
                                                      id = p.CatalogCode,
                                                      name = p.CatalogText_vi,
                                                  }).ToList();

                //Loại thời gian nhắc nhở
                var remindCycleLst = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.RemindCycle)
                                                                .Select(p => new ISDSelectStringItem()
                                                                {
                                                                    id = p.CatalogCode,
                                                                    name = p.CatalogText_vi,
                                                                });

                //Nguồn khách hàng
                var showroomList = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.CustomerSource)
                                                             .Select(p => new ISDSelectStringItem()
                                                             {
                                                                 id = p.CatalogCode,
                                                                 name = p.CatalogText_vi,
                                                             });

                //KH biết đến AC qua
                var channelList = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.Appoitment_Channel)
                                                            .Select(p => new ISDSelectStringItem()
                                                            {
                                                                id = p.CatalogCode,
                                                                name = p.CatalogText_vi,
                                                            });

                //NV tiếp khách
                var saleEmployeeList = _unitOfWork.SalesEmployeeRepository.GetAllForDropdownlist()
                                                                        .Select(p => new ISDSelectStringItem()
                                                                        {
                                                                            id = p.SalesEmployeeCode,
                                                                            name = p.SalesEmployeeName,
                                                                        });

                //Phân loại khách hàng (cũ/mới)
                var customerClassList = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.CustomerClass)
                                                                    .Select(p => new ISDSelectStringItem()
                                                                    {
                                                                        id = p.CatalogCode,
                                                                        name = p.CatalogText_vi,
                                                                    });

                //Nguồn tiếp nhận
                var taskSourceList = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.TaskSource)
                                                            .Select(p => new ISDRadioStringItem()
                                                            {
                                                                value = p.CatalogCode,
                                                                label = p.CatalogText_vi,
                                                            });

                //Hài lòng khách hàng
                var customerSatisfaction = _context.RatingModel.Where(p => p.ReferenceId == task.TaskId && p.RatingTypeCode == ConstCatalogType.CustomerSatisfaction)
                                                               .FirstOrDefault();
                if (customerSatisfaction != null)
                {
                    task.CustomerSatisfactionCode = customerSatisfaction.Ratings;
                    task.CustomerSatisfactionReviews = customerSatisfaction.Reviews;
                }
                var customerSatisfactionList = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.CustomerSatisfaction)
                                                            .Select(p => new ISDSelectStringItem()
                                                            {
                                                                id = p.CatalogCode,
                                                                name = p.CatalogText_vi,
                                                            });

                //Nhân viên kinh doanh của đơn vị thi công
                var SaleSupervisorList = (from p in _context.PersonInChargeModel
                                          join s in _context.SalesEmployeeModel on p.SalesEmployeeCode equals s.SalesEmployeeCode
                                          join acc in _context.AccountModel on s.SalesEmployeeCode equals acc.EmployeeCode
                                          from r in acc.RolesModel
                                          where p.ProfileId == task.ConstructionUnit
                                          && (CompanyCode == null || p.CompanyCode == CompanyCode)
                                          select new
                                          {
                                              SalesEmployeeCode = p.SalesEmployeeCode,
                                              SalesEmployeeName = s.SalesEmployeeName,
                                              DepartmentName = r.RolesName,
                                              isEmployeeGroup = r.isEmployeeGroup
                                          }).ToList();

                if (SaleSupervisorList != null && SaleSupervisorList.Count > 0)
                {
                    var SaleSupervisor = SaleSupervisorList.Where(p => p.isEmployeeGroup == true).FirstOrDefault();
                    if (SaleSupervisor == null)
                    {
                        SaleSupervisor = SaleSupervisorList.FirstOrDefault();
                    }
                    task.Construction_SalesSupervisorName = SaleSupervisor.SalesEmployeeName;
                }

                //Ý kiến khách hàng (ratings)
                var customerRatings = new List<ISDSelectStringItem>();
                customerRatings.Add(new ISDSelectStringItem()
                {
                    id = "none",
                    name = "Không ý kiến"
                });
                customerRatings.Add(new ISDSelectStringItem()
                {
                    id = "rating",
                    name = "Đánh giá theo sao & ý kiến khác"
                });
                //customerRatings.Add(new ISDSelectStringItem()
                //{
                //    id = "other",
                //    name = "Khác"
                //});
                //Đánh giá theo sao
                //1. Về sản phẩm
                var ticket_CustomerReviews_Product = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.Ticket_CustomerReviews_Product)
                                                                                .Select(p => new ISDSelectStringItem()
                                                                                {
                                                                                    id = p.CatalogCode,
                                                                                    name = p.CatalogText_vi,
                                                                                });
                //2. Về dịch vụ
                var ticket_CustomerReviews_Service = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.Ticket_CustomerReviews_Service)
                                                                              .Select(p => new ISDSelectStringItem()
                                                                              {
                                                                                  id = p.CatalogCode,
                                                                                  name = p.CatalogText_vi,
                                                                              });

                return _APISuccess(new
                {
                    Task = task,
                    StatusList = statusLst,
                    IssueTypeList = issueTypeLst,
                    ReporterList = reporterLst,
                    PriorityList = priorityLst,
                    AssigneeList = assigneeLst,
                    TransitionButtonList = stepButtonLst.Distinct(),
                    CreateByContent = string.Format("Được tạo bởi {0} lúc {1:dd/MM/yyyy HH:mm}", task.CreateByFullName, task.CreateTime),
                    LastEditByContent = !string.IsNullOrEmpty(task.LastEditByFullName) ? string.Format("Được cập nhập lần cuối bởi {0} lúc {1:dd/MM/yyyy HH:mm}", task.LastEditByFullName, task.LastEditTime) : null,
                    isCheckOut = isCheckOut,
                    isCheckInOutComplete = isCheckInOutComplete,
                    WorkFlowConfigList = configList,
                    StoreList = storeLst,
                    VisitTypeList = visitTypeLst,
                    TaskAssignTypeList = taskAssignTypeLst,
                    TaskReporterTypeList = taskReporterTypeLst,
                    RemindCycleList = remindCycleLst,
                    ShowroomList = showroomList,
                    ChannelList = channelList,
                    SaleEmployeeList = saleEmployeeList,
                    ContactList = contactList,
                    CustomerClassList = customerClassList,
                    IsDisabledSummary = isDisabledSummary,
                    IsSavePermission = isSavePermission,
                    SubtaskTypeList = subtaskTypeList,
                    SubtaskParentTaskId = task.TaskId,
                    TaskSourceList = taskSourceList,
                    AddressList = addressList,
                    CustomerSatisfactionList = customerSatisfactionList,
                    CustomerRatingsList = customerRatings,
                    ProductRatingsList = ticket_CustomerReviews_Product,
                    ServiceRatingsList = ticket_CustomerReviews_Service,
                });
            });
        }
        #endregion Get Task Detail

        #region Update Task

        #region Update Summary
        public ActionResult UpdateSummary(Guid AccountId, Guid TaskId, string Summary, string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                //Push Notification
                bool isSentNotification = false;
                string notificationMessage = string.Empty;
                string currentEmployeeCode = _context.AccountModel.Where(p => p.AccountId == AccountId).Select(p => p.EmployeeCode).FirstOrDefault();
                var currentAccountName = _unitOfWork.SalesEmployeeRepository.GetSaleEmployeeNameBy(currentEmployeeCode);

                var task = _context.TaskModel.Where(p => p.TaskId == TaskId).FirstOrDefault();
                if (task != null)
                {
                    //Push Notification 
                    if (task.Summary != Summary)
                    {
                        isSentNotification = true;
                        notificationMessage += string.Format(LanguageResource.TaskGeneralNotificationMessage, currentAccountName, LanguageResource.Task_Summary, task.Summary, Summary);
                    }
                    task.Summary = Summary;
                    task.LastEditBy = AccountId;
                    task.LastEditTime = DateTime.Now;

                    _context.CurrentAccountId = AccountId;
                    _context.Entry(task).State = EntityState.Modified;
                    _context.SaveChanges();

                    #region Push Notification
                    if (isSentNotification == true)
                    {
                        PushNotification(AccountId, TaskId, notificationMessage, task);
                    }
                    #endregion Push Notification

                }
                return _APISuccess(null);
            });
        }
        #endregion Update Summary

        #region Update Description
        public ActionResult UpdateDescription(Guid AccountId, Guid TaskId, string Description, string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                //Push Notification
                bool isSentNotification = false;
                string notificationMessage = string.Empty;
                string currentEmployeeCode = _context.AccountModel.Where(p => p.AccountId == AccountId).Select(p => p.EmployeeCode).FirstOrDefault();
                var currentAccountName = _unitOfWork.SalesEmployeeRepository.GetSaleEmployeeNameBy(currentEmployeeCode);

                var task = _context.TaskModel.Where(p => p.TaskId == TaskId).FirstOrDefault();
                if (task != null)
                {
                    //Push Notification 
                    if (task.Description != Description)
                    {
                        isSentNotification = true;
                        notificationMessage += string.Format(LanguageResource.TaskGeneralNotificationMessage, currentAccountName, LanguageResource.Description, task.Description, Description);
                    }
                    task.Description = Description;
                    task.LastEditBy = AccountId;
                    task.LastEditTime = DateTime.Now;

                    _context.CurrentAccountId = AccountId;
                    _context.Entry(task).State = EntityState.Modified;
                    _context.SaveChanges();

                    #region Push Notification
                    if (isSentNotification == true)
                    {
                        PushNotification(AccountId, TaskId, notificationMessage, task);
                    }
                    #endregion Push Notification

                }
                return _APISuccess(null);
            });
        }
        #endregion Update Description

        #region Update Private Task
        public ActionResult UpdatePrivateTask(Guid AccountId, Guid TaskId, bool isPrivate, string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                //Push Notification
                bool isSentNotification = false;
                string notificationMessage = string.Empty;
                string currentEmployeeCode = _context.AccountModel.Where(p => p.AccountId == AccountId).Select(p => p.EmployeeCode).FirstOrDefault();
                var currentAccountName = _unitOfWork.SalesEmployeeRepository.GetSaleEmployeeNameBy(currentEmployeeCode);

                var task = _context.TaskModel.Where(p => p.TaskId == TaskId).FirstOrDefault();
                if (task != null)
                {
                    //Push Notification 
                    if (task.isPrivate != isPrivate)
                    {
                        isSentNotification = true;
                        notificationMessage += string.Format(LanguageResource.TaskGeneralNotificationMessage, currentAccountName, LanguageResource.TaskStatus, task.isPrivate == true ? LanguageResource.Task_Private : LanguageResource.Task_Public, isPrivate == true ? LanguageResource.Task_Private : LanguageResource.Task_Public);
                    }
                    task.isPrivate = isPrivate;
                    task.LastEditBy = AccountId;
                    task.LastEditTime = DateTime.Now;

                    _context.CurrentAccountId = AccountId;
                    _context.Entry(task).State = EntityState.Modified;
                    _context.SaveChanges();

                    #region Push Notification
                    if (isSentNotification == true)
                    {
                        PushNotification(AccountId, TaskId, notificationMessage, task);
                    }
                    #endregion Push Notification

                }
                return _APISuccess(null);
            });
        }
        #endregion Update Private Task

        #region Update Task Status
        public ActionResult UpdateTaskStatus(Guid AccountId, Guid TaskId, Guid TaskStatusId, string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                //Push Notification
                bool isSentNotification = false;
                string notificationMessage = string.Empty;
                string currentEmployeeCode = _context.AccountModel.Where(p => p.AccountId == AccountId).Select(p => p.EmployeeCode).FirstOrDefault();
                var currentAccountName = _unitOfWork.SalesEmployeeRepository.GetSaleEmployeeNameBy(currentEmployeeCode);

                var task = _context.TaskModel.Where(p => p.TaskId == TaskId).FirstOrDefault();
                var taskStatus = new ISDSelectGuidItemWithColor();
                var stepButtonLst = new List<StepButtonViewModel>();
                if (task != null)
                {
                    //Push Notification 
                    if (task.TaskStatusId != TaskStatusId)
                    {
                        isSentNotification = true;
                        var fromStatus = _context.TaskStatusModel.Where(p => p.TaskStatusId == task.TaskStatusId).Select(p => p.TaskStatusName).FirstOrDefault();
                        var toStatus = _context.TaskStatusModel.Where(p => p.TaskStatusId == TaskStatusId).Select(p => p.TaskStatusName).FirstOrDefault();
                        notificationMessage += string.Format(LanguageResource.TaskGeneralNotificationMessage, currentAccountName, LanguageResource.TaskStatus, fromStatus, toStatus);
                    }

                    task.TaskStatusId = TaskStatusId;
                    task.LastEditBy = AccountId;
                    task.LastEditTime = DateTime.Now;

                    _context.CurrentAccountId = AccountId;
                    _context.Entry(task).State = EntityState.Modified;
                    _context.SaveChanges();

                    taskStatus = (from p in _context.TaskStatusModel
                                  where p.TaskStatusId == TaskStatusId
                                  orderby p.OrderIndex
                                  select new ISDSelectGuidItemWithColor()
                                  {
                                      id = p.TaskStatusId,
                                      name = p.TaskStatusName,
                                      color = p.ProcessCode == ConstProcess.todo ? "#000" : "#fff",
                                      bgColor = p.ProcessCode == ConstProcess.todo ? "#fff" : (p.ProcessCode == ConstProcess.processing ? "#0052CC" : "#398439"),
                                      iconType = "MaterialCommunityIcons",
                                      iconName = "progress-check"
                                  }).FirstOrDefault();

                    #region Step button
                    var statusTransitionLst = (from p in _context.StatusTransitionModel
                                               where p.WorkFlowId == task.WorkFlowId && p.FromStatusId == task.TaskStatusId
                                               select p).ToList();

                    //check if current account is reporter or assignee
                    var currentAccount = _context.AccountModel.Where(p => p.AccountId == AccountId).FirstOrDefault();
                    if (currentAccount != null)
                    {
                        //reporter
                        if (currentAccount.EmployeeCode == task.Reporter)
                        {
                            var reporterStatusTransitionLst = statusTransitionLst.Where(p => p.isReporterPermission == true)
                                                                                .Select(p => new StepButtonViewModel()
                                                                                {
                                                                                    TransitionName = p.TransitionName,
                                                                                    TransitionStatusId = p.ToStatusId,
                                                                                    Color = p.Color,
                                                                                }).ToList();
                            if (reporterStatusTransitionLst != null && reporterStatusTransitionLst.Count > 0)
                            {
                                stepButtonLst.AddRange(reporterStatusTransitionLst);
                            }
                        }
                        //assignee
                        var taskAssignList = _context.TaskAssignModel.Where(p => p.TaskId == TaskId).ToList();
                        if (taskAssignList != null && taskAssignList.Count > 0 && taskAssignList.Select(p => p.SalesEmployeeCode).Contains(currentAccount.EmployeeCode))
                        {
                            var asigneeStatusTransitionLst = statusTransitionLst.Where(p => p.isAssigneePermission == true)
                                                                                .Select(p => new StepButtonViewModel()
                                                                                {
                                                                                    TransitionName = p.TransitionName,
                                                                                    TransitionStatusId = p.ToStatusId,
                                                                                    Color = p.Color,
                                                                                }).ToList();
                            if (asigneeStatusTransitionLst != null && asigneeStatusTransitionLst.Count > 0)
                            {
                                stepButtonLst.AddRange(asigneeStatusTransitionLst);
                            }
                        }
                    }

                    if (stepButtonLst != null && stepButtonLst.Count > 0)
                    {
                        stepButtonLst = stepButtonLst.GroupBy(m => new { m.TransitionName, m.TransitionStatusId, m.Color, m.isRequiredComment })
                                                    .Select(group => group.First())  // instead of First you can also apply your logic here what you want to take, for example an OrderBy
                                                    .ToList();
                    }

                    #endregion Step button

                    #region Push Notification
                    if (isSentNotification == true)
                    {
                        PushNotification(AccountId, TaskId, notificationMessage, task);
                    }
                    #endregion Push Notification

                }
                return _APISuccess(new { taskStatus = taskStatus, transitionButtonList = stepButtonLst });
            });
        }
        #endregion Update Task Status

        #region Update Task Status With Transition
        public ActionResult UpdateTaskStatusWithTransition(Guid AccountId, Guid TaskId, Guid FromTaskStatusId, Guid ToTaskStatusId, string TaskStatusComment, string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                //Push Notification
                bool isSentNotification = false;
                string notificationMessage = string.Empty;
                string currentEmployeeCode = _context.AccountModel.Where(p => p.AccountId == AccountId).Select(p => p.EmployeeCode).FirstOrDefault();
                var currentAccountName = _unitOfWork.SalesEmployeeRepository.GetSaleEmployeeNameBy(currentEmployeeCode);

                var task = _context.TaskModel.Where(p => p.TaskId == TaskId).FirstOrDefault();
                var taskStatus = new ISDSelectGuidItemWithColor();
                var stepButtonLst = new List<StepButtonViewModel>();
                if (task != null)
                {
                    //Push Notification 
                    if (task.TaskStatusId != ToTaskStatusId)
                    {
                        isSentNotification = true;
                        var fromStatus = _context.TaskStatusModel.Where(p => p.TaskStatusId == task.TaskStatusId).Select(p => p.TaskStatusName).FirstOrDefault();
                        var toStatus = _context.TaskStatusModel.Where(p => p.TaskStatusId == ToTaskStatusId).Select(p => p.TaskStatusName).FirstOrDefault();
                        notificationMessage += string.Format(LanguageResource.TaskGeneralNotificationMessage, currentAccountName, LanguageResource.TaskStatus, fromStatus, toStatus);
                    }

                    task.TaskStatusId = ToTaskStatusId;
                    task.LastEditBy = AccountId;
                    task.LastEditTime = DateTime.Now;

                    _context.CurrentAccountId = AccountId;
                    _context.Entry(task).State = EntityState.Modified;

                    //Comment
                    //Comment
                    TaskCommentModel comment = new TaskCommentModel();
                    comment.TaskCommentId = Guid.NewGuid();
                    comment.TaskId = TaskId;
                    comment.Comment = TaskStatusComment == "undefined" ? string.Empty : TaskStatusComment;
                    comment.FromStatusId = FromTaskStatusId;
                    comment.ToStatusId = ToTaskStatusId;
                    comment.CreateBy = AccountId;
                    comment.CreateTime = DateTime.Now;

                    //Save task comment
                    _context.Entry(comment).State = EntityState.Added;
                    _context.SaveChanges();

                    taskStatus = (from p in _context.TaskStatusModel
                                  where p.TaskStatusId == ToTaskStatusId
                                  orderby p.OrderIndex
                                  select new ISDSelectGuidItemWithColor()
                                  {
                                      id = p.TaskStatusId,
                                      name = p.TaskStatusName,
                                      color = p.ProcessCode == ConstProcess.todo ? "#000" : "#fff",
                                      bgColor = p.ProcessCode == ConstProcess.todo ? "#fff" : (p.ProcessCode == ConstProcess.processing ? "#0052CC" : "#398439"),
                                      iconType = "MaterialCommunityIcons",
                                      iconName = "progress-check"
                                  }).FirstOrDefault();

                    #region Step button
                    var statusTransitionLst = (from p in _context.StatusTransitionModel
                                               where p.WorkFlowId == task.WorkFlowId && p.FromStatusId == task.TaskStatusId
                                               select p).ToList();

                    //check if current account is reporter or assignee
                    var currentAccount = _context.AccountModel.Where(p => p.AccountId == AccountId).FirstOrDefault();
                    if (currentAccount != null)
                    {
                        //reporter
                        if (currentAccount.EmployeeCode == task.Reporter)
                        {
                            var reporterStatusTransitionLst = statusTransitionLst.Where(p => p.isReporterPermission == true)
                                                                                .Select(p => new StepButtonViewModel()
                                                                                {
                                                                                    TransitionName = p.TransitionName,
                                                                                    TransitionStatusId = p.ToStatusId,
                                                                                    Color = p.Color,
                                                                                    isRequiredComment = p.isRequiredComment,
                                                                                }).ToList();
                            if (reporterStatusTransitionLst != null && reporterStatusTransitionLst.Count > 0)
                            {
                                stepButtonLst.AddRange(reporterStatusTransitionLst);
                            }
                        }
                        //assignee
                        var taskAssignList = _context.TaskAssignModel.Where(p => p.TaskId == TaskId).ToList();
                        if (taskAssignList != null && taskAssignList.Count > 0 && taskAssignList.Select(p => p.SalesEmployeeCode).Contains(currentAccount.EmployeeCode))
                        {
                            var asigneeStatusTransitionLst = statusTransitionLst.Where(p => p.isAssigneePermission == true)
                                                                                .Select(p => new StepButtonViewModel()
                                                                                {
                                                                                    TransitionName = p.TransitionName,
                                                                                    TransitionStatusId = p.ToStatusId,
                                                                                    Color = p.Color,
                                                                                    isRequiredComment = p.isRequiredComment,
                                                                                }).ToList();
                            if (asigneeStatusTransitionLst != null && asigneeStatusTransitionLst.Count > 0)
                            {
                                stepButtonLst.AddRange(asigneeStatusTransitionLst);
                            }
                        }
                    }

                    #endregion Step button

                    #region Push Notification
                    if (isSentNotification == true)
                    {
                        PushNotification(AccountId, TaskId, notificationMessage, task);
                    }
                    #endregion Push Notification

                }
                return _APISuccess(new { taskStatus = taskStatus, transitionButtonList = stepButtonLst });
            });
        }
        #endregion Update Task Status With Transition

        #region Update Assignee
        public ActionResult UpdateAssignee(Guid AccountId, Guid TaskId, string Assignee, string type, string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                //Push Notification
                bool isSentNotification = false;
                string notificationMessage = string.Empty;
                string currentEmployeeCode = _context.AccountModel.Where(p => p.AccountId == AccountId).Select(p => p.EmployeeCode).FirstOrDefault();
                var currentAccountName = _unitOfWork.SalesEmployeeRepository.GetSaleEmployeeNameBy(currentEmployeeCode);

                if (type == ConstUpdateType.Add)
                {
                    var taskAssign = new TaskAssignModel();
                    taskAssign.TaskAssignId = Guid.NewGuid();
                    taskAssign.TaskId = TaskId;
                    taskAssign.SalesEmployeeCode = Assignee;
                    taskAssign.CreateBy = AccountId;
                    taskAssign.CreateTime = DateTime.Now;

                    _context.Entry(taskAssign).State = EntityState.Added;

                    isSentNotification = true;
                    var toAssignee = _unitOfWork.SalesEmployeeRepository.GetSaleEmployeeNameBy(Assignee);

                    notificationMessage = string.Format(LanguageResource.TaskAssigneeNotificationMessage, currentAccountName, LanguageResource.Change.ToLower(), LanguageResource.Assignee);
                }
                else if (type == ConstUpdateType.Remove)
                {
                    var taskAssign = _context.TaskAssignModel.Where(p => p.TaskId == TaskId && p.SalesEmployeeCode == Assignee).FirstOrDefault();
                    if (taskAssign != null)
                    {
                        _context.Entry(taskAssign).State = EntityState.Deleted;

                        isSentNotification = true;
                        var fromAssignee = _unitOfWork.SalesEmployeeRepository.GetSaleEmployeeNameBy(Assignee);
                        notificationMessage = string.Format(LanguageResource.TaskAssigneeNotificationMessage, currentAccountName, LanguageResource.Change.ToLower(), LanguageResource.Assignee);
                    }
                }

                _context.CurrentAccountId = AccountId;
                _context.SaveChanges();

                //Get new assign list
                var taskAssignList = (from p in _context.TaskAssignModel
                                      join s in _context.SalesEmployeeModel on p.SalesEmployeeCode equals s.SalesEmployeeCode
                                      join c in _context.CatalogModel on new { TaskAssignTypeCode = p.TaskAssignTypeCode, Type = ConstCatalogType.TaskAssignType } equals new { TaskAssignTypeCode = c.CatalogCode, Type = c.CatalogTypeCode } into taskAssignTypeTemp
                                      from taskAssignType in taskAssignTypeTemp.DefaultIfEmpty()
                                      where p.TaskId == TaskId
                                      orderby p.CreateTime
                                      select new TaskAssignViewModel()
                                      {
                                          TaskAssignId = p.TaskAssignId,
                                          SalesEmployeeCode = p.SalesEmployeeCode,
                                          TaskAssignTypeCode = p.TaskAssignTypeCode,
                                          TaskAssignTypeName = taskAssignType.CatalogText_vi,
                                          SalesEmployeeName = s.SalesEmployeeName
                                      }).ToList();

                //Phòng ban theo NV được phân công
                if (taskAssignList != null && taskAssignList.Count > 0)
                {
                    foreach (var item in taskAssignList)
                    {
                        var role = (from acc in _context.AccountModel
                                    from r in acc.RolesModel
                                    where acc.EmployeeCode == item.SalesEmployeeCode
                                    && r.isEmployeeGroup == true
                                    select r.RolesName).FirstOrDefault();
                        item.RoleName = role != null ? role : "";
                    }
                }

                if (taskAssignList != null && taskAssignList.Count() > 0)
                {
                    foreach (var assignee in taskAssignList)
                    {
                        if (!string.IsNullOrEmpty(assignee.SalesEmployeeName))
                        {
                            assignee.LogoName = assignee.SalesEmployeeName.GetCharacterForLogoName();
                        }

                        //Phòng ban theo NV được phân công
                        var role = (from acc in _context.AccountModel
                                    from r in acc.RolesModel
                                    where acc.EmployeeCode == assignee.SalesEmployeeCode
                                    && r.isEmployeeGroup == true
                                    select r.RolesName).FirstOrDefault();
                        assignee.RoleName = role != null ? role : "";
                    }
                }

                #region Push Notification
                if (isSentNotification == true)
                {
                    var task = _context.TaskModel.Where(p => p.TaskId == TaskId).FirstOrDefault();
                    PushNotification(AccountId, TaskId, notificationMessage, task);
                }
                #endregion Push Notification

                return _APISuccess(taskAssignList);
            });
        }
        #endregion Update Assignee

        #region Update Reporter
        public ActionResult UpdateReporter(Guid AccountId, Guid TaskId, string Reporter, string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                //Push Notification
                bool isSentNotification = false;
                string notificationMessage = string.Empty;
                string currentEmployeeCode = _context.AccountModel.Where(p => p.AccountId == AccountId).Select(p => p.EmployeeCode).FirstOrDefault();
                var currentAccountName = _unitOfWork.SalesEmployeeRepository.GetSaleEmployeeNameBy(currentEmployeeCode);

                var task = _context.TaskModel.Where(p => p.TaskId == TaskId).FirstOrDefault();
                string ReporterName = string.Empty;
                string ReporterLogoName = string.Empty;
                if (task != null)
                {
                    #region //notification
                    if (task.Reporter != Reporter)
                    {
                        isSentNotification = true;
                        var fromReporter = _unitOfWork.SalesEmployeeRepository.GetSaleEmployeeNameBy(task.Reporter);
                        var toReporter = _unitOfWork.SalesEmployeeRepository.GetSaleEmployeeNameBy(Reporter);
                        notificationMessage += string.Format(LanguageResource.TaskGeneralNotificationMessage, currentAccountName, LanguageResource.Reporter, fromReporter, toReporter);
                    }
                    #endregion

                    #region Cập nhật "NV theo dõi/giám sát"
                    task.Reporter = Reporter;
                    task.LastEditBy = AccountId;
                    task.LastEditTime = DateTime.Now;

                    _context.CurrentAccountId = AccountId;
                    _context.Entry(task).State = EntityState.Modified;
                    _context.SaveChanges();
                    #endregion

                    var reporterName = _unitOfWork.SalesEmployeeRepository.GetSaleEmployeeNameBy(Reporter);
                    if (!string.IsNullOrEmpty(reporterName))
                    {
                        ReporterName = reporterName;
                        ReporterLogoName = reporterName?.GetCharacterForLogoName();
                    }

                    #region Push Notification
                    if (isSentNotification == true)
                    {
                        PushNotification(AccountId, TaskId, notificationMessage, task);
                    }
                    #endregion Push Notification
                }

                return _APISuccess(new { Reporter, ReporterName, ReporterLogoName });
            });
        }
        #endregion Update Reporter

        #region Update Priority
        public ActionResult UpdatePriority(Guid AccountId, Guid TaskId, string PriorityCode, string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                var task = _context.TaskModel.Where(p => p.TaskId == TaskId).FirstOrDefault();
                var priority = new ISDSelectStringItemWithColor();
                if (task != null)
                {
                    task.PriorityCode = PriorityCode;
                    task.LastEditBy = AccountId;
                    task.LastEditTime = DateTime.Now;

                    _context.CurrentAccountId = AccountId;
                    _context.Entry(task).State = EntityState.Modified;
                    _context.SaveChanges();

                    priority = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.Priority).Where(p => p.CatalogCode == PriorityCode).Select(p => new ISDSelectStringItemWithColor()
                    {
                        id = p.CatalogCode,
                        name = p.CatalogText_vi,
                        color = p.CatalogCode == ConstPriotityCode.LOW ? "#008000" : (p.CatalogCode == ConstPriotityCode.NORMAL ? "#FFA500" : (p.CatalogCode == ConstPriotityCode.HIGH ? "#FF4500" : "#FF0000")),
                        iconType = "AntDesign",
                        iconName = p.CatalogCode == ConstPriotityCode.LOW ? "arrowdown" : "arrowup",
                    }).FirstOrDefault();
                }

                return _APISuccess(priority);
            });
        }
        #endregion Update Priority

        #region Update Attachment
        public ActionResult UpdateAttachment(Guid AccountId, Guid TaskId, List<HttpPostedFileBase> AttachmentFile, string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                if (AttachmentFile != null && AttachmentFile.Count > 0)
                {
                    foreach (var attachment in AttachmentFile)
                    {
                        FileAttachmentModel fileNew = SaveFileAttachment(AccountId, TaskId, attachment);

                        //Task File mapping
                        Task_File_Mapping mapping = new Task_File_Mapping();
                        mapping.FileAttachmentId = fileNew.FileAttachmentId;
                        mapping.TaskId = TaskId;
                        _context.Entry(mapping).State = EntityState.Added;
                    }

                    _context.CurrentAccountId = AccountId;
                    _context.SaveChanges();


                }

                var commentList = _unitOfWork.TaskRepository.GetTaskCommentList(TaskId);
                var taskFileList = _unitOfWork.TaskRepository.GetTaskFileList(commentList, TaskId);
                if (taskFileList != null && taskFileList.Count > 0)
                {
                    foreach (var file in taskFileList)
                    {
                        file.FileUrl = ConstDomain.Domain + "/Upload/Document/" + file.FileUrl;
                    }
                }

                return _APISuccess(taskFileList);
            });
        }

        private FileAttachmentModel SaveFileAttachment(Guid AccountId, Guid ObjectId, HttpPostedFileBase item)
        {
            //FileAttachmentModel
            string FileExtension = _unitOfWork.UtilitiesRepository.FileExtension(item.FileName);
            string FileType = _unitOfWork.UtilitiesRepository.GetFileTypeByExtension(FileExtension);
            var fileNew = new FileAttachmentModel();
            //1. GUID
            fileNew.FileAttachmentId = Guid.NewGuid();
            //2. Mã Profile
            fileNew.ObjectId = ObjectId;
            //3. Tên file
            fileNew.FileAttachmentName = item.FileName;
            //4. Đường dẫn
            fileNew.FileUrl = UploadDocumentFile(item, "Document", ConstDomain.DocumentDomain, FileType: FileType);
            //5. Đuôi file
            fileNew.FileExtention = FileExtension;
            //7. Loại file
            fileNew.FileAttachmentCode = FileType;
            //7. Người tạo
            fileNew.CreateBy = AccountId;
            //8. Thời gian tạo
            fileNew.CreateTime = DateTime.Now;
            _context.Entry(fileNew).State = EntityState.Added;
            return fileNew;
        }

        public ActionResult DeleteAttachment(Guid AccountId, Guid? FileAttachmentId, string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                if (FileAttachmentId.HasValue)
                {
                    var file = _context.FileAttachmentModel.FirstOrDefault(p => p.FileAttachmentId == FileAttachmentId);
                    if (file != null)
                    {
                        //Delete in Task_File_Mapping
                        var task_mapp = _context.Task_File_Mapping.FirstOrDefault(p => p.FileAttachmentId == FileAttachmentId);
                        if (task_mapp != null)
                        {
                            _context.Entry(task_mapp).State = EntityState.Deleted;
                        }
                        //Delete in Comment_File_Mapping
                        var comment_mapp = _context.Comment_File_Mapping.FirstOrDefault(p => p.FileAttachmentId == FileAttachmentId);
                        if (comment_mapp != null)
                        {
                            _context.Entry(comment_mapp).State = EntityState.Deleted;
                        }
                        //Delete in FileAttachmentModel
                        _context.Entry(file).State = EntityState.Deleted;

                        _context.CurrentAccountId = AccountId;
                        _context.SaveChanges();
                    }
                }

                return _APISuccess(null);
            });
        }
        #endregion Update Attachment

        #region Update Comment
        public ActionResult UpdateComment(Guid AccountId, Guid TaskId, string Comment, List<HttpPostedFileBase> AttachmentFile, string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                //Push Notification
                bool isSentNotification = false;
                string notificationMessage = string.Empty;
                string currentEmployeeCode = _context.AccountModel.Where(p => p.AccountId == AccountId).Select(p => p.EmployeeCode).FirstOrDefault();
                var currentAccountName = _unitOfWork.SalesEmployeeRepository.GetSaleEmployeeNameBy(currentEmployeeCode);

                if (!string.IsNullOrEmpty(Comment) && Comment != "undefined" && Comment != "null")
                {
                    isSentNotification = true;
                }

                var task = _context.TaskModel.Where(p => p.TaskId == TaskId).FirstOrDefault();
                if (task != null)
                {
                    //Comment
                    TaskCommentModel comment = new TaskCommentModel();
                    comment.TaskCommentId = Guid.NewGuid();
                    comment.TaskId = TaskId;
                    comment.Comment = Comment == "undefined" ? string.Empty : Comment;
                    comment.FromStatusId = task.TaskStatusId;
                    comment.ToStatusId = task.TaskStatusId;
                    comment.CreateBy = AccountId;
                    comment.CreateTime = DateTime.Now;

                    //Save task comment
                    _context.Entry(comment).State = EntityState.Added;

                    //AttachmentFile
                    if (AttachmentFile != null && AttachmentFile.Count > 0)
                    {
                        foreach (var item in AttachmentFile)
                        {
                            FileAttachmentModel fileNew = SaveFileAttachment(AccountId, comment.TaskCommentId, item);

                            //Comment File mapping
                            Comment_File_Mapping mapping = new Comment_File_Mapping();
                            mapping.FileAttachmentId = fileNew.FileAttachmentId;
                            mapping.TaskCommentId = comment.TaskCommentId;
                            _context.Entry(mapping).State = EntityState.Added;
                        }
                    }

                    _context.CurrentAccountId = AccountId;
                    _context.SaveChanges();

                    notificationMessage = string.Format(LanguageResource.TaskCommentNotificationMessage, currentAccountName);

                }

                //Load comments of task
                #region Comments
                var taskCommentList = _unitOfWork.TaskRepository.GetTaskCommentList(TaskId);
                #endregion

                //Load files attachment upload
                #region File Attachment
                var taskFileList = _unitOfWork.TaskRepository.GetTaskFileList(taskCommentList, TaskId);
                if (taskFileList != null && taskFileList.Count > 0)
                {
                    foreach (var file in taskFileList)
                    {
                        file.FileUrl = ConstDomain.Domain + "/Upload/Document/" + file.FileUrl;
                    }
                }
                #endregion

                #region Push Notification
                if (isSentNotification == true)
                {
                    PushNotification(AccountId, TaskId, notificationMessage, task);
                }
                #endregion Push Notification

                return _APISuccess(new { taskCommentList, taskFileList });
            });
        }
        #endregion Update Comment

        #region Update Receive Date
        public ActionResult UpdateReceiveDate(Guid AccountId, Guid TaskId, string ReceiveDate, string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                if (!string.IsNullOrEmpty(ReceiveDate))
                {

                    var ReceiveDateValue = _unitOfWork.RepositoryLibrary.VNStringToDateTime(ReceiveDate);

                    //Push Notification
                    bool isSentNotification = false;
                    string notificationMessage = string.Empty;
                    string currentEmployeeCode = _context.AccountModel.Where(p => p.AccountId == AccountId).Select(p => p.EmployeeCode).FirstOrDefault();
                    var currentAccountName = _unitOfWork.SalesEmployeeRepository.GetSaleEmployeeNameBy(currentEmployeeCode);

                    var task = _context.TaskModel.Where(p => p.TaskId == TaskId).FirstOrDefault();
                    if (task != null)
                    {
                        //Push Notification 
                        if (task.ReceiveDate != ReceiveDateValue)
                        {
                            isSentNotification = true;
                            notificationMessage += string.Format(LanguageResource.TaskGeneralNotificationMessage, currentAccountName, LanguageResource.Task_ReceiveDate, task.ReceiveDate.HasValue ? task.ReceiveDate.Value.ToString("dd/MM/yyyy") : "dd/MM/yyyy", ReceiveDateValue.Value.ToString("dd/MM/yyyy"));
                        }
                        task.ReceiveDate = ReceiveDateValue;
                        task.LastEditBy = AccountId;
                        task.LastEditTime = DateTime.Now;

                        _context.CurrentAccountId = AccountId;
                        _context.Entry(task).State = EntityState.Modified;
                        _context.SaveChanges();

                        #region Push Notification
                        if (isSentNotification == true)
                        {
                            //PushNotification(AccountId, TaskId, notificationMessage, task);
                        }
                        #endregion Push Notification

                    }
                    return _APISuccess(null);
                }
                return _APIError("Vui lòng chọn ngày tiếp nhận!");
            });
        }
        #endregion Update Receive Date

        #region Update Start Date
        public ActionResult UpdateStartDate(Guid AccountId, Guid TaskId, string StartDate, string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                if (!string.IsNullOrEmpty(StartDate))
                {

                    var StartDateValue = _unitOfWork.RepositoryLibrary.VNStringToDateTime(StartDate);

                    //Push Notification
                    bool isSentNotification = false;
                    string notificationMessage = string.Empty;
                    string currentEmployeeCode = _context.AccountModel.Where(p => p.AccountId == AccountId).Select(p => p.EmployeeCode).FirstOrDefault();
                    var currentAccountName = _unitOfWork.SalesEmployeeRepository.GetSaleEmployeeNameBy(currentEmployeeCode);

                    var task = _context.TaskModel.Where(p => p.TaskId == TaskId).FirstOrDefault();
                    if (task != null)
                    {
                        //Push Notification 
                        if (task.StartDate != StartDateValue)
                        {
                            isSentNotification = true;
                            notificationMessage += string.Format(LanguageResource.TaskGeneralNotificationMessage, currentAccountName, LanguageResource.Task_StartDate, task.StartDate.HasValue ? task.StartDate.Value.ToString("dd/MM/yyyy") : "dd/MM/yyyy", StartDateValue.Value.ToString("dd/MM/yyyy"));
                        }
                        task.StartDate = StartDateValue;
                        task.LastEditBy = AccountId;
                        task.LastEditTime = DateTime.Now;

                        _context.CurrentAccountId = AccountId;
                        _context.Entry(task).State = EntityState.Modified;
                        _context.SaveChanges();

                        #region Push Notification
                        if (isSentNotification == true)
                        {
                            //PushNotification(AccountId, TaskId, notificationMessage, task);
                        }
                        #endregion Push Notification

                    }
                    return _APISuccess(null);
                }
                return _APIError("Vui lòng chọn ngày tiếp nhận!");
            });
        }
        #endregion Update Start Date

        #region Update Estimate End Date
        public ActionResult UpdateEstimateEndDate(Guid AccountId, Guid TaskId, string EstimateEndDate, string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                if (!string.IsNullOrEmpty(EstimateEndDate))
                {

                    var EstimateEndDateValue = _unitOfWork.RepositoryLibrary.VNStringToDateTime(EstimateEndDate);

                    //Push Notification
                    bool isSentNotification = false;
                    string notificationMessage = string.Empty;
                    string currentEmployeeCode = _context.AccountModel.Where(p => p.AccountId == AccountId).Select(p => p.EmployeeCode).FirstOrDefault();
                    var currentAccountName = _unitOfWork.SalesEmployeeRepository.GetSaleEmployeeNameBy(currentEmployeeCode);

                    var task = _context.TaskModel.Where(p => p.TaskId == TaskId).FirstOrDefault();
                    if (task != null)
                    {
                        //Push Notification 
                        if (task.EstimateEndDate != EstimateEndDateValue)
                        {
                            isSentNotification = true;
                            notificationMessage += string.Format(LanguageResource.TaskGeneralNotificationMessage, currentAccountName, LanguageResource.Task_EstimateEndDate, task.EstimateEndDate.HasValue ? task.EstimateEndDate.Value.ToString("dd/MM/yyyy") : "dd/MM/yyyy", EstimateEndDateValue.Value.ToString("dd/MM/yyyy"));
                        }
                        task.EstimateEndDate = EstimateEndDateValue;
                        task.LastEditBy = AccountId;
                        task.LastEditTime = DateTime.Now;

                        _context.CurrentAccountId = AccountId;
                        _context.Entry(task).State = EntityState.Modified;
                        _context.SaveChanges();

                        #region Push Notification
                        if (isSentNotification == true)
                        {
                            //PushNotification(AccountId, TaskId, notificationMessage, task);
                        }
                        #endregion Push Notification

                    }
                    return _APISuccess(null);
                }
                return _APIError("Vui lòng chọn ngày tiếp nhận!");
            });
        }
        #endregion Update Estimate End Date

        #region Update End Date
        public ActionResult UpdateEndDate(Guid AccountId, Guid TaskId, string EndDate, string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                if (!string.IsNullOrEmpty(EndDate))
                {

                    var EndDateValue = _unitOfWork.RepositoryLibrary.VNStringToDateTime(EndDate);

                    //Push Notification
                    bool isSentNotification = false;
                    string notificationMessage = string.Empty;
                    string currentEmployeeCode = _context.AccountModel.Where(p => p.AccountId == AccountId).Select(p => p.EmployeeCode).FirstOrDefault();
                    var currentAccountName = _unitOfWork.SalesEmployeeRepository.GetSaleEmployeeNameBy(currentEmployeeCode);

                    var task = _context.TaskModel.Where(p => p.TaskId == TaskId).FirstOrDefault();
                    if (task != null)
                    {
                        //Push Notification 
                        if (task.EndDate != EndDateValue)
                        {
                            isSentNotification = true;
                            notificationMessage += string.Format(LanguageResource.TaskGeneralNotificationMessage, currentAccountName, LanguageResource.Task_EndDate, task.EndDate.HasValue ? task.EndDate.Value.ToString("dd/MM/yyyy") : "dd/MM/yyyy", EndDateValue.Value.ToString("dd/MM/yyyy"));
                        }
                        task.EndDate = EndDateValue;
                        task.LastEditBy = AccountId;
                        task.LastEditTime = DateTime.Now;

                        _context.CurrentAccountId = AccountId;
                        _context.Entry(task).State = EntityState.Modified;
                        _context.SaveChanges();

                        #region Push Notification
                        if (isSentNotification == true)
                        {
                            //PushNotification(AccountId, TaskId, notificationMessage, task);
                        }
                        #endregion Push Notification

                    }
                    return _APISuccess(null);
                }
                return _APIError("Vui lòng chọn ngày tiếp nhận!");
            });
        }
        #endregion Update Estimate End Date

        #region Update TaskAssignType
        public ActionResult UpdateTaskAssignType(Guid AccountId, Guid TaskAssignId, string TaskAssignTypeCode, string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                var task = _context.TaskAssignModel.Where(p => p.TaskAssignId == TaskAssignId).FirstOrDefault();
                if (task != null)
                {
                    if (task.TaskAssignTypeCode != TaskAssignTypeCode)
                    {
                        task.TaskAssignTypeCode = TaskAssignTypeCode;

                        var taskMaster = _context.TaskModel.Where(p => p.TaskId == task.TaskId).FirstOrDefault();
                        if (taskMaster != null)
                        {
                            taskMaster.LastEditBy = AccountId;
                            taskMaster.LastEditTime = DateTime.Now;

                            _context.Entry(taskMaster).State = EntityState.Modified;
                        }

                        _context.CurrentAccountId = AccountId;
                        _context.Entry(task).State = EntityState.Modified;
                        _context.SaveChanges();
                    }
                }
                return _APISuccess(null);
            });
        }
        #endregion Update TaskAssignType

        #region Update VisitType
        public ActionResult UpdateVisitType(Guid AccountId, Guid TaskId, string VisitTypeCode, string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                var task = _context.TaskModel.Where(p => p.TaskId == TaskId).FirstOrDefault();
                if (task != null)
                {
                    if (task.VisitTypeCode != VisitTypeCode)
                    {
                        task.VisitTypeCode = VisitTypeCode;

                        _context.CurrentAccountId = AccountId;
                        _context.Entry(task).State = EntityState.Modified;
                        _context.SaveChanges();
                    }
                }
                return _APISuccess(null);
            });
        }
        #endregion Update VisitType

        #region Update VisitAddress
        public ActionResult UpdateVisitAddress(Guid AccountId, Guid TaskId, string VisitAddress, string lat, string lng, string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                var task = _context.TaskModel.Where(p => p.TaskId == TaskId).FirstOrDefault();
                if (task != null)
                {
                    if (task.VisitAddress != VisitAddress)
                    {
                        task.VisitAddress = VisitAddress;
                        if (!string.IsNullOrEmpty(VisitAddress))
                        {
                            if (string.IsNullOrEmpty(lat) || string.IsNullOrEmpty(lng))
                            {
                                var locationService = new GoogleLocationService(GoogleMapAPIKey);
                                var point = locationService.GetLatLongFromAddress(VisitAddress);
                                task.lat = point.Latitude.ToString();
                                task.lng = point.Longitude.ToString();
                            }
                        }

                        _context.CurrentAccountId = AccountId;
                        _context.Entry(task).State = EntityState.Modified;
                        _context.SaveChanges();
                    }
                }
                return _APISuccess(null);
            });
        }
        #endregion Update VisitAddress

        #region Update Remind
        public ActionResult UpdateRemind(Guid AccountId, Guid TaskId, bool? isRemind, int? RemindTime, string RemindCycle, string RemindStartDate_String, string RemindStartTime_String, bool? isRemindForAssignee, bool? isRemindForReporter, string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                var task = _context.TaskModel.Where(p => p.TaskId == TaskId).FirstOrDefault();
                if (task != null)
                {
                    task.isRemind = isRemind;
                    if (task.isRemind == true)
                    {
                        task.RemindTime = RemindTime;
                        task.RemindCycle = RemindCycle;
                        task.isRemindForReporter = isRemindForReporter;
                        task.isRemindForAssignee = isRemindForAssignee;
                        if (!string.IsNullOrEmpty(RemindStartDate_String))
                        {
                            task.RemindStartDate = _unitOfWork.RepositoryLibrary.VNStringToDateTime(RemindStartDate_String);
                            if (!string.IsNullOrEmpty(RemindStartTime_String))
                            {
                                var remindStartTime = _unitOfWork.RepositoryLibrary.VNStringToTimeSpan(RemindStartTime_String);
                                task.RemindStartDate = task.RemindStartDate.Value.Add(remindStartTime.Value);
                            }
                        }
                    }

                    _context.CurrentAccountId = AccountId;
                    _context.Entry(task).State = EntityState.Modified;
                    _context.SaveChanges();

                    var taskViewModel = new TaskViewModel();
                    taskViewModel.TaskId = TaskId;
                    taskViewModel.RemindTime = RemindTime;
                    taskViewModel.RemindCycle = RemindCycle;
                    taskViewModel.RemindStartDate = task.RemindStartDate;
                    taskViewModel.isRemind = isRemind;
                    //Update remind task
                    _unitOfWork.TaskRepository.UpdateRemindTask(taskViewModel, AccountId);
                }
                return _APISuccess(null);
            });
        }
        #endregion Update Remind

        #region Update Profile
        public ActionResult UpdateProfile(Guid AccountId, Guid TaskId, Guid? ProfileId, string ProfileName, string ProfileAddress, Guid? ContactId, string ContactName, string ContactAddress, string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                var task = _context.TaskModel.Where(p => p.TaskId == TaskId).FirstOrDefault();
                if (task != null)
                {
                    #region Profile/ Khách hàng
                    //So sánh Profile:
                    //1. Nếu trong DB có ProfileId nhưng truyền lên không có ProfileId => xóa Profile 
                    //2. Nếu trong DB không có ProfileId và tham số truyền lên có ProfileId => thêm mới Profile 
                    if (ProfileId == null)
                    {
                        task.ProfileId = null;
                        var taskReference = _context.TaskReferenceModel.Where(p => p.TaskId == TaskId && p.Type == ConstTaskReference.Account).FirstOrDefault();
                        if (taskReference != null)
                        {
                            _context.Entry(taskReference).State = EntityState.Deleted;
                        }
                    }
                    else
                    {
                        task.ProfileId = ProfileId;
                        var taskReference = _context.TaskReferenceModel.Where(p => p.TaskId == TaskId && p.ObjectId == ProfileId).FirstOrDefault();
                        if (taskReference == null)
                        {
                            TaskReferenceModel referenceAccount = new TaskReferenceModel();
                            referenceAccount.ObjectId = ProfileId;
                            referenceAccount.Type = ConstTaskReference.Account;
                            referenceAccount.TaskId = TaskId;
                            referenceAccount.CreateBy = AccountId;
                            _unitOfWork.TaskRepository.CreateTaskReference(referenceAccount);
                        }
                    }
                    #endregion

                    #region Contact/ Liên hệ chính
                    //So sánh tương tự Profile
                    if (ContactId == null)
                    {
                        var taskReference = _context.TaskReferenceModel.Where(p => p.TaskId == TaskId && p.Type == ConstTaskReference.Contact).FirstOrDefault();
                        if (taskReference != null)
                        {
                            _context.Entry(taskReference).State = EntityState.Deleted;
                        }

                        var taskContact = _context.TaskContactModel.Where(p => p.TaskId == TaskId && p.isMain == true).FirstOrDefault();
                        if (taskContact != null)
                        {
                            _context.Entry(taskContact).State = EntityState.Deleted;
                        }
                    }
                    else
                    {
                        var taskReference = _context.TaskReferenceModel.Where(p => p.TaskId == TaskId && p.ObjectId == ContactId).FirstOrDefault();
                        if (taskReference == null)
                        {
                            //Tìm profile id dựa theo contact nếu user không chọn profile (khách hàng)
                            if (ProfileId == null)
                            {
                                var profileId = _unitOfWork.ProfileRepository.GetProfileByContact(ContactId);
                                ProfileId = profileId;
                            }
                            TaskContactModel contact = new TaskContactModel();
                            contact.TaskContactId = Guid.NewGuid();
                            contact.TaskId = TaskId;
                            contact.ContactId = ContactId;
                            contact.isMain = true;
                            contact.CreateBy = AccountId;
                            _unitOfWork.TaskRepository.CreateTaskContact(contact);

                            //Lưu vào bảng Task reference
                            TaskReferenceModel reference = new TaskReferenceModel();
                            reference.ObjectId = ContactId;
                            reference.Type = ConstTaskReference.Contact;
                            reference.TaskId = TaskId;
                            reference.CreateBy = AccountId;
                            _unitOfWork.TaskRepository.CreateTaskReference(reference);
                        }
                    }
                    #endregion
                    _context.Entry(task).State = EntityState.Modified;
                    _context.CurrentAccountId = AccountId;
                    _context.SaveChanges();
                }
                return _APISuccess(null);
            });
        }
        #endregion Update Profile

        #region Update Task
        public ActionResult UpdateTask(TaskViewModel taskViewModel, List<TaskAssignViewModel> taskAssignList, List<TaskReporterViewModel> taskReporterList, string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                //Push Notification
                string notificationMessage = string.Empty;
                string currentAccountName = string.Empty;
                string currentEmployeeCode = string.Empty;
                string taskCode = string.Empty;

                //convert string to datetime
                var _repoLibrary = new RepositoryLibrary();

                #region Ngày tháng
                //Ngày tiếp nhận
                if (!string.IsNullOrEmpty(taskViewModel.ReceiveDate_String))
                {
                    taskViewModel.ReceiveDate = _repoLibrary.VNStringToDateTime(taskViewModel.ReceiveDate_String);
                }
                //Ngày bắt đầu
                if (!string.IsNullOrEmpty(taskViewModel.StartDate_String))
                {
                    taskViewModel.StartDate = _repoLibrary.VNStringToDateTime(taskViewModel.StartDate_String);
                }
                if (!string.IsNullOrEmpty(taskViewModel.StartTime_String))
                {
                    taskViewModel.StartTime = _repoLibrary.VNStringToTimeSpan(taskViewModel.StartTime_String);
                    taskViewModel.StartDate = taskViewModel.StartDate.Value.Add(taskViewModel.StartTime.Value);
                }
                //Ngày đến hạn
                if (!string.IsNullOrEmpty(taskViewModel.EstimateEndDate_String))
                {
                    taskViewModel.EstimateEndDate = _repoLibrary.VNStringToDateTime(taskViewModel.EstimateEndDate_String);
                }
                if (!string.IsNullOrEmpty(taskViewModel.EstimateEndTime_String))
                {
                    taskViewModel.EstimateEndTime = _repoLibrary.VNStringToTimeSpan(taskViewModel.EstimateEndTime_String);
                    taskViewModel.EstimateEndDate = taskViewModel.EstimateEndDate.Value.Add(taskViewModel.EstimateEndTime.Value);
                }
                //Ngày kết thúc
                if (!string.IsNullOrEmpty(taskViewModel.EndDate_String))
                {
                    taskViewModel.EndDate = _repoLibrary.VNStringToDateTime(taskViewModel.EndDate_String);
                }
                //Ngày bắt đầu nhắc nhở
                if (!string.IsNullOrEmpty(taskViewModel.RemindStartDate_String))
                {
                    taskViewModel.RemindStartDate = _repoLibrary.VNStringToDateTime(taskViewModel.RemindStartDate_String);
                }
                if (!string.IsNullOrEmpty(taskViewModel.RemindStartTime_String))
                {
                    taskViewModel.RemindStartTime = _repoLibrary.VNStringToTimeSpan(taskViewModel.RemindStartTime_String);
                    taskViewModel.RemindStartDate = taskViewModel.RemindStartDate.Value.Add(taskViewModel.RemindStartTime.Value);
                }
                //Ngày thi công
                if (!string.IsNullOrEmpty(taskViewModel.Date1_String))
                {
                    taskViewModel.Date1 = _repoLibrary.VNStringToDateTime(taskViewModel.Date1_String);
                }
                //Ngày ghé thăm
                if (!string.IsNullOrEmpty(taskViewModel.VisitDate_String))
                {
                    taskViewModel.VisitDate = _repoLibrary.VNStringToDateTime(taskViewModel.VisitDate_String);
                }
                #endregion

                #region Handle null value
                if (!string.IsNullOrEmpty(taskViewModel.Description) && taskViewModel.Description == "null")
                {
                    taskViewModel.Description = null;
                }
                if (!string.IsNullOrEmpty(taskViewModel.CustomerReviews) && taskViewModel.CustomerReviews == "null")
                {
                    taskViewModel.CustomerReviews = null;
                }
                if (!string.IsNullOrEmpty(taskViewModel.Property5) && taskViewModel.Property5 == "null")
                {
                    taskViewModel.Property5 = null;
                }
                if (!string.IsNullOrEmpty(taskViewModel.CustomerSatisfactionReviews) && taskViewModel.CustomerSatisfactionReviews == "null")
                {
                    taskViewModel.CustomerSatisfactionReviews = null;
                }
                if (!string.IsNullOrEmpty(taskViewModel.VisitAddress) && (taskViewModel.VisitAddress == "undefined" || taskViewModel.VisitAddress == "null"))
                {
                    taskViewModel.VisitAddress = null;
                }
                if (!string.IsNullOrEmpty(taskViewModel.lat) && taskViewModel.lat == "undefined")
                {
                    taskViewModel.lat = null;
                }
                if (!string.IsNullOrEmpty(taskViewModel.lng) && taskViewModel.lng == "undefined")
                {
                    taskViewModel.lng = null;
                }
                #endregion Handle null value
                //Nguồn tiếp nhận
                taskViewModel.Property4 = taskViewModel.TaskSourceCode;

                var task = _context.TaskModel.FirstOrDefault(p => p.TaskId == taskViewModel.TaskId);
                if (task != null)
                {
                    taskCode = taskViewModel.WorkFlowCode + "." + task.TaskCode;
                    currentEmployeeCode = _unitOfWork.SalesEmployeeRepository.GetSaleEmployeeCodeBy(taskViewModel.CreateBy);
                    currentAccountName = _unitOfWork.SalesEmployeeRepository.GetSaleEmployeeNameBy(currentEmployeeCode);

                    #region Assignee
                    _unitOfWork.TaskRepository.SaveTaskAssignee(taskAssignList, task.TaskId, taskViewModel.CreateBy);
                    #endregion

                    #region Reporter
                    _unitOfWork.TaskRepository.SaveTaskReporter(taskReporterList, task.TaskId, taskViewModel.CreateBy);
                    #endregion

                    #region Update task
                    //2020-11-10: Nếu là thăm hỏi KH thì lấy ưu tiên ngày TH thực tế, nếu không có thì lấy qua ngày dự kiến
                    DateTime? startDate = taskViewModel.StartDate;
                    if (taskViewModel.Type == ConstWorkFlowCategory.THKH && taskViewModel.EndDate.HasValue)
                    {
                        startDate = taskViewModel.EndDate;
                    }
                    var summary = _unitOfWork.TaskRepository.GetSummary(taskViewModel.WorkFlowId, taskViewModel.VisitTypeCode, taskViewModel.ReceiveDate, startDate, taskViewModel.ProfileId, taskAssignList);
                    if (!string.IsNullOrEmpty(summary))
                    {
                        taskViewModel.Summary = summary;
                    }
                    task.Summary = taskViewModel.Summary;
                    task.Reporter = taskViewModel.Reporter;
                    task.TaskStatusId = taskViewModel.TaskStatusId;
                    task.CommonMistakeCode = taskViewModel.CommonMistakeCode;
                    task.Description = taskViewModel.Description;

                    if (string.IsNullOrEmpty(taskViewModel.PriorityCode) && string.IsNullOrEmpty(task.PriorityCode))
                    {
                        var listPriority = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.Priority);
                        if (listPriority != null && listPriority.Count > 0)
                        {
                            taskViewModel.PriorityCode = listPriority[0].CatalogCode;
                        }
                    }

                    if (string.IsNullOrEmpty(taskViewModel.PriorityCode) && !string.IsNullOrEmpty(task.PriorityCode))
                    {
                        taskViewModel.PriorityCode = task.PriorityCode;
                    }

                    task.PriorityCode = taskViewModel.PriorityCode;
                    task.ReceiveDate = taskViewModel.ReceiveDate;
                    task.StartDate = taskViewModel.StartDate;
                    task.EstimateEndDate = taskViewModel.EstimateEndDate;
                    //Get task process code
                    var processCode = _unitOfWork.TaskStatusRepository.GetProcessCodeByTaskStatus(task.WorkFlowId, taskViewModel.TaskStatusId);
                    if (taskViewModel.EndDate != task.EndDate && taskViewModel.EndDate != null && processCode != ConstProcess.completed)
                    {
                        return _APIError("Vui lòng chuyển trạng thái sang \"Hoàn thành\" trước khi cập nhật \"Ngày kết thúc\"!");
                    }
                    task.EndDate = taskViewModel.EndDate;
                    //Tự động cập nhật ngày kết thúc khi chuyển trạng thái
                    if (task.EndDate == null)
                    {
                        var AutoEndDate = _unitOfWork.TaskStatusRepository.CheckAutoEndDateByTaskStatus(task.WorkFlowId, taskViewModel.TaskStatusId);

                        if (AutoEndDate == true)
                        {
                            task.EndDate = DateTime.Now;
                        }
                    }
                    //Riêng tư
                    if (taskViewModel.isPrivate.HasValue)
                    {
                        task.isPrivate = taskViewModel.isPrivate;
                    }

                    //Nhắc nhở
                    task.isRemind = taskViewModel.isRemind;
                    if (task.isRemind == true)
                    {
                        task.RemindTime = taskViewModel.RemindTime;
                        task.RemindCycle = taskViewModel.RemindCycle;
                        task.isRemindForReporter = taskViewModel.isRemindForReporter;
                        task.isRemindForAssignee = taskViewModel.isRemindForAssignee;
                        task.RemindStartDate = taskViewModel.RemindStartDate;
                    }
                    //Update remind task
                    _unitOfWork.TaskRepository.UpdateRemindTask(taskViewModel, taskViewModel.CreateBy);

                    task.LastEditBy = taskViewModel.CreateBy;
                    task.LastEditTime = DateTime.Now;

                    //task.WorkFlowId = task.WorkFlowId;
                    if (taskViewModel.StoreId != Guid.Empty)
                    {
                        task.StoreId = taskViewModel.StoreId;
                    }
                    task.ProfileId = taskViewModel.ProfileId;
                    task.ConstructionUnit = taskViewModel.ConstructionUnit;
                    task.ConstructionUnitContact = taskViewModel.ConstructionUnitContact;
                    if (!string.IsNullOrEmpty(taskViewModel.ServiceTechnicalTeamCode) && taskViewModel.ServiceTechnicalTeamCode != "null" && taskViewModel.ServiceTechnicalTeamCode != "undefined")
                    {
                        task.ServiceTechnicalTeamCode = taskViewModel.ServiceTechnicalTeamCode;
                    }
                    task.CustomerReviews = taskViewModel.CustomerReviews;
                    task.ErrorTypeCode = taskViewModel.ErrorTypeCode;
                    task.ErrorCode = taskViewModel.ErrorCode;

                    #region VisitAddress
                    //Required VisitAddress
                    var IsHasVisitAddress = _context.WorkFlowConfigModel
                                                .Where(p => p.WorkFlowId == taskViewModel.WorkFlowId
                                                    && p.FieldCode == "VisitAddress"
                                                    && p.IsRequired == true).FirstOrDefault();
                    if (IsHasVisitAddress != null && string.IsNullOrEmpty(taskViewModel.VisitAddress))
                    {
                        return _APIError(string.Format(LanguageResource.Required, IsHasVisitAddress.Note ?? LanguageResource.Task_VisitAddress));
                    }
                    task.VisitAddress = taskViewModel.VisitAddress;
                    //Get lat lng of the VisitAddress
                    if (!string.IsNullOrEmpty(taskViewModel.VisitAddress))
                    {
                        //Nếu chưa lấy được lat lng ở client => Lấy lat lng theo VisitAddress bằng GoogleLocationService
                        if (string.IsNullOrEmpty(taskViewModel.lat) || string.IsNullOrEmpty(taskViewModel.lng))
                        {
                            try
                            {
                                var locationService = new GoogleLocationService(GoogleMapAPIKey);
                                var point = locationService.GetLatLongFromAddress(taskViewModel.VisitAddress);
                                task.lat = point.Latitude.ToString();
                                task.lng = point.Longitude.ToString();
                            }
                            catch (Exception)
                            {

                            }
                        }
                        else
                        {
                            task.lat = taskViewModel.lat;
                            task.lng = taskViewModel.lng;
                        }
                    }
                    #endregion

                    task.isRequiredCheckin = taskViewModel.isRequiredCheckin;
                    task.VisitTypeCode = taskViewModel.VisitTypeCode;
                    //Bảo hành
                    task.Property1 = taskViewModel.Property1;
                    task.Property2 = taskViewModel.Property2;
                    task.Property3 = taskViewModel.Property3;
                    task.Property4 = taskViewModel.Property4;
                    task.Property5 = taskViewModel.Property5;
                    //Ngày
                    task.Date1 = taskViewModel.Date1;
                    task.Date2 = taskViewModel.Date2;
                    task.Date3 = taskViewModel.Date3;
                    task.Date4 = taskViewModel.Date4;
                    task.Date5 = taskViewModel.Date5;
                    task.ParentTaskId = taskViewModel.ParentTaskId;
                    //NV kinh doanh
                    task.SalesSupervisorCode = taskViewModel.SalesSupervisorCode;
                    //Giá trị ĐTB
                    task.Property6 = taskViewModel.Property6;

                    //Hài lòng khách hàng
                    if (!string.IsNullOrEmpty(taskViewModel.CustomerSatisfactionReviews) || !string.IsNullOrEmpty(taskViewModel.CustomerSatisfactionCode))
                    {
                        var customerSatisfaction = _context.RatingModel.Where(p => p.ReferenceId == task.TaskId && p.RatingTypeCode == ConstCatalogType.CustomerSatisfaction).FirstOrDefault();
                        if (customerSatisfaction != null)
                        {
                            customerSatisfaction.Reviews = taskViewModel.CustomerSatisfactionReviews;
                            customerSatisfaction.Ratings = taskViewModel.CustomerSatisfactionCode;
                            _context.Entry(customerSatisfaction).State = EntityState.Modified;
                        }
                        else
                        {
                            RatingModel newCustomerSatisfaction = new RatingModel();
                            newCustomerSatisfaction.RatingId = Guid.NewGuid();
                            newCustomerSatisfaction.RatingTypeCode = ConstCatalogType.CustomerSatisfaction;
                            newCustomerSatisfaction.ReferenceId = task.TaskId;
                            newCustomerSatisfaction.Ratings = taskViewModel.CustomerSatisfactionCode;
                            newCustomerSatisfaction.Reviews = taskViewModel.CustomerSatisfactionReviews;
                            _context.Entry(newCustomerSatisfaction).State = EntityState.Added;
                        }
                    }

                    //Ý kiến khách hàng + đánh giá
                    if (!string.IsNullOrEmpty(taskViewModel.CustomerRatings))
                    {
                        task.Property5 = taskViewModel.CustomerRatings;
                        //Nếu dữ liệu cũ có rating thì xóa đi thêm lại
                        var existRatingProduct = _context.RatingModel.Where(p => p.ReferenceId == task.TaskId && p.RatingTypeCode == ConstCatalogType.Ticket_CustomerReviews_Product).FirstOrDefault();
                        if (existRatingProduct != null)
                        {
                            _context.Entry(existRatingProduct).State = EntityState.Deleted;
                        }
                        var existRatingService = _context.RatingModel.Where(p => p.ReferenceId == task.TaskId && p.RatingTypeCode == ConstCatalogType.Ticket_CustomerReviews_Service).FirstOrDefault();
                        if (existRatingService != null)
                        {
                            _context.Entry(existRatingService).State = EntityState.Deleted;
                        }
                        var existsCustomerReviews = _context.RatingModel.Where(p => p.ReferenceId == task.TaskId && p.RatingTypeCode == null && p.Reviews != null).FirstOrDefault();
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
                                ratingProduct.ReferenceId = task.TaskId;
                                ratingProduct.Ratings = taskViewModel.Ticket_CustomerReviews_Product;
                                _context.Entry(ratingProduct).State = EntityState.Added;
                            }

                            //2.2. Về dịch vụ
                            if (!string.IsNullOrEmpty(taskViewModel.Ticket_CustomerReviews_Service))
                            {
                                RatingModel ratingService = new RatingModel();
                                ratingService.RatingId = Guid.NewGuid();
                                ratingService.RatingTypeCode = ConstCatalogType.Ticket_CustomerReviews_Service;
                                ratingService.ReferenceId = task.TaskId;
                                ratingService.Ratings = taskViewModel.Ticket_CustomerReviews_Service;
                                _context.Entry(ratingService).State = EntityState.Added;
                            }

                            //2.3. Ý kiến khác => nhập nội dung => lưu thông tin vào RatingModel (chỉ lưu reviews)
                            if (!string.IsNullOrEmpty(taskViewModel.Property5))
                            {
                                RatingModel customerReviews = new RatingModel();
                                customerReviews.RatingId = Guid.NewGuid();
                                customerReviews.ReferenceId = task.TaskId;
                                customerReviews.Reviews = taskViewModel.Property5;
                                _context.Entry(customerReviews).State = EntityState.Added;
                            }
                        }
                        //3. Khác => nhập nội dung => lưu thông tin vào RatingModel (chỉ lưu reviews)
                        //else if (taskViewModel.CustomerRatings == "other")
                        //{
                        //    if (!string.IsNullOrEmpty(taskViewModel.Property5))
                        //    {
                        //        RatingModel customerReviews = new RatingModel();
                        //        customerReviews.RatingId = Guid.NewGuid();
                        //        customerReviews.ReferenceId = task.TaskId;
                        //        customerReviews.Reviews = taskViewModel.Property5;
                        //        _context.Entry(customerReviews).State = EntityState.Added;
                        //    }
                        //}
                    }

                    #region Update task contact
                    var existContact = (from p in _context.TaskContactModel
                                        where p.TaskId == task.TaskId
                                        select p).FirstOrDefault();
                    if (taskViewModel.ContactId != null)
                    {
                        if (existContact != null)
                        {
                            existContact.ContactId = taskViewModel.ContactId;
                        }
                        else
                        {
                            TaskContactModel contact = new TaskContactModel();
                            contact.TaskContactId = Guid.NewGuid();
                            contact.TaskId = task.TaskId;
                            contact.ContactId = taskViewModel.ContactId;
                            contact.isMain = true;
                            contact.CreateBy = taskViewModel.CreateBy;
                            contact.CreateTime = DateTime.Now;
                            _context.Entry(contact).State = EntityState.Added;
                        }
                    }
                    else if (taskViewModel.ContactId == null && existContact != null)
                    {
                        _context.Entry(existContact).State = EntityState.Deleted;
                    }
                    #endregion

                    #region Appointment
                    var appointment = _context.AppointmentModel.FirstOrDefault(p => p.AppointmentId == task.TaskId);
                    if (appointment != null)
                    {
                        appointment.PrimaryContactId = taskViewModel.ContactId;
                        appointment.ShowroomCode = taskViewModel.ShowroomCode;
                        appointment.SaleEmployeeCode = taskViewModel.SaleEmployeeCode;
                        appointment.CategoryCode = taskViewModel.CategoryCode;
                        appointment.CustomerClassCode = taskViewModel.CustomerClassCode;
                        appointment.VisitDate = taskViewModel.VisitDate;
                        appointment.ChannelCode = taskViewModel.ChannelCode;
                        appointment.Requirement = taskViewModel.Requirement;
                    }
                    #endregion

                    #endregion

                    _context.CurrentAccountId = taskViewModel.CreateBy;
                    _context.SaveChanges();
                    //Kiểm tra Có phải trạng thái cuối cùng không (Hoàn thành)
                    //Nếu là trạng thái cuối cùng
                    //Nhóm - làm chung (task.IsTogether == true): Chỉ cần 1 người cập nhật trạng thái
                    //Nhóm - làm riêng (task.IsTogether == false): Tất cả subtask cập nhật hoàn thành => task chính hoàn thành
                    if (task.ParentTaskId != null && task.ParentTaskId != Guid.Empty)
                    {
                        try
                        {
                            //Lấy TaskStatus của trạng thái cuối cùng theo WorkFlowId
                            var StatusCode = _unitOfWork.TaskStatusRepository.GetBy(task.TaskStatusId).ProcessCode;
                            //Nếu trạng thái cuối cùng == trạng thái của task (Giao việc)
                            if (StatusCode != null && (StatusCode == ConstProcess.completed))
                            {
                                //Kiểm tra các Task khác (Có cùng Task Cha) 
                                //Nếu tất cả đều đã cập nhật trạng thái cuối => Cập nhật task cha
                                var taskParentId = task.ParentTaskId;
                                if (taskParentId != null && taskParentId != Guid.Empty)
                                {
                                    //Lấy danh sách subTask
                                    var sTaskList = _unitOfWork.TaskRepository.GetSubtaskList(taskParentId.Value);
                                    if (sTaskList != null && sTaskList.Count() > 0)
                                    {
                                        //Lấy danh sách subtask có cùng trạng thái cuối cùng
                                        var sTaskCheckStatusList = sTaskList.Where(x => x.TaskStatusId == task.TaskStatusId).ToList();
                                        //Kiểm tra 2 danh sách subtask, nếu bằng nhau => tất cả task đã update trạng thái 
                                        //Cập nhật task cha
                                        if (sTaskCheckStatusList != null && sTaskList.Count() == sTaskCheckStatusList.Count())
                                        {
                                            var parent = _context.TaskModel.Find(taskParentId);
                                            var parentProcess = _unitOfWork.TaskStatusRepository.GetTaskStatusByWorkFlow(parent.WorkFlowId).Where(x => x.ProcessCode == StatusCode).FirstOrDefault();
                                            if (parentProcess != null)
                                            {
                                                parent.TaskStatusId = parentProcess.TaskStatusId.Value;
                                                parent.LastEditBy = CurrentUser.AccountId;
                                                parent.LastEditTime = DateTime.Now;
                                                _context.SaveChanges();
                                            }

                                        }
                                        //Nếu sTaskList > sTaskCheckStatusList => còn task chưa cập nhật => bỏ qua
                                    }
                                }

                            }
                        }
                        catch (Exception)
                        {
                            return _APIError(null, "Đã có lỗi khi cập nhật trạng thái, vui lòng thử lại!");
                        }

                    }
                    #region Push notification
                    taskCode = taskViewModel.WorkFlowCode + "." + (!string.IsNullOrEmpty(task.SubtaskCode) ? task.SubtaskCode : task.TaskCode.ToString());
                    notificationMessage = string.Format("{0} vừa được cập nhật bởi {1}", taskCode, currentAccountName);

                    PushNotification(taskViewModel.CreateBy.Value, taskViewModel.TaskId, notificationMessage, task);
                    #endregion Push notification
                }
                return _APISuccess(null, "Cập nhật công việc thành công!");
            });
        }
        #endregion Update Task

        private void PushNotification(Guid AccountId, Guid TaskId, string notificationMessage, TaskModel task, bool? isSchedule = null, string scheduleNotificationMessage = null)
        {
            //get device list except current user
            string[] deviceLst = new string[] { };
            string taskCode = string.Empty;
            List<Guid> accountLst = new List<Guid>();
            var deviceIdList = new List<string>();
            var workFlow = _context.WorkFlowModel.Where(p => p.WorkFlowId == task.WorkFlowId).FirstOrDefault();
            if (workFlow != null)
            {
                taskCode = workFlow.WorkFlowCode + "." + task.TaskCode;
            }

            //var accountDeviceLst = _context.Account_Device_Mapping.Where(p => p.AccountId != AccountId).ToList();
            var accountDeviceLst = _context.Account_Device_Mapping.ToList();

            //assignee
            var assigneeList = _context.TaskAssignModel.Where(p => p.TaskId == task.TaskId).ToList();
            if (assigneeList != null && assigneeList.Count > 0)
            {
                //Nếu phân công cho nhóm thì gửi thông báo đến tất cả nhân viên thuộc nhóm đó
                if (task.IsAssignGroup == true)
                {
                    var rolesCodeLst = assigneeList.Select(p => p.RolesCode).ToList();
                    var accountList = (from a in _context.AccountModel
                                       from r in a.RolesModel
                                       where rolesCodeLst.Contains(r.RolesCode)
                                       select a.AccountId
                                      ).ToList();
                    if (accountList != null && accountList.Count > 0)
                    {
                        var deviceAccountLst = accountDeviceLst.Where(p => accountList.Contains(p.AccountId)).Select(p => p.DeviceId).Distinct().ToList();
                        if (deviceAccountLst != null && deviceAccountLst.Count > 0)
                        {
                            deviceIdList.AddRange(deviceAccountLst);
                        }

                        accountLst.AddRange(accountList);
                    }
                }
                else
                {
                    foreach (var taskAssign in assigneeList)
                    {
                        var account = _context.AccountModel.Where(p => p.EmployeeCode == taskAssign.SalesEmployeeCode).FirstOrDefault();
                        if (account != null)
                        {
                            var deviceAccountLst = accountDeviceLst.Where(p => p.AccountId == account.AccountId).Select(p => p.DeviceId).ToList();
                            if (deviceAccountLst != null && deviceAccountLst.Count > 0)
                            {
                                deviceIdList.AddRange(deviceAccountLst);
                                accountLst.Add(account.AccountId);
                            }
                        }
                    }
                }
            }

            //reporter
            var accountReporter = _context.AccountModel.Where(p => p.EmployeeCode == task.Reporter && p.AccountId != AccountId).FirstOrDefault();
            if (accountReporter != null)
            {
                var deviceAccountLst = accountDeviceLst.Where(p => p.AccountId == accountReporter.AccountId).Select(p => p.DeviceId).ToList();
                if (deviceAccountLst != null && deviceAccountLst.Count > 0)
                {
                    deviceIdList.AddRange(deviceAccountLst);
                    accountLst.Add(accountReporter.AccountId);
                }
            }

            //reporter list
            var taskReporterLst = _context.TaskReporterModel.Where(p => p.TaskId == TaskId).ToList();
            if (taskReporterLst != null && taskReporterLst.Count > 0)
            {
                foreach (var taskReporter in taskReporterLst)
                {
                    var account = _context.AccountModel.Where(p => p.EmployeeCode == taskReporter.SalesEmployeeCode && p.AccountId != AccountId).FirstOrDefault();
                    if (account != null)
                    {
                        var deviceAccountLst = accountDeviceLst.Where(p => p.AccountId == account.AccountId).Select(p => p.DeviceId).ToList();
                        if (deviceAccountLst != null && deviceAccountLst.Count > 0)
                        {
                            deviceIdList.AddRange(deviceAccountLst);
                            accountLst.Add(account.AccountId);
                        }
                    }
                }
            }

            //salessupervisorcode (NV kinh doanh)
            if (!string.IsNullOrEmpty(task.SalesSupervisorCode))
            {
                var accountSalesSupervisorCode = _context.AccountModel.Where(p => p.EmployeeCode == task.SalesSupervisorCode).FirstOrDefault();
                if (accountSalesSupervisorCode != null)
                {
                    var deviceAccountLst = accountDeviceLst.Where(p => p.AccountId == accountSalesSupervisorCode.AccountId).Select(p => p.DeviceId).ToList();
                    if (deviceAccountLst != null && deviceAccountLst.Count > 0)
                    {
                        deviceIdList.AddRange(deviceAccountLst);
                        accountLst.Add(accountSalesSupervisorCode.AccountId);
                    }
                }
            }

            //create by => Update: 2021-09-29: AC yêu cầu không cần gửi thông báo cho người tạo 
            //var createBy = accountDeviceLst.Where(p => p.AccountId == task.CreateBy).Select(p => p.DeviceId).ToList();
            //if (createBy != null && createBy.Count > 0)
            //{
            //    deviceIdList.AddRange(createBy);
            //    accountLst.Add(task.CreateBy.Value);
            //}

            if (deviceIdList != null && deviceIdList.Count > 0)
            {
                deviceLst = deviceIdList.Distinct().ToArray();
            }

            //push notification
            string summary = task.Summary;
            _unitOfWork.TaskRepository.PushNotification(TaskId, notificationMessage, deviceLst, summary, accountLst.Distinct().ToList());

            if (isSchedule == true && task.EstimateEndDate.HasValue)
            {
                TimeSpan time = new TimeSpan(8, 0, 0);
                //Nếu gửi thông báo task đến hạn thì gửi cho tất cả nhân viên liên quan đến task
                var scheduleAccountDeviceLst = _context.Account_Device_Mapping.Where(p => accountLst.Contains(p.AccountId)).Select(p => p.DeviceId).ToList();
                deviceLst = scheduleAccountDeviceLst.Distinct().ToArray();
                _unitOfWork.TaskRepository.PushNotification(TaskId, scheduleNotificationMessage, deviceLst, summary, accountLst.Distinct().ToList(), task.EstimateEndDate.Value.Add(time));
            }
            _context.SaveChanges();
        }
        #endregion Update Task

        #region Get Mission Count
        public ActionResult GetMissionBy(string KanbanCode, string UserNameCode, string CompanyCode, string token, string key, Guid? AccountId = null)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                //Load column base on KanbanCode
                Guid KanbanId = Guid.Empty;
                var kanban = _context.KanbanModel.Where(p => p.KanbanCode == KanbanCode).FirstOrDefault();
                if (kanban != null)
                {
                    KanbanId = kanban.KanbanId;
                }

                var searchModel = new TaskSearchViewModel();
                #region //Start Date
                searchModel.StartCommonDate = "Custom";
                #endregion

                #region //Receive Date
                searchModel.ReceiveCommonDate = "Custom";
                #endregion

                #region //Estimate End Date
                searchModel.EstimateEndCommonDate = "Custom";
                #endregion

                #region //End Date
                searchModel.EndCommonDate = "Custom";
                #endregion

                searchModel.Type = KanbanCode;
                searchModel.KanbanId = KanbanId;
                searchModel.IsMobile = true;

                //Trạng thái chưa hoàn thành
                searchModel.TaskProcessCode = ConstTaskStatus.Incomplete;
                //List<string> processCodeList = new List<string>();

                //if (KanbanCode != ConstKanbanType.MyWork && KanbanCode != ConstKanbanType.MyFollow && string.IsNullOrEmpty(searchModel.TaskProcessCode))
                //{
                //    //searchModel.TaskProcessCode = ConstTaskStatus.Incomplete;
                //}
                //else
                //{
                //    if (string.IsNullOrEmpty(searchModel.TaskProcessCode))
                //    {
                //        processCodeList.Add(ConstTaskStatus.Todo);
                //        processCodeList.Add(ConstTaskStatus.Processing);
                //        processCodeList.Add(ConstTaskStatus.Incomplete);
                //        processCodeList.Add(ConstTaskStatus.CompletedOnTime);
                //        processCodeList.Add(ConstTaskStatus.CompletedExpire);
                //        processCodeList.Add(ConstTaskStatus.Expired);
                //    }
                //}

                //Giao cho tôi => MyWork: Assignee = UserNameCode
                if (KanbanCode == ConstKanbanType.MyWork)
                {
                    searchModel.Assignee = UserNameCode;
                }
                //Đang theo dõi => MyFollow: Reporter = UserNameCode
                if (KanbanCode == ConstKanbanType.MyFollow)
                {
                    searchModel.Reporter = UserNameCode;
                }
                string DomainImageWorkFlow = ConstDomain.Domain + "/Upload/WorkFlow/";
                int filteredResultsCount = 0;

                string Type = KanbanCode;
                var WorkflowList = _unitOfWork.WorkFlowRepository.GetWorkFlowBy(Type, CompanyCode).Select(p => p.WorkFlowId).ToList();
                var taskList = _unitOfWork.TaskRepository.SearchQueryTaskProc(searchModel, out filteredResultsCount, DomainImageWorkFlow: DomainImageWorkFlow, AccountId: AccountId, workflowList: WorkflowList);

                return _APISuccess(taskList.Count);
            });
        }
        #endregion Get Mission Count

        #region Helper get master data

        #region Get WorkFlow
        public ActionResult GetWorkFlow(string Type, string CompanyCode, bool? isCreate, string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                //Type = 'TICKET' & CompanyCode = '1000' => Xử lý khiếu nại 
                //Type = 'TICKET' & CompanyCode = '2000' => Bảo hành
                if (Type == ConstWorkFlowCategory.TICKET && CompanyCode == "2000")
                {
                    Type = ConstWorkFlowCategory.TICKET_MLC;
                }
                var srcLst = _unitOfWork.WorkFlowRepository.GetWorkFlowBy(Type, CompanyCode)
                                                //.Where(p => p.WorkFlowCode != ConstWorkFlow.GT)
                                                .Select(p => new ISDSelectGuidItemWithCode()
                                                {
                                                    id = p.WorkFlowId,
                                                    name = p.WorkFlowName,
                                                    code = p.WorkFlowCode,
                                                }).ToList();
                //Nếu là thêm mới => Không cho thêm loại Ghé thăm (GT)
                if (isCreate == true)
                {
                    srcLst = srcLst.Where(p => p.code != ConstWorkFlow.GT).ToList();
                }
                return _APISuccess(srcLst);
            });
        }
        #endregion Get WorkFlow

        #region Get Priority
        public ActionResult GetPriority(string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                var srcLst = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.Priority).Select(p => new ISDSelectStringItemWithColor()
                {
                    id = p.CatalogCode,
                    name = p.CatalogText_vi,
                    color = p.CatalogCode == ConstPriotityCode.LOW ? "#008000" : (p.CatalogCode == ConstPriotityCode.NORMAL ? "#FFA500" : (p.CatalogCode == ConstPriotityCode.HIGH ? "#FF4500" : "#FF0000")),
                    iconType = "AntDesign",
                    iconName = p.CatalogCode == ConstPriotityCode.LOW ? "arrowdown" : "arrowup",
                }).ToList();
                return _APISuccess(srcLst);
            });
        }
        #endregion Get Priority

        #region Get service technical team 
        public ActionResult GetServiceTechnicalTeam(string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                //ServiceTechnicalTeamCode
                var srcLst = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.ServiceTechnicalTeam)
                                                        .Select(p => new ISDSelectStringItem()
                                                        {
                                                            id = p.CatalogCode,
                                                            name = p.CatalogText_vi,
                                                        });
                return _APISuccess(srcLst);
            });
        }
        #endregion Get service technical team 

        #region Get store
        public ActionResult GetStore(string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                var srcLst = _unitOfWork.StoreRepository.GetAllStore()
                                                        .Select(p => new ISDSelectGuidItem()
                                                        {
                                                            id = p.StoreId,
                                                            name = p.StoreName,
                                                        });
                return _APISuccess(srcLst);
            });
        }
        #endregion Get store

        #region Get task status
        public ActionResult GetTaskStatus(Guid WorkFlowId, string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                var srcLst = _unitOfWork.TaskStatusRepository.GetTaskStatusByWorkFlow(WorkFlowId)
                                                            .Select(p => new ISDSelectGuidItemWithColor()
                                                            {
                                                                id = p.TaskStatusId.Value,
                                                                name = p.TaskStatusName,
                                                                color = p.ProcessCode == ConstProcess.todo ? "#000" : "#fff",
                                                                bgColor = p.ProcessCode == ConstProcess.todo ? "#fff" : (p.ProcessCode == ConstProcess.processing ? "#0052CC" : "#398439"),
                                                                iconType = "MaterialCommunityIcons",
                                                                iconName = "progress-check"
                                                            }).ToList();
                return _APISuccess(srcLst);
            });
        }
        #endregion Get task status

        #region Get assignee
        public ActionResult GetAssignee(string RolesCode, string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                if (!string.IsNullOrEmpty(RolesCode))
                {
                    Guid GroupId = Guid.Empty;
                    bool isValid = Guid.TryParse(RolesCode, out GroupId);
                    List<SalesEmployeeViewModel> taskAssignList = new List<SalesEmployeeViewModel>();
                    if (isValid == true)
                    {
                        taskAssignList = (from p in _context.TaskGroupDetailModel
                                          join acc in _context.AccountModel on p.AccountId equals acc.AccountId
                                          join se in _context.SalesEmployeeModel.Include(s => s.DepartmentModel) on acc.EmployeeCode equals se.SalesEmployeeCode
                                          where p.GroupId == GroupId
                                          orderby acc.EmployeeCode
                                          select new SalesEmployeeViewModel()
                                          {
                                              SalesEmployeeCode = acc.EmployeeCode,
                                              SalesEmployeeName = se.SalesEmployeeName,
                                              DepartmentName = se.DepartmentModel.DepartmentName,
                                          }).ToList();
                    }
                    else
                    {
                        taskAssignList = (from p in _context.RolesModel
                                          from r in p.AccountModel
                                          join acc in _context.AccountModel on r.AccountId equals acc.AccountId
                                          join se in _context.SalesEmployeeModel.Include(s => s.DepartmentModel) on acc.EmployeeCode equals se.SalesEmployeeCode
                                          orderby acc.EmployeeCode
                                          where p.RolesCode == RolesCode
                                          select new SalesEmployeeViewModel()
                                          {
                                              SalesEmployeeCode = acc.EmployeeCode,
                                              SalesEmployeeName = se.SalesEmployeeName,
                                              DepartmentName = se.DepartmentModel.DepartmentName,
                                          }).ToList();
                    }
                    var result = taskAssignList.Select(p => new ISDSelectStringItemWithIcon()
                    {
                        id = p.SalesEmployeeCode,
                        name = p.SalesEmployeeCode + " | " + p.SalesEmployeeName + " | " + p.DepartmentName,
                        icon = p.SalesEmployeeName.GetCharacterForLogoName(),
                        description = p.DepartmentName,
                        additional = RolesCode,
                    }).ToList();

                    return _APISuccess(result.OrderBy(p => p.id));
                }
                else
                {
                    var srcLst = _unitOfWork.SalesEmployeeRepository.GetAllForDropdownlist()
                                                               .Select(p => new ISDSelectStringItemWithIcon()
                                                               {
                                                                   id = p.SalesEmployeeCode,
                                                                   name = p.SalesEmployeeName + " | " + p.RolesName,
                                                                   icon = p.SalesEmployeeName.GetCharacterForLogoName(),
                                                                   description = p.RolesName,
                                                                   additional = RolesCode
                                                               }).ToList();
                    //srcLst.AddRange(srcLst);
                    return _APISuccess(srcLst.OrderBy(p => p.id));
                }
            });
        }
        #endregion Get assignee

        #region Get task assign type
        public ActionResult GetTaskAssignType(Guid? WorkFlowId, string token, string key)
        {
            string taskAssignType = ConstCatalogType.TaskAssignType;
            if (WorkFlowId.HasValue)
            {
                var workFlowConfig = _context.WorkFlowConfigModel.Where(p => p.WorkFlowId == WorkFlowId && p.FieldCode == "RoleName").FirstOrDefault();
                if (workFlowConfig != null)
                {
                    taskAssignType = workFlowConfig.Parameters;
                }
            }
            var srcLst = _unitOfWork.CatalogRepository.GetBy(taskAssignType)
                                                        .Select(p => new ISDSelectStringItem
                                                        {
                                                            id = p.CatalogCode,
                                                            name = p.CatalogText_vi,
                                                        }).ToList();
            return _APISuccess(srcLst);
        }
        #endregion Get task assign type

        #region Get task reporter type
        public ActionResult GetTaskReporterType(string token, string key)
        {
            var srcLst = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.TaskReporterType)
                                                        .Select(p => new ISDSelectStringItem
                                                        {
                                                            id = p.CatalogCode,
                                                            name = p.CatalogText_vi,
                                                        }).ToList();
            return _APISuccess(srcLst);
        }
        #endregion Get task reporter type

        #region Get task process
        public ActionResult GetTaskProcess(string token, string key)
        {
            //Status 
            var statusLst = _unitOfWork.TaskStatusRepository.GetTaskStatusList();
            var srcLst = statusLst.Select(p => new ISDSelectStringItem
            {
                id = p.StatusCode,
                name = p.StatusName,
            }).ToList();
            return _APISuccess(srcLst);
        }
        #endregion Get task process

        #region Get task status for search
        public ActionResult GetTaskStatusForSearch(string Type, string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                //TaskStatusCode
                var srcLst = new List<ISDSelectStringItem>();
                var result = (from wf in _context.WorkFlowModel
                              join ts in _context.TaskStatusModel on wf.WorkFlowId equals ts.WorkFlowId
                              where wf.WorkflowCategoryCode == Type
                              select new
                              {
                                  ts.OrderIndex,
                                  ts.TaskStatusCode,
                                  ts.TaskStatusName
                              }).Distinct().OrderBy(p => p.OrderIndex).ToList();
                if (result != null && result.Count > 0)
                {
                    srcLst = result.Select(p => new ISDSelectStringItem()
                    {
                        id = p.TaskStatusCode,
                        name = p.TaskStatusName,
                    }).ToList();
                }


                return _APISuccess(srcLst);
            });
        }
        #endregion Get task status for search

        #region Get reporter
        public ActionResult GetReporter(string Type, string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                //var srcLst = _unitOfWork.SalesEmployeeRepository.GetAllForDropdownlist()
                //                                             .Select(p => new ISDSelectStringItemWithIcon()
                //                                             {
                //                                                 id = p.SalesEmployeeCode,
                //                                                 name = p.SalesEmployeeName,
                //                                                 icon = p.SalesEmployeeName.GetCharacterForLogoName(),
                //                                                 description = p.RolesName
                //                                             }).Where(p => p.id.Contains("1999")).ToList();
                //var srcLst2 = _unitOfWork.SalesEmployeeRepository.GetAllForDropdownlist()
                //                                            .Select(p => new ISDSelectStringItemWithIcon()
                //                                            {
                //                                                id = p.SalesEmployeeCode,
                //                                                name = p.SalesEmployeeName,
                //                                                icon = p.SalesEmployeeName.GetCharacterForLogoName(),
                //                                                description = p.RolesName,
                //                                            }).Where(p => !p.id.Contains("1999")).ToList();
                if (Type.Contains(ConstWorkFlowCategory.BOOKING_VISIT) || Type.Contains(ConstWorkFlowCategory.SUBTASK_BOOKINGVISIT))
                {
                    var srcLst = _unitOfWork.SalesEmployeeRepository.GetSalesEmployeeByRoles(ConstRoleCode.BAOVE)
                                                             .Select(p => new ISDSelectStringItemWithIcon()
                                                             {
                                                                 id = p.SalesEmployeeCode,
                                                                 name = p.SalesEmployeeName,
                                                                 icon = p.SalesEmployeeName.GetCharacterForLogoName(),
                                                                 description = p.RolesName
                                                             }).ToList();

                    return _APISuccess(srcLst.OrderBy(p => p.id));
                }
                else
                {
                    var srcLst = _unitOfWork.SalesEmployeeRepository.GetAllForDropdownlist()
                                                            .Select(p => new ISDSelectStringItemWithIcon()
                                                            {
                                                                id = p.SalesEmployeeCode,
                                                                name = p.SalesEmployeeName,
                                                                icon = p.SalesEmployeeName.GetCharacterForLogoName(),
                                                                description = p.RolesName
                                                            }).ToList();

                    return _APISuccess(srcLst.OrderBy(p => p.id));
                }
            });
        }
        #endregion Get reporter

        #region Get common date
        public ActionResult GetCommonDate(string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                var srcLst = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.CommonDate2)
                                                            .Select(p => new ISDSelectStringItemWithIcon()
                                                            {
                                                                id = p.CatalogCode,
                                                                name = p.CatalogText_vi,
                                                            }).ToList();
                return _APISuccess(srcLst);
            });
        }

        public ActionResult GetDateBy(string CommonDate, string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                DateTime? fromDate;
                DateTime? toDate;

                CommonDateRepository _commonDateRepository = new CommonDateRepository(_context);
                _commonDateRepository.GetDateBy(CommonDate, out fromDate, out toDate);

                return _APISuccess(new
                {
                    FromDate = string.Format("{0:dd/MM/yyyy}", fromDate),
                    ToDate = string.Format("{0:dd/MM/yyyy}", toDate)
                });
            });
        }
        #endregion Get common date

        #region Get WorkFlow Config Field
        public ActionResult GetTaskConfigField(string Type, Guid WorkFlowId, string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                List<MobileWorkFlowConfigViewModel> workFlowConfigList = new List<MobileWorkFlowConfigViewModel>();
                //workFlowConfigList = (from config in _context.WorkFlowConfigModel
                //                      join field in _context.WorkFlowFieldModel on config.FieldCode equals field.FieldCode
                //                      where config.WorkFlowId == WorkFlowId
                //                      select new MobileWorkFlowConfigViewModel()
                //                      {
                //                          FieldCode = config.FieldCode,
                //                          FieldName = string.IsNullOrEmpty(config.Note) ? field.FieldName : config.Note,
                //                          OrderIndex = field.OrderIndex,
                //                          IsRequired = config.IsRequired,
                //                      }).OrderBy(p => p.OrderIndex).ToList();
                workFlowConfigList = _context.WorkFlowConfigModel.Where(p => p.WorkFlowId == WorkFlowId)
                                                          .Select(p => new MobileWorkFlowConfigViewModel()
                                                          {
                                                              FieldCode = p.FieldCode,
                                                              FieldName = p.Note,
                                                              IsRequired = p.IsRequired,
                                                              HideWhenAdd = p.HideWhenAdd,
                                                              AddDefaultValue = p.AddDefaultValue,
                                                              HideWhenEdit = p.HideWhenEdit,
                                                              EditDefaultValue = p.EditDefaultValue,
                                                          }).ToList();
                foreach (var item in workFlowConfigList)
                {
                    if (string.IsNullOrEmpty(item.FieldName))
                    {
                        item.FieldName = PropertyHelper.GetDisplayNameByString<TaskViewModel>(item.FieldCode);
                    }
                }

                return _APISuccess(workFlowConfigList);
            });
        }

        public ActionResult GetTaskConfigFieldBy(string WorkFlowCode, string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                List<MobileDynamicWorkFlowConfigViewModel> workFlowConfigList = new List<MobileDynamicWorkFlowConfigViewModel>();
                var WorkFlowId = _context.WorkFlowModel.Where(p => p.WorkFlowCode == WorkFlowCode).Select(p => p.WorkFlowId).FirstOrDefault();
                if (WorkFlowId != Guid.Empty)
                {
                    //workFlowConfigList = _context.WorkFlowConfigModel.Where(p => p.WorkFlowId == WorkFlowId)
                    //                                     .Select(p => new MobileWorkFlowConfigViewModel()
                    //                                     {
                    //                                         FieldCode = p.FieldCode,
                    //                                         FieldName = p.Note,
                    //                                         IsRequired = p.IsRequired,
                    //                                         HideWhenAdd = p.HideWhenAdd,
                    //                                         AddDefaultValue = p.AddDefaultValue,
                    //                                         HideWhenEdit = p.HideWhenEdit,
                    //                                         EditDefaultValue = p.EditDefaultValue,
                    //                                     }).ToList();
                    //foreach (var item in workFlowConfigList)
                    //{
                    //    if (string.IsNullOrEmpty(item.FieldName))
                    //    {
                    //        item.FieldName = PropertyHelper.GetDisplayNameByString<TaskViewModel>(item.FieldCode);
                    //    }
                    //}
                    workFlowConfigList.Add(new MobileDynamicWorkFlowConfigViewModel
                    {
                        typeField = "TextInput",
                        textField = "Tiêu đề",
                        placeHolder = "--Nhập tiêu đề--",
                        stateName = "Summary",
                        stateValue = string.Empty,
                    });
                    workFlowConfigList.Add(new MobileDynamicWorkFlowConfigViewModel
                    {
                        typeField = "TextArea",
                        textField = "Mô tả",
                        placeHolder = "--Nhập mô tả--",
                        stateName = "Description",
                        stateValue = string.Empty,
                    });
                    workFlowConfigList.Add(new MobileDynamicWorkFlowConfigViewModel
                    {
                        typeField = "DatePicker",
                        textField = "Ngày đến hạn",
                        placeHolder = "--Nhập mô tả--",
                        stateName = "EstimateEndDate",
                        stateValue = null,
                    });
                    workFlowConfigList.Add(new MobileDynamicWorkFlowConfigViewModel
                    {
                        typeField = "Switch",
                        textField = "Phân công cho",
                        stateName = "IsAssignGroup",
                        stateValue = false,
                    });
                    workFlowConfigList.Add(new MobileDynamicWorkFlowConfigViewModel
                    {
                        typeField = "DropDown",
                        textField = "NV được phân công",
                        stateName = "Assignee",
                        DropDownData = null,
                    });
                    workFlowConfigList.Add(new MobileDynamicWorkFlowConfigViewModel
                    {
                        typeField = "DropDown",
                        textField = "NV theo dõi giám sát",
                        stateName = "Reporter",
                        stateValue = null,
                        DropDownData = null,
                    });
                    if (workFlowConfigList != null && workFlowConfigList.Count > 0)
                    {
                        foreach (var item in workFlowConfigList)
                        {
                            if (item.typeField == "DropDown")
                            {
                                //Assignee
                                if (item.stateName == "Assignee")
                                {
                                    var data = _unitOfWork.SalesEmployeeRepository.GetAllForDropdownlistBy(isMobile: true)
                                                       .Select(p => new ISDSelectStringItem()
                                                       {
                                                           id = p.SalesEmployeeCode,
                                                           name = p.SalesEmployeeName,
                                                       }).ToList();
                                    item.DropDownData = data;
                                }

                                //Reporter
                                if (item.stateName == "Reporter")
                                {
                                    var data = _unitOfWork.SalesEmployeeRepository.GetAllForDropdownlistBy(isMobile: true)
                                                       .Select(p => new ISDSelectStringItem()
                                                       {
                                                           id = p.SalesEmployeeCode,
                                                           name = p.SalesEmployeeName,
                                                       }).ToList();
                                    item.DropDownData = data;
                                }
                            }
                        }
                    }
                }

                return _APISuccess(workFlowConfigList);
            });
        }
        #endregion Get WorlFlow Config Field

        #region Get visit type
        public ActionResult GetVisitType(string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                var visitTypeLst = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.VisitType)
                                               .Select(p => new ISDSelectStringItem()
                                               {
                                                   id = p.CatalogCode,
                                                   name = p.CatalogText_vi,
                                               }).ToList();

                return _APISuccess(visitTypeLst);
            });
        }
        #endregion Get visit type

        #region Get contact
        public ActionResult GetContactBy(Guid? ProfileId, string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                if (ProfileId.HasValue && ProfileId != Guid.Empty)
                {
                    var srcLst = _unitOfWork.ProfileRepository.GetContactListOfProfile(ProfileId);
                    return _APISuccess(srcLst);
                }
                else
                {
                    return _APISuccess(null);
                }
            });
        }
        #endregion Get contact

        #region Get remind cycle
        public ActionResult GetRemindCycle(string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                var remindCycleLst = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.RemindCycle).
                                        Select(p => new ISDSelectStringItem()
                                        {
                                            id = p.CatalogCode,
                                            name = p.CatalogText_vi,
                                        });
                return _APISuccess(remindCycleLst);
            });
        }
        #endregion Get remind cycle

        #region Get task source code
        public ActionResult GetTaskSourceCode(string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                var taskSourceList = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.TaskSource)
                                                                 .Select(p => new ISDRadioStringItem()
                                                                 {
                                                                     label = p.CatalogText_vi,
                                                                     value = p.CatalogCode,
                                                                 });
                return _APISuccess(taskSourceList);
            });
        }
        #endregion Get task source code

        #region Get is disabled summary
        public ActionResult GetIsDisabledSummary(Guid WorkFlowId, string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                var isDisabledSummary = _context.WorkFlowModel.Where(p => p.WorkFlowId == WorkFlowId).Select(p => p.IsDisabledSummary).FirstOrDefault();
                return _APISuccess(isDisabledSummary);
            });
        }
        #endregion Get is disable summary

        #region Get subtask type list
        public ActionResult GetSubtaskTypeList(Guid WorkFlowId, string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                //return type list of subtask based on WorkFlowId
                var subtaskTypeList = _unitOfWork.WorkFlowRepository.GetTypeByParentWorkFlow(WorkFlowId)
                                                             .Select(p => new ISDSelectStringItem()
                                                             {
                                                                 id = p.WorkFlowCategoryCode,
                                                                 name = p.WorkFlowCategoryName,
                                                             });
                return _APISuccess(subtaskTypeList);
            });
        }
        #endregion Get subtask type list

        #region Get sales supervisor code
        public ActionResult GetSalesSupervisor(string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                var srcLst = _unitOfWork.SalesEmployeeRepository.GetAllForDropdownlist()
                                                            .Select(p => new ISDSelectStringItemWithIcon()
                                                            {
                                                                id = p.SalesEmployeeCode,
                                                                name = p.SalesEmployeeName,
                                                                icon = p.SalesEmployeeName.GetCharacterForLogoName(),
                                                                description = p.RolesName,
                                                            }).Where(p => p.id.Contains("1999")).ToList();
                var srcLst2 = _unitOfWork.SalesEmployeeRepository.GetAllForDropdownlist()
                                                            .Select(p => new ISDSelectStringItemWithIcon()
                                                            {
                                                                id = p.SalesEmployeeCode,
                                                                name = p.SalesEmployeeName,
                                                                icon = p.SalesEmployeeName.GetCharacterForLogoName(),
                                                                description = p.RolesName,
                                                            }).Where(p => !p.id.Contains("1999")).ToList();
                srcLst.AddRange(srcLst2);
                return _APISuccess(srcLst.OrderBy(p => p.id));
            });
        }
        #endregion Get assignee

        #region Get department
        public ActionResult GetDepartment(string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                var rolesList = _context.RolesModel.Where(p => p.Actived == true && p.isEmployeeGroup == true)
                                                                 .Select(p => new ISDSelectStringItem()
                                                                 {
                                                                     id = p.RolesCode,
                                                                     name = p.RolesName,
                                                                 });
                return _APISuccess(rolesList);
            });
        }
        #endregion Get department

        #region Get role
        public ActionResult GetRole(Guid? AccountId, string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                //var roleList = _unitOfWork.AccountRepository.GetRolesList(isEmployeeGroup: true)
                //                                            .Select(p => new ISDSelectStringItem()
                //                                            {
                //                                                id = p.RolesCode,
                //                                                name = p.RolesName,
                //                                            });

                var roleList = _unitOfWork.AccountRepository.GetRolesList(isEmployeeGroup: true)
                                                            .Select(p => new ISDSelectStringItem()
                                                            {
                                                                id = p.RolesCode,
                                                                name = p.RolesName,
                                                            }).ToList();
                //Nếu có tạo nhóm ngoài hệ thống thì lấy thêm thông tin
                //Lấy tất cả thông tin nhóm được tạo từ CurrentUser
                var group = _context.TaskGroupModel.Where(p => p.CreatedAccountId == AccountId && p.GroupType == ConstTaskGroupType.TaskGroup)
                                                    .Select(p => new ISDSelectStringItem()
                                                    {
                                                        id = p.GroupId.ToString(),
                                                        name = p.GroupName,
                                                    }).ToList();
                if (group != null && group.Count > 0)
                {
                    int groupIndex = 0;
                    foreach (var item in group)
                    {
                        roleList.Insert(groupIndex, item);
                        groupIndex++;
                    }
                }
                return _APISuccess(roleList);
            });
        }
        #endregion Get role

        #region Get customer satisfaction
        public ActionResult GetCustomerSatisfaction(string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                var customerSatisfactionList = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.CustomerSatisfaction)
                                                                 .Select(p => new ISDRadioStringItem()
                                                                 {
                                                                     label = p.CatalogText_vi,
                                                                     value = p.CatalogCode,
                                                                 });
                return _APISuccess(customerSatisfactionList);
            });
        }
        #endregion Get customer satisfaction

        #region Get customer ratings
        public ActionResult GetCustomerRatings(string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                var customerRatings = new List<ISDSelectStringItem>();
                customerRatings.Add(new ISDSelectStringItem()
                {
                    id = "none",
                    name = "Không ý kiến"
                });
                customerRatings.Add(new ISDSelectStringItem()
                {
                    id = "rating",
                    name = "Đánh giá theo sao & ý kiến khác"
                });
                //customerRatings.Add(new ISDSelectStringItem()
                //{
                //    id = "other",
                //    name = "Khác"
                //});
                return _APISuccess(customerRatings);
            });
        }

        public ActionResult GetStarRatings(string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                //Đánh giá theo sao
                //1. Về sản phẩm
                var ticket_CustomerReviews_Product = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.Ticket_CustomerReviews_Product)
                                                                            .Select(p => new ISDSelectStringItem()
                                                                            {
                                                                                id = p.CatalogCode,
                                                                                name = p.CatalogText_vi,
                                                                            });
                //2. Về dịch vụ
                var ticket_CustomerReviews_Service = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.Ticket_CustomerReviews_Service)
                                                                              .Select(p => new ISDSelectStringItem()
                                                                              {
                                                                                  id = p.CatalogCode,
                                                                                  name = p.CatalogText_vi,
                                                                              });
                //Tài khoản bảo vệ
                var baoVe = (from acc in _context.AccountModel
                             from r in acc.RolesModel
                             where r.RolesCode == "BAOVE"
                             orderby acc.EmployeeCode
                             select acc.EmployeeCode).FirstOrDefault();
                return _APISuccess(new { productRatingsList = ticket_CustomerReviews_Product, serviceRatingsList = ticket_CustomerReviews_Service, baoVe = baoVe });
            });
        }
        #endregion Get customer ratings

        #endregion Helper get master data

        #region Create new task
        public ActionResult CreateTask(TaskViewModel taskViewModel, List<TaskAssignViewModel> taskAssignList, List<TaskReporterViewModel> taskReporterList, string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                //Push Notification
                string notificationMessage = string.Empty;
                string currentAccountName = string.Empty;
                string taskCode = string.Empty;
                bool isPushNotificationWithSchedule = false;

                taskViewModel.TaskId = Guid.NewGuid();
                //convert string to datetime
                var _repoLibrary = new RepositoryLibrary();

                #region Null value
                if (!string.IsNullOrEmpty(taskViewModel.VisitAddress) && (taskViewModel.VisitAddress == "undefined" || taskViewModel.VisitAddress == "null"))
                {
                    taskViewModel.VisitAddress = null;
                }
                if (!string.IsNullOrEmpty(taskViewModel.lat) && taskViewModel.lat == "undefined")
                {
                    taskViewModel.lat = null;
                }
                if (!string.IsNullOrEmpty(taskViewModel.lng) && taskViewModel.lng == "undefined")
                {
                    taskViewModel.lng = null;
                }
                if (!string.IsNullOrEmpty(taskViewModel.ProfileAddress) && taskViewModel.ProfileAddress == "undefined")
                {
                    taskViewModel.ProfileAddress = null;
                }
                if (!string.IsNullOrEmpty(taskViewModel.CustomerReviews) && taskViewModel.CustomerReviews == "undefined")
                {
                    taskViewModel.CustomerReviews = null;
                }
                if (!string.IsNullOrEmpty(taskViewModel.Property5) && taskViewModel.Property5 == "undefined")
                {
                    taskViewModel.Property5 = null;
                }
                #endregion Null value

                #region Ngày tháng
                //Ngày tiếp nhận
                if (!string.IsNullOrEmpty(taskViewModel.ReceiveDate_String))
                {
                    taskViewModel.ReceiveDate = _repoLibrary.VNStringToDateTime(taskViewModel.ReceiveDate_String);
                }
                //Ngày bắt đầu
                if (!string.IsNullOrEmpty(taskViewModel.StartDate_String))
                {
                    taskViewModel.StartDate = _repoLibrary.VNStringToDateTime(taskViewModel.StartDate_String);
                }
                if (!string.IsNullOrEmpty(taskViewModel.StartTime_String))
                {
                    taskViewModel.StartTime = _repoLibrary.VNStringToTimeSpan(taskViewModel.StartTime_String);
                    taskViewModel.StartDate = taskViewModel.StartDate.Value.Add(taskViewModel.StartTime.Value);
                }
                //Ngày đến hạn
                if (!string.IsNullOrEmpty(taskViewModel.EstimateEndDate_String))
                {
                    taskViewModel.EstimateEndDate = _repoLibrary.VNStringToDateTime(taskViewModel.EstimateEndDate_String);
                    //Nếu có thông tin ngày đến hạn thì gửi thông báo khi task đến hạn
                    isPushNotificationWithSchedule = true;
                }
                if (!string.IsNullOrEmpty(taskViewModel.EstimateEndTime_String))
                {
                    taskViewModel.EstimateEndTime = _repoLibrary.VNStringToTimeSpan(taskViewModel.EstimateEndTime_String);
                    taskViewModel.EstimateEndDate = taskViewModel.EstimateEndDate.Value.Add(taskViewModel.EstimateEndTime.Value);
                }
                //Ngày kết thúc
                if (!string.IsNullOrEmpty(taskViewModel.EndDate_String))
                {
                    taskViewModel.EndDate = _repoLibrary.VNStringToDateTime(taskViewModel.EndDate_String);
                }
                //Ngày bắt đầu nhắc nhở
                if (!string.IsNullOrEmpty(taskViewModel.RemindStartDate_String))
                {
                    taskViewModel.RemindStartDate = _repoLibrary.VNStringToDateTime(taskViewModel.RemindStartDate_String);
                }
                if (!string.IsNullOrEmpty(taskViewModel.RemindStartTime_String))
                {
                    taskViewModel.RemindStartTime = _repoLibrary.VNStringToTimeSpan(taskViewModel.RemindStartTime_String);
                    taskViewModel.RemindStartDate = taskViewModel.RemindStartDate.Value.Add(taskViewModel.RemindStartTime.Value);
                }
                //Ngày thi công
                if (!string.IsNullOrEmpty(taskViewModel.Date1_String))
                {
                    taskViewModel.Date1 = _repoLibrary.VNStringToDateTime(taskViewModel.Date1_String);
                }
                #endregion

                if (taskViewModel.CreateBy != null)
                {
                    var salesEmployeeCode = _context.AccountModel.Where(p => p.AccountId == taskViewModel.CreateBy).Select(p => p.EmployeeCode).FirstOrDefault();
                    if (salesEmployeeCode != null)
                    {
                        taskViewModel.SalesEmployeeCode = salesEmployeeCode;
                    }
                    //TaskStatusId
                    var taskStatus = _unitOfWork.TaskStatusRepository.GetTaskStatusByWorkFlow(taskViewModel.WorkFlowId);
                    if (taskViewModel.TaskStatusId == Guid.Empty)
                    {
                        taskViewModel.TaskStatusId = (Guid)taskStatus[0].TaskStatusId;
                    }
                    //StoreId
                    if (taskViewModel.StoreId == null || taskViewModel.StoreId == Guid.Empty)
                    {
                        var saleOrg = _unitOfWork.StoreRepository.GetStoreByCompanyPermission(taskViewModel.CreateBy, taskViewModel.CompanyCode);
                        taskViewModel.StoreId = _unitOfWork.StoreRepository.GetStoreIdBySaleOrgCode(saleOrg[0].SaleOrgCode);
                    }
                    //PriorityCode
                    if (string.IsNullOrEmpty(taskViewModel.PriorityCode))
                    {
                        var listPriority = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.Priority);
                        if (listPriority != null && listPriority.Count > 0)
                        {
                            taskViewModel.PriorityCode = listPriority.Where(p => p.CatalogCode == ConstPriotityCode.NORMAL)
                                                                     .Select(p => p.CatalogCode).FirstOrDefault();
                        }
                    }

                    #region Tiêu đề/ Summary
                    //2020-11-10: Nếu là thăm hỏi KH thì lấy ưu tiên ngày TH thực tế, nếu không có thì lấy qua ngày dự kiến
                    DateTime? startDate = taskViewModel.StartDate;
                    if (taskViewModel.Type == ConstWorkFlowCategory.THKH && taskViewModel.EndDate.HasValue)
                    {
                        startDate = taskViewModel.EndDate;
                    }
                    var summary = _unitOfWork.TaskRepository.GetSummary(taskViewModel.WorkFlowId, taskViewModel.VisitTypeCode, taskViewModel.ReceiveDate, startDate, taskViewModel.ProfileId, taskAssignList);
                    if (!string.IsNullOrEmpty(summary))
                    {
                        taskViewModel.Summary = summary;
                    }
                    #endregion

                    #region Assignee/ Người được phân công
                    //Lưu theo từng nhân viên
                    if ((taskViewModel.IsAssignGroup == null || taskViewModel.IsAssignGroup == false) && taskAssignList != null && taskAssignList.Count > 0)
                    {
                        foreach (var item in taskAssignList)
                        {
                            if (item.SalesEmployeeCode != null || item.RolesCode != null)
                            {
                                //Save task assign employee 
                                //Lưu nhân viên được phân công (kỹ thuật/ giám sát)/Nhóm được phân công
                                item.TaskId = taskViewModel.TaskId;
                                item.CreateBy = CurrentUser.AccountId;
                                _unitOfWork.TaskRepository.CreateTaskAssign(item);

                                //Save task reference
                                TaskReferenceModel referenceAssignee = new TaskReferenceModel();
                                referenceAssignee.TaskId = taskViewModel.TaskId;
                                referenceAssignee.CreateBy = CurrentUser.AccountId;
                                if (taskViewModel.IsAssignGroup != true && !string.IsNullOrEmpty(item.SalesEmployeeCode))
                                {
                                    referenceAssignee.SalesEmployeeCode = item.SalesEmployeeCode;
                                    referenceAssignee.Type = ConstTaskReference.Employee;
                                }
                                //if (taskViewModel.IsAssignGroup && !string.IsNullOrEmpty(item.RolesCode))
                                //{
                                //    referenceAssignee.RolesCode = item.RolesCode;
                                //    referenceAssignee.Type = ConstTaskReference.Group;
                                //}
                                _unitOfWork.TaskRepository.CreateTaskReference(referenceAssignee);
                            }
                        }
                    }
                    //Lưu theo nhóm: chỉ cần lưu các nhân viên theo custom không cần lưu nhóm nào
                    if (taskViewModel.IsAssignGroup == true && (taskAssignList == null || taskAssignList.Count == 0))
                    {
                        return _APIError("Vui lòng chọn ít nhất một nhân viên trong nhóm được phân công");
                    }
                    //Lưu theo nhóm - làm chung

                    if (taskViewModel.IsAssignGroup == true && taskViewModel.IsTogether == true)
                    {
                        foreach (var item in taskAssignList)
                        {
                            if (item.SalesEmployeeCode != null || item.RolesCode != null)
                            {
                                //Save task assign employee 
                                //Lưu nhân viên được phân công (kỹ thuật/ giám sát)/Nhóm được phân công
                                item.TaskId = taskViewModel.TaskId;
                                item.CreateBy = CurrentUser.AccountId;
                                _unitOfWork.TaskRepository.CreateTaskAssign(item);

                                //Save task reference
                                TaskReferenceModel referenceAssignee = new TaskReferenceModel();
                                referenceAssignee.TaskId = taskViewModel.TaskId;
                                referenceAssignee.CreateBy = CurrentUser.AccountId;

                                referenceAssignee.SalesEmployeeCode = item.SalesEmployeeCode;
                                referenceAssignee.Type = ConstTaskReference.Employee;

                                //if (taskViewModel.IsAssignGroup && !string.IsNullOrEmpty(item.RolesCode))
                                //{
                                //    referenceAssignee.RolesCode = item.RolesCode;
                                //    referenceAssignee.Type = ConstTaskReference.Group;
                                //}
                                _unitOfWork.TaskRepository.CreateTaskReference(referenceAssignee);
                            }
                        }
                    }
                    #endregion

                    #region Reporter/Người theo dõi/giám sát
                    if (taskReporterList != null && taskReporterList.Count > 0)
                    {
                        foreach (var item in taskReporterList)
                        {
                            if (!string.IsNullOrEmpty(item.SalesEmployeeCode))
                            {
                                //Save task report
                                //Lưu Người theo dõi/giám sát
                                item.TaskId = taskViewModel.TaskId;
                                item.CreateBy = taskViewModel.CreateBy;
                                _unitOfWork.TaskRepository.CreateTaskReporter(item);

                                //Save task reference
                                TaskReferenceModel referenceReporter = new TaskReferenceModel();
                                referenceReporter.TaskId = taskViewModel.TaskId;
                                referenceReporter.CreateBy = taskViewModel.CreateBy;
                                referenceReporter.SalesEmployeeCode = item.SalesEmployeeCode;
                                referenceReporter.Type = ConstTaskReference.Employee;
                                _unitOfWork.TaskRepository.CreateTaskReference(referenceReporter);
                            }
                        }
                    }
                    #endregion

                    #region SaleOrg/ Chi nhánh
                    //Lưu chi nhánh vào bảng task reference
                    TaskReferenceModel referenceStore = new TaskReferenceModel();
                    referenceStore.ObjectId = taskViewModel.StoreId;
                    referenceStore.Type = ConstTaskReference.Store;
                    referenceStore.TaskId = taskViewModel.TaskId;
                    referenceStore.CreateBy = taskViewModel.CreateBy;
                    _unitOfWork.TaskRepository.CreateTaskReference(referenceStore);
                    #endregion

                    #region Contact/ Liên hệ chính
                    //Lưu vào bảng TaskContact
                    if (taskViewModel.ContactId != null)
                    {
                        //Tìm profile id dựa theo contact nếu user không chọn profile (khách hàng)
                        if (taskViewModel.ProfileId == null)
                        {
                            var profileId = _unitOfWork.ProfileRepository.GetProfileByContact(taskViewModel.ContactId);
                            taskViewModel.ProfileId = profileId;
                        }
                        TaskContactModel contact = new TaskContactModel();
                        contact.TaskContactId = Guid.NewGuid();
                        contact.TaskId = taskViewModel.TaskId;
                        contact.ContactId = taskViewModel.ContactId;
                        contact.isMain = true;
                        contact.CreateBy = taskViewModel.CreateBy;
                        _unitOfWork.TaskRepository.CreateTaskContact(contact);

                        //Lưu vào bảng Task reference
                        TaskReferenceModel reference = new TaskReferenceModel();
                        reference.ObjectId = taskViewModel.ContactId;
                        reference.Type = ConstTaskReference.Contact;
                        reference.TaskId = taskViewModel.TaskId;
                        reference.CreateBy = taskViewModel.CreateBy;
                        _unitOfWork.TaskRepository.CreateTaskReference(reference);
                    }
                    #endregion

                    #region Profile/ Khách hàng
                    //Lưu khách hàng vào bảng task reference
                    if (taskViewModel.ProfileId != null)
                    {
                        TaskReferenceModel referenceAccount = new TaskReferenceModel();
                        referenceAccount.ObjectId = taskViewModel.ProfileId;
                        referenceAccount.Type = ConstTaskReference.Account;
                        referenceAccount.TaskId = taskViewModel.TaskId;
                        referenceAccount.CreateBy = taskViewModel.CreateBy;
                        _unitOfWork.TaskRepository.CreateTaskReference(referenceAccount);
                    }
                    #endregion

                    taskViewModel.CompanyId = _unitOfWork.CompanyRepository.GetCompanyIdBy(taskViewModel.CompanyCode);
                    taskViewModel.CreateBy = taskViewModel.CreateBy;
                    taskViewModel.CreateTime = DateTime.Now;
                    taskViewModel.Actived = true;
                    //Get lat lng of the VisitAddress
                    if (!string.IsNullOrEmpty(taskViewModel.VisitAddress))
                    {
                        if (string.IsNullOrEmpty(taskViewModel.lat) || string.IsNullOrEmpty(taskViewModel.lng))
                        {
                            try
                            {
                                var locationService = new GoogleLocationService(GoogleMapAPIKey);
                                var point = locationService.GetLatLongFromAddress(taskViewModel.VisitAddress);
                                taskViewModel.lat = point.Latitude.ToString();
                                taskViewModel.lng = point.Longitude.ToString();
                            }
                            catch (Exception)
                            {

                            }
                        }
                    }
                    if (taskViewModel.isPrivate == null)
                    {
                        taskViewModel.isPrivate = false;
                    }

                    //StartDate: Nếu cấu hình không hiển thị khi thêm mới và giá trị NULL thì set value = EstimateEndDate
                    var configList = _context.WorkFlowConfigModel.Where(p => p.WorkFlowId == taskViewModel.WorkFlowId).ToList();
                    if (configList != null && configList.Count > 0)
                    {
                        foreach (var item in configList)
                        {
                            if (item.FieldCode == "StartDate" && item.HideWhenAdd == true && taskViewModel.StartDate == null)
                            {
                                taskViewModel.StartDate = taskViewModel.EstimateEndDate;
                            }
                        }
                    }
                    _unitOfWork.TaskRepository.Create(taskViewModel);

                    _context.CurrentAccountId = taskViewModel.CreateBy;
                    _context.SaveChanges();

                    #region Push notification
                    var task = _context.TaskModel.Where(p => p.TaskId == taskViewModel.TaskId).FirstOrDefault();
                    if (task != null)
                    {
                        taskCode = taskViewModel.WorkFlowCode + "." + (!string.IsNullOrEmpty(task.SubtaskCode) ? task.SubtaskCode : task.TaskCode.ToString());
                        currentAccountName = _unitOfWork.SalesEmployeeRepository.GetSaleEmployeeNameBy(taskViewModel.SalesEmployeeCode);
                        notificationMessage = string.Format("{0} vừa được tạo bởi {1}", taskCode, currentAccountName);

                        string scheduleNotificationMessage = string.Empty;
                        if (isPushNotificationWithSchedule == true)
                        {
                            scheduleNotificationMessage = string.Format("{0} cần được hoàn thành vào {1:dd/MM/yyyy}", taskViewModel.Summary, taskViewModel.EstimateEndDate);
                        }
                        PushNotification(taskViewModel.CreateBy.Value, taskViewModel.TaskId, notificationMessage, task, isPushNotificationWithSchedule, scheduleNotificationMessage);
                    }
                    #endregion Push notification

                    //Create remind task
                    _unitOfWork.TaskRepository.UpdateRemindTask(taskViewModel, taskViewModel.CreateBy);

                    //Tạo subtask cho nhân viên nếu phân theo nhóm và chọn làm riêng
                    if (taskViewModel.IsAssignGroup == true && (taskViewModel.IsTogether == null || taskViewModel.IsTogether == false))
                    {
                        _unitOfWork.TaskRepository.UpdateGroupAssignTask(taskViewModel.TaskId, taskAssignList);
                    }
                }
                return _APISuccess(null, "Thêm mới công việc thành công!");
            });
        }
        #endregion Create new task

        #region Check location
        public ActionResult CheckLocation(CheckMobileLocationViewModel location, string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                //calculate distance between two geolocation
                var distance = DistanceTo(location.Latitude, location.Longtitude, location.CurrentLatitude, location.CurrentLongtitude);

                //find distance value from config
                ConfigRepository config = new ConfigRepository(_context);
                var checkinDistance = config.GetBy("CheckinDistance");
                if (checkinDistance != null)
                {
                    //if distance < checkinDistance => display modal popup on mobile
                    //else return error message
                    if (distance < Convert.ToDouble(checkinDistance))
                    {
                        location.Distance = distance;
                        return _APISuccess(location);
                    }
                    else
                    {
                        return _APIError(string.Format("Khoảng cách check in/out cho phép là: {0}m/{1}m", distance.ToString("n0"), checkinDistance));
                        //return _APISuccess(location);
                    }
                }
                else
                {
                    return _APIError("Chưa cấu hình khoảng cách cho phép check in/out");
                }

            });
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
        #endregion Check location

        #region Save check in/out
        public ActionResult SaveCheckIn(CheckMobileLocationViewModel location, string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                //1. Check in: Create new record check in
                //2. Check out: Update record with additional fields
                if (location.isCheckOut == true)
                {
                    var checkInOut = _context.CheckInOutModel.Where(p => p.CheckInOutId == location.CheckInOutId).FirstOrDefault();
                    if (checkInOut != null)
                    {
                        checkInOut.CheckOutLat = location.CurrentLatitude.ToString();
                        checkInOut.CheckOutLng = location.CurrentLongtitude.ToString();
                        checkInOut.CheckOutDistance = (decimal)location.Distance;
                        checkInOut.CheckOutBy = location.AccountId;
                        checkInOut.CheckOutTime = DateTime.Now;

                        _context.Entry(checkInOut).State = EntityState.Modified;

                        //[2020-11-13 15:51]: khi check out thì trạng thái tự chuyển Thành: Đã hoàn thành và Cập nhật luôn ngày kết thúc 
                        //Lấy trạng thái hoàn thành
                        var task = _context.TaskModel.Where(p => p.TaskId == checkInOut.TaskId).FirstOrDefault();
                        if (task != null)
                        {
                            var WorkFlowId = task.WorkFlowId;
                            if (WorkFlowId != null)
                            {
                                var taskStatusCompleteId = _context.TaskStatusModel.Where(p => p.WorkFlowId == WorkFlowId && p.ProcessCode == ConstProcess.completed)
                                                                                    .Select(p => p.TaskStatusId).FirstOrDefault();
                                if (taskStatusCompleteId != null)
                                {
                                    task.TaskStatusId = taskStatusCompleteId;
                                    task.EndDate = DateTime.Now;
                                }
                            }

                            _context.Entry(task).State = EntityState.Modified;
                        }
                    }
                }
                else
                {
                    CheckInOutModel addCheckInOut = new CheckInOutModel();
                    addCheckInOut.CheckInOutId = Guid.NewGuid();
                    addCheckInOut.TaskId = location.TaskId;
                    addCheckInOut.CheckInLat = location.CurrentLatitude.ToString();
                    addCheckInOut.CheckInLng = location.CurrentLongtitude.ToString();
                    addCheckInOut.CheckInDistance = (decimal)location.Distance;
                    addCheckInOut.CheckInBy = location.AccountId;
                    addCheckInOut.CheckInTime = DateTime.Now;

                    _context.Entry(addCheckInOut).State = EntityState.Added;
                }

                _context.CurrentAccountId = location.AccountId;
                _context.SaveChanges();

                return _APISuccess(null, "Cập nhật check in/out thành công!");
            });
        }
        #endregion Save check in/out

        #region Filter data
        public ActionResult FilterTask(TaskSearchViewModel searchViewModel, string KanbanCode, string UserNameCode, string CompanyCode, string StartFromDate_String, string StartToDate_String, string EstimateEndFromDate_String, string EstimateEndToDate_String, string token, string key, Guid? AccountId = null, List<Guid> WorkflowList = null, bool? isCreated = null, bool? isReporter = null, bool? isAssignee = null)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                if (string.IsNullOrEmpty(CompanyCode))
                {
                    var account = _context.AccountModel.Where(p => p.EmployeeCode == UserNameCode).FirstOrDefault();
                    if (account != null)
                    {
                        var store = account.StoreModel.FirstOrDefault();
                        if (store != null)
                        {
                            CompanyCode = _context.CompanyModel.Where(p => p.CompanyId == store.CompanyId).Select(p => p.CompanyCode).FirstOrDefault();
                        }
                    }
                }

                if (KanbanCode == ConstWorkFlowCategory.TICKET && CompanyCode == "2000")
                {
                    KanbanCode = ConstWorkFlowCategory.TICKET_MLC;
                }
                if (KanbanCode == "My_Follow")
                {
                    KanbanCode = "M_BOOKING_VISIT";
                }

                //Ngày bắt đầu
                if (!string.IsNullOrEmpty(StartFromDate_String))
                {
                    searchViewModel.StartFromDate = _unitOfWork.RepositoryLibrary.VNStringToDateTime(StartFromDate_String);
                }
                if (!string.IsNullOrEmpty(StartToDate_String))
                {
                    searchViewModel.StartToDate = _unitOfWork.RepositoryLibrary.VNStringToDateTime(StartToDate_String);
                }
                //Ngày đến hạn
                if (!string.IsNullOrEmpty(EstimateEndFromDate_String))
                {
                    searchViewModel.EstimateEndFromDate = _unitOfWork.RepositoryLibrary.VNStringToDateTime(EstimateEndFromDate_String);
                }
                if (!string.IsNullOrEmpty(EstimateEndToDate_String))
                {
                    searchViewModel.EstimateEndToDate = _unitOfWork.RepositoryLibrary.VNStringToDateTime(EstimateEndToDate_String);
                }

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

                //Load column base on KanbanCode
                //Guid KanbanId = Guid.Empty;

                searchViewModel.Type = KanbanCode;
                //searchViewModel.KanbanId = KanbanId;
                searchViewModel.IsMobile = true;
                //Trạng thái chưa hoàn thành
                //if (KanbanCode != ConstKanbanType.MyWork && KanbanCode != ConstKanbanType.MyFollow && string.IsNullOrEmpty(searchViewModel.TaskProcessCode))
                //{
                //    searchViewModel.TaskProcessCode = ConstTaskStatus.Incomplete;
                //}

                List<string> processCodeList = new List<string>();
                if (searchViewModel.TaskProcessCode == null)
                {
                    processCodeList.Add(ConstTaskStatus.Todo);
                    processCodeList.Add(ConstTaskStatus.Processing);
                    processCodeList.Add(ConstTaskStatus.Incomplete);
                    processCodeList.Add(ConstTaskStatus.CompletedOnTime);
                    processCodeList.Add(ConstTaskStatus.CompletedExpire);
                    processCodeList.Add(ConstTaskStatus.Expired);
                }

                ////Giao cho tôi => MyWork: Assignee = UserNameCode
                //if (KanbanCode == ConstKanbanType.MyWork)
                //{
                //    searchViewModel.Assignee = UserNameCode;
                //}
                ////Đang theo dõi => MyFollow: Reporter = UserNameCode
                //if (KanbanCode == ConstKanbanType.MyFollow)
                //{
                //    searchViewModel.Reporter = UserNameCode;
                //}




                if (isCreated == true && string.IsNullOrEmpty(searchViewModel.CreateBy))
                {
                    searchViewModel.CreateBy = UserNameCode;
                }
                if (isReporter == true && string.IsNullOrEmpty(searchViewModel.Reporter))
                {
                    searchViewModel.Reporter = UserNameCode;
                }
                if (isAssignee == true && string.IsNullOrEmpty(searchViewModel.Assignee))
                {
                    searchViewModel.Assignee = UserNameCode;
                }
                //if (string.IsNullOrEmpty(searchViewModel.CreateBy) && string.IsNullOrEmpty(searchViewModel.Reporter) && string.IsNullOrEmpty(searchViewModel.Assignee))
                //{
                //    searchViewModel.CreateBy = UserNameCode;
                //    searchViewModel.Reporter = UserNameCode;
                //    searchViewModel.Assignee = UserNameCode;
                //}
                //var searchModel = new TaskSearchViewModel();
                //if (isCreated == true)
                //{
                //    searchModel.CreateBy = UserNameCode;
                //}
                //if (isReporter == true)
                //{
                //    searchModel.Reporter = UserNameCode;
                //}
                //if (isAssignee == true)
                //{
                //    searchModel.Assignee = UserNameCode;
                //}

                //Trên mobile mặc định sẽ hiển thị danh sách task theo người đang đăng nhập nếu không truyền các tham số:
                //1.Người tạo
                //2.NV theo dõi
                //3.NV được phân công
                if (string.IsNullOrEmpty(searchViewModel.CreateBy) && string.IsNullOrEmpty(searchViewModel.Reporter) && string.IsNullOrEmpty(searchViewModel.Assignee))
                {
                    if (KanbanCode == ConstWorkFlowCategory.TICKET_MLC)
                    {
                        searchViewModel.Assignee = UserNameCode;
                    }
                    else if (KanbanCode == ConstWorkFlowCategory.MISSION)
                    {
                        searchViewModel.CreateBy = UserNameCode;
                        searchViewModel.Reporter = UserNameCode;
                        searchViewModel.Assignee = UserNameCode;
                    }
                }

                string DomainImageWorkFlow = ConstDomain.Domain + "/Upload/WorkFlow/";
                int filteredResultsCount = 0;

                //var taskList = _unitOfWork.TaskRepository.SearchQueryTask(searchViewModel, DomainImageWorkFlow, AccountId: AccountId).OrderBy(p => p.TaskStatusOrderIndex).ToList();
                var taskList = _unitOfWork.TaskRepository.SearchQueryTaskProc(searchViewModel, out filteredResultsCount, DomainImageWorkFlow: DomainImageWorkFlow, AccountId: AccountId, workflowList: WorkflowList, processCodeList: processCodeList);

                if (taskList != null && taskList.Count > 0)
                {
                    foreach (var item in taskList)
                    {
                        //Assignee
                        var assigneeLst = (from p in _context.TaskAssignModel
                                           join s in _context.SalesEmployeeModel on p.SalesEmployeeCode equals s.SalesEmployeeCode
                                           where p.TaskId == item.TaskId
                                           select new SalesEmployeeViewModel
                                           {
                                               SalesEmployeeCode = s.SalesEmployeeCode,
                                               SalesEmployeeName = s.SalesEmployeeName
                                           }).ToList();
                        if (assigneeLst != null && assigneeLst.Count() > 0)
                        {
                            item.AssigneeName = string.Join(", ", assigneeLst.Select(p => p.SalesEmployeeName).ToArray());
                            //Assignee on kanban
                            var avaLst = new List<string>();
                            foreach (var emp in assigneeLst)
                            {
                                avaLst.Add(emp.SalesEmployeeName.GetCharacterForLogoName());
                            }

                            if (avaLst != null && avaLst.Count > 0)
                            {
                                item.Avatar = string.Join(", ", avaLst);
                            }
                        }

                        //check in/out
                        if (item.isRequiredCheckin == true)
                        {
                            var checkInOutComplete = _context.CheckInOutModel.Where(p => p.TaskId == item.TaskId && p.CheckOutBy != null).FirstOrDefault();
                            if (checkInOutComplete != null)
                            {
                                item.isCheckInOutComplete = true;
                            }
                        }
                    }
                }

                //Check có hiển thị kanban hay không?
                var columns = new List<KanbanColumnViewModel>();
                var kanban = _context.KanbanModel.Where(p => p.KanbanCode == KanbanCode).FirstOrDefault();
                if (kanban != null)
                {
                    columns = _unitOfWork.KanbanTaskRepository.GetColumnKanban(kanban.KanbanId, HasUnmapped: false, IsMobile: true);
                }
                if (kanban.Actived != true)
                {
                    return _APISuccess(new { kanbanDetailList = taskList.OrderByDescending(p => p.TaskCode) });
                }

                return _APISuccess(new { columns, kanbanDetailList = taskList.OrderByDescending(p => p.TaskCode) });
            });
        }
        #endregion Filter data

        #region Get Estimated End Date of Task
        public ActionResult GetEstimatedEndDateOfTask(string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                //Gửi thông báo khi công việc đến hạn cho tất cả mọi người có liên quan đến task
                var currentDate = DateTime.Now;


                return _APISuccess(null);
            });
        }
        #endregion Get Estimated End Date of Task

        #region Calendar
        public ActionResult CalendarList(TaskSearchViewModel searchViewModel, string CompanyCode, string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
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

                return _APISuccess(res);

            });
        }
        #endregion

        #region Test
        public ActionResult ConvertStringToTimeSpan(string inputTime)
        {
            var _repoLibrary = new RepositoryLibrary();
            var time = _repoLibrary.VNStringToTimeSpan(inputTime);
            var result = DateTime.Now.Date.Add(time.Value);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}