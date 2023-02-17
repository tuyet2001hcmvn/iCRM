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
using System.Web.Services;

namespace Sale.Controllers
{
    public class PlateFeeController : BaseController
    {
        // GET: PlateFee
        #region Index
        [ISDAuthorizationAttribute]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult _Search(string PlateFeeName = "", bool? Actived = null)
        {
            return ExecuteSearch(() =>
            {
                var list = (from p in _context.PlateFeeModel
                            orderby p.CreatedDate descending
                            where
                            //search by PlateFeeName
                            (PlateFeeName == "" || p.PlateFeeName.Contains(PlateFeeName))
                            //search by Actived
                            && (Actived == null || p.Actived == Actived)
                            select new Search_PlateFeeViewModel()
                            {
                                PlateFeeId = p.PlateFeeId,
                                PlateFeeCode = p.PlateFeeCode,
                                PlateFeeName = p.PlateFeeName,
                                Actived = p.Actived
                            }).ToList();

                foreach (var item in list)
                {
                    var productList = (from p in _context.PlateFeeModel
                                       from pr in p.ProductModel
                                       where p.PlateFeeId == item.PlateFeeId
                                       select pr.ProductId).ToList();
                    item.NumberOfProduct = productList.Count;
                }

                return PartialView(list);
            });
        }
        #endregion

        //GET: /PlateFee/Create
        #region Create
        [ISDAuthorizationAttribute]
        public ActionResult Create()
        {
            CreateViewBag();
            return View();
        }
        [HttpPost]
        [ValidateAjax]
        [ValidateInput(false)] //need when using ckeditor, do not delete
        [ISDAuthorizationAttribute]
        public ActionResult Create(PlateFeeViewModel viewModel, List<PlateFeeDetailModel> detailList, List<Detail_PeriodicallyCheckingViewModel> chosenList)
        {
            return ExecuteContainer(() =>
            {
                PlateFeeModel model = new PlateFeeModel();
                model.PlateFeeId = Guid.NewGuid();
                model.PlateFeeCode = viewModel.PlateFeeCode;
                model.PlateFeeName = viewModel.PlateFeeName;
                model.Description = viewModel.Description;
                model.Actived = viewModel.Actived;
                model.CreatedUser = CurrentUser.UserName;
                model.CreatedDate = DateTime.Now;

                if (detailList != null)
                {
                    foreach (var item in detailList)
                    {
                        PlateFeeDetailModel detail = new PlateFeeDetailModel();
                        detail.PlateFeeDetailId = Guid.NewGuid();
                        detail.PlateFeeId = model.PlateFeeId;
                        detail.Province = item.Province;
                        detail.Price = item.Price;
                        _context.Entry(detail).State = EntityState.Added;
                    }
                }

                //many to many product
                if (chosenList != null)
                {
                    var ProductId = chosenList.Select(p => p.ProductId).ToList();
                    ManyToMany(model, ProductId);
                }

                _context.Entry(model).State = EntityState.Added;
                _context.SaveChanges();

                return Json(new
                {
                    Code = System.Net.HttpStatusCode.Created,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Create_Success, LanguageResource.Sale_PlateFee.ToLower())
                });
            });
        }
        #endregion Create

        //GET: /PlateFee/Edit
        #region Edit
        [ISDAuthorizationAttribute]
        public ActionResult Edit(Guid id)
        {
            var model = (from p in _context.PlateFeeModel
                         where p.PlateFeeId == id
                         select new PlateFeeViewModel()
                         {
                             PlateFeeId = p.PlateFeeId,
                             PlateFeeCode = p.PlateFeeCode,
                             PlateFeeName = p.PlateFeeName,
                             Actived = p.Actived,
                             Description = p.Description
                         }).FirstOrDefault();

            if (model == null)
            {
                return RedirectToAction("ErrorText", "Error", new { area = "", statusCode = 404, exception = string.Format(LanguageResource.Error_NotExist, LanguageResource.Sale_PlateFee.ToLower()) });
            }

            var chosenList = (from p in _context.PlateFeeModel
                              from pr in p.ProductModel
                              join product in _context.ProductModel on pr.ProductId equals product.ProductId
                              join c in _context.CategoryModel on product.CategoryId equals c.CategoryId into cg
                              from c1 in cg.DefaultIfEmpty()
                              join br in _context.CategoryModel on product.BrandId equals br.CategoryId
                              join cf in _context.ConfigurationModel on product.ConfigurationId equals cf.ConfigurationId
                              where p.PlateFeeId == id
                              select new Detail_PeriodicallyCheckingViewModel()
                              {
                                  ProductId = product.ProductId,
                                  ERPProductCode = product.ERPProductCode,
                                  ProductCode = product.ProductCode,
                                  ProductName = product.ProductName,
                                  CategoryName = c1.CategoryName,
                                  ConfigurationName = cf.ConfigurationName,
                                  Actived = p.Actived
                              }).ToList();

            CreateViewBag();
            ViewBag.chosenList = chosenList;

            return View(model);
        }
        [HttpPost]
        [ValidateAjax]
        [ValidateInput(false)] //need when using ckeditor, do not delete
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PlateFeeViewModel viewModel, List<PlateFeeDetailViewModel> detailList, List<Detail_PeriodicallyCheckingViewModel> chosenList)
        {
            return ExecuteContainer(() =>
            {
                var model = _context.PlateFeeModel
                                    .FirstOrDefault(p => p.PlateFeeId == viewModel.PlateFeeId);
                if (model != null)
                {
                    model.PlateFeeName = viewModel.PlateFeeName;
                    model.Description = viewModel.Description;
                    model.Actived = viewModel.Actived;
                    model.LastModifyUser = CurrentUser.UserName;
                    model.LastModifyDate = DateTime.Now;

                    //delete all detail in db
                    var existDetailList = _context.PlateFeeDetailModel.Where(p => p.PlateFeeId == viewModel.PlateFeeId)
                                                  .ToList();
                    _context.PlateFeeDetailModel.RemoveRange(existDetailList);
                    _context.SaveChanges();
                    // add new detail rows
                    if (detailList != null)
                    {
                        foreach (var item in detailList)
                        {
                            PlateFeeDetailModel detail = new PlateFeeDetailModel();
                            detail.PlateFeeDetailId = Guid.NewGuid();
                            detail.PlateFeeId = model.PlateFeeId;
                            detail.Province = item.Province;
                            detail.Price = item.Price;
                            _context.Entry(detail).State = EntityState.Added;
                        }
                    }

                    //many to many product
                    if (chosenList != null)
                    {
                        var ProductId = chosenList.Select(p => p.ProductId).ToList();
                        ManyToMany(model, ProductId);
                    }

                    _context.Entry(model).State = EntityState.Modified;
                    _context.SaveChanges();
                }

                return Json(new
                {
                    Code = System.Net.HttpStatusCode.Created,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Edit_Success, LanguageResource.Sale_PlateFee.ToLower())
                });
            });
        }
        #endregion Edit

        #region Detail Product Partial
        public ActionResult _DetailChecking(List<Detail_PeriodicallyCheckingViewModel> chosenList)
        {
            if (chosenList != null)
            {
                return PartialView(chosenList);
            }
            return PartialView();
        }
        public ActionResult _DetailCheckingInner(List<Detail_PeriodicallyCheckingViewModel> chosenList = null)
        {
            if (chosenList == null)
            {
                chosenList = new List<Detail_PeriodicallyCheckingViewModel>();
            }
            return PartialView(chosenList);
        }
        //Popup Search Product
        public ActionResult _ProductSearch()
        {
            ProductSearchViewModel model = new ProductSearchViewModel();
            CreateViewBag();
            return PartialView(model);
        }
        //Popup Search Product Result
        public ActionResult _ProductSearchResult(ProductSearchViewModel model, List<Guid> chosenList)
        {
            return ExecuteSearch(() =>
            {
                var productList = (from p in _context.ProductModel
                                   join c in _context.CategoryModel on p.CategoryId equals c.CategoryId into cg
                                   from c1 in cg.DefaultIfEmpty()
                                   join br in _context.CategoryModel on p.BrandId equals br.CategoryId
                                   join cf in _context.ConfigurationModel on p.ConfigurationId equals cf.ConfigurationId
                                   where
                                   //search by ERPProductCode
                                   (string.IsNullOrEmpty(model.SearchERPProductCode) || p.ERPProductCode.Contains(model.SearchERPProductCode))
                                   //search by ProductCode
                                   && (string.IsNullOrEmpty(model.SearchProductCode) || p.ProductCode.Contains(model.SearchProductCode))
                                   //search by BrandId
                                   && (model.SearchBrandId == null || p.BrandId == model.SearchBrandId)
                                   //search by CategoryId
                                   && (model.SearchCategoryId == null || p.CategoryId == model.SearchCategoryId)
                                   //search by ConfigurationId
                                   && (model.SearchConfigurationId == null || p.ConfigurationId == model.SearchConfigurationId)
                                   //search by isHot
                                   && (model.isHot == null || p.isHot == model.isHot)
                                   //Actived = true
                                   && p.Actived == true
                                   select new Detail_PeriodicallyCheckingViewModel()
                                   {
                                       ProductId = p.ProductId,
                                       ERPProductCode = p.ERPProductCode,
                                       ProductCode = p.ProductCode,
                                       ProductName = p.ProductName,
                                       CategoryName = c1.CategoryName,
                                       ConfigurationName = cf.ConfigurationName,
                                       Actived = p.Actived
                                   }).ToList();

                //select list search without products in productTable (product detail table)
                if (chosenList != null)
                {
                    productList = productList.Where(p => !chosenList.Contains(p.ProductId)).ToList();
                }

                return PartialView(productList);
            });
        }
        //Choose Product into List
        public ActionResult InsertProduct(List<Guid> existList, List<Guid> productList)
        {
            List<Detail_PeriodicallyCheckingViewModel> chosenList = new List<Detail_PeriodicallyCheckingViewModel>();
            if (existList != null && existList.Count > 0)
            {
                foreach (var item in existList)
                {
                    Detail_PeriodicallyCheckingViewModel model = new Detail_PeriodicallyCheckingViewModel();
                    var product = (from p in _context.ProductModel
                                   join c in _context.CategoryModel on p.CategoryId equals c.CategoryId into cg
                                   from c1 in cg.DefaultIfEmpty()
                                   join br in _context.CategoryModel on p.BrandId equals br.CategoryId
                                   join cf in _context.ConfigurationModel on p.ConfigurationId equals cf.ConfigurationId
                                   where p.ProductId == item
                                   select new Detail_PeriodicallyCheckingViewModel()
                                   {
                                       ProductId = p.ProductId,
                                       ERPProductCode = p.ERPProductCode,
                                       ProductCode = p.ProductCode,
                                       ProductName = p.ProductName,
                                       CategoryName = c1.CategoryName,
                                       ConfigurationName = cf.ConfigurationName,
                                       Actived = p.Actived
                                   }).FirstOrDefault();

                    if (product != null)
                    {
                        chosenList.Add(product);
                    }
                }
            }
            if (productList != null && productList.Count > 0)
            {
                foreach (var item in productList)
                {
                    Detail_PeriodicallyCheckingViewModel model = new Detail_PeriodicallyCheckingViewModel();
                    var product = (from p in _context.ProductModel
                                   join c in _context.CategoryModel on p.CategoryId equals c.CategoryId into cg
                                   from c1 in cg.DefaultIfEmpty()
                                   join br in _context.CategoryModel on p.BrandId equals br.CategoryId
                                   join cf in _context.ConfigurationModel on p.ConfigurationId equals cf.ConfigurationId
                                   where p.ProductId == item
                                   select new Detail_PeriodicallyCheckingViewModel()
                                   {
                                       ProductId = p.ProductId,
                                       ERPProductCode = p.ERPProductCode,
                                       ProductCode = p.ProductCode,
                                       ProductName = p.ProductName,
                                       CategoryName = c1.CategoryName,
                                       ConfigurationName = cf.ConfigurationName,
                                       Actived = p.Actived
                                   }).FirstOrDefault();

                    if (product != null)
                    {
                        chosenList.Add(product);
                    }
                }
            }
            return PartialView("_DetailCheckingInner", chosenList);
        }
        //Delete Product
        public ActionResult DeleteProduct(List<Guid> productList, Guid DeleteProductId)
        {
            var List = productList.Where(p => p != DeleteProductId).ToList();

            List<Detail_PeriodicallyCheckingViewModel> chosenList = new List<Detail_PeriodicallyCheckingViewModel>();
            foreach (var item in List)
            {
                Detail_PeriodicallyCheckingViewModel model = new Detail_PeriodicallyCheckingViewModel();
                var product = (from p in _context.ProductModel
                               join c in _context.CategoryModel on p.CategoryId equals c.CategoryId into cg
                               from c1 in cg.DefaultIfEmpty()
                               join br in _context.CategoryModel on p.BrandId equals br.CategoryId
                               join cf in _context.ConfigurationModel on p.ConfigurationId equals cf.ConfigurationId
                               where p.ProductId == item
                               select new Detail_PeriodicallyCheckingViewModel()
                               {
                                   ProductId = p.ProductId,
                                   ERPProductCode = p.ERPProductCode,
                                   ProductCode = p.ProductCode,
                                   ProductName = p.ProductName,
                                   CategoryName = c1.CategoryName,
                                   ConfigurationName = cf.ConfigurationName,
                                   Actived = p.Actived
                               }).FirstOrDefault();

                if (product != null)
                {
                    chosenList.Add(product);
                }
            }
            return PartialView("_DetailCheckingInner", chosenList);
        }
        #endregion

        #region Detail Plate Fee: Province, Price
        public ActionResult _DetailPlateFee(Guid? PlateFeeId, int mode)
        {
            //Edit
            if (mode != 1)
            {
                var detailList = (from p in _context.PlateFeeDetailModel
                                  where p.PlateFeeId == PlateFeeId
                                  select new PlateFeeDetailViewModel()
                                  {
                                      PlateFeeDetailId = p.PlateFeeDetailId,
                                      PlateFeeId = p.PlateFeeId,
                                      Province = p.Province,
                                      Price = p.Price
                                  }).ToList();
                return PartialView(detailList);
            }
            return PartialView();
        }
        public ActionResult _DetailPlateFeeInner(List<PlateFeeDetailViewModel> detailList = null)
        {
            if (detailList == null)
            {
                detailList = new List<PlateFeeDetailViewModel>();
            }
            return PartialView(detailList);
        }
        public ActionResult InsertDetailPlateFee(List<PlateFeeDetailViewModel> detailList, string Province, decimal? Price)
        {
            if (detailList == null)
            {
                detailList = new List<PlateFeeDetailViewModel>();
            }

            var existList = detailList.Select(p => p.Province).ToList();
            if (!existList.Contains(Province))
            {
                PlateFeeDetailViewModel model = new PlateFeeDetailViewModel();
                model.Province = Province;
                model.Price = Price;
                detailList.Add(model);
            }

            return PartialView("_DetailPlateFeeInner", detailList);
        }
        //delete row detail
        public ActionResult DeleteDetailPlateFee(List<PlateFeeDetailViewModel> detailList, int STT)
        {
            var List = detailList.Where(p => p.STT != STT).ToList();
            return PartialView("_DetailPlateFeeInner", List);
        }
        //source autocomplete
        public ActionResult GetProviveAndDistrict(string provinceName)
        {
            List<string> list = new List<string>();
            var provinceList = (from p in _context.ProvinceModel
                                where p.Actived == true && p.ProvinceCode.Contains(provinceName)
                                select p.ProvinceCode).Take(5).ToList();
            list.AddRange(provinceList);

            var districtList = (from p in _context.DistrictModel
                                where p.Actived == true && p.DistrictCode.Contains(provinceName)
                                select p.DistrictCode).Take(5).ToList();
            list.AddRange(districtList);

            return Json(list, JsonRequestBehavior.AllowGet);
        }
        #endregion Detail Plate Fee: Province, Price

        #region CreateViewBag, Helper
        public void CreateViewBag(Guid? SearchBrandId = null, Guid? SearchCategoryId = null, Guid? SearchConfigurationId = null)
        {
            //Get list Brand
            var brandList = _context.CategoryModel.Where(p => p.ParentCategoryId == null && p.Actived == true)
                                                  .OrderBy(p => p.OrderIndex).ToList();
            ViewBag.SearchBrandId = new SelectList(brandList, "CategoryId", "CategoryName", SearchBrandId);

            //Get list CategoryId
            if (SearchBrandId != null)
            {
                var categoryList = _context.CategoryModel.Where(p => p.ParentCategoryId == SearchBrandId && p.Actived == true)
                                                  .OrderBy(p => p.OrderIndex).ToList();
                ViewBag.SearchCategoryId = new SelectList(categoryList, "CategoryId", "CategoryName", SearchCategoryId);
            }

            //Get list Configuration
            var configList = _context.ConfigurationModel.Where(p => p.Actived == true).OrderBy(p => p.OrderIndex).ToList();
            ViewBag.SearchConfigurationId = new SelectList(configList, "ConfigurationId", "ConfigurationName", SearchConfigurationId);
        }
        //GetCategoryByBrand
        public ActionResult GetCategoryByBrand(Guid? SearchBrandId = null)
        {
            //Get list CategoryId
            var categoryList = _context.CategoryModel.Where(p => p.Actived == true && p.ParentCategoryId == SearchBrandId)
                                                     .OrderBy(p => p.OrderIndex).ToList();
            var CategoryIdList = new SelectList(categoryList, "CategoryId", "CategoryName");

            return Json(CategoryIdList, JsonRequestBehavior.AllowGet);
        }
        //Many to many
        private void ManyToMany(PlateFeeModel model, List<Guid> ProductId)
        {
            if (model.ProductModel != null)
            {
                model.ProductModel.Clear();
            }
            foreach (var item in ProductId)
            {
                var product = _context.ProductModel.Find(item);
                if (product != null)
                {
                    model.ProductModel.Add(product);
                }
            }
        }
        #endregion

        #region Remote Validation
        private bool IsExists(string PlateFeeCode)
        {
            return (_context.PlateFeeModel.FirstOrDefault(p => p.PlateFeeCode == PlateFeeCode) != null);
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult CheckExistingPlateFeeCode(string PlateFeeCode, string PlateFeeCodeValid)
        {
            try
            {
                if (PlateFeeCodeValid != PlateFeeCode)
                {
                    return Json(!IsExists(PlateFeeCode));
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
    }
}