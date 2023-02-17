using ISD.EntityModels;
using ISD.Extensions;
using ISD.Resources;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web.Mvc;
using ISD.Core;

namespace Permission.Controllers
{
    public class FunctionController : BaseController
    {
        //GET: /Function/Index
        #region Index, _Search
        [ISDAuthorizationAttribute]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult _Search(string FunctionName)
        {
            return ExecuteSearch(() =>
            {
                var FunctionNameIsNullOrEmpty = string.IsNullOrEmpty(FunctionName);
                var funcList = _context.FunctionModel.Where(p => FunctionNameIsNullOrEmpty || p.FunctionName.ToLower().Contains(FunctionName.ToLower()))
                                                  .ToList();
                return PartialView(funcList);
            });
        }
        #endregion

        //GET: /Function/Create
        #region Create
        [ISDAuthorizationAttribute]
        public ActionResult Create()
        {
            return View();
        }
        //POST: Create
        [HttpPost]
        [ValidateAjax]
        [ISDAuthorizationAttribute]
        public JsonResult Create(FunctionModel model)
        {
            return ExecuteContainer(() =>
            {
                _context.Entry(model).State = EntityState.Added;
                _context.SaveChanges();
                return Json(new
                {
                    Code = System.Net.HttpStatusCode.Created,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Create_Success, LanguageResource.Permission_FunctionModel.ToLower())
                });
            });
        }
        #endregion

        //GET: /Function/Edit
        #region Edit
        [ISDAuthorizationAttribute]
        public ActionResult Edit(string id)
        {
            var func = _context.FunctionModel.FirstOrDefault(p => p.FunctionId == id);
            if (func == null)
            {
                return RedirectToAction("ErrorText", "Error", new { area = "", statusCode = 404, exception = string.Format(LanguageResource.Error_NotExist, LanguageResource.Permission_FunctionModel.ToLower()) });
            }
            return View(func);
        }
        //POST: Edit
        [HttpPost]
        [ISDAuthorizationAttribute]
        public JsonResult Edit(FunctionModel model)
        {
            return ExecuteContainer(() =>
            {
                _context.Entry(model).State = EntityState.Modified;
                _context.SaveChanges();
                return Json(new
                {
                    Code = System.Net.HttpStatusCode.Created,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Edit_Success, LanguageResource.Permission_FunctionModel.ToLower())
                });
            });
        }
        #endregion

        //GET: /Function/Delete
        #region Delete
        [HttpPost]
        [ISDAuthorizationAttribute]
        public ActionResult Delete(string id)
        {
            return ExecuteDelete(() =>
            {
                var func = _context.FunctionModel.FirstOrDefault(p => p.FunctionId == id);
                if (func != null)
                {
                    if (func.PageModel != null)
                    {
                        func.PageModel.Clear();
                    }
                    _context.Entry(func).State = EntityState.Deleted;

                    //Delete in PagePermission
                    var pagePermission = _context.PagePermissionModel.Where(p => p.FunctionId == id).ToList();
                    if (pagePermission != null && pagePermission.Count > 0)
                    {
                        _context.PagePermissionModel.RemoveRange(pagePermission);
                    }
                    _context.SaveChanges();

                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.Created,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Delete_Success, LanguageResource.Permission_FunctionModel.ToLower())
                    });
                }
                else
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotModified,
                        Success = false,
                        Data = ""
                    });
                }
            });
        }
        #endregion 

        //GET: /Function/ExportCreate
        public ActionResult ExportCreate()
        {
            return RedirectToAction("Index");
        }

        public ActionResult ImportCreate()
        {

            return RedirectToAction("Index");
        }
    }
}