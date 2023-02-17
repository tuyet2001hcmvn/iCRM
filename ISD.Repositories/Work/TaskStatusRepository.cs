using ISD.Constant;
using ISD.EntityModels;
using ISD.Repositories.Infrastructure.Extensions;
using ISD.Resources;
using ISD.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.Repositories
{
    public class TaskStatusRepository
    {
        EntityDataContext _context;
        /// <summary>
        /// Khởi tạo task repository
        /// </summary>
        /// <param name="dataContext">EntityDataContext</param>
        public TaskStatusRepository(EntityDataContext dataContext)
        {
            _context = dataContext;
        }

        /// <summary>
        /// Lấy danh sach tất cả task status
        /// </summary>
        /// <returns>List TaskStatusViewModel</returns>
        public List<TaskStatusViewModel> GetAll()
        {
            var taskStatusList = _context.TaskStatusModel.Where(p => p.Actived == true)
                                            .Select(p => new TaskStatusViewModel
                                            {
                                                TaskStatusId = p.TaskStatusId,
                                                TaskStatusName = p.TaskStatusName,
                                                WorkFlowId = p.WorkFlowId,
                                                OrderIndex = p.OrderIndex,
                                            }).OrderBy(p => p.OrderIndex).ToList();
            return taskStatusList;
        }

        /// <summary>
        /// Get TaskStatus by Id
        /// </summary>
        /// <param name="TaskStatusId">Guid</param>
        /// <returns>TaskStatusViewModel</returns>
        public TaskStatusViewModel GetBy(Guid TaskStatusId)
        {
            var taskStatus = _context.TaskStatusModel.Where(p => p.Actived == true && p.TaskStatusId == TaskStatusId)
                                            .Select(p => new TaskStatusViewModel
                                            {
                                                TaskStatusId = p.TaskStatusId,
                                                TaskStatusName = p.TaskStatusName,
                                                TaskStatusCode = p.TaskStatusCode,
                                                WorkFlowId = p.WorkFlowId,
                                                OrderIndex = p.OrderIndex,
                                                ProcessCode = p.ProcessCode,
                                            }).FirstOrDefault();
            return taskStatus;
        }

        /// <summary>
        /// Lấy danh sách task status theo workflow
        /// </summary>
        /// <param name="workFlowId">Guid workflowId</param>
        /// <returns>List TaskStatusViewModel</returns>
        public List<TaskStatusViewModel> GetTaskStatusByWorkFlow(Guid workFlowId, string Category = null)
        {
            var taskStatusList = (from p in _context.TaskStatusModel
                                  join c in _context.CatalogModel on p.ProcessCode equals c.CatalogCode
                                  where p.Actived == true 
                                  && p.WorkFlowId == workFlowId
                                  && ((Category == null || Category == "") || p.Category == Category)
                                  orderby p.OrderIndex
                                  select new TaskStatusViewModel
                                  {
                                      TaskStatusId = p.TaskStatusId,
                                      TaskStatusCode = p.TaskStatusCode,
                                      TaskStatusName = p.TaskStatusName,
                                      ProcessCode = p.ProcessCode,
                                      ProcessName = c.CatalogText_vi,
                                      WorkFlowId = p.WorkFlowId,
                                      OrderIndex = p.OrderIndex,
                                      CreateBy = p.CreateBy,
                                      CreateTime = p.CreateTime,
                                      LastEditBy = p.LastEditBy,
                                      LastEditTime = p.LastEditTime,
                                      Actived = p.Actived,
                                      Category = p.Category
                                  }).ToList();
            return taskStatusList;
        }

        public List<TaskStatusViewModel> GetTaskStatusByWorkFlowForConfig(Guid workFlowId)
        {
            var taskStatusList = (from p in _context.TaskStatusModel
                                  join c in _context.CatalogModel on p.ProcessCode equals c.CatalogCode
                                  join t in _context.StatusTransitionModel on p.TaskStatusId equals t.FromStatusId into tmp
                                  from de in tmp.DefaultIfEmpty()
                                  join to in _context.TaskStatusModel on de.ToStatusId equals to.TaskStatusId into tmp2
                                  from too in tmp2.DefaultIfEmpty()
                                  where p.Actived == true && p.WorkFlowId == workFlowId
                                  orderby p.OrderIndex
                                  select new TaskStatusViewModel
                                  {
                                      TaskStatusId = p.TaskStatusId,
                                      TaskStatusCode = p.TaskStatusCode,
                                      TaskStatusName = p.TaskStatusName,
                                      ProcessCode = p.ProcessCode,
                                      ProcessName = c.CatalogText_vi,
                                      StatusTransition = too.TaskStatusName,
                                      StatusTransitionId = de.StatusTransitionId,
                                      WorkFlowId = p.WorkFlowId,
                                      OrderIndex = p.OrderIndex,
                                      CreateBy = p.CreateBy,
                                      CreateTime = p.CreateTime,
                                      LastEditBy = p.LastEditBy,
                                      LastEditTime = p.LastEditTime,
                                      Actived = p.Actived
                                  });
            return taskStatusList.ToList();
        }

        /// <summary>
        /// Lấy danh mục Category dựa theo taskStatus hiện tại của task
        /// </summary>
        /// <param name="TaskStatusId"></param>
        /// <returns></returns>
        public string GetCategoryByTaskStatus(Guid? TaskStatusId)
        {
            var Category = (from p in _context.CatalogModel
                            join ts in _context.TaskStatusModel on p.CatalogCode equals ts.Category
                            where p.CatalogTypeCode == ConstCatalogType.HasRequest
                            && ts.TaskStatusId == TaskStatusId
                            select p.CatalogCode).FirstOrDefault();
            return Category;
        }

        /// <summary>
        /// Lấy list Task Status tiếp theo dựa vào status hiện tại
        /// </summary>
        /// <param name="WorkFlowId"></param>
        /// <param name="TaskStatusId"></param>
        /// <returns>List TaskStatusViewModel</returns>
        public List<TaskStatusViewModel> GetNextTaskStatus(Guid WorkFlowId, Guid TaskStatusId, Guid? KanbanDetailId)
        {
            var procesCode = (from p in _context.TaskStatusModel
                              where p.TaskStatusId == TaskStatusId
                              && p.Actived == true
                              && p.WorkFlowId == WorkFlowId
                              select p.ProcessCode).FirstOrDefault();

            var lst = (from p in _context.Kanban_TaskStatus_Mapping
                       where p.KanbanDetailId == KanbanDetailId
                       select p.TaskStatusId).ToList();

            var taskStatusList = (from p in _context.TaskStatusModel
                                  where procesCode == p.ProcessCode
                                  && p.WorkFlowId == WorkFlowId
                                  && lst.Contains(p.TaskStatusId)
                                  orderby p.OrderIndex
                                  select new TaskStatusViewModel
                                  {
                                      TaskStatusId = p.TaskStatusId,
                                      TaskStatusCode = p.TaskStatusCode,
                                      TaskStatusName = p.TaskStatusName,
                                      ProcessCode = p.ProcessCode,
                                      WorkFlowId = p.WorkFlowId,
                                      OrderIndex = p.OrderIndex,
                                      CreateBy = p.CreateBy,
                                      CreateTime = p.CreateTime,
                                      LastEditBy = p.LastEditBy,
                                      LastEditTime = p.LastEditTime,
                                      Actived = p.Actived
                                  })
                                  .ToList();
            return taskStatusList;
        }

        public string GetProcessCodeByTaskStatus(Guid WorkFlowId, Guid TaskStatusId)
        {
            var procesCode = (from p in _context.TaskStatusModel
                              where p.TaskStatusId == TaskStatusId
                              && p.Actived == true
                              && p.WorkFlowId == WorkFlowId
                              select p.ProcessCode).FirstOrDefault();
            return procesCode;
        }

        /// <summary>
        /// Kiểm tra cấu hình tự động cập nhật end date
        /// </summary>
        /// <param name="WorkFlowId"></param>
        /// <param name="TaskStatusId"></param>
        /// <returns>AutoUpdateEndDate</returns>
        public bool? CheckAutoEndDateByTaskStatus(Guid WorkFlowId, Guid TaskStatusId)
        {
            var result = (from p in _context.TaskStatusModel
                              where p.TaskStatusId == TaskStatusId
                              && p.Actived == true
                              && p.WorkFlowId == WorkFlowId
                              select p.AutoUpdateEndDate).FirstOrDefault();
            return result;
        }

        /// <summary>
        /// Tạo mới taskstatus
        /// </summary>
        /// <param name="taskStatusViewModel">TaskStatusViewModel</param>
        /// <returns>TaskStatusModel</returns>
        public TaskStatusModel Create(TaskStatusViewModel taskStatusViewModel)
        {
            var taskStatus = new TaskStatusModel();
            taskStatus.MapTaskStatus(taskStatusViewModel);
            taskStatus.TaskStatusId = Guid.NewGuid();
            taskStatus.CreateBy = taskStatusViewModel.CreateBy;
            taskStatus.CreateTime = taskStatusViewModel.CreateTime;
            _context.TaskStatusModel.Add(taskStatus);
            return taskStatus;
        }

        /// <summary>
        /// Cập nhật task status
        /// </summary>
        /// <param name="viewModel">TaskStatusViewModel</param>
        public void Update(TaskStatusViewModel viewModel)
        {
            var taskStatusInDb = _context.TaskStatusModel.FirstOrDefault(p => p.TaskStatusId == viewModel.TaskStatusId);
            if (taskStatusInDb != null)
            {
                taskStatusInDb.MapTaskStatus(viewModel);
                taskStatusInDb.LastEditBy = viewModel.LastEditBy;
                taskStatusInDb.LastEditTime = viewModel.LastEditTime;
                taskStatusInDb.Actived = viewModel.Actived;

                _context.Entry(taskStatusInDb).State = EntityState.Modified;
            }
        }

        /// <summary>
        /// Lưu trạng thái (task status) khi cập nhật tại kanban
        /// </summary>
        /// <param name="task"></param>
        /// <param name="taskStatusList"></param>
        /// <param name="KanbanId"></param>
        /// <param name="NextColumnName"></param>
        /// <param name="CurrentUserId"></param>
        /// <returns>TaskViewModel</returns>
        public TaskViewModel SaveTaskStatus(TaskViewModel task, List<TaskStatusViewModel> taskStatusList, Guid KanbanId, string NextColumnName, Guid? CurrentUserId)
        {
            var taskStatusIdList = taskStatusList.Select(p => p.TaskStatusId).ToList();
            if (taskStatusIdList.Contains(task.TaskStatusId))
            {
                var taskModel = _context.TaskModel.FirstOrDefault(p => p.TaskId == task.TaskId);
                if (taskModel != null)
                {
                    var newStatus = taskStatusList.FirstOrDefault(p => p.TaskStatusId == task.TaskStatusId);
                    //Cập nhật trạng thái mới để load màu dropdownlist
                    task.ProcessCode = newStatus.ProcessCode;

                    //Nếu chuyển sang trạng thái Hoàn thành => Set Ngày kết thúc là ngày hiện tại
                    //if (task.ProcessCode == ConstTaskStatus.Completed)
                    //{
                    //    taskModel.EndDate = DateTime.Now;
                    //}
                    taskModel.TaskStatusId = task.TaskStatusId;
                    taskModel.LastEditBy = CurrentUserId;
                    taskModel.LastEditTime = DateTime.Now;
                    _context.Entry(taskModel).State = EntityState.Modified;
                    _context.SaveChanges();
                }
            }
            return task;
        }

        /// <summary>
        /// Lấy danh sách task status để filter
        /// </summary>
        /// <returns></returns>
        public List<TaskStatusDropdownList> GetTaskStatusList()
        {
            var statusLst = new List<TaskStatusDropdownList>();
            //Incomplete = Todo + Processing
            statusLst.Add(new TaskStatusDropdownList { StatusCode = ConstTaskStatus.Incomplete, StatusName = LanguageResource.Incomplete });
            //Todo
            statusLst.Add(new TaskStatusDropdownList { StatusCode = ConstTaskStatus.Todo, StatusName = LanguageResource.Todo });
            //Processing
            statusLst.Add(new TaskStatusDropdownList { StatusCode = ConstTaskStatus.Processing, StatusName = LanguageResource.Processing });
            //Completed
            //statusLst.Add(new TaskStatusDropdownList { StatusCode = ConstTaskStatus.Completed, StatusName = LanguageResource.Completed });
            //CompletedOnTime
            statusLst.Add(new TaskStatusDropdownList { StatusCode = ConstTaskStatus.CompletedOnTime, StatusName = LanguageResource.CompletedOnTime });
            //CompletedExpire
            statusLst.Add(new TaskStatusDropdownList { StatusCode = ConstTaskStatus.CompletedExpire, StatusName = LanguageResource.CompletedExpire });
            //Expired
            statusLst.Add(new TaskStatusDropdownList { StatusCode = ConstTaskStatus.Expired, StatusName = LanguageResource.Expired });

            return statusLst;
        }

        public string GetDefaultValue(Guid workFlowId, string fieldCode)
        {
            var config = _context.WorkFlowConfigModel.Where(p => p.WorkFlowId == workFlowId && p.FieldCode == fieldCode).FirstOrDefault();
            if (config != null && !string.IsNullOrEmpty(config.AddDefaultValue))
            {
                return config.AddDefaultValue;
            }
            return null;
        }
    }
}
