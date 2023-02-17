using ISD.Core;
using ISD.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ISD.Constant;
using System.Data.Entity;
using System.Web;
using ISD.Repositories;

namespace ISD.Admin.Controllers
{
    public class SharedController : BaseController
    {
        // GET: Shared
        public ActionResult _Sidebar(Guid? SelectedModuleId = null)
        {
            PermissionViewModel lst = (PermissionViewModel)Session["Menu"];
            var ret = new PermissionViewModel();
            //All Module Load by Cookies
            if (SelectedModuleId == null)
            {
                HttpCookie selectedModule = Request.Cookies["selected_module"];
                if (selectedModule != null)
                {
                    SelectedModuleId = new Guid(selectedModule["ModuleId"].ToString());
                }
            }
            //Home Page load all
            else if (SelectedModuleId == new Guid())
            {
                SelectedModuleId = null;
            }
            
            //Load Menu And Page
            if (SelectedModuleId != null)
            {
                //Get By Module
                //Get Page By Module
                ret.PageModel = lst.PageModel.Where(p => p.ModuleId == SelectedModuleId.Value).ToList();
                //Get menu by Page
                ret.MenuModel = (from m in lst.MenuModel
                                 join p in ret.PageModel on m.MenuId equals p.MenuId
                                 select m).Distinct().ToList();
            }
            else
            {
                //Get All
                ret.MenuModel = lst.MenuModel;
                ret.PageModel = lst.PageModel.Select(p => new PageViewModel() {
                     PageId = p.PageId,
                     MenuId = p.MenuId,
                     PageName = p.PageName,
                     PageUrl = p.PageUrl,
                     //OrderIndex = p.OrderIndex,
                     Parameter = p.Parameter
                }).ToList();
            }
            return PartialView("./Partials/_Sidebar", ret);
        }
        public ActionResult _Module(string SelectedModuleName = "")
        {
            ViewBag.SelectedItemText = Resources.LanguageResource.Btn_Choose;
            //Get text from /Module/SelectedModule
            if (!string.IsNullOrEmpty(SelectedModuleName))
            {
                ViewBag.SelectedItemText = SelectedModuleName;
            }
            else
            {
                //Get text from HttpCookie
                HttpCookie selectedModule = Request.Cookies["selected_module"];
                if (selectedModule != null)
                {
                    ViewBag.SelectedItemText = selectedModule["ModuleName"].ToString();
                }
            }
            //Danh sách các module
            //var moduleList = _context.ModuleModel.OrderBy(p => p.OrderIndex).ToList();
            var moduleList = new List<ModuleViewModel>();
            if (Session["Menu"] != null)
            {
                moduleList = ((PermissionViewModel)Session["Menu"]).ModuleModel;
            }

            return PartialView("./Partials/_Module", moduleList);
        }
        #region Notification
        public ActionResult _NotificationMenu()
        {
            List<GH_NotificationViewModel> lst = new List<GH_NotificationViewModel>();
            //Có notify select => insert vào lst
            ViewBag.Total = lst.Count;

            return PartialView("./Partials/_NotificationMenu", lst);
        }

        //Trang chi tiết tất cả các thông báo
        public ActionResult NotificationDetail()
        {
            List<GH_NotificationViewModel> lst = new List<GH_NotificationViewModel>();
            //Có notify select => insert vào lst
            return View("../Home/_NotificationDetail", lst);
        }

        //Check thông báo đã đọc => Không hiển thị trên notif nữa
        public void CheckIsComplete(List<Guid> ItemIsComplete)
        {
            if (ItemIsComplete != null && ItemIsComplete.Count > 0)
            {
                foreach (var item in ItemIsComplete)
                {
                    var model = _context.GH_NotificationModel.FirstOrDefault(p => p.NotificationId == item);
                    if (model != null)
                    {
                        model.isComplete = true;
                        _context.Entry(model).State = EntityState.Modified;
                    }
                }
                _context.SaveChanges();
            }
        }
        #endregion

        public ActionResult _StoreModalPartial()
        {
            var _storeRepository = new StoreRepository(_context);
            var storeList = _storeRepository.GetStoreByPermission(CurrentUser.AccountId);

            ViewBag.CurrentSaleOrg = new SelectList(storeList, "SaleOrgCode", "StoreName", CurrentUser.SaleOrg);
            return PartialView("./Partials/_StoreModalPartial");
        }
        public ActionResult _LogChange(LogChangeViewModel model)
        {
            //Gọi lên database lấy user tạo và cập nhật 1 câu query
            var accountList = _context.AccountModel
                                      .Where(p => p.AccountId == model.CreateBy || p.AccountId == model.LastEditBy)
                                      .Select(p => new { p.AccountId, p.UserName }).ToList();
            //User tạo
            model.CreateName = accountList.Where(p => p.AccountId == model.CreateBy).Select(p => p.UserName).FirstOrDefault();
            //User cập nhật lần cuối
            model.LastEditName = accountList.Where(p => p.AccountId == model.LastEditBy).Select(p => p.UserName).FirstOrDefault();
            //User extend dữ liệu khách hàng
            model.ExtendCreateName = accountList.Where(p => p.AccountId == model.ExtendCreateBy).Select(p => p.UserName).FirstOrDefault();
            return PartialView(model);
        }
    }
}