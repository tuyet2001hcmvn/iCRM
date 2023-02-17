using AutoMapper;
using AutoMapper.QueryableExtensions;
using ISD.Constant;
using ISD.EntityModels;
using ISD.Extensions;
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
using ISD.Repositories.Excel;

namespace Sale.Controllers
{

    public class ConfigurationSearchViewModel
    {
        public string ConfigurationName { get; set; }
        public bool? Actived { get; set; }
    }

    public class ConfigurationController : BaseController
    {
        // GET: Configuration
        #region Index
        [ISDAuthorizationAttribute]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult _Search(ConfigurationSearchViewModel searchViewModel)
        {
            Session["frmSearchConfiguration"] = searchViewModel;

            return ExecuteSearch(() =>
            {
                var config = (from p in _context.ConfigurationModel
                              orderby p.OrderIndex.HasValue descending, p.OrderIndex
                              where
                              //search by ConfigurationName
                              (searchViewModel.ConfigurationName == null || p.ConfigurationName.Contains(searchViewModel.ConfigurationName))
                              //search by Actived
                              && (searchViewModel.Actived == null || p.Actived == searchViewModel.Actived)
                              select p)
                              .ToList();

                return PartialView(config);
            });
        }
        #endregion

        //GET: /Configuration/Create
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
        public JsonResult Create(ConfigurationModel model)
        {
            return ExecuteContainer(() =>
            {
                model.ConfigurationId = Guid.NewGuid();
                _context.Entry(model).State = EntityState.Added;
                _context.SaveChanges();

                return Json(new
                {
                    Code = System.Net.HttpStatusCode.Created,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Create_Success, LanguageResource.Sale_Configuration.ToLower())
                });
            });
        }
        #endregion

        //GET: /Configuration/Edit
        #region Edit
        [ISDAuthorizationAttribute]
        public ActionResult Edit(Guid id)
        {
            var config = _context.ConfigurationModel.FirstOrDefault(p => p.ConfigurationId == id);
            if (config == null)
            {
                return RedirectToAction("ErrorText", "Error", new { area = "", statusCode = 404, exception = string.Format(LanguageResource.Error_NotExist, LanguageResource.Sale_Configuration.ToLower()) });
            }
            return View(config);
        }
        //POST: Edit
        [HttpPost]
        [ISDAuthorizationAttribute]
        public JsonResult Edit(ConfigurationModel model)
        {
            return ExecuteContainer(() =>
            {
                _context.Entry(model).State = EntityState.Modified;
                _context.SaveChanges();

                return Json(new
                {
                    Code = System.Net.HttpStatusCode.Created,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Edit_Success, LanguageResource.Sale_Configuration.ToLower())
                });
            });
        }
        #endregion

        //Export
        #region Export to excel
        public ActionResult ExportCreate()
        {
            List<ConfigurationExcelViewModel> configuration = new List<ConfigurationExcelViewModel>();
            return Export(configuration, isEdit: false);
        }

        public ActionResult ExportEdit(ConfigurationSearchViewModel searchViewModel)
        {

            searchViewModel = (ConfigurationSearchViewModel)Session["frmSearchConfiguration"];


            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ConfigurationModel, ConfigurationExcelViewModel>();
            });

            //Get data filter
            //Get data from server
            var viewModelList = (from p in _context.ConfigurationModel
                                 orderby p.OrderIndex.HasValue descending, p.OrderIndex
                                 where
                                 //search by ConfigurationName
                                 (searchViewModel.ConfigurationName == null || p.ConfigurationName.Contains(searchViewModel.ConfigurationName))
                                 //search by Actived
                                 && (searchViewModel.Actived == null || p.Actived == searchViewModel.Actived)
                                 select p).ProjectTo<ConfigurationExcelViewModel>(config)
                               .ToList();
            return Export(viewModelList, isEdit: true);
        }

        const string controllerCode = ConstExcelController.Configuration;
        const int startIndex = 8;
        [ISDAuthorizationAttribute]
        public FileContentResult Export(List<ConfigurationExcelViewModel> viewModel, bool isEdit)
        {
            #region Master
            List<ExcelTemplate> columns = new List<ExcelTemplate>();

            columns.Add(new ExcelTemplate() { ColumnName = "ConfigurationId", isAllowedToEdit = false });
            columns.Add(new ExcelTemplate() { ColumnName = "ConfigurationCode", isAllowedToEdit = isEdit == true ? false : true });
            columns.Add(new ExcelTemplate() { ColumnName = "ConfigurationName", isAllowedToEdit = true });
            columns.Add(new ExcelTemplate() { ColumnName = "OrderIndex", isAllowedToEdit = true });
            columns.Add(new ExcelTemplate() { ColumnName = "Actived", isAllowedToEdit = true, isBoolean = true });
            #endregion Master

            //Header
            string fileheader = string.Format(LanguageResource.Export_ExcelHeader, LanguageResource.Sale_Configuration);
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
                Content = string.Format(LanguageResource.Export_ExcelWarningActived, LanguageResource.Sale_Configuration),
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
                                        ConfigurationExcelViewModel configurationIsValid = CheckTemplate(dr.ItemArray, index);

                                        if (!string.IsNullOrEmpty(configurationIsValid.Error))
                                        {
                                            string error = configurationIsValid.Error;
                                            errorList.Add(error);
                                        }
                                        else
                                        {
                                            string result = ExecuteImportExcelConfiguration(configurationIsValid);
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
                                string error = string.Format(LanguageResource.Validation_ImportCheckController, LanguageResource.Sale_Configuration);
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
        public string ExecuteImportExcelConfiguration(ConfigurationExcelViewModel configurationIsValid)
        {
            //Check:
            //1. If Id == "" then => Insert
            //2. Else then => Update
            #region Insert
            if (configurationIsValid.isNullValueId == true)
            {
                try
                {
                    var configurationCodeIsExist = _context.ConfigurationModel.FirstOrDefault(p => p.ConfigurationCode == configurationIsValid.ConfigurationCode);
                    if (configurationCodeIsExist != null)
                    {
                        return string.Format(LanguageResource.Validation_Already_Exists, configurationIsValid.ConfigurationCode);
                    }
                    else
                    {
                        ConfigurationModel configuration = new ConfigurationModel();
                        configuration.ConfigurationId = Guid.NewGuid();
                        configuration.ConfigurationCode = configurationIsValid.ConfigurationCode;
                        configuration.ConfigurationName = configurationIsValid.ConfigurationName;
                        configuration.OrderIndex = configurationIsValid.OrderIndex;
                        configuration.Actived = configurationIsValid.Actived;
                        _context.Entry(configuration).State = EntityState.Added;
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
                    ConfigurationModel configuration = _context.ConfigurationModel.FirstOrDefault(p => p.ConfigurationId == configurationIsValid.ConfigurationId);
                    if (configuration != null)
                    {
                        configuration.ConfigurationName = configurationIsValid.ConfigurationName;
                        configuration.OrderIndex = configurationIsValid.OrderIndex;
                        configuration.Actived = configurationIsValid.Actived;
                        _context.Entry(configuration).State = EntityState.Modified;
                    }
                    else
                    {
                        return string.Format(LanguageResource.Validation_ImportExcelIdNotExist,
                                                LanguageResource.ConfigurationId, configurationIsValid.ConfigurationId,
                                                string.Format(LanguageResource.Export_ExcelHeader,
                                                LanguageResource.Sale_Configuration));
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
        public ConfigurationExcelViewModel CheckTemplate(object[] row, int index)
        {
            ConfigurationExcelViewModel configurationVM = new ConfigurationExcelViewModel();
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
                            configurationVM.RowIndex = rowIndex;
                            break;
                        //ConfigurationId
                        case 1:
                            fieldName = LanguageResource.ConfigurationId;
                            string configurationId = row[i].ToString();
                            if (string.IsNullOrEmpty(configurationId))
                            {
                                configurationVM.isNullValueId = true;
                            }
                            else
                            {
                                configurationVM.ConfigurationId = Guid.Parse(configurationId);
                                configurationVM.isNullValueId = false;
                            }
                            break;
                        //ConfigurationCode
                        case 2:
                            fieldName = LanguageResource.Configuration_ConfigurationCode;
                            string configurationCode = row[i].ToString();
                            if (string.IsNullOrEmpty(configurationCode))
                            {
                                configurationVM.Error = string.Format(LanguageResource.Validation_ImportRequired, string.Format(LanguageResource.Required, LanguageResource.Configuration_ConfigurationCode), configurationVM.RowIndex);
                            }
                            else
                            {
                                configurationVM.ConfigurationCode = configurationCode;
                            }
                            break;
                        //ConfigurationName
                        case 3:
                            fieldName = LanguageResource.Configuration_ConfigurationName;
                            string configurationName = row[i].ToString();
                            if (string.IsNullOrEmpty(configurationName))
                            {
                                configurationVM.Error = string.Format(LanguageResource.Validation_ImportRequired, string.Format(LanguageResource.Required, LanguageResource.Configuration_ConfigurationName), configurationVM.RowIndex);
                            }
                            else
                            {
                                configurationVM.ConfigurationName = configurationName;
                            }
                            break;
                        //OrderIndex
                        case 4:
                            fieldName = LanguageResource.OrderIndex;
                            configurationVM.OrderIndex = GetTypeFunction<int>(row[i].ToString(), i);
                            break;
                        //Actived
                        case 5:
                            fieldName = LanguageResource.Actived;
                            configurationVM.Actived = GetTypeFunction<bool>(row[i].ToString(), i);
                            break;
                    }
                    #endregion Convert data to import
                }
            }
            catch (FormatException ex)
            {
                var Message = ex.Message;
                configurationVM.Error = string.Format(LanguageResource.Validation_ImportCastValid, fieldName, index);
            }
            catch (InvalidCastException ex)
            {
                var Message = ex.Message;
                configurationVM.Error = string.Format(LanguageResource.Validation_ImportCastValid, fieldName, index);
            }
            catch (Exception ex)
            {
                var Message = ex.Message;
                configurationVM.Error = string.Format(LanguageResource.Validate_ImportException, fieldName, index);
            }
            return configurationVM;
        }
        #endregion Check data type


        #endregion Import from excel

        //GET: /Configuration/Delete
        #region Delete
        [HttpPost]
        [ISDAuthorizationAttribute]
        public ActionResult Delete(Guid id)
        {
            return ExecuteDelete(() =>
            {
                var config = _context.ConfigurationModel.FirstOrDefault(p => p.ConfigurationId == id);
                if (config != null)
                {
                    _context.Entry(config).State = EntityState.Deleted;
                    _context.SaveChanges();

                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.Created,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Delete_Success, LanguageResource.Sale_Configuration.ToLower())
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
        private bool IsExists(string ConfigurationCode)
        {
            return (_context.ConfigurationModel.FirstOrDefault(p => p.ConfigurationCode == ConfigurationCode) != null);
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult CheckExistingConfigurationCode(string ConfigurationCode, string ConfigurationCodeValid)
        {
            try
            {
                if (ConfigurationCodeValid != ConfigurationCode)
                {
                    return Json(!IsExists(ConfigurationCode));
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