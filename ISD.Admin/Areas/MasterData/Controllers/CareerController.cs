using ISD.EntityModels;
using ISD.Extensions;
using ISD.Resources;
using ISD.Core;
using ISD.ViewModels.MasterData;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace MasterData.Controllers
{
    public class CareerController : BaseController
    {
        // GET: Career
        #region Index
        [ISDAuthorizationAttribute]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult _Search(CareerSearchViewModel searchViewModel)
        {
            Session["frmSearchProvince"] = searchViewModel;
            return ExecuteSearch(() =>
            {
                var careerList = (from c in _context.CareerModel                            
                                    where
                                    //search by CareerName
                                    (searchViewModel.CareerName == null || c.CareerName.Contains(searchViewModel.CareerName))
                                    //search by Actived
                                    && (searchViewModel.Actived == null || c.Actived == searchViewModel.Actived)
                                    select c).ToList();

                return PartialView(careerList);
            });          
        }
        #endregion
        //GET: /Province/Create
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
        public JsonResult Create(CareerModel model)
        {
            return ExecuteContainer(() =>
            {
                model.CareerId = Guid.NewGuid();
                _context.Entry(model).State = EntityState.Added;
                _context.SaveChanges();

                return Json(new
                {
                    Code = System.Net.HttpStatusCode.Created,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Create_Success, LanguageResource.MasterData_Career.ToLower())
                });
            });
        }

        #endregion

        //GET: /Province/Edit
        #region Edit
        [ISDAuthorizationAttribute]
        public ActionResult Edit(Guid id)
        {
            var career = (from c in _context.CareerModel
                            where c.CareerId == id
                            select new CareerViewModel()
                            {
                                CareerId = c.CareerId,
                                CareerCode = c.CareerCode,
                                CareerCodeValid = c.CareerCode,
                                CareerName = c.CareerName,                               
                                Actived = c.Actived
                            }).FirstOrDefault();
            if (career == null)
            {
                return RedirectToAction("ErrorText", "Error", new { area = "", statusCode = 404, exception = string.Format(LanguageResource.Error_NotExist, LanguageResource.MasterData_Career.ToLower()) });
            }
            return View(career);
        }
        //POST: Edit
        [HttpPost]
        [ISDAuthorizationAttribute]
        public JsonResult Edit(CareerModel model)
        {
            return ExecuteContainer(() =>
            {
                _context.Entry(model).State = EntityState.Modified;
                _context.SaveChanges();

                return Json(new
                {
                    Code = System.Net.HttpStatusCode.Created,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Edit_Success, LanguageResource.MasterData_Career.ToLower())
                });
            });
        }

        #endregion
        //Check trùng
        #region Remote Validation
        private bool IsExists(string CareerCode)
        {
            return (_context.CareerModel.FirstOrDefault(p => p.CareerCode == CareerCode) != null);
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult CheckExistingCareerCode(string CareerCode, string CareerCodeValid)
        {
            try
            {
                if (CareerCodeValid != CareerCode)
                {
                    return Json(!IsExists(CareerCode));
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