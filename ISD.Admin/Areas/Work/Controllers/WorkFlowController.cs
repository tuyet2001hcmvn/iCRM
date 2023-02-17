using ISD.Constant;
using ISD.Extensions;
using ISD.Repositories;
using ISD.Resources;
using ISD.ViewModels;
using ISD.Core;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ISD.EntityModels;

namespace Work.Controllers
{
    public class WorkFlowController : BaseController
    {
        #region Index
        // GET: WorkFlow
        [ISDAuthorizationAttribute]
        public ActionResult Index()
        {
            ViewBag.Actived = true;
            return View();
        }

        public ActionResult _Search(WorkFlowSearchViewModel searchModel)
        {
            return ExecuteSearch(() =>
            {
                var listProduct = _unitOfWork.WorkFlowRepository.Search(searchModel);
                return PartialView(listProduct);
            });
        }
        #endregion

        #region Create
        [ISDAuthorizationAttribute]
        public ActionResult Create()
        {
            CreateViewBag();
            return View();
        }

        [HttpPost]
        public JsonResult Create(WorkFlowViewModel viewModel, List<TaskStatusViewModel> taskStatusList, HttpPostedFileBase FileUpload, List<WorkFlowConfigViewModel> configList)
        {
            return ExecuteContainer(() =>
            {
                var workFlowId = Guid.NewGuid();
                var createBy = CurrentUser.AccountId;
                var createTime = DateTime.Now;

                // Hình ảnh
                if (FileUpload != null)
                {
                    viewModel.ImageUrl = getFileName(FileUpload, string.Format("WorkFlow_{0}", workFlowId.ToString()));
                }

                //Create Workflow
                viewModel.WorkFlowId = workFlowId;
                viewModel.CreateBy = createBy;
                viewModel.CreateTime = createTime;
                viewModel.Actived = true;
                _unitOfWork.WorkFlowRepository.Create(viewModel);

                //Create taskstatus
                #region Task Status
                foreach (var taskStatusVM in taskStatusList)
                {
                    if (taskStatusVM != null)
                    {
                        //3 Quy trình
                        taskStatusVM.WorkFlowId = workFlowId;
                        //5 Người tạo
                        taskStatusVM.CreateBy = createBy;
                        //6 Thời gian tạo
                        taskStatusVM.CreateTime = createTime;
                        //7 Trạng thái
                        taskStatusVM.Actived = true;
                        _unitOfWork.TaskStatusRepository.Create(taskStatusVM);
                    }
                }
                #endregion

                //Add config list
                #region WorkFlow Config
                if (configList != null && configList.Count > 0)
                {
                    foreach (var item in configList)
                    {
                        if (!string.IsNullOrEmpty(item.FieldCode))
                        {
                            WorkFlowConfigModel configModel = new WorkFlowConfigModel();
                            configModel.WorkFlowId = viewModel.WorkFlowId;
                            configModel.FieldCode = item.FieldCode;
                            configModel.OrderIndex = item.OrderIndex;
                            configModel.IsRequired = item.IsRequired;
                            configModel.Parameters = item.Parameters;
                            configModel.Note = item.Note;
                            configModel.HideWhenAdd = item.HideWhenAdd;
                            configModel.AddDefaultValue = item.AddDefaultValue;
                            configModel.HideWhenEdit = item.HideWhenEdit;
                            configModel.EditDefaultValue = item.EditDefaultValue;
                            _context.Entry(configModel).State = EntityState.Added;
                        }
                    }
                }
                #endregion

                _context.SaveChanges();

                return Json(new
                {
                    Code = HttpStatusCode.Created,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Create_Success, LanguageResource.WorkFlow.ToLower()),
                });
            });
        }
        #endregion

        #region Edit
        [ISDAuthorizationAttribute]
        public ActionResult Edit(Guid id)
        {
            var workFlow = _unitOfWork.WorkFlowRepository.GetWorkFlow(id);
            ViewBag.TaskStatus = _unitOfWork.TaskStatusRepository.GetTaskStatusByWorkFlow(id);

            CreateViewBag(workFlow.WorkFlowId);
            return View(workFlow);
        }
        [HttpPost]
        public JsonResult Edit(WorkFlowViewModel viewModel, List<TaskStatusViewModel> taskStatusList, HttpPostedFileBase FileUpload, List<WorkFlowFieldViewModel> configList)
        {
            return ExecuteContainer(() =>
            {
                var editBy = CurrentUser.AccountId;
                var editTime = DateTime.Now;

                if (FileUpload != null)
                {
                    //viewModel.ImageUrl = getFileName(FileUpload);
                    viewModel.ImageUrl = getFileName(FileUpload, string.Format("WorkFlow_{0}", viewModel.WorkFlowId.ToString()));
                }

                //Update WorkFlow
                viewModel.LastEditBy = editBy;
                viewModel.LastEditTime = editTime;
                viewModel.Actived = true;
                _unitOfWork.WorkFlowRepository.Update(viewModel);

                #region Update taskStatus
                //Xoá những taskstatus đa xoá trên view
                //Danh sách task status từ view
                var existTaskStatusIdLst = taskStatusList.Where(p => p.TaskStatusId != null).Select(p => p.TaskStatusId).ToList();
                //Danh sách task status cua workflow từ db
                var taskStatusIdListInDb = _unitOfWork.TaskStatusRepository.GetTaskStatusByWorkFlow(viewModel.WorkFlowId).Select(p => p.TaskStatusId).ToList();
                foreach (var taskStatusIdDb in taskStatusIdListInDb)
                {
                    //Nếu có trong db nhưng ko có trong view => Xoá
                    if (!existTaskStatusIdLst.Contains(taskStatusIdDb))
                    {
                        var delMapping = _context.Kanban_TaskStatus_Mapping.Where(p => p.TaskStatusId == taskStatusIdDb).ToList();
                        _context.Kanban_TaskStatus_Mapping.RemoveRange(delMapping);

                        var delItem = _context.TaskStatusModel.FirstOrDefault(p => p.TaskStatusId == taskStatusIdDb);
                        _context.Entry(delItem).State = EntityState.Deleted;
                    }
                }

                foreach (var taskStatusVM in taskStatusList)
                {
                    taskStatusVM.WorkFlowId = viewModel.WorkFlowId;
                    taskStatusVM.Actived = true;

                    //Nếu có Id => Sửa
                    if (taskStatusVM.TaskStatusId != Guid.Empty && taskStatusVM.TaskStatusId != null)
                    {
                        taskStatusVM.LastEditBy = editBy;
                        taskStatusVM.LastEditTime = editTime;
                        if (taskStatusVM.ProcessCode == null)
                        {
                            var delMapping = _context.Kanban_TaskStatus_Mapping.Where(p => p.TaskStatusId == taskStatusVM.TaskStatusId).ToList();
                            _context.Kanban_TaskStatus_Mapping.RemoveRange(delMapping);

                            var delItem = _context.TaskStatusModel.FirstOrDefault(p => p.TaskStatusId == taskStatusVM.TaskStatusId);
                            _context.Entry(delItem).State = EntityState.Deleted;
                        }
                        else
                        {
                            _unitOfWork.TaskStatusRepository.Update(taskStatusVM);
                        }
                    }
                    //Nếu không có Id => Thêm
                    else
                    {
                        taskStatusVM.TaskStatusId = Guid.NewGuid();
                        taskStatusVM.CreateBy = editBy;
                        taskStatusVM.CreateTime = editTime;
                        if (taskStatusVM.ProcessCode != null)
                        {
                            _unitOfWork.TaskStatusRepository.Create(taskStatusVM);
                        }
                    }
                }
                #endregion

                #region Update WorkFlow Config
                //Delete all config list
                var exitConfigList = _context.WorkFlowConfigModel.Where(p => p.WorkFlowId == viewModel.WorkFlowId).ToList();
                _context.WorkFlowConfigModel.RemoveRange(exitConfigList);
                //Add config list
                if (configList != null && configList.Count > 0)
                {
                    foreach (var item in configList)
                    {
                        if (!string.IsNullOrEmpty(item.FieldCode))
                        {
                            WorkFlowConfigModel configModel = new WorkFlowConfigModel();
                            configModel.WorkFlowId = viewModel.WorkFlowId;
                            configModel.FieldCode = item.FieldCode;
                            configModel.OrderIndex = item.OrderIndex_Config;
                            configModel.IsRequired = item.IsRequired;
                            configModel.Parameters = item.Parameters;
                            configModel.Note = item.Note;
                            configModel.HideWhenAdd = item.HideWhenAdd;
                            configModel.AddDefaultValue = item.AddDefaultValue;
                            configModel.HideWhenEdit = item.HideWhenEdit;
                            configModel.EditDefaultValue = item.EditDefaultValue;
                            _context.Entry(configModel).State = EntityState.Added;
                        }
                    }
                }
                #endregion

                _context.SaveChanges();

                return Json(new
                {
                    Code = HttpStatusCode.Created,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Edit_Success, LanguageResource.WorkFlow.ToLower()),
                });
            });
        }
        #endregion

        #region Config transition
        // [ISDAuthorizationAttribute]
        public ActionResult Config(Guid id)
        {
            var taskStatusList = _unitOfWork.TaskStatusRepository.GetTaskStatusByWorkFlowForConfig(id);
            return View(taskStatusList);
        }
        public ActionResult _CreateTransition(Guid TaskStatusId)
        {
            var taskStatus = _unitOfWork.TaskStatusRepository.GetBy(TaskStatusId);
            StatusTransitionViewModel transitionVM = new StatusTransitionViewModel
            {
                WorkFlowId = taskStatus.WorkFlowId,
                FromStatusId = TaskStatusId,
                isRequiredComment = false,
            };
            var taskStatusList = _unitOfWork.TaskStatusRepository.GetTaskStatusByWorkFlow((Guid)taskStatus.WorkFlowId);
            ViewBag.FromStatusId = new SelectList(taskStatusList, "TaskStatusId", "TaskStatusName", TaskStatusId);
            ViewBag.ToStatusId = new SelectList(taskStatusList, "TaskStatusId", "TaskStatusName");
            return PartialView("_FormStatusTransition", transitionVM);
        }

        public ActionResult _EditTransition(Guid StatusTransactionId)
        {
            var statusTransition = _context.StatusTransitionModel.FirstOrDefault(p => p.StatusTransitionId == StatusTransactionId);
            StatusTransitionViewModel transitionVM = new StatusTransitionViewModel
            {
                StatusTransitionId = statusTransition.StatusTransitionId,
                WorkFlowId = statusTransition.WorkFlowId,
                FromStatusId = statusTransition.FromStatusId,
                ToStatusId = statusTransition.ToStatusId,
                TransitionName = statusTransition.TransitionName,
                Description = statusTransition.Description,
                isAssigneePermission = statusTransition.isAssigneePermission,
                isReporterPermission = statusTransition.isReporterPermission,
                Color = statusTransition.Color,
                isRequiredComment = statusTransition.isRequiredComment,
            };
            var taskStatusList = _unitOfWork.TaskStatusRepository.GetTaskStatusByWorkFlow((Guid)statusTransition.WorkFlowId);
            ViewBag.FromStatusId = new SelectList(taskStatusList, "TaskStatusId", "TaskStatusName", statusTransition.FromStatusId);
            ViewBag.ToStatusId = new SelectList(taskStatusList, "TaskStatusId", "TaskStatusName", statusTransition.ToStatusId);
            return PartialView("_FormStatusTransition", transitionVM);
        }

        [HttpPost]
        public ActionResult Save(StatusTransitionViewModel statusTransitionVM)
        {
            return ExecuteContainer(() =>
            {
                if (statusTransitionVM.StatusTransitionId == Guid.Empty)
                {
                    #region Create
                    statusTransitionVM.StatusTransitionId = Guid.NewGuid();

                    _unitOfWork.WorkFlowRepository.CreateTransition(statusTransitionVM);
                    _context.SaveChanges();

                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.Created,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Create_Success, LanguageResource.Customer_CustomerAddress.ToLower())
                    });
                    #endregion
                }
                else
                {
                    #region Edit
                    var ret = _unitOfWork.WorkFlowRepository.UpdateTransition(statusTransitionVM);
                    if (ret)
                    {
                        _context.SaveChanges();
                        return Json(new
                        {
                            Code = System.Net.HttpStatusCode.Created,
                            Success = true,
                            Data = string.Format(LanguageResource.Alert_Edit_Success, LanguageResource.Customer_CustomerAddress.ToLower())
                        });
                    }
                    else
                    {
                        return Json(new
                        {
                            Code = System.Net.HttpStatusCode.NotFound,
                            Success = false,
                            Data = ""
                        });
                    }
                    #endregion
                }
            });
        }

        public ActionResult DeleteTransition(Guid id)
        {
            return ExecuteDelete(()=>
            {
                var statusTransition = _context.StatusTransitionModel.FirstOrDefault(p => p.StatusTransitionId == id);
                if (statusTransition == null)
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotFound,
                        Success = false,
                        Data = ""
                    });
                }
                _context.Entry(statusTransition).State = EntityState.Deleted;
                _context.SaveChanges();
                return Json(new
                {
                    Code = System.Net.HttpStatusCode.Created,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Delete_Success, LanguageResource.StatusTransition.ToLower())
                });
            });
        }
        #endregion

        #region ViewBag
        private void CreateViewBag(Guid? WorkFlowId = null)
        {
            ViewBag.processList = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.Process);

            List<WorkFlowFieldViewModel> resultList = new List<WorkFlowFieldViewModel>();
            List<string> existFieldCodeList = new List<string>();
            if (WorkFlowId != null)
            {
                var configList = (from p in _context.WorkFlowConfigModel
                                  join f in _context.WorkFlowFieldModel on p.FieldCode equals f.FieldCode
                                  where p.WorkFlowId == WorkFlowId
                                  select new WorkFlowFieldViewModel
                                  {
                                      FieldCode = p.FieldCode,
                                      FieldName = f.FieldName,
                                      OrderIndex = f.OrderIndex,
                                      Description = f.Description,
                                      OrderIndex_Config = p.OrderIndex,
                                      IsChoose = true,
                                      IsRequired = p.IsRequired,
                                      Note = p.Note,
                                      Parameters = p.Parameters,
                                      HideWhenAdd = p.HideWhenAdd,
                                      AddDefaultValue = p.AddDefaultValue,
                                      HideWhenEdit = p.HideWhenEdit,
                                      EditDefaultValue = p.EditDefaultValue
                                  }).ToList();

                resultList.AddRange(configList);
                existFieldCodeList = configList.Select(p => p.FieldCode).ToList();
            }
            var fieldList = (from p in _context.WorkFlowFieldModel
                             where !existFieldCodeList.Contains(p.FieldCode)
                             orderby p.OrderIndex
                             select new WorkFlowFieldViewModel()
                             {
                                 FieldCode = p.FieldCode,
                                 FieldName = p.FieldName,
                                 OrderIndex = p.OrderIndex,
                                 Description = p.Description
                             }).ToList();
            resultList.AddRange(fieldList);

            ViewBag.WorkFlowField = resultList;
        }

        public string getFileName(HttpPostedFileBase file, string fileName = null)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                fileName = Path.GetFileName(file.FileName);
            }
            else
            {
                var extension = file.FileName.Substring(file.FileName.LastIndexOf("."), 4);
                fileName = fileName + extension;
            }

            //Create dynamically folder to save file
            var existPath = Server.MapPath("~/Upload/WorkFlow");
            Directory.CreateDirectory(existPath);
            var path = Path.Combine(existPath, fileName);

            file.SaveAs(path);

            return fileName;
        }
        #endregion

        #region workflow
        public ActionResult Workflow(Guid id)
            {
            var taskStatusList = (from p in _context.TaskStatusModel
                                  join c in _context.CatalogModel
                                  on p.ProcessCode equals c.CatalogCode
                                  where p.WorkFlowId == id && c.CatalogTypeCode == "process"

                                  select new TaskStatusViewModel
                                  {
                                      WorkFlowId = p.WorkFlowId,
                                      PositionLef = p.PositionLeft,
                                      PositionRight = p.PositionRight,
                                      taskId = p.TaskStatusId.ToString(),
                                      TaskStatusName = p.TaskStatusName,
                                      color = c.CatalogText_en,
                                      typeShape = "rectangle"
                                  }).ToList();
            var listTransition = (from p in _context.StatusTransitionModel
                                  where p.WorkFlowId == id && p.BranchName != null

                                  group p by new { p.FromStatusId, p.unsignedBranchName } into g
                                  select new
                                  {
                                      fromStatustId = g.Key.FromStatusId,
                                      unsignedBranchName = g.Key.unsignedBranchName,
                                      count = g.Count()
                                  }).ToList();
            foreach (var item in listTransition)
            {
                if (item.count > 1) 
                {
                    string taskStatusId = "";
                    var transtatusId = (_context.StatusTransitionModel.Where(x => x.FromStatusId == item.fromStatustId && x.unsignedBranchName == item.unsignedBranchName)).FirstOrDefault();
                    var list = (_context.StatusTransitionModel.Where(x => x.FromStatusId == transtatusId.FromStatusId)).ToList();
                    foreach (var i in list)
                    {
                        taskStatusId = taskStatusId + i.StatusTransitionId.ToString() + "+";
                    }
                    taskStatusId = taskStatusId.Substring(0, taskStatusId.Length - 1);
                    
                    taskStatusList.Add(new TaskStatusViewModel()
                    {

                        WorkFlowId = Guid.Empty,
                        PositionLef = transtatusId.BranchPositionLeft == null ? 1031 : transtatusId.BranchPositionLeft,
                        PositionRight = transtatusId.BranchPositionRight == null ? 293 : transtatusId.BranchPositionRight,

                        //taskId = taskStatusId,
                        taskId = transtatusId.FromStatusId + "+" +(transtatusId.unsignedBranchName== null ?"null" : transtatusId.unsignedBranchName),
                        TaskStatusName = transtatusId.BranchName,
                        color = "#FFF,#000",
                        typeShape = "rhombus"
                    });
                }
            }

            return View(taskStatusList);
        }
        // [HttpGet]
        public ActionResult GetStatusTransition(Guid id)
        {
            var statusTrasition = (from p in _context.StatusTransitionModel
                                   where p.WorkFlowId == id
                                   orderby p.FromStatusId
                                   select new
                                   {
                                       p.StatusTransitionId,
                                       p.unsignedBranchName,
                                       p.FromStatusId,
                                       p.ToStatusId,
                                       p.TransitionName,
                                       p.StatusTransitionIn,
                                       p.StatusTransitionOut
                                   }).ToList();
            //statusTrasition.Add(new
            //{
            //    StatusTransitionId = "",
            //    FromStatusId = Guid.Parse("EC0479B8-831F-4068-A497-D21F2543B125"),
            //    ToStatusId = Guid.Parse("EC0479B8-831F-4068-A497-D21F2543B125"),
            //    TransitionName = "Test",
            //    FromPosition = "",
            //    ToPosition = ""
            //});
            ////var aaa = JsonConvert.SerializeObject(statusTrasition);


            return Json(new
            {
                Code = System.Net.HttpStatusCode.Created,
                Success = true,
                Data = statusTrasition
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]

        public ActionResult UpdatePositionTaskTransition(string id, int positionLeft, int positionRight, string shape)
        {



            if (id.Length <= 36 && shape == "rectangle")
            {
                Guid statusTransId = Guid.Parse(id);
                var taskStatus = _context.TaskStatusModel.FirstOrDefault(p => p.TaskStatusId == statusTransId);
                taskStatus.PositionLeft = positionLeft;
                taskStatus.PositionRight = positionRight;

                _context.Entry(taskStatus).State = EntityState.Modified;
                _context.SaveChanges();
                return Json(new
                {
                    Code = System.Net.HttpStatusCode.OK,
                    Success = false,
                    Data = ""
                });
            }
            else
            {
                string[] param = id.Split('+');
                Guid fromStatusId = Guid.Parse(param[0]);
                string branchName = param[1];
                var listStatusTransition = (_context.StatusTransitionModel.Where(x => x.FromStatusId == fromStatusId && x.unsignedBranchName == branchName)).ToList();
                foreach (var statusTransition in listStatusTransition)
                {
                    var unsignedName = _unitOfWork.UtilitiesRepository.RemoveSign4VietnameseString(statusTransition.BranchName.ToUpper().Replace(" ", "_"));
                    //var  statusTransition = (_context.StatusTransitionModel.Where(x => x.StatusTransitionId == statusTransId)).FirstOrDefault();
                    statusTransition.BranchIn = "Top";
                    statusTransition.BranchOut = "Buttom";
                    statusTransition.BranchPositionLeft = positionLeft;
                    statusTransition.BranchPositionRight = positionRight;
                    statusTransition.unsignedBranchName = unsignedName;
                    _context.Entry(statusTransition).State = EntityState.Modified;
                }
                _context.SaveChanges();
                return Json(new
                {
                    Code = System.Net.HttpStatusCode.OK,
                    Success = false,
                    Data = ""
                });
            }

        }

        [HttpPost]
        public ActionResult DeleteStatusTransition(Guid transitionId)
        {
            return ExecuteDelete(() =>
            {
                var statusTransition = _context.StatusTransitionModel.Where(x => x.StatusTransitionId == transitionId).FirstOrDefault();
                //var listCondition = _context.AutoConditionModel.Where(x => x.StatusTransitionId == transitionId).ToList();
                if (statusTransition != null)
                {
                    _context.Entry(statusTransition).State = EntityState.Deleted;
                    //_context.AutoConditionModel.RemoveRange(listCondition);
                    _context.SaveChanges();
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.OK,
                        Success = true,
                        Data = ""
                    });
                }
                return Json(new
                {
                    Code = System.Net.HttpStatusCode.NotFound,
                    Success = false,
                    Data = ""
                });

            });
        }

        public ActionResult FindStatusTransition(Guid transitionId)
        {
            var statusTransition = _context.StatusTransitionModel.Where(x => x.StatusTransitionId == transitionId).FirstOrDefault();
            if (statusTransition != null)
            {
                return Json(new
                {
                    Code = System.Net.HttpStatusCode.OK,
                    Success = false,
                    Data = statusTransition.TransitionName
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new
            {
                Code = System.Net.HttpStatusCode.NotFound,
                Success = false,
                Data = "Not Found"
            }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult _WFCreateTransition(string fromStatusId, string toStatusId, string fromPosition, string toPosition, string statusTransitionId)
        {
            // Edit Trasition
            if (statusTransitionId != "00000000-0000-0000-0000000000")
            {
                var id = Guid.Parse(statusTransitionId);
                var transition = (_context.StatusTransitionModel.Where(x => x.StatusTransitionId == id)).FirstOrDefault();
                var taskStatus = _unitOfWork.TaskStatusRepository.GetBy((Guid)transition.FromStatusId);
                var FromStatusId = ((fromStatusId == null || fromStatusId.Length > 36) ? transition.FromStatusId : Guid.Parse(fromStatusId));
                var ToStatusId = ((toStatusId == null || toStatusId.Length > 36) ? transition.ToStatusId : Guid.Parse(toStatusId));
                StatusTransitionViewModel statusTransition;
                if (fromPosition == "" && toPosition == "")
                {
                    statusTransition = (from p in _context.StatusTransitionModel
                                        where p.StatusTransitionId == id
                                        select new StatusTransitionViewModel
                                        {
                                            BranchName = p.BranchName,
                                            TransitionName = p.TransitionName,
                                            Description = p.Description,
                                            Color = p.Color,
                                            WorkFlowId = taskStatus.WorkFlowId,
                                            FromStatusId = FromStatusId,
                                            ToStatusId = ToStatusId,
                                            isRequiredComment = false,
                                            StatusTransitionIn = p.StatusTransitionIn,
                                            StatusTransitionOut = p.StatusTransitionOut,
                                            isAutomaticTransitions = p.isAutomaticTransitions
                                        }).FirstOrDefault();
                }
                else
                {

                    //var FromStatusId = ((fromStatusId == null || fromStatusId.Length >36)  ? transition.FromStatusId : Guid.Parse(fromStatusId));
                    //var ToStatusId = (toStatusId == null ? transition.ToStatusId : Guid.Parse(toStatusId));
                    statusTransition = (from p in _context.StatusTransitionModel
                                        where p.StatusTransitionId == id
                                        select new StatusTransitionViewModel
                                        {
                                            BranchName = p.BranchName,
                                            TransitionName = p.TransitionName,
                                            Description = p.Description,
                                            Color = p.Color,
                                            WorkFlowId = taskStatus.WorkFlowId,
                                            FromStatusId = FromStatusId,
                                            ToStatusId = ToStatusId,
                                            isRequiredComment = false,
                                            StatusTransitionIn = fromPosition,
                                            StatusTransitionOut = toPosition,
                                            isAutomaticTransitions = p.isAutomaticTransitions
                                        }).FirstOrDefault();


                }
                var taskStatusList = _unitOfWork.TaskStatusRepository.GetTaskStatusByWorkFlow((Guid)taskStatus.WorkFlowId);
                ViewBag.listTaskStatus = new SelectList(taskStatusList, "TaskStatusId", "TaskStatusName");
                //ViewBag.ToStatusId = new SelectList(taskStatusList, "TaskStatusId", "TaskStatusName", ToStatusId);
                ViewBag.Type = true; // update or delete
                return PartialView("_FormStatusTransition", statusTransition);
            }
            // Create Transition
            else
            {
                var taskStatus = _unitOfWork.TaskStatusRepository.GetBy(Guid.Parse(fromStatusId));
                StatusTransitionViewModel transitionVM = new StatusTransitionViewModel
                {
                    BranchName = null,
                    WorkFlowId = taskStatus.WorkFlowId,
                    FromStatusId = Guid.Parse(fromStatusId),
                    ToStatusId = Guid.Parse(toStatusId),
                    isRequiredComment = false,
                    isAutomaticTransitions = false,
                    StatusTransitionIn = fromPosition,
                    StatusTransitionOut = toPosition,
                };
                var taskStatusList = _unitOfWork.TaskStatusRepository.GetTaskStatusByWorkFlow((Guid)taskStatus.WorkFlowId);
                ViewBag.listTaskStatus = new SelectList(taskStatusList, "TaskStatusId", "TaskStatusName");
                //ViewBag.ToStatusId = new SelectList(taskStatusList, "TaskStatusId", "TaskStatusName", toStatusId);
                ViewBag.Type = false; // delete
                return PartialView("_FormStatusTransition", transitionVM);
            }


        }

        // update status model
        public ActionResult FindTaskStatus(Guid? taskstatusId, Guid? workflowId)
        {
            // add task status
            if (taskstatusId == Guid.Empty || taskstatusId == null)
            {
                var ListProcess = (_context.CatalogModel.Where(x => x.CatalogTypeCode == "process")).ToList();

                TaskStatusViewModel taskstatus = new TaskStatusViewModel();
                taskstatus.WorkFlowId = workflowId;

                ViewBag.ProcessCode = new SelectList(ListProcess, "CatalogCode", "CatalogCode");
                return PartialView("_FormUpdateTaskStatus", taskstatus);
            }
            //edit taskStatus
            else
            {
                var ListProcess = (_context.CatalogModel.Where(x => x.CatalogTypeCode == "process")).ToList();

                var taskstatus = (from p in _context.TaskStatusModel
                                  where p.TaskStatusId == taskstatusId
                                  select new TaskStatusViewModel
                                  {
                                      TaskStatusId = p.TaskStatusId,
                                      TaskStatusName = p.TaskStatusName,
                                      WorkFlowId = p.WorkFlowId,
                                      OrderIndex = p.OrderIndex,
                                      CreateBy = p.CreateBy,
                                      CreateTime = p.CreateTime,
                                      LastEditBy = p.LastEditBy,
                                      LastEditTime = p.LastEditTime,
                                      Actived = p.Actived,
                                      TaskStatusCode = p.TaskStatusCode,
                                      ProcessCode = p.ProcessCode,
                                      Category = p.Category,
                                      PositionLef = p.PositionLeft,
                                      PositionRight = p.PositionRight,
                                      AutoUpdateEndDate = p.AutoUpdateEndDate
                                  }).FirstOrDefault();

                ViewBag.ProcessCode = new SelectList(ListProcess, "CatalogCode", "CatalogCode", taskstatus.ProcessCode);
                return PartialView("_FormUpdateTaskStatus", taskstatus);
            }
        }

        // update status
        public ActionResult UpdateTaskStatus(TaskStatusModel model)
        {
            //create task status
            if (model.TaskStatusId == Guid.Empty)
            {
                model.TaskStatusId = Guid.NewGuid();
                model.Actived = true;
                _context.Entry(model).State = EntityState.Added;
                _context.SaveChanges();
                return Json(new
                {
                    Code = System.Net.HttpStatusCode.Created,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Create_Success, LanguageResource.WorkFlow.ToLower())
                });
            }
            else
            {
                _context.Entry(model).State = EntityState.Modified;
                _context.SaveChanges();
                return Json(new
                {
                    Code = System.Net.HttpStatusCode.Created,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Create_Success, LanguageResource.WorkFlow.ToLower())
                });

            }

        }
        // delete task status
        public ActionResult DeleteTaskStatus(Guid taskStatusId)
        {
            return ExecuteDelete(() =>
            {
                var statusTransition = (_context.StatusTransitionModel.Where(x => x.FromStatusId == taskStatusId || x.ToStatusId == taskStatusId)).ToList();
                if (statusTransition.Count() > 0)
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.BadRequest,
                        Success = false,
                        Data = "Delete Error !!!.The DELETE statement conflicted with the REFERENCE...."
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var taskStatus = (_context.TaskStatusModel.Where(x => x.TaskStatusId == taskStatusId)).FirstOrDefault();
                    _context.Entry(taskStatus).State = EntityState.Deleted;
                    _context.SaveChanges();
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.OK,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Create_Success, LanguageResource.TaskStatus.ToLower())
                    }, JsonRequestBehavior.AllowGet);
                }

            });

        }
        #endregion

        #region Autocondition
        public ActionResult CreateAutoCondtion(Guid workFlowId, Guid? statusTransitionId)
        {
            var workFlowCF = (from p in _context.WorkFlowConfigModel
                              where p.WorkFlowId == workFlowId
                              select new
                              {
                                  fieldCode = p.FieldCode,
                                  fieldName = p.Note ?? p.FieldCode
                              }).ToList();



            if (statusTransitionId != null)
            {
                var conditionVM = (from p in _context.AutoConditionModel
                                   where p.StatusTransitionId == statusTransitionId
                                   orderby p.OrderIndex
                                   select new AutoConditionViewModel()
                                   {
                                       AutoConditionId = p.AutoConditionId,
                                       AdditionalSQLText = p.AdditionalSQLText,
                                       ConditionType = p.ConditionType,
                                       Field = p.Field,
                                       ComparisonType = p.ComparisonType,
                                       ValueType = p.ValueType,
                                       Value = p.Value,
                                       SQLText = p.SQLText
                                   }).ToList();
                ViewBag.ListField = new SelectList(workFlowCF, "fieldCode", "fieldName");
                return PartialView("_AutoCondition", conditionVM);
            }
            else
            {
                var initial = new AutoConditionViewModel()
                {
                    AdditionalSQLText = "",
                    ConditionType = "",
                    Field = "",
                    ComparisonType = "",
                    ValueType = "",
                    Value = "",
                    SQLText = ""
                };
                List<AutoConditionViewModel> conditionVM = new List<AutoConditionViewModel>();
                conditionVM.Add(initial);
                ViewBag.ListField = new SelectList(workFlowCF, "fieldCode", "fieldName");
                return PartialView("_AutoCondition", conditionVM);
            }
        }
        // Delete condition
        public ActionResult DeleteCondition(Guid autoConditionId)
        {
            var condition = (_context.AutoConditionModel.Where(x => x.AutoConditionId == autoConditionId)).FirstOrDefault();

            if (condition != null)
            {
                _context.Entry(condition).State = EntityState.Deleted;
                _context.SaveChanges();
                return Json(new
                {
                    Code = System.Net.HttpStatusCode.OK,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Create_Success, LanguageResource.AutoCondition.ToLower())
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new
                {
                    Code = System.Net.HttpStatusCode.NotFound,
                    Success = false,
                    Data = string.Format(LanguageResource.Alert_NotExist_Delete, LanguageResource.AutoCondition.ToLower())
                }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Test
        //public ActionResult SaveTest(StatusTransitionViewModel statusTransitionVM, List<AutoConditionModel> listCondition)
        [HttpPost]
        public ActionResult SaveTest(StatusTransitionViewModel statusTransitionVM, List<AutoConditionModel> listCondition)
        {
            // Save new trasition 
            if (statusTransitionVM.TransitionName != null)
            {
                if (statusTransitionVM.BranchName != null)
                {
                    statusTransitionVM.unsignedBranchName = _unitOfWork.UtilitiesRepository.RemoveSign4VietnameseString(statusTransitionVM.BranchName.ToUpper().Replace(" ", "_"));
                }

                string id = statusTransitionVM.StatusTransitionId.ToString();
                if (statusTransitionVM.StatusTransitionId == Guid.Empty || id == "00000000-0000-0000-0000000000")
                {
                    #region Create
                    statusTransitionVM.StatusTransitionId = Guid.NewGuid();

                    _unitOfWork.WorkFlowRepository.CreateTransition(statusTransitionVM);
                    // add auto condition
                    if (listCondition != null)
                    {
                        for (int i = 0; i < listCondition.Count(); i++)
                        {
                            listCondition[i].AutoConditionId = Guid.NewGuid();
                            listCondition[i].OrderIndex = i;
                            listCondition[i].StatusTransitionId = statusTransitionVM.StatusTransitionId;
                            _context.Entry(listCondition[i]).State = EntityState.Added;

                        }
                    }

                    _context.SaveChanges();

                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.Created,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Create_Success, LanguageResource.WorkFlow.ToLower())
                    });
                    #endregion
                }
                else
                {
                    // edit transition
                    #region Edit
                    // update status trandisition
                    if (statusTransitionVM.BranchName != null)
                    {
                        statusTransitionVM.unsignedBranchName = _unitOfWork.UtilitiesRepository.RemoveSign4VietnameseString(statusTransitionVM.BranchName.ToUpper().Replace(" ", "_"));
                    }
                    var ret = _unitOfWork.WorkFlowRepository.UpdateTransition(statusTransitionVM);
                    // remove auto condition
                    var listAutocondition = (_context.AutoConditionModel.Where(x => x.StatusTransitionId == statusTransitionVM.StatusTransitionId)).ToList();
                    _context.AutoConditionModel.RemoveRange(listAutocondition);

                    // add new auto condition
                    if (listCondition != null)
                    {
                        for (int i = 0; i < listCondition.Count(); i++)
                        {
                            listCondition[i].AutoConditionId = Guid.NewGuid();
                            listCondition[i].OrderIndex = i;
                            listCondition[i].StatusTransitionId = statusTransitionVM.StatusTransitionId;
                            _context.Entry(listCondition[i]).State = EntityState.Added;

                        }
                    }

                    if (ret)
                    {

                        _context.SaveChanges();
                        return Json(new
                        {
                            Code = System.Net.HttpStatusCode.Created,
                            Success = true,
                            Data = string.Format(LanguageResource.Alert_Edit_Success, LanguageResource.Customer_CustomerAddress.ToLower())
                        });
                    }
                    else
                    {
                        return Json(new
                        {
                            Code = System.Net.HttpStatusCode.NotFound,
                            Success = false,
                            Data = ""
                        });
                    }
                    #endregion
                }

            }
            else
            {
                return Json(new
                {
                    Code = System.Net.HttpStatusCode.NotFound,
                    Success = false,
                    Data = "Vui lòng nhập đầy đủ  thông tin "
                });
            };
        }
        #endregion
    }
}