using AutoMapper;
using AutoMapper.QueryableExtensions;
using ISD.Constant;
using ISD.EntityModels;
using ISD.Extensions;
using ISD.Resources;
using ISD.Core;
using ISD.ViewModels;
using ISD.ViewModels.MasterData;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using ISD.Repositories.Excel;

namespace MasterData.Controllers
{
    public class ProvinceController : BaseController
    {
        // GET: Province
        #region Index
        [ISDAuthorizationAttribute]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult _Search(ProvinceSearchViewModel searchViewModel)
        {
            Session["frmSearchProvince"] = searchViewModel;
            return ExecuteSearch(() =>
            {
                var provinceList = (from p in _context.ProvinceModel
                                    orderby p.Area, p.OrderIndex, p.ProvinceName
                                    where
                                    //search by ProvinceName
                                    (searchViewModel.ProvinceName == null || p.ProvinceName.Contains(searchViewModel.ProvinceName))
                                    //search by Actived
                                    && (searchViewModel.Actived == null || p.Actived == searchViewModel.Actived)
                                    select p).ToList();

                return PartialView(provinceList);
            });
        }
        #endregion

        //GET: /Province/Create
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
        public JsonResult Create(ProvinceModel model)
        {
            return ExecuteContainer(() =>
            {
                model.ProvinceId = Guid.NewGuid();
                _context.Entry(model).State = EntityState.Added;
                _context.SaveChanges();

                return Json(new
                {
                    Code = System.Net.HttpStatusCode.Created,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Create_Success, LanguageResource.MasterData_Province.ToLower())
                });
            });
        }
        #endregion

        //GET: /Province/Edit
        #region Edit
        [ISDAuthorizationAttribute]
        public ActionResult Edit(Guid id)
        {
            var province = (from p in _context.ProvinceModel
                            where p.ProvinceId == id
                            select new ProvinceViewModel()
                            {
                                ProvinceId = p.ProvinceId,
                                ProvinceCode = p.ProvinceCode,
                                ProvinceCodeValid = p.ProvinceCode,
                                ProvinceName = p.ProvinceName,
                                Area = p.Area,
                                OrderIndex = p.OrderIndex,
                                Actived = p.Actived,
                                ConfigPriceCode = p.ConfigPriceCode,
                                IsHasLicensePrice = p.IsHasLicensePrice,
                            }).FirstOrDefault();
            if (province == null)
            {
                return RedirectToAction("ErrorText", "Error", new { area = "", statusCode = 404, exception = string.Format(LanguageResource.Error_NotExist, LanguageResource.MasterData_Province.ToLower()) });
            }

            //Giá tính thuế tính theo 1 trong 3 loại:
            //1. giá xuất hóa đơn
            ViewBag.GiaXuatHD = _context.ApplicationConfig.Where(p => p.ConfigKey == ConstProvince.GiaXuatHoaDonKey).Select(p => p.ConfigValue).FirstOrDefault();
            //2. giá áp thuế
            ViewBag.GiaApThue = _context.ApplicationConfig.Where(p => p.ConfigKey == ConstProvince.GiaApThueKey).Select(p => p.ConfigValue).FirstOrDefault();
            //3. giá cao hơn
            ViewBag.GiaCaoHon = _context.ApplicationConfig.Where(p => p.ConfigKey == ConstProvince.GiaCaoHonKey).Select(p => p.ConfigValue).FirstOrDefault();

            return View(province);
        }
        //POST: Edit
        [HttpPost]
        [ISDAuthorizationAttribute]
        public JsonResult Edit(ProvinceModel model)
        {
            return ExecuteContainer(() =>
            {
                _context.Entry(model).State = EntityState.Modified;
                _context.SaveChanges();

                return Json(new
                {
                    Code = System.Net.HttpStatusCode.Created,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Edit_Success, LanguageResource.MasterData_Province.ToLower())
                });
            });
        }
        #endregion

        //Export
        #region Export to excel
        public ActionResult ExportCreate()
        {
            List<ProvinceExcelViewModel> viewModel = new List<ProvinceExcelViewModel>();
            return Export(viewModel, isEdit: false);
        }

        public ActionResult ExportEdit(ProvinceSearchViewModel searchViewModel)
        {
            searchViewModel = (ProvinceSearchViewModel)Session["frmSearchProvince"];

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ProvinceModel, ProvinceExcelViewModel>();
            });

            //Get data filter
            //Get data from server
            var province = (from p in _context.ProvinceModel
                            orderby p.Area, p.OrderIndex, p.ProvinceName
                            where
                            //search by ProvinceName
                            (searchViewModel.ProvinceName == null || p.ProvinceName.Contains(searchViewModel.ProvinceName))
                            //search by Actived
                            && (searchViewModel.Actived == null || p.Actived == searchViewModel.Actived)
                            select p).ProjectTo<ProvinceExcelViewModel>(config)
                               .ToList();
            return Export(province, isEdit: true);
        }

        const string controllerCode = ConstExcelController.Province;
        const int startIndex = 8;
        [ISDAuthorizationAttribute]
        public FileContentResult Export(List<ProvinceExcelViewModel> viewModel, bool isEdit)
        {
            #region Master
            List<ExcelTemplate> columns = new List<ExcelTemplate>();

            columns.Add(new ExcelTemplate() { ColumnName = "ProvinceId", isAllowedToEdit = false });
            columns.Add(new ExcelTemplate() { ColumnName = "ProvinceCode", isAllowedToEdit = true });
            columns.Add(new ExcelTemplate() { ColumnName = "ProvinceName", isAllowedToEdit = true });
            columns.Add(new ExcelTemplate() { ColumnName = "Area", isAllowedToEdit = true });
            columns.Add(new ExcelTemplate() { ColumnName = "OrderIndex", isAllowedToEdit = true });
            columns.Add(new ExcelTemplate() { ColumnName = "Actived", isAllowedToEdit = true, isBoolean = true });
            // TODO: Upload hình ảnh
            //columns.Add(new ExcelTemplate() { ColumnName = "ImageUrl ", isAllowedToEdit = false });
            #endregion Master

            //Header
            string fileheader = string.Format(LanguageResource.Export_ExcelHeader, LanguageResource.MasterData_Province);
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
                Content = string.Format(LanguageResource.Export_ExcelWarningActived, LanguageResource.MasterData_Province),
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
                                        ProvinceExcelViewModel provinceIsValid = CheckTemplate(dr.ItemArray, index);

                                        if (!string.IsNullOrEmpty(provinceIsValid.Error))
                                        {
                                            string error = provinceIsValid.Error;
                                            errorList.Add(error);
                                        }
                                        else
                                        {
                                            string result = ExecuteImportExcelProvince(provinceIsValid);
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
                                string error = string.Format(LanguageResource.Validation_ImportCheckController, LanguageResource.MasterData_Province);
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
        public string ExecuteImportExcelProvince(ProvinceExcelViewModel provinceIsValid)
        {
            //Check:
            //1. If Id == "" then => Insert
            //2. Else then => Update
            #region Insert
            if (provinceIsValid.isNullValueId == true)
            {
                try
                {
                    var provinceCodeIsExist = _context.ProvinceModel.FirstOrDefault(p => p.ProvinceCode == provinceIsValid.ProvinceCode);
                    if (provinceCodeIsExist != null)
                    {
                        return string.Format(LanguageResource.Validation_Already_Exists, provinceIsValid.ProvinceCode);
                    }
                    else
                    {
                        ProvinceModel province = new ProvinceModel();
                        province.ProvinceId = Guid.NewGuid();
                        province.ProvinceCode = provinceIsValid.ProvinceCode;
                        province.ProvinceName = provinceIsValid.ProvinceName;
                        province.Area = provinceIsValid.Area;
                        province.OrderIndex = provinceIsValid.OrderIndex;
                        province.Actived = provinceIsValid.Actived;
                        _context.Entry(province).State = EntityState.Added;
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
                    ProvinceModel province = _context.ProvinceModel.FirstOrDefault(p => p.ProvinceId == provinceIsValid.ProvinceId);
                    if (province != null)
                    {
                        province.ProvinceCode = provinceIsValid.ProvinceCode;
                        province.ProvinceName = provinceIsValid.ProvinceName;
                        province.Area = provinceIsValid.Area;
                        province.OrderIndex = provinceIsValid.OrderIndex;
                        province.Actived = provinceIsValid.Actived;
                        _context.Entry(province).State = EntityState.Modified;
                    }
                    else
                    {
                        return string.Format(LanguageResource.Validation_ImportExcelIdNotExist,
                                                LanguageResource.Excel_ProvinceId, provinceIsValid.ProvinceId,
                                                string.Format(LanguageResource.Export_ExcelHeader,
                                                LanguageResource.MasterData_Province));
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
        public ProvinceExcelViewModel CheckTemplate(object[] row, int index)
        {
            ProvinceExcelViewModel provinceVM = new ProvinceExcelViewModel();
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
                            provinceVM.RowIndex = rowIndex;
                            break;
                        //ProvinceId
                        case 1:
                            fieldName = LanguageResource.Excel_ProvinceId;
                            string provinceId = row[i].ToString();
                            if (string.IsNullOrEmpty(provinceId))
                            {
                                provinceVM.isNullValueId = true;
                            }
                            else
                            {
                                provinceVM.ProvinceId = Guid.Parse(provinceId);
                                provinceVM.isNullValueId = false;
                            }
                            break;
                        //ProvinceCode
                        case 2:
                            fieldName = LanguageResource.Province_ProvinceCode;
                            string provinceCode = row[i].ToString();
                            if (string.IsNullOrEmpty(provinceCode))
                            {
                                provinceVM.Error = string.Format(LanguageResource.Validation_ImportRequired, string.Format(LanguageResource.Required, LanguageResource.Province_ProvinceCode), provinceVM.RowIndex);
                            }
                            else
                            {
                                provinceVM.ProvinceCode = provinceCode;
                            }
                            break;
                        //ProvinceName
                        case 3:
                            fieldName = LanguageResource.Province_ProvinceName;
                            string provinceName = row[i].ToString();
                            if (string.IsNullOrEmpty(provinceName))
                            {
                                provinceVM.Error = string.Format(LanguageResource.Validation_ImportRequired, string.Format(LanguageResource.Required, LanguageResource.Province_ProvinceName), provinceVM.RowIndex);
                            }
                            else
                            {
                                provinceVM.ProvinceName = provinceName;
                            }
                            break;
                        //Area
                        case 4:
                            fieldName = LanguageResource.Area;
                            provinceVM.Area = GetTypeFunction<int>(row[i].ToString(), i);
                            break;
                        //OrderIndex
                        case 5:
                            fieldName = LanguageResource.OrderIndex;
                            provinceVM.OrderIndex = GetTypeFunction<int>(row[i].ToString(), i);
                            break;
                        //Actived
                        case 6:
                            fieldName = LanguageResource.Actived;
                            provinceVM.Actived = GetTypeFunction<bool>(row[i].ToString(), i);
                            break;
                    }
                    #endregion Convert data to import
                }
            }
            catch (FormatException ex)
            {
                var Message = ex.Message;
                provinceVM.Error = string.Format(LanguageResource.Validation_ImportCastValid, fieldName, index);
            }
            catch (InvalidCastException ex)
            {
                var Message = ex.Message;
                provinceVM.Error = string.Format(LanguageResource.Validation_ImportCastValid, fieldName, index);
            }
            catch (Exception ex)
            {
                var Message = ex.Message;
                provinceVM.Error = string.Format(LanguageResource.Validate_ImportException, fieldName, index);
            }
            return provinceVM;
        }
        #endregion Check data type


        #endregion Import from excel

        //GET: /Province/Delete
        #region Delete
        [HttpPost]
        [ISDAuthorizationAttribute]
        public ActionResult Delete(Guid id)
        {
            return ExecuteDelete(() =>
            {
                var province = _context.ProvinceModel.FirstOrDefault(p => p.ProvinceId == id);
                if (province != null)
                {
                    _context.Entry(province).State = EntityState.Deleted;
                    _context.SaveChanges();

                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.Created,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Delete_Success, LanguageResource.MasterData_Province.ToLower())
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
        private bool IsExists(string ProvinceCode)
        {
            return (_context.ProvinceModel.FirstOrDefault(p => p.ProvinceCode == ProvinceCode) != null);
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult CheckExistingProvinceCode(string ProvinceCode, string ProvinceCodeValid)
        {
            try
            {
                if (ProvinceCodeValid != ProvinceCode)
                {
                    return Json(!IsExists(ProvinceCode));
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