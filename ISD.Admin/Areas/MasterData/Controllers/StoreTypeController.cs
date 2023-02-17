using ISD.EntityModels;
using ISD.Extensions;
using ISD.Resources;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ISD.Core;

namespace MasterData.Controllers
{
    public class StoreTypeController : BaseController
    {
        // GET: StoreType
        #region Index
        [ISDAuthorizationAttribute]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult _Search(string StoreTypeName = "", bool? Actived = null)
        {
            return ExecuteSearch(() =>
            {
                var storeTypeList = (from p in _context.StoreTypeModel
                                   orderby p.OrderIndex
                                   where
                                   //search by StoreTypeName
                                   (StoreTypeName == "" || p.StoreTypeName.Contains(StoreTypeName))
                                   //search by Actived
                                   && (Actived == null || p.Actived == Actived)
                                   select p).ToList();

                return PartialView(storeTypeList);
            });
        }
        #endregion

        //GET: /StoreType/Create
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
        public JsonResult Create(StoreTypeModel model)
        {
            return ExecuteContainer(() =>
            {
                model.StoreTypeId = Guid.NewGuid();
                _context.Entry(model).State = EntityState.Added;
                _context.SaveChanges();

                return Json(new
                {
                    Code = System.Net.HttpStatusCode.Created,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Create_Success, LanguageResource.MasterData_StoreType.ToLower())
                });
            });
        }
        #endregion

        //GET: /StoreType/Edit
        #region Edit
        [ISDAuthorizationAttribute]
        public ActionResult Edit(Guid id)
        {
            var storeType = _context.StoreTypeModel.FirstOrDefault(p => p.StoreTypeId == id);
            if (storeType == null)
            {
                return RedirectToAction("ErrorText", "Error", new { area = "", statusCode = 404, exception = string.Format(LanguageResource.Error_NotExist, LanguageResource.MasterData_Company.ToLower()) });
            }
            return View(storeType);
        }
        //POST: Edit
        [HttpPost]
        [ISDAuthorizationAttribute]
        public JsonResult Edit(StoreTypeModel model)
        {
            return ExecuteContainer(() =>
            {
                _context.Entry(model).State = EntityState.Modified;
                _context.SaveChanges();

                return Json(new
                {
                    Code = System.Net.HttpStatusCode.Created,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Edit_Success, LanguageResource.MasterData_StoreType.ToLower())
                });
            });
        }
        #endregion

        //GET: /StoreType/Delete
        #region Delete
        [HttpPost]
        [ISDAuthorizationAttribute]
        public ActionResult Delete(Guid id)
        {
            return ExecuteDelete(() =>
            {
                var storeType = _context.StoreTypeModel.FirstOrDefault(p => p.StoreTypeId == id);
                if (storeType != null)
                {
                    _context.Entry(storeType).State = EntityState.Deleted;
                    _context.SaveChanges();

                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.Created,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Delete_Success, LanguageResource.MasterData_StoreType.ToLower())
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
    }
}