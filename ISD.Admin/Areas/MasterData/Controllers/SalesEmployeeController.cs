using AutoMapper;
using AutoMapper.QueryableExtensions;
using ISD.EntityModels;
using ISD.Extensions;
using ISD.Resources;
using ISD.ViewModels;
using ISD.Constant;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ISD.ViewModels.MasterData;
using System.Transactions;
using ISD.Core;
using ISD.Repositories.Excel;

namespace MasterData.Controllers
{
    public class SalesEmployeeController : BaseController
    {
        // GET: SalesEmployee
        #region Index
        [ISDAuthorizationAttribute]
        public ActionResult Index()
        {
            //Get department
            var deparmentList = _context.DepartmentModel
                .Where(p => p.Actived == true)
                .OrderBy(p => p.OrderIndex)
                .ToList();
            var departmentNull = new DepartmentModel
            {
                DepartmentId = Guid.Empty,
                DepartmentName = LanguageResource.Department_NotSet
            };
            deparmentList.Add(departmentNull);

            ViewBag.DepartmentId = new SelectList(deparmentList, "DepartmentId", "DepartmentName");
            return View();
        }

        public ActionResult _Search(string SalesEmployeeCode = "", string SalesEmployeeName = "", Guid ? DepartmentId = null)
        {
            return ExecuteSearch(() =>
            {
                List<SalesEmployeeViewModel> laborList = new List<SalesEmployeeViewModel>();
                //not set department
                if (DepartmentId == Guid.Empty)
                {
                    laborList = (from p in _context.SalesEmployeeModel.Include(s => s.DepartmentModel)
                                 where
                                 //search by SalesEmployeeName
                                 (SalesEmployeeName == "" || p.SalesEmployeeName.Contains(SalesEmployeeName))
                                 //search by SalesEmployeeCode
                                 && (SalesEmployeeCode == "" || p.SalesEmployeeCode.Contains(SalesEmployeeCode))
                                 && p.DepartmentId == null
                                 select new SalesEmployeeViewModel()
                                 {
                                     SalesEmployeeCode = p.SalesEmployeeCode,
                                     SalesEmployeeName = p.SalesEmployeeName,
                                     SalesEmployeeShortName = p.SalesEmployeeShortName,
                                     Phone = p.Phone,
                                     Email = p.Email,
                                     Actived = p.Actived,
                                     DepartmentName = p.DepartmentModel.DepartmentName
                                 }).ToList();
                }
                else
                {   
                    // all department
                    if (DepartmentId == null)
                    {
                        laborList = (from p in _context.SalesEmployeeModel.Include(s=>s.DepartmentModel)
                                     where
                                     //search by SalesEmployeeName
                                     (SalesEmployeeName == "" || p.SalesEmployeeName.Contains(SalesEmployeeName))
                                     //search by SalesEmployeeCode
                                     && (SalesEmployeeCode == "" || p.SalesEmployeeCode.Contains(SalesEmployeeCode))
                                     select new SalesEmployeeViewModel()
                                     {
                                         SalesEmployeeCode = p.SalesEmployeeCode,
                                         SalesEmployeeName = p.SalesEmployeeName,
                                         SalesEmployeeShortName = p.SalesEmployeeShortName,
                                         Phone = p.Phone,
                                         Email = p.Email,
                                         Actived = p.Actived,
                                         DepartmentName = p.DepartmentModel.DepartmentName
                                     }).ToList();
                    }
                    else
                    {
                        //by departmentid 
                        laborList = (from p in _context.SalesEmployeeModel.Include(s => s.DepartmentModel)
                                     where
                                     //search by SalesEmployeeName
                                     (SalesEmployeeName == "" || p.SalesEmployeeName.Contains(SalesEmployeeName))
                                     //search by SalesEmployeeCode
                                     && (SalesEmployeeCode == "" || p.SalesEmployeeCode.Contains(SalesEmployeeCode))
                                     && p.DepartmentId == DepartmentId
                                     select new SalesEmployeeViewModel()
                                     {
                                         SalesEmployeeCode = p.SalesEmployeeCode,
                                         SalesEmployeeName = p.SalesEmployeeName,
                                         SalesEmployeeShortName = p.SalesEmployeeShortName,
                                         Phone = p.Phone,
                                         Email = p.Email,
                                         Actived = p.Actived,
                                         DepartmentName = p.DepartmentModel.DepartmentName
                                     }).ToList();
                    }
                }
               

                return PartialView(laborList);
            });
        }
        #endregion

        // GET: SalesEmployee/Create
        #region Create
        [ISDAuthorizationAttribute]
        public ActionResult Create()
        {
            CreateViewBag();
            return View();
        }

        [HttpPost]
        [ISDAuthorizationAttribute]
        public JsonResult Create(SalesEmployeeModel model)
        {
            return ExecuteContainer(() =>
            {
                model.CreateBy = CurrentUser.AccountId;
                model.CreateTime = DateTime.Now;
                model.SalesEmployeeName = model.SalesEmployeeName/*.FirstCharToUpper()*/;
                model.SalesEmployeeShortName = !string.IsNullOrEmpty(model.SalesEmployeeShortName) ? model.SalesEmployeeShortName : model.SalesEmployeeName;
                model.AbbreviatedName = model.SalesEmployeeName.ToAbbreviation();
                _context.Entry(model).State = EntityState.Added;
                _context.SaveChanges();
                return Json(new
                {
                    Code = HttpStatusCode.Created,
                    Success = true,
                    Data = (string.Format(LanguageResource.Alert_Create_Success, LanguageResource.MasterData_SalesEmployee.ToLower()))
                });
            });
        }
        #endregion

        // GET: SalesEmployee/Edit
        #region Edit
        [ISDAuthorizationAttribute]
        public ActionResult Edit(string id)
        {
            var employee = _context.SalesEmployeeModel.FirstOrDefault(p => p.SalesEmployeeCode == id);
            if (employee == null)
            {
                return RedirectToAction("ErrorText", "Error", new { area = "", statusCode = 404, exception = string.Format(LanguageResource.Error_NotExist, LanguageResource.MasterData_SalesEmployee.ToLower()) });
            }
            CreateViewBag(null,null,employee.DepartmentId);
            return View(employee);
        }

        [HttpPost]
        [ISDAuthorizationAttribute]
        public JsonResult Edit(SalesEmployeeModel model)
        {
            return ExecuteContainer(() =>
            {
                model.SalesEmployeeName = model.SalesEmployeeName/*.FirstCharToUpper()*/;
                model.SalesEmployeeShortName = !string.IsNullOrEmpty(model.SalesEmployeeShortName) ? model.SalesEmployeeShortName : model.SalesEmployeeName;
                model.AbbreviatedName = model.SalesEmployeeName.ToAbbreviation();
                model.LastEditBy = CurrentUser.AccountId;
                model.LastEditTime = DateTime.Now;
                //Tìm trong account nếu có user reference đến thì => update tên luôn
                var acc = _context.AccountModel.Where(p => p.EmployeeCode == model.SalesEmployeeCode).FirstOrDefault();
                if (acc != null)
                {
                    acc.FullName = model.SalesEmployeeName;
                    _context.Entry(acc).State = EntityState.Modified;
                }
                _context.Entry(model).State = EntityState.Modified;
                _context.SaveChanges();

                return Json(new
                {
                    Code = HttpStatusCode.Created,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Edit_Success, LanguageResource.MasterData_SalesEmployee.ToLower())
                });
            });
        }
        #endregion

        // GET: SalesEmployee/Delete
        #region Delete
        [HttpPost]
        [ISDAuthorizationAttribute]
        public ActionResult Delete(string id)
        {
            return ExecuteDelete(() =>
            {
                var employee = _context.SalesEmployeeModel.FirstOrDefault(p => p.SalesEmployeeCode == id);
                if (employee != null)
                {
                    _context.Entry(employee).State = EntityState.Deleted;
                    _context.SaveChanges();
                    return Json(new
                    {
                        Code = HttpStatusCode.Created,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Delete_Success, LanguageResource.MasterData_SalesEmployee.ToLower())
                    });
                }
                else
                {
                    return Json(new
                    {
                        Code = HttpStatusCode.NotModified,
                        Success = false,
                        Data = ""
                    });
                }
            });
        }
        #endregion

        const string controllerCode = ConstExcelController.SalesEmployee;
        const int startIndex = 9;
        #region Export to Excell
        public ActionResult ExportCreate()
        {
            List<SalesEmployeeExcelViewModel> viewModel = new List<SalesEmployeeExcelViewModel>();
            return Export(viewModel, isEdit: false);
        }

        public ActionResult ExportEdit(SalesEmployeeSearchViewModel searchViewModel)
        {
            searchViewModel = (SalesEmployeeSearchViewModel)Session["frmSearchSalesEmployee"];

            var config = new MapperConfiguration(cfg => { cfg.CreateMap<SalesEmployeeViewModel, SalesEmployeeExcelViewModel>(); });
            //Get data from server
            var salesEmployees = (from p in _context.SalesEmployeeModel.Include(s => s.DepartmentModel)
                                  orderby p.SalesEmployeeCode
                                  select new SalesEmployeeViewModel
                                  {
                                      SalesEmployeeCode = p.SalesEmployeeCode,
                                      SalesEmployeeName = p.SalesEmployeeName,
                                      Actived = p.Actived,
                                      DepartmentCode = p.DepartmentModel.DepartmentCode
                                  }).ToList();
            List<SalesEmployeeExcelViewModel> salesEmployee = new List<SalesEmployeeExcelViewModel>();
            foreach (var e in salesEmployees)
            {
                salesEmployee.Add(new SalesEmployeeExcelViewModel
                {
                    SalesEmployeeCode = e.SalesEmployeeCode,
                    DepartmentCode = e.DepartmentCode,
                    SalesEmployeeName = e.SalesEmployeeName,
                    Actived = e.Actived
                });
            }
            //var salesEmployee = (from p in _context.SalesEmployeeModel.Include(s => s.DepartmentModel)
                                 //orderby p.SalesEmployeeCode
                                 //select new SalesEmployeeViewModel {
                                 //    SalesEmployeeCode = p.SalesEmployeeCode,
                                 //    SalesEmployeeName = p.SalesEmployeeName,
                                 //    Actived = p.Actived,
                                 //    DepartmentName = p.DepartmentModel.DepartmentName
                                 //})
                                 //.ProjectTo<SalesEmployeeExcelViewModel>(config).ToList();

            return Export(salesEmployee, isEdit: true);
        }

        
        [ISDAuthorizationAttribute]
        public FileContentResult Export(List<SalesEmployeeExcelViewModel> viewModel, bool isEdit)
        {
            #region Master
            //Tạo mẫu
            List<ExcelTemplate> columns = new List<ExcelTemplate>();

            
            if (isEdit == true)
            {
                columns.Add(new ExcelTemplate() { ColumnName = "SalesEmployeeCode", isAllowedToEdit = false });
            }
            else
            {
                columns.Add(new ExcelTemplate() { ColumnName = "SalesEmployeeCode", isAllowedToEdit = true,});
            }
            columns.Add(new ExcelTemplate() { ColumnName = "SalesEmployeeName", isAllowedToEdit = true });
            columns.Add(new ExcelTemplate() { ColumnName = "DepartmentCode", isAllowedToEdit = true });
            columns.Add(new ExcelTemplate() { ColumnName = "Actived", isAllowedToEdit = true, isBoolean = true });

            #endregion

            //Header
            string fileHeader = string.Format(LanguageResource.Export_ExcelHeader, LanguageResource.MasterData_SalesEmployee);

            heading.Add(new ExcelHeadingTemplate()
            {
                Content = controllerCode,
                RowsToIgnore = 1,
                isWarning = false,
                isCode = true,
            });
            heading.Add(new ExcelHeadingTemplate()
            {
                Content = fileHeader.ToUpper(),
                RowsToIgnore = 1,
                isWarning = false,
                isCode = false,
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
                Content = string.Format(LanguageResource.Export_ExcelWarningActived, LanguageResource.MasterData_SalesEmployee),
                RowsToIgnore = 1,
                isWarning = true,
                isCode = false
            });

            //Nội dung file
            byte[] fileContent = ClassExportExcel.ExportExcel(viewModel, columns, heading, true);

            //Insert or Edit
            string exportType = LanguageResource.exportType_Insert;
            if(isEdit == true)
            {
                exportType = LanguageResource.exportType_Edit;
            }
            //Ten file sau khi export
            string fileNameWithFormat = string.Format("{0}_{1}.xlsx", exportType, _unitOfWork.UtilitiesRepository.RemoveSign4VietnameseString(fileHeader.ToUpper().Replace(" ", "_")));

            return File(fileContent, ClassExportExcel.ExcelContentType, fileNameWithFormat);

        }
        #endregion

        #region Import from excel

        [ISDAuthorizationAttribute]
        public ActionResult Import()
        {
            return ExcuteImportExcel(() =>
            {
                DataSet ds = GetDataSetFromExcelNew();
                List<string> errorList = new List<string>();
                if (ds.Tables != null && ds.Tables.Count > 0)
                {

                    using (TransactionScope ts = new TransactionScope())
                    {
                        DataTable dt = ds.Tables[0];
                        //foreach (DataTable dt in ds.Tables)
                        //{
                        //Get controller code from Excel file
                        string contCode = dt.Rows[0][0].ToString();
                       // string contCode = dt.Columns[0].ColumnName.ToString();
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
                                        SalesEmployeeExcelViewModel salesEmployeeIsValid = CheckTemplate(dr.ItemArray, index);

                                        if (!string.IsNullOrEmpty(salesEmployeeIsValid.Error))
                                        {
                                            string error = salesEmployeeIsValid.Error;
                                            errorList.Add(error);
                                        }
                                        else
                                        {
                                            string result = ExecuteImportExcelSalesEmployee(salesEmployeeIsValid);
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
                                string error = string.Format(LanguageResource.Validation_ImportCheckController, LanguageResource.MasterData_SalesEmployee);
                                errorList.Add(error);
                            }

                       // }
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
        public string ExecuteImportExcelSalesEmployee(SalesEmployeeExcelViewModel salesEmployeeIsValid)
        {
            //Get employee in Db by employeeCode
            SalesEmployeeModel salesEmployee = _context.SalesEmployeeModel
                                                     .FirstOrDefault(p => p.SalesEmployeeCode == salesEmployeeIsValid.SalesEmployeeCode);

            //Check:
            //1. If employee == null then => Insert
            //2. Else then => Update
            #region Insert
            if (salesEmployee == null)
            {
                try
                {
                    SalesEmployeeModel salesEmployeeNew = new SalesEmployeeModel();
                    salesEmployeeNew.SalesEmployeeCode = salesEmployeeIsValid.SalesEmployeeCode;
                    salesEmployeeNew.SalesEmployeeName = salesEmployeeIsValid.SalesEmployeeName;
                    salesEmployeeNew.DepartmentId = salesEmployeeIsValid.DepartmentId;
                    salesEmployeeNew.Actived = salesEmployeeIsValid.Actived;
                    _context.Entry(salesEmployeeNew).State = EntityState.Added;
                    
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
                        salesEmployee.SalesEmployeeName = salesEmployeeIsValid.SalesEmployeeName;
                        salesEmployee.DepartmentId = salesEmployeeIsValid.DepartmentId;
                        salesEmployee.Actived = salesEmployeeIsValid.Actived;
                        _context.Entry(salesEmployee).State = EntityState.Modified;

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
        public SalesEmployeeExcelViewModel CheckTemplate(object[] row, int index)
        {
            SalesEmployeeExcelViewModel salesEmployeeVM = new SalesEmployeeExcelViewModel();
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
                            salesEmployeeVM.RowIndex = rowIndex;
                            break;
                        //SalesEmployee_Code
                        case 1:
                            fieldName = LanguageResource.SalesEmployee_Code;
                            string salesEmployeeCode = row[i].ToString();
                            if (string.IsNullOrEmpty(salesEmployeeCode))
                            {
                                salesEmployeeVM.Error = string.Format(LanguageResource.Validation_ImportRequired, string.Format(LanguageResource.Required, LanguageResource.SalesEmployee_Code), salesEmployeeVM.RowIndex);
                            }
                            else
                            {
                                salesEmployeeVM.SalesEmployeeCode = salesEmployeeCode;
                            }
                            break;
                        //SalesEmployee_Name
                        case 2:
                            fieldName = LanguageResource.SalesEmployee_Name;
                            string salesEmployeeName = row[i].ToString();
                            if (string.IsNullOrEmpty(salesEmployeeName))
                            {
                                salesEmployeeVM.Error = string.Format(LanguageResource.Validation_ImportRequired, string.Format(LanguageResource.Required, LanguageResource.SalesEmployee_Name), salesEmployeeVM.RowIndex);
                            }
                            else
                            {
                                salesEmployeeVM.SalesEmployeeName = salesEmployeeName;
                            }
                            break;
                        
                        case 3:
                            fieldName = LanguageResource.Master_Department;
                            string department = row[i].ToString();
                            if (string.IsNullOrEmpty(department))
                            {
                                salesEmployeeVM.DepartmentId = null;
                            }
                            else
                            {
                                salesEmployeeVM.DepartmentId = _context.DepartmentModel.FirstOrDefault(d => d.DepartmentCode == department).DepartmentId;
                            }
                            break;
                        //Actived
                        case 4:
                            fieldName = LanguageResource.Actived;
                            salesEmployeeVM.Actived = GetTypeFunction<bool>(row[i].ToString(), i);
                            break;
                    }
                    #endregion Convert data to import
                }
            }
            catch (FormatException ex)
            {
                var Message = ex.Message;
                salesEmployeeVM.Error = string.Format(LanguageResource.Validation_ImportCastValid, fieldName, index);
            }
            catch (InvalidCastException ex)
            {
                var Message = ex.Message;
                salesEmployeeVM.Error = string.Format(LanguageResource.Validation_ImportCastValid, fieldName, index);
            }
            catch (Exception ex)
            {
                var Message = ex.Message;
                salesEmployeeVM.Error = string.Format(LanguageResource.Validate_ImportException, fieldName, index);
            }
            return salesEmployeeVM;
        }
        #endregion
        #endregion

        #region Remote Validation

        private bool IsExists(string SalesEmployeeCode)
        {
            return (_context.SalesEmployeeModel.FirstOrDefault(p => p.SalesEmployeeCode == SalesEmployeeCode) != null);
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult CheckExistingSalesEmployeeCode(string SalesEmployeeCode, string SalesEmployeeCodeValid)
        {
            try
            {
                if(SalesEmployeeCodeValid != SalesEmployeeCode)
                {
                    return Json(!IsExists(SalesEmployeeCode));
                }
                else
                {
                    return Json(true);
                }
            }
            catch//(Exception ex)
            {
                return Json(false);
            }
        }
        #endregion

        public void CreateViewBag(Guid? CompanyId = null, Guid? StoreId = null, Guid? DepartmentId = null)
        {
            //Get list Company
            var compList = _context.CompanyModel.Where(p => p.Actived == true).OrderBy(p => p.OrderIndex).ToList();
            ViewBag.CompanyId = new SelectList(compList, "CompanyId", "CompanyName", CompanyId);

            //Get Store
            var storeList = _context.StoreModel.Where(p => p.Actived == true).OrderBy(p => p.OrderIndex).ToList();
            ViewBag.StoreId = new SelectList(storeList, "StoreId", "StoreName", StoreId);

            //Get department
            var deparmentList = _context.DepartmentModel.Include(p => p.StoreModel).Where(p => p.Actived == true).OrderBy(p => p.OrderIndex).ToList();

            ViewBag.DepartmentId = new SelectList(deparmentList, "DepartmentId", "DepartmentName", DepartmentId);
        }
        //create department by store id
        public ActionResult CreateDepartmentByStore(Guid? StoreId)
        {
            var departmentList = _context.DepartmentModel.Where(p => p.Actived == true && p.StoreId == StoreId)
                                                            .OrderBy(p => p.OrderIndex).ToList();
            var selectList = new SelectList(departmentList, "DepartmentId", "DepartmentName");
            return Json(selectList, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetEmployeeByDepartment(Guid? DepartmentId)
        {
            if (DepartmentId != null && DepartmentId != Guid.Empty)
            {
                var listEmployee = (from p in _context.SalesEmployeeModel
                                    join a in _context.AccountModel on p.SalesEmployeeCode equals a.EmployeeCode
                                    where p.Actived == true && p.DepartmentId==DepartmentId
                                    orderby p.SalesEmployeeCode
                                    select new SalesEmployeeViewModel
                                    {
                                        SalesEmployeeCode = p.SalesEmployeeCode,
                                        SalesEmployeeName = p.SalesEmployeeCode + " | " + p.SalesEmployeeName,
                                        RolesName = a.RolesModel.Where(m => m.isEmployeeGroup == true).Select(p => p.RolesName).FirstOrDefault(),
                                    }).ToList();
                var slEmployee = new SelectList(listEmployee, "SalesEmployeeCode", "SalesEmployeeName");
                return Json(slEmployee, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var listEmployee = _unitOfWork.SalesEmployeeRepository.GetAllForDropdownlist(); ;
                var slEmployee = new SelectList(listEmployee, "SalesEmployeeCode", "SalesEmployeeName");
                return Json(slEmployee, JsonRequestBehavior.AllowGet);
            }

        }
    }
}