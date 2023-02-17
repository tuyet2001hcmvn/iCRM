using ISD.EntityModels;
using ISD.Extensions;
using ISD.Resources;
using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ISD.Core;

namespace MasterData.Controllers
{
    public class CompanyController : BaseController
    {
        // GET: Company
        #region Index
        [ISDAuthorizationAttribute]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult _Search(string CompanyName = "", bool? Actived = null)
        {
            return ExecuteSearch(() =>
            {
                var companyList = (from p in _context.CompanyModel
                                   orderby p.OrderIndex
                                   where
                                   //search by CompanyName
                                   (CompanyName == "" || p.CompanyName.Contains(CompanyName))
                                   //search by Actived
                                   && (Actived == null || p.Actived == Actived)
                                   select p).ToList();

                return PartialView(companyList);
            });
        }
        #endregion

        //GET: /Company/Create
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
        public JsonResult Create(CompanyModel model, HttpPostedFileBase FileUpload)
        {
            return ExecuteContainer(() =>
            {
                model.CompanyId = Guid.NewGuid();
                if (FileUpload != null)
                {
                    model.Logo = getFileName(FileUpload);
                }
                _context.Entry(model).State = EntityState.Added;
                _context.SaveChanges();

                return Json(new
                {
                    Code = System.Net.HttpStatusCode.Created,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Create_Success, LanguageResource.MasterData_Company.ToLower())
                });
            });
        }
        #endregion

        //GET: /Company/Edit
        #region Edit
        [ISDAuthorizationAttribute]
        public ActionResult Edit(Guid id)
        {
            var company = _context.CompanyModel.FirstOrDefault(p => p.CompanyId == id);
            if (company == null)
            {
                return RedirectToAction("ErrorText", "Error", new { area = "", statusCode = 404, exception = string.Format(LanguageResource.Error_NotExist, LanguageResource.MasterData_Company.ToLower()) });
            }
            return View(company);
        }
        //POST: Edit
        [HttpPost]
        [ISDAuthorizationAttribute]
        public JsonResult Edit(CompanyModel model, HttpPostedFileBase FileUpload)
        {
            return ExecuteContainer(() =>
            {
                if (FileUpload != null)
                {
                    model.Logo = getFileName(FileUpload);
                }
                _context.Entry(model).State = EntityState.Modified;
                _context.SaveChanges();

                return Json(new
                {
                    Code = System.Net.HttpStatusCode.Created,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Edit_Success, LanguageResource.MasterData_Company.ToLower())
                });
            });
        }
        #endregion

        //GET: /Company/Delete
        #region Delete
        [HttpPost]
        [ISDAuthorizationAttribute]
        public ActionResult Delete(Guid id)
        {
            return ExecuteDelete(() =>
            {
                var company = _context.CompanyModel.FirstOrDefault(p => p.CompanyId == id);
                if (company != null)
                {
                    _context.Entry(company).State = EntityState.Deleted;
                    _context.SaveChanges();

                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.Created,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Delete_Success, LanguageResource.MasterData_Company.ToLower())
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
        private bool IsExists(string CompanyCode)
        {
            return (_context.CompanyModel.FirstOrDefault(p => p.CompanyCode == CompanyCode) != null);
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult CheckExistingCompanyCode(string CompanyCode, string CompanyCodeValid)
        {
            try
            {
                if (CompanyCodeValid != CompanyCode)
                {
                    return Json(!IsExists(CompanyCode));
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

        public string getFileName(HttpPostedFileBase file)
        {
            var fileName = Path.GetFileName(file.FileName);

            //Create dynamically folder to save file
            var existPath = Server.MapPath("~/Upload/Company");
            Directory.CreateDirectory(existPath);
            var path = Path.Combine(existPath, fileName);

            file.SaveAs(path);

            return fileName;
        }
    }
}