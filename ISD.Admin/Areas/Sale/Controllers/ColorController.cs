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

namespace Sale.Controllers
{
    public class ColorController : BaseController
    {
        // GET: Color
        #region Index
        [ISDAuthorizationAttribute]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult _Search(ColorSearchViewModel searchViewModel)
        {
            Session["frmSearchColor"] = searchViewModel;
            return ExecuteSearch(() =>
            {
                var color = (from p in _context.ColorModel
                             orderby p.OrderIndex.HasValue descending, p.OrderIndex
                             where
                             //search by ColorName
                             (searchViewModel.ColorName == null || p.ColorName.Contains(searchViewModel.ColorName))
                             //search by Actived
                             && (searchViewModel.Actived == null || p.Actived == searchViewModel.Actived)
                             select p)
                             .ToList();

                return PartialView(color);
            });
        }
        #endregion

        //GET: /Color/Create
        #region Create
        [ISDAuthorizationAttribute]
        public ActionResult Create()
        {
            return View();
        }
        //POST: Create
        [HttpPost]
        [ValidateAjax]
        [ISDAuthorizationAttribute]
        public JsonResult Create(ColorModel model)
        {
            return ExecuteContainer(() =>
            {
                model.ColorId = Guid.NewGuid();
                _context.Entry(model).State = EntityState.Added;
                _context.SaveChanges();

                return Json(new
                {
                    Code = System.Net.HttpStatusCode.Created,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Create_Success, LanguageResource.Sale_Color.ToLower())
                });
            });
        }
        #endregion

        //GET: /Color/Edit
        #region Edit
        [ISDAuthorizationAttribute]
        public ActionResult Edit(Guid id)
        {
            var color = _context.ColorModel.FirstOrDefault(p => p.ColorId == id);
            if (color == null)
            {
                return RedirectToAction("ErrorText", "Error", new { area = "", statusCode = 404, exception = string.Format(LanguageResource.Error_NotExist, LanguageResource.Sale_Color.ToLower()) });
            }
            return View(color);
        }
        //POST: Edit
        [HttpPost]
        [ISDAuthorizationAttribute]
        public JsonResult Edit(ColorModel model)
        {
            return ExecuteContainer(() =>
            {
                _context.Entry(model).State = EntityState.Modified;
                _context.SaveChanges();

                return Json(new
                {
                    Code = System.Net.HttpStatusCode.Created,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Edit_Success, LanguageResource.Sale_Color.ToLower())
                });
            });
        }
        #endregion

        //Export
        #region Export to excel
        public ActionResult ExportCreate()
        {
            List<ColorExcelViewModel> viewModel = new List<ColorExcelViewModel>();
            return Export(viewModel, isEdit: false);
        }

        public ActionResult ExportEdit(ColorSearchViewModel searchViewModel)
        {

            searchViewModel = (ColorSearchViewModel)Session["frmSearchColor"];


            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ColorModel, ColorExcelViewModel>();
            });

            //Get data filter
            //Get data from server
            var color = (from p in _context.ColorModel
                         orderby p.OrderIndex.HasValue descending, p.OrderIndex
                         where
                         //search by ColorName
                         (searchViewModel.ColorName == null || p.ColorName.Contains(searchViewModel.ColorName))
                         //search by Actived
                         && (searchViewModel.Actived == null || p.Actived == searchViewModel.Actived)
                         select p).ProjectTo<ColorExcelViewModel>(config)
                               .ToList();
            return Export(color, isEdit: true);
        }

        const string controllerCode = ConstExcelController.Color;
        const int startIndex = 8;
        [ISDAuthorizationAttribute]
        public FileContentResult Export(List<ColorExcelViewModel> viewModel, bool isEdit)
        {
            #region Master
            List<ExcelTemplate> columns = new List<ExcelTemplate>();

            columns.Add(new ExcelTemplate() { ColumnName = "ColorId", isAllowedToEdit = false });
            columns.Add(new ExcelTemplate() { ColumnName = "ColorShortName", isAllowedToEdit = isEdit == true ? false : true });
            columns.Add(new ExcelTemplate() { ColumnName = "ColorCode", isAllowedToEdit = true });
            columns.Add(new ExcelTemplate() { ColumnName = "ColorName", isAllowedToEdit = true });
            columns.Add(new ExcelTemplate() { ColumnName = "OrderIndex", isAllowedToEdit = true });
            columns.Add(new ExcelTemplate() { ColumnName = "Actived", isAllowedToEdit = true, isBoolean = true });
            // TODO: Upload hình ảnh
            //columns.Add(new ExcelTemplate() { ColumnName = "ImageUrl ", isAllowedToEdit = false });
            #endregion Master

            //Header
            string fileheader = string.Format(LanguageResource.Export_ExcelHeader, LanguageResource.Sale_Color);
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
                Content = string.Format(LanguageResource.Export_ExcelWarningActived, LanguageResource.Sale_Color),
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
            string fileNameWithFormat = string.Format("{0}_{1}.xlsx", exportType, RemoveSign4VietnameseString(fileheader.ToUpper()).Replace(" ", "_"));

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
                                        ColorExcelViewModel colorIsValid = CheckTemplate(dr.ItemArray, index);

                                        if (!string.IsNullOrEmpty(colorIsValid.Error))
                                        {
                                            string error = colorIsValid.Error;
                                            errorList.Add(error);
                                        }
                                        else
                                        {
                                            string result = ExecuteImportExcelColor(colorIsValid);
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
                                string error = string.Format(LanguageResource.Validation_ImportCheckController, LanguageResource.Sale_Color);
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
        public string ExecuteImportExcelColor(ColorExcelViewModel colorIsValid)
        {
            //Check:
            //1. If Id == "" then => Insert
            //2. Else then => Update
            #region Insert
            if (colorIsValid.isNullValueId == true)
            {
                try
                {
                    var colorCodeIsExist = _context.ColorModel
                                                   .FirstOrDefault(p => p.ColorShortName == colorIsValid.ColorShortName);
                    if (colorCodeIsExist != null)
                    {
                        return string.Format(LanguageResource.Validation_Already_Exists, colorIsValid.ColorShortName);
                    }
                    else
                    {
                        ColorModel color = new ColorModel();
                        color.ColorId = Guid.NewGuid();
                        color.ColorShortName = colorIsValid.ColorShortName;
                        color.ColorCode = colorIsValid.ColorCode;
                        color.ColorName = colorIsValid.ColorName;
                        color.OrderIndex = colorIsValid.OrderIndex;
                        color.Actived = colorIsValid.Actived;
                        _context.Entry(color).State = EntityState.Added;
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
                    ColorModel color = _context.ColorModel.FirstOrDefault(p => p.ColorId == colorIsValid.ColorId);
                    if (color != null)
                    {
                        color.ColorCode = colorIsValid.ColorCode;
                        color.ColorName = colorIsValid.ColorName;
                        color.OrderIndex = colorIsValid.OrderIndex;
                        color.Actived = colorIsValid.Actived;
                        _context.Entry(color).State = EntityState.Modified;
                    }
                    else
                    {
                        return string.Format(LanguageResource.Validation_ImportExcelIdNotExist,
                                                LanguageResource.ColorId, colorIsValid.ColorId,
                                                string.Format(LanguageResource.Export_ExcelHeader,
                                                LanguageResource.Sale_Color));
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
        public ColorExcelViewModel CheckTemplate(object[] row, int index)
        {
            ColorExcelViewModel colorVM = new ColorExcelViewModel();
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
                            colorVM.RowIndex = rowIndex;
                            break;
                        //ColorId
                        case 1:
                            fieldName = LanguageResource.ColorId;
                            string colorId = row[i].ToString();
                            if (string.IsNullOrEmpty(colorId))
                            {
                                colorVM.isNullValueId = true;
                            }
                            else
                            {
                                colorVM.ColorId = Guid.Parse(colorId);
                                colorVM.isNullValueId = false;
                            }
                            break;
                        //ColorShortName (Mã màu)
                        case 2:
                            fieldName = LanguageResource.Color_ColorShortName;
                            string colorCode = row[i].ToString();
                            if (string.IsNullOrEmpty(colorCode))
                            {
                                colorVM.Error = string.Format(LanguageResource.Validation_ImportRequired, string.Format(LanguageResource.Required, LanguageResource.Color_ColorShortName), colorVM.RowIndex);
                            }
                            else
                            {
                                colorVM.ColorShortName = colorCode;
                            }
                            break;
                        //ColorCode (Mã RGB)
                        case 3:
                            fieldName = LanguageResource.Color_ColorCode;
                            string colorRGBCode = row[i].ToString();
                            if (string.IsNullOrEmpty(colorRGBCode))
                            {
                                colorVM.Error = string.Format(LanguageResource.Validation_ImportRequired, string.Format(LanguageResource.Required, LanguageResource.Color_ColorCode), colorVM.RowIndex);
                            }
                            else
                            {
                                colorVM.ColorCode = colorRGBCode;
                            }
                            break;
                        //ColorName
                        case 4:
                            fieldName = LanguageResource.Color_ColorName;
                            string colorName = row[i].ToString();
                            if (string.IsNullOrEmpty(colorName))
                            {
                                colorVM.Error = string.Format(LanguageResource.Validation_ImportRequired, string.Format(LanguageResource.Required, LanguageResource.Color_ColorName), colorVM.RowIndex);
                            }
                            else
                            {
                                colorVM.ColorName = colorName;
                            }
                            break;
                        //OrderIndex
                        case 5:
                            fieldName = LanguageResource.OrderIndex;
                            colorVM.OrderIndex = GetTypeFunction<int>(row[i].ToString(), i);
                            break;
                        //Actived
                        case 6:
                            fieldName = LanguageResource.Actived;
                            colorVM.Actived = GetTypeFunction<bool>(row[i].ToString(), i);
                            break;
                    }
                    #endregion Convert data to import
                }
            }
            catch (FormatException ex)
            {
                colorVM.Error = string.Format(LanguageResource.Validation_ImportCastValid, fieldName, index);
            }
            catch (InvalidCastException ex)
            {
                colorVM.Error = string.Format(LanguageResource.Validation_ImportCastValid, fieldName, index);
            }
            catch (Exception ex)
            {
                colorVM.Error = string.Format(LanguageResource.Validate_ImportException, fieldName, index);
            }
            return colorVM;
        }
        #endregion Check data type


        #endregion Import from excel

        //GET: /Color/Delete
        #region Delete
        [HttpPost]
        [ISDAuthorizationAttribute]
        public ActionResult Delete(Guid id)
        {
            return ExecuteDelete(() =>
            {
                var color = _context.ColorModel.FirstOrDefault(p => p.ColorId == id);
                if (color != null)
                {
                    _context.Entry(color).State = EntityState.Deleted;
                    _context.SaveChanges();

                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.Created,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Delete_Success, LanguageResource.Sale_Color.ToLower())
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
        private bool IsExists(string ColorShortName)
        {
            return (_context.ColorModel.FirstOrDefault(p => p.ColorShortName == ColorShortName) != null);
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult CheckExistingColorShortName(string ColorShortName, string ColorShortNameValid)
        {
            try
            {
                if (ColorShortNameValid != ColorShortName)
                {
                    return Json(!IsExists(ColorShortName));
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