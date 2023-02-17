using ISD.Constant;
using ISD.EntityModels;
using ISD.Extensions;
using ISD.Repositories;
using ISD.Resources;
using ISD.ViewModels;
using ISD.Core;
using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Collections.Generic;

namespace Maintenance.Controllers
{
    public class ProductWarrantyController : BaseController
    {
        #region Index
        // GET: ProductWarranty
        [ISDAuthorizationAttribute]
        public ActionResult Index()
        {
            ViewBag.Actived = true;

            ViewBag.PageId = GetPageId("/Maintenance/ProductWarranty");
            CreateViewBag();
            return View();
        }
        [HttpPost]
        public ActionResult _PaggingServerSide(DatatableViewModel model, ProductWarrantySearchViewModel searchViewModel)
        {
            try
            {
                // action inside a standard controller
                int filteredResultsCount;
                int totalResultsCount;

                var query = _unitOfWork.ProductWarrantyRepository.SearchQuery(searchViewModel);

                var res = CustomSearchRepository.CustomSearchFunc<ProductWarrantyViewModel>(model, out filteredResultsCount, out totalResultsCount, query, "STT");
                if (res != null && res.Count() > 0)
                {
                    int i = model.start;
                    foreach (var item in res)
                    {
                        i++;
                        item.STT = i;
                        item.Address = item.Address + item.WardName + item.DistrictName + item.ProvinceName;
                        if (item.Address.StartsWith(","))
                        {
                            item.Address = item.Address.TrimStart(',').Trim();
                        }
                    }
                }

                return Json(new
                {
                    draw = model.draw,
                    recordsTotal = totalResultsCount,
                    recordsFiltered = filteredResultsCount,
                    data = res
                });
            }
            catch //(Exception ex)
            {
                return Json(null);
            }
        }

        #endregion Index

        #region Create
        [ISDAuthorizationAttribute]
        public ActionResult Create(Guid? CopyFrom  = null)
        {
            var productWarVM = new ProductWarrantyViewModel
            {
                FromDate = DateTime.Now
            };
            CreateViewBag();
            ViewBag.Actived = true;
            ViewBag.CopyFrom = CopyFrom;
            if (CopyFrom != null)
            {
                productWarVM = _unitOfWork.ProductWarrantyRepository.GetById(CopyFrom.Value);
            }
            
            return View(productWarVM);
        }
        [HttpPost]
        public JsonResult Create(ProductWarrantyViewModel viewModel)
        {
            return ExecuteContainer(() =>
            {
                //tính time hết hạn bảo hành
                var toDate = GetToDateTime(viewModel.WarrantyId, viewModel.FromDate);
                viewModel.ToDate = toDate;
                
                //Nếu có đơn hàng và sản phẩm, check SP thuộc đơn hàng đã tồn tại chưa
                if (!string.IsNullOrEmpty(viewModel.OrderDelivery))
                {
                    var existProductWarranty = (from p in _context.ProductWarrantyModel
                                                where p.OrderDelivery == viewModel.OrderDelivery
                                                && p.ProductId == viewModel.ProductId
                                                select p).FirstOrDefault();
                    if (existProductWarranty != null)
                    {
                        return Json(new
                        {
                            Code = HttpStatusCode.NotModified,
                            Success = false,
                            Data = "Sản phẩm \"" + viewModel.ProductName + "\" thuộc phiếu giao hàng \"" + viewModel.OrderDelivery + "\" đã được đăng ký bảo hành!",
                        });
                    }
                }
                _unitOfWork.ProductWarrantyRepository.Create(viewModel);
                _context.SaveChanges();

                return Json(new
                {
                    Code = HttpStatusCode.Created,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Create_Success, LanguageResource.Maintenance_ProWarranty.ToLower()),
                });
            });
        }

        #endregion Create

        #region Edit
        [ISDAuthorizationAttribute]
        public ActionResult Edit(Guid id)
        {
            var proWarranty = _unitOfWork.ProductWarrantyRepository.GetById(id);
            if (proWarranty == null)
            {
                return RedirectToAction("ErrorText", "Error", new { area = "", statusCode = 404, exception = string.Format(LanguageResource.Error_NotExist, LanguageResource.MasterData_Company.ToLower()) });
            }
            CreateViewBag(proWarranty.WarrantyId, proWarranty.Age, proWarranty.ProvinceId, proWarranty.DistrictId, proWarranty.WardId,proWarranty.CompanyId);
            return View(proWarranty);
        }
        [HttpPost]
        public JsonResult Edit(ProductWarrantyViewModel viewModel)
        {
            return ExecuteContainer(() =>
            {

                //=> Không check nữa vì OD có thể kích hoạt theo từng số lượng => có thể trùng
                //if (!string.IsNullOrEmpty(viewModel.OrderDelivery))
                //{
                //    var existProductWarranty = (from p in _context.ProductWarrantyModel
                //                                where p.OrderDelivery == viewModel.OrderDelivery
                //                                && p.ProductId == viewModel.ProductId
                //                                select p).FirstOrDefault();
                //    if (existProductWarranty != null && existProductWarranty.ProductWarrantyId != viewModel.ProductWarrantyId)
                //    {
                //        return Json(new
                //        {
                //            Code = HttpStatusCode.NotModified,
                //            Success = false,
                //            Data = "Sản phẩm \"" + viewModel.ProductName + "\" thuộc phiếu giao hàng \"" + viewModel.OrderDelivery + "\" đã được đăng ký bảo hành!",
                //        });
                //    }
                //}
                viewModel.LastEditBy = CurrentUser.AccountId;
                viewModel.LastEditTime = DateTime.Now;
                viewModel.ToDate = GetToDateTime(viewModel.WarrantyId, viewModel.FromDate);
                _unitOfWork.ProductWarrantyRepository.Update(viewModel);
                _context.SaveChanges();

                return Json(new
                {
                    Coed = HttpStatusCode.Created,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Edit_Success, LanguageResource.Maintenance_ProWarranty.ToLower())
                });
            });
        }

        #endregion Edit

        #region Delete
        [HttpPost]
        [ISDAuthorizationAttribute]
        public ActionResult Delete(Guid id)
        {
            return ExecuteDelete(() =>
            {
                var result = _unitOfWork.ProductWarrantyRepository.Delete(id);
                if (result)
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.Created,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Delete_Success, LanguageResource.Maintenance_ProWarranty.ToLower())
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

        #endregion Delete

        #region Viewbag, Helper
        private void CreateViewBag(Guid? WarrantyId = null, string Age = null, Guid? ProvinceId = null, Guid? DistrictId = null, Guid? WardId = null, Guid? companyId =null)
        {
            //Chỉ lấy khách hàng (B | C)
            var customerTypeList = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.CustomerType);
            customerTypeList = customerTypeList.Where(p => p.CatalogCode != ConstCustomerType.Contact).ToList();
            ViewBag.CustomerTypeCode = new SelectList(customerTypeList, "CatalogCode", "CatalogText_vi");

            var warrantyList = _unitOfWork.WarrantyRepository.GetAll();
            ViewBag.WarrantyId = new SelectList(warrantyList, "WarrantyId", "WarrantyName", WarrantyId);

            //var categoryList = _context.CategoryModel.OrderBy(p => p.OrderIndex).ToList();
            //ViewBag.CategoryId = new SelectList(categoryList, "CategoryId", "CategoryName");
            //ViewBag.SearchCategoryId = new SelectList(categoryList, "CategoryId", "CategoryName");

            //Phân loại vật tư
            var PhanLoaiVatTu = _context.CategoryModel.Where(p => p.CategoryCode == ConstCategoryCode.PHANLOAIVATTU).FirstOrDefault();
            var parentCategoryList = _context.CategoryModel.Where(p => p.ParentCategoryId == PhanLoaiVatTu.CategoryId && p.Actived == true)
                                                         .Select(p => new
                                                         {
                                                             CategoryId = p.CategoryId,
                                                             CategoryCode = p.CategoryCode,
                                                             CategoryName = p.CategoryCode + " | " + p.CategoryName,
                                                             OrderIndex = p.OrderIndex
                                                         })
                                                         .OrderBy(p => p.CategoryCode).ToList();
            ViewBag.ParentCategoryId = new SelectList(parentCategoryList, "CategoryId", "CategoryName");
            ViewBag.SearchParentCategoryId = new SelectList(parentCategoryList, "CategoryId", "CategoryName");

            //Nhóm vật tư
            var NhomVatTu = _context.CategoryModel.Where(p => p.CategoryCode == ConstCategoryCode.NHOMVATTU).FirstOrDefault();
            var categoryList = _context.CategoryModel.Where(p => p.ParentCategoryId == NhomVatTu.CategoryId && p.Actived == true)
                                                         .Select(p => new
                                                         {
                                                             CategoryId = p.CategoryId,
                                                             CategoryCode = p.CategoryCode,
                                                             CategoryName = p.CategoryCode + " | " + p.CategoryName,
                                                             OrderIndex = p.OrderIndex
                                                         })
                                                         .OrderBy(p => p.CategoryCode).ToList();
            ViewBag.CategoryId = new SelectList(categoryList, "CategoryId", "CategoryName");
            ViewBag.SearchCategoryId = new SelectList(categoryList, "CategoryId", "CategoryName");

            #region //Get list Age (Độ tuổi)
            var ageList = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.Age);
            ViewBag.Age = new SelectList(ageList, "CatalogCode", "CatalogText_vi", Age);
            #endregion

            #region //Get list Province (Tỉnh/Thành phố)
            var _provinceRepository = new ProvinceRepository(_context);
            var provinceList = _provinceRepository.GetAll();
            ViewBag.ProvinceId = new SelectList(provinceList, "ProvinceId", "ProvinceName", ProvinceId);
            #endregion

            #region //Get list District (Quận/Huyện)
            var _districtRepository = new DistrictRepository(_context);
            var districtList = _districtRepository.GetBy(ProvinceId);
            ViewBag.DistrictId = new SelectList(districtList, "DistrictId", "DistrictName", DistrictId);
            #endregion

            #region //Get list Ward (Phường/Xã)
            var _wardRepository = new WardRepository(_context);
            var wardList = _wardRepository.GetBy(DistrictId);
            ViewBag.WardId = new SelectList(wardList, "WardId", "WardName", WardId);
            #endregion

            var companyList = _context.CompanyModel.OrderBy(p => p.CompanyCode).Select(p => new
                                                    {
                                                        CompanyId = p.CompanyId,
                                                        CompanyName = p.CompanyCode + " | " + p.CompanyName
                                                    }).ToList();
            ViewBag.CompanyId = new SelectList(companyList, "CompanyId", "CompanyName", companyId);
        }

        [HttpGet]
        public ActionResult ToDate(Guid WarrantyId, DateTime FromDate)
        {
            var toDate = GetToDateTime(WarrantyId, FromDate);
            var ret = string.Format("{0:yyyy-MM-dd}", toDate);
            return Json(new
            {
                Code = System.Net.HttpStatusCode.OK,
                Success = true,
                Data = ret
            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Tính ngày hết hạn bảo hành
        /// </summary>
        /// <param name="WarrantyId">WarrantyId</param>
        /// <param name="FromDate">FromDate</param>
        /// <returns>ToDate</returns>
        private DateTime GetToDateTime(Guid WarrantyId, DateTime FromDate)
        {
            var warranty = _unitOfWork.WarrantyRepository.GetWarranty(WarrantyId);
            int duration = warranty.Duration;
            var toDate = FromDate.AddMonths(duration);
            return toDate;
        }

        public ActionResult GetProfileInfo(Guid? ProfileId)
        {
            var profile = (from p in _context.ProfileModel
                           where p.ProfileId == ProfileId
                           select new ProductWarrantyViewModel()
                           {
                               Warranty_ProfileName = p.ProfileName,
                               ProfileShortName = p.ProfileShortName,
                               Address = p.Address,
                               Age = p.Age,
                               ProvinceId = p.ProvinceId,
                               DistrictId = p.DistrictId,
                               WardId = p.WardId,
                               Phone = p.Phone,
                               Email = p.Email
                           }).FirstOrDefault();
            if (profile != null)
            {
                return Json(profile, JsonRequestBehavior.AllowGet);
            }
            return Json(false);
        }
        #endregion Viewbag

        #region Check exist SerriNo
        private bool IsExistsSerriNo(string SerriNo)
        {
            return (_context.ProductWarrantyModel.FirstOrDefault(p => p.SerriNo == SerriNo) != null);
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult CheckExistingSerriNo(string SerriNo, string SerriNoValid)
        {
            try
            {
                if (SerriNoValid != SerriNo)
                {
                    return Json(!IsExistsSerriNo(SerriNo));
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


        #region autocomplete
        /// <summary>
        /// Lấy danh sách số serial
        /// </summary>
        /// <returns></returns>
        public ActionResult SearchSerialNumberForAutocomple(string search)
        {
            var result = new List<ISDSelectItem2>();
            result = (from p in _context.ProductWarrantyModel
                      where p.Actived == true
                      && (search == null || p.SerriNo.Contains(search))
                      orderby p.SerriNo
                      select new ISDSelectItem2
                      {
                          text = p.SerriNo,
                          value = p.SerriNo
                      }).Distinct().Take(10).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Lấy danh sách số SaleOrder
        /// </summary>
        /// <returns></returns>
        public ActionResult SearchSaleOrderForAutocomple(string search)
        {
            var result = new List<ISDSelectItem2>();
            result = (from p in _context.ProductWarrantyModel
                      where p.Actived == true
                      && (search != null && p.SaleOrder.Contains(search))
                      orderby p.SaleOrder
                      select new ISDSelectItem2
                      {
                          text = p.SaleOrder,
                          value = p.SaleOrder
                      }).Distinct().Take(10).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Lấy danh sách số OrderDelivery
        /// </summary>
        /// <returns></returns>
        public ActionResult SearchOrderDeliveryForAutocomple(string search)
        {
            var result = new List<ISDSelectItem2>();
            result = (from p in _context.ProductWarrantyModel
                      where p.Actived == true
                      && (search == null || p.OrderDelivery.Contains(search))
                      orderby p.OrderDelivery
                      select new ISDSelectItem2
                      {
                          text = p.OrderDelivery,
                          value = p.OrderDelivery
                      }).Distinct().Take(10).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}