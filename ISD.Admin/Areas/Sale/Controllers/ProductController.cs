using ISD.Constant;
using ISD.EntityModels;
using ISD.Extensions;
using ISD.Repositories;
using ISD.Resources;
using ISD.ViewModels;
using ISD.ViewModels.Extension;
using ISD.ViewModels.Sale;
using ISD.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using ISD.Repositories.Excel;
using System.Net;
using System.Data.Entity.Validation;

namespace Sale.Controllers
{
    public class ProductController : BaseController
    {
        // GET: Product
        #region Index
        [ISDAuthorizationAttribute]
        public ActionResult Index()
        {
            CreateViewBag();
            return View();
        }
        public ActionResult _Search(ProductSearchViewModel searchViewModel)
        {
            return ExecuteSearch(() =>
            {
                //Get data filter for Export edit
                Session["frmSearchProduct"] = searchViewModel;

                var productList = (from p in _context.ProductModel
                                   join c in _context.CategoryModel on p.CategoryId equals c.CategoryId into catmp
                                   from ca in catmp.DefaultIfEmpty()
                                       //join br in _context.CategoryModel on c.ParentCategoryId equals br.CategoryId
                                   join br in _context.CategoryModel on p.ParentCategoryId equals br.CategoryId
                                   join com in _context.CompanyModel on p.CompanyId equals com.CompanyId
                                   //join cf in _context.ConfigurationModel on p.ConfigurationId equals cf.ConfigurationId
                                   where
                                   //search by ERPProductCode
                                   (searchViewModel.ERPProductCode == null || p.ERPProductCode.Contains(searchViewModel.ERPProductCode))
                                   //search by ProductCode
                                   && (searchViewModel.ProductCode == null || p.ProductCode.Contains(searchViewModel.ProductCode))
                                   //search by ProductName
                                   && (searchViewModel.ProductName == null || p.ProductName.Contains(searchViewModel.ProductName))
                                   //search by BrandId
                                   //&& (searchViewModel.BrandId == null || p.BrandId == searchViewModel.BrandId)
                                   //search by ParentCategoryId
                                   && (searchViewModel.ParentCategoryId == null || p.ParentCategoryId == searchViewModel.ParentCategoryId)
                                   //search by CategoryId
                                   && (searchViewModel.CategoryId == null || p.CategoryId == searchViewModel.CategoryId)
                                   //search by ConfigurationId
                                   //&& (searchViewModel.ConfigurationId == null || p.ConfigurationId == searchViewModel.ConfigurationId)
                                   //search by isHot
                                   //&& (searchViewModel.isHot == null || p.isHot == searchViewModel.isHot)
                                   //search by Actived
                                   && (searchViewModel.Actived == null || p.Actived == searchViewModel.Actived)
                                   orderby com.CompanyCode, p.ERPProductCode
                                   select new ProductViewModel()
                                   {
                                       ProductId = p.ProductId,
                                       ERPProductCode = p.ERPProductCode,
                                       ProductCode = p.ProductCode,
                                       ProductGroups = p.ProductGroups,
                                       ProductName = p.ProductName,
                                       ParentCategoryId = p.ParentCategoryId,
                                       ParentCategoryName = br.CategoryName,
                                       CategoryId = p.CategoryId,
                                       CategoryName = ca.CategoryName,
                                       //ConfigurationName = cf.ConfigurationName,
                                       ImageUrl = p.ImageUrl,
                                       OrderIndex = p.OrderIndex,
                                       Actived = p.Actived,
                                       CompanyCode = com.CompanyCode, 
                                       //Đơn giá
                                       Price = p.Price,
                                       //Giá trị mẫu
                                       SampleValue = p.SampleValue,
                                       //Giá trị công
                                       ProcessingValue = p.ProcessingValue,
                                   }).ToList();

                return PartialView(productList);
            });
        }

        public ActionResult _PaggingServerSide(DatatableViewModel model, ProductSearchViewModel searchViewModel)
        {
            try
            {
                //Get data filter for Export edit
                Session["frmSearchProduct"] = searchViewModel;
                // action inside a standard controller
                int filteredResultsCount;
                int totalResultsCount;

                var query = (from p in _context.ProductModel
                             join c in _context.CategoryModel on p.CategoryId equals c.CategoryId into caTmp
                             from ca in caTmp.DefaultIfEmpty()
                                 //join br in _context.CategoryModel on c.ParentCategoryId equals br.CategoryId
                             join br in _context.CategoryModel on p.ParentCategoryId equals br.CategoryId
                             join com in _context.CompanyModel on p.CompanyId equals com.CompanyId into cmTmp
                             from cmp in cmTmp.DefaultIfEmpty()
                                 //join cf in _context.ConfigurationModel on p.ConfigurationId equals cf.ConfigurationId
                             where
                             //search by ERPProductCode
                             (searchViewModel.ERPProductCode == null || p.ERPProductCode.Contains(searchViewModel.ERPProductCode))
                             //search by ProductCode
                             && (searchViewModel.ProductCode == null || p.ProductCode.Contains(searchViewModel.ProductCode))
                             //search by ProductName
                             && (searchViewModel.ProductName == null || p.ProductName.Contains(searchViewModel.ProductName))
                             //search by BrandId
                             //&& (searchViewModel.BrandId == null || p.BrandId == searchViewModel.BrandId)
                             //search by ParentCategoryId
                             && (searchViewModel.ParentCategoryId == null || p.ParentCategoryId == searchViewModel.ParentCategoryId)
                             //search by CategoryId
                             && (searchViewModel.CategoryId == null || p.CategoryId == searchViewModel.CategoryId)
                             //search by ConfigurationId
                             //&& (searchViewModel.ConfigurationId == null || p.ConfigurationId == searchViewModel.ConfigurationId)
                             //search by isHot
                             //&& (searchViewModel.isHot == null || p.isHot == searchViewModel.isHot)
                             //search by Actived
                             && (searchViewModel.Actived == null || p.Actived == searchViewModel.Actived)
                             orderby cmp.CompanyCode, p.ERPProductCode
                             select new ProductViewModel()
                             {
                                 ProductId = p.ProductId,
                                 ERPProductCode = p.ERPProductCode,
                                 ProductCode = p.ProductCode,
                                 ProductGroups = p.ProductGroups,
                                 ProductName = p.ProductName,
                                 ParentCategoryId = p.ParentCategoryId,
                                 ParentCategoryName = br.CategoryName,
                                 CategoryId = p.CategoryId,
                                 CategoryName = ca.CategoryName,
                                 //ConfigurationName = cf.ConfigurationName,
                                 ImageUrl = p.ImageUrl,
                                 OrderIndex = p.OrderIndex,
                                 Actived = p.Actived,
                                 CompanyCode = cmp.CompanyCode,
                                 //Đơn giá
                                 Price = p.Price,
                                 //Giá trị mẫu
                                 SampleValue = p.SampleValue,
                                 //Giá trị công
                                 ProcessingValue = p.ProcessingValue,
                             });

                var res = CustomSearchRepository.CustomSearchFunc<ProductViewModel>(model, out filteredResultsCount, out totalResultsCount, query, "STT");
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
                    data = res
                });
            }
            catch //(Exception ex)
            {
                return Json(null);
            }
        }
        #endregion

        //GET: /Product/Create
        #region Create
        [ISDAuthorizationAttribute]
        public ActionResult Create()
        {
            var companyId = CurrentUser.CompanyId;
            CreateViewBag(CompanyId: companyId);

            ProductViewModel viewModel = new ProductViewModel();
            viewModel.isHot = false;
            viewModel.isInventory = false;
            return View(viewModel);
        }
        //POST: Create
        [HttpPost]
        [ValidateAjax]
        [ISDAuthorizationAttribute]
        public JsonResult Create(ProductModel model, List<ProductViewModel> propertiesList, List<ProductViewModel> specificationsList, List<ProductViewModel> accessoryList, HttpPostedFileBase ImageUrl, ProductAttributeModel attr)
        {
            return ExecuteContainer(() =>
            {
                model.ProductId = Guid.NewGuid();
                if (ImageUrl != null)
                {
                    model.ImageUrl = Upload(ImageUrl, "Color");
                }
                attr.ProductId = model.ProductId;

                #region Add details
                ////Properties
                //if (propertiesList != null)
                //{
                //    foreach (var item in propertiesList)
                //    {
                //        PropertiesProductModel properties = new PropertiesProductModel();
                //        properties.PropertiesId = Guid.NewGuid();
                //        properties.ProductId = model.ProductId;
                //        properties.Subject = item.Subject;
                //        properties.SubjectColor = item.SubjectColor;
                //        properties.Description = item.Description;
                //        properties.DescriptionColor = item.DescriptionColor;
                //        properties.Image = item.Image;
                //        properties.BackgroundColor = item.BackgroundColor;
                //        properties.X = item.X;
                //        properties.Y = item.Y;
                //        _context.Entry(properties).State = EntityState.Added;
                //    }
                //}
                ////Specifications
                //if (specificationsList != null)
                //{
                //    foreach (var item in specificationsList)
                //    {
                //        SpecificationsProductModel specifications = new SpecificationsProductModel();
                //        specifications.SpecificationsProductId = Guid.NewGuid();
                //        specifications.ProductId = model.ProductId;
                //        specifications.SpecificationsId = item.SpecificationsId;
                //        specifications.Description = item.SpecificationsDescription;
                //        _context.Entry(specifications).State = EntityState.Added;
                //    }
                //}
                ////Accessory
                //if (accessoryList != null)
                //{
                //    foreach (var item in accessoryList)
                //    {
                //        AccessoryProductModel accessory = new AccessoryProductModel();
                //        accessory.AccessoryProductId = Guid.NewGuid();
                //        accessory.ProductId = model.ProductId;
                //        accessory.AccessoryId = item.AccessoryId;
                //        accessory.Price = item.Price;
                //        _context.Entry(accessory).State = EntityState.Added;
                //    }
                //}
                #endregion Add details

                _context.Entry(model).State = EntityState.Added;
                _context.Entry(attr).State = EntityState.Added;
                _context.SaveChanges();

                return Json(new
                {
                    Code = System.Net.HttpStatusCode.Created,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Create_Success, LanguageResource.Sale_Product.ToLower())
                });
            });
        }
        #endregion

        //GET: /Product/Edit
        #region Edit
        [ISDAuthorizationAttribute]
        public ActionResult Edit(Guid id)
        {
            var product = (from p in _context.ProductModel
                           join c in _context.CategoryModel on p.CategoryId equals c.CategoryId into catmp
                           from ca in catmp.DefaultIfEmpty()
                               //join br in _context.CategoryModel on c.ParentCategoryId equals br.CategoryId
                           join br in _context.CategoryModel on p.ParentCategoryId equals br.CategoryId
                           join at in _context.ProductAttributeModel on p.ProductId equals at.ProductId into tmpAttr
                           from attr in tmpAttr.DefaultIfEmpty()
                           where p.ProductId == id
                           select new ProductViewModel()
                           {
                               ProductId = p.ProductId,
                               ProductGroups = p.ProductGroups,
                               ProductName = p.ProductName,
                               ERPProductCode = p.ERPProductCode,
                               ProductCode = p.ProductCode,
                               ParentCategoryId = br.CategoryId,
                               CategoryId = p.CategoryId,
                               //ConfigurationId = p.ConfigurationId,
                               //GuaranteePeriod = p.GuaranteePeriod,
                               ImageUrl = p.ImageUrl,
                               OrderIndex = p.OrderIndex,
                               isHot = p.isHot == true ? true : false,
                               isInventory = p.isInventory == true ? true : false,
                               Actived = p.Actived == true ? true : false,
                               Unit = attr.Unit,
                               //Loại xe: Ga số, tay côn
                               //ProductTypeId = p.ProductTypeId,
                               //dung tích xi lanh
                               //CylinderCapacity = p.CylinderCapacity
                               CompanyId = p.CompanyId,
                               //Attribute
                               Description = attr.Description,
                               Color = attr.Color,
                               Thickness = attr.Thickness,
                               Allocation = attr.Allocation,
                               Grade = attr.Grade,
                               Surface = attr.Surface,
                               NumberOfSurface = attr.NumberOfSurface,
                               GrossWeight = attr.GrossWeight,
                               NetWeight = attr.NetWeight,
                               WeightUnit = attr.WeightUnit,
                               //Đơn giá
                               Price = p.Price,
                               //Giá trị mẫu
                               SampleValue = p.SampleValue,
                               //Giá trị công
                               ProcessingValue = p.ProcessingValue,
                               WarrantyId = p.WarrantyId
                           }).FirstOrDefault();

            if (product == null)
            {
                return RedirectToAction("ErrorText", "Error", new { area = "", statusCode = 404, exception = string.Format(LanguageResource.Error_NotExist, LanguageResource.Sale_Product.ToLower()) });
            }
            CreateViewBag(product.ParentCategoryId, product.CategoryId, product.ConfigurationId, product.CompanyId, Color: product.Color, warrantyId: product.WarrantyId);
            return View(product);
        }
        [HttpPost]
        //[ValidateAjax]
        [ValidateInput(false)] //need when using ckeditor, do not delete
        [ValidateAntiForgeryToken]
        [ISDAuthorizationAttribute]
        //, List<ProductViewModel> propertiesList
        public ActionResult Edit(ProductModel model, List<ProductViewModel> specificationsList, List<ProductViewModel> accessoryList, HttpPostedFileBase ImageUrl, ProductAttributeModel attrModel)
        {
            return ExecuteContainer(() =>
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    var product = _context.ProductModel.FirstOrDefault(p => p.ProductId == model.ProductId);
                    if (product != null)
                    {
                        product.ERPProductCode = model.ERPProductCode;
                        product.ProductCode = model.ProductCode;
                        product.ProductGroups = model.ProductGroups;
                        product.ProductName = model.ProductName;
                        //product.CylinderCapacity = model.CylinderCapacity;
                        //product.BrandId = model.BrandId;
                        product.ParentCategoryId = model.ParentCategoryId;
                        product.CategoryId = model.CategoryId;
                        //product.ConfigurationId = model.ConfigurationId;
                        //product.GuaranteePeriod = model.GuaranteePeriod;
                        product.OrderIndex = model.OrderIndex;
                        product.isHot = model.isHot;
                        product.isInventory = model.isInventory;
                        product.Actived = model.Actived;
                        product.CompanyId = model.CompanyId;
                        //Giá trị mẫu
                        product.SampleValue = model.SampleValue;
                        //Giá trị công
                        product.ProcessingValue = model.ProcessingValue;
                        //Đơn giá
                        product.Price = model.Price;
                        product.SampleValue = model.SampleValue;
                        product.ProcessingValue = model.ProcessingValue;

                        product.WarrantyId = model.WarrantyId;
                        if (ImageUrl != null)
                        {
                            product.ImageUrl = Upload(ImageUrl, "Color");
                        }
                        //Product Attribute
                        var attr = _context.ProductAttributeModel.FirstOrDefault(p => p.ProductId == model.ProductId);
                        if (attr != null)
                        {
                            attr.Description = attrModel.Description;
                            attr.Unit = attrModel.Unit;
                            attr.Color = attrModel.Color;
                            attr.Thickness = attrModel.Thickness;
                            attr.Allocation = attrModel.Allocation;
                            attr.Grade = attrModel.Grade;
                            attr.Surface = attrModel.Surface;
                            attr.NumberOfSurface = attrModel.NumberOfSurface;
                            attr.GrossWeight = attrModel.GrossWeight;
                            attr.NetWeight = attrModel.NetWeight;
                            attr.WeightUnit = attrModel.WeightUnit;
                            _context.Entry(attr).State = EntityState.Modified;
                        }
                        else
                        {
                            attrModel.ProductId = model.ProductId;
                            _context.Entry(attrModel).State = EntityState.Added;
                        }

                        #region Delete and Add details
                        ////Specifications
                        ////delete
                        //var deleteSpecificationsList = _context.SpecificationsProductModel
                        //                                       .Where(p => p.ProductId == model.ProductId).ToList();
                        //_context.SpecificationsProductModel.RemoveRange(deleteSpecificationsList);
                        //_context.SaveChanges();

                        //if (specificationsList != null)
                        //{
                        //    foreach (var item in specificationsList)
                        //    {
                        //        SpecificationsProductModel specifications = new SpecificationsProductModel();
                        //        specifications.SpecificationsProductId = Guid.NewGuid();
                        //        specifications.ProductId = model.ProductId;
                        //        specifications.SpecificationsId = item.SpecificationsId;
                        //        specifications.Description = item.SpecificationsDescription;
                        //        _context.Entry(specifications).State = EntityState.Added;
                        //    }
                        //}
                        ////Accessory
                        ////delete
                        //var deleteAccessoryList = _context.AccessoryProductModel.Where(p => p.ProductId == model.ProductId).ToList();
                        //_context.AccessoryProductModel.RemoveRange(deleteAccessoryList);
                        //_context.SaveChanges();

                        //if (accessoryList != null)
                        //{
                        //    foreach (var item in accessoryList)
                        //    {
                        //        AccessoryProductModel accessory = new AccessoryProductModel();
                        //        accessory.AccessoryProductId = Guid.NewGuid();
                        //        accessory.ProductId = model.ProductId;
                        //        accessory.AccessoryId = item.AccessoryId;
                        //        accessory.Price = item.Price;
                        //        _context.Entry(accessory).State = EntityState.Added;
                        //    }
                        //}
                        #endregion Delete and Add details

                        _context.Entry(product).State = EntityState.Modified;
                        _context.SaveChanges();
                    }
                    ts.Complete();
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.Created,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Edit_Success, LanguageResource.Sale_Product.ToLower())
                    });
                }
            });
        }
        #endregion Edit

        //GET: /Product/Delete
        #region Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ISDAuthorizationAttribute]
        public ActionResult Delete(Guid id)
        {
            return ExecuteDelete(() =>
            {
                var product = _context.ProductModel.FirstOrDefault(p => p.ProductId == id);
                if (product != null)
                {
                    #region delete details
                    //Color
                    if (product.ColorProductModel != null)
                    {
                        //delete ImageProductModel
                        var imageList = _context.ImageProductModel.Where(p => p.ProductId == id).ToList();
                        _context.ImageProductModel.RemoveRange(imageList);
                        _context.SaveChanges();

                        //delete ColorProductModel
                        var colorList = _context.ColorProductModel.Where(p => p.ProductId == id).ToList();
                        _context.ColorProductModel.RemoveRange(colorList);
                        _context.SaveChanges();
                    }
                    //PromotionModel
                    if (product.PromotionModel != null)
                    {
                        product.PromotionModel.Clear();
                    }
                    //CustomerPromotionModel
                    if (product.CustomerPromotionModel != null)
                    {
                        product.CustomerPromotionModel.Clear();
                    }
                    //AccessoryProductModel
                    if (product.AccessoryProductModel != null)
                    {
                        var detailList = _context.AccessoryProductModel.Where(p => p.ProductId == id).ToList();
                        _context.AccessoryProductModel.RemoveRange(detailList);
                        _context.SaveChanges();
                    }
                    //ColorProductModel
                    if (product.ColorProductModel != null)
                    {
                        var detailList = _context.ColorProductModel.Where(p => p.ProductId == id).ToList();
                        _context.ColorProductModel.RemoveRange(detailList);
                        _context.SaveChanges();
                    }
                    //PropertiesProductModel
                    if (product.PropertiesProductModel != null)
                    {
                        var detailList = _context.PropertiesProductModel.Where(p => p.ProductId == id).ToList();
                        _context.PropertiesProductModel.RemoveRange(detailList);
                        _context.SaveChanges();
                    }
                    //SpecificationsProductModel
                    if (product.SpecificationsProductModel != null)
                    {
                        var detailList = _context.SpecificationsProductModel.Where(p => p.ProductId == id).ToList();
                        _context.SpecificationsProductModel.RemoveRange(detailList);
                        _context.SaveChanges();
                    }
                    #endregion delete details

                    _context.Entry(product).State = EntityState.Deleted;
                    _context.SaveChanges();

                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.Created,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Delete_Success, LanguageResource.Sale_Product.ToLower())
                    });
                }
                else
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotModified,
                        Success = false,
                        Data = string.Format(LanguageResource.Alert_NotExist_Delete, LanguageResource.Sale_Product.ToLower())
                    });
                }
            });
        }
        #endregion

        #region Detail Partial Color
        public ActionResult _ProductColor(Guid? ProductId, int mode)
        {
            ViewBag.Mode = mode;

            #region ViewBag
            var styleAllList = _context.StyleModel.Where(p => p.Actived == true)
                                                  .Select(p =>
                                                  new
                                                  {
                                                      Id = p.StyleId,
                                                      Name = p.StyleName,
                                                      OrderIndex = p.OrderIndex
                                                  }).OrderBy(p => p.OrderIndex).ToList();
            ViewBag.StyleId = new SelectList(styleAllList, "Id", "Name");

            var colorAllList = _context.ColorModel.Where(p => p.Actived == true)
                                                  .OrderBy(p => p.OrderIndex)
                                                  .ToList()
                                                  .Select(p =>
                                                  new
                                                  {
                                                      Id = p.ColorId,
                                                      Name = string.Format("{0} | {1}", p.ColorShortName, p.ColorName)
                                                  });
            ViewBag.MainColorId = new SelectList(colorAllList, "Id", "Name");
            #endregion ViewBag

            //Edit
            if (mode != 1)
            {
                List<ProductViewModel> colorList = new List<ProductViewModel>();
                if (ProductId != null)
                {
                    colorList = GetColorListBy(ProductId);
                }
                return PartialView(colorList);
            }
            //Create
            else
            {
                return PartialView();
            }
        }

        //list product to show on table
        public ActionResult _ProductColorInner(List<ProductViewModel> colorList = null)
        {
            if (colorList == null)
            {
                colorList = new List<ProductViewModel>();
            }
            return PartialView(colorList);
        }

        //delete row detail
        public ActionResult DeleteProductColor(List<ProductViewModel> colorList, Guid STT)
        {
            //delete image list
            var imageList = _context.ImageProductModel.Where(p => p.ColorProductId == STT).ToList();
            _context.ImageProductModel.RemoveRange(imageList);
            _context.SaveChanges();

            //delete color product
            var colorProduct = _context.ColorProductModel.Where(p => p.ColorProductId == STT).FirstOrDefault();
            var id = colorProduct.ProductId;
            _context.Entry(colorProduct).State = EntityState.Deleted;
            _context.SaveChanges();

            colorList = GetColorListBy(id);
            return PartialView("_ProductColorInner", colorList);
        }

        //insert row detail
        public ActionResult InsertProductColor(Guid? StyleId, Guid MainColorId, Guid ProductId, string MainColorCode, List<HttpPostedFileBase> ColorStyleImage, string ProductCode, string ProductName, string ERPProductCode)
        {
            //Check lỗi trùng
            var colorProductIsExist = _context.ColorProductModel
                                        //Màu sắc
                                        .Where(p => p.MainColorId == MainColorId &&
                                                    //Kiểu dáng
                                                    p.StyleId == StyleId &&
                                                    //Phiên bản
                                                    p.ProductId == ProductId).FirstOrDefault();
            var product = _context.ProductModel.Where(p => p.ProductId == ProductId).FirstOrDefault();
            if (colorProductIsExist == null)
            {
                //Save product
                if (product != null)
                {
                    //var mainColor = _context.ColorModel.FirstOrDefault(p => p.ColorCode == MainColorCode);
                    int i = 0;

                    #region Save data
                    //Save color product
                    ColorProductModel color = new ColorProductModel();
                    color.ColorProductId = Guid.NewGuid();
                    color.ProductId = ProductId;
                    color.StyleId = StyleId;
                    color.MainColorId = MainColorId;
                    _context.Entry(color).State = EntityState.Added;

                    foreach (var item in ColorStyleImage)
                    {
                        //save image product
                        ImageProductModel image = new ImageProductModel();
                        if (i == 0)
                        {
                            image.isDefault = true;
                        }
                        image.ImageId = Guid.NewGuid();
                        image.ProductId = ProductId;
                        image.ColorProductId = color.ColorProductId;
                        image.ImageUrl = Upload(item, "Color");
                        _context.Entry(image).State = EntityState.Added;
                        _context.SaveChanges();

                        i++;
                    }
                    #endregion save data
                }
            }
            // Tien NOTE: fix lỗi khi thêm màu sắc mới, nếu đã có rồi nó sẽ vào else và list ở front end bị mất tất => xóa else
            //else
            //{
            //    return RedirectToAction("ErrorText", "Error", new { area = "", statusCode = 404, exception = string.Format(LanguageResource.Validation_Already_Exists, LanguageResource.ColorAndStyle) });
            //}

            // TODO: TIEN khi có lỗi thì hiển thị báo lỗi, cái này chưa làm
            var colorList = GetColorListBy(product.ProductId);
            return PartialView("_ProductColorInner", colorList);
        }
        #endregion Detail Partial Color

        #region Modal Popup Image
        public ActionResult ImageDetails(Guid? ColorProductId)
        {
            var product = (from p in _context.ColorProductModel.AsEnumerable()
                           join pr in _context.ProductModel on p.ProductId equals pr.ProductId
                           join mcolor in _context.ColorModel on p.MainColorId equals mcolor.ColorId
                           where p.ColorProductId == ColorProductId
                           select new
                           {
                               ProductName = pr.ProductName.ToUpper(),
                               ColorName = string.Format("{0}: {1}", LanguageResource.Sale_Color, mcolor.ColorName)
                           }).FirstOrDefault();

            return Json(new
            {
                Code = System.Net.HttpStatusCode.Created,
                Success = true,
                Data = product
            });
        }

        //list detail image show on modal popup table
        public ActionResult _ImageDetailsInner(Guid? ColorProductId, List<ProductViewModel> detailColorList)
        {
            if (detailColorList == null)
            {
                detailColorList = _context.ImageProductModel.Where(p => p.ColorProductId == ColorProductId)
                                                  .Select(p =>
                                                  new ProductViewModel()
                                                  {
                                                      ColorProductId = p.ColorProductId,
                                                      ProductId = p.ProductId,
                                                      ImageId = p.ImageId,
                                                      DetailImage = p.ImageUrl,
                                                      isDefault = p.isDefault
                                                  }).ToList();
            }
            //Lưu lại mã màu
            ViewBag.ColorProductId = ColorProductId;
            return PartialView(detailColorList);
        }

        //change default image
        public ActionResult ChangeDefaultImage(Guid? ColorProductId, Guid? ImageId)
        {
            var imageList = _context.ImageProductModel.Where(p => p.ColorProductId == ColorProductId).ToList();
            foreach (var item in imageList)
            {
                if (item.ImageId == ImageId)
                {
                    item.isDefault = true;
                }
                else
                {
                    item.isDefault = null;
                }
                _context.Entry(item).State = EntityState.Modified;
                _context.SaveChanges();
            }
            var detailColorList = imageList.Select(p =>
                                            new ProductViewModel()
                                            {
                                                ColorProductId = p.ColorProductId,
                                                ProductId = p.ProductId,
                                                ImageId = p.ImageId,
                                                DetailImage = p.ImageUrl,
                                                isDefault = p.isDefault
                                            }).ToList();
            return PartialView("_ImageDetailsInner", detailColorList);
        }

        //insert image
        public ActionResult InsertImage(HttpPostedFileBase DetailImage, Guid popupColorProductId, Guid ProductId)
        {
            var ColorProductId = popupColorProductId;

            //insert image
            ImageProductModel model = new ImageProductModel();
            model.ImageId = Guid.NewGuid();
            model.ColorProductId = ColorProductId;
            model.ProductId = ProductId;
            model.ImageUrl = Upload(DetailImage, "Color");

            _context.Entry(model).State = EntityState.Added;
            _context.SaveChanges();
            //select image list
            List<ProductViewModel> detailColorList = _context.ImageProductModel.Where(p => p.ColorProductId == ColorProductId)
                                                  .Select(p =>
                                                  new ProductViewModel()
                                                  {
                                                      ColorProductId = p.ColorProductId,
                                                      ProductId = p.ProductId,
                                                      ImageId = p.ImageId,
                                                      DetailImage = p.ImageUrl,
                                                      isDefault = p.isDefault
                                                  }).ToList();
            return PartialView("_ImageDetailsInner", detailColorList);
        }

        //delete image
        public ActionResult DeleteImage(Guid? ColorProductId, Guid? ImageId)
        {
            //delete image
            var image = _context.ImageProductModel.FirstOrDefault(p => p.ImageId == ImageId);
            if (image != null)
            {
                _context.Entry(image).State = EntityState.Deleted;
                _context.SaveChanges();
            }
            //select image list
            var detailColorList = _context.ImageProductModel.Where(p => p.ColorProductId == ColorProductId)
                                                            .Select(p =>
                                                            new ProductViewModel()
                                                            {
                                                                ColorProductId = p.ColorProductId,
                                                                ProductId = p.ProductId,
                                                                ImageId = p.ImageId,
                                                                DetailImage = p.ImageUrl,
                                                                isDefault = p.isDefault
                                                            }).ToList();
            return PartialView("_ImageDetailsInner", detailColorList);
        }
        #endregion Modal Popup Image

        #region Detail Partial Properties
        public ActionResult _ProductProperties(Guid? ProductId, int mode)
        {
            ViewBag.Mode = mode;
            var colorAllList = _context.ColorModel.Where(p => p.Actived == true)
                                                  .Select(p =>
                                                  new
                                                  {
                                                      Id = p.ColorCode,
                                                      Name = p.ColorName,
                                                      OrderIndex = p.OrderIndex
                                                  }).OrderBy(p => p.OrderIndex).ToList();
            ViewBag.SubjectColor = new SelectList(colorAllList, "Id", "Name");
            ViewBag.DescriptionColor = new SelectList(colorAllList, "Id", "Name");
            ViewBag.BackgroundColor = new SelectList(colorAllList, "Id", "Name");

            //Edit
            if (mode != 1)
            {
                List<ProductViewModel> propertiesList = new List<ProductViewModel>();
                if (ProductId != null)
                {
                    propertiesList = (from p in _context.PropertiesProductModel
                                      join pr in _context.ProductModel on p.ProductId equals pr.ProductId
                                      where p.ProductId == ProductId
                                      select new ProductViewModel()
                                      {
                                          PropertiesId = p.PropertiesId,
                                          ProductId = pr.ProductId,
                                          Subject = p.Subject,
                                          SubjectColor = p.SubjectColor,
                                          Description = p.Description,
                                          DescriptionColor = p.DescriptionColor,
                                          Image = p.Image,
                                          BackgroundColor = p.BackgroundColor,
                                          X = p.X,
                                          Y = p.Y,
                                          ProductCode = pr.ProductCode,
                                          ProductName = pr.ProductName,
                                          ERPProductCode = pr.ERPProductCode
                                      }).ToList();
                }
                return PartialView(propertiesList);
            }
            //Create
            else
            {
                return PartialView();
            }
        }

        public ActionResult _ProductProperties2(Guid? ProductId, int mode)
        {

            // Load first image
            var product = _context.ProductModel.Find(ProductId);
            ViewBag.ProductId = ProductId;
            if (product == null || product.ImageUrl == null)
            {
                ViewBag.PropertiesImageUrl = "noimage.jpg";
            }
            else
            {
                ViewBag.PropertiesImageUrl = product.ImageUrl;
            }

            // Set to the pointer_div
            //Edit
            if (mode != 1)
            {
                List<PropertiesViewModel> propertiesList = new List<PropertiesViewModel>();
                if (ProductId != null)
                {
                    propertiesList = (from p in _context.PropertiesProductModel
                                      join pr in _context.ProductModel on p.ProductId equals pr.ProductId
                                      where p.ProductId == ProductId
                                      select new PropertiesViewModel
                                      {
                                          PropertiesId = p.PropertiesId,
                                          X = p.X,
                                          Y = p.Y,
                                          Subject = p.Subject,
                                          Description = p.Description,
                                          Image = p.Image
                                      }).ToList();
                }


                return PartialView(propertiesList);
            }
            //Create
            else
            {
                return PartialView();
            }
        }

        public ActionResult PropertiesSave()
        {
            try
            {
                // Upload
                // Save
                // Get Data
                // Return

                var model = new PropertiesProductModel();
                model.PropertiesId = Guid.NewGuid();
                model.ProductId = new Guid(Request.Params["ProductId"]);
                model.Subject = Request.Params["Subject"];
                model.Description = Request.Params["Description"];
                model.X = Request.Params["X"];
                model.Y = Request.Params["Y"];
                var fileContent = Request.Files[0];
                var uploadedImage = Upload(fileContent, "Properties");
                model.Image = uploadedImage;

                _context.PropertiesProductModel.Add(model);
                _context.SaveChanges();

                var vm = new PropertiesViewModel()
                {
                    PropertiesId = model.PropertiesId,
                    X = model.X,
                    Y = model.Y,
                    Subject = model.Subject,
                    Description = model.Description,
                    Image = model.Image
                };

                return Json(vm, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json("Upload failed");
            }
        }

        public ActionResult PropertiesRemove()
        {

            try
            {
                Guid id = new Guid(Request.Params["PropertiesId"]);
                var ProductId = new Guid(Request.Params["ProductId"]);

                var model = _context.PropertiesProductModel.SingleOrDefault(p => p.PropertiesId == id);

                // Remove 1 properties
                if (model != null)
                {
                    var thumparth = Server.MapPath("~/Upload/Properties/" + model.Image);
                    if (System.IO.File.Exists(thumparth))
                    {
                        System.IO.File.Delete(thumparth);
                    }

                    _context.PropertiesProductModel.Remove(model);
                    _context.SaveChanges();
                }

                // Retrieve all the model agains
                List<PropertiesViewModel> propertiesList = new List<PropertiesViewModel>();
                if (ProductId != null)
                {
                    propertiesList = (from p in _context.PropertiesProductModel
                                      join pr in _context.ProductModel on p.ProductId equals pr.ProductId
                                      where p.ProductId == ProductId
                                      select new PropertiesViewModel
                                      {
                                          PropertiesId = p.PropertiesId,
                                          X = p.X,
                                          Y = p.Y,
                                          Subject = p.Subject,
                                          Description = p.Description,
                                          Image = p.Image
                                      }).ToList();
                }

                return Json(propertiesList, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {
                return Json("Remove failed");
            }

        }

        //list product to show on table
        public ActionResult _ProductPropertiesInner(List<ProductViewModel> propertiesList = null)
        {
            if (propertiesList == null)
            {
                propertiesList = new List<ProductViewModel>();
            }
            return PartialView(propertiesList);
        }

        //delete row detail
        public ActionResult DeleteProductProperties(List<ProductViewModel> propertiesList, int STT)
        {
            var List = propertiesList.Where(p => p.STT != STT).ToList();
            return PartialView("_ProductPropertiesInner", List);
        }

        //insert row detail
        public ActionResult InsertProductProperties(List<ProductViewModel> propertiesList, string Subject, string SubjectColor, string Description, string DescriptionColor, HttpPostedFileBase Image, string BackgroundColor, string X, string Y, string ProductCode, string ProductName, string ERPProductCode)
        {
            if (propertiesList == null)
            {
                propertiesList = new List<ProductViewModel>();
            }
            ProductViewModel viewModel = new ProductViewModel();
            viewModel.Subject = Subject;
            viewModel.SubjectColor = SubjectColor;
            viewModel.Description = Description;
            viewModel.DescriptionColor = DescriptionColor;
            viewModel.Image = Upload(Image, "Properties");
            viewModel.BackgroundColor = BackgroundColor;
            viewModel.X = X;
            viewModel.Y = Y;
            viewModel.ProductCode = ProductCode;
            viewModel.ProductName = ProductName;
            viewModel.ERPProductCode = ERPProductCode;

            propertiesList.Add(viewModel);
            return PartialView("_ProductPropertiesInner", propertiesList);
        }
        #endregion Detail Partial Properties

        #region Detail Partial Specifications
        public ActionResult _ProductSpecifications(Guid? ProductId, int mode)
        {
            ViewBag.Mode = mode;
            var specificationsAllList = _context.SpecificationsModel.Where(p => p.Actived == true)
                                                .Select(p =>
                                                new
                                                {
                                                    Id = p.SpecificationsId,
                                                    Name = p.SpecificationsName,
                                                    OrderIndex = p.OrderIndex
                                                }).OrderBy(p => p.OrderIndex).ToList();
            ViewBag.SpecificationsId = new SelectList(specificationsAllList, "Id", "Name");
            //Edit
            if (mode != 1)
            {
                List<ProductViewModel> specificationsList = new List<ProductViewModel>();
                if (ProductId != null)
                {
                    specificationsList = (from p in _context.SpecificationsProductModel
                                          join pr in _context.ProductModel on p.ProductId equals pr.ProductId
                                          join sp in _context.SpecificationsModel on p.SpecificationsId equals sp.SpecificationsId
                                          where p.ProductId == ProductId
                                          orderby sp.OrderIndex
                                          select new ProductViewModel()
                                          {
                                              SpecificationsId = p.SpecificationsId,
                                              SpecificationsProductId = p.SpecificationsProductId,
                                              ProductId = pr.ProductId,
                                              SpecificationsName = sp.SpecificationsName,
                                              SpecificationsDescription = p.Description,
                                              ProductCode = pr.ProductCode,
                                              ProductName = pr.ProductName,
                                              ERPProductCode = pr.ERPProductCode
                                          }).ToList();
                }
                return PartialView(specificationsList);
            }
            //Create
            else
            {
                return PartialView();
            }
        }

        //list product to show on table
        public ActionResult _ProductSpecificationsInner(List<ProductViewModel> specificationsList = null)
        {
            if (specificationsList == null)
            {
                specificationsList = new List<ProductViewModel>();
            }
            return PartialView(specificationsList);
        }

        //delete row detail
        public ActionResult DeleteProductSpecifications(List<ProductViewModel> specificationsList, int STT)
        {
            var List = specificationsList.Where(p => p.STT != STT).ToList();
            return PartialView("_ProductSpecificationsInner", List);
        }

        //insert row detail
        public ActionResult InsertProductSpecifications(List<ProductViewModel> specificationsList, Guid SpecificationsId, string SpecificationsDescription, string ProductCode, string ProductName, string ERPProductCode)
        {
            if (specificationsList == null)
            {
                specificationsList = new List<ProductViewModel>();
            }

            //if product exist in list not insert
            var SpecificationsIdList = specificationsList.Select(p => p.SpecificationsId).ToList();
            if (!SpecificationsIdList.Contains(SpecificationsId))
            {
                ProductViewModel viewModel = new ProductViewModel();
                viewModel.SpecificationsId = SpecificationsId;
                var specification = _context.SpecificationsModel.FirstOrDefault(p => p.SpecificationsId == SpecificationsId);
                if (specification != null)
                {
                    viewModel.SpecificationsName = specification.SpecificationsName;
                }
                viewModel.SpecificationsDescription = SpecificationsDescription;
                viewModel.ProductCode = ProductCode;
                viewModel.ProductName = ProductName;
                viewModel.ERPProductCode = ERPProductCode;

                specificationsList.Add(viewModel);
            }
            return PartialView("_ProductSpecificationsInner", specificationsList);
        }
        #endregion Detail Partial Specifications

        #region Detail Partial Accessory
        public ActionResult _ProductAccessory(Guid? ProductId, int mode)
        {
            ViewBag.Mode = mode;
            //AccessoryCategory
            var accCategoryList = _context.AccessoryCategoryModel.Where(p => p.Actived == true)
                                          .Select(p =>
                                          new
                                          {
                                              Id = p.AccessoryCategoryId,
                                              Name = p.AccessoryCategoryName,
                                              OrderIndex = p.OrderIndex
                                          }).OrderBy(p => p.OrderIndex).ToList();
            ViewBag.AccessoryCategoryId = new SelectList(accCategoryList, "Id", "Name");
            //Edit
            if (mode != 1)
            {
                List<ProductViewModel> accessoryList = new List<ProductViewModel>();
                if (ProductId != null)
                {
                    accessoryList = (from p in _context.AccessoryProductModel
                                     join pr in _context.ProductModel on p.ProductId equals pr.ProductId
                                     join ac in _context.AccessoryModel on p.AccessoryId equals ac.AccessoryId
                                     join acCate in _context.AccessoryCategoryModel on ac.AccessoryCategoryId
                                                                                    equals acCate.AccessoryCategoryId
                                     where p.ProductId == ProductId
                                     orderby ac.OrderIndex, acCate.OrderIndex
                                     select new ProductViewModel()
                                     {
                                         AccessoryId = p.AccessoryId,
                                         AccessoryName = ac.AccessoryName,
                                         AccessoryCategoryId = ac.AccessoryCategoryId,
                                         AccessoryCategoryName = acCate.AccessoryCategoryName,
                                         ProductId = p.ProductId,
                                         SampleValue = pr.SampleValue,
                                         ProcessingValue = pr.ProcessingValue,
                                         Price = p.Price,
                                         ProductCode = pr.ProductCode,
                                         ProductName = pr.ProductName,
                                         ERPProductCode = pr.ERPProductCode
                                     }).ToList();
                }
                return PartialView(accessoryList);
            }
            //Create
            else
            {
                return PartialView();
            }
        }

        //list product to show on table
        public ActionResult _ProductAccessoryInner(List<ProductViewModel> accessoryList = null)
        {
            if (accessoryList == null)
            {
                accessoryList = new List<ProductViewModel>();
            }
            return PartialView(accessoryList);
        }

        //delete row detail
        public ActionResult DeleteProductAccessory(List<ProductViewModel> accessoryList, int STT)
        {
            var List = accessoryList.Where(p => p.STT != STT).ToList();
            return PartialView("_ProductAccessoryInner", List);
        }

        //insert row detail
        public ActionResult InsertProductAccessory(List<ProductViewModel> accessoryList, Guid AccessoryId, decimal? Price, string ProductCode, string ProductName, string ERPProductCode)
        {
            if (accessoryList == null)
            {
                accessoryList = new List<ProductViewModel>();
            }

            var AccessoryIdList = accessoryList.Select(p => p.AccessoryId).ToList();

            // Tien NOTE: Nếu đã tồn tại thì không làm gì, còn chưa tồn tại thì thêm vô
            if (!AccessoryIdList.Contains(AccessoryId))
            {
                ProductViewModel viewModel = new ProductViewModel();
                viewModel.AccessoryId = AccessoryId;
                var accessory = (from p in _context.AccessoryModel
                                 join ac in _context.AccessoryCategoryModel on p.AccessoryCategoryId equals ac.AccessoryCategoryId
                                 where p.AccessoryId == AccessoryId
                                 select new
                                 {
                                     AccessoryName = p.AccessoryName,
                                     AccessoryCategoryName = ac.AccessoryCategoryName
                                 }).FirstOrDefault();
                viewModel.AccessoryName = accessory.AccessoryName;
                viewModel.AccessoryCategoryName = accessory.AccessoryCategoryName;
                viewModel.Price = Price;
                viewModel.ProcessingValue = Price;
                viewModel.SampleValue = Price;
                viewModel.ProductCode = ProductCode;
                viewModel.ProductName = ProductName;
                viewModel.ERPProductCode = ERPProductCode;

                accessoryList.Add(viewModel);
            }
            return PartialView("_ProductAccessoryInner", accessoryList);
        }
        #endregion Detail Partial Accessory

        #region Detail Partial Promotion
        public ActionResult _ProductPromotion(Guid? ProductId)
        {
            var promotion = (from p in _context.ProductModel
                             from m in p.PromotionModel
                             where p.ProductId == ProductId
                             orderby m.EffectToDate descending
                             select new ProductViewModel()
                             {
                                 ProductId = p.ProductId,
                                 PromotionId = m.PromotionId,
                                 PromotionName = m.PromotionName,
                                 EffectFromDate = m.EffectFromDate,
                                 EffectToDate = m.EffectToDate
                             }).ToList();

            return PartialView(promotion);
        }
        public ActionResult _ProductCustomerPromotion(Guid? id)
        {
            var promotion = (from p in _context.ProductModel
                             from m in p.CustomerPromotionModel
                             where p.ProductId == id
                             orderby m.EffectToDate descending
                             select new ProductViewModel()
                             {
                                 ProductId = p.ProductId,
                                 PromotionId = m.PromotionId,
                                 PromotionName = m.PromotionName,
                                 EffectFromDate = m.EffectFromDate,
                                 EffectToDate = m.EffectToDate
                             }).ToList();

            return PartialView(promotion);
        }
        #endregion Detail Partial Promotion

        #region Detail Partial Price
        public ActionResult _ProductPrice(Guid? ProductId)
        {
            var priceList = (from p in _context.PriceProductModel
                             join c in _context.ColorModel on p.MainColorId equals c.ColorId into cList
                             from color in cList.DefaultIfEmpty()
                             join s in _context.StyleModel on p.StyleId equals s.StyleId into sList
                             from style in sList.DefaultIfEmpty()
                             where p.ProductId == ProductId
                             select new PriceProductViewModel()
                             {
                                 PriceProductCode = p.PriceProductCode,
                                 Price = p.Price,
                                 PostDate = p.PostDate,
                                 PostTime = p.PostTime,
                                 UserPost = p.UserPost,
                                 ColorName = color == null ? "" : (color.ColorShortName + " | " + color.ColorName),
                                 StyleName = style == null ? "" : style.StyleName
                             }).ToList();

            if (priceList == null)
            {
                priceList = new List<PriceProductViewModel>();
            }
            return PartialView(priceList);
        }
        #endregion Detail Partial Price

        #region Detail Partial Stock Warehouse
        public ActionResult _ProductWarehouse(Guid? ProductId)
        {
            var stockList = (from p in _context.WarehouseProductModel.AsEnumerable()
                                 //Warehouse
                             join w in _context.WarehouseModel on p.WarehouseId equals w.WarehouseId
                             //Color: MainColor
                             join c in _context.ColorModel on p.MainColorId equals c.ColorId
                             //Style
                             join s in _context.StyleModel on p.StyleId equals s.StyleId into sg
                             from s1 in sg.DefaultIfEmpty()
                             where p.ProductId == ProductId && w.Actived == true
                             select new ProductViewModel()
                             {
                                 WarehouseName = w.WarehouseName,
                                 MainColorProductCode = c.ColorCode,
                                 MainColorShortName = c.ColorShortName,
                                 MainColorProductName = c.ColorName,
                                 StyleWarehouseName = s1 == null ? "" : s1.StyleName,
                                 Quantity = p.Quantity,
                                 ProductWarehousePostDate = (p.PostDate != null && p.PostTime != null) ? p.PostDate + p.PostTime : null,
                                 ProductWarehouseUserPost = p.UserPost
                             }).ToList();

            if (stockList == null)
            {
                stockList = new List<ProductViewModel>();
            }
            return PartialView(stockList);
        }
        #endregion Detail Partial Stock Warehouse

        #region CreateViewBag, Helper
        public void CreateViewBag(Guid? ParentCategoryId = null, Guid? CategoryId = null, Guid? ConfigurationId = null, Guid? CompanyId = null, string Color = null, Guid? warrantyId = null)
        {
            var PhanLoaiVatTu = _context.CategoryModel.Where(p => p.CategoryCode == ConstCategoryCode.PHANLOAIVATTU).FirstOrDefault();
            var NhomVatTu = _context.CategoryModel.Where(p => p.CategoryCode == ConstCategoryCode.NHOMVATTU).FirstOrDefault();

            //Get list Brand
            var brandList = _context.CategoryModel.Where(p => p.ParentCategoryId == PhanLoaiVatTu.CategoryId && p.Actived == true)
                                                  .Select(p => new
                                                  {
                                                      CategoryId = p.CategoryId,
                                                      CategoryCode = p.CategoryCode,
                                                      CategoryName = p.CategoryCode + " | " + p.CategoryName,
                                                      OrderIndex = p.OrderIndex
                                                  })
                                                  .OrderBy(p => p.CategoryCode).ToList();
            ViewBag.ParentCategoryId = new SelectList(brandList, "CategoryId", "CategoryName", ParentCategoryId);

            //Get list CategoryId
            //if (ParentCategoryId != null)
            //{
            //    var categoryList = _context.CategoryModel.Where(p => p.ParentCategoryId == ParentCategoryId && p.Actived == true)
            //                                             .Select(p => new
            //                                             {
            //                                                 CategoryId = p.CategoryId,
            //                                                 CategoryCode = p.CategoryCode,
            //                                                 CategoryName = p.CategoryCode + " | " + p.CategoryName,
            //                                                 OrderIndex = p.OrderIndex
            //                                             })
            //                                             .OrderBy(p => p.CategoryCode).ToList();
            //    ViewBag.CategoryId = new SelectList(categoryList, "CategoryId", "CategoryName", CategoryId);
            //}
            var categoryList = _context.CategoryModel.Where(p => p.ParentCategoryId == NhomVatTu.CategoryId && p.Actived == true)
                                                         .Select(p => new
                                                         {
                                                             CategoryId = p.CategoryId,
                                                             CategoryCode = p.CategoryCode,
                                                             CategoryName = p.CategoryCode + " | " + p.CategoryName,
                                                             OrderIndex = p.OrderIndex
                                                         })
                                                         .OrderBy(p => p.CategoryCode).ToList();
            ViewBag.CategoryId = new SelectList(categoryList, "CategoryId", "CategoryName", CategoryId);

            //Get list Configuration
            //var configList = _context.ConfigurationModel.Where(p => p.Actived == true).OrderBy(p => p.OrderIndex).ToList();
            //ViewBag.ConfigurationId = new SelectList(configList, "ConfigurationId", "ConfigurationName", ConfigurationId);

            //Loại xe
            //var productTypeList = _context.ProductTypeModel.ToList();
            //ViewBag.ProductTypeId = new SelectList(productTypeList, "ProductTypeId", "ProductTypeName", productTypeId);

            var companyLst = _context.CompanyModel.Where(p => p.Actived == true)
                                     .Select(p => new
                                     {
                                         CompanyId = p.CompanyId,
                                         CompanyCode = p.CompanyCode,
                                         CompanyName = p.CompanyCode + " | " + p.CompanyName
                                     })
                                     .OrderBy(p => p.CompanyCode)
                                     .ToList();
            ViewBag.CompanyId = new SelectList(companyLst, "CompanyId", "CompanyName", CompanyId);

            //Màu sắc
            var colorList = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.ProductColor);
            ViewBag.Color = new SelectList(colorList, "CatalogCode", "CatalogCode", Color);

            //Thời gian bảo hành
            var warrantyList = _context.WarrantyModel.Where(p => p.Actived == true).OrderBy(p => p.Duration).ToList();
            ViewBag.WarrantyId = new SelectList(warrantyList, "WarrantyId", "WarrantyName", warrantyId);
        }
        //GetCategoryByBrand
        public ActionResult GetCategoryByBrand(Guid? ParentCategoryId = null)
        {
            //Get list CategoryId
            var categoryList = _context.CategoryModel.Where(p => p.Actived == true && p.ParentCategoryId == ParentCategoryId)
                                                     .Select(p => new
                                                     {
                                                         CategoryId = p.CategoryId,
                                                         CategoryCode = p.CategoryCode,
                                                         CategoryName = p.CategoryCode + " | " + p.CategoryName,
                                                         OrderIndex = p.OrderIndex
                                                     })
                                                     .OrderBy(p => p.CategoryCode).ToList();
            var CategoryIdList = new SelectList(categoryList, "CategoryId", "CategoryName");

            return Json(CategoryIdList, JsonRequestBehavior.AllowGet);
        }
        //GetAccessoryByCategory
        public ActionResult GetAccessoryByCategory(Guid? AccessoryCategoryId = null)
        {
            //Accessory
            var AccessoryList = _context.AccessoryModel.Where(p => p.Actived == true && p.AccessoryCategoryId == AccessoryCategoryId)
                                          .Select(p =>
                                          new
                                          {
                                              Id = p.AccessoryId,
                                              Name = p.AccessoryName,
                                              OrderIndex = p.OrderIndex
                                          }).OrderBy(p => p.OrderIndex).ToList();
            var accList = new SelectList(AccessoryList, "Id", "Name");

            return Json(accList, JsonRequestBehavior.AllowGet);
        }
        //Get Color list by ProductId
        private List<ProductViewModel> GetColorListBy(Guid? ProductId)
        {
            List<ProductViewModel> colorList;
            colorList = //Danh sách màu
                                    (from colorproductlist in _context.ColorProductModel
                                         //sản phẩm
                                     join product in _context.ProductModel on colorproductlist.ProductId equals product.ProductId
                                     //kiểu dáng
                                     join stmp in _context.StyleModel on colorproductlist.StyleId equals stmp.StyleId into slist
                                     from style in slist.DefaultIfEmpty()
                                         //Màu chính
                                     join color in _context.ColorModel on colorproductlist.MainColorId equals color.ColorId
                                     //Ảnh đại diện của màu sắc theo phiên bản
                                     join imgtmp in _context.ImageProductModel on new { colorproductlist.ColorProductId, colorproductlist.ProductId } equals new { imgtmp.ColorProductId, imgtmp.ProductId } into imgLst
                                     from image in imgLst.Where(p => p.isDefault == true).DefaultIfEmpty()
                                         //get by productid
                                     where colorproductlist.ProductId == ProductId
                                     select new ProductViewModel()
                                     {
                                         ProductId = product.ProductId,
                                         ProductCode = product.ProductCode,
                                         //Style
                                         StyleId = colorproductlist.StyleId,
                                         StyleName = style.StyleName,
                                         //MainColor
                                         MainColorId = colorproductlist.MainColorId,
                                         MainColorCode = color.ColorCode,
                                         MainColorShortName = color.ColorShortName,
                                         MainColorName = color.ColorName,
                                         //Number Of Image
                                         NumberOfImage = imgLst.Count(),
                                         //Default image url
                                         ColorStyleImage = image == null ? ConstImageUrl.noImage : image.ImageUrl,
                                         ColorProductId = colorproductlist.ColorProductId,
                                     }).ToList();
            return colorList;
        }


        /// <summary>
        /// Lấy danh sách mã màu (dùng cho autocomplete)
        /// </summary>
        /// <returns></returns>
        public ActionResult SearchProductColorList(string search)
        {
            var result = new List<ISDSelectItem2>();
            result = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.ProductColor)
                                                                .Where(p => search == null || p.CatalogCode.ToLower().Contains(search.ToLower()))
                                                                .Select(p => new ISDSelectItem2()
                                                                {
                                                                    value = p.CatalogCode,
                                                                    text = p.CatalogCode
                                                                }).Take(10).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Lấy danh sách mã sap sản phẩm (dùng cho autocomplete)
        /// </summary>
        /// <returns></returns>
        public ActionResult SearchERPProductForAutocomple(string search)
        {
            var result = new List<ISDSelectItem2>();
            result = (from p in _context.ProductModel
                      where p.Actived == true
                      && (search == null || p.ERPProductCode.Contains(search))
                      && p.CompanyId == CurrentUser.CompanyId 
                      orderby p.ERPProductCode
                      select new ISDSelectItem2
                      {
                          text = p.ERPProductCode,
                          value = p.ERPProductCode
                      }).Take(10).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SearchProductByCodeOrName(string SearchText)
        {
            var _productRepository = new ProductRepository(_context);
            var result = _productRepository.GetForAutocomple(SearchText);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SearchProductBy(string search, string type)
        {
            var _productRepository = new ProductRepository(_context);
            var result = _productRepository.GetForAutocomplete(search, type);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Remote Validation
        private bool IsExists(string ProductCode)
        {
            return (_context.ProductModel.FirstOrDefault(p => p.ProductCode == ProductCode) != null);
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult CheckExistingProductCode(string ProductCode, string ProductCodeValid)
        {
            try
            {
                if (ProductCodeValid != ProductCode)
                {
                    return Json(!IsExists(ProductCode));
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

        //Upload many image
        #region Upload multiple images
        public ActionResult UploadMultipleImages()
        {
            return View();
        }
        [HttpPost]
        public ActionResult UploadMultipleImages(bool? UploadType, List<HttpPostedFileBase> imageList)
        {
            return ExecuteContainer(() =>
            {
                try
                {
                    //get product not exist in db
                    List<string> notExistList = new List<string>();
                    //get image wrong name
                    List<string> wrongNameList = new List<string>();
                    //get ColorStyle not exist in db (ColorProductModel)
                    List<string> notExistColorStyle = new List<string>();
                    //messageList
                    var message = string.Empty;
                    List<string> messageList = new List<string>();

                    using (TransactionScope ts = new TransactionScope())
                    {
                        foreach (var image in imageList)
                        {
                            //get type of file
                            string extension = Path.GetExtension(image.FileName);
                            if (extension == ".jpg" || extension == ".png")
                            {
                                var originalFilename = Path.GetFileName(image.FileName);
                                var filename = originalFilename.Substring(0, originalFilename.IndexOf("."));

                                #region Upload product main images
                                if (UploadType == true)
                                {
                                    //get product code
                                    //originalFilename = MASP.jpg
                                    //filename = MASP
                                    //product code = MASP
                                    string ProductCode = filename;

                                    //check product code exist
                                    var product = _context.ProductModel.FirstOrDefault(p => p.ProductCode == ProductCode);
                                    if (product != null)
                                    {
                                        product.ImageUrl = Upload(image, "Color");
                                        _context.Entry(product).State = EntityState.Modified;
                                        //_context.SaveChanges();
                                    }
                                    else
                                    {
                                        notExistList.Add(ProductCode);
                                    }
                                }
                                #endregion Upload product main images

                                #region Upload ColorStyle images
                                else
                                {
                                    //get product code
                                    //originalFilename = MASP_MAUSAC_KIEUDANG_Note.jpg
                                    //filename = MASP_MAUSAC_KIEUDANG
                                    //product code = MASP
                                    //color code = MAUSAC
                                    //style code = KIEUDANG
                                    var part = filename.Split(new string[] { "_" }, StringSplitOptions.None);
                                    if (part.Length == 4)
                                    {
                                        //Product
                                        string ProductCode = part[0].ToString();
                                        Guid ProductId = Guid.Empty;
                                        var product = _context.ProductModel.FirstOrDefault(p => p.ProductCode == ProductCode);
                                        if (product != null)
                                        {
                                            ProductId = product.ProductId;
                                            //Color
                                            //Get list color in ColorProductModel by current product
                                            var ColorProduct = (from p in _context.ColorProductModel
                                                                join c in _context.ColorModel on p.MainColorId equals c.ColorId
                                                                where p.ProductId == ProductId
                                                                select new
                                                                {
                                                                    ColorId = c.ColorId,
                                                                    ColorShortName = c.ColorShortName
                                                                }).ToList();
                                            var ColorShortNameList = ColorProduct.Select(p => p.ColorShortName).ToList();

                                            string ColorCode = part[1].ToString();
                                            Guid ColorId = Guid.Empty;
                                            //Get ColorId base on ColorCode in ColorProduct, not get first color in db
                                            if (ColorShortNameList.Contains(ColorCode))
                                            {
                                                ColorId = ColorProduct.Where(p => p.ColorShortName == ColorCode)
                                                                      .Select(p => p.ColorId).FirstOrDefault();
                                            }
                                            //Style
                                            string StyleCode = part[2].ToString();
                                            Guid? StyleId = Guid.Empty;
                                            //ColorStyle has no style => StyleId = null
                                            if (StyleCode == "0")
                                            {
                                                StyleId = null;
                                            }
                                            //ColorStyle has style => get StyleId in db
                                            else
                                            {
                                                var style = _context.StyleModel.FirstOrDefault(p => p.StyleCode == StyleCode);
                                                if (style != null)
                                                {
                                                    StyleId = style.StyleId;
                                                }
                                            }
                                            //ColorStyle
                                            var ColorStyle = _context.ColorProductModel.Where(p => p.ProductId == ProductId
                                                                                                && p.MainColorId == ColorId
                                                                                                && p.StyleId == StyleId)
                                                                                       .FirstOrDefault();
                                            if (ColorStyle != null)
                                            {
                                                ImageProductModel model = new ImageProductModel();
                                                model.ImageId = Guid.NewGuid();
                                                model.ColorProductId = ColorStyle.ColorProductId;
                                                model.ProductId = ProductId;
                                                model.ImageUrl = Upload(image, "Color");
                                                _context.Entry(model).State = EntityState.Added;
                                            }
                                            else
                                            {
                                                //ColorStyle not exist in db
                                                notExistColorStyle.Add(filename);
                                            }
                                        }
                                        else
                                        {
                                            //ColorStyle not exist in db
                                            notExistColorStyle.Add(filename);
                                        }
                                    }
                                    else
                                    {
                                        //file named wrong
                                        wrongNameList.Add(filename);
                                    }
                                }
                                #endregion Upload ColorStyle images
                            }
                        }
                        //save changes when success
                        if (notExistList.Count == 0 && notExistColorStyle.Count == 0 && wrongNameList.Count == 0)
                        {
                            _context.SaveChanges();
                        }
                        ts.Complete();

                        #region Return
                        if (notExistList.Count > 0 || notExistColorStyle.Count > 0 || wrongNameList.Count > 0)
                        {
                            //return list of product not exist
                            if (notExistList.Count > 0)
                            {
                                message = string.Format(LanguageResource.Alert_Upload_Fail,
                                                        string.Join("', '", notExistList.ToArray()));
                                messageList.Add(message);
                            }
                            //return list of image not exist ColorStyle
                            if (notExistColorStyle.Count > 0)
                            {
                                message = string.Format(LanguageResource.Alert_Upload_NotExistColorStyle,
                                                        string.Join("', '", notExistColorStyle.ToArray()));
                                messageList.Add(message);
                            }
                            if (wrongNameList.Count > 0)
                            {
                                message = string.Format(LanguageResource.Alert_Upload_WrongName,
                                                        string.Join("', '", wrongNameList.ToArray()));
                                messageList.Add(message);
                            }
                            message = LanguageResource.Alert_Upload_Again;
                            messageList.Add(message);
                            return Json(new
                            {
                                Code = System.Net.HttpStatusCode.NotModified,
                                Success = false,
                                Data = messageList
                            });
                        }
                        //return success
                        else
                        {
                            message = LanguageResource.Alert_UploadMultiple_Success;
                            return Json(new
                            {
                                Code = System.Net.HttpStatusCode.Created,
                                Success = true,
                                Data = message
                            });
                        }
                        #endregion
                    }
                }
                catch (Exception ex)
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotModified,
                        Success = false,
                        Data = ex.Message
                    });
                }
            });
        }
        #endregion Upload multiple images

        //GET: /Product/ExportToExcel
        #region Export to excel
        public ActionResult ExportCreate()
        {
            List<ProductExcelViewModel> product = new List<ProductExcelViewModel>();
            List<ColorStyleProductExcelViewModel> productDetail = new List<ColorStyleProductExcelViewModel>();
            List<SpecificationProductExcelViewModel> productSpecification = new List<SpecificationProductExcelViewModel>();
            List<AccessoryProductExcelViewModel> productAccessory = new List<AccessoryProductExcelViewModel>();
            return Export(product, productDetail, productSpecification, productAccessory, false);
        }

        public ActionResult ExportEdit(ProductSearchViewModel searchViewModel)
        {
            //Get data filter
            searchViewModel = (ProductSearchViewModel)Session["frmSearchProduct"];
            //Get data from server
            var product = (from p in _context.ProductModel
                           join c in _context.CategoryModel on p.CategoryId equals c.CategoryId into caTmp
                           from ca in caTmp.DefaultIfEmpty()
                               //join br in _context.CategoryModel on c.ParentCategoryId equals br.CategoryId
                           join br in _context.CategoryModel on p.ParentCategoryId equals br.CategoryId
                           join com in _context.CompanyModel on p.CompanyId equals com.CompanyId into cmTmp
                           from cmp in cmTmp.DefaultIfEmpty()
                           join attr in _context.ProductAttributeModel on p.ProductId equals attr.ProductId into atg
                           from at in atg.DefaultIfEmpty()
                               //join cf in _context.ConfigurationModel on p.ConfigurationId equals cf.ConfigurationId
                           where
                           //search by ERPProductCode
                           (searchViewModel.ERPProductCode == null || p.ERPProductCode.Contains(searchViewModel.ERPProductCode))
                           //search by ProductCode
                           && (searchViewModel.ProductCode == null || p.ProductCode.Contains(searchViewModel.ProductCode))
                           //search by ProductName
                           && (searchViewModel.ProductName == null || p.ProductName.Contains(searchViewModel.ProductName))
                           //search by BrandId
                           //&& (searchViewModel.BrandId == null || p.BrandId == searchViewModel.BrandId)
                           //search by ParentCategoryId
                           && (searchViewModel.ParentCategoryId == null || p.ParentCategoryId == searchViewModel.ParentCategoryId)
                           //search by CategoryId
                           && (searchViewModel.CategoryId == null || p.CategoryId == searchViewModel.CategoryId)
                           //search by ConfigurationId
                           //&& (searchViewModel.ConfigurationId == null || p.ConfigurationId == searchViewModel.ConfigurationId)
                           //search by isHot
                           && (searchViewModel.isHot == null || p.isHot == searchViewModel.isHot)
                           //search by Actived
                           && (searchViewModel.Actived == null || p.Actived == searchViewModel.Actived)
                           orderby cmp.CompanyCode, p.ERPProductCode
                           select new ProductExcelViewModel()
                           {
                               ProductId = p.ProductId,
                               ERPProductCode = p.ERPProductCode,
                               ProductCode = p.ProductCode,
                               ProductName = p.ProductName,
                               ProductGroups = p.ProductGroups,
                               //BrandName = br.CategoryName,
                               ParentCategoryName = br.CategoryCode + "_" + br.CategoryName,
                               CategoryName = (ca.CategoryName != null) ? ca.CategoryCode + "_" + ca.CategoryName : null,
                               //CylinderCapacity = p.CylinderCapacity,
                               //GuaranteePeriod = p.GuaranteePeriod,
                               Color = at.Color,
                               OrderIndex = p.OrderIndex,
                               isHot = p.isHot,
                               Actived = p.Actived,
                               CompanyCode = cmp.CompanyCode,
                               Price = p.Price,
                               SampleValue = p.SampleValue,
                               ProcessingValue = p.ProcessingValue
                           }).ToList();

            //var productColorStyleDetail = (from p in product
            //                               join c in _context.ColorProductModel on p.ProductId equals c.ProductId
            //                               //Color
            //                               join color in _context.ColorModel on c.MainColorId equals color.ColorId
            //                               //Style
            //                               join s in _context.StyleModel on c.StyleId equals s.StyleId into sg
            //                               from style in sg.DefaultIfEmpty()
            //                               select new ColorStyleProductExcelViewModel()
            //                               {
            //                                   ColorProductId = c.ColorProductId,
            //                                   ProductName = p.ProductName,
            //                                   ColorName = color.ColorShortName + " | " + color.ColorName,
            //                                   StyleName = style != null ? style.StyleName : "",
            //                               }).ToList();

            //var productSpecification = (from pr in product
            //                            join p in _context.SpecificationsProductModel on pr.ProductId equals p.ProductId
            //                            join s in _context.SpecificationsModel on p.SpecificationsId equals s.SpecificationsId
            //                            select new SpecificationProductExcelViewModel()
            //                            {
            //                                SpecificationsProductId = p.SpecificationsProductId,
            //                                ProductName = pr.ProductName,
            //                                SpecificationsName = s.SpecificationsName,
            //                                Description = p.Description
            //                            }).ToList();

            //var productAccessory = (from pr in product
            //                        join pa in _context.AccessoryProductModel on pr.ProductId equals pa.ProductId
            //                        join a in _context.AccessoryModel on pa.AccessoryId equals a.AccessoryId
            //                        join ca in _context.AccessoryCategoryModel on a.AccessoryCategoryId equals ca.AccessoryCategoryId
            //                        select new AccessoryProductExcelViewModel()
            //                        {
            //                            AccessoryProductId = pa.AccessoryProductId,
            //                            ProductName = pr.ProductName,
            //                            AccessoryName = ca.AccessoryCategoryName + " - " + a.AccessoryName,
            //                            Price = pa.Price
            //                        }).ToList();

            return Export(product, null, null, null, true);
        }

        const string controllerCode = ConstExcelController.Product;
        const int startIndex = 8;
        const int startDetailIndex = 7;
        [ISDAuthorizationAttribute]
        public FileContentResult Export(List<ProductExcelViewModel> product, List<ColorStyleProductExcelViewModel> productDetail = null, List<SpecificationProductExcelViewModel> productSpecification = null, List<AccessoryProductExcelViewModel> productAccessory = null, bool isEdit = true)
        {
            var PhanLoaiVatTu = _context.CategoryModel.Where(p => p.CategoryCode == ConstCategoryCode.PHANLOAIVATTU).FirstOrDefault();
            var NhomVatTu = _context.CategoryModel.Where(p => p.CategoryCode == ConstCategoryCode.NHOMVATTU).FirstOrDefault();

            #region Dropdownlist
            #region Master
            List<DropdownModel> ParentCategoryId = (from c in _context.CategoryModel
                                                    where c.ParentCategoryId == PhanLoaiVatTu.CategoryId && c.Actived == true
                                                    orderby c.CategoryCode
                                                    select new DropdownModel()
                                                    {
                                                        Id = c.CategoryId,
                                                        Name = c.CategoryCode + "_" + c.CategoryName,
                                                    }).ToList();

            List<DropdownModel> CategoryId = (from c in _context.CategoryModel
                                              where c.ParentCategoryId == NhomVatTu.CategoryId && c.Actived == true
                                              orderby c.CategoryCode
                                              select new DropdownModel()
                                              {
                                                  Id = c.CategoryId,
                                                  Name = c.CategoryCode + "_" + c.CategoryName,
                                              }).OrderBy(p => p.Name).ToList();
            //Đời xe
            //List<DropdownModel> ConfigurationId = (from c in _context.ConfigurationModel
            //                                       where c.Actived == true
            //                                       orderby c.OrderIndex.HasValue descending, c.OrderIndex
            //                                       select new DropdownModel()
            //                                       {
            //                                           Id = c.ConfigurationId,
            //                                           Name = c.ConfigurationName,
            //                                       }).ToList();

            //Mã màu
            List<DropdownIdTypeStringModel> Color = (from c in _context.CatalogModel
                                                     where c.CatalogTypeCode == ConstCatalogType.ProductColor //&& c.Actived == true
                                                     orderby c.OrderIndex, c.CatalogCode
                                                     select new DropdownIdTypeStringModel()
                                                     {
                                                         Id = c.CatalogCode,
                                                         Name = c.CatalogCode
                                                     }).OrderBy(p => p.Name).ToList();
            #endregion

            #region Detail

            //Màu sắc
            //List<DropdownModel> ColorId = (from c in _context.ColorModel
            //                               where c.Actived == true
            //                               orderby c.OrderIndex.HasValue descending, c.OrderIndex
            //                               select new DropdownModel()
            //                               {
            //                                   Id = c.ColorId,
            //                                   Name = c.ColorShortName + " | " + c.ColorName,
            //                               }).ToList();
            ////Kiểu dáng
            //List<DropdownModel> StyleId = (from c in _context.StyleModel
            //                               where c.Actived == true
            //                               orderby c.OrderIndex.HasValue descending, c.OrderIndex
            //                               select new DropdownModel()
            //                               {
            //                                   Id = c.StyleId,
            //                                   Name = c.StyleName,
            //                               }).ToList();
            //Sản phẩm đã filter
            List<DropdownModel> ProductId = (from c in product
                                             orderby c.OrderIndex.HasValue descending, c.OrderIndex
                                             select new DropdownModel()
                                             {
                                                 Id = c.ProductId,
                                                 Name = c.ProductName,
                                             }).ToList();
            #endregion

            #region Specification
            //Thông số
            //List<DropdownModel> SpecificationId = (from p in _context.SpecificationsModel
            //                                       where p.Actived == true
            //                                       orderby p.OrderIndex.HasValue descending, p.OrderIndex
            //                                       select new DropdownModel()
            //                                       {
            //                                           Id = p.SpecificationsId,
            //                                           Name = p.SpecificationsName
            //                                       }).ToList();
            #endregion Specification

            #region Accessory
            //Thông số
            //List<DropdownModel> AccessoryId = (from p in _context.AccessoryModel
            //                                   join c in _context.AccessoryCategoryModel on p.AccessoryCategoryId equals c.AccessoryCategoryId
            //                                   where p.Actived == true
            //                                   orderby c.OrderIndex.HasValue descending, c.OrderIndex,
            //                                           p.OrderIndex.HasValue descending, p.OrderIndex
            //                                   select new DropdownModel()
            //                                   {
            //                                       Id = p.AccessoryId,
            //                                       Name = c.AccessoryCategoryName + " - " + p.AccessoryName
            //                                   }).ToList();
            #endregion Accessory
            #endregion

            //Columns to take
            List<ExcelTemplate> columns = new List<ExcelTemplate>();
            #region Master
            //Id
            columns.Add(new ExcelTemplate() { ColumnName = "ProductId", isAllowedToEdit = false });
            //Mã công ty
            columns.Add(new ExcelTemplate() { ColumnName = "CompanyCode", isAllowedToEdit = isEdit == true ? false : true });
            //Mã ERP
            columns.Add(new ExcelTemplate() { ColumnName = "ERPProductCode", isAllowedToEdit = isEdit == true ? false : true });
            //Mã sản phẩm
            columns.Add(new ExcelTemplate() { ColumnName = "ProductCode", isAllowedToEdit = true });
            //Tên sản phẩm
            columns.Add(new ExcelTemplate() { ColumnName = "ProductGroups", isAllowedToEdit = true });
            columns.Add(new ExcelTemplate() { ColumnName = "ProductName", isAllowedToEdit = true });
            //Phân loại vật tư
            columns.Add(new ExcelTemplate()
            {
                ColumnName = "ParentCategoryName",
                isAllowedToEdit = true,
                isDropdownlist = true,
                TypeId = ConstExcelController.GuidId,
                DropdownData = ParentCategoryId
            });
            //Nhóm vật tư
            columns.Add(new ExcelTemplate()
            {
                ColumnName = "CategoryName",
                isAllowedToEdit = true,
                isDropdownlist = true,
                TypeId = ConstExcelController.GuidId,
                DropdownData = CategoryId
            });
            ////Dung tích xi lanh
            //columns.Add(new ExcelTemplate() { ColumnName = "CylinderCapacity", isAllowedToEdit = true });
            ////Đời xe
            //columns.Add(new ExcelTemplate()
            //{
            //    ColumnName = "ConfigurationName",
            //    isAllowedToEdit = true,
            //    isDropdownlist = true,
            //    TypeId = ConstExcelController.GuidId,
            //    DropdownData = ConfigurationId
            //});
            ////Thời hạn bảo hành
            //columns.Add(new ExcelTemplate() { ColumnName = "GuaranteePeriod", isAllowedToEdit = true });
            //Mã màu
            //columns.Add(new ExcelTemplate()
            //{
            //    ColumnName = "Color",
            //    isAllowedToEdit = true,
            //    isDropdownlist = true,
            //    TypeId = ConstExcelController.StringId,
            //    DropdownIdTypeStringData = Color
            //});
            //Giá
            columns.Add(new ExcelTemplate() { ColumnName = "Price", isAllowedToEdit = true, isCurrency = true });
            //Giá gia công
            columns.Add(new ExcelTemplate() { ColumnName = "ProcessingValue", isAllowedToEdit = true, isCurrency = true });
            //Giá mẫu
            columns.Add(new ExcelTemplate() { ColumnName = "SampleValue", isAllowedToEdit = true, isCurrency = true });
            //Thứ tự hiển thị
            columns.Add(new ExcelTemplate() { ColumnName = "OrderIndex", isAllowedToEdit = true });
            //Sản phẩm nổi bật
            //columns.Add(new ExcelTemplate() { ColumnName = "isHot", isAllowedToEdit = true, isBoolean = true });
            //Trạng thái
            columns.Add(new ExcelTemplate() { ColumnName = "Actived", isAllowedToEdit = true, isBoolean = true });

            //Header
            string fileheader = string.Format(LanguageResource.Export_ExcelHeader, LanguageResource.Sale_Product);
            //List<ExcelHeadingTemplate> heading initialize in BaseController
            //Default:
            //          1. heading[0] is controller code
            //          2. heading[1] is file name
            //          3. headinf[2] is warning (edit)
            heading.Add(new ExcelHeadingTemplate()
            {
                Content = controllerCode,
                RowsToIgnore = 1,
                isWarning = false,
                isCode = true
            });
            heading.Add(new ExcelHeadingTemplate()
            {
                Content = fileheader.ToUpper(),
                RowsToIgnore = 1,
                isWarning = false,
                isCode = false
            });
            heading.Add(new ExcelHeadingTemplate()
            {
                Content = LanguageResource.Export_ExcelWarning1,
                RowsToIgnore = 0,
                isWarning = true,
                isCode = false
            });
            heading.Add(new ExcelHeadingTemplate()
            {
                Content = LanguageResource.Export_ExcelWarning2,
                RowsToIgnore = 0,
                isWarning = true,
                isCode = false
            });
            //Sản phẩm nổi bật
            //heading.Add(new ExcelHeadingTemplate()
            //{
            //    Content = LanguageResource.Export_ExcelWarningProductIsHot,
            //    RowsToIgnore = 0,
            //    isWarning = true,
            //    isCode = false
            //});
            //Trạng thái
            heading.Add(new ExcelHeadingTemplate()
            {
                Content = string.Format(LanguageResource.Export_ExcelWarningActived, LanguageResource.Sale_Product),
                RowsToIgnore = 1,
                isWarning = true,
                isCode = false
            });

            List<ExcelSheetTemplate> formatList = new List<ExcelSheetTemplate>();
            formatList.Add(new ExcelSheetTemplate()
            {
                ColumnsToTake = columns,
                Heading = heading,
                showSlno = true
            });
            #endregion Master

            #region Detail
            //List<ExcelTemplate> columnsDetail = new List<ExcelTemplate>();
            ////ProductName
            //columnsDetail.Add(new ExcelTemplate()
            //{
            //    ColumnName = "ProductName",
            //    isAllowedToEdit = true,
            //    isDropdownlist = true,
            //    TypeId = ConstExcelController.GuidId,
            //    DropdownData = ProductId
            //});
            ////Color
            //columnsDetail.Add(new ExcelTemplate()
            //{
            //    ColumnName = "ColorName",
            //    isAllowedToEdit = true,
            //    isDropdownlist = true,
            //    TypeId = ConstExcelController.GuidId,
            //    DropdownData = ColorId
            //});
            ////Style
            //columnsDetail.Add(new ExcelTemplate()
            //{
            //    ColumnName = "StyleName",
            //    isAllowedToEdit = true,
            //    isDropdownlist = true,
            //    TypeId = ConstExcelController.GuidId,
            //    DropdownData = StyleId
            //});
            //List<ExcelHeadingTemplate> heading2 = new List<ExcelHeadingTemplate>();
            //CreateExportHeader(heading2, LanguageResource.ExcelHeading_ColorStyleProduct, controllerCode);
            //formatList.Add(new ExcelSheetTemplate()
            //{
            //    ColumnsToTake = columnsDetail,
            //    Heading = heading2,
            //    showSlno = true
            //});
            #endregion Detail

            #region Specification
            //List<ExcelTemplate> columnsSpecification = new List<ExcelTemplate>();
            ////SpecificationsProductId
            //columnsSpecification.Add(new ExcelTemplate()
            //{
            //    ColumnName = "SpecificationsProductId",
            //    isAllowedToEdit = false,
            //});
            ////ProductName
            //columnsSpecification.Add(new ExcelTemplate()
            //{
            //    ColumnName = "ProductName",
            //    isAllowedToEdit = true,
            //    isDropdownlist = true,
            //    TypeId = ConstExcelController.GuidId,
            //    DropdownData = ProductId
            //});
            ////SpecificationId
            //columnsSpecification.Add(new ExcelTemplate()
            //{
            //    ColumnName = "SpecificationsName",
            //    isAllowedToEdit = true,
            //    isDropdownlist = true,
            //    TypeId = ConstExcelController.GuidId,
            //    DropdownData = SpecificationId
            //});
            //columnsSpecification.Add(new ExcelTemplate() { ColumnName = "Description", isAllowedToEdit = true });
            //List<ExcelHeadingTemplate> heading3 = new List<ExcelHeadingTemplate>();
            //CreateExportHeader(heading3, LanguageResource.ExcelHeading_Specification, controllerCode);
            //formatList.Add(new ExcelSheetTemplate()
            //{
            //    ColumnsToTake = columnsSpecification,
            //    Heading = heading3,
            //    showSlno = true
            //});
            #endregion Specification

            #region Accessory
            //List<ExcelTemplate> columnsAccessory = new List<ExcelTemplate>();
            ////AccessoryProductId
            //columnsAccessory.Add(new ExcelTemplate()
            //{
            //    ColumnName = "AccessoryProductId",
            //    isAllowedToEdit = false,
            //});
            ////ProductName
            //columnsAccessory.Add(new ExcelTemplate()
            //{
            //    ColumnName = "ProductName",
            //    isAllowedToEdit = true,
            //    isDropdownlist = true,
            //    TypeId = ConstExcelController.GuidId,
            //    DropdownData = ProductId
            //});
            ////AccessoryId
            //columnsAccessory.Add(new ExcelTemplate()
            //{
            //    ColumnName = "AccessoryName",
            //    isAllowedToEdit = true,
            //    isDropdownlist = true,
            //    TypeId = ConstExcelController.GuidId,
            //    DropdownData = AccessoryId
            //});
            //columnsAccessory.Add(new ExcelTemplate() { ColumnName = "Price", isAllowedToEdit = true });
            //List<ExcelHeadingTemplate> heading4 = new List<ExcelHeadingTemplate>();
            //CreateExportHeader(heading4, LanguageResource.ExcelHeading_Accessory, controllerCode);
            //formatList.Add(new ExcelSheetTemplate()
            //{
            //    ColumnsToTake = columnsAccessory,
            //    Heading = heading4,
            //    showSlno = true
            //});
            #endregion Accessory

            //Body
            byte[] filecontent;
            //if (isEdit == true)
            //{
            //    filecontent = ClassExportExcelProduct.ExportExcelProduct(formatList, product, productDetail, productSpecification, productAccessory);
            //}
            //else
            //{
            //    filecontent = ClassExportExcel.ExportExcel(product, columns, heading, true);
            //}
            filecontent = ClassExportExcel.ExportExcel(product, columns, heading, true);
            //File name
            //Insert => THEM_MOI
            //Edit => CAP_NHAT
            string exportType = LanguageResource.exportType_Insert;
            if (isEdit == true)
            {
                exportType = LanguageResource.exportType_Edit;
            }
            string fileNameWithFormat = string.Format("{0}_{1}.xlsx", exportType, _unitOfWork.UtilitiesRepository.RemoveSign4VietnameseString(fileheader.ToUpper()).Replace(" ", "_"));

            return File(filecontent, ClassExportExcel.ExcelContentType, fileNameWithFormat);
        }
        #endregion Export to excel

        //Import
        #region Import from excel

        #region Import
        [ISDAuthorizationAttribute]
        public ActionResult Import()
        {
            return ExcuteImportExcel(() =>
            {
                //dataset sắp xếp các sheet theo alphabet
                DataSet ds = GetDataSetFromExcel();
                List<string> errorList = new List<string>();
                if (ds.Tables != null && ds.Tables.Count > 0)
                {
                    using (TransactionScope ts = new TransactionScope())
                    {
                        //int startRowIndex = 0;
                        bool isDetail = false;
                        int sheet = 0;
                        for (int i = 0; i < ds.Tables.Count; i++)
                        {
                            DataTable dt = ds.Tables[i];
                            switch (i)
                            {
                                case 0:
                                    //startRowIndex = startIndex;
                                    break;
                                case 1:
                                    //startRowIndex = startDetailIndex;
                                    isDetail = true;
                                    sheet = 1;
                                    break;
                                case 2:
                                    //startRowIndex = startDetailIndex;
                                    isDetail = true;
                                    sheet = 2;
                                    break;
                                case 3:
                                    //startRowIndex = startDetailIndex;
                                    isDetail = true;
                                    sheet = 3;
                                    break;
                                default:
                                    //startRowIndex = startIndex;
                                    break;
                            }
                            //Get controller code from Excel file
                            string contCode = dt.Columns[0].ColumnName.ToString();
                            //Import data with accordant controller and action
                            //Nếu là sheet chính mới thực hiện update
                            if (sheet == 0)
                            {
                                if (contCode == controllerCode)
                                {
                                    var index = 0;
                                    foreach (DataRow dr in dt.Rows)
                                    {
                                        if (dt.Rows.IndexOf(dr) >= startIndex && !string.IsNullOrEmpty(dr.ItemArray[0].ToString()))
                                        {
                                            index++;
                                            //Check correct template
                                            if (isDetail == true)
                                            {
                                                //Màu sắc - Kiểu dáng
                                                if (sheet == 1)
                                                {
                                                    ColorStyleProductExcelViewModel colorProductIsValid = CheckTemplateDetail(dr.ItemArray, index);

                                                    if (!string.IsNullOrEmpty(colorProductIsValid.Error))
                                                    {
                                                        string error = colorProductIsValid.Error;
                                                        errorList.Add(error);
                                                    }
                                                    else
                                                    {
                                                        string result = ExecuteImportExcelProductDetail(colorProductIsValid);
                                                        if (result != LanguageResource.ImportSuccess)
                                                        {
                                                            errorList.Add(result);
                                                            //if file is unchanged => break foreach loop and return error
                                                            if (result == LanguageResource.Validation_ImportExcelUnchangeFile)
                                                            {
                                                                break;
                                                            }
                                                        }
                                                    }
                                                }
                                                //Thông số kỹ thuật
                                                if (sheet == 3)
                                                {
                                                    SpecificationProductExcelViewModel specificationIsValid = CheckTemplateSpecifications(dr.ItemArray, index);

                                                    if (!string.IsNullOrEmpty(specificationIsValid.Error))
                                                    {
                                                        string error = specificationIsValid.Error;
                                                        errorList.Add(error);
                                                    }
                                                    else
                                                    {
                                                        string result = ExecuteImportExcelSpecification(specificationIsValid);
                                                        if (result != LanguageResource.ImportSuccess)
                                                        {
                                                            errorList.Add(result);
                                                            //if file is unchanged => break foreach loop and return error
                                                            if (result == LanguageResource.Validation_ImportExcelUnchangeFile)
                                                            {
                                                                break;
                                                            }
                                                        }
                                                    }
                                                }
                                                //Phụ kiện
                                                if (sheet == 2)
                                                {
                                                    AccessoryProductExcelViewModel accIsValid = CheckTemplateAccessory(dr.ItemArray, index);

                                                    if (!string.IsNullOrEmpty(accIsValid.Error))
                                                    {
                                                        string error = accIsValid.Error;
                                                        errorList.Add(error);
                                                    }
                                                    else
                                                    {
                                                        string result = ExecuteImportExcelAccessory(accIsValid);
                                                        if (result != LanguageResource.ImportSuccess)
                                                        {
                                                            errorList.Add(result);
                                                            //if file is unchanged => break foreach loop and return error
                                                            if (result == LanguageResource.Validation_ImportExcelUnchangeFile)
                                                            {
                                                                break;
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                ProductExcelViewModel productIsValid = CheckTemplate(dr.ItemArray, index);

                                                if (!string.IsNullOrEmpty(productIsValid.Error))
                                                {
                                                    string error = productIsValid.Error;
                                                    errorList.Add(error);
                                                }
                                                else
                                                {
                                                    string result = ExecuteImportExcelProduct(productIsValid);
                                                    if (result != LanguageResource.ImportSuccess)
                                                    {
                                                        errorList.Add(result);
                                                        //if file is unchanged => break foreach loop and return error
                                                        if (result == LanguageResource.Validation_ImportExcelUnchangeFile)
                                                        {
                                                            break;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    string error = string.Format(LanguageResource.Validation_ImportCheckController, LanguageResource.Sale_Product);
                                    errorList.Add(error);
                                }
                            }
                        }
                        if (errorList != null && errorList.Count > 0)
                        {
                            return Json(new
                            {
                                Code = System.Net.HttpStatusCode.Created,
                                Success = false,
                                Data = errorList
                            });
                        }
                        ts.Complete();
                        return Json(new
                        {
                            Code = System.Net.HttpStatusCode.Created,
                            Success = true,
                            Data = LanguageResource.ImportSuccess
                        });
                    }
                }
                else
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.Created,
                        Success = false,
                        Data = LanguageResource.Validation_ImportExcelFile
                    });
                }
            });
        }
        #endregion Import

        #region Insert/Update data from excel file

        #region Master
        public string ExecuteImportExcelProduct(ProductExcelViewModel productIsValid)
        {
            //Check:
            //1. If Id == "" then => Insert
            //2. Else then => Update

            //Check loại xe thuộc hãng xe nào
            //var parentCategoryIsValid = _context.CategoryModel.Where(p => p.CategoryId == productIsValid.CategoryId).Select(p => p.ParentCategoryId).FirstOrDefault();
            //if (productIsValid.CategoryId != null && productIsValid.ParentCategoryId != parentCategoryIsValid)
            //{
            //    return string.Format(LanguageResource.Validation_ImportCategory_CRM, productIsValid.CategoryName, productIsValid.ParentCategoryName, productIsValid.RowIndex);
            //}

            #region Insert
            if (productIsValid.isNullValueId == true)
            {
                try
                {
                    var productCodeIsExist = _context.ProductModel.FirstOrDefault(p => p.ERPProductCode == productIsValid.ERPProductCode);
                    if (productCodeIsExist != null)
                    {
                        return string.Format(LanguageResource.Validation_Already_Exists, productIsValid.ProductCode);
                    }
                    else
                    {
                        ProductModel product = new ProductModel();
                        product.ProductId = Guid.NewGuid();
                        product.CompanyId = _context.CompanyModel.Where(p => p.CompanyCode == productIsValid.CompanyCode).Select(p => p.CompanyId).FirstOrDefault();
                        product.ERPProductCode = productIsValid.ERPProductCode;
                        product.ProductCode = productIsValid.ProductCode;
                        product.ProductGroups = productIsValid.ProductGroups;
                        product.ProductName = productIsValid.ProductName;
                        //product.BrandId = productIsValid.BrandId;
                        product.ParentCategoryId = productIsValid.ParentCategoryId;
                        product.CategoryId = productIsValid.CategoryId;
                        //product.CylinderCapacity = productIsValid.CylinderCapacity;
                        //product.ConfigurationId = (Guid)productIsValid.ConfigurationId;
                        //product.GuaranteePeriod = productIsValid.GuaranteePeriod;
                        product.OrderIndex = productIsValid.OrderIndex;
                        product.isHot = productIsValid.isHot;
                        product.Price = productIsValid.Price;
                        product.ProcessingValue = productIsValid.ProcessingValue;
                        product.SampleValue = productIsValid.SampleValue;
                        product.Actived = productIsValid.Actived;
                        _context.Entry(product).State = EntityState.Added;

                        ProductAttributeModel attr = new ProductAttributeModel();
                        attr.ProductId = product.ProductId;
                        attr.Color = productIsValid.Color;
                        _context.Entry(attr).State = EntityState.Added;
                    }
                }
                catch (Exception ex)
                {
                    if (ex.InnerException != null)
                    {
                        return string.Format("Thêm mới đã xảy ra lỗi: {0}", ex.InnerException.Message);
                    }
                    else
                    {
                        return string.Format("Thêm mới đã xảy ra lỗi: {0}", ex.Message);
                    }
                }
            }
            #endregion Insert

            #region Update
            else
            {
                try
                {
                    if (productIsValid.ParentCategoryId == null)
                    {
                        return LanguageResource.Validation_ImportExcelUnchangeFile;
                    }
                    else
                    {
                        ProductModel product = _context.ProductModel.FirstOrDefault(p => p.ProductId == productIsValid.ProductId);
                        if (product != null)
                        {
                            product.ERPProductCode = productIsValid.ERPProductCode;
                            product.ProductCode = productIsValid.ProductCode;
                            product.ProductGroups = productIsValid.ProductGroups;
                            product.ProductName = productIsValid.ProductName;
                            //product.BrandId = productIsValid.BrandId;
                            product.ParentCategoryId = productIsValid.ParentCategoryId;
                            product.CategoryId = productIsValid.CategoryId;
                            //product.CylinderCapacity = productIsValid.CylinderCapacity;
                            //product.ConfigurationId = (Guid)productIsValid.ConfigurationId;
                            //product.GuaranteePeriod = productIsValid.GuaranteePeriod;
                            product.OrderIndex = productIsValid.OrderIndex;
                            product.isHot = productIsValid.isHot;
                            product.Price = productIsValid.Price;
                            product.ProcessingValue = productIsValid.ProcessingValue;
                            product.SampleValue = productIsValid.SampleValue;
                            product.Actived = productIsValid.Actived;
                            _context.Entry(product).State = EntityState.Modified;

                            //Màu sắc
                            var existAttr = _context.ProductAttributeModel.FirstOrDefault(p => p.ProductId == productIsValid.ProductId);
                            if (existAttr != null)
                            {
                                existAttr.Color = productIsValid.Color;
                                _context.Entry(existAttr).State = EntityState.Modified;
                            }
                            else
                            {
                                ProductAttributeModel attr = new ProductAttributeModel();
                                attr.ProductId = productIsValid.ProductId;
                                attr.Color = productIsValid.Color;
                                _context.Entry(attr).State = EntityState.Added;
                            }
                        }
                        else
                        {
                            return string.Format(LanguageResource.Validation_ImportExcelIdNotExist,
                                                    LanguageResource.ProductId, productIsValid.ProductId,
                                                    string.Format(LanguageResource.Export_ExcelHeader,
                                                    LanguageResource.Sale_Product));
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (ex.InnerException != null)
                    {
                        return string.Format("Cập nhật đã xảy ra lỗi: {0}", ex.InnerException.Message);
                    }
                    else
                    {
                        return string.Format("Cập nhật đã xảy ra lỗi: {0}", ex.Message);
                    }
                }
            }
            #endregion Update

            _context.SaveChanges();
            return LanguageResource.ImportSuccess;
        }
        #endregion Master

        #region Detail
        List<ColorProductModel> colorProductIsExistList = new List<ColorProductModel>();
        public string ExecuteImportExcelProductDetail(ColorStyleProductExcelViewModel productDetailIsValid)
        {
            //Check:
            //1. If Id == "" then => Insert
            //2. Else then => Update
            #region Insert
            try
            {
                //Nếu màu sắc và kiểu dáng đã chưa có trong db => thêm mới
                var ColorStyle = _context.ColorProductModel.Where(p => p.MainColorId == productDetailIsValid.ColorId
                                                                    && p.StyleId == productDetailIsValid.StyleId)
                                                           .FirstOrDefault();
                if (ColorStyle == null)
                {
                    var product = _context.ProductModel.Where(p => p.ProductId == productDetailIsValid.ProductId)
                                                   .FirstOrDefault();
                    if (product != null)
                    {
                        ColorProductModel colorProduct = new ColorProductModel();
                        colorProduct.ColorProductId = Guid.NewGuid();
                        colorProduct.ProductId = productDetailIsValid.ProductId;
                        colorProduct.MainColorId = productDetailIsValid.ColorId;
                        colorProduct.StyleId = productDetailIsValid.StyleId;
                        _context.Entry(colorProduct).State = EntityState.Added;
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    return string.Format("Thêm mới đã xảy ra lỗi: {0}", ex.InnerException.Message);
                }
                else
                {
                    return string.Format("Thêm mới đã xảy ra lỗi: {0}", ex.Message);
                }
            }
            #endregion Insert

            _context.SaveChanges();
            return LanguageResource.ImportSuccess;
        }
        #endregion Detail

        #region Specification
        List<SpecificationsProductModel> spcificationIsExistList = new List<SpecificationsProductModel>();
        public string ExecuteImportExcelSpecification(SpecificationProductExcelViewModel specificationIsValid)
        {
            //Check:
            //1. If Id == "" then => Insert
            //2. Else then => Update
            #region Insert
            if (specificationIsValid.isNullValueId == true)
            {
                try
                {
                    //Check lỗi trùng
                    var specificationIsExist = _context.SpecificationsProductModel
                                                       .Where(p => p.ProductId == specificationIsValid.ProductId
                                                           && p.SpecificationsId == specificationIsValid.SpecificationsId)
                                                       .FirstOrDefault();
                    var product = _context.ProductModel.Where(p => p.ProductId == specificationIsValid.ProductId)
                                                       .FirstOrDefault();
                    if (specificationIsExist == null)
                    {
                        if (product != null)
                        {
                            SpecificationsProductModel model = new SpecificationsProductModel();
                            model.SpecificationsProductId = Guid.NewGuid();
                            model.ProductId = specificationIsValid.ProductId;
                            model.SpecificationsId = specificationIsValid.SpecificationsId;
                            model.Description = specificationIsValid.Description;
                            _context.Entry(model).State = EntityState.Added;
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (ex.InnerException != null)
                    {
                        return string.Format("Thêm mới đã xảy ra lỗi: {0}", ex.InnerException.Message);
                    }
                    else
                    {
                        return string.Format("Thêm mới đã xảy ra lỗi: {0}", ex.Message);
                    }
                }
            }
            #endregion Insert

            #region Update
            else
            {
                try
                {
                    SpecificationsProductModel model = _context.SpecificationsProductModel
                                                               .FirstOrDefault(p => p.SpecificationsProductId == specificationIsValid.SpecificationsProductId);
                    if (model != null)
                    {
                        model.ProductId = specificationIsValid.ProductId;
                        model.SpecificationsId = specificationIsValid.SpecificationsId;
                        model.Description = specificationIsValid.Description;

                        _context.Entry(model).State = EntityState.Modified;
                    }
                    else
                    {
                        return string.Format(LanguageResource.Validation_ImportExcelIdNotExist,
                                                LanguageResource.SpecificationsProductId, model.SpecificationsProductId,
                                                string.Format(LanguageResource.Export_ExcelHeader,
                                                LanguageResource.ExcelHeading_Specification));
                    }
                }
                catch (Exception ex)
                {
                    if (ex.InnerException != null)
                    {
                        return string.Format("Cập nhật đã xảy ra lỗi: {0}", ex.InnerException.Message);
                    }
                    else
                    {
                        return string.Format("Cập nhật đã xảy ra lỗi: {0}", ex.Message);
                    }
                }
            }
            #endregion Update
            _context.SaveChanges();
            return LanguageResource.ImportSuccess;
        }
        #endregion Specification

        #region Accessory
        List<AccessoryProductModel> accIsExistList = new List<AccessoryProductModel>();
        public string ExecuteImportExcelAccessory(AccessoryProductExcelViewModel accIsValid)
        {
            //Check:
            //1. If Id == "" then => Insert
            //2. Else then => Update
            #region Insert
            if (accIsValid.isNullValueId == true)
            {
                try
                {
                    //Check lỗi trùng
                    var accIsExist = _context.AccessoryProductModel
                                             .Where(p => p.ProductId == accIsValid.ProductId
                                                 && p.AccessoryId == accIsValid.AccessoryId)
                                             .FirstOrDefault();
                    var product = _context.ProductModel.Where(p => p.ProductId == accIsValid.ProductId)
                                                       .FirstOrDefault();
                    if (accIsExist == null)
                    {
                        if (product != null)
                        {
                            AccessoryProductModel model = new AccessoryProductModel();
                            model.AccessoryProductId = Guid.NewGuid();
                            model.ProductId = accIsValid.ProductId;
                            model.AccessoryId = accIsValid.AccessoryId;
                            model.Price = accIsValid.Price;
                            _context.Entry(model).State = EntityState.Added;
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (ex.InnerException != null)
                    {
                        return string.Format("Thêm mới đã xảy ra lỗi: {0}", ex.InnerException.Message);
                    }
                    else
                    {
                        return string.Format("Thêm mới đã xảy ra lỗi: {0}", ex.Message);
                    }
                }
            }
            #endregion Insert

            #region Update
            else
            {
                try
                {
                    AccessoryProductModel model = _context.AccessoryProductModel
                                                          .FirstOrDefault(p => p.AccessoryProductId == accIsValid.AccessoryProductId);
                    if (model != null)
                    {
                        model.ProductId = accIsValid.ProductId;
                        model.AccessoryId = accIsValid.AccessoryId;
                        model.Price = accIsValid.Price;

                        _context.Entry(model).State = EntityState.Modified;
                    }
                    else
                    {
                        return string.Format(LanguageResource.Validation_ImportExcelIdNotExist,
                                                LanguageResource.AccessoryProductId, model.AccessoryProductId,
                                                string.Format(LanguageResource.Export_ExcelHeader,
                                                LanguageResource.ExcelHeading_Accessory));
                    }
                }
                catch (Exception ex)
                {
                    if (ex.InnerException != null)
                    {
                        return string.Format("Cập nhật đã xảy ra lỗi: {0}", ex.InnerException.Message);
                    }
                    else
                    {
                        return string.Format("Cập nhật đã xảy ra lỗi: {0}", ex.Message);
                    }
                }
            }
            #endregion Update
            _context.SaveChanges();
            return LanguageResource.ImportSuccess;
        }
        #endregion Accessory

        #endregion Insert/Update data from excel file

        #region Check data type 

        #region Master
        public ProductExcelViewModel CheckTemplate(object[] row, int index)
        {
            ProductExcelViewModel productVM = new ProductExcelViewModel();
            var fieldName = "";
            try
            {
                for (int i = 0; i <= row.Length; i++)
                {
                    #region Convert data to import
                    switch (i)
                    {
                        //Index
                        case 0:
                            fieldName = LanguageResource.NumberIndex;
                            int rowIndex = int.Parse(row[i].ToString());
                            productVM.RowIndex = rowIndex;
                            break;
                        //ProductId
                        case 1:
                            fieldName = LanguageResource.ProductId;
                            string productId = row[i].ToString();
                            if (string.IsNullOrEmpty(productId))
                            {
                                productVM.isNullValueId = true;
                            }
                            else
                            {
                                productVM.ProductId = Guid.Parse(productId);
                                productVM.isNullValueId = false;
                            }
                            break;
                        //CompanyCode
                        case 2:
                            fieldName = LanguageResource.Company_CompanyCode;
                            string companyCode = row[i].ToString();
                            if (string.IsNullOrEmpty(companyCode))
                            {
                                productVM.Error = string.Format(LanguageResource.Validation_ImportRequired, string.Format(LanguageResource.Required, LanguageResource.Company_CompanyCode), productVM.RowIndex);
                            }
                            else
                            {
                                productVM.CompanyCode = companyCode;
                            }
                            break;
                        //ERPProductCode
                        case 3:
                            fieldName = LanguageResource.Product_ERPProductCode;
                            string erpProductCode = row[i].ToString();
                            if (string.IsNullOrEmpty(erpProductCode))
                            {
                                productVM.Error = string.Format(LanguageResource.Validation_ImportRequired, string.Format(LanguageResource.Required, LanguageResource.Product_ERPProductCode), productVM.RowIndex);
                            }
                            else
                            {
                                productVM.ERPProductCode = erpProductCode;
                            }
                            break;
                        //ProductCode
                        case 4:
                            fieldName = LanguageResource.Product_ProductCode;
                            string productCode = row[i].ToString();
                            if (string.IsNullOrEmpty(productCode))
                            {
                                productVM.Error = string.Format(LanguageResource.Validation_ImportRequired, string.Format(LanguageResource.Required, LanguageResource.Product_ProductCode), productVM.RowIndex);
                            }
                            else
                            {
                                productVM.ProductCode = productCode;
                            }
                            break;
                        //ProductGroups
                        case 5:
                            fieldName = LanguageResource.Product_ProductGroups;
                            string productGroups = row[i].ToString();
                            if (string.IsNullOrEmpty(productGroups))
                            {
                                productVM.Error = string.Format(LanguageResource.Validation_ImportRequired, string.Format(LanguageResource.Required, LanguageResource.Product_ProductGroups), productVM.RowIndex);
                            }
                            else
                            {
                                productVM.ProductGroups = productGroups;
                            }
                            break;
                        //ProductName
                        case 6:
                            fieldName = LanguageResource.Product_ProductName;
                            string productName = row[i].ToString();
                            if (string.IsNullOrEmpty(productName))
                            {
                                productVM.Error = string.Format(LanguageResource.Validation_ImportRequired, string.Format(LanguageResource.Required, LanguageResource.Product_ProductName), productVM.RowIndex);
                            }
                            else
                            {
                                productVM.ProductName = productName;
                            }
                            break;
                        //ParentCategoryName
                        case 7:
                            fieldName = LanguageResource.Sale_Brand;
                            string brandName = row[i].ToString();
                            if (string.IsNullOrEmpty(brandName))
                            {
                                productVM.Error = string.Format(LanguageResource.Validation_ImportRequired, string.Format(LanguageResource.Required, LanguageResource.Sale_Brand), productVM.RowIndex);
                            }
                            else
                            {
                                productVM.ParentCategoryName = row[i].ToString();
                            }
                            break;
                        //CategoryName
                        case 8:
                            fieldName = LanguageResource.Sale_Category;
                            productVM.CategoryName = row[i].ToString();
                            break;
                        //Price
                        case 9:
                            fieldName = LanguageResource.Price;
                            productVM.Price = GetTypeFunction<decimal?>(row[i].ToString(), i);
                            break;

                        //ProcessingValue
                        case 10:
                            fieldName = LanguageResource.ProcessingValue;
                            productVM.ProcessingValue = GetTypeFunction<decimal?>(row[i].ToString(), i);
                            break;

                        //SampleValue
                        case 11:
                            fieldName = LanguageResource.SampleValue;
                            productVM.SampleValue = GetTypeFunction<decimal?>(row[i].ToString(), i);
                            break;
                        //OrderIndex
                        case 12:
                            fieldName = LanguageResource.OrderIndex;
                            productVM.OrderIndex = GetTypeFunction<int>(row[i].ToString(), i);
                            break;
                        //Actived
                        case 13:
                            fieldName = LanguageResource.Actived;
                            productVM.Actived = GetTypeFunction<bool>(row[i].ToString(), i);
                            break;
                        //ParentCategoryId
                        case 14:
                            fieldName = LanguageResource.Sale_Brand;
                            productVM.ParentCategoryId = GetTypeFunction<Guid>(row[i].ToString(), i);
                            break;
                        //CategoryId
                        case 15:
                            fieldName = LanguageResource.Sale_Category;
                            productVM.CategoryId = GetTypeFunction<Guid>(row[i].ToString(), i);
                            break;
                        ////CylinderCapacity
                        //case 7:
                        //    fieldName = LanguageResource.CylinderCapacity;
                        //    productVM.CylinderCapacity = GetTypeFunction<decimal>(row[i].ToString(), i);

                        //    break;
                        ////ConfigurationName
                        //case 8:
                        //    fieldName = LanguageResource.Sale_Configuration;
                        //    string configName = row[i].ToString();
                        //    if (string.IsNullOrEmpty(configName))
                        //    {
                        //        productVM.Error = string.Format(LanguageResource.Validation_ImportRequired, string.Format(LanguageResource.Required, LanguageResource.Sale_Configuration), productVM.RowIndex);
                        //    }
                        //    else
                        //    {
                        //        productVM.ConfigurationName = row[i].ToString();
                        //    }
                        //    break;
                        ////ConfigurationId
                        //case 19:
                        //    fieldName = LanguageResource.Sale_Configuration;
                        //    productVM.ConfigurationId = GetTypeFunction<Guid>(row[i].ToString(), i);
                        //    break;
                        ////GuaranteePeriod
                        //case 9:
                        //    fieldName = LanguageResource.Product_GuaranteePeriod;
                        //    productVM.GuaranteePeriod = row[i].ToString();
                        //    break;
                        //ProductColorCode
                        //case 15:
                        //    fieldName = LanguageResource.ProductColorCode;
                        //    productVM.Color = row[i].ToString();
                        //    break;
                        //isHot
                        //case 8:
                        //    fieldName = LanguageResource.Product_isHot;
                        //    productVM.isHot = GetTypeFunction<bool>(row[i].ToString(), i);
                        //    break;
                        
                    }
                    #endregion Convert data to import
                }
            }
            catch (FormatException ex)
            {
                var Message = ex.Message;
                productVM.Error = string.Format(LanguageResource.Validation_ImportCastValid, fieldName, index);
            }
            catch (InvalidCastException ex)
            {
                var Message = ex.Message;
                productVM.Error = string.Format(LanguageResource.Validation_ImportCastValid, fieldName, index);
            }
            catch (Exception ex)
            {
                var Message = ex.Message;
                productVM.Error = string.Format(LanguageResource.Validate_ImportException, fieldName, index);
            }
            return productVM;
        }
        #endregion Master

        #region Detail
        public ColorStyleProductExcelViewModel CheckTemplateDetail(object[] row, int index)
        {
            ColorStyleProductExcelViewModel colorStyleProductVM = new ColorStyleProductExcelViewModel();
            var fieldName = "";
            try
            {
                for (int i = 0; i <= row.Length; i++)
                {
                    #region Convert data to import
                    switch (i)
                    {
                        //Index
                        case 0:
                            fieldName = LanguageResource.NumberIndex;
                            int rowIndex = int.Parse(row[i].ToString());
                            colorStyleProductVM.RowIndex = rowIndex;
                            break;
                        //ProductName
                        case 1:
                            fieldName = LanguageResource.Product_ProductName;
                            string productName = row[i].ToString();
                            if (string.IsNullOrEmpty(productName))
                            {
                                colorStyleProductVM.Error = string.Format(LanguageResource.Validation_ImportRequired, string.Format(LanguageResource.Required, LanguageResource.Product_ProductName), colorStyleProductVM.RowIndex);
                            }
                            else
                            {
                                colorStyleProductVM.ProductName = productName;
                            }
                            break;
                        //ProductId
                        case 4:
                            fieldName = LanguageResource.ProductId;
                            colorStyleProductVM.ProductId = GetTypeFunction<Guid>(row[i].ToString(), i);
                            break;
                        //ColorName
                        case 2:
                            fieldName = LanguageResource.Color_ColorName;
                            string colorName = row[i].ToString();
                            if (string.IsNullOrEmpty(colorName))
                            {
                                colorStyleProductVM.Error = string.Format(LanguageResource.Validation_ImportRequired, string.Format(LanguageResource.Required, LanguageResource.Color_ColorName), colorStyleProductVM.RowIndex);
                            }
                            else
                            {
                                colorStyleProductVM.ColorName = colorName;
                            }
                            break;
                        //ColorId
                        case 7:
                            fieldName = LanguageResource.ColorId;
                            colorStyleProductVM.ColorId = GetTypeFunction<Guid>(row[i].ToString(), i);
                            break;
                        //StyleName
                        case 3:
                            fieldName = LanguageResource.Style_StyleName;
                            string styleName = row[i].ToString();
                            colorStyleProductVM.StyleName = string.IsNullOrEmpty(styleName) ? "-- Tiêu chuẩn --" : styleName;
                            break;
                        //StyleId
                        case 10:
                            fieldName = LanguageResource.StyleId;
                            colorStyleProductVM.StyleId = GetTypeFunction<Guid>(row[i].ToString(), i);
                            break;
                    }
                    #endregion Convert data to import
                }
            }
            catch (FormatException ex)
            {
                var Message = ex.Message;
                colorStyleProductVM.Error = string.Format(LanguageResource.Validation_ImportCastValid, fieldName, index);
            }
            catch (InvalidCastException ex)
            {
                var Message = ex.Message;
                colorStyleProductVM.Error = string.Format(LanguageResource.Validation_ImportCastValid, fieldName, index);
            }
            catch (Exception ex)
            {
                var Message = ex.Message;
                colorStyleProductVM.Error = string.Format(LanguageResource.Validate_ImportException, fieldName, index);
            }
            return colorStyleProductVM;
        }
        #endregion Detail

        #region Specification
        public SpecificationProductExcelViewModel CheckTemplateSpecifications(object[] row, int index)
        {
            SpecificationProductExcelViewModel specificationVM = new SpecificationProductExcelViewModel();
            var fieldName = "";
            try
            {
                for (int i = 0; i <= row.Length; i++)
                {
                    #region Convert data to import
                    switch (i)
                    {
                        //Index
                        case 0:
                            fieldName = LanguageResource.NumberIndex;
                            int rowIndex = int.Parse(row[i].ToString());
                            specificationVM.RowIndex = rowIndex;
                            break;
                        //SpecificationsProductId
                        case 1:
                            fieldName = LanguageResource.SpecificationsProductId;
                            string SpecificationsProductId = row[i].ToString();
                            if (string.IsNullOrEmpty(SpecificationsProductId))
                            {
                                specificationVM.isNullValueId = true;
                            }
                            else
                            {
                                specificationVM.SpecificationsProductId = Guid.Parse(SpecificationsProductId);
                                specificationVM.isNullValueId = false;
                            }
                            break;
                        //ProductName
                        case 2:
                            fieldName = LanguageResource.Product_ProductName;
                            string productName = row[i].ToString();
                            if (string.IsNullOrEmpty(productName))
                            {
                                specificationVM.Error = string.Format(LanguageResource.Validation_ImportRequired, string.Format(LanguageResource.Required, LanguageResource.Product_ProductName), specificationVM.RowIndex);
                            }
                            else
                            {
                                specificationVM.ProductName = productName;
                            }
                            break;
                        //ProductId
                        case 5:
                            fieldName = LanguageResource.ProductId;
                            specificationVM.ProductId = GetTypeFunction<Guid>(row[i].ToString(), i);
                            break;
                        //SpecificationsName
                        case 3:
                            fieldName = LanguageResource.Specifications_SpecificationsName;
                            string SpecificationsName = row[i].ToString();
                            if (string.IsNullOrEmpty(SpecificationsName))
                            {
                                specificationVM.Error = string.Format(LanguageResource.Validation_ImportRequired, string.Format(LanguageResource.Required, LanguageResource.Specifications_SpecificationsName), specificationVM.RowIndex);
                            }
                            else
                            {
                                specificationVM.SpecificationsName = SpecificationsName;
                            }
                            break;
                        //SpecificationsId
                        case 8:
                            fieldName = LanguageResource.Specifications_SpecificationsName;
                            specificationVM.SpecificationsId = GetTypeFunction<Guid>(row[i].ToString(), i);
                            break;
                        //Description
                        case 4:
                            fieldName = LanguageResource.Description;
                            string Description = row[i].ToString();
                            if (string.IsNullOrEmpty(Description))
                            {
                                specificationVM.Error = string.Format(LanguageResource.Validation_ImportRequired, string.Format(LanguageResource.Required, LanguageResource.Description), specificationVM.RowIndex);
                            }
                            else
                            {
                                specificationVM.Description = Description;
                            }
                            break;
                    }
                    #endregion Convert data to import
                }
            }
            catch (FormatException ex)
            {
                var Message = ex.Message;
                specificationVM.Error = string.Format(LanguageResource.Validation_ImportCastValid, fieldName, index);
            }
            catch (InvalidCastException ex)
            {
                var Message = ex.Message;
                specificationVM.Error = string.Format(LanguageResource.Validation_ImportCastValid, fieldName, index);
            }
            catch (Exception ex)
            {
                var Message = ex.Message;
                specificationVM.Error = string.Format(LanguageResource.Validate_ImportException, fieldName, index);
            }
            return specificationVM;
        }
        #endregion Specification

        #region Accessory
        public AccessoryProductExcelViewModel CheckTemplateAccessory(object[] row, int index)
        {
            AccessoryProductExcelViewModel accessoryVM = new AccessoryProductExcelViewModel();
            var fieldName = "";
            try
            {
                for (int i = 0; i <= row.Length; i++)
                {
                    #region Convert data to import
                    switch (i)
                    {
                        //Index
                        case 0:
                            fieldName = LanguageResource.NumberIndex;
                            int rowIndex = int.Parse(row[i].ToString());
                            accessoryVM.RowIndex = rowIndex;
                            break;
                        //AccessoryProductId
                        case 1:
                            fieldName = LanguageResource.AccessoryProductId;
                            string AccessoryProductId = row[i].ToString();
                            if (string.IsNullOrEmpty(AccessoryProductId))
                            {
                                accessoryVM.isNullValueId = true;
                            }
                            else
                            {
                                accessoryVM.AccessoryProductId = Guid.Parse(AccessoryProductId);
                                accessoryVM.isNullValueId = false;
                            }
                            break;
                        //ProductName
                        case 2:
                            fieldName = LanguageResource.Product_ProductName;
                            string productName = row[i].ToString();
                            if (string.IsNullOrEmpty(productName))
                            {
                                accessoryVM.Error = string.Format(LanguageResource.Validation_ImportRequired, string.Format(LanguageResource.Required, LanguageResource.Product_ProductName), accessoryVM.RowIndex);
                            }
                            else
                            {
                                accessoryVM.ProductName = productName;
                            }
                            break;
                        //ProductId
                        case 5:
                            fieldName = LanguageResource.ProductId;
                            accessoryVM.ProductId = GetTypeFunction<Guid>(row[i].ToString(), i);
                            break;
                        //AccessoryName
                        case 3:
                            fieldName = LanguageResource.Accessory_AccessoryName;
                            string AccessoryName = row[i].ToString();
                            if (string.IsNullOrEmpty(AccessoryName))
                            {
                                accessoryVM.Error = string.Format(LanguageResource.Validation_ImportRequired, string.Format(LanguageResource.Required, LanguageResource.Accessory_AccessoryName), accessoryVM.RowIndex);
                            }
                            else
                            {
                                accessoryVM.AccessoryName = AccessoryName;
                            }
                            break;
                        //AccessoryId
                        case 8:
                            fieldName = LanguageResource.Accessory_AccessoryName;
                            accessoryVM.AccessoryId = GetTypeFunction<Guid>(row[i].ToString(), i);
                            break;
                        //Description
                        case 4:
                            fieldName = LanguageResource.Price;
                            string Price = row[i].ToString();
                            if (string.IsNullOrEmpty(Price))
                            {
                                accessoryVM.Error = string.Format(LanguageResource.Validation_ImportRequired, string.Format(LanguageResource.Required, LanguageResource.Description), accessoryVM.RowIndex);
                            }
                            else
                            {
                                accessoryVM.Price = GetTypeFunction<int>(row[i].ToString(), i);
                            }
                            break;
                    }
                    #endregion Convert data to import
                }
            }
            catch (FormatException ex)
            {
                var Message = ex.Message;
                accessoryVM.Error = string.Format(LanguageResource.Validation_ImportCastValid, fieldName, index);
            }
            catch (InvalidCastException ex)
            {
                var Message = ex.Message;
                accessoryVM.Error = string.Format(LanguageResource.Validation_ImportCastValid, fieldName, index);
            }
            catch (Exception ex)
            {
                var Message = ex.Message;
                accessoryVM.Error = string.Format(LanguageResource.Validate_ImportException, fieldName, index);
            }
            return accessoryVM;
        }
        #endregion Accessory

        #endregion Check data type

        #endregion Import from excel

        #region Modal search product component
        [HttpPost]
        public ActionResult _ProductSearchResult(DatatableViewModel model, ProductSearchViewModel productSearchModel)
        {
            return ExecuteSearch(() =>
            {
                int filteredResultsCount;
                int totalResultsCount;

                ProductRepository _productRepository = new ProductRepository(_context);
                var query = _productRepository.SearchQuery(productSearchModel);

                var res = CustomSearchRepository.CustomSearchFunc<ProductViewModel>(model, out filteredResultsCount, out totalResultsCount, query, "STT");
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
                    data = res
                });
            });
        }
        #endregion

        #region Sync Product
        public ActionResult SyncProduct(string CompanyCode, string ERPProductCode)
        {
            bool Success = true;
            string Message = string.Empty;
            try
            {
                var materialType = "Z" + ERPProductCode.Substring(0, 2);

                var result = _unitOfWork.ProductRepository.SyncMaterialByERPProductCode(CompanyCode, 1, materialType, ERPProductCode, out Message);

            }
            catch (Exception ex)
            {
                Message = ex.Message;
                if (ex.InnerException != null)
                {
                    if (ex.InnerException.InnerException != null)
                    {
                        Message = ex.InnerException.InnerException.Message;
                    }
                }
            }
            return Json(new { Success, Message }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SyncProductColor()
        {
            return ExecuteContainer(() =>
            {
                bool Success = true;
                string Message = string.Empty;
                var productColorList = new List<CatalogModel>();
                

                _unitOfWork.ProductRepository.SyncMaterialroductColor();

                //foreach (var item in dataACLibrary)
                //{
                //    var dataCRM = _context.CatalogModel.Where(x => x.CatalogCode.Trim() == item.CatalogCode.Trim() && x.CatalogTypeCode == item.CatalogTypeCode).FirstOrDefault();
                //    if (dataCRM == null)
                //    {

                //        var data = new CatalogModel()
                //        {
                //            CatalogId = Guid.NewGuid(),
                //            CatalogCode = item.CatalogCode.Trim(),
                //            CatalogTypeCode = item.CatalogTypeCode,
                //            Actived = true,
                //            OrderIndex = 10
                //        };
                //        productColorList.Add(data);
                //    }
                //}
                //if (productColorList != null && productColorList.Count() > 0)
                //{
                //    _context.CatalogModel.AddRange(productColorList);
                //}
                //_context.SaveChanges();
                return Json(new { Success, Message, Count = productColorList.Count() }, JsonRequestBehavior.AllowGet);
            });
        }
        #endregion


        public ActionResult GetProduct(string search)
        {
            var product = (from p in _context.ProductModel
                           join ca in _context.CategoryModel on p.ParentCategoryId equals ca.CategoryId
                           where p.Actived == true
                           && (p.ProductCode.Contains(search) || p.ProductName.Contains(search) || p.ERPProductCode.Contains(search))
                           && ca.IsTrackTrend == true
                           select new ISDSelectItem()
                           {
                               value = p.ProductId,
                               text = p.ProductName
                           }).Take(10000).ToList();
            return Json(product, JsonRequestBehavior.AllowGet);
        }

    }
}