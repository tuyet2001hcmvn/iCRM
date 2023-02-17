using ISD.Constant;
using ISD.EntityModels;
using ISD.Extensions;
using ISD.Resources;
using ISD.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using ISD.Core;

namespace Permission.Controllers
{
    public class AccessMobileController : BaseController
    {
        // GET: Access/Index
        #region Index
        [ISDAuthorizationAttribute]
        public ActionResult Index()
        {
            //get all roles
            var roleList = _context.RolesModel.Where(p => p.Actived == true).OrderBy(p => p.OrderIndex).ToList();
            //Get Account by AccountId
            var accountId = CurrentUser.AccountId;
            var accountFilter = _context.AccountModel.Where(p => p.AccountId == accountId).FirstOrDefault();
            if (accountFilter.RolesModel != null && accountFilter.RolesModel.Count > 0)
            {
                var filterRoles = roleList.Where(p => p.OrderIndex >= accountFilter.RolesModel.Min(e => e.OrderIndex)).OrderBy(p => p.OrderIndex).ToList();
                roleList = filterRoles;
            }
            ViewBag.RolesId = new SelectList(roleList, "RolesId", "RolesName", null);

            return View();
        }

        public ActionResult _AccessFormPartial(Guid? RolesId)
        {
            bool isSystemAdmin = false;
            //Get Roles by AccountId
            var accountId = CurrentUser.AccountId;
            var roles = _context.AccountModel.Where(p => p.AccountId == accountId).FirstOrDefault();
            if (roles.RolesModel != null && roles.RolesModel.Count > 0)
            {
                foreach (var role in roles.RolesModel)
                {
                    if (role.OrderIndex == ConstRoles.isSysAdmin)
                    {
                        isSystemAdmin = true;
                    }
                }
            }
            List<MenuViewModel> menuLst = new List<MenuViewModel>();
            menuLst = _context.MenuModel.Select(p => new MenuViewModel()
            {
                Icon = p.Icon,
                MenuId = p.MenuId,
                MenuName = p.MenuName,
                OrderIndex = p.OrderIndex,
                MobileScreenViewModels = p.MobileScreenModel.Where(e => (e.isSystem == false || (e.isSystem == true && isSystemAdmin)) && e.Actived == true)
                        .Select(e => new MobileScreenViewModel()
                        {
                            MobileScreenId = e.MobileScreenId,
                            ScreenName = e.ScreenName,
                            OrderIndex = e.OrderIndex,
                            FunctionViewModels = e.FunctionModel.Select(f => new FunctionViewModel()
                            {
                                FunctionId = f.FunctionId,
                                FunctionName = f.FunctionName,
                                OrderIndex = f.OrderIndex,
                                MobileScreenId = e.MobileScreenId
                            }).OrderBy(f => f.FunctionId == ConstFunction.Access ? 0 : 1).ThenBy(f => f.OrderIndex).ToList()
                        }).OrderBy(e => e.OrderIndex).ToList()
            }).ToList();

            //select 
            if (menuLst != null && menuLst.Count > 0)
            {
                foreach (var menu in menuLst)
                {
                    if (menu.MobileScreenViewModels != null && menu.MobileScreenViewModels.Count > 0)
                    {
                        foreach (var page in menu.MobileScreenViewModels)
                        {
                            if (page.FunctionViewModels != null && page.FunctionViewModels.Count > 0)
                            {
                                foreach (var function in page.FunctionViewModels)
                                {
                                    var fp = _context.MobileScreenPermissionModel.FirstOrDefault(p => p.MobileScreenId == page.MobileScreenId && p.FunctionId == function.FunctionId && p.RolesId == RolesId);
                                    if (fp != null)
                                    {
                                        function.Selected = true;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return PartialView(menuLst);
        }
        #endregion

        #region Edit permission
        [HttpPost]
        public ActionResult EditPermission(Guid RolesId, Guid MobileScreenId, string FunctionId, bool Check)
        {
            return ExecuteContainer(() =>
            {
                //Checkbox has value = TRUYCAP is required before check remaining functions
                var allFunc = _context.MobileScreenPermissionModel.Where(p => p.RolesId == RolesId && p.MobileScreenId == MobileScreenId).ToList();
                //if (FunctionId != ConstFunction.Access)
                //{
                //    var accessFunc = allFunc.Where(p => p.FunctionId == ConstFunction.Access).FirstOrDefault();
                //    if (accessFunc == null)
                //    {
                //        return Json(new
                //        {
                //            Code = System.Net.HttpStatusCode.Created,
                //            Success = false,
                //            Data = string.Format(LanguageResource.Alert_Access)
                //        });
                //    }
                //}
                //1. Check = true => Thêm
                //2. Check = false => Xóa
                var permission = _context.MobileScreenPermissionModel.Where(p => p.RolesId == RolesId && p.MobileScreenId == MobileScreenId && p.FunctionId == FunctionId).FirstOrDefault();
                if (permission == null && Check == true)
                {
                    MobileScreenPermissionModel perModel = new MobileScreenPermissionModel();
                    perModel.RolesId = RolesId;
                    perModel.MobileScreenId = MobileScreenId;
                    perModel.FunctionId = FunctionId;
                    _context.Entry(perModel).State = EntityState.Added;
                }
                else
                {
                    if (FunctionId == ConstFunction.Access)
                    {
                        if (allFunc != null && allFunc.Count > 0)
                        {
                            _context.MobileScreenPermissionModel.RemoveRange(allFunc);
                        }
                    }
                    else
                    {
                        _context.Entry(permission).State = EntityState.Deleted;
                    }
                }
                _context.SaveChanges();
                return Json(new
                {
                    Code = System.Net.HttpStatusCode.Created,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Edit_Success, LanguageResource.Permission.ToLower())
                });
            });
        }
        #endregion
    }
}