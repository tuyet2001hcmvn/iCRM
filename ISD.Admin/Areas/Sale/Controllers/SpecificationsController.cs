using ISD.EntityModels;
using ISD.Extensions;
using ISD.Resources;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sale.Controllers
{
    public class SpecificationsController : BaseController
    {
        // GET: Specifications
        #region Index
        [ISDAuthorizationAttribute]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult _Search(string SpecificationsName = "", bool? Actived = null)
        {
            return ExecuteSearch(() =>
            {
                var specifications = (from p in _context.SpecificationsModel
                                      orderby p.OrderIndex
                                      where
                                      //search by CategoryName
                                      (SpecificationsName == "" || p.SpecificationsName.Contains(SpecificationsName))
                                      //search by Actived
                                      && (Actived == null || p.Actived == Actived)
                                      select p)
                                      .ToList();

                return PartialView(specifications);
            });
        }
        #endregion

        //GET: /Specifications/Create
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
        public JsonResult Create(SpecificationsModel model)
        {
            return ExecuteContainer(() =>
            {
                model.SpecificationsId = Guid.NewGuid();
                _context.Entry(model).State = EntityState.Added;
                _context.SaveChanges();

                return Json(new
                {
                    Code = System.Net.HttpStatusCode.Created,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Create_Success, LanguageResource.Sale_Specifications.ToLower())
                });
            });
        }
        #endregion

        //GET: /Specifications/Edit
        #region Edit
        [ISDAuthorizationAttribute]
        public ActionResult Edit(Guid id)
        {
            var specifications = _context.SpecificationsModel.FirstOrDefault(p => p.SpecificationsId == id);
            if (specifications == null)
            {
                return RedirectToAction("ErrorText", "Error", new { area = "", statusCode = 404, exception = string.Format(LanguageResource.Error_NotExist, LanguageResource.Sale_Specifications.ToLower()) });
            }
            return View(specifications);
        }
        //POST: Edit
        [HttpPost]
        [ISDAuthorizationAttribute]
        public JsonResult Edit(SpecificationsModel model)
        {
            return ExecuteContainer(() =>
            {
                _context.Entry(model).State = EntityState.Modified;
                _context.SaveChanges();

                return Json(new
                {
                    Code = System.Net.HttpStatusCode.Created,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Edit_Success, LanguageResource.Sale_Specifications.ToLower())
                });
            });
        }
        #endregion

        //GET: /Specifications/Delete
        #region Delete
        [HttpPost]
        [ISDAuthorizationAttribute]
        public ActionResult Delete(Guid id)
        {
            return ExecuteDelete(() =>
            {
                var specifications = _context.SpecificationsModel.FirstOrDefault(p => p.SpecificationsId == id);
                if (specifications != null)
                {
                    _context.Entry(specifications).State = EntityState.Deleted;
                    _context.SaveChanges();

                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.Created,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Delete_Success, LanguageResource.Sale_Specifications.ToLower())
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
        private bool IsExists(string SpecificationsCode)
        {
            return (_context.SpecificationsModel.FirstOrDefault(p => p.SpecificationsCode == SpecificationsCode) != null);
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult CheckExistingSpecificationsCode(string SpecificationsCode, string SpecificationsCodeValid)
        {
            try
            {
                if (SpecificationsCodeValid != SpecificationsCode)
                {
                    return Json(!IsExists(SpecificationsCode));
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