using ISD.Constant;
using ISD.Core;
using ISD.EntityModels;
using ISD.Repositories;
using ISD.Resources;
using ISD.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Customer.Controllers
{
    public class ActivitiesController : BaseController
    {
        private ActivitiesRepository _activitiesRepository;

        public ActivitiesController()
        {
            _activitiesRepository = new ActivitiesRepository(_context);
        }
        // GET: Activities
        public ActionResult _List(Guid? id, bool? isLoadContent = false)
        {
            return ExecuteSearch(() =>
            {
                var result = new List<ProjectActivitiesViewModel>();
                result = _activitiesRepository.GetAll(id);

                //Nếu có cấu hình transition thì hiển thị nút cập nhật theo cấu hình
                //Lấy danh sách cấu hình
                var workFlow = _context.WorkFlowModel.Where(p => p.WorkFlowCode == ConstWorkFlow.PROJECT).FirstOrDefault();
                if (workFlow != null)
                {
                    //Lấy trạng thái mới nhất để lọc những trạng thái kế tiếp chuyển đến
                    var task = _context.TaskModel.Where(p => p.ProfileId == id).FirstOrDefault();
                    if (task != null)
                    {
                        var lastestStatusId = _context.StatusTransition_Task_Mapping.Where(p => p.TaskId == task.TaskId)
                                                                                    .OrderByDescending(p => p.CreateTime)
                                                                                    .Select(p => p.ToStatusId)
                                                                                    .FirstOrDefault();
                        var transitionLst = _context.StatusTransitionModel.Where(p => p.WorkFlowId == workFlow.WorkFlowId).ToList();
                        if (lastestStatusId != null)
                        {
                            ViewBag.TransitionList = transitionLst.Where(p => p.FromStatusId == lastestStatusId).OrderBy(p => p.TransitionName).ToList();
                        }
                    }
                }

                if (isLoadContent == true)
                {
                    return PartialView("_ListContent", result);
                }

                return PartialView(result);
            });
        }

        public ActionResult _Create(Guid ProfileId, Guid? ToStatusId)
        {
            TaskTransitionLogViewModel adbVM = new TaskTransitionLogViewModel();
            adbVM.ApproveTime = DateTime.Now;
            var task = _context.TaskModel.Where(p => p.ProfileId == ProfileId).FirstOrDefault();
            if (task != null)
            {
                adbVM.TaskId = task.TaskId;
                if (task.TaskStatusId != null && task.TaskStatusId != Guid.Empty)
                {
                    adbVM.FromStatusId = task.TaskStatusId;
                }
                if (ToStatusId.HasValue)
                {
                    adbVM.ToStatusId = ToStatusId;
                }
                //Nếu đã có dữ liệu transition thì hiển thị trạng thái mới nhất đã lưu
                var transition = _context.StatusTransition_Task_Mapping.Where(p => p.TaskId == task.TaskId).OrderByDescending(p => p.CreateTime).FirstOrDefault();
                if (transition != null)
                {
                    adbVM.FromStatusId = transition.ToStatusId;
                }

                CreateViewBag(task.WorkFlowId, adbVM.FromStatusId, ToStatusId, CurrentUser.AccountId);
            }
            else
            {
                CreateViewBag();
            }
            return PartialView("_FormActivities", adbVM);
        }

        public ActionResult _Edit(Guid TaskTransitionLogId)
        {
            var activitiesVM = _activitiesRepository.GetById(TaskTransitionLogId);
            CreateViewBag(FromStatusId: activitiesVM.FromStatusId, ToStatusId: activitiesVM.ToStatusId, AppoveBy: activitiesVM.ApproveBy);
            return PartialView("_FormActivities", activitiesVM);
        }

        [HttpPost]
        public ActionResult Save(StatusTransition_Task_Mapping mapping)
        {
            if (!mapping.ToStatusId.HasValue || mapping.ToStatusId == Guid.Empty)
            {
                ModelState.AddModelError("ToStatusId", "Vui lòng chọn thông tin \"Trạng thái chuyển đến\"");
            }
            return ExecuteContainer(() =>
            {
                //Thêm mới
                if (mapping.TaskTransitionLogId == Guid.Empty)
                {
                    _activitiesRepository.Create(mapping, CurrentUser.AccountId);
                }
                else
                {
                    _activitiesRepository.Update(mapping);
                }
                _context.SaveChanges();

                return Json(new
                {
                    Code = System.Net.HttpStatusCode.Created,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Edit_Success, LanguageResource.TaskStatus.ToLower())
                });
            });
        }

        #region Delete
        [HttpPost]
        public ActionResult Delete(Guid id)
        {
            return ExecuteDelete(() =>
            {
                var activities = _context.StatusTransition_Task_Mapping.FirstOrDefault(p => p.TaskTransitionLogId == id);
                if (activities != null)
                {
                    _context.Entry(activities).State = EntityState.Deleted;
                    _context.SaveChanges();

                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.Created,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Delete_Success, LanguageResource.TabActivities.ToLower())
                    });
                }
                else
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotModified,
                        Success = false,
                        Data = ""
                    });
                }
            });
        }
        #endregion

        #region CreateViewBag, Helper
        public void CreateViewBag(Guid? WorkFlowId = null, Guid? FromStatusId = null, Guid? ToStatusId = null, Guid? AppoveBy = null)
        {
            //FromStatusId
            if (!WorkFlowId.HasValue)
            {
                WorkFlowId = _context.WorkFlowModel.Where(p => p.WorkFlowCode == ConstWorkFlow.PROJECT).Select(p => p.WorkFlowId).FirstOrDefault();
            }
            var statusLst = _unitOfWork.TaskStatusRepository.GetTaskStatusByWorkFlow(WorkFlowId.Value);
            ViewBag.FromStatusId = new SelectList(statusLst, "TaskStatusId", "TaskStatusName", FromStatusId);

            //ToStatusId
            ViewBag.ToStatusId = new SelectList(statusLst, "TaskStatusId", "TaskStatusName", ToStatusId);

            //ApproveBy
            var approveLst = _unitOfWork.SalesEmployeeRepository.GetAllForDropdownlist();
            ViewBag.ApproveBy = new SelectList(approveLst, "AccountId", "SalesEmployeeName", AppoveBy);

        }

        public ActionResult GetTransitionList(Guid ProfileId)
        {
            return ExecuteSearch(() =>
            {
                List<StatusTransitionViewModel> result = new List<StatusTransitionViewModel>();
                //Nếu có cấu hình transition thì hiển thị nút cập nhật theo cấu hình
                //Lấy danh sách cấu hình
                var workFlow = _context.WorkFlowModel.Where(p => p.WorkFlowCode == ConstWorkFlow.PROJECT).FirstOrDefault();
                if (workFlow != null)
                {
                    //Lấy trạng thái mới nhất để lọc những trạng thái kế tiếp chuyển đến
                    var task = _context.TaskModel.Where(p => p.ProfileId == ProfileId).FirstOrDefault();
                    if (task != null)
                    {
                        var lastestStatusId = _context.StatusTransition_Task_Mapping.Where(p => p.TaskId == task.TaskId)
                                                                                    .OrderByDescending(p => p.CreateTime)
                                                                                    .Select(p => p.ToStatusId)
                                                                                    .FirstOrDefault();
                        var transitionLst = _context.StatusTransitionModel.Where(p => p.WorkFlowId == workFlow.WorkFlowId).ToList();
                        if (lastestStatusId != null)
                        {
                            result = transitionLst.Where(p => p.FromStatusId == lastestStatusId).OrderBy(p => p.TransitionName)
                                                    .Select(p => new StatusTransitionViewModel() {
                                                        Color = p.Color,
                                                        TransitionName = p.TransitionName,
                                                        ToStatusId = p.ToStatusId,
                                                        isRequiredComment = p.isRequiredComment,
                                                    }).ToList();
                        }
                    }
                }

                return Json(new
                {
                    Success = true,
                    Data = result,
                }, JsonRequestBehavior.AllowGet);
            });
        }
        #endregion
    }
}