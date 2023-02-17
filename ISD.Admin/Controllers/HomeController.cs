using System.Web.Mvc;
using ISD.Core;
using System.Linq;
using System;
using ISD.ViewModels;
using ISD.Repositories;
using ISD.Constant;
using ISD.ViewModels.MasterData;
using System.Collections.Generic;

namespace ISD.Admin.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult ChangeLanguage(string lang, string returnUrl)
        {
            new MultiLanguage().SetLanguage(lang);
            //new MultiLanguage().SetLanguage("en");
            if (string.IsNullOrEmpty(returnUrl))
            {
                returnUrl = "/";
            }
            return Redirect(returnUrl);
        }

        public ActionResult Index()
        {
            var _taskRepository = new TaskRepository(_context);
            //Reset selected_module
            if (Request.Cookies["selected_module"] != null)
            {
                Response.Cookies["selected_module"].Expires = DateTime.Now.AddDays(-1);
            }
            ViewBag.SelectedModuleId = new Guid();
            ViewBag.SelectedModuleName = Resources.LanguageResource.Btn_Choose;


            if (CurrentUser.isShowChoseModule)
            {
                PermissionViewModel lst = (PermissionViewModel)Session["Menu"];
                //Hiển thị lựa chọn module
                var moduleList = lst.ModuleModel;
                ViewBag.isShowChoseModule = CurrentUser.isShowChoseModule;
                ViewBag.ModuleList = moduleList
                                        .Where(p => p.isSystemModule == false)
                                        .OrderBy(p => p.OrderIndex)
                                        .Select(p => new ModuleViewModel()
                                        {
                                            ModuleId = p.ModuleId,
                                            ModuleName = p.ModuleName,
                                            ImageUrl = p.ImageUrl,
                                            Description = p.Description
                                        }).ToList();
            }
            if (CurrentUser.isShowDashboard)
            {
                //Hiển thị dashboard thống kê
                ViewBag.isShowDashBoard = CurrentUser.isShowDashboard;
                //1. Tổng số lượng khách hàng
                var customerCount = _context.ProfileModel.Where(p => p.CustomerTypeCode == ConstProfileType.Account && p.Actived == true).Count();
                ViewBag.Customer = string.Format("{0:n0}", customerCount);
                //2. Tổng số lượng liên hệ
                var contactCount = _context.ProfileModel.Where(p => p.CustomerTypeCode == ConstProfileType.Contact && p.Actived == true).Count();
                ViewBag.Contact = string.Format("{0:n0}", contactCount);
                //3. Tổng công việc
                var taskCount = _taskRepository.GetCountOfTask(ConstTaskStatus.All, null);
                ViewBag.Task = string.Format("{0:n0}", taskCount);

                #region Task: Công việc
                //1. Việc cần làm
                //Là việc có status với ProcessCode (Giai đoạn) = Todo
                ViewBag.Todo = _taskRepository.GetCountOfTask(ConstTaskStatus.Todo, CurrentUser.EmployeeCode);
                //2. Đang thực hiện
                //Là việc có TaskModel.Reporter = CurrentUserId
                //ViewBag.Follow = _taskRepository.GetCountOfTask(ConstTaskStatus.Follow, CurrentUser.EmployeeCode);
                ViewBag.Incomplete = _taskRepository.GetCountOfTask(ConstTaskStatus.Processing, CurrentUser.EmployeeCode);
                //3. Việc đã hoàn thành
                //Là việc có TaskModel.Reporter = CurrentUserId
                //Là việc có TaskAssignModel.SalesEmployeeCode = CurrentUserCode
                ViewBag.Completed = _taskRepository.GetCountOfTask(ConstTaskStatus.Completed, CurrentUser.EmployeeCode);
                //4. Quá hạn
                //Là việc có status với ProcessCode (Giai đoạn) <> Completed và chưa được phân cho ai
                //ViewBag.Unassign = _taskRepository.GetCountOfTask(ConstTaskStatus.Unassign, CurrentUser.EmployeeCode);
                ViewBag.Expired = _taskRepository.GetCountOfTask(ConstTaskStatus.Expired, CurrentUser.EmployeeCode);
                #endregion
            }

            //Thông tin cập nhật
            var TotalTask = 0;
            var taskList = _taskRepository.Get4TaskRecently(out TotalTask);
            ViewBag.TotalTask = string.Format("{0:n0}", TotalTask);

            //Bảng tin
            //var newsCategory = _context.NewsCategoryModel.FirstOrDefault(p => p.OrderIndex == 1);
            //ViewBag.News = _context.NewsModel.Where(p => p.NewsCategoryId == newsCategory.NewsCategoryId && p.CreateBy == CurrentUser.AccountId).ToList();
            var ret = new List<NewsViewModel>();
            ret = (from n in _context.NewsModel
                       //join m in _context.News_Company_Mapping on n.NewsId equals m.NewsId
                       //join c in _context.CompanyModel on m.CompanyId equals c.CompanyId
                   join nc in _context.NewsCategoryModel on n.NewsCategoryId equals nc.NewsCategoryId
                   join empg in _context.TaskGroupDetailModel on n.GroupEmployeeId equals empg.GroupId into tgTemp
                   from tg in tgTemp.DefaultIfEmpty()
                   join cr in _context.AccountModel on n.CreateBy equals cr.AccountId
                   join empTemp in _context.SalesEmployeeModel on cr.EmployeeCode equals empTemp.SalesEmployeeCode into empList
                   from emp in empList.DefaultIfEmpty()
                   join sTemp in _context.CatalogModel on new { CatalogCode = n.Summary, CatalogTypeCode = ConstCatalogType.BANGTIN_SUMMARY } equals new { sTemp.CatalogCode, sTemp.CatalogTypeCode } into sList
                   from s in sList.DefaultIfEmpty()
                   join dTemp in _context.CatalogModel on new { CatalogCode = n.Detail, CatalogTypeCode = ConstCatalogType.BANGTIN_DETAIL } equals new { dTemp.CatalogCode, dTemp.CatalogTypeCode } into dList
                   from d in dList.DefaultIfEmpty()
                   join tTemp in _context.CatalogModel on new { CatalogCode = n.TypeNews, CatalogTypeCode = ConstCatalogType.News_Type } equals new { tTemp.CatalogCode, tTemp.CatalogTypeCode } into tList
                   from t in tList.DefaultIfEmpty()
                   where //c.CompanyCode == CompanyCode
                    nc.NewsCategoryCode == ConstNewsCategoryCode.BangTin
                   && n.isShowOnWeb == true
                   && ((tg != null && tg.AccountId == CurrentUser.AccountId) || n.CreateBy == CurrentUser.AccountId)
                   && (n.EndTime == null || DateTime.Now >= n.EndTime)
                   select new NewsViewModel()
                   {
                       NewsId = n.NewsId,
                       Title = n.Title,
                       CreateTime = n.ScheduleTime != null ? n.ScheduleTime : n.CreateTime,
                       //Nếu tin tức không có hình ảnh thì lấy ảnh của loại tin tức
                       ImageUrl = (n.ImageUrl != null && n.ImageUrl != ConstImageUrl.noImage) ? ConstDomain.Domain + "/Upload/News/" + n.ImageUrl : ConstDomain.Domain + "/Upload/NewsCategory/" + nc.ImageUrl,
                       SummaryName = s.CatalogText_vi,
                       Summary = s.CatalogCode,
                       DetailName = d.CatalogText_vi,
                       Detail = d.CatalogCode,
                       TypeNews = t.CatalogText_vi,
                       CreateByName = emp.SalesEmployeeName,
                       ScheduleTime = n.ScheduleTime,
                   }).ToList();
            ViewBag.News = ret.GroupBy(g => new { g.NewsId, g.Title, g.CreateTime,  g.ImageUrl, g.SummaryName, g.Summary, g.DetailName, g.Detail, g.TypeNews, g.CreateByName, g.ScheduleTime })
                        .Select(p => new NewsViewModel()
                        {
                            NewsId = p.Key.NewsId,
                            Title = p.Key.Title,
                            CreateTime = p.Key.CreateTime,
                            ImageUrl = p.Key.ImageUrl,
                            SummaryName = p.Key.SummaryName,
                            Summary = p.Key.Summary,
                            DetailName = p.Key.DetailName,
                            Detail = p.Key.Detail,
                            TypeNews = p.Key.TypeNews,
                            CreateByName = p.Key.CreateByName,
                            ScheduleTime = p.Key.ScheduleTime,
                        }).ToList();
            //if (newsCategory != null)
            //{
            //    ViewBag.ImageUrlNewsCategory = newsCategory.ImageUrl;
            //}
            //else
            //{
            //    NewsCategoryModel NewsCategory = new NewsCategoryModel();
            //    ViewBag.ImageUrlNewsCategory = NewsCategory.ImageUrl;
            //}
            CreateViewBag();

            return View(taskList);
        }
        public ActionResult _Reload(string SelectedCommonDate = null)
        {
         

            DateTime? fromDate;
            DateTime? toDate;
            DateTime? fromPreviousDay;
            DateTime? toPreviousDay;

            _unitOfWork.CommonDateRepository.GetDateBy(SelectedCommonDate, out fromDate, out toDate, out fromPreviousDay, out toPreviousDay);
            #region Ghé thăm
            var appointmentCount = (from t in _context.AppointmentModel
                                    join ts in _context.TaskModel on t.AppointmentId equals ts.TaskId
                                    join p in _context.ProfileModel on ts.ProfileId equals p.ProfileId
                                    join s in _context.StoreModel on ts.StoreId equals s.StoreId
                                    where t.VisitDate >= fromDate && t.VisitDate <= toDate && (ts.isDeleted == null || ts.isDeleted == false)
                                    select t).Count();
            var appointmentPreviousCount = (from t in _context.AppointmentModel
                                            join ts in _context.TaskModel on t.AppointmentId equals ts.TaskId
                                            join p in _context.ProfileModel on ts.ProfileId equals p.ProfileId
                                            join s in _context.StoreModel on ts.StoreId equals s.StoreId
                                            where t.VisitDate >= fromPreviousDay && t.VisitDate <= toPreviousDay && (ts.isDeleted == null || ts.isDeleted == false)
                                   select t).Count();
            ViewBag.AppointmentCount = string.Format("{0:n0}", appointmentCount);
            ViewBag.AppointmentPreviousCount = string.Format("{0:n0}", appointmentPreviousCount);
            #endregion

            #region Thăm hỏi khách hàng
            var THKHCount = (from t in _context.TaskModel
                             join w in _context.WorkFlowModel on t.WorkFlowId equals w.WorkFlowId
                             join s in _context.TaskStatusModel on t.TaskStatusId equals s.TaskStatusId
                             where w.WorkflowCategoryCode == "THKH" &&
                                    t.StartDate >= fromDate && 
                                    t.StartDate <= toDate &&
                                    s.ProcessCode == "completed" && 
                                    (t.isDeleted == null || t.isDeleted == false)
                             select t).Count();
            var THKHUnsuccessCount = (from t in _context.TaskModel
                                      join w in _context.WorkFlowModel on t.WorkFlowId equals w.WorkFlowId
                                      join s in _context.TaskStatusModel on t.TaskStatusId equals s.TaskStatusId
                                      where w.WorkflowCategoryCode == "THKH" &&
                                      t.StartDate >= fromDate &&
                                      t.StartDate <= toDate &&
                                      s.ProcessCode != "completed" &&
                                    (t.isDeleted == null || t.isDeleted == false)
                                      select t).Count();
            ViewBag.THKHCount = string.Format("{0:n0}", THKHCount);
            ViewBag.THKHUnsuccessCount = string.Format("{0:n0}", THKHUnsuccessCount);
            #endregion

            #region Nhiệm vụ khác
            var activitiesCount = (from t in _context.TaskModel
                             join w in _context.WorkFlowModel on t.WorkFlowId equals w.WorkFlowId
                             join s in _context.TaskStatusModel on t.TaskStatusId equals s.TaskStatusId
                             where w.WorkflowCategoryCode == "ACTIVITIES" &&
                                    t.StartDate >= fromDate &&
                                    t.StartDate <= toDate &&
                                    s.ProcessCode == "completed" &&
                                    (t.isDeleted == null || t.isDeleted == false)
                             select t).Count();
            var activitiesUnsuccessCount = (from t in _context.TaskModel
                                      join w in _context.WorkFlowModel on t.WorkFlowId equals w.WorkFlowId
                                      join s in _context.TaskStatusModel on t.TaskStatusId equals s.TaskStatusId
                                      where w.WorkflowCategoryCode == "ACTIVITIES" &&
                                      t.StartDate >= fromDate &&
                                      t.StartDate <= toDate &&
                                      s.ProcessCode != "completed" &&
                                    (t.isDeleted == null || t.isDeleted == false)
                                      select t).Count();
            ViewBag.ActivitiesCount = string.Format("{0:n0}", activitiesCount);
            ViewBag.ActivitiesUnsuccessCount = string.Format("{0:n0}", activitiesUnsuccessCount);
            #endregion

            CreateViewBag(SelectedCommonDate);
            return PartialView();
        }

        public ActionResult App()
        {
            return Redirect("itms-services://?action=download-manifest&amp;url=https://giahoamobile.citek.vn/app.plist");
        }

        public void CreateViewBag(string SelectedCommonDate = null)
        {
            //CommonDate
            if (SelectedCommonDate == null)
            {
                SelectedCommonDate = "ThisMonth";
            }

            var commonDateList = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.CommonDate2);
            ViewBag.CommonDate = new SelectList(commonDateList, "CatalogCode", "CatalogText_vi", SelectedCommonDate);

        }
    }
}