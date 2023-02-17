using ISD.EntityModels;
using ISD.Extensions;
using ISD.Resources;
using ISD.ViewModels;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using ISD.Core;
using System.Data;
using System.Collections.Generic;
using System.Transactions;
using ISD.Constant;
using ISD.ViewModels.MasterData;

namespace MasterData.Controllers
{
    public class DepartmentController : BaseController
    {
        // GET: Department
        #region Index
        [ISDAuthorizationAttribute]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult _Search(string DepartmentCode = "", string DepartmentName = "", bool? Actived = null)
        {
            return ExecuteSearch(() =>
            {
                var departmentList = (from d in _context.DepartmentModel
                                      orderby d.OrderIndex
                                      where (DepartmentCode == "" || d.DepartmentCode.Contains(DepartmentCode))
                                      && (DepartmentName == "" || d.DepartmentName.Contains(DepartmentName))
                                      && (Actived == null || d.Actived == Actived)
                                      select new DepartmentViewModel
                                      {
                                          DepartmentId = d.DepartmentId,
                                          StoreId = d.StoreId,
                                          DepartmentCode = d.DepartmentCode,
                                          DepartmentName = d.DepartmentName,
                                          OrderIndex = d.OrderIndex,
                                          Actived = d.Actived,
                                      }).ToList();

                return PartialView(departmentList);
            });
        }
        #endregion
        public JsonResult GetDepartmentByCompany(Guid? CompanyId)
        {
            if(CompanyId!=null && CompanyId!=Guid.Empty)
            {
                var listDepartment = _context.DepartmentModel.Where(s => s.Actived == true && s.CompanyId == CompanyId);
                var slDepartment = new SelectList(listDepartment, "DepartmentId", "DepartmentName");
                return Json(slDepartment, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var listDepartment = _context.DepartmentModel.Where(s => s.Actived == true);
                var slDepartment = new SelectList(listDepartment, "DepartmentId", "DepartmentName");
                return Json(slDepartment, JsonRequestBehavior.AllowGet);
            }
            
        }
        //GET: /Department/Create
        #region Create
        [ISDAuthorizationAttribute]
        public ActionResult Create()
        {
            CreateViewBag();
            return View();
        }
        //POST: Create
        [HttpPost]
        [ValidateAjax]
        [ISDAuthorizationAttribute]
        public JsonResult Create(DepartmentViewModel departmentVM)
        {
            return ExecuteContainer(() =>
            {

                var modelNew = new DepartmentModel
                {
                    DepartmentId = Guid.NewGuid(),
                    CompanyId = departmentVM.CompanyId,
                    StoreId = departmentVM.StoreId,
                    DepartmentCode = departmentVM.DepartmentCode,
                    DepartmentName = departmentVM.DepartmentName,
                    OrderIndex = departmentVM.OrderIndex,
                    CreateBy = CurrentUser.AccountId,
                    CreateTime = DateTime.Now,
                    Actived = true
                };

                _context.Entry(modelNew).State = EntityState.Added;
                _context.SaveChanges();

                return Json(new
                {
                    Code = System.Net.HttpStatusCode.Created,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Create_Success, LanguageResource.MasterData_Store.ToLower())
                });

            });
        }
        #endregion

        //GET: /Department/Edit
        #region Edit
        [ISDAuthorizationAttribute]
        public ActionResult Edit(Guid id)
        {
            var departmentInDb = _context.DepartmentModel.FirstOrDefault(p => p.DepartmentId == id && p.Actived == true);
            if (departmentInDb == null)
            {
                return RedirectToAction("ErrorText", "Error", new { area = "", statusCode = 404, exception = string.Format(LanguageResource.Error_NotExist, LanguageResource.Master_Department.ToLower()) });
            }
            var departmentVM = new DepartmentViewModel
            {
                DepartmentId = departmentInDb.DepartmentId,
                CompanyId = departmentInDb.CompanyId,
                StoreId = departmentInDb.StoreId,
                DepartmentCode = departmentInDb.DepartmentCode,
                DepartmentName = departmentInDb.DepartmentName,
                OrderIndex = departmentInDb.OrderIndex,
                Actived = departmentInDb.Actived
            };
            
            CreateViewBag(departmentVM.StoreId, departmentVM.CompanyId);
            return View(departmentVM);
        }
        //POST: Edit
        [HttpPost]
        [ISDAuthorizationAttribute]
        public JsonResult Edit(DepartmentViewModel departmentVM)
        {
            return ExecuteContainer(() =>
            {
                var modelDb = _context.DepartmentModel.FirstOrDefault(p => p.DepartmentId == departmentVM.DepartmentId);
                if (modelDb != null)
                {
                    modelDb.CompanyId = departmentVM.CompanyId;
                    modelDb.StoreId = departmentVM.StoreId;
                    modelDb.DepartmentCode = departmentVM.DepartmentCode;
                    modelDb.DepartmentName = departmentVM.DepartmentName;
                    modelDb.OrderIndex = departmentVM.OrderIndex;
                    modelDb.LastEditBy = CurrentUser.AccountId;
                    modelDb.LastEditTime = DateTime.Now;
                }
                _context.Entry(modelDb).State = EntityState.Modified;
                _context.SaveChanges();

                return Json(new
                {
                    Code = System.Net.HttpStatusCode.Created,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Edit_Success, LanguageResource.Master_Department.ToLower())
                });
            });
        }
        #endregion

        //GET: /Department/Delete
        #region Delete
        [HttpPost]
        [ISDAuthorizationAttribute]
        public ActionResult Delete(Guid id)
        {
            return ExecuteDelete(() =>
            {
                var department = _context.DepartmentModel.FirstOrDefault(p => p.DepartmentId == id);
                if (department != null)
                {
                    _context.Entry(department).State = EntityState.Deleted;
                    _context.SaveChanges();

                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.Created,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Delete_Success, LanguageResource.Master_Department.ToLower())
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

        #region ViewBag, Helper
        public void CreateViewBag(Guid? StoreId = null, Guid? CompanyId = null)
        {
            //Cong ty
            var companyList = _context.CompanyModel.Where(p => p.Actived).OrderBy(p => p.OrderIndex).ToList();

            ViewBag.CompanyId = new SelectList(companyList, "CompanyId", "CompanyName", CompanyId);
            //Chi nhanh
            var storeList = _context.StoreModel.Where(p => p.Actived && (CompanyId != null && p.CompanyId == CompanyId)).OrderBy(p => p.OrderIndex).ToList();
            ViewBag.StoreId = new SelectList(storeList, "StoreId", "StoreName", StoreId);
        }
        //Get company by companyId
        public ActionResult GetStoreByCompany(Guid? CompanyId)
        {
            var storeList = _context.StoreModel.Where(p => p.Actived && p.CompanyId == CompanyId)
                                                            .Select(p => new StoreViewModel()
                                                            {
                                                                StoreId = p.StoreId,
                                                                StoreName = p.StoreName
                                                            }).ToList();
            var selectList = new SelectList(storeList, "StoreId", "StoreName");
            return Json(selectList, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Remote Validation
        private bool IsExists(string DepartmentCode)
        {
            return (_context.DepartmentModel.FirstOrDefault(p => p.DepartmentCode == DepartmentCode) != null);
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult CheckExistingDepartmentCode(string DepartmentCode,  string DepartmentCodeValid)
        {
            try
            {
                if (DepartmentCodeValid != DepartmentCode)
                {
                    return Json(!IsExists(DepartmentCode));
                }
                else
                {
                    return Json(true);
                }
            }
            catch// (Exception ex)
            {
                return Json(false);
            }
        }
        #endregion
        const string controllerCode = ConstExcelController.Department;
        const int startIndex = 9;
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
                          //  string contCode = dt.Columns[0].ColumnName.ToString()
                           string contCode = dt.Rows[0][0].ToString();
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
                                        DepartmentExcelViewModel departmentValid = CheckTemplate(dr.ItemArray, index);

                                        if (!string.IsNullOrEmpty(departmentValid.Error))
                                        {
                                            string error = departmentValid.Error;
                                            errorList.Add(error);
                                        }
                                        else
                                        {
                                            string result = ExecuteImportExcelSalesEmployee(departmentValid);
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
        public string ExecuteImportExcelSalesEmployee(DepartmentExcelViewModel departmentValid)
        {
            //Get employee in Db by employeeCode
            DepartmentModel depatmentExist = _context.DepartmentModel
                                                     .FirstOrDefault(p => p.DepartmentCode == departmentValid.DepartmentCode);
           
            //Check:
            //1. If employee == null then => Insert
            //2. Else then => Update
            #region Insert
            if (depatmentExist == null)
            {
                try
                {
                    DepartmentModel deparment = new DepartmentModel();
                    deparment.DepartmentId = Guid.NewGuid();
                    deparment.DepartmentCode = departmentValid.DepartmentCode;
                    deparment.DepartmentName = departmentValid.DepartmentName;                 
                    deparment.Actived = departmentValid.Actived;
                    _context.Entry(deparment).State = EntityState.Added;

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
                    depatmentExist.DepartmentName = departmentValid.DepartmentName;
                    depatmentExist.DepartmentCode = departmentValid.DepartmentCode;                    
                    depatmentExist.Actived = departmentValid.Actived;

                    _context.Entry(depatmentExist).State = EntityState.Modified;

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
      
        public DepartmentExcelViewModel CheckTemplate(object[] row, int index)
        {
            DepartmentExcelViewModel departmentViewModel = new DepartmentExcelViewModel();
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
                            departmentViewModel.RowIndex = rowIndex;
                            break;
                        //SalesEmployee_Code
                        case 1:
                            fieldName = LanguageResource.Department_DepartmentCode;
                            string departmentCode = row[i].ToString();
                            if (string.IsNullOrEmpty(departmentCode))
                            {
                                departmentViewModel.Error = string.Format(LanguageResource.Validation_ImportRequired, string.Format(LanguageResource.Required, LanguageResource.Department_DepartmentCode), departmentViewModel.RowIndex);
                            }
                            else
                            {
                                departmentViewModel.DepartmentCode = departmentCode;
                            }
                            break;
                        //SalesEmployee_Name
                        case 2:
                            fieldName = LanguageResource.Department_DepartmentName;
                            string departmentName = row[i].ToString();
                            if (string.IsNullOrEmpty(departmentName))
                            {
                                departmentViewModel.Error = string.Format(LanguageResource.Validation_ImportRequired, string.Format(LanguageResource.Required, LanguageResource.Department_DepartmentName), departmentViewModel.RowIndex);
                            }
                            else
                            {
                                departmentViewModel.DepartmentName = departmentName;
                            }
                            break;
                        //SalesEmployee_Name
                        case 3:
                            fieldName = LanguageResource.Company_CompanyCode;
                            string companyCode = row[i].ToString();
                            if (string.IsNullOrEmpty(companyCode))
                            {
                                departmentViewModel.CompanyId = null;
                            }
                            else
                            {
                                departmentViewModel.CompanyId = _context.CompanyModel.FirstOrDefault(d => d.CompanyCode == companyCode).CompanyId;
                            }
                            break;
                        case 4:
                            fieldName = LanguageResource.Store_StoreCode;
                            string storeCode = row[i].ToString();
                            if (string.IsNullOrEmpty(storeCode))
                            {
                                departmentViewModel.StoreId = null;
                            }
                            else
                            {
                                departmentViewModel.StoreId = _context.StoreModel.FirstOrDefault(d => d.SaleOrgCode == storeCode).StoreId;
                            }
                            break;
                        //Actived
                        case 5:
                            fieldName = LanguageResource.Actived;
                            departmentViewModel.Actived = GetTypeFunction<bool>(row[i].ToString(), i);
                            break;
                    }
                    #endregion Convert data to import
                }
            }
            catch (FormatException ex)
            {
                var Message = ex.Message;
                departmentViewModel.Error = string.Format(LanguageResource.Validation_ImportCastValid, fieldName, index);
            }
            catch (InvalidCastException ex)
            {
                var Message = ex.Message;
                departmentViewModel.Error = string.Format(LanguageResource.Validation_ImportCastValid, fieldName, index);
            }
            catch (Exception ex)
            {
                var Message = ex.Message;
                departmentViewModel.Error = string.Format(LanguageResource.Validate_ImportException, fieldName, index);
            }
            return departmentViewModel;
        }

    }
}
