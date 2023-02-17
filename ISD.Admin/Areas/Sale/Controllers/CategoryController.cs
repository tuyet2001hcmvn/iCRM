using ISD.Constant;
using ISD.EntityModels;
using ISD.Extensions;
using ISD.Repositories.Excel;
using ISD.Resources;
using ISD.ViewModels;
using ISD.ViewModels.Sale;
using ISD.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;

namespace Sale.Controllers
{
    public class CategorySearchViewModel
    {
        public Guid? ParentCategoryId { get; set; }
        public string CategoryName { get; set; }
        public bool? Actived { get; set; }
    }

    public class CategoryController : BaseController
    {
        // GET: Category
        #region Index
        [ISDAuthorizationAttribute]
        public ActionResult Index()
        {
            //Get list Brand
            //var brandList = _context.CategoryModel.Where(p => p.ParentCategoryId == null && p.Actived == true)
            //                                      .OrderBy(p => p.OrderIndex).ToList();
            //ViewBag.ParentCategoryId = new SelectList(brandList, "CategoryId", "CategoryName");

            return View();
        }

        public ActionResult _Search(CategorySearchViewModel searchViewModel)
        {
            Session["frmSearchCategory"] = searchViewModel;


            return ExecuteSearch(() =>
            {
                var NhomVatTu = _context.CategoryModel.Where(p => p.CategoryCode == ConstCategoryCode.NHOMVATTU).FirstOrDefault();

                var category = (from p in _context.CategoryModel
                                    //join to get BrandName
                                //join br in _context.CategoryModel on p.ParentCategoryId equals br.CategoryId
                                orderby p.CategoryCode
                                where
                                //(p.ParentCategoryId != null)
                                p.ParentCategoryId == NhomVatTu.CategoryId
                                //search by ParentCategoryId
                                && (searchViewModel.ParentCategoryId == null || p.ParentCategoryId == searchViewModel.ParentCategoryId)
                                //search by CategoryName
                                && (searchViewModel.CategoryName == null || p.CategoryName.Contains(searchViewModel.CategoryName))
                                //search by Actived
                                && (searchViewModel.Actived == null || p.Actived == searchViewModel.Actived)
                                select new CategoryViewModel()
                                {
                                    CategoryId = p.CategoryId,
                                    CategoryCode = p.CategoryCode,
                                    CategoryName = p.CategoryName,
                                    ParentCategoryId = p.ParentCategoryId,
                                    //ParentCategoryName = br.CategoryName,
                                    OrderIndex = p.OrderIndex,
                                    Actived = p.Actived,
                                    ImageUrl = p.ImageUrl
                                })
                               .ToList();

                //Chỉ hiển thị nút Xóa nếu chưa có SP nào sử dụng loại xe này
                var categoryProductList = _context.ProductModel.Where(p => p.Actived == true)
                                                               .Select(p => p.CategoryId).Distinct().ToList();
                foreach (var item in category)
                {
                    if (!categoryProductList.Contains(item.CategoryId))
                    {
                        item.isDelete = true;
                    }
                }

                return PartialView(category);
            });
        }
        #endregion

        //GET: /Category/Create 
        #region Create
        [ISDAuthorizationAttribute]
        public ActionResult Create()
        {
            CategoryViewModel viewModel = new CategoryViewModel();

            //Get list Brand
            //var brandList = _context.CategoryModel.Where(p => p.ParentCategoryId == null && p.Actived == true)
            //                                      .OrderBy(p => p.OrderIndex).ToList();
            //ViewBag.ParentCategoryId = new SelectList(brandList, "CategoryId", "CategoryName");
            //ProductType
            var producttypelist = _context.ProductTypeModel.ToList();
            ViewBag.ProductTypeId = new SelectList(producttypelist, "ProductTypeId", "ProductTypeName");
            //viewModel.IsTrackTrend = false;
            return View(viewModel);
        }
        //POST: Create
        [HttpPost]
        [ValidateAjax]
        [ISDAuthorizationAttribute]
        public JsonResult Create(CategoryModel model, HttpPostedFileBase ImageUrl)
        {
            return ExecuteContainer(() =>
            {
                var NhomVatTu = _context.CategoryModel.Where(p => p.CategoryCode == ConstCategoryCode.NHOMVATTU).FirstOrDefault();

                model.CategoryId = Guid.NewGuid();
                model.ParentCategoryId = NhomVatTu.CategoryId;
                if (ImageUrl != null)
                {
                    model.ImageUrl = Upload(ImageUrl, "Category");
                }
                _context.Entry(model).State = EntityState.Added;
                _context.SaveChanges();

                return Json(new
                {
                    Code = System.Net.HttpStatusCode.Created,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Create_Success, LanguageResource.Sale_Category.ToLower())
                });
            });
        }
        #endregion

        //GET: /Category/Edit
        #region Edit
        [ISDAuthorizationAttribute]
        public ActionResult Edit(Guid id)
        {
            var category = (from p in _context.CategoryModel
                                //join to get BrandName
                            //join br in _context.CategoryModel on p.ParentCategoryId equals br.CategoryId
                            where p.CategoryId == id
                            select new CategoryViewModel()
                            {
                                CategoryId = p.CategoryId,
                                CategoryCode = p.CategoryCode,
                                CategoryName = p.CategoryName,
                                //ParentCategoryId = p.ParentCategoryId,
                                //ParentCategoryName = br.CategoryName,
                                OrderIndex = p.OrderIndex,
                                Actived = p.Actived,
                                ImageUrl = p.ImageUrl,
                                ProductTypeId = p.ProductTypeId,
                                IsTrackTrend = p.IsTrackTrend ?? false
                            })
                           .FirstOrDefault();
            if (category == null)
            {
                return RedirectToAction("ErrorText", "Error", new { area = "", statusCode = 404, exception = string.Format(LanguageResource.Error_NotExist, LanguageResource.Sale_Category.ToLower()) });
            }

            //var producttypelist = _context.ProductTypeModel.ToList();
            //ViewBag.ProductTypeId = new SelectList(producttypelist, "ProductTypeId", "ProductTypeName", category.ProductTypeId);

            return View(category);
        }
        //POST: Edit
        [HttpPost]
        [ISDAuthorizationAttribute]
        public JsonResult Edit(CategoryViewModel viewModel, HttpPostedFileBase ImageUrl)
        {
            return ExecuteContainer(() =>
            {
                var NhomVatTu = _context.CategoryModel.Where(p => p.CategoryCode == ConstCategoryCode.NHOMVATTU).FirstOrDefault();

                var modelUpdate = _context.CategoryModel.Find(viewModel.CategoryId);
                if (modelUpdate == null)
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotFound,
                        Success = false,
                        Data = LanguageResource.Mobile_NotFound
                    });
                }

                if (ImageUrl != null)
                {
                    modelUpdate.ImageUrl = Upload(ImageUrl, "Category");
                }
                modelUpdate.CategoryName = viewModel.CategoryName;
                modelUpdate.ProductTypeId = viewModel.ProductTypeId;
                modelUpdate.OrderIndex = viewModel.OrderIndex;
                modelUpdate.Actived = viewModel.Actived.Value;
                modelUpdate.IsTrackTrend = viewModel.IsTrackTrend;
                modelUpdate.ParentCategoryId = NhomVatTu.CategoryId;
                _context.Entry(modelUpdate).State = EntityState.Modified;
                _context.SaveChanges();

                return Json(new
                {
                    Code = System.Net.HttpStatusCode.Created,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Edit_Success, LanguageResource.Sale_Category.ToLower())
                });
            });
        }
        #endregion

        //Export
        #region Export to excel
        public ActionResult ExportCreate()
        {
            List<CategoryExcelViewModel> category = new List<CategoryExcelViewModel>();
            return Export(category, isEdit: false);
        }

        public ActionResult ExportEdit(CategorySearchViewModel searchViewModel)
        {

            searchViewModel = (CategorySearchViewModel)Session["frmSearchCategory"];

            var NhomVatTu = _context.CategoryModel.Where(p => p.CategoryCode == ConstCategoryCode.NHOMVATTU).FirstOrDefault();
            //var config = new MapperConfiguration(cfg =>
            //{
            //    cfg.CreateMap<CategoryViewModel, CategoryExcelViewModel>();
            //});

            //Get data filter
            //searchViewModel = (ProductSearchViewModel)Session["frmSearch"];
            //Get data from server
            var viewModelList = (from p in _context.CategoryModel
                                     //join to get BrandName
                                 //join br in _context.CategoryModel on p.ParentCategoryId equals br.CategoryId
                                 join t in _context.ProductTypeModel on p.ProductTypeId equals t.ProductTypeId into tg
                                 from type in tg.DefaultIfEmpty()
                                 orderby p.ParentCategoryId, p.OrderIndex.HasValue descending, p.OrderIndex
                                 where
                                 //(p.ParentCategoryId != null)
                                 p.ParentCategoryId == NhomVatTu.CategoryId
                                 //search by ParentCategoryId
                                 && (searchViewModel.ParentCategoryId == null || p.ParentCategoryId == searchViewModel.ParentCategoryId)
                                 //search by CategoryName
                                 && (searchViewModel.CategoryName == null || p.CategoryName.Contains(searchViewModel.CategoryName))
                                 //search by Actived
                                 && (searchViewModel.Actived == null || p.Actived == searchViewModel.Actived)
                                 select new CategoryExcelViewModel()
                                 {
                                     CategoryId = p.CategoryId,
                                     CategoryCode = p.CategoryCode,
                                     CategoryName = p.CategoryName,
                                     //BrandName = br.CategoryName,
                                     ParentCategoryId = p.ParentCategoryId,
                                     ProductTypeId = p.ProductTypeId,
                                     ProductTypeName = (type != null) ? type.ProductTypeName : null,
                                     OrderIndex = p.OrderIndex,
                                     Actived = p.Actived,
                                    // IsTrackTrend = p.IsTrackTrend
                                 })
                               .ToList();
            return Export(viewModelList, isEdit: true);
        }

        const string controllerCode = ConstExcelController.Category;
        const int startIndex = 8;
        [ISDAuthorizationAttribute]
        public FileContentResult Export(List<CategoryExcelViewModel> viewModel, bool isEdit)
        {

            //Hãng xe
            //List<DropdownModel> BrandId = (from c in _context.CategoryModel
            //                               where c.ParentCategoryId == null && c.Actived == true
            //                               orderby c.OrderIndex.HasValue descending, c.OrderIndex
            //                               select new DropdownModel()
            //                               {
            //                                   Id = c.CategoryId,
            //                                   Name = c.CategoryName,
            //                               }).ToList();
            //Loại xe
            List<DropdownIdTypeIntModel> ProductTypeId = (from p in _context.ProductTypeModel
                                                          orderby p.ProductTypeName
                                                          select new DropdownIdTypeIntModel()
                                                          {
                                                              Id = p.ProductTypeId,
                                                              Name = p.ProductTypeName,
                                                          }).ToList();

            #region Master
            List<ExcelTemplate> columns = new List<ExcelTemplate>();

            columns.Add(new ExcelTemplate() { ColumnName = "CategoryId", isAllowedToEdit = false });
            columns.Add(new ExcelTemplate() { ColumnName = "CategoryCode", isAllowedToEdit = isEdit == true ? false : true });
            columns.Add(new ExcelTemplate() { ColumnName = "CategoryName", isAllowedToEdit = true });
            //columns.Add(new ExcelTemplate()
            //{
            //    ColumnName = "BrandName",
            //    isAllowedToEdit = true,
            //    isDropdownlist = true,
            //    TypeId = ConstExcelController.GuidId,
            //    DropdownData = BrandId
            //});
            columns.Add(new ExcelTemplate() { ColumnName = "OrderIndex", isAllowedToEdit = true });
            //columns.Add(new ExcelTemplate() { ColumnName = "IsTrackTrend", isAllowedToEdit = true, isBoolean = true });
            columns.Add(new ExcelTemplate() { ColumnName = "Actived", isAllowedToEdit = true, isBoolean = true });
            // TODO: Upload hình ảnh
            //columns.Add(new ExcelTemplate() { ColumnName = "ImageUrl ", isAllowedToEdit = false });
            #endregion Master

            //Header
            string fileheader = string.Format(LanguageResource.Export_ExcelHeader, LanguageResource.Sale_Category);
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

            //Trạng thái
            heading.Add(new ExcelHeadingTemplate()
            {
                Content = string.Format(LanguageResource.Export_ExcelWarningActived, LanguageResource.Sale_Category),
                RowsToIgnore = 1,
                isWarning = true,
                isCode = false
            });

            //Body
            byte[] filecontent = ClassExportExcel.ExportExcel(viewModel, columns, heading, true);
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
        [ISDAuthorizationAttribute]
        public ActionResult Import()
        {
            return ExcuteImportExcel(() =>
            {
                DataSet ds = GetDataSetFromExcel();
                List<string> errorList = new List<string>();
                if (ds.Tables != null && ds.Tables.Count > 0)
                {
                    using (TransactionScope ts = new TransactionScope())
                    {
                        foreach (DataTable dt in ds.Tables)
                        {
                            //Get controller code from Excel file
                            string contCode = dt.Columns[0].ColumnName.ToString();
                            //Import data with accordant controller and action
                            if (contCode == controllerCode)
                            {
                                var index = 0;
                                foreach (DataRow dr in dt.Rows)
                                {
                                    if (dt.Rows.IndexOf(dr) >= startIndex && !string.IsNullOrEmpty(dr.ItemArray[0].ToString()))
                                    {
                                        index++;
                                        //Check correct template
                                        CategoryExcelViewModel categoryIsValid = CheckTemplate(dr.ItemArray, index);

                                        if (!string.IsNullOrEmpty(categoryIsValid.Error))
                                        {
                                            string error = categoryIsValid.Error;
                                            errorList.Add(error);
                                        }
                                        else
                                        {
                                            string result = ExecuteImportExcelBrand(categoryIsValid);
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
                            else
                            {
                                string error = string.Format(LanguageResource.Validation_ImportCheckController, LanguageResource.Sale_Product);
                                errorList.Add(error);
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

        #region Insert/Update data from excel file
        public string ExecuteImportExcelBrand(CategoryExcelViewModel categoryIsValid)
        {
            var NhomVatTu = _context.CategoryModel.Where(p => p.CategoryCode == ConstCategoryCode.NHOMVATTU).FirstOrDefault();
            //Check:
            //1. If Id == "" then => Insert
            //2. Else then => Update
            #region Insert
            if (categoryIsValid.isNullValueId == true)
            {
                try
                {
                    var categoryCodeIsExist = _context.CategoryModel
                                                      .FirstOrDefault(p => p.CategoryCode == categoryIsValid.CategoryCode);
                    if (categoryCodeIsExist != null)
                    {
                        return string.Format(LanguageResource.Validation_Already_Exists, categoryIsValid.CategoryCode);
                    }
                    else
                    {
                        CategoryModel category = new CategoryModel();
                        category.CategoryId = Guid.NewGuid();
                        category.CategoryCode = categoryIsValid.CategoryCode;
                        category.CategoryName = categoryIsValid.CategoryName;
                        //category.ParentCategoryId = categoryIsValid.ParentCategoryId;
                        category.ParentCategoryId = NhomVatTu.CategoryId;
                        category.ProductTypeId = categoryIsValid.ProductTypeId;
                        category.OrderIndex = categoryIsValid.OrderIndex;
                       // category.IsTrackTrend = categoryIsValid.IsTrackTrend;
                        category.Actived = categoryIsValid.Actived == true ? true : false;
                        _context.Entry(category).State = EntityState.Added;
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
                    if (categoryIsValid.ParentCategoryId == null)
                    {
                        return LanguageResource.Validation_ImportExcelUnchangeFile;
                    }
                    CategoryModel category = _context.CategoryModel
                                                     .FirstOrDefault(p => p.CategoryId == categoryIsValid.CategoryId);
                    if (category != null)
                    {
                        category.CategoryName = categoryIsValid.CategoryName;
                        //category.ParentCategoryId = categoryIsValid.ParentCategoryId;
                        category.ParentCategoryId = NhomVatTu.CategoryId;
                        category.ProductTypeId = categoryIsValid.ProductTypeId;
                        category.OrderIndex = categoryIsValid.OrderIndex;
                        //category.IsTrackTrend = categoryIsValid.IsTrackTrend;
                        category.Actived = categoryIsValid.Actived == true ? true : false;
                        _context.Entry(category).State = EntityState.Modified;
                    }
                    else
                    {
                        return string.Format(LanguageResource.Validation_ImportExcelIdNotExist,
                                                LanguageResource.CategoryId, categoryIsValid.CategoryId,
                                                string.Format(LanguageResource.Export_ExcelHeader,
                                                LanguageResource.Sale_Category));
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
        #endregion Insert/Update data from excel file

        #region Check data type 
        public CategoryExcelViewModel CheckTemplate(object[] row, int index)
        {
            CategoryExcelViewModel categoryVM = new CategoryExcelViewModel();
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
                            categoryVM.RowIndex = rowIndex;
                            break;
                        //CategoryId
                        case 1:
                            fieldName = LanguageResource.CategoryId;
                            string categoryId = row[i].ToString();
                            if (string.IsNullOrEmpty(categoryId))
                            {
                                categoryVM.isNullValueId = true;
                            }
                            else
                            {
                                categoryVM.CategoryId = Guid.Parse(categoryId);
                                categoryVM.isNullValueId = false;
                            }
                            break;
                        //CategoryCode
                        case 2:
                            fieldName = LanguageResource.Category_CategoryCode;
                            string categoryCode = row[i].ToString();
                            if (string.IsNullOrEmpty(categoryCode))
                            {
                                categoryVM.Error = string.Format(LanguageResource.Validation_ImportRequired, string.Format(LanguageResource.Required, LanguageResource.Category_CategoryCode), categoryVM.RowIndex);
                            }
                            else
                            {
                                categoryVM.CategoryCode = categoryCode;
                            }
                            break;
                        //CategoryName
                        case 3:
                            fieldName = LanguageResource.Category_CategoryName;
                            string categoryName = row[i].ToString();
                            if (string.IsNullOrEmpty(categoryName))
                            {
                                categoryVM.Error = string.Format(LanguageResource.Validation_ImportRequired, string.Format(LanguageResource.Required, LanguageResource.Category_CategoryName), categoryVM.RowIndex);
                            }
                            else
                            {
                                categoryVM.CategoryName = categoryName;
                            }
                            break;
                        //BrandName
                        //case 4:
                        //    fieldName = LanguageResource.Sale_Brand;
                        //    string brandName = row[i].ToString();
                        //    if (string.IsNullOrEmpty(brandName))
                        //    {
                        //        categoryVM.Error = string.Format(LanguageResource.Validation_ImportRequired, string.Format(LanguageResource.Required, LanguageResource.Sale_Brand), categoryVM.RowIndex);
                        //    }
                        //    else
                        //    {
                        //        categoryVM.BrandName = brandName;
                        //    }
                        //    break;
                        //ParentCategoryId: 4
                        //case 8:
                        //    fieldName = LanguageResource.Sale_Brand;
                        //    categoryVM.ParentCategoryId = GetTypeFunction<Guid>(row[i].ToString(), i);
                        //    break;

                        
                        //OrderIndex
                        case 4:
                            fieldName = LanguageResource.OrderIndex;
                            categoryVM.OrderIndex = GetTypeFunction<int>(row[i].ToString(), i);
                            break;
                        //ProductTypeId: 4
                        //case 5:
                        //    fieldName = LanguageResource.Category_TrackTrend;
                        //    categoryVM.IsTrackTrend = GetTypeFunction<bool>(row[i].ToString(), i);
                        //    break;
                        //Actived
                        case 5:
                            fieldName = LanguageResource.Actived;
                            categoryVM.Actived = GetTypeFunction<bool>(row[i].ToString(), i);
                            break;
                    }
                    #endregion Convert data to import
                }
            }
            catch (FormatException ex)
            {
                var Message = ex.Message;
                categoryVM.Error = string.Format(LanguageResource.Validation_ImportCastValid, fieldName, index);
            }
            catch (InvalidCastException ex)
            {
                var Message = ex.Message;
                categoryVM.Error = string.Format(LanguageResource.Validation_ImportCastValid, fieldName, index);
            }
            catch (Exception ex)
            {
                var Message = ex.Message;
                categoryVM.Error = string.Format(LanguageResource.Validate_ImportException, fieldName, index);
            }
            return categoryVM;
        }
        #endregion Check data type


        #endregion Import from excel

        //GET: /Category/Delete
        #region Delete
        [HttpPost]
        [ISDAuthorizationAttribute]
        public ActionResult Delete(Guid id)
        {
            return ExecuteDelete(() =>
            {
                var category = _context.CategoryModel.FirstOrDefault(p => p.CategoryId == id);
                if (category != null)
                {
                    _context.Entry(category).State = EntityState.Deleted;
                    _context.SaveChanges();

                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.Created,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Delete_Success, LanguageResource.Sale_Category.ToLower())
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
        private bool IsExists(string CategoryCode)
        {
            return (_context.CategoryModel.FirstOrDefault(p => p.CategoryCode == CategoryCode) != null);
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult CheckExistingCategoryCode(string CategoryCode, string CategoryCodeValid)
        {
            try
            {
                if (CategoryCodeValid != CategoryCode)
                {
                    return Json(!IsExists(CategoryCode));
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
    }
}