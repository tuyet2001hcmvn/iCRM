using ISD.EntityModels;
using ISD.Extensions;
using ISD.Resources;
using ISD.Core;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace MasterData.Controllers
{
    public class CatalogController : BaseController
    {
        // GET: Catalog
        #region Index
        [ISDAuthorizationAttribute]
        public ActionResult Index()
        {
            CreateViewBag();
            return View();
        }

        public ActionResult _Search(string catalogCode = "", string catalogText_vi = "",string CatalogTypeCode = "", bool? Actived = null)
        {
            return ExecuteSearch(() =>
            {
                var catalog = (from p in _context.CatalogModel
                               orderby p.CatalogTypeCode, p.OrderIndex
                               where
                               //search by catalogCode
                               (catalogCode == "" || p.CatalogCode.Contains(catalogCode))
                               //Search by catalog Text En
                               && (catalogText_vi == "" || p.CatalogText_vi.Contains(catalogText_vi))
                               //search by catalogtypecode
                               &&(CatalogTypeCode == "" || p.CatalogTypeCode==CatalogTypeCode)
                               //Search by Actived
                               && (Actived == null || p.Actived == Actived)
                               select p).ToList();
                return PartialView(catalog);
            });
        }
        #endregion
        //GET: /Catalog/Create
        #region Create
        [ISDAuthorizationAttribute]
        public ActionResult Create()
        {
            CreateViewBag();
            return View();
        }
        //POST: Create
        [HttpPost]
        [ValidateAjax]
        [ISDAuthorizationAttribute]
        public JsonResult Create(CatalogModel model)
        {
            return ExecuteContainer(() =>
            {
                model.CatalogId = Guid.NewGuid();
                _context.Entry(model).State = EntityState.Added;
                _context.SaveChanges();

                return Json(new
                {
                    Code = System.Net.HttpStatusCode.Created,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Create_Success, LanguageResource.MasterData_Catalog.ToLower())
                });
            });
        }
        #endregion

        //GET: /Catalog/Edit
        #region Edit
        [ISDAuthorizationAttribute]
        public ActionResult Edit(Guid id)
        {
            var catalog = _context.CatalogModel.FirstOrDefault(p => p.CatalogId == id);
            if(catalog == null)
            {
                return RedirectToAction("ErrorText", "Error", new { area = "", statusCode = 404, exception = string.Format(LanguageResource.Error_NotExist, LanguageResource.MasterData_Catalog.ToLower()) });
            }
            CreateViewBag(catalog.CatalogTypeCode);
            return View(catalog);
        }
        //POST: Edit
        [HttpPost]
        [ISDAuthorizationAttribute]
        public JsonResult Edit(CatalogModel model)
        {
            return ExecuteContainer(() =>
            {
                _context.Entry(model).State = EntityState.Modified;
                _context.SaveChanges();

                return Json(new
                {
                    Code = System.Net.HttpStatusCode.Created,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Edit_Success, LanguageResource.MasterData_Catalog.ToLower())
                });
            });
        }
        #endregion

        //GET: /Catalog/Delete
        #region Delete
        [HttpPost]
        [ISDAuthorizationAttribute]
        public ActionResult Delete(Guid id)
        {
            return ExecuteDelete(() =>
            {
                var catalog = _context.CatalogModel.FirstOrDefault(p => p.CatalogId == id);
                if (catalog != null)
                {
                    _context.Entry(catalog).State = EntityState.Deleted;
                    _context.SaveChanges();

                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.Created,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Delete_Success, LanguageResource.MasterData_Catalog.ToLower())
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

        #region CreateViewBag, Helper
        public void CreateViewBag(string CatalogTypeCode = null)
        {
            //Get list CatalogType
            var catalogTypeList = _context.CatalogTypeModel.Where(p => p.Actived == true)
                                                           .OrderBy(p => p.CatalogTypeName).ToList();

            ViewBag.CatalogTypeCode = new SelectList(catalogTypeList, "CatalogTypeCode", "CatalogTypeName", CatalogTypeCode);
        }
        #endregion
    }
}