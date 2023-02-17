using AutoMapper;
using ISD.Constant;
using ISD.EntityModels;
using ISD.Extensions;
using ISD.Core;
using ISD.Core.ActionFilters;
using ISD.Repositories;
using ISD.Resources;
using ISD.ViewModels;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MasterData.Controllers
{
    public class StoreController : BaseController
    {
        // GET: Store
        #region Index
        [ISDAuthorizationAttribute]
        public ActionResult Index()
        {
            //Get list Company
            var compList = _context.CompanyModel.Where(p => p.Actived == true).OrderBy(p => p.OrderIndex).ToList();
            ViewBag.CompanyId = new SelectList(compList, "CompanyId", "CompanyName");

            //Get provinces
            var provinceList = _context.ProvinceModel.Where(p => p.Actived == true)
                                                     .OrderBy(p => p.Area).ThenBy(p => p.ProvinceName)
                                                     .ToList();
            ViewBag.ProvinceId = new SelectList(provinceList, "ProvinceId", "ProvinceName");

            //Nguồn khách hàng
            //Get customer source
            var customerSourceList = _context.CatalogModel.Where(p => p.Actived == true && p.CatalogTypeCode == ConstCatalogType.CustomerSource)
                                                .Select(p => new CatalogViewModel
                                                {
                                                    CatalogCode = p.CatalogCode,
                                                    CatalogText_vi = p.CatalogCode + " | " + p.CatalogText_vi,
                                                }).ToList();
            ViewBag.DefaultCustomerSource = new SelectList(customerSourceList, "CatalogCode", "CatalogText_vi");

            return View();
        }

        public ActionResult _Search(Guid? CompanyId = null, string StoreName = "", string SaleOrgCode = "", Guid? ProvinceId = null, bool? Actived = null, string DefaultCustomerSource = "")
        {
            return ExecuteSearch(() =>
            {
                var store = (from p in _context.StoreModel
                             join c in _context.CompanyModel on p.CompanyId equals c.CompanyId
                             join province in _context.ProvinceModel on p.ProvinceId equals province.ProvinceId into pG
                             from pGroup in pG.DefaultIfEmpty()
                             orderby c.OrderIndex, p.OrderIndex
                             where
                             //search by ParentCategoryId
                             (CompanyId == null || p.CompanyId == CompanyId)
                             //search by CategoryName
                             && (StoreName == "" || p.StoreName.Contains(StoreName))
                             //search by SaleOrgCode
                             && (SaleOrgCode == "" || p.SaleOrgCode.Contains(SaleOrgCode))
                             //search by ProvinceId
                             && (ProvinceId == null || p.ProvinceId == ProvinceId)
                             && (DefaultCustomerSource == "" || p.DefaultCustomerSource == DefaultCustomerSource)
                             //search by Actived
                             && (Actived == null || p.Actived == Actived)
                             select new StoreViewModel()
                             {
                                 CompanyName = c.CompanyName,
                                 StoreId = p.StoreId,
                                 //StoreCode = p.StoreCode,
                                 SaleOrgCode = p.SaleOrgCode,
                                 //SaleOrgCode_KetToan = p.SaleOrgCode_KetToan,
                                 StoreName = p.StoreName,
                                 CompanyId = p.CompanyId,
                                 StoreAddress = p.StoreAddress,
                                 Actived = p.Actived,
                                 ProvinceName = pGroup.ProvinceName,
                             })
                             .ToList();

                return PartialView(store);
            });
        }
        #endregion

        //GET: /Store/Create
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
        public JsonResult Create(StoreViewModel model, HttpPostedFileBase LogoUrl, HttpPostedFileBase ImageUrl)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<StoreViewModel, StoreModel>()
                    .ForMember(x => x.AccountModel, opt => opt.Ignore())
                    .ForMember(x => x.MaterialMinMaxPriceModel, opt => opt.Ignore())
                    .ForMember(dest => dest.LogoUrl, opt => opt.Condition(src => src.LogoUrl != null))
                    .ForMember(dest => dest.ImageUrl, opt => opt.Condition(src => src.ImageUrl != null));
                cfg.ValidateInlineMaps = false;
                //AccountModel
                //MaterialMinMaxPriceModel
            });

            IMapper mapper = config.CreateMapper();
            return ExecuteContainer(() =>
            {
                var storeNew = new StoreModel();
                mapper.Map(model, storeNew);
                storeNew.StoreId = Guid.NewGuid();

                if (LogoUrl != null)
                {
                    model.LogoUrl = Upload(LogoUrl, "Store/Logo");
                }
                if (ImageUrl != null)
                {
                    model.ImageUrl = Upload(ImageUrl, "Store/Image");
                }

                // Area
                var Area = _unitOfWork.ProvinceRepository.GetAreaByProvince(model.ProvinceId);
                storeNew.Area = Area;

                //Kho
                var stock = new Stock_Store_Mapping
                {
                    StoreId = storeNew.StoreId,
                    StockId = (Guid)model.StockId,
                    isMain = true
                };
                _context.Entry(stock).State = EntityState.Added;
                _context.Entry(storeNew).State = EntityState.Added;
                _context.SaveChanges();

                return Json(new
                {
                    Code = System.Net.HttpStatusCode.Created,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Create_Success, LanguageResource.MasterData_Store.ToLower())
                });
            });
        }
        #endregion

        //GET: /Store/Edit
        #region Edit
        [ISDAuthorizationAttribute]
        [ApplicationLog("Edit Store {id}")]
        public ActionResult Edit(Guid id)
        {
            var store = (from p in _context.StoreModel
                         join c in _context.CompanyModel on p.CompanyId equals c.CompanyId
                         join s in _context.Stock_Store_Mapping on p.StoreId equals s.StoreId into tmp
                         from st in tmp.DefaultIfEmpty()
                         orderby p.CompanyId, p.OrderIndex
                         where p.StoreId == id
                         select new StoreViewModel()
                         {
                             CompanyName = c.CompanyName,
                             StoreId = p.StoreId,
                             //StoreCode = p.StoreCode,
                             SaleOrgCode = p.SaleOrgCode,
                             //SaleOrgCode_KetToan = p.SaleOrgCode_KetToan,
                             StoreName = p.StoreName,
                             InvoiceStoreName = p.InvoiceStoreName,
                             CompanyId = p.CompanyId,
                             StoreTypeId = p.StoreTypeId,
                             StoreAddress = p.StoreAddress,
                             ProvinceId = p.ProvinceId,
                             DistrictId = p.DistrictId,
                             TelProduct = p.TelProduct,
                             TelService = p.TelService,
                             ImageUrl = p.ImageUrl,
                             LogoUrl = p.LogoUrl,
                             OrderIndex = p.OrderIndex,
                             Actived = p.Actived,
                             mLat = p.mLat,
                             mLong = p.mLong,
                             Fax = p.Fax,
                             StockId = st.StockId,
                             DefaultCustomerSource = p.DefaultCustomerSource,
                             SMSTemplateCode = p.SMSTemplateCode,
                             Email = p.Email,
                         })
                         .FirstOrDefault();
            if (store == null)
            {
                return RedirectToAction("ErrorText", "Error", new { area = "", statusCode = 404, exception = string.Format(LanguageResource.Error_NotExist, LanguageResource.MasterData_Store.ToLower()) });
            }
            CreateViewBag(store.StoreTypeId, store.ProvinceId, store.DistrictId, store.StockId, store.DefaultCustomerSource, store.SMSTemplateCode);
            return View(store);
        }
        //POST: Edit
        [HttpPost]
        [ISDAuthorizationAttribute]
        public JsonResult Edit(StoreViewModel model, HttpPostedFileBase LogoUrl, HttpPostedFileBase ImageUrl, string PaymentMethod_Cash)
        {
            // Fix lỗi Edit bị mất hình: nguyên nhân do khi null nó gán null vào nên mất hình
            // Tien: sử dụng AutoMapper để khỏi gán từng cái
            // Nếu LogoUrl khác null thì mới map từ model vào modelDb
            // Nếu ImageUrl khác null thì mới map từ model vào modelDb

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<StoreViewModel, StoreModel>()
                    .ForMember(x => x.AccountModel, opt => opt.Ignore())
                    .ForMember(x => x.MaterialMinMaxPriceModel, opt => opt.Ignore())
                    .ForMember(dest => dest.LogoUrl, opt => opt.Condition(src => src.LogoUrl != null))
                    .ForMember(dest => dest.ImageUrl, opt => opt.Condition(src => src.ImageUrl != null));
                cfg.ValidateInlineMaps = false;
                //AccountModel
                //MaterialMinMaxPriceModel
            });

            IMapper mapper = config.CreateMapper();
            return ExecuteContainer(() =>
            {
                var modelDb = _context.StoreModel.FirstOrDefault(p => p.StoreId == model.StoreId);
                if (modelDb != null)
                {
                    //modelDb = mapper.Map<StoreModel>(model);
                    mapper.Map(model, modelDb);
                }
                if (LogoUrl != null)
                {
                    modelDb.LogoUrl = Upload(LogoUrl, "Store/Logo");
                }
                if (ImageUrl != null)
                {
                    modelDb.ImageUrl = Upload(ImageUrl, "Store/Image");
                }
                // 1 chinh nhánh - 1 kho
                var ret = _context.Stock_Store_Mapping.Where(p => p.StoreId == model.StoreId).FirstOrDefault();
                if (ret == null)
                {
                    var storeStock = new Stock_Store_Mapping
                    {
                        StoreId = model.StoreId,
                        StockId = (Guid)model.StockId,
                        isMain = true
                    };
                    _context.Entry(storeStock).State = EntityState.Added;
                }
                else
                {
                    var stockDb = _context.Stock_Store_Mapping.FirstOrDefault(p => p.StoreId == model.StoreId);
                    //Nếu khác mới làm
                    if (stockDb != null && stockDb.StockId != (Guid)model.StockId)
                    {
                        //Nếu có chọn => add thêm vào db
                        if (model.StockId != null)
                        {
                            var newStockDB = new Stock_Store_Mapping()
                            {
                                StoreId = model.StoreId,
                                StockId = (Guid)model.StockId,
                                isMain = true
                            };
                            _context.Entry(stockDb).State = EntityState.Deleted;
                            _context.Entry(newStockDB).State = EntityState.Added;
                        }
                        //Nếu không chọn xóa bỏ cái cũ
                        else
                        {
                            _context.Entry(stockDb).State = EntityState.Deleted;
                        }
                    }
                }

                var Area = _unitOfWork.ProvinceRepository.GetAreaByProvince(model.ProvinceId);
                modelDb.Area = Area;

                _context.Entry(modelDb).State = EntityState.Modified;
                _context.SaveChanges();

                return Json(new
                {
                    Code = System.Net.HttpStatusCode.Created,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Edit_Success, LanguageResource.MasterData_Store.ToLower())
                });
            });
        }
        #endregion
        /*
        #region Tài khoản
        public ActionResult _PaymentMethodByStore(string SaleOrgCode)
        {
            var paymentList = (from p in _context.PaymentMethodModel
                               where p.SaleOrg == SaleOrgCode
                               orderby p.PaymentMethodCode
                               select new PaymentMethodViewModel()
                               {
                                   PaymentMethodId = p.PaymentMethodId,
                                   PaymentMethodCode = p.PaymentMethodCode,
                                   PaymentMethodAccount = p.PaymentMethodAccount,
                                   PaymentMethodType = p.PaymentMethodType,
                                   Actived = p.Actived
                               }).ToList();
            if (paymentList == null)
            {
                paymentList = new List<PaymentMethodViewModel>();
            }
            ViewBag.Account_Cash = paymentList.Where(p => p.PaymentMethodType == ConstPaymentMethodByStore.TienMat)
                                              .Select(p => p.PaymentMethodAccount).FirstOrDefault();
            return PartialView(paymentList);
        }
        //Partial List Chuyển khoản
        public ActionResult _Transfer(List<PaymentMethodViewModel> paymentList)
        {
            var transferList = paymentList.Where(p => p.PaymentMethodType == ConstPaymentMethodByStore.ChuyenKhoan)
                                          .ToList();
            return PartialView(transferList);
        }
        //Partial List Trả góp
        public ActionResult _Installment(List<PaymentMethodViewModel> paymentList)
        {
            var installmentList = paymentList.Where(p => p.PaymentMethodType == ConstPaymentMethodByStore.TraGop)
                                             .ToList();
            return PartialView(installmentList);
        }
        //Modal popup thêm sửa
        public ActionResult _PaymentMethodModal(Guid? PaymentMethodId, int? PaymentMethodType)
        {
            var payment = (from p in _context.PaymentMethodModel
                           where p.PaymentMethodId == PaymentMethodId
                           select new PaymentMethodViewModel()
                           {
                               PaymentMethodId = p.PaymentMethodId,
                               PaymentMethodCode = p.PaymentMethodCode,
                               PaymentMethodAccount = p.PaymentMethodAccount,
                               PaymentMethodType = p.PaymentMethodType,
                               Actived = p.Actived
                           }).FirstOrDefault();
            if (payment == null)
            {
                payment = new PaymentMethodViewModel();
                payment.PaymentMethodType = PaymentMethodType;
            }
            return PartialView(payment);
        }
        public ActionResult UpdatePaymentMethod(Guid? PaymentMethodId, string PaymentMethodCode, string SaleOrg, int? PaymentMethodType, string PaymentMethodAccount, bool? Actived)
        {
            //Insert
            if (PaymentMethodId == null || PaymentMethodId == Guid.Empty)
            {
                PaymentMethodModel model = new PaymentMethodModel();
                model.PaymentMethodId = Guid.NewGuid();
                model.PaymentMethodCode = PaymentMethodCode;
                model.PaymentMethodAccount = PaymentMethodAccount;
                model.PaymentMethodType = PaymentMethodType;
                model.SaleOrg = SaleOrg;
                model.Actived = Actived;
                model.CreatedUser = CurrentUser.UserName;
                model.CreatedTime = DateTime.Now;
                _context.Entry(model).State = EntityState.Added;
            }
            //Update
            else
            {
                var payment = _context.PaymentMethodModel.FirstOrDefault(p => p.PaymentMethodId == PaymentMethodId);
                if (payment != null)
                {
                    payment.PaymentMethodCode = PaymentMethodCode;
                    payment.PaymentMethodAccount = PaymentMethodAccount;
                    payment.PaymentMethodType = PaymentMethodType;
                    payment.Actived = Actived;
                    payment.LastModifiedUser = CurrentUser.UserName;
                    payment.LastModifiedTime = DateTime.Now;
                    _context.Entry(payment).State = EntityState.Modified;
                }
            }
            _context.SaveChanges();

            var paymentList = (from p in _context.PaymentMethodModel
                               where p.SaleOrg == SaleOrg
                               orderby p.PaymentMethodCode
                               select new PaymentMethodViewModel()
                               {
                                   PaymentMethodId = p.PaymentMethodId,
                                   PaymentMethodCode = p.PaymentMethodCode,
                                   PaymentMethodAccount = p.PaymentMethodAccount,
                                   PaymentMethodType = p.PaymentMethodType,
                                   Actived = p.Actived
                               }).ToList();
            if (paymentList == null)
            {
                paymentList = new List<PaymentMethodViewModel>();
            }
            if (PaymentMethodType == ConstPaymentMethodByStore.ChuyenKhoan)
            {
                return PartialView("_Transfer", paymentList.Where(p => p.PaymentMethodType == ConstPaymentMethodByStore.ChuyenKhoan).ToList());
            }
            if (PaymentMethodType == ConstPaymentMethodByStore.TraGop)
            {
                return PartialView("_Installment", paymentList.Where(p => p.PaymentMethodType == ConstPaymentMethodByStore.TraGop).ToList());
            }
            return PartialView();
        }
        #endregion
        */
        //GET: /Store/Delete
        #region Delete
        [HttpPost]
        [ISDAuthorizationAttribute]
        public ActionResult Delete(Guid id)
        {
            return ExecuteDelete(() =>
            {
                var store = _context.StoreModel.FirstOrDefault(p => p.StoreId == id);
                if (store != null)
                {
                    if (store.AccountModel != null && store.AccountModel.Count > 0)
                    {
                        store.AccountModel.Clear();
                    }
                    _context.Entry(store).State = EntityState.Deleted;
                    _context.SaveChanges();

                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.Created,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Delete_Success, LanguageResource.MasterData_Store.ToLower())
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
        //private bool IsExists(string StoreCode)
        //{
        //    return (_context.StoreModel.FirstOrDefault(p => p.StoreCode == StoreCode) != null);
        //}
        //[AllowAnonymous]
        //[HttpPost]
        //public ActionResult CheckExistingStoreCode(string StoreCode, string StoreCodeValid)
        //{
        //    try
        //    {
        //        if (StoreCodeValid != StoreCode)
        //        {
        //            return Json(!IsExists(StoreCode));
        //        }
        //        else
        //        {
        //            return Json(true);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(false);
        //    }
        //}
        #endregion

        #region CreateViewBag, Helper
        public void CreateViewBag(Guid? StoreTypeId = null, Guid? ProvinceId = null, Guid? DistrictId = null, Guid? StockId = null, string DefaultCustomerSource = null, string SMSTemplateCode = null)
        {
            //Get list StoreType
            var storeTypeList = _context.StoreTypeModel.Where(p => p.Actived == true).OrderBy(p => p.OrderIndex).ToList();
            ViewBag.StoreTypeId = new SelectList(storeTypeList, "StoreTypeId", "StoreTypeName", StoreTypeId);

            //Get list Company
            var compList = _context.CompanyModel.Where(p => p.Actived == true).OrderBy(p => p.OrderIndex).ToList();
            ViewBag.CompanyId = new SelectList(compList, "CompanyId", "CompanyName");

            //Get list Province
            var provinceList = _context.ProvinceModel.Where(p => p.Actived == true)
                                                     .OrderBy(p => p.Area)
                                                     .ThenBy(p => p.ProvinceName).ToList();
            ViewBag.ProvinceId = new SelectList(provinceList, "ProvinceId", "ProvinceName", ProvinceId);

            //Get list District
            var districtList = _context.DistrictModel.Where(p => p.Actived == true && (ProvinceId == null || p.ProvinceId == ProvinceId))
                                                     .Select(p => new ISD.ViewModels.DistrictViewModel()
                                                     {
                                                         DistrictId = p.DistrictId,
                                                         DistrictName = p.Appellation + " " + p.DistrictName,
                                                         OrderIndex = p.OrderIndex
                                                     })
                                                     .OrderBy(p => p.OrderIndex).ToList();
            ViewBag.DistrictId = new SelectList(districtList, "DistrictId", "DistrictName", DistrictId);

            //Get list Stock
            var stockList = _context.StockModel.Where(p => p.Actived == true)
                                                .Select(p => new StockViewModel
                                                {
                                                    StockId = p.StockId,
                                                    StockName = p.StockCode + " | " + p.StockName,
                                                }).ToList();
            ViewBag.StockId = new SelectList(stockList, "StockId", "StockName", StockId);

            //Get customer source
            var customerSourceList = _context.CatalogModel.Where(p => p.Actived == true && p.CatalogTypeCode == ConstCatalogType.CustomerSource)
                                                .Select(p => new CatalogViewModel
                                                {
                                                    CatalogCode = p.CatalogCode,
                                                    CatalogText_vi = p.CatalogCode + " | " + p.CatalogText_vi,
                                                }).ToList();
            ViewBag.DefaultCustomerSource = new SelectList(customerSourceList, "CatalogCode", "CatalogText_vi", DefaultCustomerSource);

            //Get sms template
            var smsTemplateList = _context.CatalogModel.Where(p => p.Actived == true && p.CatalogTypeCode == ConstCatalogType.SMSTemplate)
                                               .Select(p => new CatalogViewModel
                                               {
                                                   CatalogCode = p.CatalogCode,
                                                   CatalogText_vi = p.CatalogCode + " | " + p.CatalogText_vi,
                                               }).ToList();
            ViewBag.SMSTemplateCode = new SelectList(smsTemplateList, "CatalogCode", "CatalogText_vi", SMSTemplateCode);
        }

        //GetDistrictBy
        public ActionResult GetDistrictBy(Guid? ProvinceId)
        {
            var districtList = _context.DistrictModel.Where(p => p.Actived == true && p.ProvinceId == ProvinceId)
                                                     .Select(p => new ISD.ViewModels.DistrictViewModel()
                                                     {
                                                         DistrictId = p.DistrictId,
                                                         DistrictName = p.Appellation + " " + p.DistrictName,
                                                         OrderIndex = p.OrderIndex
                                                     })
                                                     .OrderBy(p => p.OrderIndex).ToList();
            var lst = new SelectList(districtList, "DistrictId", "DistrictName");
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        #endregion

        public JsonResult GetStoreByCompany(Guid? CompanyId, bool IfCompanyIdNullGetAll = false)
        {
            var listStore = _unitOfWork.StoreRepository.GetStoreByCompany(CompanyId, CurrentUser.isViewByStore, CurrentUser.AccountId, IfCompanyIdNullGetAll);
            var slStore = new SelectList(listStore, "StoreId", "StoreName");
            return Json(slStore, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetStoreByCompany_ViewPermission(Guid? CompanyId)
        {
            var listStore = _unitOfWork.StoreRepository.GetStoreByViewPermission(CurrentUser.AccountId, CompanyId);
            var slStore = new SelectList(listStore, "StoreId", "StoreName");
            return Json(slStore, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllStoreForDropDown()
        {
            var listStore = _unitOfWork.StoreRepository.GetAllStore();
            var slStore = new SelectList(listStore, "StoreId", "StoreName");
            return Json(slStore, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetStoreByCustomerSource(string CustomerSourceCode)
        {
            var listStore = _unitOfWork.StoreRepository.GetStoreByCustomerSource(CustomerSourceCode);
            var slStore = new SelectList(listStore, "StoreId", "StoreName");
            return Json(slStore, JsonRequestBehavior.AllowGet);
        }
    }
}