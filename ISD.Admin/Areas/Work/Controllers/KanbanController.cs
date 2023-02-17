using ISD.Repositories;
using ISD.Resources;
using ISD.EntityModels;
using ISD.ViewModels;
using ISD.Core;
using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Data.Entity;

namespace Work.Controllers
{
    public class KanbanController : BaseController
    {
        // GET: Kanban/Index/{id}
        // Config Kanban base on Workflow
        #region Index
        public ActionResult Index(Guid id)
        {
            KanbanViewModel viewModel = new KanbanViewModel() { KanbanId = id };
            CreateViewBag();
            var title = _context.KanbanModel.FirstOrDefault(p => p.KanbanId == id);
            ViewBag.Title = string.Format("{0} {1} {2}", LanguageResource.Configuration, LanguageResource.Kanban.ToLower(), title.KanbanName);
            return View(viewModel);
        }
        [HttpPost]
        public ActionResult SaveTaskStatusMapping(Guid KanbanId, Guid TaskStatusId, int? OrderIndex)
        {
            return ExecuteContainer(() =>
            {
                var mapping = (from p in _context.Kanban_TaskStatus_Mapping
                               join d in _context.KanbanDetailModel on p.KanbanDetailId equals d.KanbanDetailId
                               join m in _context.KanbanModel on d.KanbanId equals m.KanbanId
                               where m.KanbanId == KanbanId
                               && p.TaskStatusId == TaskStatusId
                               select p).FirstOrDefault();
                if (mapping != null)
                {
                    _context.Entry(mapping).State = EntityState.Deleted;
                }

                var KanbanDetailId = _context.KanbanDetailModel.Where(p => p.KanbanId == KanbanId && p.OrderIndex == OrderIndex)
                                                               .Select(p => p.KanbanDetailId).FirstOrDefault();
                if (KanbanDetailId != null && KanbanDetailId != Guid.Empty)
                {
                    Kanban_TaskStatus_Mapping map = new Kanban_TaskStatus_Mapping();
                    map.KanbanDetailId = KanbanDetailId;
                    map.TaskStatusId = TaskStatusId;

                    _context.Entry(map).State = EntityState.Added;
                }
                _context.SaveChanges();
                return Json(new
                {
                    Code = HttpStatusCode.Created,
                    Success = true,
                    Data = ""
                });
            });
        }
        #endregion

        #region CreateViewBag, Helper
        public void CreateViewBag(Guid? WorkFlowId = null)
        {
            var listWorkFlow = _unitOfWork.WorkFlowRepository.GetAll();
            ViewBag.WorkFlowId = new SelectList(listWorkFlow, "WorkFlowId", "WorkFlowName", WorkFlowId);
        }

        public ActionResult GetAllWorkflow(Guid KanbanId, Guid? WorkFlowId)
        {
            //Load column base on KanbanCode
            var columns = _unitOfWork.KanbanTaskRepository.GetColumnKanban(KanbanId, HasUnmapped: true);
            var kanbanDetailList = _unitOfWork.KanbanTaskRepository.GetKanbanDetailList(KanbanId, WorkFlowId, HasUnmapped: true);

            return Json(new { columns, kanbanDetailList }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAllTask(Guid KanbanId)
        {
            //Load column base on KanbanCode
            var columns = _unitOfWork.KanbanTaskRepository.GetColumnKanban(KanbanId, HasUnmapped: false);
            var taskList = _unitOfWork.KanbanTaskRepository.GetTaskList(KanbanId);

            return Json(new { columns, kanbanDetailList = taskList }, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}