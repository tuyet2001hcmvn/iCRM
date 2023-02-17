using ISD.Constant;
using ISD.EntityModels;
using ISD.Extensions;
using ISD.Resources;
using ISD.ViewModels;
using ISD.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Transactions;
using System.Web.Mvc;
using ISD.Repositories.Excel;

namespace Permission.Controllers
{
    public class RolesController : BaseController
    {
        //Config
        bool isShowConfigLoginByTime = false;

        const string controllerCode = ConstExcelController.Roles;
        const int startIndex = 6;

        //GET: /Roles/Index
        #region Index + Search
        [ISDAuthorizationAttribute]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult _Search(string RolesName)
        {
            return ExecuteSearch(() =>
            {
                var RolesNameIsNullOrEmpty = string.IsNullOrEmpty(RolesName);
                var roleList = _context.RolesModel.Where(p => (RolesNameIsNullOrEmpty || p.RolesName.ToLower().Contains(RolesName.ToLower()))
                                                              && p.Actived == true)
                                                  .OrderBy(p => p.OrderIndex).ToList();

                return PartialView(RolesInCurrentAccount(roleList));
            });
        }
        #endregion

        //GET: /Roles/Create
        #region Create
        [ISDAuthorizationAttribute]
        public ActionResult Create()
        {
            RolesModel model = new RolesModel();
            model.WorkingTimeFrom = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            model.WorkingTimeTo = model.WorkingTimeFrom.Value.AddMonths(1).AddDays(-1);
            CreateViewBag();
            return View(model);
        }
        [HttpPost]
        [ValidateAjax]
        public JsonResult Create(RolesModel model, int? FromHour, int? FromMinute, int? ToHour, int? ToMinute)
        {
            return ExecuteContainer(() =>
            {
                model.RolesId = Guid.NewGuid();
                model.RolesCode = model.RolesCode.ToUpper();
                if (isShowConfigLoginByTime)
                {
                    //WorkingTimeFrom
                    DateTime fromTime = new DateTime(2019, 01, 01);
                    fromTime = fromTime.AddHours(FromHour.Value).AddMinutes(FromMinute.Value);
                    model.WorkingTimeFrom = fromTime;
                    //WorkingTimeTo
                    DateTime toTime = new DateTime(2019, 01, 01);
                    toTime = toTime.AddHours(ToHour.Value).AddMinutes(ToMinute.Value);
                    model.WorkingTimeTo = toTime;
                }

                _context.Entry(model).State = EntityState.Added;
                _context.SaveChanges();
                return Json(new
                {
                    Code = System.Net.HttpStatusCode.Created,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Create_Success, LanguageResource.Permission_RolesModel.ToLower())
                });
            });
        }
        #endregion

        //GET: /Roles/Edit
        #region Edit
        [ISDAuthorizationAttribute]
        public ActionResult Edit(Guid id)
        {
            var role = _context.RolesModel.FirstOrDefault(p => p.RolesId == id);
            if (role == null)
            {
                return HttpNotFound();
            }
            else
            {
                if (!(role.WorkingTimeFrom.HasValue && role.WorkingTimeTo.HasValue))
                {
                    role.WorkingTimeFrom = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                    role.WorkingTimeTo = role.WorkingTimeFrom.Value.AddMonths(1).AddDays(-1);
                }

            }
            CreateViewBag(role.WorkingTimeFrom.Value.Hour, role.WorkingTimeFrom.Value.Minute, role.WorkingTimeTo.Value.Hour, role.WorkingTimeTo.Value.Minute, role.isEmployeeGroup);
            return View(role);
        }
        [HttpPost]
        [ValidateAjax]
        [ISDAuthorizationAttribute]
        public JsonResult Edit(RolesModel model, int? FromHour, int? FromMinute, int? ToHour, int? ToMinute)
        {
            return ExecuteContainer(() =>
            {
                //Nếu không phân quyền đăng nhập thì set WorkingTimeFrom và WorkingTimeTo = null
                if (isShowConfigLoginByTime)
                {
                    if (model.isCheckLoginByTime != true)
                    {
                        model.WorkingTimeFrom = null;
                        model.WorkingTimeTo = null;
                    }
                    else
                    {
                        DateTime fromTime = new DateTime(2019, 01, 01);
                        fromTime = fromTime.AddHours(FromHour.Value).AddMinutes(FromMinute.Value);
                        model.WorkingTimeFrom = fromTime;
                        DateTime toTime = new DateTime(2019, 01, 01);
                        toTime = toTime.AddHours(ToHour.Value).AddMinutes(ToMinute.Value);
                        model.WorkingTimeTo = toTime;
                    }
                }
                _context.Entry(model).State = EntityState.Modified;
                _context.SaveChanges();

                return Json(new
                {
                    Code = System.Net.HttpStatusCode.Created,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Edit_Success, LanguageResource.Permission_RolesModel.ToLower())
                });
            });
        }

        public ActionResult _ListEmployeeOfRole(Guid id)
        {
            var listResult = (from p in _context.AccountModel
                              where p.RolesModel.Any(r => r.RolesId == id)
                              select new AccountResultViewModel
                              {
                                  AccountId = p.AccountId,
                                  EmployeeCode = p.EmployeeCode,
                                  UserName = p.UserName,
                                  FullName = p.FullName,
                                  Actived = p.Actived,
                              }).ToList();

            return PartialView(listResult);
        }
        #endregion

        //GET: /Roles/Delete
        #region Delete
        [HttpPost]
        [ISDAuthorizationAttribute]
        public ActionResult Delete(Guid id)
        {
            return ExecuteDelete(() =>
            {
                var role = _context.RolesModel.FirstOrDefault(p => p.RolesId == id);
                if (role != null)
                {
                    if (role.AccountModel != null)
                    {
                        role.AccountModel.Clear();
                    }
                    _context.Entry(role).State = EntityState.Deleted;

                    //Delete in PagePermission
                    var pagePermission = _context.PagePermissionModel.Where(p => p.RolesId == id).ToList();
                    if (pagePermission != null && pagePermission.Count > 0)
                    {
                        _context.PagePermissionModel.RemoveRange(pagePermission);
                    }

                    _context.SaveChanges();

                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.Created,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Delete_Success, LanguageResource.Permission_RolesModel.ToLower())
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
        private bool IsExists(string RolesCode)
        {
            return (_context.RolesModel.FirstOrDefault(p => p.RolesCode == RolesCode) != null);
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult CheckExistingRolesCode(string RolesCode, string RolesCodeValid)
        {
            try
            {
                if (RolesCodeValid != RolesCode)
                {
                    return Json(!IsExists(RolesCode));
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

        #region Helper
        //Get roles by current account
        public List<RolesModel> RolesInCurrentAccount(List<RolesModel> roleList)
        {
            var accountId = CurrentUser.AccountId;
            var accountFilter = _context.AccountModel.Where(p => p.AccountId == accountId).FirstOrDefault();
            if (accountFilter.RolesModel != null && accountFilter.RolesModel.Count > 0)
            {
                var filterRoles = roleList.Where(p => p.OrderIndex >= accountFilter.RolesModel.Min(e => e.OrderIndex)).OrderBy(p => p.OrderIndex).ToList();
                roleList = filterRoles;
            }
            return roleList;
        }

        private void CreateViewBag(int? FromHour = null, int? FromMinute = null, int? ToHour = null, int? ToMinute = null, bool? isEmployeeGroup = null)
        {
            ViewBag.IsShowConfigLoginByTime = isShowConfigLoginByTime;
            //Giờ bắt đầu
            List<ISDSelectIntItem> fromHourList = new List<ISDSelectIntItem>();
            //Giờ kết thúc
            List<ISDSelectIntItem> toHourList = new List<ISDSelectIntItem>();
            for (int i = 0; i < 24; i++)
            {
                fromHourList.Add(new ISDSelectIntItem()
                {
                    id = i,
                    name = i < 10 ? string.Format("0{0}", i) : i.ToString()
                });
                toHourList.Add(new ISDSelectIntItem()
                {
                    id = i,
                    name = i < 10 ? string.Format("0{0}", i) : i.ToString()
                });
            }

            //Giờ bắt đầu
            List<ISDSelectIntItem> fromMinuteList = new List<ISDSelectIntItem>();
            //Giờ kết thúc
            List<ISDSelectIntItem> toMinuteList = new List<ISDSelectIntItem>();
            for (int i = 0; i < 60; i++)
            {
                fromMinuteList.Add(new ISDSelectIntItem()
                {
                    id = i,
                    name = i < 10 ? string.Format("0{0}", i) : i.ToString()
                });
                toMinuteList.Add(new ISDSelectIntItem()
                {
                    id = i,
                    name = i < 10 ? string.Format("0{0}", i) : i.ToString()
                });
            }

            ViewBag.FromHour = new SelectList(fromHourList, "id", "name", FromHour);
            ViewBag.ToHour = new SelectList(toHourList, "id", "name", ToHour);
            ViewBag.FromMinute = new SelectList(fromMinuteList, "id", "name", FromMinute);
            ViewBag.ToMinute = new SelectList(toMinuteList, "id", "name", ToMinute);


            #region //isEmployeeGroup (Nhóm nhân viên)
            var isEmployeeGroupList = new List<SelectListItem>() {
                new SelectListItem() {
                     Text = LanguageResource.Actions,
                     Value = false.ToString()
                },
                new SelectListItem() {
                     Text = LanguageResource.DepartmentId,
                     Value = true.ToString()
                }
            };
            ViewBag.isEmployeeGroup = new SelectList(isEmployeeGroupList, "Value", "Text", isEmployeeGroup);
            #endregion
        }

        #endregion Helper

        //GET: /Roles/ExportToExcel
        #region Export to excel
        public ActionResult ExportCreate()
        {
            List<RolesModel> roles = new List<RolesModel>();
            return Export(roles);
        }

        public ActionResult ExportEdit()
        {
            //Get data from server
            List<RolesModel> roles = _context.RolesModel.OrderBy(p => p.OrderIndex).ToList();
            return Export(RolesInCurrentAccount(roles));
        }

        public FileContentResult Export(List<RolesModel> roles)
        {
            //Columns to take
            List<ExcelTemplate> columns = new List<ExcelTemplate>();
            columns.Add(new ExcelTemplate() { ColumnName = "RolesId", isAllowedToEdit = false });
            columns.Add(new ExcelTemplate() { ColumnName = "RolesCode", isAllowedToEdit = true });
            columns.Add(new ExcelTemplate() { ColumnName = "RolesName", isAllowedToEdit = true });
            columns.Add(new ExcelTemplate() { ColumnName = "OrderIndex", isAllowedToEdit = true });
            columns.Add(new ExcelTemplate() { ColumnName = "Actived", isAllowedToEdit = true });
            //Header
            string fileheader = string.Format(LanguageResource.Export_ExcelHeader, LanguageResource.Permission_RolesModel);

            //List<ExcelHeadingTemplate> heading initialize in BaseController
            CreateExportHeader(fileheader, controllerCode);

            //Body
            byte[] filecontent = ClassExportExcel.ExportExcel(roles, columns, heading, true);
            //File name
            string fileNameWithFormat = string.Format("{0}.xlsx", _unitOfWork.UtilitiesRepository.RemoveSign4VietnameseString(fileheader.ToUpper()).Replace(" ", "_"));

            return File(filecontent, ClassExportExcel.ExcelContentType, fileNameWithFormat);
        }

        #endregion Export to excel

        //POST: /Roles/ImportFromExcel
        #region Import from excel
        public ActionResult Import()
        {
            DataSet ds = GetDataSetFromExcel();
            List<string> errorList = new List<string>();
            return ExcuteImportExcel(() =>
            {
                if (ds.Tables.Count > 0)
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
                                foreach (DataRow dr in dt.Rows)
                                {
                                    if (dt.Rows.IndexOf(dr) >= startIndex && !string.IsNullOrEmpty(dr.ItemArray[0].ToString()))
                                    {
                                        //Check correct template
                                        RolesViewModel rolesIsValid = CheckTemplate(dr.ItemArray);

                                        if (!string.IsNullOrEmpty(rolesIsValid.Error))
                                        {
                                            string error = rolesIsValid.Error;
                                            errorList.Add(error);
                                        }
                                        else
                                        {
                                            string result = ExecuteImportExcelRoles(rolesIsValid);
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
                                string error = string.Format(LanguageResource.Validation_ImportCheckController, LanguageResource.Permission_RolesModel);
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
        public string ExecuteImportExcelRoles(RolesViewModel rolesIsValid)
        {
            //Check:
            //1. If RolesId == "" then => Insert
            //2. Else then => Update
            if (rolesIsValid.isNullValueId == true)
            {
                RolesModel roles = new RolesModel();
                roles.RolesId = Guid.NewGuid();
                roles.RolesCode = rolesIsValid.RolesCode.ToUpper();
                roles.RolesName = rolesIsValid.RolesName;
                roles.OrderIndex = rolesIsValid.OrderIndex;
                roles.Actived = rolesIsValid.Actived;

                _context.Entry(roles).State = EntityState.Added;
            }
            else
            {
                RolesModel roles = _context.RolesModel.Where(p => p.RolesId == rolesIsValid.RolesId).FirstOrDefault();
                if (roles != null)
                {
                    if (roles.RolesCode != rolesIsValid.RolesCode)
                    {
                        roles.RolesCode = rolesIsValid.RolesCode;
                    }
                    if (roles.RolesName != rolesIsValid.RolesName)
                    {
                        roles.RolesName = rolesIsValid.RolesName;
                    }
                    if (roles.OrderIndex != rolesIsValid.OrderIndex)
                    {
                        roles.OrderIndex = rolesIsValid.OrderIndex;
                    }
                    if (roles.Actived != rolesIsValid.Actived)
                    {
                        roles.Actived = rolesIsValid.Actived;
                    }
                    _context.Entry(roles).State = EntityState.Modified;
                }
                else
                {
                    return string.Format(LanguageResource.Validation_ImportExcelIdNotExist,
                                            LanguageResource.RolesId, rolesIsValid.RolesId,
                                            string.Format(LanguageResource.Export_ExcelHeader,
                                            LanguageResource.Permission_RolesModel));
                }
            }
            _context.SaveChanges();
            return LanguageResource.ImportSuccess;
        }
        #endregion Insert/Update data from excel file

        #region Check data type 
        public RolesViewModel CheckTemplate(object[] row)
        {
            RolesViewModel rolesVM = new RolesViewModel();
            for (int i = 0; i <= row.Length; i++)
            {
                //Row index
                if (i == 0)
                {
                    int rowIndex = int.Parse(row[i].ToString());
                    rolesVM.RowIndex = rowIndex;
                }
                //RolesId
                else if (i == 1)
                {
                    string rolesId = row[i].ToString();
                    if (string.IsNullOrEmpty(rolesId))
                    {
                        rolesVM.isNullValueId = true;
                    }
                    else
                    {
                        rolesVM.RolesId = Guid.Parse(rolesId);
                        rolesVM.isNullValueId = false;
                    }
                }
                //RolesCode
                else if (i == 2)
                {
                    string rolesCode = row[i].ToString();
                    if (string.IsNullOrEmpty(rolesCode))
                    {
                        rolesVM.Error = string.Format(LanguageResource.Validation_ImportRequired, string.Format(LanguageResource.Required, LanguageResource.RolesCode), rolesVM.RowIndex);
                    }
                    else
                    {
                        rolesVM.RolesCode = rolesCode;
                    }
                }
                //RolesName
                else if (i == 3)
                {
                    string rolesName = row[i].ToString();
                    if (string.IsNullOrEmpty(rolesName))
                    {
                        rolesVM.Error = string.Format(LanguageResource.Validation_ImportRequired, string.Format(LanguageResource.Required, LanguageResource.RolesName), rolesVM.RowIndex);
                    }
                    else
                    {
                        rolesVM.RolesName = rolesName;
                    }
                }
                //OrderIndex
                else if (i == 4)
                {
                    int orderIndex = string.IsNullOrEmpty(row[i].ToString()) ? 0 : int.Parse(row[i].ToString());
                    rolesVM.OrderIndex = orderIndex;
                }
                //Actived
                else if (i == 5)
                {
                    rolesVM.Actived = string.IsNullOrEmpty(row[i].ToString()) ? true : bool.Parse(row[i].ToString());
                }
            }
            return rolesVM;
        }
        #endregion Check data type

        #endregion Import from excel
    }
}