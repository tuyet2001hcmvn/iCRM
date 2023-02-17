using ISD.EntityModels;
using ISD.Extensions;
using ISD.Resources;
using ISD.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sale.Controllers
{
    public class AccessoryCategoryController : BaseController
    {
        // GET: AccessoryCategory
        #region Index
        [ISDAuthorizationAttribute]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult _Search(string AccessoryCategoryName = "", bool? Actived = null)
        {
            return ExecuteSearch(() =>
            {
                var accCategory = (from p in _context.AccessoryCategoryModel
                                orderby p.OrderIndex
                                where
                                //search by AccessoryCategoryName
                                (AccessoryCategoryName == "" || p.AccessoryCategoryName.Contains(AccessoryCategoryName))
                                //search by Actived
                                && (Actived == null || p.Actived == Actived)
                                select p)
                               .ToList();

                return PartialView(accCategory);
            });
        }
        #endregion

        //GET: /AccessoryCategory/Create
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
        public JsonResult Create(AccessoryCategoryModel model, HttpPostedFileBase ImageUrl)
        {
            return ExecuteContainer(() =>
            {
                model.AccessoryCategoryId = Guid.NewGuid();
                if (ImageUrl != null)
                {
                    model.ImageUrl = Upload(ImageUrl, "Category");
                }
                _context.Entry(model).State = EntityState.Added;
                _context.SaveChanges();

                return Json(new
                {
                    Code = System.Net.HttpStatusCode.Created,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Create_Success, LanguageResource.Sale_AccessoryCategory.ToLower())
                });
            });
        }
        #endregion

        //GET: /AccessoryCategory/Edit
        #region Edit
        [ISDAuthorizationAttribute]
        public ActionResult Edit(Guid id)
        {
            var accCategory = _context.AccessoryCategoryModel.FirstOrDefault(p => p.AccessoryCategoryId == id);
            if (accCategory == null)
            {
                return RedirectToAction("ErrorText", "Error", new { area = "", statusCode = 404, exception = string.Format(LanguageResource.Error_NotExist, LanguageResource.Sale_AccessoryCategory.ToLower()) });
            }
            return View(accCategory);
        }
        //POST: Edit
        [HttpPost]
        [ISDAuthorizationAttribute]
        public JsonResult Edit(AccessoryCategoryViewModel viewModel, HttpPostedFileBase ImageUrl)
        {
            return ExecuteContainer(() =>
            {
                var modelUpdate = _context.AccessoryCategoryModel.Find(viewModel.AccessoryCategoryId);
                modelUpdate.AccessoryCategoryName = viewModel.AccessoryCategoryName;
                modelUpdate.OrderIndex = viewModel.OrderIndex;
                modelUpdate.Actived = viewModel.Actived;
                if (ImageUrl != null)
                {
                    modelUpdate.ImageUrl = Upload(ImageUrl, "AccessoryCategory");
                }

                _context.Entry(modelUpdate).State = EntityState.Modified;
                _context.SaveChanges();

                return Json(new
                {
                    Code = System.Net.HttpStatusCode.Created,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Edit_Success, LanguageResource.Sale_AccessoryCategory.ToLower())
                });
            });
        }
        #endregion

        //GET: /AccessoryCategory/Delete
        #region Delete
        [HttpPost]
        [ISDAuthorizationAttribute]
        public ActionResult Delete(Guid id)
        {
            return ExecuteDelete(() =>
            {
                var accCategory = _context.AccessoryCategoryModel.FirstOrDefault(p => p.AccessoryCategoryId == id);
                if (accCategory != null)
                {
                    _context.Entry(accCategory).State = EntityState.Deleted;
                    _context.SaveChanges();

                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.Created,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Delete_Success, LanguageResource.Sale_Brand.ToLower())
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

        #region Remote Validation
        private bool IsExists(string AccessoryCategoryCode)
        {
            return (_context.AccessoryCategoryModel.FirstOrDefault(p => p.AccessoryCategoryCode == AccessoryCategoryCode) != null);
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult CheckExistingAccessoryCategoryCode(string AccessoryCategoryCode, string AccessoryCategoryCodeValid)
        {
            try
            {
                if (AccessoryCategoryCodeValid != AccessoryCategoryCode)
                {
                    return Json(!IsExists(AccessoryCategoryCode));
                }
                else
                {
                    return Json(true);
                }
            }
            catch (Exception ex)
            {
                return Json(false);
            }
        }
        #endregion
    }
}