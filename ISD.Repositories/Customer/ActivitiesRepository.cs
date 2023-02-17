using ISD.EntityModels;
using ISD.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using ISD.Constant;
using System.Threading.Tasks;

namespace ISD.Repositories
{
    public class ActivitiesRepository
    {
        private EntityDataContext _context;
        public ActivitiesRepository(EntityDataContext dataContext)
        {
            _context = dataContext;
        }

        public List<ProjectActivitiesViewModel> GetAll(Guid? profileId)
        {
            var activitiesList = (from a in _context.StatusTransition_Task_Mapping
                                      //from status
                                  join ts in _context.TaskStatusModel on a.FromStatusId equals ts.TaskStatusId into bGroup
                                  from b in bGroup.DefaultIfEmpty()
                                      //to status
                                  join ts2 in _context.TaskStatusModel on a.ToStatusId equals ts2.TaskStatusId into cGroup
                                  from c in cGroup.DefaultIfEmpty()
                                      //approve by
                                  join ap in _context.AccountModel on a.ApproveBy equals ap.AccountId into dGroup
                                  from d in dGroup.DefaultIfEmpty()
                                  join sTemp in _context.SalesEmployeeModel on d.EmployeeCode equals sTemp.SalesEmployeeCode into sList
                                  from s in sList.DefaultIfEmpty()
                                      //create by
                                  join cb in _context.AccountModel on a.CreateBy equals cb.AccountId into eGroup
                                  from e in eGroup.DefaultIfEmpty()
                                  join t in _context.TaskModel on a.TaskId equals t.TaskId
                                  where t.ProfileId == profileId
                                  orderby a.CreateTime descending
                                  select new ProjectActivitiesViewModel
                                  {
                                      TaskTransitionLogId = a.TaskTransitionLogId,
                                      FromStatusName = b.TaskStatusName,
                                      ToStatusName = c.TaskStatusName,
                                      Note = a.Note,
                                      ApproveName = s.SalesEmployeeShortName,
                                      ApproveTime = a.ApproveTime,
                                      CreateName = e.FullName,
                                      CreateTime = a.CreateTime,
                                  }).ToList();
            return activitiesList;
        }

        public TaskTransitionLogViewModel GetById(Guid TaskTransitionLogId)
        {
                var activities = _context.StatusTransition_Task_Mapping.FirstOrDefault(p => p.TaskTransitionLogId == TaskTransitionLogId);
                var activitiesVM = new TaskTransitionLogViewModel()
                {
                   TaskTransitionLogId = activities.TaskTransitionLogId,
                   TaskId = activities.TaskId,
                   FromStatusId = activities.FromStatusId,
                   ToStatusId = activities.ToStatusId,
                   Note = activities.Note,
                   ApproveBy = activities.ApproveBy,
                   ApproveTime = activities.ApproveTime,
                };
                return activitiesVM;
        }

        public void Create(StatusTransition_Task_Mapping model, Guid? CurrentUserId)
        {
            model.TaskTransitionLogId = Guid.NewGuid();
            model.CreateBy = CurrentUserId;
            model.CreateTime = DateTime.Now;
            if (model.ToStatusId != null && model.ToStatusId != Guid.Empty)
            {
                var status = new TaskStatusRepository(_context).GetBy(model.ToStatusId.Value);
                if (status != null)
                {
                    model.Number1 = new ConstantClass.CommonFunction().ConvertStringToDecimal(status.TaskStatusName.Replace("%", ""));
                }
            }
            _context.Entry(model).State = EntityState.Added;
        }

        public void Update(StatusTransition_Task_Mapping model)
        {
            var existModel = _context.StatusTransition_Task_Mapping.Where(p => p.TaskTransitionLogId == model.TaskTransitionLogId).FirstOrDefault();
            if (existModel != null)
            {
                existModel.ToStatusId = model.ToStatusId;
                existModel.Note = model.Note;
                existModel.ApproveBy = model.ApproveBy;
                existModel.ApproveTime = model.ApproveTime;
                if (model.ToStatusId != null && model.ToStatusId != Guid.Empty)
                {
                    var status = new TaskStatusRepository(_context).GetBy(model.ToStatusId.Value);
                    if (status != null)
                    {
                        existModel.Number1 = new ConstantClass.CommonFunction().ConvertStringToDecimal(status.TaskStatusName.Replace("%",""));
                    }
                }
                _context.Entry(existModel).State = EntityState.Modified;
            }
        }
    }
}
