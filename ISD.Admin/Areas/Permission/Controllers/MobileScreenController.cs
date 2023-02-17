using ISD.Constant;
using ISD.EntityModels;
using ISD.Extensions;
using ISD.Resources;
using ISD.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using ISD.Core;

namespace Permission.Controllers
{
    public class MobileScreenController : BaseController
    {
        //GET: /Page/Index
        #region Index, _Search
        [ISDAuthorizationAttribute]
        public ActionResult Index()
        {
            CreateViewBag();
            return View();
        }
        public ActionResult _Search(string ScreenName, bool? Actived, Guid? MenuId)
        {
            return ExecuteSearch(() =>
            {
                var ScreenNameIsNullOrEmpty = string.IsNullOrEmpty(ScreenName);
                //var pageList = _context.MobileScreenModel.Where(p => (ScreenNameIsNullOrEmpty || p.ScreenName.ToLower().Contains(ScreenName.ToLower()))
                //                                                && (Actived == null || p.Actived == Actived)
                //                                                && (MenuId == null || p.MenuId == MenuId))
                //                                  .OrderBy(p => p.OrderIndex)
                //                                  .ToList();
                var pageList = _context.Database.SqlQuery<MobileScreenModel>(@"usp_SearchMobileScreen @SearchText, @Actived",
                                     new SqlParameter("@SearchText", ScreenName),
                                     new SqlParameter("@Actived", Actived ?? (object)DBNull.Value))
                                .OrderBy(p => p.OrderIndex)
                               .ToList();

                return PartialView(pageList);
            });
        }
        #endregion

        //GET: /Page/Create
        #region Create
        [ISDAuthorizationAttribute]
        public ActionResult Create()
        {
            ScreenFunctionViewModel page = new ScreenFunctionViewModel()
            {
                Actived = true,
                isCreated = false,
                isReporter = false,
                isAssignee = false,
                FunctionList = funcWithOrderBy(),
                ActivedFunctionList = funcWithOrderBy().Where(p => p.FunctionId == ConstFunction.Access).ToList()
            };
            CreateViewBag();
            return View(page);
        }

        //POST: Create
        [HttpPost]
        [ValidateAjax]
        [ISDAuthorizationAttribute]
        public JsonResult Create(MobileScreenModel model, List<string> FunctionId)
        {
            return ExecuteContainer(() =>
            {
                //Save data in MobileScreenModel
                model.MobileScreenId = Guid.NewGuid();
                model.Actived = true;
                model.Visible = true;
                model.isSystem = false;
                //Save data in PageFunctionModel
                if (FunctionId != null)
                {
                    ManyToMany(model, FunctionId);
                }
                //Save
                _context.Entry(model).State = EntityState.Added;
                _context.SaveChanges();
                return Json(new
                {
                    Code = System.Net.HttpStatusCode.Created,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Create_Success, LanguageResource.Permission_MobileScreen.ToLower())
                });
            });
        }
        #endregion

        //GET: /Page/Edit
        #region Edit
        [ISDAuthorizationAttribute]
        public ActionResult Edit(Guid id)
        {
            var mobilePrefix = _context.ApplicationConfig.SingleOrDefault(a => a.ConfigKey == "MobilePrefix");
            
            var page = (from p in _context.MobileScreenModel.AsEnumerable()
                        where p.MobileScreenId == id
                        select new ScreenFunctionViewModel()
                        {
                            MobileScreenId = p.MobileScreenId,
                            ScreenName = p.ScreenName,
                            ScreenCode = p.ScreenCode,
                            MenuId = p.MenuId,
                            OrderIndex = p.OrderIndex,
                            Icon = p.Icon,
                            IconType = p.IconType,
                            IconName = p.IconName,
                            Visible = p.Visible,
                            Actived = p.Actived,
                            isCreated = p.isCreated,
                            isReporter = p.isReporter,
                            isAssignee = p.isAssignee,
                            FunctionList = funcWithOrderBy(),
                            ActivedFunctionList = p.FunctionModel
                                                   .Where(f => f.FunctionName.Contains(mobilePrefix.ConfigValue ?? "M_")).ToList()
                        }).FirstOrDefault();
            CreateViewBag(page.MenuId);
            return View(page);
        }
        //POST: Edit
        [HttpPost]
        [ISDAuthorizationAttribute]
        public JsonResult Edit(MobileScreenModel model, List<string> FunctionId)
        {
            return ExecuteContainer(() =>
            {
                var page = _context.MobileScreenModel.Where(p => p.MobileScreenId == model.MobileScreenId)
                                                   .Include(p => p.FunctionModel).FirstOrDefault();
                if (page != null)
                {
                    //master page
                    page.ScreenName = model.ScreenName;
                    page.ScreenCode = model.ScreenCode;
                    page.MenuId = model.MenuId;
                    page.OrderIndex = model.OrderIndex;
                    page.Icon = model.Icon;

                    page.IconType = model.IconType;
                    page.IconName = model.IconName;

                    page.Visible = model.Visible;
                    page.Actived = model.Actived;
                    page.isCreated = model.isCreated;
                    page.isReporter = model.isReporter;
                    page.isAssignee = model.isAssignee;
                    //detail function
                    if (FunctionId != null)
                    {
                        ManyToMany(page, FunctionId);
                    }
                    _context.Entry(page).State = EntityState.Modified;
                    _context.SaveChanges();
                }

                return Json(new
                {
                    Code = System.Net.HttpStatusCode.Created,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Edit_Success, LanguageResource.Permission_MobileScreen.ToLower())
                });
            });
        }
        #endregion

        #region Delete
        [HttpPost]
        [ISDAuthorizationAttribute]
        public ActionResult Delete(Guid id)
        {
            return ExecuteDelete(() =>
            {
                var page = _context.MobileScreenModel.FirstOrDefault(p => p.MobileScreenId == id);
                if (page != null)
                {
                    if (page.FunctionModel != null)
                    {
                        page.FunctionModel.Clear();
                    }
                    _context.Entry(page).State = EntityState.Deleted;

                    //Delete in PagePermission
                    var pagePermission = _context.MobileScreenPermissionModel.Where(p => p.MobileScreenId == id).ToList();
                    if (pagePermission != null && pagePermission.Count > 0)
                    {
                        _context.MobileScreenPermissionModel.RemoveRange(pagePermission);
                    }

                    _context.SaveChanges();

                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.Created,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Delete_Success, LanguageResource.Permission_MobileScreen.ToLower())
                    });
                }
                else
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotModified,
                        Success = false,
                        Data = string.Format(LanguageResource.Alert_NotExist_Delete, LanguageResource.Permission_MobileScreen.ToLower())
                    });
                }
            });
        }
        #endregion

        #region Helper
        private void CreateViewBag(Guid? MenuId = null, Guid? FunctionId = null)
        {
            // MenuId
            var MenuList = _context.MenuModel.OrderBy(p => p.OrderIndex).ToList();
            ViewBag.MenuId = new SelectList(MenuList, "MenuId", "MenuName", MenuId);

            // FunctionId
            var FunctionList = _context.FunctionModel.OrderBy(p => p.FunctionId).ToList();
            ViewBag.FunctionId = new SelectList(FunctionList, "FunctionId", "FunctionName", FunctionId);
        }

        private void ManyToMany(MobileScreenModel model, List<string> funcList)
        {
            if (model.FunctionModel != null)
            {
                model.FunctionModel.Clear();
            }
            if (funcList != null && funcList.Count > 0)
            {
                foreach (var item in funcList)
                {
                    var itemAdd = _context.FunctionModel.Find(item);
                    model.FunctionModel.Add(itemAdd);
                }
            }
        }

        public List<FunctionModel> funcWithOrderBy()
        {
            var mobilePrefix = _context.ApplicationConfig.SingleOrDefault(a => a.ConfigKey == "MobilePrefix");

            //get all function
            var funcList = _context.FunctionModel
                                .Where(f => f.FunctionId.Contains(mobilePrefix.ConfigValue ?? "M_"))
                                .OrderBy(p => p.FunctionId == ConstFunction.Access ? 0 : 1)
                                .ThenBy(p => p.FunctionId)
                                .ToList();
            return funcList;
        }
        #endregion
    }
}