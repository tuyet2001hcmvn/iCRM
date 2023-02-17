using ISD.Constant;
using ISD.EntityModels;
using ISD.Extensions;
using ISD.Resources;
using ISD.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.Repositories
{
    public class KanbanTaskRepository
    {
        EntityDataContext _context;

        /// <summary>
        /// Khởi tạo Kanban Task Repository
        /// </summary>
        /// <param name="db">Entiry Data Context</param>
        public KanbanTaskRepository(EntityDataContext db)
        {
            _context = db;
        }

        /// <summary>
        /// Lấy danh sách task status để cấu hình
        /// </summary>
        /// <param name="KanbanId"></param>
        /// <param name="WorkFlowId"></param>
        /// <param name="HasUnmapped">có column "Chưa cấu hình" để phân loại</param>
        /// <returns></returns>
        public List<KanbanTaskViewModel> GetKanbanDetailList(Guid KanbanId, Guid? WorkFlowId, bool? HasUnmapped)
        {
            var kanbanDetailList = (from p in _context.Kanban_TaskStatus_Mapping
                                    join k in _context.KanbanDetailModel on p.KanbanDetailId equals k.KanbanDetailId
                                    join s in _context.TaskStatusModel on p.TaskStatusId equals s.TaskStatusId
                                    join w in _context.WorkFlowModel on s.WorkFlowId equals w.WorkFlowId
                                    where (WorkFlowId == null || w.WorkFlowId == WorkFlowId)
                                    && k.KanbanId == KanbanId
                                    select new KanbanTaskViewModel
                                    {
                                        id = s.TaskStatusId.ToString(),
                                        state = k.OrderIndex.ToString(),
                                        label = w.WorkFlowCode + "." + s.TaskStatusCode,
                                        tags = w.WorkFlowName + " - " + s.TaskStatusName,
                                        code = s.ProcessCode
                                    }).ToList();

            if (HasUnmapped == true)
            {
                var columns = GetColumnKanban(KanbanId, HasUnmapped);
                var count = columns.Count();
                var taskStatusIdList = (from p in _context.Kanban_TaskStatus_Mapping
                                        join d in _context.KanbanDetailModel on p.KanbanDetailId equals d.KanbanDetailId
                                        join m in _context.KanbanModel on d.KanbanId equals m.KanbanId
                                        where m.KanbanId == KanbanId
                                        select p.TaskStatusId).ToList();

                var UnmappedList = (from p in _context.WorkFlowModel
                                    join s in _context.TaskStatusModel on p.WorkFlowId equals s.WorkFlowId
                                    where (WorkFlowId == null || p.WorkFlowId == WorkFlowId)
                                    && !taskStatusIdList.Contains(s.TaskStatusId)
                                    orderby p.OrderIndex, s.OrderIndex
                                    select new KanbanTaskViewModel
                                    {
                                        id = s.TaskStatusId.ToString(),
                                        state = count.ToString(),
                                        label = p.WorkFlowCode + "." + s.TaskStatusCode,
                                        tags = p.WorkFlowName + " - " + s.TaskStatusName,
                                        code = ConstProcess.unmapped
                                    }).ToList();

                if (kanbanDetailList == null)
                {
                    kanbanDetailList = new List<KanbanTaskViewModel>();
                }
                kanbanDetailList.AddRange(UnmappedList);
            }
            return kanbanDetailList;
        }

        /// <summary>
        /// Lấy danh sách các column của kanban
        /// </summary>
        /// <param name="KanbanId"></param>
        /// <param name="HasUnmapped">có column "Chưa cấu hình" để phân loại</param>
        /// <returns></returns>
        public List<KanbanColumnViewModel> GetColumnKanban(Guid? KanbanId, bool? HasUnmapped, bool? IsMobile = false)
        {
            var columns = (from p in _context.KanbanDetailModel
                           where p.KanbanId == KanbanId
                           orderby p.OrderIndex
                           select new KanbanColumnViewModel
                           {
                               text = IsMobile == true ? p.ColumnName : "<i class='fa fa-tasks'></i>" + p.ColumnName,
                               dataField = p.OrderIndex.ToString()
                           })
                           .ToList();

            if (HasUnmapped == true)
            {
                var count = columns.Count() + 1;
                if (columns == null)
                {
                    columns = new List<KanbanColumnViewModel>();
                }
                columns.Add(new KanbanColumnViewModel
                {
                    text = LanguageResource.NotYetConfiguration,
                    dataField = count.ToString()
                });
            }
            return columns;
        }

        /// <summary>
        /// Lấy danh sách các task để hiển thị ở kanban đã được cấu hình
        /// </summary>
        /// <param name="KanbanId"></param>
        /// <returns></returns>
        public List<KanbanTaskViewModel> GetTaskList(Guid KanbanId, bool? IsMobile = false)
        {
            var taskList = (from p in _context.TaskModel
                            join st in _context.TaskStatusModel on p.TaskStatusId equals st.TaskStatusId
                            join m in _context.Kanban_TaskStatus_Mapping on st.TaskStatusId equals m.TaskStatusId
                            join d in _context.KanbanDetailModel on m.KanbanDetailId equals d.KanbanDetailId
                            join w in _context.WorkFlowModel on st.WorkFlowId equals w.WorkFlowId
                            where d.KanbanId == KanbanId
                            select new KanbanTaskViewModel
                            {
                                id = p.TaskId.ToString(),
                                state = d.OrderIndex.ToString(),
                                label = p.Summary,
                                tags = w.WorkFlowCode + "." + p.TaskCode,
                                code = st.ProcessCode
                            }).ToList();

            //Assignee
            if (taskList != null && taskList.Count > 0)
            {
                foreach (var item in taskList)
                {
                    var TaskId = Guid.Parse(item.id);
                    item.label = IsMobile == true ? item.label : string.Format("<a href='/Work/Task/Edit/{0}'>{1}</a>", TaskId, item.label);
                    var empName = (from p in _context.TaskAssignModel
                                   join s in _context.SalesEmployeeModel on p.SalesEmployeeCode equals s.SalesEmployeeCode
                                   where p.TaskId == TaskId
                                   select new SalesEmployeeViewModel
                                   {
                                       SalesEmployeeCode = p.SalesEmployeeCode,
                                       SalesEmployeeName = s.SalesEmployeeName
                                   }).ToList();
                    if (empName != null && empName.Count > 0)
                    {
                        foreach (var emp in empName)
                        {
                            var href = string.Format("/Work/Task?Type=MyWork&tab=tab-kanban&Assignee={0}", emp.SalesEmployeeCode);
                            item.tags += string.Format(",<a href='{0}' title='{1}' target='_blank'><img src='https://i0.wp.com/avatar-management--avatars.us-west-2.prod.public.atl-paas.net/initials/{2}-0.png?ssl=1' /></a>", href, emp.SalesEmployeeName, emp.SalesEmployeeName.GetCharacterForLogoName());
                        }
                    }
                }
            }
            return taskList;
        }
    }
}
