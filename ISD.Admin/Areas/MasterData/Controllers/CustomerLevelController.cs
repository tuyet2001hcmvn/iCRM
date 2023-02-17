using ISD.EntityModels;
using ISD.Extensions;
using ISD.Resources;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using ISD.Core;

namespace MasterData.Controllers
{
    public class CustomerLevelController : BaseController
    {
        // GET: CustomerLevel
        #region Index
        [ISDAuthorizationAttribute]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult _Search(string CustomerLevelName = "", bool? Actived = null)
        {
            return ExecuteSearch(() =>
            {
                var customerLevelList = (from p in _context.CustomerLevelModel
                                         orderby p.OrderIndex
                                         where
                                         //search by CustomerLevelName
                                         (CustomerLevelName == "" || p.CustomerLevelName.Contains(CustomerLevelName))
                                         //search by Actived
                                         && (Actived == null || p.Actived == Actived)
                                         select p).ToList();

                return PartialView(customerLevelList);
            });
        }
        #endregion

        //GET: /CustomerLevel/Create
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
        public JsonResult Create(CustomerLevelModel model)
        {
            return ExecuteContainer(() =>
            {
                model.CustomerLevelId = Guid.NewGuid();
                _context.Entry(model).State = EntityState.Added;
                _context.SaveChanges();

                return Json(new
                {
                    Code = System.Net.HttpStatusCode.Created,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Create_Success, LanguageResource.MasterData_CustomerLevel.ToLower())
                });
            });
        }
        #endregion

        //GET: /CustomerLevel/Edit
        #region Edit
        [ISDAuthorizationAttribute]
        public ActionResult Edit(Guid id)
        {
            var customerLevel = _context.CustomerLevelModel.FirstOrDefault(p => p.CustomerLevelId == id);
            if (customerLevel == null)
            {
                return RedirectToAction("ErrorText", "Error", new { area = "", statusCode = 404, exception = string.Format(LanguageResource.Error_NotExist, LanguageResource.MasterData_CustomerLevel.ToLower()) });
            }
            return View(customerLevel);
        }
        //POST: Edit
        [HttpPost]
        [ISDAuthorizationAttribute]
        public JsonResult Edit(CustomerLevelModel model)
        {
            return ExecuteContainer(() =>
            {
                _context.Entry(model).State = EntityState.Modified;
                _context.SaveChanges();

                return Json(new
                {
                    Code = System.Net.HttpStatusCode.Created,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Edit_Success, LanguageResource.MasterData_CustomerLevel.ToLower())
                });
            });
        }
        #endregion

        //GET: /CustomerLevel/Delete
        #region Delete
        [HttpPost]
        [ISDAuthorizationAttribute]
        public ActionResult Delete(Guid id)
        {
            return ExecuteDelete(() =>
            {
                var customerLevel = _context.CustomerLevelModel.FirstOrDefault(p => p.CustomerLevelId == id);
                if (customerLevel != null)
                {
                    _context.Entry(customerLevel).State = EntityState.Deleted;
                    _context.SaveChanges();

                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.Created,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Delete_Success, LanguageResource.MasterData_CustomerLevel.ToLower())
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
        private bool IsExists(string CustomerLevelCode)
        {
            return (_context.CustomerLevelModel.FirstOrDefault(p => p.CustomerLevelCode == CustomerLevelCode) != null);
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult CheckExistingCustomerLevelCode(string CustomerLevelCode, string CustomerLevelCodeValid)
        {
            try
            {
                if (CustomerLevelCodeValid != CustomerLevelCode)
                {
                    return Json(!IsExists(CustomerLevelCode));
                }
                else
                {
                    return Json(true);
                }
            }
            catch //(Exception ex)
            {
                return Json(false);
            }
        }
        #endregion
    }
}