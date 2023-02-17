using ISD.Constant;
using ISD.Extensions;
using ISD.Resources;
using ISD.ViewModels;
using ISD.Core;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Warehouse.Controllers
{
    public class StockReceivingController : BaseController
    {
        #region Index
        // GET: StockReceiving
        [ISDAuthorizationAttribute]
        public ActionResult Index()
        {
            ViewBag.PageId = GetPageId("/Warehouse/StockReceiving");
            CreateViewBagForSearch();
            return View();
        }
        public ActionResult _Search(StockReceivingSearchViewModel searchViewModel)
        {
            var stockReceiving = _unitOfWork.StockReceivingMasterRepository.Search(searchViewModel);
            return PartialView(stockReceiving);
        }
        #endregion

        #region Create
        [ISDAuthorizationAttribute]
        public ActionResult Create()
        {
            var companyId = CurrentUser.CompanyId;
            var storeId = _unitOfWork.StoreRepository.GetStoreIdBySaleOrgCode(CurrentUser.SaleOrg);
            var saleEmployeeCode = CurrentUser.EmployeeCode;

            var stockReciveVM = new StockReceivingMasterViewModel
            {
                DocumentDate = DateTime.Now
            };

            ViewBag.StockId = _unitOfWork.StockRepository.GetStockIdByStockCode(CurrentUser.SaleOrg);

            ViewBag.ListStockRecevingDetail = _context.StockReceivingDetailModel.Select(p => new StockReceivingDetailViewModel { }).ToList();
            ViewBag.StockList = _unitOfWork.StockRepository.GetStockByStore(storeId);
            CreateViewBag(companyId, storeId, saleEmployeeCode);
            return View(stockReciveVM);
        }

        [HttpPost]
        public JsonResult Create(StockReceivingViewModel viewModel, List<StockReceivingDetailViewModel> stockReceivingDetailList)
        {
            return ExecuteContainer(() =>
            {
                //Get datekey from documentDate
                var dateKey = _unitOfWork.UtilitiesRepository.ConvertDateTimeToInt(viewModel.DocumentDate);
                var dateKeyNow = _unitOfWork.UtilitiesRepository.ConvertDateTimeToInt(DateTime.Now);
                if (dateKey > dateKeyNow)
                {
                    return Json(new
                    {
                        Code = HttpStatusCode.NotModified,
                        Success = false,
                        Data = "Vui lòng nhập ngày chứng từ là ngày hiện tại hoặc trước ngày hiện tại!"
                    });
                }
                if (stockReceivingDetailList == null || stockReceivingDetailList.Count == 0)
                {
                    return Json(new
                    {
                        Code = HttpStatusCode.NotModified,
                        Success = false,
                        Data = "Vui lòng nhập ít nhất 1 sản phẩm để nhập kho!"
                    });
                }
                var stockid = stockReceivingDetailList.First().StockId;
                var dateKeyExist = _context.StockReceivingDetailModel.Where(p => p.StockId == stockid).Max(p => p.DateKey);
                if (dateKey < dateKeyExist)
                {
                    return Json(new
                    {
                        Code = HttpStatusCode.NotModified,
                        Success = false,
                        Data = "Vui lòng chọn ngày chứng từ trùng hoặc sau ngày chừng từ gần nhât!"
                    });
                }

                //Nhập kho không require Nhà cung cấp (ProfileId)
                if (viewModel.ProfileId == Guid.Empty)
                {
                    viewModel.ProfileId = null;
                }
                viewModel.StockReceivingId = Guid.NewGuid();
                viewModel.CreateBy = CurrentUser.AccountId;
                viewModel.CreateTime = DateTime.Now;
                _unitOfWork.StockReceivingMasterRepository.Create(viewModel);
                //Add Stock Receiving Detail
                if (stockReceivingDetailList.Count > 0 && stockReceivingDetailList != null)
                {
                    foreach (var stockRecevingDetailVM in stockReceivingDetailList)
                    {
                        stockRecevingDetailVM.StockReceivingDetailId = Guid.NewGuid();
                        stockRecevingDetailVM.StockReceivingId = viewModel.StockReceivingId;
                        stockRecevingDetailVM.DateKey = dateKey;
                        _unitOfWork.StockRecevingDetailRepository.Create(stockRecevingDetailVM);
                    }
                }
                _context.SaveChanges();

                return Json(new
                {
                    Code = HttpStatusCode.Created,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Create_Success, LanguageResource.StockReceiving.ToLower()),
                    Id = viewModel.StockReceivingId,
                });
            });
        }
        #endregion

        #region View
        public ActionResult View(Guid id)
        {
            var stockReceive = _unitOfWork.StockReceivingMasterRepository.GetById(id);
            if (stockReceive == null)
            {
                return RedirectToAction("ErrorText", "Error", new { area = "", statusCode = 404, exception = string.Format(LanguageResource.Error_NotExist, LanguageResource.StockReceive_Bill.ToLower()) });
            }
            ViewBag.ListStockRecevingDetail = _unitOfWork.StockRecevingDetailRepository.GetByStockReceiveMaster(id);
            if (stockReceive.ProfileId != null && stockReceive.ProfileId != Guid.Empty)
            {
                ViewBag.Supplier = _unitOfWork.ProfileRepository.GetById((Guid)stockReceive.ProfileId);
            }
            return View(stockReceive);
        }
        #endregion

        #region Product
        public ActionResult InsertProductStock(StockReceivingDetailViewModel model, List<StockReceivingDetailViewModel> stockReceivingDetailList)
        {
            if (stockReceivingDetailList == null)
            {
                stockReceivingDetailList = new List<StockReceivingDetailViewModel>();
            }
            stockReceivingDetailList.Add(model);

            return PartialView("_ProductStockDetailInner", stockReceivingDetailList);
        }
        public ActionResult RemoveProductStock(List<StockReceivingDetailViewModel> stockReceivingDetailList, int STT)
        {
            stockReceivingDetailList = stockReceivingDetailList.Where(p => p.STT != STT).ToList();
            return PartialView("_ProductStockDetailInner", stockReceivingDetailList);
        }
        #endregion

        #region Delete
        [HttpPost]
        [ISDAuthorizationAttribute]
        public ActionResult Delete(Guid? id, string DeletedReason)
        {
            return ExecuteDelete(() =>
            {
                if (string.IsNullOrEmpty(DeletedReason))
                {
                    return Json(new
                    {
                        Code = HttpStatusCode.NotModified,
                        Success = false,
                        Data = "Vui lòng nhập thông tin \"Lý do hủy\"!"
                    });
                }
                var stockReceivDb = _context.StockReceivingMasterModel.FirstOrDefault(p => p.StockReceivingId == id);
                if (stockReceivDb == null)
                {
                    return Json(new
                    {
                        Code = HttpStatusCode.NotModified,
                        Success = false,
                        Data = ""
                    });
                }
                else
                {
                    stockReceivDb.DeletedBy = CurrentUser.AccountId;
                    stockReceivDb.DeletedTime = DateTime.Now;
                    stockReceivDb.isDeleted = true;
                    stockReceivDb.DeletedReason = DeletedReason;
                    _context.Entry(stockReceivDb).State = EntityState.Modified;

                    //update hủy detail
                    var listStockDetailDb = _context.StockReceivingDetailModel.Where(p => p.StockReceivingId == id).ToList();
                    if (listStockDetailDb != null && listStockDetailDb.Count > 0)
                    {
                        foreach (var stockDetail in listStockDetailDb)
                        {
                            stockDetail.isDeleted = true;
                            _context.Entry(stockDetail).State = EntityState.Modified;
                        }
                    }
                    _context.SaveChanges();

                    return Json(new
                    {
                        Code = HttpStatusCode.Created,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Delete_Success, LanguageResource.StockReceiving.ToLower())
                    });
                }
            });
        }
        #endregion

        #region Cancel
        [HttpPost]
        [ISDAuthorizationAttribute]
        public ActionResult Cancel(Guid? id, string DeletedReason)
        {
            return ExecuteDelete(() =>
            {
                if (string.IsNullOrEmpty(DeletedReason))
                {
                    return Json(new
                    {
                        Code = HttpStatusCode.NotModified,
                        Success = false,
                        Data = "Vui lòng nhập thông tin \"Lý do hủy\"!"
                    });
                }
                var stockReceivDb = _context.StockReceivingMasterModel.FirstOrDefault(p => p.StockReceivingId == id);
                if (stockReceivDb == null)
                {
                    return Json(new
                    {
                        Code = HttpStatusCode.NotModified,
                        Success = false,
                        Data = string.Format(LanguageResource.Alert_NotExist_Cancel, LanguageResource.StockReceiving.ToLower())
                    });
                }
                else
                {
                    stockReceivDb.DeletedBy = CurrentUser.AccountId;
                    stockReceivDb.DeletedTime = DateTime.Now;
                    stockReceivDb.isDeleted = true;
                    stockReceivDb.DeletedReason = DeletedReason;
                    _context.Entry(stockReceivDb).State = EntityState.Modified;

                    //update hủy detail
                    var listStockDetailDb = _context.StockReceivingDetailModel.Where(p => p.StockReceivingId == id).ToList();
                    if (listStockDetailDb != null && listStockDetailDb.Count > 0)
                    {
                        foreach (var stockDetail in listStockDetailDb)
                        {
                            stockDetail.isDeleted = true;
                            _context.Entry(stockDetail).State = EntityState.Modified;
                        }
                    }
                    _context.SaveChanges();

                    return Json(new
                    {
                        Code = HttpStatusCode.Created,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Cancel_Success, LanguageResource.StockReceiving.ToLower())
                    });
                }
            });
        }
        #endregion

        #region ViewBag, Helper
        private void CreateViewBag(Guid? CompanyId = null, Guid? StoreId = null, string SaleEmployeeCode = "")
        {
            //Dropdown Company
            var companyList = _unitOfWork.CompanyRepository.GetAll();
            ViewBag.CompanyId = new SelectList(companyList, "CompanyId", "CompanyName", CompanyId);

            //Dropdown Store
            var storeList = _unitOfWork.StoreRepository.GetAllStore();
            ViewBag.StoreId = new SelectList(storeList, "StoreId", "StoreName", StoreId);

            //Dropdow nhà cung cấp

            //ViewBag.ProfileId = new SelectList();

            //Dropdown Nhân viên
            var saleEmployeeList = _unitOfWork.SalesEmployeeRepository.GetAllForDropdownlist();
            ViewBag.SalesEmployeeCode = new SelectList(saleEmployeeList, "SalesEmployeeCode", "SalesEmployeeName", SaleEmployeeCode);

            //Dropdown Stock
            var listStock = _unitOfWork.StockRepository.GetAllForDropdown();
            ViewBag.StockId = new SelectList(listStock, "StockId", "StockName");

            //Dropdown Product
            ViewBag.ProductList = _unitOfWork.ProductRepository.GetProInventory();
            //ViewBag.ProductId = new SelectList(productList, "ProductId", "ProductName");

            //Chỉ lấy khách hàng (B | C)
            //var customerTypeList = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.CustomerType);
            //customerTypeList = customerTypeList.Where(p => p.CatalogCode != ConstCustomerType.Contact).ToList();
            //ViewBag.CustomerTypeCode = new SelectList(customerTypeList, "CatalogCode", "CatalogText_vi");
        }

        private void CreateViewBagForSearch()
        {
            //Dropdown Company
            var companyList = _unitOfWork.CompanyRepository.GetAll();
            ViewBag.SearchCompanyId = new SelectList(companyList, "CompanyId", "CompanyName");

            //Dropdown Store
            var storeList = _unitOfWork.StoreRepository.GetAllStore();
            ViewBag.SearchStoreId = new SelectList(storeList, "StoreId", "StoreName");

            //Dropdown Nhân viên
            var saleEmployeeList = _unitOfWork.SalesEmployeeRepository.GetAllForDropdownlist();
            ViewBag.SearchSalesEmployeeCode = new SelectList(saleEmployeeList, "SalesEmployeeCode", "SalesEmployeeName");

            //Dropdown Stock
            var listStock = _unitOfWork.StockRepository.GetAllForDropdown();
            ViewBag.SearchStockId = new SelectList(listStock, "StockId", "StockName");
        }
        public ActionResult GetProductCode(string ProductCode)
        {
            var catalogue = _context.CategoryModel.FirstOrDefault(s => s.CategoryCode == "CTL");
            var product = (from p in _context.ProductModel
                           where  p.Actived == true
                           && (p.ProductCode.Contains(ProductCode) || p.ProductName.Contains(ProductCode) || p.ERPProductCode.Contains(ProductCode))
                           && p.ParentCategoryId == catalogue.CategoryId
                           select new
                           {
                               ProductId = p.ProductId,
                               ProductCode = p.ProductCode,
                               //ProductCodeText = p.ProductCode + " | " + p.ProductName,
                               ProductCodeText = p.ProductName,
                               ProductName = p.ProductName,
                           }).Take(5).ToList();
            return Json(product, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ProductCode"></param>
        /// <returns></returns>
        public ActionResult SearchProduct(string search)
        {
            var catalogue = _context.CategoryModel.FirstOrDefault(s => s.CategoryCode == "CTL");
            var product = (from p in _context.ProductModel
                           where  p.Actived == true
                           && (p.ProductCode.Contains(search) || p.ProductName.Contains(search) || p.ERPProductCode.Contains(search))
                           && p.ParentCategoryId == catalogue.CategoryId
                           select new ISDSelectItem()
                           {
                               value = p.ProductId,
                               text = p.ProductName
                           }).Take(10).ToList();
            return Json(product, JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult GetProductCodeByCategoryId(string ProductCode, Guid  CategoryId)
        {
            var catalogue = _context.CategoryModel.FirstOrDefault(s => s.CategoryCode == "CTL");
            var product = (from p in _context.ProductModel
                           where p.Actived == true
                           && (p.ProductCode.Contains(ProductCode) || p.ProductName.Contains(ProductCode) || p.ERPProductCode.Contains(ProductCode))
                           && p.ParentCategoryId == catalogue.CategoryId
                           && p.CategoryId == CategoryId
                           select new
                           {
                               ProductId = p.ProductId,
                               ProductCode = p.ProductCode,
                               //ProductCodeText = p.ProductCode + " | " + p.ProductName,
                               ProductCodeText = p.ProductName,
                               ProductName = p.ProductName,
                           }).Take(5).ToList();
            return Json(product, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetProductCodeIsInventory(string ProductCode)
        {
            var catalogue = _context.CategoryModel.FirstOrDefault(s => s.CategoryCode == "CTL");
            var product = (from p in _context.ProductModel
                           where p.isInventory == true
                           && p.Actived == true
                           && (p.ProductCode.Contains(ProductCode) || p.ProductName.Contains(ProductCode) || p.ERPProductCode.Contains(ProductCode))
                           && p.ParentCategoryId== catalogue.CategoryId
                           select new
                           {
                               ProductId = p.ProductId,
                               ProductCode = p.ProductCode,
                               //ProductCodeText = p.ProductCode + " | " + p.ProductName,
                               ProductCodeText = p.ProductName,
                               ProductName = p.ProductName,
                           }).Take(5).ToList();
            return Json(product, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetStockByStore(Guid StoreId)
        {
            var stock = _unitOfWork.StockRepository.GetStockByStore2(StoreId).FirstOrDefault();
            //var slStock = new SelectList(stock, "StockId", "StockName");
            return Json(stock, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Nhà cung cấp
        public ActionResult _CustomerSearch(string prefix = "")
        {
            ViewBag.PreFix = prefix;
            var customerTypeList = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.CustomerType);
            customerTypeList = customerTypeList.Where(p => p.CatalogCode != ConstCustomerType.Contact).ToList();
            ViewBag.CustomerTypeCode = new SelectList(customerTypeList, "CatalogCode", "CatalogText_vi");
            return PartialView();
        }

        public ActionResult _CustomerSearchResult(ProfileSearchViewModel searchViewModel, string prefix = "")
        {
            var lst = _unitOfWork.ProfileRepository.Search(searchViewModel);
            ViewBag.PreFix = prefix;
            return PartialView(lst);
        }
        #endregion
    }
}