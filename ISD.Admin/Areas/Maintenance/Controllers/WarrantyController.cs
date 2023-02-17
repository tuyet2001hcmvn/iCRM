using ISD.EntityModels;
using ISD.Extensions;
using ISD.Repositories;
using ISD.Resources;
using ISD.Core;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Maintenance.Controllers
{
    public class WarrantyController : BaseController
    {
        #region Index
        // GET: Warranty
        [ISDAuthorizationAttribute]
        public ActionResult Index()
        {
            return View();
        }
        

        public ActionResult _Search(string warrantyCode = "", string warrantyName = "", bool? actived = null)
        {
            return ExecuteSearch(() =>
            {
                var warrantyList = _unitOfWork.WarrantyRepository.Search(warrantyCode,warrantyName,actived);
                return PartialView(warrantyList);
            });
        }
        #endregion

        #region Create
        [ISDAuthorizationAttribute]
        public ActionResult Create()
        {
            return View();
        }
        //POST: Create
        [HttpPost]
        public JsonResult Create(WarrantyModel model)
        {
            return ExecuteContainer(() =>
            {
                model.WarrantyId = Guid.NewGuid();
                model.CreateBy = CurrentUser.AccountId;
                model.CreateTime = DateTime.Now;
                model.Actived = true;
                //Call Create
                _unitOfWork.WarrantyRepository.Create(model);
                _context.SaveChanges();
                return Json(new
                {
                    Code = System.Net.HttpStatusCode.Created,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Create_Success, LanguageResource.Maintenance_Warranty.ToLower())
                });
            });
        }
        #endregion

        #region Edit
        [ISDAuthorizationAttribute]
        public ActionResult Edit(Guid id)
        {
            var warranty = _unitOfWork.WarrantyRepository.GetWarranty(id);
            if(warranty == null)
            {
                return RedirectToAction("ErrorText", "Error", new { area = "", statusCode = 404, exception = string.Format(LanguageResource.Error_NotExist, LanguageResource.Maintenance_Warranty.ToLower()) });
            }
            return View(warranty);
        }
        [HttpPost]
        public JsonResult Edit(WarrantyModel warrantyModel)
        {
           return ExecuteContainer(() =>
            {
                warrantyModel.LastEditBy = CurrentUser.AccountId;
                warrantyModel.LastEditTime = DateTime.Now;
                _unitOfWork.WarrantyRepository.Update(warrantyModel);
                _context.SaveChanges();
                return Json(new
                {
                    Code = System.Net.HttpStatusCode.Created,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Edit_Success, LanguageResource.Maintenance_Warranty.ToLower())
                });
            });
        }
        #endregion

        #region Remote Validate
        private bool IsExistsWarrantyCode(string WarrantyCode)
        {
            return (_context.WarrantyModel.FirstOrDefault(p => p.WarrantyCode == WarrantyCode) != null);
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult CheckExistingWarrantyCode(string WarrantyCode, string WarrantyCodeValid)
        {
            try
            {
                if (WarrantyCodeValid != WarrantyCode)
                {
                    return Json(!IsExistsWarrantyCode(WarrantyCode));
                }
                else
                {
                    return Json(true);
                }
            }
            catch// (Exception ex)
            {
                return Json(false);
            }
        }
        #endregion
    }
}