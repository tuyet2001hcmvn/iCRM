using ISD.Constant;
using ISD.EntityModels;
using ISD.Extensions;
using ISD.Resources;
using ISD.ViewModels;
using ISD.Core;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Permission.Controllers
{
    public class AccessController : BaseController
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
                PageViewModels = p.PageModel.Where(e => (e.isSystem == false || (e.isSystem == true && isSystemAdmin)) && e.Actived == true).Select(e => new PageViewModel()
                {
                    PageId = e.PageId,
                    PageName = e.PageName,
                    OrderIndex = e.OrderIndex,
                    FunctionViewModels = e.FunctionModel.Select(f => new FunctionViewModel()
                    {
                        FunctionId = f.FunctionId,
                        FunctionName = f.FunctionName,
                        PageId = e.PageId
                    }).OrderBy(f => f.FunctionId == ConstFunction.Access ? 0 : 1).ToList()
                }).OrderBy(e => e.OrderIndex).ToList()
            }).ToList();

            //select 
            if (menuLst != null && menuLst.Count > 0)
            {
                foreach (var menu in menuLst)
                {
                    if (menu.PageViewModels != null && menu.PageViewModels.Count > 0)
                    {
                        foreach (var page in menu.PageViewModels)
                        {
                            if (page.FunctionViewModels != null && page.FunctionViewModels.Count > 0)
                            {
                                foreach (var function in page.FunctionViewModels)
                                {
                                    var fp = _context.PagePermissionModel.FirstOrDefault(p => p.PageId == page.PageId && p.FunctionId == function.FunctionId && p.RolesId == RolesId);
                                    if (fp != null)
                                    {
                                        function.Selected = true;
                                    }
                                }

                                //if all functions in page are checked => select all checkbox set checked
                                var functions = page.FunctionViewModels.Where(p => p.Selected == true).ToList();
                                if (functions.Count == page.FunctionViewModels.Count)
                                {
                                    page.isChooseAll = true;
                                }
                            }
                        }

                        //if all pages in menu are checked => select all checkbox set checked
                        var pages = menu.PageViewModels.Where(p => p.isChooseAll == true).ToList();
                        if (pages.Count == menu.PageViewModels.Count)
                        {
                            menu.isChooseAll = true;
                        }
                    }
                }
            }

            return PartialView(menuLst);
        }
        #endregion

        #region Edit permission
        [HttpPost]
        public ActionResult EditPermission(Guid RolesId, Guid PageId, string FunctionId, bool Check)
        {
            return ExecuteContainer(() =>
            {
                //Checkbox has value = TRUYCAP is required before check remaining functions
                var allFunc = _context.PagePermissionModel.Where(p => p.RolesId == RolesId && p.PageId == PageId).ToList();
                if (FunctionId != ConstFunction.Access)
                {
                    var accessFunc = allFunc.Where(p => p.FunctionId == ConstFunction.Access).FirstOrDefault();
                    if (accessFunc == null)
                    {
                        return Json(new
                        {
                            Code = System.Net.HttpStatusCode.Created,
                            Success = false,
                            Data = string.Format(LanguageResource.Alert_Access)
                        });
                    }
                }
                //1. Check = true => Thêm
                //2. Check = false => Xóa
                var permission = _context.PagePermissionModel.Where(p => p.RolesId == RolesId && p.PageId == PageId && p.FunctionId == FunctionId).FirstOrDefault();
                if (permission == null && Check == true)
                {
                    PagePermissionModel perModel = new PagePermissionModel();
                    perModel.RolesId = RolesId;
                    perModel.PageId = PageId;
                    perModel.FunctionId = FunctionId;
                    _context.Entry(perModel).State = EntityState.Added;
                }
                else
                {
                    if (FunctionId == ConstFunction.Access)
                    {
                        if (allFunc != null && allFunc.Count > 0)
                        {
                            _context.PagePermissionModel.RemoveRange(allFunc);
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

        #region Choose all the functions in page
        [HttpPost]
        public ActionResult UpdateFuncInPage(Guid RolesId, Guid PageId)
        {
            return ExecuteContainer(() =>
            {
                //select all functions in page by role
                var permission = (from p in _context.PagePermissionModel
                                  where p.RolesId == RolesId
                                  && p.PageId == PageId
                                  select p.FunctionId);

                //select function need to add
                var PageFunctionLst = (from p in _context.PageModel
                                       from f in p.FunctionModel
                                       where p.PageId == PageId
                                       && !permission.Contains(f.FunctionId)
                                       group new { p, f } by new { p.PageId, f.FunctionId } into g
                                       select new { g.Key.PageId, g.Key.FunctionId }).ToList();

                if (PageFunctionLst != null && PageFunctionLst.Count > 0)
                {
                    foreach (var item in PageFunctionLst)
                    {
                        PagePermissionModel model = new PagePermissionModel()
                        {
                            RolesId = RolesId,
                            PageId = item.PageId,
                            FunctionId = item.FunctionId
                        };
                        _context.Entry(model).State = EntityState.Added;
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

        public ActionResult DeleteFuncInPage(Guid RolesId, Guid PageId)
        {
            return ExecuteContainer(() =>
            {
                var pages = (from p in _context.PagePermissionModel
                             where p.RolesId == RolesId
                             && p.PageId == PageId
                             select p).ToList();
                _context.PagePermissionModel.RemoveRange(pages);
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

        #region Choose all the functions in menu
        [HttpPost]
        public ActionResult UpdateFuncInMenu(Guid RolesId, Guid MenuId)
        {
            return ExecuteContainer(() =>
            {
                var permission = (from p in _context.PagePermissionModel
                                  join page in _context.PageModel on p.PageId equals page.PageId
                                  where p.RolesId == RolesId
                                  && page.MenuId == MenuId
                                  group p by new { p.FunctionId, p.PageId } into g
                                  select new PagePermissionViewModel
                                  {
                                      PageId = g.Key.PageId,
                                      FunctionId = g.Key.FunctionId
                                  });

                var PageFunctionLst = (from p in _context.PageModel
                                       from f in p.FunctionModel
                                       where p.MenuId == MenuId
                                       group new { p, f } by new { p.PageId, f.FunctionId } into g
                                       select new PagePermissionViewModel
                                       {
                                           PageId = g.Key.PageId,
                                           FunctionId = g.Key.FunctionId
                                       });

                var addList = (from p in PageFunctionLst
                               where !permission.Contains(p)
                               select p).ToList();

                if (addList != null && addList.Count > 0)
                {
                    foreach (var item in addList)
                    {
                        PagePermissionModel model = new PagePermissionModel()
                        {
                            RolesId = RolesId,
                            PageId = item.PageId,
                            FunctionId = item.FunctionId
                        };
                        _context.Entry(model).State = EntityState.Added;
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

        public ActionResult DeleteFuncInMenu(Guid RolesId, Guid MenuId)
        {
            return ExecuteContainer(() =>
            {
                var pages = _context.PageModel.Where(p => p.MenuId == MenuId).Select(p => p.PageId).ToList();
                var functions = (from p in _context.PagePermissionModel
                                 where p.RolesId == RolesId
                                 && pages.Contains(p.PageId)
                                 select p).ToList();

                _context.PagePermissionModel.RemoveRange(functions);
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