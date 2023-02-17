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
    public class PeriodicallyCheckingController : BaseController
    {
        // GET: PeriodicallyChecking
        #region Index
        [ISDAuthorizationAttribute]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult _Search(string PeriodicallyCheckingCode = "", string PeriodicallyCheckingName = "", bool? Actived = null)
        {
            return ExecuteSearch(() =>
            {
                var check = (from p in _context.PeriodicallyCheckingModel
                             where
                             //search by PeriodicallyCheckingCode
                             (PeriodicallyCheckingCode == "" || p.PeriodicallyCheckingCode.Contains(PeriodicallyCheckingCode))
                             //search by PeriodicallyCheckingName
                             && (PeriodicallyCheckingName == "" || p.PeriodicallyCheckingName.Contains(PeriodicallyCheckingName))
                             //search by Actived
                             && (Actived == null || p.Actived == Actived)
                             select new Search_PeriodicallyCheckingViewModel()
                             {
                                 PeriodicallyCheckingId = p.PeriodicallyCheckingId,
                                 PeriodicallyCheckingCode = p.PeriodicallyCheckingCode,
                                 PeriodicallyCheckingName = p.PeriodicallyCheckingName,
                                 Actived = p.Actived
                             })
                             .ToList();

                foreach (var item in check)
                {
                    var productList = (from p in _context.PeriodicallyCheckingModel
                                       from pr in p.ProductModel
                                       where p.PeriodicallyCheckingId == item.PeriodicallyCheckingId
                                       select pr.ProductId).ToList();
                    item.NumberOfProduct = productList.Count;
                }
                
                return PartialView(check);
            });
        }
        #endregion Index

        //GET: /PeriodicallyChecking/Create
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
        public ActionResult Create(PeriodicallyCheckingViewModel viewModel, HttpPostedFileBase FileUrl, List<Detail_PeriodicallyCheckingViewModel> chosenList)
        {
            return ExecuteContainer(() =>
            {
                PeriodicallyCheckingModel model = new PeriodicallyCheckingModel();
                model.PeriodicallyCheckingId = Guid.NewGuid();
                model.PeriodicallyCheckingCode = viewModel.PeriodicallyCheckingCode;
                model.PeriodicallyCheckingName = viewModel.PeriodicallyCheckingName;
                model.Description = viewModel.Description;
                if (FileUrl != null)
                {
                    model.FileUrl = Upload(FileUrl, "PeriodicallyChecking");
                }
                model.Actived = viewModel.Actived;
                model.CreatedUser = CurrentUser.UserName;
                model.CreatedDate = DateTime.Now;

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
                    Data = string.Format(LanguageResource.Alert_Create_Success, LanguageResource.Sale_PeriodicallyChecking.ToLower())
                });
            });
        }
        #endregion Create

        //GET: /PeriodicallyChecking/Edit
        #region Edit
        [ISDAuthorizationAttribute]
        public ActionResult Edit(Guid id)
        {
            var model = (from p in _context.PeriodicallyCheckingModel
                         where p.PeriodicallyCheckingId == id
                         select new PeriodicallyCheckingViewModel()
                         {
                             PeriodicallyCheckingId = p.PeriodicallyCheckingId,
                             PeriodicallyCheckingCode = p.PeriodicallyCheckingCode,
                             PeriodicallyCheckingName = p.PeriodicallyCheckingName,
                             Actived = p.Actived,
                             Description = p.Description,
                             FileUrl = p.FileUrl
                         }).FirstOrDefault();

            if (model == null)
            {
                return RedirectToAction("ErrorText", "Error", new { area = "", statusCode = 404, exception = string.Format(LanguageResource.Error_NotExist, LanguageResource.Sale_PeriodicallyChecking.ToLower()) });
            }

            var chosenList = (from p in _context.PeriodicallyCheckingModel
                              from pr in p.ProductModel
                              join product in _context.ProductModel on pr.ProductId equals product.ProductId
                              join c in _context.CategoryModel on product.CategoryId equals c.CategoryId into cg
                              from c1 in cg.DefaultIfEmpty()
                              join br in _context.CategoryModel on product.BrandId equals br.CategoryId
                              join cf in _context.ConfigurationModel on product.ConfigurationId equals cf.ConfigurationId
                              where p.PeriodicallyCheckingId == id
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
        public ActionResult Edit(PeriodicallyCheckingViewModel viewModel, HttpPostedFileBase FileUrl, List<Detail_PeriodicallyCheckingViewModel> chosenList)
        {
            return ExecuteContainer(() =>
            {
                var model = _context.PeriodicallyCheckingModel
                                    .FirstOrDefault(p => p.PeriodicallyCheckingId == viewModel.PeriodicallyCheckingId);
                if (model != null)
                {
                    model.PeriodicallyCheckingName = viewModel.PeriodicallyCheckingName;
                    model.Description = viewModel.Description;
                    if (FileUrl != null)
                    {
                        model.FileUrl = Upload(FileUrl, "PeriodicallyChecking");
                    }
                    model.Actived = viewModel.Actived;
                    model.LastModifyUser = CurrentUser.UserName;
                    model.LastModifyDate = DateTime.Now;

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
                    Data = string.Format(LanguageResource.Alert_Edit_Success, LanguageResource.Sale_PeriodicallyChecking.ToLower())
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
        private void ManyToMany(PeriodicallyCheckingModel model, List<Guid> ProductId)
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
        private bool IsExists(string PeriodicallyCheckingCode)
        {
            return (_context.PeriodicallyCheckingModel.FirstOrDefault(p => p.PeriodicallyCheckingCode == PeriodicallyCheckingCode) != null);
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult CheckExistingPeriodicallyCheckingCode(string PeriodicallyCheckingCode, string PeriodicallyCheckingCodeValid)
        {
            try
            {
                if (PeriodicallyCheckingCodeValid != PeriodicallyCheckingCode)
                {
                    return Json(!IsExists(PeriodicallyCheckingCode));
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