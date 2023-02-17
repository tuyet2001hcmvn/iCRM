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

    public class StyleSearchViewModel
    {
        public string StyleName { get; set; }
        public bool? Actived { get; set; }
    }

    public class StyleController : BaseController
    {
        // GET: Style
        #region Index
        [ISDAuthorizationAttribute]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult _Search(StyleSearchViewModel searchViewModel)
        {
            Session["frmSearchStyle"] = searchViewModel;

            return ExecuteSearch(() =>
            {
                var style = (from p in _context.StyleModel
                             orderby p.OrderIndex.HasValue descending, p.OrderIndex
                             where
                             //search by StyleName
                             (searchViewModel.StyleName == null || p.StyleName.Contains(searchViewModel.StyleName))
                             //search by Actived
                             && (searchViewModel.Actived == null || p.Actived == searchViewModel.Actived)
                             select p)
                             .ToList();

                return PartialView(style);
            });
        }
        #endregion

        //GET: /Style/Create
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
        public JsonResult Create(StyleModel model)
        {
            return ExecuteContainer(() =>
            {
                model.StyleId = Guid.NewGuid();
                _context.Entry(model).State = EntityState.Added;
                _context.SaveChanges();

                return Json(new
                {
                    Code = System.Net.HttpStatusCode.Created,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Create_Success, LanguageResource.Sale_Style.ToLower())
                });
            });
        }
        #endregion

        //GET: /Style/Edit
        #region Edit
        [ISDAuthorizationAttribute]
        public ActionResult Edit(Guid id)
        {
            var style = _context.StyleModel.FirstOrDefault(p => p.StyleId == id);
            if (style == null)
            {
                return RedirectToAction("ErrorText", "Error", new { area = "", statusCode = 404, exception = string.Format(LanguageResource.Error_NotExist, LanguageResource.Sale_Style.ToLower()) });
            }
            return View(style);
        }
        //POST: Edit
        [HttpPost]
        [ISDAuthorizationAttribute]
        public JsonResult Edit(StyleModel model)
        {
            return ExecuteContainer(() =>
            {
                _context.Entry(model).State = EntityState.Modified;
                _context.SaveChanges();

                return Json(new
                {
                    Code = System.Net.HttpStatusCode.Created,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Edit_Success, LanguageResource.Sale_Style.ToLower())
                });
            });
        }
        #endregion

        //Export
        #region Export to excel
        public ActionResult ExportCreate()
        {
            List<StyleExcelViewModel> viewModel = new List<StyleExcelViewModel>();
            return Export(viewModel, isEdit: false);
        }

        public ActionResult ExportEdit(StyleSearchViewModel searchViewModel)
        {

            searchViewModel = (StyleSearchViewModel)Session["frmSearchStyle"];


            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<StyleModel, StyleExcelViewModel>();
            });

            //Get data filter
            //Get data from server
            var style = (from p in _context.StyleModel
                         orderby p.OrderIndex
                         where
                         //search by StyleName
                         (searchViewModel.StyleName == null || p.StyleName.Contains(searchViewModel.StyleName))
                         //search by Actived
                         && (searchViewModel.Actived == null || p.Actived == searchViewModel.Actived)
                         select p).ProjectTo<StyleExcelViewModel>(config)
                               .ToList();
            return Export(style, isEdit: true);
        }

        const string controllerCode = ConstExcelController.Style;
        const int startIndex = 8;
        [ISDAuthorizationAttribute]
        public FileContentResult Export(List<StyleExcelViewModel> viewModel, bool isEdit)
        {
            #region Master
            List<ExcelTemplate> columns = new List<ExcelTemplate>();

            columns.Add(new ExcelTemplate() { ColumnName = "StyleId", isAllowedToEdit = false });
            columns.Add(new ExcelTemplate() { ColumnName = "StyleCode", isAllowedToEdit = isEdit == true ? false : true });
            columns.Add(new ExcelTemplate() { ColumnName = "StyleName", isAllowedToEdit = true });
            columns.Add(new ExcelTemplate() { ColumnName = "OrderIndex", isAllowedToEdit = true });
            columns.Add(new ExcelTemplate() { ColumnName = "Actived", isAllowedToEdit = true, isBoolean = true });
            // TODO: Upload hình ảnh
            //columns.Add(new ExcelTemplate() { ColumnName = "ImageUrl ", isAllowedToEdit = false });
            #endregion Master

            //Header
            string fileheader = string.Format(LanguageResource.Export_ExcelHeader, LanguageResource.Sale_Style);
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
                Content = string.Format(LanguageResource.Export_ExcelWarningActived, LanguageResource.Sale_Style),
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
                                        StyleExcelViewModel styleIsValid = CheckTemplate(dr.ItemArray, index);

                                        if (!string.IsNullOrEmpty(styleIsValid.Error))
                                        {
                                            string error = styleIsValid.Error;
                                            errorList.Add(error);
                                        }
                                        else
                                        {
                                            string result = ExecuteImportExcelStyle(styleIsValid);
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
                                string error = string.Format(LanguageResource.Validation_ImportCheckController, LanguageResource.Sale_Style);
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
        public string ExecuteImportExcelStyle(StyleExcelViewModel styleIsValid)
        {
            //Check:
            //1. If Id == "" then => Insert
            //2. Else then => Update
            #region Insert
            if (styleIsValid.isNullValueId == true)
            {
                try
                {
                    var styleCodeIsExist = _context.StyleModel.FirstOrDefault(p => p.StyleCode == styleIsValid.StyleCode);
                    if (styleCodeIsExist != null)
                    {
                        return string.Format(LanguageResource.Validation_Already_Exists, styleIsValid.StyleCode);
                    }
                    else
                    {
                        StyleModel style = new StyleModel();
                        style.StyleId = Guid.NewGuid();
                        style.StyleCode = styleIsValid.StyleCode;
                        style.StyleName = styleIsValid.StyleName;
                        style.OrderIndex = styleIsValid.OrderIndex;
                        style.Actived = styleIsValid.Actived;
                        _context.Entry(style).State = EntityState.Added;
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
                    StyleModel style = _context.StyleModel.FirstOrDefault(p => p.StyleId == styleIsValid.StyleId);
                    if (style != null)
                    {
                        style.StyleName = styleIsValid.StyleName;
                        style.OrderIndex = styleIsValid.OrderIndex;
                        style.Actived = styleIsValid.Actived;
                        _context.Entry(style).State = EntityState.Modified;
                    }
                    else
                    {
                        return string.Format(LanguageResource.Validation_ImportExcelIdNotExist,
                                                LanguageResource.StyleId, styleIsValid.StyleId,
                                                string.Format(LanguageResource.Export_ExcelHeader,
                                                LanguageResource.Sale_Style));
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
        public StyleExcelViewModel CheckTemplate(object[] row, int index)
        {
            StyleExcelViewModel styleVM = new StyleExcelViewModel();
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
                            styleVM.RowIndex = rowIndex;
                            break;
                        //StyleId
                        case 1:
                            fieldName = LanguageResource.StyleId;
                            string styleId = row[i].ToString();
                            if (string.IsNullOrEmpty(styleId))
                            {
                                styleVM.isNullValueId = true;
                            }
                            else
                            {
                                styleVM.StyleId = Guid.Parse(styleId);
                                styleVM.isNullValueId = false;
                            }
                            break;
                        //StyleCode
                        case 2:
                            fieldName = LanguageResource.Style_StyleCode;
                            string styleCode = row[i].ToString();
                            if (string.IsNullOrEmpty(styleCode))
                            {
                                styleVM.Error = string.Format(LanguageResource.Validation_ImportRequired, string.Format(LanguageResource.Required, LanguageResource.Style_StyleCode), styleVM.RowIndex);
                            }
                            else
                            {
                                styleVM.StyleCode = styleCode;
                            }
                            break;
                        //StyleName
                        case 3:
                            fieldName = LanguageResource.Style_StyleName;
                            string styleName = row[i].ToString();
                            if (string.IsNullOrEmpty(styleName))
                            {
                                styleVM.Error = string.Format(LanguageResource.Validation_ImportRequired, string.Format(LanguageResource.Required, LanguageResource.Style_StyleName), styleVM.RowIndex);
                            }
                            else
                            {
                                styleVM.StyleName = styleName;
                            }
                            break;
                        //OrderIndex
                        case 4:
                            fieldName = LanguageResource.OrderIndex;
                            styleVM.OrderIndex = GetTypeFunction<int>(row[i].ToString(), i);
                            break;
                        //Actived
                        case 5:
                            fieldName = LanguageResource.Actived;
                            styleVM.Actived = GetTypeFunction<bool>(row[i].ToString(), i);
                            break;
                    }
                    #endregion Convert data to import
                }
            }
            catch (FormatException ex)
            {
                styleVM.Error = string.Format(LanguageResource.Validation_ImportCastValid, fieldName, index);
            }
            catch (InvalidCastException ex)
            {
                styleVM.Error = string.Format(LanguageResource.Validation_ImportCastValid, fieldName, index);
            }
            catch (Exception ex)
            {
                styleVM.Error = string.Format(LanguageResource.Validate_ImportException, fieldName, index);
            }
            return styleVM;
        }
        #endregion Check data type


        #endregion Import from excel

        //GET: /Style/Delete
        #region Delete
        [HttpPost]
        [ISDAuthorizationAttribute]
        public ActionResult Delete(Guid id)
        {
            return ExecuteDelete(() =>
            {
                var style = _context.StyleModel.FirstOrDefault(p => p.StyleId == id);
                if (style != null)
                {
                    _context.Entry(style).State = EntityState.Deleted;
                    _context.SaveChanges();

                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.Created,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Delete_Success, LanguageResource.Sale_Style.ToLower())
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
        private bool IsExists(string StyleCode)
        {
            return (_context.StyleModel.FirstOrDefault(p => p.StyleCode == StyleCode) != null);
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult CheckExistingStyleCode(string StyleCode, string StyleCodeValid)
        {
            try
            {
                if (StyleCodeValid != StyleCode)
                {
                    return Json(!IsExists(StyleCode));
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