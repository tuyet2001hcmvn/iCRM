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
    public class WarehouseController : BaseController
    {
        // GET: Warehouse
        #region Index
        [ISDAuthorizationAttribute]
        public ActionResult Index()
        {
            //Get list CompanyId
            var compList = _context.CompanyModel.Where(p => p.Actived == true).OrderBy(p => p.OrderIndex).ToList();
            ViewBag.CompanyId = new SelectList(compList, "CompanyId", "CompanyName");

            return View();
        }

        public ActionResult _Search(Guid? StoreId = null, string WarehouseName = "", bool? Actived = null)
        {
            return ExecuteSearch(() =>
            {
                var warehouse = (from p in _context.WarehouseModel
                                 //join to get Store
                                 join br in _context.StoreModel on p.StoreId equals br.StoreId
                                 //join to order by company
                                 join c in _context.CompanyModel on br.CompanyId equals c.CompanyId
                                 orderby c.CompanyId, p.OrderIndex
                                 where
                                 //search by StoreId
                                 (StoreId == null || p.StoreId == StoreId)
                                 //search by WarehouseName
                                 && (WarehouseName == "" || p.WarehouseName.Contains(WarehouseName))
                                 //search by Actived
                                 && (Actived == null || p.Actived == Actived)
                                 select new WarehouseViewModel()
                                 {
                                     WarehouseId = p.WarehouseId,
                                     WarehouseCode = p.WarehouseCode,
                                     WarehouseShortName = p.WarehouseShortName,
                                     WarehouseName = p.WarehouseName,
                                     StoreId = p.StoreId,
                                     StoreName = br.StoreName,
                                     OrderIndex = p.OrderIndex,
                                     Actived = p.Actived
                                 })
                                 .ToList();

                return PartialView(warehouse);
            });
        }
        #endregion

        //GET: /Warehouse/Create
        #region Create
        [ISDAuthorizationAttribute]
        public ActionResult Create()
        {
            //Get list CompanyId
            var compList = _context.CompanyModel.Where(p => p.Actived == true).OrderBy(p => p.OrderIndex).ToList();
            ViewBag.CompanyId = new SelectList(compList, "CompanyId", "CompanyName");

            WarehouseViewModel viewModel = new WarehouseViewModel();
            return View(viewModel);
        }
        //POST: Create
        [HttpPost]
        [ValidateAjax]
        [ISDAuthorizationAttribute]
        public JsonResult Create(WarehouseModel model)
        {
            return ExecuteContainer(() =>
            {
                model.WarehouseId = Guid.NewGuid();
                _context.Entry(model).State = EntityState.Added;
                _context.SaveChanges();

                return Json(new
                {
                    Code = System.Net.HttpStatusCode.Created,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Create_Success, LanguageResource.Sale_Warehouse.ToLower())
                });
            });
        }
        #endregion

        //GET: /Warehouse/View
        #region View
        [ISDAuthorizationAttribute]
        public ActionResult View(Guid id)
        {
            var warehouse = (from p in _context.WarehouseModel
                                 //join to get Store
                             join br in _context.StoreModel on p.StoreId equals br.StoreId
                             //join to get Company
                             join c in _context.CompanyModel on br.CompanyId equals c.CompanyId
                             orderby p.StoreId, p.OrderIndex
                             where p.WarehouseId == id
                             select new WarehouseViewModel()
                             {
                                 WarehouseId = p.WarehouseId,
                                 WarehouseCode = p.WarehouseCode,
                                 WarehouseName = p.WarehouseName,
                                 StoreId = p.StoreId,
                                 StoreName = br.StoreName,
                                 CompanyId = c.CompanyId,
                                 CompanyName = c.CompanyName,
                                 OrderIndex = p.OrderIndex,
                                 Actived = p.Actived
                             })
                             .FirstOrDefault();
            if (warehouse == null)
            {
                return RedirectToAction("ErrorText", "Error", new { area = "", statusCode = 404, exception = string.Format(LanguageResource.Error_NotExist, LanguageResource.Sale_Warehouse.ToLower()) });
            }
            return View(warehouse);
        }
        #endregion View

        //GET: /Warehouse/Edit
        #region Edit
        [ISDAuthorizationAttribute]
        public ActionResult Edit(Guid id)
        {
            var warehouse = (from p in _context.WarehouseModel
                             //join to get Store
                             join br in _context.StoreModel on p.StoreId equals br.StoreId
                             //join to get Company
                             join c in _context.CompanyModel on br.CompanyId equals c.CompanyId
                             orderby p.StoreId, p.OrderIndex
                             where p.WarehouseId == id
                             select new WarehouseViewModel()
                             {
                                 WarehouseId = p.WarehouseId,
                                 WarehouseCode = p.WarehouseCode,
                                 WarehouseShortName = p.WarehouseShortName,
                                 WarehouseName = p.WarehouseName,
                                 StoreId = p.StoreId,
                                 StoreName = br.StoreName,
                                 CompanyId = c.CompanyId,
                                 CompanyName = c.CompanyName,
                                 OrderIndex = p.OrderIndex,
                                 Actived = p.Actived
                             })
                             .FirstOrDefault();
            if (warehouse == null)
            {
                return RedirectToAction("ErrorText", "Error", new { area = "", statusCode = 404, exception = string.Format(LanguageResource.Error_NotExist, LanguageResource.Sale_Warehouse.ToLower()) });
            }
            return View(warehouse);
        }
        //POST: Edit
        [HttpPost]
        [ISDAuthorizationAttribute]
        public JsonResult Edit(WarehouseModel model)
        {
            return ExecuteContainer(() =>
            {
                _context.Entry(model).State = EntityState.Modified;
                _context.SaveChanges();

                return Json(new
                {
                    Code = System.Net.HttpStatusCode.Created,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Edit_Success, LanguageResource.Sale_Warehouse.ToLower())
                });
            });
        }
        #endregion

        //GET: /Warehouse/Delete
        #region Delete
        [HttpPost]
        [ISDAuthorizationAttribute]
        public ActionResult Delete(Guid id)
        {
            return ExecuteDelete(() =>
            {
                var warehouse = _context.WarehouseModel.FirstOrDefault(p => p.WarehouseId == id);
                if (warehouse != null)
                {
                    _context.Entry(warehouse).State = EntityState.Deleted;
                    _context.SaveChanges();

                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.Created,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Delete_Success, LanguageResource.Sale_Warehouse.ToLower())
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

        #region Detail Partial Stock Warehouse
        public ActionResult _ProductWarehouse(Guid? id)
        {
            var stockList = (from p in _context.WarehouseProductModel.AsEnumerable()
                             //Product
                             join pr in _context.ProductModel on p.ProductId equals pr.ProductId
                             //Color: MainColor
                             join c in _context.ColorModel on p.MainColorId equals c.ColorId
                             //Style
                             join s in _context.StyleModel on p.StyleId equals s.StyleId into sg
                             from s1 in sg.DefaultIfEmpty()
                             where p.WarehouseId == id
                             select new WarehouseViewModel()
                             {
                                 ProductName = pr.ProductName,
                                 MainColorProductCode = c.ColorCode,
                                 MainColorProductName = c.ColorName,
                                 StyleWarehouseName = s1 == null ? "" : s1.StyleName,
                                 Quantity = p.Quantity,
                                 ProductWarehousePostDate = (p.PostDate != null && p.PostTime != null) ? p.PostDate + p.PostTime : null,
                                 ProductWarehouseUserPost = p.UserPost
                             }).ToList();

            if (stockList == null)
            {
                stockList = new List<WarehouseViewModel>();
            }
            return PartialView(stockList);
        }
        #endregion Detail Partial Stock Warehouse

        #region Remote Validation
        private bool IsExists(string WarehouseCode)
        {
            return (_context.WarehouseModel.FirstOrDefault(p => p.WarehouseCode == WarehouseCode) != null);
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult CheckExistingWarehouseCode(string WarehouseCode, string WarehouseCodeValid)
        {
            try
            {
                if (WarehouseCodeValid != WarehouseCode)
                {
                    return Json(!IsExists(WarehouseCode));
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

        #region Helper
        public ActionResult GetStoreByCompany(Guid? CompanyId = null)
        {
            //Get list StoreId
            var storeList = _context.StoreModel.Where(p => p.Actived == true && p.CompanyId == CompanyId)
                                               .OrderBy(p => p.OrderIndex).ToList();
            var StoreIdList = new SelectList(storeList, "StoreId", "StoreName");

            return Json(StoreIdList, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}