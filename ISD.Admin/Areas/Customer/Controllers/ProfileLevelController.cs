using ISD.EntityModels;
using ISD.Extensions;
using ISD.Resources;
using System;
using System.Linq;
using System.Web.Mvc;
using ISD.Core;

namespace Customer.Controllers
{
    public class ProfileLevelController : BaseController
    {
        // GET: ProfileLevel
        #region Index
        // GET: Warranty
        [ISDAuthorizationAttribute]
        public ActionResult Index()
        {
            CreateViewBag();
            return View();
        }
        public ActionResult _Search(Guid? CompanyId = null, string CustomerLevelCode = "", string CustomerLevelName = "", bool? Actived = null)
        {
            return ExecuteSearch(() =>
            {
                var lst = _unitOfWork.ProfileLevelRepository.Search(CompanyId, CustomerLevelCode, CustomerLevelName, Actived);
                return PartialView(lst);
            });
        }
        #endregion

        #region Create
        [ISDAuthorizationAttribute]
        public ActionResult Create()
        {
            CreateViewBag();
            return View();
        }
        //POST: Create
        [HttpPost]
        public JsonResult Create(ProfileLevelModel model)
        {
            return ExecuteContainer(() =>
            {
                var existCodeUsing = _context.ProfileLevelModel
                                             .FirstOrDefault(p => p.CustomerLevelCode == model.CustomerLevelCode && p.Actived == true);
                if (existCodeUsing != null)
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotModified,
                        Success = false,
                        Data = string.Format(LanguageResource.IsBeingUsed, LanguageResource.CustomerLevelCode)
                    });
                }
                model.CustomerLevelId = Guid.NewGuid();
                model.CreateBy = CurrentUser.AccountId;
                model.CreateTime = DateTime.Now;
                model.Actived = true;
                //Call Create
                _unitOfWork.ProfileLevelRepository.Create(model);
                _context.SaveChanges();
                return Json(new
                {
                    Code = System.Net.HttpStatusCode.Created,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Create_Success, LanguageResource.Customer_ProfileLevel.ToLower())
                });
            });
        }
        #endregion

        #region Edit
        [ISDAuthorizationAttribute]
        public ActionResult Edit(Guid id)
        {
            var level = _unitOfWork.ProfileLevelRepository.GetProfileLevel(id);
            if (level == null)
            {
                return RedirectToAction("ErrorText", "Error", new { area = "", statusCode = 404, exception = string.Format(LanguageResource.Error_NotExist, LanguageResource.Customer_ProfileLevel.ToLower()) });
            }
            CreateViewBag(level.CompanyId);
            return View(level);
        }
        [HttpPost]
        public JsonResult Edit(ProfileLevelModel model)
        {
            return ExecuteContainer(() =>
            {
                model.LastEditBy = CurrentUser.AccountId;
                model.LastEditTime = DateTime.Now;
                _unitOfWork.ProfileLevelRepository.Update(model);
                _context.SaveChanges();

                return Json(new
                {
                    Code = System.Net.HttpStatusCode.Created,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Edit_Success, LanguageResource.Customer_ProfileLevel.ToLower())
                });
            });
        }
        #endregion

        public void CreateViewBag(Guid? CompanyId = null)
        {
            var companyLst = _context.CompanyModel.Where(p => p.Actived == true)
                                                  .OrderBy(p => p.CompanyCode).ToList();
            ViewBag.CompanyId = new SelectList(companyLst, "CompanyId", "CompanyName", CompanyId);
        }
    }
}