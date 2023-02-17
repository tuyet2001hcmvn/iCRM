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
    public class StockTransferController : BaseController
    {
        #region Index
        // GET: StockTransfer
        public ActionResult Index()
        {
            ViewBag.PageId = GetPageId("/Warehouse/StockTransfer");
            CreateViewBagForSearch();
            return View();
        }
        public ActionResult _Search(TransferSearchViewModel searchViewModel)
        {
            var listTranfer = _unitOfWork.TransferRepository.Search(searchViewModel);
            return PartialView(listTranfer);
        }
        #endregion

        #region Create
        [ISDAuthorizationAttribute]
        public ActionResult Create()
        {
            var companyId = CurrentUser.CompanyId;
            var storeId = _unitOfWork.StoreRepository.GetStoreIdBySaleOrgCode(CurrentUser.SaleOrg);
            var employeeCode = CurrentUser.EmployeeCode;
            CreateViewBag(companyId, storeId, employeeCode);
            ViewBag.StockList = _unitOfWork.StockRepository.GetAll();
            ViewBag.ViewType = "Transfer";
            return View();
        }
        [HttpPost]
        public JsonResult Create(TransferViewModel viewModel, List<TransferDetailViewModel> transferDetailList)
        {
            return ExecuteContainer(() =>
            {
                viewModel.TransferId = Guid.NewGuid();
                viewModel.CreateBy = CurrentUser.AccountId;
                viewModel.CreateTime = DateTime.Now;
                _unitOfWork.TransferRepository.Create(viewModel);

                var dateKey = _unitOfWork.UtilitiesRepository.ConvertDateTimeToInt(viewModel.DocumentDate);
                if (transferDetailList != null && transferDetailList.Count > 0)
                {
                    //group kho chuyen xuat, sum sl product xuat
                    var dataForCheckStock = from p in transferDetailList
                                            group p by new { p.FromStockId, p.ProductId } into tmpList
                                            select new
                                            {
                                                tmpList.Key.FromStockId,
                                                tmpList.Key.ProductId,
                                                Sum = tmpList.Sum(p => p.Quantity),
                                            };
                    //check ton kho
                    foreach (var item in dataForCheckStock)
                    {
                        var stockId = (Guid)item.FromStockId;
                        var productId = (Guid)item.ProductId;
                        var quantyOnHand = _unitOfWork.StockRepository.GetStockOnHandBy(stockId, productId).Qty;
                        if (item.Sum > quantyOnHand)
                        {
                            return Json(new
                            {
                                Code = HttpStatusCode.NotModified,
                                Success = false,
                                Data = "Đã xảy ra lỗi: Số lượng chuyển không thể lớn hơn số lượng tồn kho!"
                            });
                        }
                    }

                    //them du lieu
                    foreach (var transferDetailVM in transferDetailList)
                    {
                        //Thêm mới detail
                        transferDetailVM.TransferDetailId = Guid.NewGuid();
                        transferDetailVM.TransferId = viewModel.TransferId;
                        transferDetailVM.DateKey = dateKey;
                        _unitOfWork.TransferDetailRepository.Create(transferDetailVM);
                    }
                }
                else
                {
                    return Json(new
                    {
                        Code = HttpStatusCode.NotModified,
                        Success = false,
                        Data = string.Format("Vui lòng nhập ít nhất 1 dòng dữ liệu sản phẩm để thực hiện chuyển kho!")
                    });
                }
                _context.SaveChanges();
                return Json(new
                {
                    Code = HttpStatusCode.Created,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Create_Success, LanguageResource.TransferMaterial.ToLower()),
                    Id = viewModel.TransferId,
                });
            });
        }
        #endregion

        #region View
        public ActionResult View(Guid id)
        {
            var stockReceive = _unitOfWork.TransferRepository.GetBy(id);
            if (stockReceive == null)
            {
                return RedirectToAction("ErrorText", "Error", new { area = "", statusCode = 404, exception = string.Format(LanguageResource.Error_NotExist, LanguageResource.StockTransfer_Bill.ToLower()) });
            }
            ViewBag.ListTransferDetail = _unitOfWork.TransferDetailRepository.GetBy(id);
            ViewBag.ViewType = "Transfer";
            return View(stockReceive);
        }
        #endregion

        #region Copy
        public ActionResult Copy(Guid id)
        {
            var stockTransfer = _unitOfWork.TransferRepository.GetBy(id);
            if (stockTransfer == null)
            {
                return RedirectToAction("ErrorText", "Error", new { area = "", statusCode = 404, exception = string.Format(LanguageResource.Error_NotExist, LanguageResource.StockTransfer_Bill.ToLower()) });
            }
            CreateViewBag(stockTransfer.CompanyId, stockTransfer.StoreId, stockTransfer.SalesEmployeeCode);
            stockTransfer.transferDetail = _unitOfWork.TransferDetailRepository.GetBy(id);
            stockTransfer.transferSender = new DeliveryViewModel()
            {
                SenderName = stockTransfer.SenderName,
                SenderPhone = stockTransfer.SenderPhone,
                SenderAddress = stockTransfer.SenderAddress,

                RecipientCompany = stockTransfer.RecipientCompany,
                RecipientName = stockTransfer.RecipientName,
                RecipientAddress = stockTransfer.RecipientAddress,
                RecipientPhone = stockTransfer.RecipientPhone,
            };
            ViewBag.ViewType = "Transfer";
            ViewBag.Action = "Copy";
            return View("Create", stockTransfer);
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
                var TransferInDb = _context.TransferModel.FirstOrDefault(p => p.TransferId == id);
                if (TransferInDb == null)
                {
                    return Json(new
                    {
                        Code = HttpStatusCode.NotFound,
                        Success = false,
                        Data = ""
                    });
                }
                TransferInDb.DeletedBy = CurrentUser.AccountId;
                TransferInDb.DeletedTime = DateTime.Now;
                TransferInDb.isDeleted = true;
                TransferInDb.DeletedReason = DeletedReason;
                _context.Entry(TransferInDb).State = EntityState.Modified;

                //Xóa detail
                var ListTransferDetailDb = _context.TransferDetailModel.Where(p => p.TransferId == id).ToList();
                if (ListTransferDetailDb != null && ListTransferDetailDb.Count > 0)
                {
                    foreach (var transferDetail in ListTransferDetailDb)
                    {
                        transferDetail.isDeleted = true;
                        _context.Entry(transferDetail).State = EntityState.Modified;
                    }
                }
                _context.SaveChanges();

                return Json(new
                {
                    Code = HttpStatusCode.Created,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Delete_Success, LanguageResource.TransferMaterial.ToLower())
                });
            });
        }
        #endregion
        
        #region Delete
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
                var TransferInDb = _context.TransferModel.FirstOrDefault(p => p.TransferId == id);
                if (TransferInDb == null)
                {
                    return Json(new
                    {
                        Code = HttpStatusCode.NotFound,
                        Success = false,
                        Data = string.Format(LanguageResource.Alert_NotExist_Cancel, LanguageResource.TransferMaterial.ToLower())
                    });
                }
                TransferInDb.DeletedBy = CurrentUser.AccountId;
                TransferInDb.DeletedTime = DateTime.Now;
                TransferInDb.isDeleted = true;
                TransferInDb.DeletedReason = DeletedReason;
                _context.Entry(TransferInDb).State = EntityState.Modified;

                //Xóa detail
                var ListTransferDetailDb = _context.TransferDetailModel.Where(p => p.TransferId == id).ToList();
                if (ListTransferDetailDb != null && ListTransferDetailDb.Count > 0)
                {
                    foreach (var transferDetail in ListTransferDetailDb)
                    {
                        transferDetail.isDeleted = true;
                        _context.Entry(transferDetail).State = EntityState.Modified;
                    }
                }
                _context.SaveChanges();

                return Json(new
                {
                    Code = HttpStatusCode.Created,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Cancel_Success, LanguageResource.TransferMaterial.ToLower())
                });
            });
        }
        #endregion

        #region Product
        public ActionResult InsertProductStock(TransferDetailViewModel model, List<TransferDetailViewModel> transferDetailList)
        {
            if (transferDetailList == null)
            {
                transferDetailList = new List<TransferDetailViewModel>();
            }
            var exist = transferDetailList.Where(p => p.ProductId == model.ProductId).FirstOrDefault();
            if (exist != null)
            {
                exist.Quantity += model.Quantity;
            }
            else
            {
                exist = new TransferDetailViewModel();
                exist.Quantity = model.Quantity;
            }
            if (exist.Quantity > model.QuantinyOnHand || model.QuantinyOnHand == null)
            {
                return Json(new { Message = "Vui lòng nhập số lưọng chuyển kho không vượt quá số lượng tồn!" }, JsonRequestBehavior.AllowGet);
            }
            exist.Quantity -= model.Quantity;
            transferDetailList.Add(model);

            return PartialView("_TransferDetailInner", transferDetailList);
        }
        public ActionResult RemoveProductStock(List<TransferDetailViewModel> transferDetailList, int STT)
        {
            transferDetailList = transferDetailList.Where(p => p.STT != STT).ToList();
            return PartialView("_TransferDetailInner", transferDetailList);
        }
        #endregion

        #region VewBag, Helper
        private void CreateViewBag(Guid? CompanyId = null, Guid? StoreId = null, string SaleEmployeeCode = "")
        {
            //Dropdown Company
            var companyList = _unitOfWork.CompanyRepository.GetAll();
            ViewBag.CompanyId = new SelectList(companyList, "CompanyId", "CompanyName", CompanyId);

            //Dropdown Store
            var storeList = _unitOfWork.StoreRepository.GetStoreByCompany(CompanyId);
            ViewBag.StoreId = new SelectList(storeList, "StoreId", "StoreName", StoreId);


            //Dropdown Nhân viên
            var saleEmployeeList = _unitOfWork.SalesEmployeeRepository.GetAllForDropdownlist();
            ViewBag.SalesEmployeeCode = new SelectList(saleEmployeeList, "SalesEmployeeCode", "SalesEmployeeName", SaleEmployeeCode);


            //Dropdown Product
            //ViewBag.ProductList = _productRepository.GetProInventory();
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
            ViewBag.SearchFromStockId = new SelectList(listStock, "StockId", "StockName");
            ViewBag.SearchToStockId = new SelectList(listStock, "StockId", "StockName");
        }

        public ActionResult GetProducOnHand(Guid ProductId, Guid StockId)
        {
            var result = _unitOfWork.StockRepository.GetStockOnHandBy((Guid)StockId, ProductId);

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}