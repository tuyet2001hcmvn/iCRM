using AutoMapper;
using AutoMapper.QueryableExtensions;
using ISD.Constant;
using ISD.EntityModels;
using ISD.Extensions;
using ISD.Repositories;
using ISD.Resources;
using ISD.ViewModels;
using ISD.ViewModels.MasterData;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Transactions;
using System.Web.Mvc;
using ISD.Core;
using ISD.Repositories.Excel;

namespace MasterData.Controllers
{
    public class DistrictController : BaseController
    {
        // GET: District
        #region Index
        [ISDAuthorizationAttribute]
        public ActionResult Index()
        {
            //Get list Province
            var provinceList = _context.ProvinceModel.Where(p => p.Actived == true)
                                                     .OrderBy(p => p.Area)
                                                     .ThenBy(p => p.ProvinceName).ToList();
            ViewBag.ProvinceId = new SelectList(provinceList, "ProvinceId", "ProvinceName");

            return View();
        }

        public ActionResult _Search(DistrictSearchViewModel searchViewModel)
        {
            Session["frmSearchDistrict"] = searchViewModel;

            return ExecuteSearch(() =>
            {
                var district = (from p in _context.DistrictModel
                                join pr in _context.ProvinceModel on p.ProvinceId equals pr.ProvinceId
                                orderby pr.Area, pr.ProvinceName, p.Appellation, p.DistrictName
                                where
                                //search by ProvinceId
                                (searchViewModel.ProvinceId == null || p.ProvinceId == searchViewModel.ProvinceId)
                                //search by DistrictName
                                && (searchViewModel.DistrictName == null || (p.DistrictName.Contains(searchViewModel.DistrictName) ||
                                                           p.Appellation.Contains(searchViewModel.DistrictName)))
                                //search by Actived
                                && (searchViewModel.Actived == null || p.Actived == searchViewModel.Actived)
                                select new DistrictViewModel()
                                {
                                    ProvinceName = pr.ProvinceName,
                                    DistrictId = p.DistrictId,
                                    DistrictCode = p.DistrictCode,
                                    DistrictName = p.Appellation + " " + p.DistrictName,
                                    OrderIndex = p.OrderIndex,
                                    Actived = p.Actived
                                })
                                .Take(200)
                                .ToList();

                return PartialView(district);
            });
        }
        #endregion

        //GET: /District/Create
        #region Create
        [ISDAuthorizationAttribute]
        public ActionResult Create()
        {
            //Get list Province
            var provinceList = _context.ProvinceModel.Where(p => p.Actived == true)
                                                     .OrderBy(p => p.Area)
                                                     .ThenBy(p => p.ProvinceName).ToList();
            ViewBag.ProvinceId = new SelectList(provinceList, "ProvinceId", "ProvinceName");

            return View();
        }
        //POST: Create
        [HttpPost]
        [ValidateAjax]
        [ISDAuthorizationAttribute]
        public JsonResult Create(DistrictModel model)
        {
            return ExecuteContainer(() =>
            {
                model.DistrictId = Guid.NewGuid();
                _context.Entry(model).State = EntityState.Added;
                _context.SaveChanges();

                return Json(new
                {
                    Code = System.Net.HttpStatusCode.Created,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Create_Success, LanguageResource.MasterData_District.ToLower())
                });
            });
        }
        #endregion

        //GET: /District/Edit
        #region Edit
        [ISDAuthorizationAttribute]
        public ActionResult Edit(Guid id)
        {
            var district = (from p in _context.DistrictModel
                            join pr in _context.ProvinceModel on p.ProvinceId equals pr.ProvinceId
                            where p.DistrictId == id
                            select new DistrictViewModel()
                            {
                                ProvinceId = p.ProvinceId,
                                ProvinceName = pr.ProvinceName,
                                DistrictId = p.DistrictId,
                                DistrictCode = p.DistrictCode,
                                DistrictCodeValid = p.DistrictCode,
                                Appellation = p.Appellation,
                                DistrictName = p.DistrictName,
                                RegisterVAT = p.RegisterVAT,
                                VehicleRegistrationSignature = p.VehicleRegistrationSignature,
                                OrderIndex = p.OrderIndex,
                                Actived = p.Actived
                            })
                         .FirstOrDefault();
            if (district == null)
            {
                return RedirectToAction("ErrorText", "Error", new { area = "", statusCode = 404, exception = string.Format(LanguageResource.Error_NotExist, LanguageResource.MasterData_District.ToLower()) });
            }
            return View(district);
        }
        //POST: Edit
        [HttpPost]
        [ISDAuthorizationAttribute]
        public JsonResult Edit(DistrictModel model)
        {
            return ExecuteContainer(() =>
            {
                _context.Entry(model).State = EntityState.Modified;
                _context.SaveChanges();

                return Json(new
                {
                    Code = System.Net.HttpStatusCode.Created,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Edit_Success, LanguageResource.MasterData_District.ToLower())
                });
            });
        }
        #endregion

        //Export
        #region Export to excel
        public ActionResult ExportCreate()
        {
            List<DistrictExcelViewModel> viewModel = new List<DistrictExcelViewModel>();
            return Export(viewModel, isEdit: false);
        }

        public ActionResult ExportEdit(DistrictSearchViewModel searchViewModel)
        {
            searchViewModel = (DistrictSearchViewModel)Session["frmSearchDistrict"];

            //Get data filter
            //Get data from server
            var district = (from p in _context.DistrictModel
                            join pr in _context.ProvinceModel on p.ProvinceId equals pr.ProvinceId
                            orderby pr.Area, pr.ProvinceName, p.Appellation, p.DistrictName
                            where
                            //search by ProvinceId
                            (searchViewModel.ProvinceId == null || p.ProvinceId == searchViewModel.ProvinceId)
                            //search by DistrictName
                            && (searchViewModel.DistrictName == null || (p.DistrictName.Contains(searchViewModel.DistrictName) ||
                                                       p.Appellation.Contains(searchViewModel.DistrictName)))
                            //search by Actived
                            && (searchViewModel.Actived == null || p.Actived == searchViewModel.Actived)
                            select new DistrictExcelViewModel()
                            {
                                ProvinceName = pr.ProvinceName,
                                ProvinceId = pr.ProvinceId,
                                DistrictId = p.DistrictId,
                                DistrictCode = p.DistrictCode,
                                Appellation = p.Appellation,
                                DistrictName = p.DistrictName,
                                OrderIndex = p.OrderIndex,
                                Actived = p.Actived
                            }).ToList();
            return Export(district, isEdit: true);
        }

        const string controllerCode = ConstExcelController.District;
        const int startIndex = 8;
        [ISDAuthorizationAttribute]
        public FileContentResult Export(List<DistrictExcelViewModel> viewModel, bool isEdit)
        {
            #region Dropdownlist
            //Tỉnh thành
            List<DropdownModel> ProvinceId = (from c in _context.ProvinceModel
                                              where c.Actived == true
                                              orderby c.Area, c.ProvinceName
                                              select new DropdownModel()
                                              {
                                                  Id = c.ProvinceId,
                                                  Name = c.ProvinceName,
                                              }).ToList();

            //Loại quận huyện
            List<DropdownIdTypeStringModel> Appellation = new List<DropdownIdTypeStringModel>();
            Appellation.Add(new DropdownIdTypeStringModel() { Id = ConstAppellation.ThanhPho, Name = ConstAppellation.ThanhPho });
            Appellation.Add(new DropdownIdTypeStringModel() { Id = ConstAppellation.Quan, Name = ConstAppellation.Quan });
            Appellation.Add(new DropdownIdTypeStringModel() { Id = ConstAppellation.Huyen, Name = ConstAppellation.Huyen });
            Appellation.Add(new DropdownIdTypeStringModel() { Id = ConstAppellation.ThiXa, Name = ConstAppellation.ThiXa });
            #endregion Dropdownlist

            #region Master
            List<ExcelTemplate> columns = new List<ExcelTemplate>();

            columns.Add(new ExcelTemplate() { ColumnName = "DistrictId", isAllowedToEdit = false });
            columns.Add(new ExcelTemplate() { ColumnName = "DistrictCode", isAllowedToEdit = true });
            //insert => dropdownlist
            //edit => not allow edit
            if (isEdit == true)
            {
                columns.Add(new ExcelTemplate()
                {
                    ColumnName = "ProvinceName",
                    isAllowedToEdit = false
                });
            }
            else
            {
                columns.Add(new ExcelTemplate()
                {
                    ColumnName = "ProvinceName",
                    isAllowedToEdit = true,
                    isDropdownlist = true,
                    TypeId = ConstExcelController.GuidId,
                    DropdownData = ProvinceId
                });
            }
            columns.Add(new ExcelTemplate()
            {
                ColumnName = "Appellation",
                isAllowedToEdit = true,
                isDropdownlist = true,
                TypeId = ConstExcelController.StringId,
                DropdownIdTypeStringData = Appellation
            });
            columns.Add(new ExcelTemplate() { ColumnName = "DistrictName", isAllowedToEdit = true });
            columns.Add(new ExcelTemplate() { ColumnName = "OrderIndex", isAllowedToEdit = true });
            columns.Add(new ExcelTemplate() { ColumnName = "Actived", isAllowedToEdit = true, isBoolean = true });
            // TODO: Upload hình ảnh
            //columns.Add(new ExcelTemplate() { ColumnName = "ImageUrl ", isAllowedToEdit = false });
            #endregion Master

            //Header
            string fileheader = string.Format(LanguageResource.Export_ExcelHeader, LanguageResource.MasterData_District);
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
                Content = string.Format(LanguageResource.Export_ExcelWarningActived, LanguageResource.MasterData_District),
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
                                        DistrictExcelViewModel districtIsValid = CheckTemplate(dr.ItemArray, index);

                                        if (!string.IsNullOrEmpty(districtIsValid.Error))
                                        {
                                            string error = districtIsValid.Error;
                                            errorList.Add(error);
                                        }
                                        else
                                        {
                                            string result = ExecuteImportExcelDistrict(districtIsValid);
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
                                string error = string.Format(LanguageResource.Validation_ImportCheckController, LanguageResource.MasterData_District);
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
        public string ExecuteImportExcelDistrict(DistrictExcelViewModel districtIsValid)
        {
            //Check:
            //1. If Id == "" then => Insert
            //2. Else then => Update
            #region Insert
            if (districtIsValid.isNullValueId == true)
            {
                try
                {
                    //Bỏ check trùng mã
                    //var districtCodeIsExist = _context.DistrictModel
                    //                                  .FirstOrDefault(p => p.ProvinceId == districtIsValid.ProvinceId 
                    //                                                    && p.DistrictCode == districtIsValid.DistrictCode);
                    //if (districtCodeIsExist != null)
                    //{
                    //    return string.Format(LanguageResource.Validation_Already_Exists_District, districtIsValid.DistrictCode, districtIsValid.ProvinceName);
                    //}
                    //else
                    //{

                    //}

                    DistrictModel district = new DistrictModel();
                    district.DistrictId = Guid.NewGuid();
                    district.DistrictCode = districtIsValid.DistrictCode;
                    district.DistrictName = districtIsValid.DistrictName;
                    district.Appellation = districtIsValid.Appellation;
                    district.ProvinceId = districtIsValid.ProvinceId;
                    district.OrderIndex = districtIsValid.OrderIndex;
                    district.Actived = districtIsValid.Actived;
                    _context.Entry(district).State = EntityState.Added;
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
                    DistrictModel district = _context.DistrictModel
                                                     .FirstOrDefault(p => p.DistrictId == districtIsValid.DistrictId);
                    if (district != null)
                    {
                        //Bỏ check trùng mã
                        //var districtCodeIsExist = _context.DistrictModel
                        //                              .FirstOrDefault(p => p.ProvinceId == district.ProvinceId
                        //                                                && p.DistrictCode == districtIsValid.DistrictCode);
                        //if (districtCodeIsExist != null)
                        //{
                        //    return string.Format(LanguageResource.Validation_Already_Exists_District, districtIsValid.DistrictCode, districtIsValid.ProvinceName);
                        //}
                        district.DistrictCode = districtIsValid.DistrictCode;
                        district.DistrictName = districtIsValid.DistrictName;
                        district.Appellation = districtIsValid.Appellation;
                        district.OrderIndex = districtIsValid.OrderIndex;
                        district.Actived = districtIsValid.Actived;
                        _context.Entry(district).State = EntityState.Modified;
                    }
                    else
                    {
                        return string.Format(LanguageResource.Validation_ImportExcelIdNotExist,
                                                LanguageResource.Excel_DistrictId, districtIsValid.DistrictId,
                                                string.Format(LanguageResource.Export_ExcelHeader,
                                                LanguageResource.MasterData_District));
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
        public DistrictExcelViewModel CheckTemplate(object[] row, int index)
        {
            DistrictExcelViewModel districtVM = new DistrictExcelViewModel();
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
                            districtVM.RowIndex = rowIndex;
                            break;
                        //DistrictId
                        case 1:
                            fieldName = LanguageResource.Excel_DistrictId;
                            string districtId = row[i].ToString();
                            if (string.IsNullOrEmpty(districtId))
                            {
                                districtVM.isNullValueId = true;
                            }
                            else
                            {
                                districtVM.DistrictId = Guid.Parse(districtId);
                                districtVM.isNullValueId = false;
                            }
                            break;
                        //DistrictCode
                        case 2:
                            fieldName = LanguageResource.District_DistrictCode;
                            string districtCode = row[i].ToString();
                            if (string.IsNullOrEmpty(districtCode))
                            {
                                districtVM.Error = string.Format(LanguageResource.Validation_ImportRequired, string.Format(LanguageResource.Required, LanguageResource.District_DistrictCode), districtVM.RowIndex);
                            }
                            else
                            {
                                districtVM.DistrictCode = districtCode;
                            }
                            break;
                        //Appellation: 3 (cũ)
                        case 4:
                            fieldName = LanguageResource.Appellation;
                            string appellation = row[i].ToString();
                            if (string.IsNullOrEmpty(appellation))
                            {
                                districtVM.Error = string.Format(LanguageResource.Validation_ImportRequired, string.Format(LanguageResource.Required, LanguageResource.Appellation), districtVM.RowIndex);
                            }
                            else
                            {
                                districtVM.Appellation = appellation;
                            }
                            break;
                        //DistrictName: 4 (cũ)
                        case 5:
                            fieldName = LanguageResource.District_DistrictName;
                            string districtName = row[i].ToString();
                            if (string.IsNullOrEmpty(districtName))
                            {
                                districtVM.Error = string.Format(LanguageResource.Validation_ImportRequired, string.Format(LanguageResource.Required, LanguageResource.District_DistrictName), districtVM.RowIndex);
                            }
                            else
                            {
                                districtVM.DistrictName = districtName;
                            }
                            break;
                        //ProvinceName
                        case 3:
                            fieldName = LanguageResource.MasterData_Province;
                            districtVM.ProvinceName = row[i].ToString();
                            break;
                        //ProvinceId
                        case 8:
                            fieldName = LanguageResource.MasterData_Province;
                            string provinceId = row[i].ToString();
                            //if excel type is insert
                            if (districtVM.isNullValueId == true)
                            {
                                if (string.IsNullOrEmpty(provinceId))
                                {
                                    districtVM.Error = string.Format(LanguageResource.Validation_ImportRequired, string.Format(LanguageResource.Required, LanguageResource.MasterData_Province), districtVM.RowIndex);
                                }
                                else
                                {
                                    districtVM.ProvinceId = GetTypeFunction<Guid>(provinceId, i);
                                }
                            }
                            break;
                        //OrderIndex
                        case 6:
                            fieldName = LanguageResource.OrderIndex;
                            districtVM.OrderIndex = GetTypeFunction<int>(row[i].ToString(), i);
                            break;
                        //Actived
                        case 7:
                            fieldName = LanguageResource.Actived;
                            districtVM.Actived = GetTypeFunction<bool>(row[i].ToString(), i);
                            break;
                    }
                    #endregion Convert data to import
                }
            }
            catch (FormatException ex)
            {
                districtVM.Error = string.Format(LanguageResource.Validation_ImportCastValid, fieldName, index) + ex.Message;
            }
            catch (InvalidCastException ex)
            {
                districtVM.Error = string.Format(LanguageResource.Validation_ImportCastValid, fieldName, index) + ex.Message;
            }
            catch (Exception ex)
            {
                districtVM.Error = string.Format(LanguageResource.Validate_ImportException, fieldName, index) + ex.Message;
            }
            return districtVM;
        }
        #endregion Check data type

        #endregion Import from excel

        //GET: /District/Delete
        #region Delete
        [HttpPost]
        [ISDAuthorizationAttribute]
        public ActionResult Delete(Guid id)
        {
            return ExecuteDelete(() =>
            {
                var district = _context.DistrictModel.FirstOrDefault(p => p.DistrictId == id);
                if (district != null)
                {
                    _context.Entry(district).State = EntityState.Deleted;
                    _context.SaveChanges();

                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.Created,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Delete_Success, LanguageResource.MasterData_District.ToLower())
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
        private bool IsExists(string DistrictCode, Guid? ProvinceId)
        {
            return (_context.DistrictModel.FirstOrDefault(p => p.ProvinceId == ProvinceId
                                                            && p.DistrictCode == DistrictCode) != null);
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult CheckExistingDistrictCode(string DistrictCode, string DistrictCodeValid, Guid? ProvinceId)
        {
            try
            {
                if (DistrictCodeValid != DistrictCode)
                {
                    return Json(!IsExists(DistrictCode, ProvinceId));
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

        #region Get district by province
        public ActionResult GetDistrictByProvince(Guid? ProvinceId)
        {
            DistrictRepository repo = new DistrictRepository(_context);
            var districtList = repo.GetBy(ProvinceId);
            var lst = new SelectList(districtList, "DistrictId", "DistrictName");
            return Json(lst, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetDistrictByMultiProvince(List<Guid?> ProvinceId)
        {
            DistrictRepository repo = new DistrictRepository(_context);
            var districtList = repo.GetBy(ProvinceId);
            var lst = new SelectList(districtList, "DistrictId", "DistrictName");
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        #endregion Get district by province
    }
}