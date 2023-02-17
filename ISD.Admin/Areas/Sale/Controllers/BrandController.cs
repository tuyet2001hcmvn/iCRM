using AutoMapper;
using AutoMapper.QueryableExtensions;
using ISD.Constant;
using ISD.EntityModels;
using ISD.Extensions;
using ISD.Resources;
using ISD.ViewModels;
using ISD.ViewModels.Sale;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using ISD.Core;
using ISD.Repositories.Excel;

namespace Sale.Controllers
{
    public class BrandSearchViewModel
    {
        public string CategoryName { get; set; }
        public bool? Actived { get; set; }
    }

    public class BrandController : BaseController
    {
        // GET: Brand
        #region Index
        [ISDAuthorizationAttribute]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult _Search(BrandSearchViewModel searchViewModel)
        {

            Session["frmSearchBrand"] = searchViewModel;

            return ExecuteSearch(() =>
            {
                var PhanLoaiVatTu = _context.CategoryModel.Where(p => p.CategoryCode == "PHANLOAIVATTU").FirstOrDefault();
                var category = (from p in _context.CategoryModel
                                orderby p.CategoryCode
                                where
                                //ParentCategory null
                                p.ParentCategoryId == PhanLoaiVatTu.CategoryId
                                //search by CategoryName
                                && (searchViewModel.CategoryName == null || p.CategoryName.Contains(searchViewModel.CategoryName))
                                //search by Actived
                                && (searchViewModel.Actived == null || p.Actived == searchViewModel.Actived)
                                select new BrandViewModel()
                                {
                                    CategoryId = p.CategoryId,
                                    CategoryCode = p.CategoryCode,
                                    CategoryName = p.CategoryName,
                                    ImageUrl = p.ImageUrl,
                                    OrderIndex = p.OrderIndex,
                                    Actived = p.Actived
                                })
                               .ToList();

                //Chỉ hiển thị nút Xóa nếu chưa có SP nào sử dụng hãng xe này
                var categoryProductList = _context.ProductModel.Where(p => p.Actived == true)
                                                               .Select(p => p.BrandId).Distinct().ToList();
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

        //GET: /Brand/Create
        #region Create
        [ISDAuthorizationAttribute]
        public ActionResult Create()
        {
            var model = new CategoryModel
            {
                IsTrackTrend = false
            };
            return View(model);
        }
        //POST: Create
        [HttpPost]
        [ValidateAjax]
        [ISDAuthorizationAttribute]
        public JsonResult Create(CategoryModel model, HttpPostedFileBase ImageUrl)
        {
            return ExecuteContainer(() =>
            {
                var PhanLoaiVatTu = _context.CategoryModel.Where(p => p.CategoryCode == "PHANLOAIVATTU").FirstOrDefault();

                model.CategoryId = Guid.NewGuid();
                model.ParentCategoryId = PhanLoaiVatTu.CategoryId;
                if (ImageUrl != null)
                {
                    model.ImageUrl = Upload(ImageUrl, "Brand");
                }
                _context.Entry(model).State = EntityState.Added;
                _context.SaveChanges();

                return Json(new
                {
                    Code = System.Net.HttpStatusCode.Created,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Create_Success, LanguageResource.Sale_Brand.ToLower())
                });
            });
        }
        #endregion

        //GET: /Brand/Edit
        #region Edit
        [ISDAuthorizationAttribute]
        public ActionResult Edit(Guid id)
        {
            var brand = _context.CategoryModel.FirstOrDefault(p => p.CategoryId == id);
            if (brand == null)
            {
                return RedirectToAction("ErrorText", "Error", new { area = "", statusCode = 404, exception = string.Format(LanguageResource.Error_NotExist, LanguageResource.Sale_Brand.ToLower()) });
            }
            if (brand.IsTrackTrend == null)
            {
                brand.IsTrackTrend = false;
            }
            return View(brand);
        }
        //POST: Edit
        [HttpPost]
        [ISDAuthorizationAttribute]
        public JsonResult Edit(CategoryViewModel viewModel, HttpPostedFileBase ImageUrl)
        {
            return ExecuteContainer(() =>
            {
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
                    modelUpdate.ImageUrl = Upload(ImageUrl, "Brand");
                }
                modelUpdate.CategoryName = viewModel.CategoryName;
                modelUpdate.OrderIndex = viewModel.OrderIndex;
                modelUpdate.IsTrackTrend = viewModel.IsTrackTrend;
                modelUpdate.Actived = viewModel.Actived.Value;
                _context.Entry(modelUpdate).State = EntityState.Modified;
                _context.SaveChanges();

                return Json(new
                {
                    Code = System.Net.HttpStatusCode.Created,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Edit_Success, LanguageResource.Sale_Brand.ToLower())
                });
            });
        }
        #endregion

        //Export
        #region Export to excel
        public ActionResult ExportCreate()
        {
            List<BrandExcelViewModel> brand = new List<BrandExcelViewModel>();
            return Export(brand, isEdit: false);
        }

        public ActionResult ExportEdit(BrandSearchViewModel searchViewModel)
        {

            searchViewModel = (BrandSearchViewModel)Session["frmSearchBrand"];


            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CategoryModel, BrandExcelViewModel>();
            });

            //Get data filter
            //searchViewModel = (ProductSearchViewModel)Session["frmSearch"];
            //Get data from server
            var brand = (from p in _context.CategoryModel
                         orderby p.OrderIndex.HasValue descending, p.OrderIndex
                         where
                         //ParentCategory null
                         p.ParentCategoryId == null
                         //search by CategoryName
                         && (searchViewModel.CategoryName == null || p.CategoryName.Contains(searchViewModel.CategoryName))
                         //search by Actived
                         && (searchViewModel.Actived == null || p.Actived == searchViewModel.Actived)
                         select p).ProjectTo<BrandExcelViewModel>(config)
                               .ToList();
            return Export(brand, isEdit: true);
        }

        const string controllerCode = ConstExcelController.Brand;
        const int startIndex = 8;
        [ISDAuthorizationAttribute]
        public FileContentResult Export(List<BrandExcelViewModel> viewModel, bool isEdit)
        {
            #region Master
            List<ExcelTemplate> columns = new List<ExcelTemplate>();

            columns.Add(new ExcelTemplate() { ColumnName = "CategoryId", isAllowedToEdit = false });
            columns.Add(new ExcelTemplate() { ColumnName = "CategoryCode", isAllowedToEdit = isEdit == true ? false : true });
            columns.Add(new ExcelTemplate() { ColumnName = "CategoryName", isAllowedToEdit = true });
            columns.Add(new ExcelTemplate() { ColumnName = "OrderIndex", isAllowedToEdit = true });
            columns.Add(new ExcelTemplate() { ColumnName = "Actived", isAllowedToEdit = true, isBoolean = true });
            // TODO: Upload hình ảnh
            //columns.Add(new ExcelTemplate() { ColumnName = "ImageUrl ", isAllowedToEdit = false });
            #endregion Master

            //Header
            string fileheader = string.Format(LanguageResource.Export_ExcelHeader, LanguageResource.Sale_Brand);
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
                Content = string.Format(LanguageResource.Export_ExcelWarningActived, LanguageResource.Sale_Brand),
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
                                        BrandExcelViewModel brandIsValid = CheckTemplate(dr.ItemArray, index);

                                        if (!string.IsNullOrEmpty(brandIsValid.Error))
                                        {
                                            string error = brandIsValid.Error;
                                            errorList.Add(error);
                                        }
                                        else
                                        {
                                            string result = ExecuteImportExcelBrand(brandIsValid);
                                            if (result != LanguageResource.ImportSuccess)
                                            {
                                                errorList.Add(result);
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                string error = string.Format(LanguageResource.Validation_ImportCheckController, LanguageResource.Sale_Brand);
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
        public string ExecuteImportExcelBrand(BrandExcelViewModel brandIsValid)
        {
            //Check:
            //1. If Id == "" then => Insert
            //2. Else then => Update
            #region Insert
            if (brandIsValid.isNullValueId == true)
            {
                try
                {
                    var brandCodeIsExist = _context.CategoryModel
                                                   .FirstOrDefault(p => p.CategoryCode == brandIsValid.CategoryCode);
                    if (brandCodeIsExist != null)
                    {
                        return string.Format(LanguageResource.Validation_Already_Exists, brandIsValid.CategoryCode);
                    }
                    else
                    {
                        CategoryModel brand = new CategoryModel();
                        brand.CategoryId = Guid.NewGuid();
                        brand.CategoryCode = brandIsValid.CategoryCode;
                        brand.CategoryName = brandIsValid.CategoryName;
                        brand.OrderIndex = brandIsValid.OrderIndex;
                        brand.Actived = brandIsValid.Actived == true ? true : false;
                        _context.Entry(brand).State = EntityState.Added;
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
                    CategoryModel brand = _context.CategoryModel.FirstOrDefault(p => p.CategoryId == brandIsValid.CategoryId);
                    if (brand != null)
                    {
                        brand.CategoryName = brandIsValid.CategoryName;
                        brand.OrderIndex = brandIsValid.OrderIndex;
                        brand.Actived = brandIsValid.Actived == true ? true : false;
                        _context.Entry(brand).State = EntityState.Modified;
                    }
                    else
                    {
                        return string.Format(LanguageResource.Validation_ImportExcelIdNotExist,
                                                LanguageResource.BrandId, brandIsValid.CategoryId,
                                                string.Format(LanguageResource.Export_ExcelHeader,
                                                LanguageResource.Sale_Brand));
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
        public BrandExcelViewModel CheckTemplate(object[] row, int index)
        {
            BrandExcelViewModel brandVM = new BrandExcelViewModel();
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
                            brandVM.RowIndex = rowIndex;
                            break;
                        //CategoryId
                        case 1:
                            fieldName = LanguageResource.BrandId;
                            string categoryId = row[i].ToString();
                            if (string.IsNullOrEmpty(categoryId))
                            {
                                brandVM.isNullValueId = true;
                            }
                            else
                            {
                                brandVM.CategoryId = Guid.Parse(categoryId);
                                brandVM.isNullValueId = false;
                            }
                            break;
                        //CategoryCode
                        case 2:
                            fieldName = LanguageResource.Brand_BrandCode;
                            string brandCode = row[i].ToString();
                            if (string.IsNullOrEmpty(brandCode))
                            {
                                brandVM.Error = string.Format(LanguageResource.Validation_ImportRequired, string.Format(LanguageResource.Required, LanguageResource.Brand_BrandCode), brandVM.RowIndex);
                            }
                            else
                            {
                                brandVM.CategoryCode = brandCode;
                            }
                            break;
                        //CategoryName
                        case 3:
                            fieldName = LanguageResource.Brand_BrandName;
                            string brandName = row[i].ToString();
                            if (string.IsNullOrEmpty(brandName))
                            {
                                brandVM.Error = string.Format(LanguageResource.Validation_ImportRequired, string.Format(LanguageResource.Required, LanguageResource.Brand_BrandName), brandVM.RowIndex);
                            }
                            else
                            {
                                brandVM.CategoryName = brandName;
                            }
                            break;
                        //OrderIndex
                        case 4:
                            fieldName = LanguageResource.OrderIndex;
                            brandVM.OrderIndex = GetTypeFunction<int>(row[i].ToString(), i);
                            break;
                        //Actived
                        case 5:
                            fieldName = LanguageResource.Actived;
                            brandVM.Actived = GetTypeFunction<bool>(row[i].ToString(), i);
                            break;
                    }
                    #endregion Convert data to import
                }
            }
            catch (FormatException ex)
            {
                var Message = ex.Message;
                brandVM.Error = string.Format(LanguageResource.Validation_ImportCastValid, fieldName, index);
            }
            catch (InvalidCastException ex)
            {
                var Message = ex.Message;
                brandVM.Error = string.Format(LanguageResource.Validation_ImportCastValid, fieldName, index);
            }
            catch (Exception ex)
            {
                var Message = ex.Message;
                brandVM.Error = string.Format(LanguageResource.Validate_ImportException, fieldName, index);
            }
            return brandVM;
        }
        #endregion Check data type


        #endregion Import from excel

        //GET: /Brand/Delete
        #region Delete
        [HttpPost]
        [ISDAuthorizationAttribute]
        public ActionResult Delete(Guid id)
        {
            return ExecuteDelete(() =>
            {
                var brand = _context.CategoryModel.FirstOrDefault(p => p.CategoryId == id);
                if (brand != null)
                {
                    _context.Entry(brand).State = EntityState.Deleted;
                    _context.SaveChanges();

                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.Created,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Delete_Success, LanguageResource.Sale_Brand.ToLower())
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