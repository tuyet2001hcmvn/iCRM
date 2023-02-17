using ISD.Constant;
using ISD.Core;
using ISD.EntityModels;
using ISD.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Work.Controllers
{
    public class AssignedGroupController : BaseController
    {
        public ActionResult Index(string Type)
        {
            CreateViewBag(Type);
            return View();
        }

        public void CreateViewBag(string Type) 
        {
            string pageUrl = "/Work/AssignedGroup";
            var parameter = "?Type=" + Type;
            var title = (from p in _context.PageModel
                         where p.PageUrl == pageUrl
                         && p.Parameter.Contains(Type)
                         select p.PageName).FirstOrDefault();
            ViewBag.Title = title;
            ViewBag.PageId = GetPageId(pageUrl, parameter);
            ViewBag.Type = Type;

            var saleEmployeeList = _unitOfWork.SalesEmployeeRepository.GetAllForDropdownlist();
            ViewBag.AccountIdList = new SelectList(saleEmployeeList, "SalesEmployeeCode", "SalesEmployeeName");
            ViewBag.SaleEmployeeList = saleEmployeeList;
        }
        #region Create
        public ActionResult Create(string Type)
        {
            CreateViewBag(Type);
            return View();
        }
        [HttpPost]
        public JsonResult Create(AssignedGroupCreateModel createViewModel)
        {
            //check tên trống
            if (string.IsNullOrEmpty(createViewModel.GroupName))
            {
                return Json(new
                {
                    IsSuccess = false,
                    Message = "Tên nhóm không được để trống"
                }, JsonRequestBehavior.AllowGet);
            }
            // check tên tồn tại
            var existGroup = _context.TaskGroupModel.FirstOrDefault(s => s.GroupName.ToUpper() == createViewModel.GroupName.ToUpper() && s.CreatedAccountId == CurrentUser.AccountId && s.GroupType == ConstTaskGroupType.TaskGroup);
            if (existGroup != null)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Message = "Tên nhóm đã tồn tại"
                }, JsonRequestBehavior.AllowGet);
            }
            //check list thành viên trống
            if (createViewModel.AccountIdList == null || createViewModel.AccountIdList.Count == 0)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Message = "Thành viên trong nhóm không được trống"
                }, JsonRequestBehavior.AllowGet);
            }

            //Master
            TaskGroupModel taskGroup = new TaskGroupModel();
            taskGroup.GroupId = Guid.NewGuid();
            taskGroup.GroupName = createViewModel.GroupName;
            taskGroup.CreatedAccountId = CurrentUser.AccountId;
            taskGroup.GroupType = createViewModel.Type;

            _context.Entry(taskGroup).State = EntityState.Added;

            //Detail
            foreach (var employeeCode in createViewModel.AccountIdList)
            {
                var account = _context.AccountModel.Where(p => p.EmployeeCode == employeeCode).FirstOrDefault();
                if (account != null)
                {
                    TaskGroupDetailModel taskGroupDetail = new TaskGroupDetailModel();
                    taskGroupDetail.GroupId = taskGroup.GroupId;
                    taskGroupDetail.AccountId = account.AccountId;
                    taskGroupDetail.Note = string.Format("{0} được tạo bởi {1}", taskGroup.GroupName, CurrentUser.UserName);

                    _context.Entry(taskGroupDetail).State = EntityState.Added;
                }
            }

            _context.SaveChanges();
            return Json(new
            {
                IsSuccess = true,
                Data = taskGroup.GroupId,
                Type= createViewModel.Type,
                Message = "Tạo mới nhóm thành công"
            }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Edit
        public ActionResult Edit(Guid id, string Type)
        {
            ViewBag.Id = id;
            CreateViewBag(Type);
            return View();
        }
        [HttpPost]
        public JsonResult Edit(Guid id, AssignedGroupCreateModel createViewModel)
        {
            //check tên trống
            if (string.IsNullOrEmpty(createViewModel.GroupName))
            {
                return Json(new
                {
                    IsSuccess = false,
                    Message = "Tên nhóm không được để trống"
                }, JsonRequestBehavior.AllowGet);
            }
            //check list thành viên trống
            if (createViewModel.AccountIdList == null || createViewModel.AccountIdList.Count == 0)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Message = "Thành viên trong nhóm không được trống"
                }, JsonRequestBehavior.AllowGet);
            }
            // check tồn tại
            var existGroup = _context.TaskGroupModel.Find(id);
            if (existGroup == null)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Message = "Nhóm không tồn tại"
                }, JsonRequestBehavior.AllowGet);
            }


            //Master
            existGroup.GroupName = createViewModel.GroupName;
            _context.Entry(existGroup).State = EntityState.Modified;

            //Detail
            //Xóa thành viên cũ
            var oldMembers = _context.TaskGroupDetailModel.Where(s => s.GroupId == existGroup.GroupId);
            foreach (var oldMember in oldMembers)
            {
                _context.Entry(oldMember).State = EntityState.Deleted;
            }
            //thêm thành viên mới
            foreach (var employeeCode in createViewModel.AccountIdList)
            {
                var account = _context.AccountModel.Where(p => p.EmployeeCode == employeeCode).FirstOrDefault();
                if (account != null)
                {
                    TaskGroupDetailModel taskGroupDetail = new TaskGroupDetailModel();
                    taskGroupDetail.GroupId = existGroup.GroupId;
                    taskGroupDetail.AccountId = account.AccountId;
                    taskGroupDetail.Note = string.Format("{0} được tạo bởi {1}", existGroup.GroupName, CurrentUser.UserName);

                    _context.Entry(taskGroupDetail).State = EntityState.Added;
                }
            }

            _context.SaveChanges();
            return Json(new
            {
                IsSuccess = true,
                Message = "Cập nhật nhóm thành công"
            }, JsonRequestBehavior.AllowGet);
        }
        #endregion


        #region Search
        [HttpGet]
        public JsonResult Search(string groupName,int pageIndex,int PageSize, string Type)
        {
            var list = from assignedGroup in _context.TaskGroupModel
                       where (assignedGroup.GroupName == null || assignedGroup.GroupName == "" || assignedGroup.GroupName.Contains(groupName))
                       &&(assignedGroup.CreatedAccountId == CurrentUser.AccountId)
                       && assignedGroup.GroupType == Type
                       select assignedGroup;
            int totalRow = list.Count();
            var paging = list.OrderBy(s=>s.GroupName).Skip((pageIndex - 1) * PageSize).Take(PageSize);
            return Json(new
            {
                IsSuccess = true,
                Data = paging,
                TotalRow = totalRow
            }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Delete
        [HttpPost]
        public JsonResult Delete(Guid id)
        {
            
            if(id == Guid.Empty)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Message = "Id không hợp lệ"
                }, JsonRequestBehavior.AllowGet);
            }
            var group = _context.TaskGroupModel.FirstOrDefault(s => s.GroupId == id);
            if(group == null)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Message = "Nhóm không tồn tại"
                }, JsonRequestBehavior.AllowGet);
            }
            var members = _context.TaskGroupDetailModel.Where(s => s.GroupId == group.GroupId);
            _context.Entry(group).State = EntityState.Deleted;
            foreach(var m in members)
            {
                _context.Entry(m).State = EntityState.Deleted;
            }
            _context.SaveChanges();
            return Json(new
            {
                IsSuccess = true,
                Message = "Xóa nhóm thành công"
            }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Get
        [HttpGet]
        public JsonResult GetById(Guid id)
        {
            var group = _context.TaskGroupModel.Find(id);
            if(group == null)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Message = "Nhóm không tồn tại"
                }, JsonRequestBehavior.AllowGet);
            }
            //GET member
            var members = _context.TaskGroupDetailModel.Where(s => s.GroupId == group.GroupId).ToList();
            //get detail info
            var membersCode = (from member in members
                              join emp in _context.AccountModel on member.AccountId equals emp.AccountId
                              join dep in _context.SalesEmployeeModel.Include(s=>s.DepartmentModel) on emp.EmployeeCode equals dep.SalesEmployeeCode
                              select new SalesEmployeeViewModel {
                                SalesEmployeeCode = emp.EmployeeCode,
                                SalesEmployeeName = dep.SalesEmployeeName,
                                DepartmentName = dep.DepartmentModel?.DepartmentName
                              }).ToList();
            AssignedGroupViewModel view = new AssignedGroupViewModel()
            {
                GroupName = group.GroupName,
                Type = group.GroupType,
                AccountIdList = membersCode.OrderBy(S => S.SalesEmployeeCode).ToList()
            };
            return Json(new
            {
                IsSuccess = true,
                Data = view
            }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        [HttpGet]
        public ActionResult GetAccountInGroup()
        {
           
            List<SalesEmployeeViewModel> taskAssignList = new List<SalesEmployeeViewModel>();
            taskAssignList = (
                              from se in _context.SalesEmployeeModel.Include(s => s.DepartmentModel)
                              join a in _context.AccountModel on se.SalesEmployeeCode equals a.EmployeeCode
                              where se.Actived == true && !se.SalesEmployeeName.Contains("đã nghỉ việc") // không get nv đã nghỉ việc
                              orderby se.SalesEmployeeCode
                              select new SalesEmployeeViewModel()
                              {
                                  SalesEmployeeCode = se.SalesEmployeeCode,
                                  SalesEmployeeName = se.SalesEmployeeName,
                                  DepartmentName = se.DepartmentModel.DepartmentName,
                              }).ToList();
            return PartialView("_SearchEmployee", taskAssignList);
        }
    }
}