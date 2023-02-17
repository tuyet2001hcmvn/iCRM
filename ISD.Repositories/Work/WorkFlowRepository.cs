using ISD.Constant;
using ISD.EntityModels;
using ISD.Repositories.Infrastructure.Extensions;
using ISD.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.Repositories
{
    public class WorkFlowRepository
    {
        EntityDataContext _context;

        /// <summary>
        /// Khởi tạo WorkFlow Repository
        /// </summary>
        /// <param name="db">EntityDataContext</param>
        public WorkFlowRepository(EntityDataContext db)
        {
            _context = db;
        }

        /// <summary>
        /// Tìm kiếm work flow
        /// </summary>
        /// <param name="searchModel">WorkFlow Search Model</param>
        /// <returns>List WorkFlowViewModel</returns>
        public List<WorkFlowViewModel> Search(WorkFlowSearchViewModel searchModel)
        {
            var workFlowList = (from w in _context.WorkFlowModel
                                join a in _context.AccountModel on w.CreateBy equals a.AccountId
                                where
                                (searchModel.WorkFlowName == null || w.WorkFlowName.Contains(searchModel.WorkFlowName))
                                && (searchModel.Actived == null || w.Actived == searchModel.Actived)
                                orderby w.OrderIndex
                                select new WorkFlowViewModel
                                {
                                    WorkFlowId = w.WorkFlowId,
                                    WorkFlowCode = w.WorkFlowCode,
                                    WorkflowCategoryCode = w.WorkflowCategoryCode,
                                    WorkFlowName = w.WorkFlowName,
                                    ImageUrl = w.ImageUrl,
                                    CreateUser = a.UserName,
                                    CreateTime = w.CreateTime,
                                    OrderIndex = w.OrderIndex,
                                    Actived = w.Actived,
                                    CompanyCode = w.CompanyCode
                                }).ToList();

            if (workFlowList != null && workFlowList.Count > 0)
            {
                foreach (var item in workFlowList)
                {
                    var taskStatusList = (from p in _context.TaskStatusModel
                                          where p.WorkFlowId == item.WorkFlowId
                                          orderby p.OrderIndex
                                          select p.TaskStatusCode).ToArray();
                    if (taskStatusList.Length > 0)
                    {
                        item.TaskStatusCode = string.Join("-", taskStatusList);
                    }
                }
            }

            return workFlowList;
        }

        /// <summary>
        /// Get All Work FLow
        /// </summary>
        /// <returns>Danh sách workflow</returns>
        public List<WorkFlowViewModel> GetAll()
        {
            var listWorkFlow = _context.WorkFlowModel.Where(p => p.Actived == true)
                                    .Select(p => new WorkFlowViewModel
                                    {
                                        WorkFlowId = p.WorkFlowId,
                                        WorkFlowCode = p.WorkFlowCode,
                                        WorkFlowName = p.WorkFlowName,
                                        WorkflowCategoryCode = p.WorkflowCategoryCode,
                                        ImageUrl = p.ImageUrl,
                                        OrderIndex = p.OrderIndex,
                                    }).OrderBy(p => p.OrderIndex).ToList();
            return listWorkFlow;
        }

        /// <summary>
        /// Get list Work FLow for Task
        /// </summary>
        /// <returns>Danh sách workflow theo yêu cầu (bảo hành/ lắp đặt/ khảo sát,..)</returns>
        public List<WorkFlowViewModel> GetWorkFlowBy(string Type, string CurrentCompanyCode = null)
        {
            //var listWorkFlow = (from p in _context.WorkFlowModel
            //                    where p.Actived == true
            //                    && (p.WorkflowCategoryCode == Type || Type == ConstWorkFlowCategory.ALL)
            //                    && (CurrentCompanyCode == null || (CurrentCompanyCode != null && p.CompanyCode.Contains(CurrentCompanyCode)))
            //                    orderby p.OrderIndex
            //                    select new WorkFlowViewModel
            //                    {
            //                        WorkFlowId = p.WorkFlowId,
            //                        WorkFlowCode = p.WorkFlowCode,
            //                        WorkFlowName = p.WorkFlowName,
            //                        WorkflowCategoryCode = p.WorkflowCategoryCode,
            //                        ImageUrl = p.ImageUrl,
            //                        OrderIndex = p.OrderIndex
            //                    })
            //                    .ToList();

            string sqlQuery = "EXEC [Task].[usp_GetWorkFlowListBy] @Type, @CurrentCompanyCode";

            #region Parameters
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter
                {
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input,
                    ParameterName = "Type",
                    Value = Type ?? (object)DBNull.Value,
                },
                new SqlParameter
                {
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input,
                    ParameterName = "CurrentCompanyCode",
                    Value = CurrentCompanyCode ?? (object)DBNull.Value,
                },
            };
            #endregion

            var listWorkFlow = _context.Database.SqlQuery<WorkFlowViewModel>(sqlQuery, parameters.ToArray()).ToList();
            return listWorkFlow;
        }

        /// <summary>
        /// Get all WorkFlow
        /// </summary>
        /// <returns>Danh sách workflow theo công ty</returns>
        public List<WorkFlowViewModel> GetAllWorkFlowBy (string CurrentCompanyCode)
        {
            string sqlQuery = "EXEC [Task].[usp_GetAllWorkFlowListBy] @CurrentCompanyCode";

            #region Parameters
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter
                {
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input,
                    ParameterName = "CurrentCompanyCode",
                    Value = CurrentCompanyCode ?? (object)DBNull.Value,
                },
            };
            #endregion

            var listWorkFlow = _context.Database.SqlQuery<WorkFlowViewModel>(sqlQuery, parameters.ToArray()).ToList();
            return listWorkFlow;
        }

        /// <summary>
        /// Lấy workflow theo workflowId
        /// </summary>
        /// <param name="workFlowId">Guid WorkflowId</param>
        /// <returns>WorkFlowViewModel</returns>
        public WorkFlowViewModel GetWorkFlow(Guid workFlowId)
        {
            var workFlow = (from w in _context.WorkFlowModel
                            where w.WorkFlowId == workFlowId
                            select new WorkFlowViewModel
                            {
                                WorkFlowId = w.WorkFlowId,
                                WorkFlowCode = w.WorkFlowCode,
                                WorkFlowName = w.WorkFlowName,
                                WorkflowCategoryCode = w.WorkflowCategoryCode,
                                ImageUrl = w.ImageUrl,
                                OrderIndex = w.OrderIndex,
                                CreateBy = w.CreateBy,
                                CreateTime = w.CreateTime,
                                LastEditBy = w.LastEditBy,
                                LastEditTime = w.LastEditTime,
                                Actived = w.Actived,
                                CompanyCode = w.CompanyCode,
                                IsDisabledSummary = w.IsDisabledSummary
                            }).FirstOrDefault();
            return workFlow;
        }

        /// <summary>
        /// Thêm mới Work Flow
        /// </summary>
        /// <param name="workFlowViewModel">WorkFlowViewModel</param>
        /// <returns>WorkFlowModel</returns>
        public WorkFlowModel Create(WorkFlowViewModel workFlowViewModel)
        {
            var workFlowNew = new WorkFlowModel();
            workFlowNew.MapWorkFlow(workFlowViewModel);
            workFlowNew.CreateBy = workFlowViewModel.CreateBy;
            workFlowNew.CreateTime = workFlowViewModel.CreateTime;
            _context.WorkFlowModel.Add(workFlowNew);
            return workFlowNew;
        }

        /// <summary>
        /// Cập nhật WorkFlow
        /// </summary>
        /// <param name="workFlowViewModel">WorkFlowViewModel</param>
        public void Update(WorkFlowViewModel workFlowViewModel)
        {
            var workFLowInDb = _context.WorkFlowModel.FirstOrDefault(p => p.WorkFlowId == workFlowViewModel.WorkFlowId);
            if (workFLowInDb != null)
            {
                workFLowInDb.MapWorkFlow(workFlowViewModel);
                workFLowInDb.LastEditBy = workFlowViewModel.LastEditBy;
                workFLowInDb.LastEditTime = workFlowViewModel.LastEditTime;

                _context.Entry(workFLowInDb).State = EntityState.Modified;
            }
        }

        /// <summary>
        /// Truyền vào workflowcode lấy workflowid
        /// </summary>
        /// <param name="WorkFlowCode">string: workflowCode</param>
        /// <returns>Guid: WorkFlowId</returns>
        public Guid FindWorkFlowIdByCode(string WorkFlowCode)
        {
            var workFlow = _context.WorkFlowModel.FirstOrDefault(p => p.WorkFlowCode == WorkFlowCode);
            return workFlow.WorkFlowId;
        }

        public StatusTransitionModel CreateTransition(StatusTransitionViewModel viewModel)
        {
            var transitionNew = new StatusTransitionModel
            {
                StatusTransitionId = viewModel.StatusTransitionId,
                TransitionName = viewModel.TransitionName,
                Description = viewModel.Description,
                unsignedBranchName = viewModel.unsignedBranchName,
                BranchName = viewModel.BranchName,
                WorkFlowId = viewModel.WorkFlowId,
                FromStatusId = viewModel.FromStatusId,
                ToStatusId = viewModel.ToStatusId,
                isAssigneePermission = viewModel.isAssigneePermission,
                isReporterPermission = viewModel.isReporterPermission,
                Color = viewModel.Color,
                StatusTransitionIn = viewModel.StatusTransitionIn,
                StatusTransitionOut = viewModel.StatusTransitionOut
            };
            _context.Entry(transitionNew).State = EntityState.Added;
            return transitionNew;
        }

        public bool UpdateTransition(StatusTransitionViewModel viewModel)
        {
            var transitionInDb = _context.StatusTransitionModel.FirstOrDefault(p => p.StatusTransitionId == viewModel.StatusTransitionId);
            if (transitionInDb == null)
            {
                return false;
            }
            transitionInDb.TransitionName = viewModel.TransitionName;
            transitionInDb.Description = viewModel.Description;
            transitionInDb.BranchName = viewModel.BranchName;
            transitionInDb.unsignedBranchName = viewModel.unsignedBranchName;
            transitionInDb.ToStatusId = viewModel.ToStatusId;
            transitionInDb.FromStatusId = viewModel.FromStatusId;
            transitionInDb.isAssigneePermission = viewModel.isAssigneePermission;
            transitionInDb.isReporterPermission = viewModel.isReporterPermission;
            transitionInDb.isRequiredComment = viewModel.isRequiredComment;
            transitionInDb.isAutomaticTransitions = viewModel.isAutomaticTransitions;
            transitionInDb.StatusTransitionIn = viewModel.StatusTransitionIn;
            transitionInDb.StatusTransitionOut = viewModel.StatusTransitionOut;
            transitionInDb.Color = viewModel.Color;


            _context.Entry(transitionInDb).State = EntityState.Modified;
            return true;
        }

        /// <summary>
        /// Lấy tên loại task (Ghé thăm, Hoạt động CSKH, Bảo hành, Điểm trưng bày, Giao việc)
        /// </summary>
        /// <param name="Type"></param>
        /// <returns></returns>
        public string GetWorkFlowCategory(string Type)
        {
            var res = _context.WorkFlowCategoryModel.Where(p => p.WorkFlowCategoryCode == Type)
                              .Select(p => p.WorkFlowCategoryName).FirstOrDefault();
            return res;
        }

        /// <summary>
        /// Lấy list Type theo workflow parent để tạo subtask
        /// </summary>
        /// <param name="WorkFlowId"></param>
        /// Lưu type subtask dưới dạng <Type>,<Type>,...
        /// Nếu có chọn giá trị mặc định WorkFlowId: <Type>,<Type>[WorkFlowId],<Type>,...
        /// <returns></returns>
        public List<WorkFlowCategoryViewModel> GetTypeByParentWorkFlow(Guid WorkFlowId)
        {
            List<string> ret = new List<string>();
            var type = _context.WorkFlowConfigModel.Where(p => p.WorkFlowId == WorkFlowId && p.FieldCode == "WorkFlowId")
                               .Select(p => p.Parameters)
                               .FirstOrDefault();

            if (!string.IsNullOrEmpty(type))
            {
                if (type.Contains(","))
                {
                    ret = type.Split(',').ToList();
                }
                else
                {
                    ret.Add(type);
                }
            }

            List<WorkFlowCategoryViewModel> res = new List<WorkFlowCategoryViewModel>();
            if (ret != null && ret.Count > 0)
            {
                foreach (var item in ret)
                {
                    Guid defaultWorkFlowId = Guid.Empty;
                    string WorkFlowCategoryCode = item;

                    if (item.Contains("[") && item.Contains("]"))
                    {
                        WorkFlowCategoryCode = item.Substring(0, item.IndexOf("["));
                    }
                    var code = _context.WorkFlowCategoryModel.FirstOrDefault(p => p.WorkFlowCategoryCode == WorkFlowCategoryCode);
                    if (code != null)
                    {
                        string WorkFlowCategoryName = code.WorkFlowCategoryName;
                        if (item.Contains("[") && item.Contains("]"))
                        {
                            var WorkFlowId_Str = item.Substring(item.IndexOf("[") + 1, 36);
                            defaultWorkFlowId = Guid.Parse(WorkFlowId_Str);
                            WorkFlowCategoryName = _context.WorkFlowModel.Where(p => p.WorkFlowId == defaultWorkFlowId)
                                                           .Select(p => p.WorkFlowName).FirstOrDefault();
                        }
                        WorkFlowCategoryViewModel model = new WorkFlowCategoryViewModel()
                        {
                            WorkFlowId = defaultWorkFlowId,
                            WorkFlowCategoryCode = WorkFlowCategoryCode,
                            WorkFlowCategoryName = WorkFlowCategoryName
                        };
                        res.Add(model);
                    }
                }
            }
            return res;
        }
    }
}
