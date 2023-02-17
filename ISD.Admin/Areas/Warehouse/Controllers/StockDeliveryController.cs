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
using ISD.Repositories;
using ISD.Constant;

namespace Warehouse.Controllers
{
    public class StockDeliveryController : BaseController
    {
        // GET: StockDelivery
        #region Index
        public ActionResult Index()
        {
            ViewBag.PageId = GetPageId("/Warehouse/StockDelivery");
            CreateViewBagForSearch();
            return View();
        }

        public ActionResult _Search(DeliverySearchViewModel deliverySearch)
        {
            var resultList = _unitOfWork.DeliveryRepository.Search(deliverySearch);
            return PartialView(resultList);
        }

        public ActionResult _PaggingServerSide(DatatableViewModel model, DeliverySearchViewModel deliverySearch)
        {
            try
            {
                // action inside a standard controller
                int filteredResultsCount;
                int totalResultsCount = model.length;
                //Page Size 
                deliverySearch.PageSize = model.length;
                //Page Number
                deliverySearch.PageNumber = model.start / model.length + 1;

                var res = _unitOfWork.DeliveryRepository.SearchQuery(deliverySearch, out filteredResultsCount);
                if (res != null && res.Count() > 0)
                {
                    int i = model.start;
                    foreach (var item in res)
                    {
                        i++;
                        item.STT = i;
                    }
                }

                return Json(new
                {
                    draw = model.draw,
                    recordsTotal = totalResultsCount,
                    recordsFiltered = filteredResultsCount,
                    data = res,
                });
            }
            catch //(Exception ex)
            {
                return Json(null);
            }
        }
        #endregion

        #region Create
        [ISDAuthorizationAttribute]
        public ActionResult Create(Guid? ProfileId, Guid? TaskId = null, string DeliveryType = null)
        {
            var companyId = CurrentUser.CompanyId;
            var storeId = _unitOfWork.StoreRepository.GetStoreIdBySaleOrgCode(CurrentUser.SaleOrg);
            var saleEmployeeCode = CurrentUser.EmployeeCode;
            ViewBag.StockId = _unitOfWork.StockRepository.GetStockIdByStockCode(CurrentUser.SaleOrg);
            if (ProfileId.HasValue)
            {
                ViewBag.Customer = _unitOfWork.ProfileRepository.GetById(ProfileId.Value);
            }
            else
            {
                ViewBag.Customer = new ProfileViewModel();
            }
            ViewBag.StockList = _unitOfWork.StockRepository.GetStockByStore(storeId);
            CreateViewBag(companyId, null, saleEmployeeCode);
            ViewBag.TaskId = TaskId;
            ViewBag.DeliveryType = DeliveryType;
            return View();
        }
        [HttpPost]
        public JsonResult Create(DeliveryViewModel viewModel, List<DeliveryDetailViewModel> deliveryDetailList)
        {
            return ExecuteContainer(() =>
            {
                if (viewModel.ProfileId == null || viewModel.ProfileId == Guid.Empty)
                {
                    return Json(new
                    {
                        Code = HttpStatusCode.NotModified,
                        Success = false,
                        Data = "Vui lòng nhập thông tin khách hàng!"
                    });
                }
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
                if (deliveryDetailList == null || deliveryDetailList.Count == 0)
                {
                    return Json(new
                    {
                        Code = HttpStatusCode.NotModified,
                        Success = false,
                        Data = "Vui lòng nhập ít nhất 1 sản phẩm để xuất kho!"
                    });
                }
                var stockid = deliveryDetailList.First().StockId;
                var dateKeyExist = _context.DeliveryDetailModel.Where(p => p.StockId == stockid).Max(p => p.DateKey);
                if (dateKey < dateKeyExist)
                {
                    return Json(new
                    {
                        Code = HttpStatusCode.NotModified,
                        Success = false,
                        Data = "Vui lòng chọn ngày chứng từ trùng hoặc sau ngày chừng từ gần nhất!"
                    });
                }

                viewModel.DeliveryId = Guid.NewGuid();
                viewModel.CreateBy = CurrentUser.AccountId;
                viewModel.CreateTime = DateTime.Now;
                _unitOfWork.DeliveryRepository.Create(viewModel);
                //Add Stock Receiving Detail
                if (deliveryDetailList != null && deliveryDetailList.Count > 0)
                {
                    foreach (var deliveryDetailVM in deliveryDetailList)
                    {
                        deliveryDetailVM.DeliveryDetailId = Guid.NewGuid();
                        deliveryDetailVM.DeliveryId = viewModel.DeliveryId;
                        deliveryDetailVM.DateKey = dateKey;
                        _unitOfWork.DeliveryRepository.CreateDetail(deliveryDetailVM);
                    }
                }
                _context.SaveChanges();

                return Json(new
                {
                    Code = HttpStatusCode.Created,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Create_Success, LanguageResource.StockDelivery.ToLower()),
                    RedirectUrl = "/Warehouse/StockDelivery",
                    Id = viewModel.DeliveryId
                });
            });
        }
        #endregion

        #region View
        public ActionResult View(Guid id)
        {
            var delivery = _unitOfWork.DeliveryRepository.GetBy(id);
            if (delivery == null)
            {
                return RedirectToAction("ErrorText", "Error", new { area = "", statusCode = 404, exception = string.Format(LanguageResource.Error_NotExist, LanguageResource.StockReceive_Bill.ToLower()) });
            }
            ViewBag.ListDeliveryDetail = _unitOfWork.DeliveryRepository.GetDetailByDeliveryId(id);
            if (delivery.ProfileId != null)
            {
                ViewBag.Supplier = _unitOfWork.ProfileRepository.GetById((Guid)delivery.ProfileId);
            }
            else
            {
                ViewBag.Supplier = new ProfileViewModel();
            }
            var shippingTypeCodeList = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.ShippingTypeCode);
            ViewBag.ShippingTypeCode = new SelectList(shippingTypeCodeList, "CatalogCode", "CatalogText_vi", delivery.ShippingTypeCode);
            
            return View(delivery);
        }
        #endregion

        #region Edit
        [ISDAuthorizationAttribute]
        public ActionResult Edit(Guid id)
        {
            var delivery = _unitOfWork.DeliveryRepository.GetBy(id);
            if (delivery == null)
            {
                return RedirectToAction("ErrorText", "Error", new { area = "", statusCode = 404, exception = string.Format(LanguageResource.Error_NotExist, LanguageResource.StockReceive_Bill.ToLower()) });
            }
            ViewBag.ListDeliveryDetail = _unitOfWork.DeliveryRepository.GetDetailByDeliveryId(id);
            if (delivery.ProfileId != null)
            {
                ViewBag.Supplier = _unitOfWork.ProfileRepository.GetById((Guid)delivery.ProfileId);
            }
            else
            {
                ViewBag.Supplier = new ProfileViewModel();
            }
            delivery.IsEdit = true;
            var shippingTypeCodeList = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.ShippingTypeCode);
            ViewBag.ShippingTypeCode = new SelectList(shippingTypeCodeList, "CatalogCode", "CatalogText_vi",delivery.ShippingTypeCode);
            return View(delivery);
        }
        [HttpPost]
        public JsonResult Edit(DeliveryViewModel viewModel, List<DeliveryDetailViewModel> deliveryDetailList)
        {
            return ExecuteContainer(() =>
            {
                viewModel.LastEditBy = CurrentUser.AccountId;
                viewModel.LastEditTime = DateTime.Now;
                _unitOfWork.DeliveryRepository.Edit(viewModel);
                _context.SaveChanges();

                return Json(new
                {
                    Code = HttpStatusCode.Created,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Edit_Success, LanguageResource.StockDelivery.ToLower())
                });
            });
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
                var deliveryInDb = _context.DeliveryModel.FirstOrDefault(p => p.DeliveryId == id);
                if (deliveryInDb == null)
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
                    deliveryInDb.DeletedBy = CurrentUser.AccountId;
                    deliveryInDb.DeletedTime = DateTime.Now;
                    deliveryInDb.isDeleted = true;
                    deliveryInDb.DeletedReason = DeletedReason;
                    _context.Entry(deliveryInDb).State = EntityState.Modified;

                    //Xoá detail
                    var listDeliveryDb = _context.DeliveryDetailModel.Where(p => p.DeliveryId == id).ToList();
                    if (listDeliveryDb != null && listDeliveryDb.Count > 0)
                    {
                        foreach (var deliveryDetail in listDeliveryDb)
                        {
                            deliveryDetail.isDeleted = true;
                            _context.Entry(deliveryDetail).State = EntityState.Modified;
                        }
                    }
                    _context.SaveChanges();

                    return Json(new
                    {
                        Code = HttpStatusCode.Created,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Delete_Success, LanguageResource.StockDelivery.ToLower())
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
                var deliveryInDb = _context.DeliveryModel.FirstOrDefault(p => p.DeliveryId == id);
                if (deliveryInDb == null)
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
                    deliveryInDb.DeletedBy = CurrentUser.AccountId;
                    deliveryInDb.DeletedTime = DateTime.Now;
                    deliveryInDb.isDeleted = true;
                    deliveryInDb.DeletedReason = DeletedReason;
                    _context.Entry(deliveryInDb).State = EntityState.Modified;

                    //Xoá detail
                    var listDeliveryDb = _context.DeliveryDetailModel.Where(p => p.DeliveryId == id).ToList();
                    if (listDeliveryDb != null && listDeliveryDb.Count > 0)
                    {
                        foreach (var deliveryDetail in listDeliveryDb)
                        {
                            deliveryDetail.isDeleted = true;
                            _context.Entry(deliveryDetail).State = EntityState.Modified;
                        }
                    }
                    _context.SaveChanges();

                    return Json(new
                    {
                        Code = HttpStatusCode.Created,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Cancel_Success, LanguageResource.StockDelivery.ToLower())
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

            var shippingTypeCodeList = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.ShippingTypeCode);
            ViewBag.ShippingTypeCode = new SelectList(shippingTypeCodeList, "CatalogCode", "CatalogText_vi");

            //Dropdow nhà cung cấp

            //ViewBag.ProfileId = new SelectList();

            //Dropdown Nhân viên
            var saleEmployeeList = _unitOfWork.SalesEmployeeRepository.GetAllForDropdownlist();
            ViewBag.SalesEmployeeCode = new SelectList(saleEmployeeList, "SalesEmployeeCode", "SalesEmployeeName", SaleEmployeeCode);

            //Dropdown Stock
            var listStock = _unitOfWork.StockRepository.GetAllForDropdown();
            ViewBag.StockId = new SelectList(listStock, "StockId", "StockName");

            //Dropdown Product
            //ViewBag.ProductList = _productRepository.GetProInventory();
            //ViewBag.ProductId = new SelectList(productList, "ProductId", "ProductName");

            //Chỉ lấy khách hàng (B | C)
            //var customerTypeList = _catalogRepository.GetBy(ConstCatalogType.CustomerType);
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

            var catalogCategoryList = _context.View_Catalog_Category.OrderBy(p => p.OrderIndex).Select(p => new ISDSelectGuidItem()
            {
                id = p.CategoryId,
                name = p.CategoryName,
            }).ToList();

            ViewBag.SearchCategoryId = new SelectList(catalogCategoryList, "id", "name");
        }

        public ActionResult GetProductDetails(Guid DeliveryId)
        {
            var lst = _unitOfWork.DeliveryRepository.GetDetailByDeliveryId(DeliveryId);
            if (lst != null && lst.Count > 0)
            {
                int index = 0;
                foreach (var item in lst)
                {
                    index++;
                    item.ProductName = index + ". " + item.ProductCode + " | " + item.ProductName + " - SL: " + Convert.ToInt32(item.Quantity);
                }
            }
            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetSalesEmployeeInfo(string SalesEmployeeCode)
        {
            var emp = (from p in _context.SalesEmployeeModel
                       where p.SalesEmployeeCode == SalesEmployeeCode
                       select new 
                       { 
                           SalesEmployeeName = p.SalesEmployeeName, 
                           Phone = p.Phone 
                       }).FirstOrDefault();
            return Json(emp, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Product
        public ActionResult InsertProductStock(DeliveryDetailViewModel model, List<DeliveryDetailViewModel> deliveryDetailList)
        {
            if (deliveryDetailList == null)
            {
                deliveryDetailList = new List<DeliveryDetailViewModel>();
            }
            var exist = deliveryDetailList.Where(p => p.ProductId == model.ProductId).FirstOrDefault();
            if (exist != null)
            {
                exist.Quantity += model.Quantity;
            }
            else
            {
                exist = new DeliveryDetailViewModel();
                exist.Quantity = model.Quantity;
            }
            if (exist.Quantity > model.ProductQuantinyOnHand || model.ProductQuantinyOnHand == null)
            {
                return Json(new { Message = "Vui lòng nhập số lưọng xuất không vượt quá số lượng tồn!" }, JsonRequestBehavior.AllowGet);
            }
            exist.Quantity -= model.Quantity;
            deliveryDetailList.Add(model);

            return PartialView("_DeliveryDetailInner", deliveryDetailList);
        }
        public ActionResult RemoveProductStock(List<DeliveryDetailViewModel> deliveryDetailList, int STT)
        {
            deliveryDetailList = deliveryDetailList.Where(p => p.STT != STT).ToList();
            return PartialView("_DeliveryDetailInner", deliveryDetailList);
        }

        /// <summary>
        /// Search popup Profile and Contact base on Type
        /// </summary>
        /// <param name="ProfileId"></param>
        /// <param name="hasNoContact"></param>
        /// <param name="Type"></param>
        /// <returns></returns>
        public ActionResult _ProductSearch()
        {
            var catalogCategoryList = _context.View_Catalog_Category.OrderBy(p => p.OrderIndex).Select(p => new ISDSelectGuidItem()
            {
                id = p.CategoryId,
                name = p.CategoryName,
            }).ToList();

            ViewBag.CategoryId = new SelectList(catalogCategoryList, "id", "name");

            return PartialView();
        }

        [HttpPost]
        //Hàm tìm kiếm dành cho popup
        public ActionResult _ProductSearchResult(Guid? CategoryId)
        {
            return ExecuteSearch(() =>
            {
                var catalogCategoryList = _context.View_Catalog_Category.OrderBy(p => p.OrderIndex).Select(p =>  p.CategoryId).ToList();

                var productList = (from p in _context.ProductModel
                                   join ca in _context.CategoryModel on p.CategoryId equals ca.CategoryId
                                   join br in _context.CategoryModel on p.ParentCategoryId equals br.CategoryId
                                   where (CategoryId == null && catalogCategoryList.Contains(p.CategoryId.Value)) || p.CategoryId == CategoryId
                                   && p.Actived == true
                                   orderby p.ERPProductCode
                                   select new ProductViewModel()
                                   {
                                       ProductId = p.ProductId,
                                       ERPProductCode = p.ERPProductCode,
                                       ProductCode = p.ProductCode,
                                       ProductName = p.ProductName,
                                       ParentCategoryId = p.ParentCategoryId,
                                       ParentCategoryName = br.CategoryName,
                                       CategoryId = p.CategoryId,
                                       CategoryName = ca.CategoryName,
                                       //ConfigurationName = cf.ConfigurationName,
                                       ImageUrl = p.ImageUrl,
                                       OrderIndex = p.OrderIndex,
                                       Actived = p.Actived,
                                       Price = p.Price
                                   }).ToList();

                return PartialView(productList);
            });
        }
        #endregion
    }
}