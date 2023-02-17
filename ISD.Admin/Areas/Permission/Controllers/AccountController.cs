using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ISD.EntityModels;
using ISD.Extensions;
using ISD.ViewModels;
using System.Data.Entity;
using ISD.Resources;
using Microsoft.SqlServer.Server;
using System.Data;
using System.Data.SqlClient;
using ISD.Constant;
using ISD.ViewModels.MasterData;
using System.Transactions;
using ISD.Core;
using ISD.Repositories.Excel;

namespace Permission.Controllers
{
    public class AccountController : BaseController
    {
        //GET: /Account/Index
        #region Index
        [ISDAuthorizationAttribute]
        public ActionResult Index()
        {
            //get all roles
            var list = _context.RolesModel.Where(p => p.Actived == true).OrderBy(p => p.OrderIndex).ToList();
            AccountViewModel viewModel = new AccountViewModel() { RolesList = RolesInCurrentAccount(list) };

            return View(viewModel);
        }

        public ActionResult _Search(string UserName, string FullName, bool? Actived, Guid[] RolesId = null)
        {
            return ExecuteSearch(() =>
            {
                //Parameters for your query
                string sqlQuery = "exec uspAccount_Search @isDeveloper, @UserName, @FullName, @Actived";
                List<SqlParameter> parameters = new List<SqlParameter>()
                {
                    new SqlParameter
                    {
                        SqlDbType = SqlDbType.Bit,
                        Direction = ParameterDirection.Input,
                        ParameterName = "isDeveloper",
                        Value = isDeveloper
                    },
                    new SqlParameter
                    {
                        SqlDbType = SqlDbType.NVarChar,
                        Direction = ParameterDirection.Input,
                        ParameterName = "UserName",
                        Value = UserName
                    },
                    new SqlParameter
                    {
                        SqlDbType = SqlDbType.NVarChar,
                        Direction = ParameterDirection.Input,
                        ParameterName = "FullName",
                        Value = FullName
                    },
                    new SqlParameter
                    {
                        SqlDbType = SqlDbType.Bit,
                        Direction = ParameterDirection.Input,
                        ParameterName = "Actived",
                        Value = Actived
                    }
                };
                #region RolesId parameter
                //Build your record
                var tableSchema = new List<SqlMetaData>(1)
                {
                    new SqlMetaData("Id", SqlDbType.UniqueIdentifier)
                }.ToArray();

                //And a table as a list of those records
                var table = new List<SqlDataRecord>();
                if (RolesId != null && RolesId.Length > 0)
                {
                    foreach (var r in RolesId)
                    {
                        var tableRow = new SqlDataRecord(tableSchema);
                        tableRow.SetGuid(0, r);
                        table.Add(tableRow);
                    }
                    parameters.Add(
                        new SqlParameter
                        {
                            SqlDbType = SqlDbType.Structured,
                            Direction = ParameterDirection.Input,
                            ParameterName = "RolesId",
                            TypeName = "[dbo].[GuidList]", //Don't forget this one!
                            Value = table
                        });
                    sqlQuery += ", @RolesId";
                }
                #endregion
                var accountList = _context.Database.SqlQuery<AccountResultViewModel>(sqlQuery, parameters.ToArray()).OrderBy(p => p.EmployeeCode).ToList();

                return PartialView(accountList);
            });
        }
        #endregion

        //GET: /Account/Create
        #region Create
        [ISDAuthorizationAttribute]
        public ActionResult Create()
        {
            //get all roles
            var list = _context.RolesModel.Where(p => p.Actived == true).OrderBy(p => p.OrderIndex).ToList();
            AccountViewModel viewModel = new AccountViewModel() { 
                RolesList = RolesInCurrentAccount(list),
                isCreatePrivateTask = false,
                isViewRevenue = false,
                isViewOpportunity = false,
            };

            CreateViewBag(TaskFilterCode: ConstFilter.CaNhan, ViewPermission: ConstViewPermission.CONGTY);
            return View(viewModel);
        }
        [HttpPost]
        [ValidateAjax]
        [ValidateAntiForgeryToken]
        [ISDAuthorizationAttribute]
        public ActionResult Create(AccountModel model, List<Guid> RolesId, List<Guid> CompanyId, List<Guid> StoreId)
        {
            return ExecuteContainer(() =>
            {
                model.AccountId = Guid.NewGuid();
                //MD5 password
                model.Password = _unitOfWork.RepositoryLibrary.GetMd5Sum(model.Password);
                if (!string.IsNullOrEmpty(model.EmployeeCode))
                {
                    var EmployeeCode = model.EmployeeCode.Substring(0, model.EmployeeCode.IndexOf('|') - 1).Trim();
                    var emp = _context.SalesEmployeeModel.FirstOrDefault(p => p.SalesEmployeeCode == EmployeeCode);
                    if (emp != null)
                    {
                        model.EmployeeCode = EmployeeCode;
                        model.FullName = emp.SalesEmployeeName;
                    }
                    else
                    {
                        return Json(new
                        {
                            Code = System.Net.HttpStatusCode.NotModified,
                            Success = false,
                            Data = "Mã nhân viên không chính xác!"
                        });
                    }
                }

                if (RolesId != null && RolesId.Count > 0)
                {

                    #region custom for TienThu App
                    //Fix cứng code
                    //Nếu là khách hàng => thì không được phép là nhân viên
                    //  - Nếu có nhiều hơn 2 roles => và có role Khách hàng
                    //      => Báo lỗi không thể vừa là nhân viên vs vừa là khách hàng
                    if (RolesId.Count >= 2 && RolesId.Contains(ConstRolesForTienThuApp.CUSTOMER))
                    {
                        return Json(new
                        {
                            Code = System.Net.HttpStatusCode.NotModified,
                            Success = false,
                            Data = LanguageResource.TTApp_Account_Role_Errormessage
                        });
                    }

                    #endregion

                    ManyToMany(model, RolesId);
                }
                else
                {
                    //Bắt buộc nhập thông tin role
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotModified,
                        Success = false,
                        Data = LanguageResource.TTApp_Account_Role_Required
                    });
                }

                #region Check Company has store value
                List<Guid> tempCompanyId = new List<Guid>();
                if (StoreId != null)
                {
                    foreach (var item in StoreId)
                    {
                        var company = _context.StoreModel.Where(p => p.StoreId == item).Select(p => p.CompanyId).FirstOrDefault();
                        tempCompanyId.Add(company);
                    }
                    ManyToManyStore(model, StoreId);
                }
                tempCompanyId = tempCompanyId.Distinct().ToList();
                if (CompanyId != null && !CompanyId.SequenceEqual(tempCompanyId))
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotModified,
                        Success = false,
                        Data = LanguageResource.Error_AccountStore
                    });
                }
                #endregion

                _context.Entry(model).State = EntityState.Added;
                _context.SaveChanges();
                return Json(new
                {
                    Code = System.Net.HttpStatusCode.Created,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Create_Success, LanguageResource.Permission_AccountModel.ToLower())
                });
            });
        }
        #endregion

        //GET: /Account/Edit
        #region Edit
        [ISDAuthorizationAttribute]
        public ActionResult Edit(Guid id)
        {
            //get all roles
            var list = _context.RolesModel.Where(p => p.Actived == true).OrderBy(p => p.OrderIndex).ToList();
            var account = (from p in _context.AccountModel.AsEnumerable()
                           join e in _context.SalesEmployeeModel on p.EmployeeCode equals e.SalesEmployeeCode into tmpList
                           from emp in tmpList.DefaultIfEmpty()
                           where p.AccountId == id
                           select new AccountViewModel()
                           {
                               AccountId = p.AccountId,
                               UserName = p.UserName,
                               FullName = p.FullName,
                               Password = p.Password,
                               EmployeeCode = emp == null ? String.Empty : emp.SalesEmployeeCode + " | " + emp.SalesEmployeeName,
                               Actived = p.Actived,
                               isViewTotal = p.isViewTotal,
                               isViewByWarehouse = p.isViewByWarehouse,
                               isReceiveNotification = p.isReceiveNotification,
                               RolesList = RolesInCurrentAccount(list),
                               ActivedRolesList = p.RolesModel.ToList(),
                               ActivedStoreList = p.StoreModel.ToList(),
                               isShowChoseModule = p.isShowChoseModule,
                               isShowDashboard = p.isShowDashboard,
                               isViewByStore = p.isViewByStore,
                               TaskFilterCode = p.TaskFilterCode,
                               isCreatePrivateTask = p.isCreatePrivateTask == true ? true : false,
                               isViewRevenue = p.isViewRevenue == true ? true : false,
                               ViewPermission = p.ViewPermission,
                               isViewOpportunity = p.isViewOpportunity
                           }).FirstOrDefault();

            if (account == null)
            {
                return RedirectToAction("ErrorText", "Error", new { area = "", statusCode = 404, exception = string.Format(LanguageResource.Error_NotExist, LanguageResource.Permission_AccountModel.ToLower()) });
            }

            //Get list permission company
            List<CompanyModel> compList = new List<CompanyModel>();
            if (account.ActivedStoreList != null && account.ActivedStoreList.Count > 0)
            {
                foreach (var item in account.ActivedStoreList)
                {
                    var company = (from p in _context.CompanyModel
                                   join s in _context.StoreModel on p.CompanyId equals s.CompanyId
                                   where s.StoreId == item.StoreId
                                   select p).FirstOrDefault();
                    compList.Add(company);
                }
                compList = compList.Distinct().OrderBy(p => p.OrderIndex).ToList();
            }
            ViewBag.ActivedCompanyList = compList;

            CreateViewBag(account.TaskFilterCode, account.ViewPermission);
            return View(account);
        }
        [HttpPost]
        [ValidateAjax]
        [ValidateAntiForgeryToken]
        [ISDAuthorizationAttribute]
        public ActionResult Edit(AccountModel model, List<Guid> RolesId, List<Guid> CompanyId, List<Guid> StoreId)
        {
            return ExecuteContainer(() =>
            {
                var account = _context.AccountModel.Where(p => p.AccountId == model.AccountId)
                                                   .Include(p => p.RolesModel).FirstOrDefault();
                if (account != null)
                {
                    //master
                    account.UserName = model.UserName;
                    if (!string.IsNullOrEmpty(model.EmployeeCode))
                    {
                        var EmployeeCode = model.EmployeeCode.Substring(0, model.EmployeeCode.IndexOf('|') - 1).Trim();
                        var emp = _context.SalesEmployeeModel.FirstOrDefault(p => p.SalesEmployeeCode == EmployeeCode);
                        if (emp != null)
                        {
                            account.EmployeeCode = EmployeeCode;
                            account.FullName = emp.SalesEmployeeName;
                        }
                        else
                        {
                            return Json(new
                            {
                                Code = System.Net.HttpStatusCode.NotModified,
                                Success = false,
                                Data = "Mã nhân viên không chính xác!"
                            });
                        }
                    }
                    else
                    {
                        account.EmployeeCode = string.Empty;
                        account.FullName = string.Empty;
                    }
                    account.isShowChoseModule = model.isShowChoseModule;
                    account.isShowDashboard = model.isShowDashboard;
                    account.Actived = model.Actived;
                    account.isViewTotal = model.isViewTotal;
                    account.isViewByWarehouse = model.isViewByWarehouse;
                    account.isReceiveNotification = model.isReceiveNotification;
                    account.isViewByStore = model.isViewByStore;
                    account.TaskFilterCode = model.TaskFilterCode;
                    account.isCreatePrivateTask = model.isCreatePrivateTask;
                    account.isViewRevenue = model.isViewRevenue;
                    account.ViewPermission = model.ViewPermission;
                    account.isViewOpportunity = model.isViewOpportunity;
                    //detail roles
                    if (RolesId != null && RolesId.Count > 0)
                    {
                        #region custom for TienThu App
                        //Fix cứng code
                        //Nếu là khách hàng => thì không được phép là nhân viên
                        //  - Nếu có nhiều hơn 2 roles => và có role Khách hàng
                        //      => Báo lỗi không thể vừa là nhân viên vs vừa là khách hàng
                        if (RolesId.Count >= 2 && RolesId.Contains(ConstRolesForTienThuApp.CUSTOMER))
                        {
                            return Json(new
                            {
                                Code = System.Net.HttpStatusCode.NotModified,
                                Success = false,
                                Data = LanguageResource.TTApp_Account_Role_Errormessage
                            });
                        }
                        #endregion
                        ManyToMany(account, RolesId);
                    }
                    else
                    {
                        //Bắt buộc nhập thông tin Role
                        return Json(new
                        {
                            Code = System.Net.HttpStatusCode.NotModified,
                            Success = false,
                            Data = LanguageResource.TTApp_Account_Role_Required
                        });
                    }

                    #region Check Company has store value
                    List<Guid> tempCompanyId = new List<Guid>();
                    if (StoreId != null)
                    {
                        foreach (var item in StoreId)
                        {
                            var company = _context.StoreModel.Where(p => p.StoreId == item).Select(p => p.CompanyId).FirstOrDefault();
                            tempCompanyId.Add(company);
                        }
                        ManyToManyStore(account, StoreId);
                    }
                    tempCompanyId = tempCompanyId.Distinct().ToList();
                    if (CompanyId != null && !CompanyId.SequenceEqual(tempCompanyId))
                    {
                        return Json(new
                        {
                            Code = System.Net.HttpStatusCode.NotModified,
                            Success = false,
                            Data = LanguageResource.Error_AccountStore
                        });
                    }
                    #endregion

                    _context.Entry(account).State = EntityState.Modified;
                    _context.SaveChanges();
                }

                return Json(new
                {
                    Code = System.Net.HttpStatusCode.Created,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Edit_Success, LanguageResource.Permission_AccountModel.ToLower())
                });
            });
        }
        #endregion Edit

        //GET: /Account/Delete
        #region Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ISDAuthorizationAttribute]
        public ActionResult Delete(Guid id)
        {
            return ExecuteDelete(() =>
            {
                var account = _context.AccountModel.FirstOrDefault(p => p.AccountId == id);
                if (account != null)
                {
                    //Account in roles
                    if (account.RolesModel != null && account.RolesModel.Count > 0)
                    {
                        var isEmployee = account.RolesModel.Where(p => p.RolesCode != "CUSTOMER").ToList();
                        if (isEmployee != null)
                        {
                            //Account in store
                            if (account.StoreModel != null && account.StoreModel.Count > 0)
                            {
                                account.StoreModel.Clear();
                            }
                        }

                        account.RolesModel.Clear();
                    }

                    _context.Entry(account).State = EntityState.Deleted;
                    _context.SaveChanges();
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.Created,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Delete_Success, LanguageResource.Permission_AccountModel.ToLower())
                    });
                }
                else
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotModified,
                        Success = false,
                        Data = string.Format(LanguageResource.Alert_NotExist_Delete, LanguageResource.Permission_AccountModel.ToLower())
                    });
                }
            });
        }
        #endregion

        #region CreateViewBag, Helper
        private void ManyToMany(AccountModel model, List<Guid> RolesId)
        {
            if (model.RolesModel != null)
            {
                model.RolesModel.Clear();
            }
            foreach (var item in RolesId)
            {
                var role = _context.RolesModel.Find(item);
                if (role != null)
                {
                    model.RolesModel.Add(role);
                }
            }
        }
        //Many To Many account-store
        private void ManyToManyStore(AccountModel model, List<Guid> StoreId)
        {
            if (model.StoreModel != null)
            {
                model.StoreModel.Clear();
            }
            foreach (var item in StoreId)
            {
                var store = _context.StoreModel.Find(item);
                if (store != null)
                {
                    model.StoreModel.Add(store);
                }
            }
        }
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
        //Get company by permission store
        public void CreateViewBag(string TaskFilterCode = null, string ViewPermission = null)
        {
            var compList = _context.CompanyModel.Where(p => p.Actived == true).OrderBy(p => p.OrderIndex).ToList();
            ViewBag.CompanyId = compList;

            #region //Phân loại quyền xem công việc
            //var ForeignList = new List<SelectListItem>() {
            //    new SelectListItem() {
            //         Text = LanguageResource.Domestic,
            //         Value = false.ToString()
            //    },
            //    new SelectListItem() {
            //         Text = LanguageResource.Foreign,
            //         Value = true.ToString()
            //    }
            //};
            var taskFilterList = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.TaskFilter)
                                                                .Select(p => new SelectListItem()
                                                                {
                                                                    Text = p.CatalogText_vi,
                                                                    Value = p.CatalogCode,
                                                                });
            ViewBag.TaskFilterCode = new SelectList(taskFilterList, "Value", "Text", TaskFilterCode);
            #endregion

            #region //Phân loại quyền xem dữ liệu
            var viewPermissionList = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.ViewPermission)
                                                                .Select(p => new SelectListItem()
                                                                {
                                                                    Text = p.CatalogText_vi,
                                                                    Value = p.CatalogCode,
                                                                });
            ViewBag.ViewPermission = new SelectList(viewPermissionList, "Value", "Text", ViewPermission);
            #endregion
        }
        //GetStoreByCompany
        public ActionResult GetStoreByCompany(Guid? CompanyId = null)
        {
            var storeList = _context.StoreModel.Where(p => p.CompanyId == CompanyId && p.Actived == true)
                                         .Select(p =>
                                         new
                                         {
                                             StoreId = p.StoreId,
                                             StoreName = p.SaleOrgCode + " | " + p.StoreName,
                                             OrderIndex = p.OrderIndex
                                         })
                                         .OrderBy(p => p.OrderIndex)
                                         .ThenBy(p => p.StoreName).ToList();
            return Json(storeList, JsonRequestBehavior.AllowGet);
        }
        //autocomplete SalesEmployeeCode
        public ActionResult GetSalesEmployeeCode(string EmployeeCode)
        {
            var lst = (from p in _context.SalesEmployeeModel
                       where p.SalesEmployeeCode.Contains(EmployeeCode)
                       select p.SalesEmployeeCode + " | " + p.SalesEmployeeName)
                       .Take(5).ToList();
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Remote Validation
        private bool IsExists(string UserName)
        {
            return (_context.AccountModel.FirstOrDefault(p => p.UserName == UserName) != null);
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult CheckExistingUserName(string UserName, string UserNameValid)
        {
            try
            {
                if (!string.IsNullOrEmpty(UserNameValid) && !string.IsNullOrEmpty(UserName) && UserNameValid.ToLower() != UserName.ToLower())
                {
                    return Json(!IsExists(UserName));
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

        private bool IsEmployeeExists(string EmployeeCode)
        {
            return (_context.AccountModel.FirstOrDefault(p => p.EmployeeCode == EmployeeCode) != null);
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult CheckExistingEmployeeCode(string EmployeeCode, string EmployeeCodeValid)
        {
            try
            {
                if (!string.IsNullOrEmpty(EmployeeCodeValid) && !string.IsNullOrEmpty(EmployeeCode) && EmployeeCodeValid.ToLower() != EmployeeCode.ToLower())
                {
                    return Json(!IsEmployeeExists(EmployeeCode));
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

        #region Change Password
        //[ISDAuthorizationAttribute]
        public ActionResult ChangePassword(string id)
        {
            ChangePasswordAccountViewModel changePassword = new ChangePasswordAccountViewModel();
            var accountId = Guid.Parse(id);
            var account = _context.AccountModel.FirstOrDefault(p => p.AccountId == accountId);
            if (account != null)
            {
                changePassword.AccountId = account.AccountId;
                changePassword.UserName = account.UserName;

            }

            return View(changePassword);
        }
        [HttpPost]
        //[ISDAuthorizationAttribute]
        public ActionResult ChangePassword(ChangePasswordAccountViewModel changePassword)
        {
            return ExecuteContainer(() =>
            {
                //MD5 old password to compare
                //var accountId = changePassword.AccountId;
                //changePassword.OldPassword = RepositoryLibrary.GetMd5Sum(changePassword.OldPassword);
                var account = _context.AccountModel.Where(p => p.AccountId == changePassword.AccountId).FirstOrDefault();
                if (account != null)
                {
                    //MD5 new password
                    account.Password = _unitOfWork.RepositoryLibrary.GetMd5Sum(changePassword.NewPassword);
                    _context.Entry(account).State = EntityState.Modified;
                    _context.SaveChanges();

                    //Anh Tiến said [08/08/2019]
                    //Mỗi lần đổi mật khẩu thành công là đá ra hết
                    //if (Request.Cookies["userInfo"] != null)
                    //{
                    //    var c = new HttpCookie("userInfo");
                    //    c.Expires = DateTime.Now.AddDays(-1);
                    //    Response.Cookies.Add(c);
                    //    Response.Redirect("/Permission/Auth/Login");
                    //    Response.End();
                    //}

                    //var ctx = Request.GetOwinContext();
                    //var authManager = ctx.Authentication;
                    ////Identity
                    //authManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                    ////Session
                    //Session["Menu"] = null;

                    //return Json(new
                    //{
                    //    Code = System.Net.HttpStatusCode.Created,
                    //    Success = true,
                    //    Data = ConstFunction.FORCE_LOGOUT
                    //});

                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.Created,
                        Success = true,
                        Data = string.Format(LanguageResource.Alert_Edit_Success, LanguageResource.Password.ToLower())
                    });
                }
                return Json(new
                {
                    Code = System.Net.HttpStatusCode.NotModified,
                    Success = false,
                    Data = LanguageResource.Alert_ChangePassword
                });
            });
        }
        #endregion

        //Define code for account excel template
        const string controllerCodeI = ConstExcelController.AcountI;
        const string controllerCodeU = ConstExcelController.AcountU;
        const int startIndex = 11;

        #region Export Excel
        public ActionResult ExportCreate()
        {
            List<AccountExcelViewModel> viewModel = new List<AccountExcelViewModel>();
            return Export(viewModel, isEdit: false);
        }

        public ActionResult ExportEdit()
        {
            //Get data from server
            var account = _context.Database.SqlQuery<AccountExcelViewModel>("[pms].[FindAccount]").ToList();

            return Export(account, isEdit: true);
        }

        #region Export function
        [ISDAuthorizationAttribute]
        public FileContentResult Export(List<AccountExcelViewModel> viewModel, bool isEdit)
        {
            #region Master
            //Tạo mẫu
            List<ExcelTemplate> columns = new List<ExcelTemplate>();
            if (isEdit == true)
            {
                columns.Add(new ExcelTemplate() { ColumnName = "AccountId", isAllowedToEdit = false });
                columns.Add(new ExcelTemplate() { ColumnName = "UserName", isAllowedToEdit = false });
            }
            else
            {
                columns.Add(new ExcelTemplate() { ColumnName = "UserName", isAllowedToEdit = true, });
                columns.Add(new ExcelTemplate() { ColumnName = "Password", isAllowedToEdit = true });
            }
            columns.Add(new ExcelTemplate() { ColumnName = "FullName", isAllowedToEdit = true });
            columns.Add(new ExcelTemplate() { ColumnName = "EmployeeCode", isAllowedToEdit = true });
            columns.Add(new ExcelTemplate() { ColumnName = "isShowChoseModule", isAllowedToEdit = true, isBoolean = true });//sau khi đăng nhập chọn module
            columns.Add(new ExcelTemplate() { ColumnName = "isShowDashboard", isAllowedToEdit = true, isBoolean = true });//sau khi đăng nhập xem thống kê
            columns.Add(new ExcelTemplate() { ColumnName = "RoleList", isAllowedToEdit = true });//Nhóm người dùng
            columns.Add(new ExcelTemplate() { ColumnName = "StoreList", isAllowedToEdit = true });//chi nhánh
            columns.Add(new ExcelTemplate() { ColumnName = "Actived", isAllowedToEdit = true, isBoolean = true });

            #endregion

            //Header
            string fileHeader = string.Format(LanguageResource.Export_ExcelHeader, LanguageResource.Permission_AccountModel);

            if (isEdit == false)
            {
                heading.Add(new ExcelHeadingTemplate()
                {
                    Content = controllerCodeI,
                    RowsToIgnore = 1,
                    isWarning = false,
                    isCode = true,
                });
            }
            else
            {
                heading.Add(new ExcelHeadingTemplate()
                {
                    Content = controllerCodeU,
                    RowsToIgnore = 1,
                    isWarning = false,
                    isCode = true,
                });
            }

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
                Content = string.Format(LanguageResource.Export_ExcelWarningActived, LanguageResource.Permission_AccountModel),
                RowsToIgnore = 0,
                isWarning = true,
                isCode = false
            });

            heading.Add(new ExcelHeadingTemplate()
            {
                Content = string.Format(LanguageResource.Export_ExcelWarningChoseModule, LanguageResource.Permission_AccountModel),
                RowsToIgnore = 0,
                isWarning = true,
                isCode = false
            });

            heading.Add(new ExcelHeadingTemplate()
            {
                Content = string.Format(LanguageResource.Export_ExcelWarningViewTotal, LanguageResource.Permission_AccountModel),
                RowsToIgnore = 0,
                isWarning = true,
                isCode = false
            });

            heading.Add(new ExcelHeadingTemplate()
            {
                Content = LanguageResource.Export_ExcelWarningAcount,
                RowsToIgnore = 1,
                isWarning = true,
                isCode = false
            });

            //Nội dung file
            byte[] fileContent = ClassExportExcel.ExportExcel(viewModel, columns, heading, true);

            //Insert or Edit
            string exportType = LanguageResource.exportType_Insert;
            if (isEdit == true)
            {
                exportType = LanguageResource.exportType_Edit;
            }
            //Ten file sau khi export
            string fileNameWithFormat = string.Format("{0}_{1}.xlsx", exportType, _unitOfWork.UtilitiesRepository.RemoveSign4VietnameseString(fileHeader.ToUpper().Replace(" ", "_")));

            return File(fileContent, ClassExportExcel.ExcelContentType, fileNameWithFormat);

        }
        #endregion


        #endregion

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
                            #region Import
                            #region Insert
                            if (contCode == controllerCodeI)
                            {
                                var index = 0;
                                foreach (DataRow dr in dt.Rows)
                                {
                                    if (dt.Rows.IndexOf(dr) >= startIndex && !string.IsNullOrEmpty(dr.ItemArray[0].ToString()))
                                    {
                                        index++;
                                        //Check correct template
                                        AccountExcelViewModel accountIsValid = CheckTemplateInsert(dr.ItemArray, index);

                                        if (!string.IsNullOrEmpty(accountIsValid.Error))
                                        {
                                            string error = accountIsValid.Error;
                                            errorList.Add(error);
                                        }
                                        else
                                        {
                                            string result = ExecuteImportExcelAccount(accountIsValid, contCode);
                                            if (result != LanguageResource.ImportSuccess)
                                            {
                                                errorList.Add(result);
                                            }
                                        }
                                    }
                                }
                            }
                            #endregion

                            #region Update
                            else
                            {
                                //Update
                                if (contCode == controllerCodeU)
                                {
                                    var index = 0;
                                    foreach (DataRow dr in dt.Rows)
                                    {
                                        if (dt.Rows.IndexOf(dr) >= startIndex && !string.IsNullOrEmpty(dr.ItemArray[0].ToString()))
                                        {
                                            index++;
                                            //Check correct template
                                            AccountExcelViewModel accountIsValid = CheckTemplateUpdate(dr.ItemArray, index);

                                            if (!string.IsNullOrEmpty(accountIsValid.Error))
                                            {
                                                string error = accountIsValid.Error;
                                                errorList.Add(error);
                                            }
                                            else
                                            {
                                                string result = ExecuteImportExcelAccount(accountIsValid, contCode);
                                                if (result != LanguageResource.ImportSuccess)
                                                {
                                                    errorList.Add(result);
                                                }
                                            }
                                        }
                                    }
                                }
                                #endregion
                                else
                                {
                                    string error = string.Format(LanguageResource.Validation_ImportCheckController, LanguageResource.Permission_AccountModel);
                                    errorList.Add(error);
                                }
                            }
                            #endregion

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
        public string ExecuteImportExcelAccount(AccountExcelViewModel accountIsValid, string code)
        {
            //Check:
            //1. If employee == null then => Insert
            //2. Else then => Update
            if (accountIsValid.UserName.ToUpper() != "SYSADMIN" && accountIsValid.UserName.ToUpper() != "ADMIN")
            {
                #region Insert
                if (code == controllerCodeI)
                {

                    //Add new Account
                    AccountModel accountNew = new AccountModel();
                    accountNew.AccountId = Guid.NewGuid();
                    accountNew.UserName = accountIsValid.UserName;
                    accountNew.Password = _unitOfWork.RepositoryLibrary.GetMd5Sum(accountIsValid.Password);
                    accountNew.FullName = accountIsValid.FullName;
                    accountNew.EmployeeCode = accountIsValid.EmployeeCode;
                    accountNew.isShowChoseModule = accountIsValid.isShowChoseModule;
                    accountNew.isShowDashboard = accountIsValid.isShowDashboard;
                    accountNew.Actived = accountIsValid.Actived;

                    //Add role
                    string[] roles = accountIsValid.RoleList.Split(',');
                    roles = roles.Select(p => p.Trim()).ToArray();
                    if (roles != null && roles.Length > 0)
                    {
                        //RoleID List
                        var RoleIDs = _context.RolesModel.Where(p => roles.Contains(p.RolesCode)).Select(p => p.RolesId).ToList();
                        ManyToMany(accountNew, RoleIDs);
                    }

                    //Add store
                    if (!string.IsNullOrEmpty(accountIsValid.StoreList))
                    {
                        string[] stores = accountIsValid.StoreList.Split(',');
                        stores = stores.Select(p => p.Trim()).ToArray();

                        if (stores != null && stores.Length > 0)
                        {
                            var StoreIDs = _context.StoreModel.Where(p => stores.Contains(p.SaleOrgCode)).Select(p => p.StoreId).ToList();
                            ManyToManyStore(accountNew, StoreIDs);
                        }
                    }
                    _context.Entry(accountNew).State = EntityState.Added;

                }
                #endregion Insert

                #region Update
                else
                {
                    //Get account in Database
                    var accountInDB = _context.AccountModel.FirstOrDefault(a => a.AccountId == accountIsValid.AccountId);

                    if (accountInDB == null)
                    {
                        return string.Format(LanguageResource.Validation_ImportExcelIdNotExist,
                                                LanguageResource.Excel_AccountId, accountIsValid.AccountId,
                                                string.Format(LanguageResource.Export_ExcelHeader,
                                                LanguageResource.Permission_AccountModel));
                    }
                    else
                    {
                        //Cập nhật tài khoản
                        accountInDB.FullName = accountIsValid.FullName;
                        accountInDB.EmployeeCode = accountIsValid.EmployeeCode;
                        accountInDB.isShowChoseModule = accountIsValid.isShowChoseModule;
                        accountInDB.isShowDashboard = accountIsValid.isShowDashboard;
                        accountInDB.Actived = accountIsValid.Actived;

                        string[] roles = accountIsValid.RoleList.Split(',');
                        roles = roles.Select(p => p.Trim()).ToArray();

                        if (roles != null && roles.Length > 0)
                        {
                            //RoleID List
                            var RoleIDs = _context.RolesModel.Where(p => roles.Contains(p.RolesCode)).Select(p => p.RolesId).ToList();
                            ManyToMany(accountInDB, RoleIDs);
                        }


                        if (!string.IsNullOrEmpty(accountIsValid.StoreList))
                        {
                            string[] stores = accountIsValid.StoreList.Split(',');
                            stores = stores.Select(p => p.Trim()).ToArray();

                            if (stores != null && stores.Length > 0)
                            {
                                var StoreIDs = _context.StoreModel.Where(p => stores.Contains(p.SaleOrgCode)).Select(p => p.StoreId).ToList();
                                ManyToManyStore(accountInDB, StoreIDs);
                            }
                        }
                        else
                        {
                            accountInDB.StoreModel.Clear();
                        }
                        _context.Entry(accountInDB).State = EntityState.Modified;
                    }
                }
            }
            #endregion Update

            _context.SaveChanges();
            return LanguageResource.ImportSuccess;
        }
        #endregion Insert/Update data from excel file

        #region Check data type update
        public AccountExcelViewModel CheckTemplateUpdate(object[] row, int index)
        {
            AccountExcelViewModel accountVM = new AccountExcelViewModel();
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
                            accountVM.RowIndex = rowIndex;
                            break;
                        //Account ID
                        case 1:
                            fieldName = LanguageResource.Excel_AccountId;
                            string accountId = row[i].ToString();
                            if (string.IsNullOrEmpty(accountId))
                            {
                                accountVM.isNullValueId = true;
                            }
                            else
                            {
                                accountVM.AccountId = Guid.Parse(accountId);
                                accountVM.isNullValueId = false;
                            }
                            break;
                        //Tên tài khoản (*) 
                        case 2:
                            fieldName = LanguageResource.UserName;
                            string userName = row[i].ToString();
                            if (string.IsNullOrEmpty(userName))
                            {
                                accountVM.Error = string.Format(LanguageResource.Validation_ImportRequired, string.Format(LanguageResource.Required, LanguageResource.UserName), accountVM.RowIndex);
                            }
                            else
                            {
                                accountVM.UserName = userName;
                            }
                            break;
                        //Họ và tên
                        case 3:
                            fieldName = LanguageResource.Account_FullName;
                            accountVM.FullName = row[i].ToString();
                            break;
                        //Mã nhân viên 
                        case 4:
                            fieldName = LanguageResource.SalesEmployee_Code;
                            string empCode = row[i].ToString();
                            if (!string.IsNullOrEmpty(empCode))
                            {
                                empCode = empCode.Trim();
                                var emp = _context.SalesEmployeeModel.FirstOrDefault(p => p.SalesEmployeeCode == empCode);
                                if (emp != null)
                                {
                                    accountVM.EmployeeCode = emp.SalesEmployeeCode;
                                    accountVM.FullName = emp.SalesEmployeeName;
                                }
                                else
                                {
                                    accountVM.Error = string.Format(LanguageResource.Validation_ImportExcelIdNotExist, LanguageResource.EmployeeCode, empCode, string.Format(LanguageResource.Export_ExcelHeader, LanguageResource.MasterData_SalesEmployee));
                                }
                            }
                            else
                            {
                                accountVM.EmployeeCode = empCode;
                            }
                            break;
                        //Chọn mô-đun 
                        case 5:
                            fieldName = LanguageResource.Account_ShowChoseModule;
                            accountVM.isShowChoseModule = GetTypeFunction<bool>(row[i].ToString(), i);
                            break;
                        //Xem thống kê
                        case 6:
                            fieldName = LanguageResource.Account_ViewTotal;
                            accountVM.isShowDashboard = GetTypeFunction<bool>(row[i].ToString(), i);
                            break;
                        //Nhóm người dùng (*) 
                        case 7:
                            fieldName = LanguageResource.Permission_RolesModel;
                            string roles = row[i].ToString();
                            if (string.IsNullOrEmpty(roles))
                            {
                                accountVM.Error = string.Format(LanguageResource.Validation_ImportRequired, string.Format(LanguageResource.Required, LanguageResource.UserName), accountVM.RowIndex);
                            }
                            else
                            {
                                accountVM.RoleList = roles;
                            }
                            break;
                        //Chi nhánh
                        case 8:
                            fieldName = LanguageResource.AcountExcel_SalesOrg;
                            accountVM.StoreList = row[i].ToString();
                            break;
                        //Actived
                        case 9:
                            fieldName = LanguageResource.Actived;
                            accountVM.Actived = GetTypeFunction<bool>(row[i].ToString(), i);
                            break;
                    }
                    #endregion Convert data to import
                }
            }
            catch (FormatException ex)
            {
                accountVM.Error = string.Format(LanguageResource.Validation_ImportCastValid, fieldName, index) + ex.Message;
            }
            catch (InvalidCastException ex)
            {
                accountVM.Error = string.Format(LanguageResource.Validation_ImportCastValid, fieldName, index) + ex.Message;
            }
            catch (Exception ex)
            {
                accountVM.Error = string.Format(LanguageResource.Validate_ImportException, fieldName, index) + ex.Message;
            }
            return accountVM;
        }
        #endregion

        #region Check data type Insert
        public AccountExcelViewModel CheckTemplateInsert(object[] row, int index)
        {
            AccountExcelViewModel accountVM = new AccountExcelViewModel();
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
                            accountVM.RowIndex = rowIndex;
                            break;

                        //Tên tài khoản (*) 
                        case 1:
                            fieldName = LanguageResource.UserName;
                            string userName = row[i].ToString();
                            if (string.IsNullOrEmpty(userName))
                            {
                                accountVM.Error = string.Format(LanguageResource.Validation_ImportRequired, string.Format(LanguageResource.Required, LanguageResource.UserName), accountVM.RowIndex);
                            }
                            else
                            {
                                accountVM.UserName = userName;
                            }
                            break;
                        case 2:
                            fieldName = LanguageResource.Password;
                            string password = row[i].ToString();
                            if (string.IsNullOrEmpty(password))
                            {
                                accountVM.Error = string.Format(LanguageResource.Validation_ImportRequired, string.Format(LanguageResource.Required, LanguageResource.Password), accountVM.RowIndex);
                            }
                            else
                            {
                                accountVM.Password = password;
                            }
                            break;
                        //Họ và tên
                        case 3:
                            fieldName = LanguageResource.Account_FullName;
                            accountVM.FullName = row[i].ToString();
                            break;
                        //Mã nhân viên 
                        case 4:
                            fieldName = LanguageResource.SalesEmployee_Code;
                            string empCode = row[i].ToString();
                            if (!string.IsNullOrEmpty(empCode))
                            {
                                empCode = empCode.Trim();
                                var emp = _context.SalesEmployeeModel.FirstOrDefault(p => p.SalesEmployeeCode == empCode);
                                if (emp != null)
                                {
                                    accountVM.EmployeeCode = emp.SalesEmployeeCode;
                                    accountVM.FullName = emp.SalesEmployeeName;
                                }
                                else
                                {
                                    accountVM.Error = string.Format(LanguageResource.Validation_ImportExcelIdNotExist, LanguageResource.EmployeeCode, empCode, string.Format(LanguageResource.Export_ExcelHeader, LanguageResource.MasterData_SalesEmployee));
                                }
                            }
                            else
                            {
                                accountVM.EmployeeCode = empCode;
                            }
                            break;
                        //Chọn mô-đun 
                        case 5:
                            fieldName = LanguageResource.Account_ShowChoseModule;
                            accountVM.isShowChoseModule = GetTypeFunction<bool>(row[i].ToString(), i);
                            break;
                        //Xem thống kê
                        case 6:
                            fieldName = LanguageResource.Account_ViewTotal;
                            accountVM.isShowDashboard = GetTypeFunction<bool>(row[i].ToString(), i);
                            break;
                        //Nhóm người dùng (*) 
                        case 7:
                            fieldName = LanguageResource.Permission_RolesModel;
                            string roles = row[i].ToString();
                            if (string.IsNullOrEmpty(roles))
                            {
                                accountVM.Error = string.Format(LanguageResource.Validation_ImportRequired, string.Format(LanguageResource.Required, LanguageResource.Permission_RolesModel), accountVM.RowIndex);
                            }
                            else
                            {
                                accountVM.RoleList = roles;
                            }
                            break;
                        //Chi nhánh
                        case 8:
                            fieldName = LanguageResource.AcountExcel_SalesOrg;
                            accountVM.StoreList = row[i].ToString();
                            break;
                        //Actived
                        case 9:
                            fieldName = LanguageResource.Actived;
                            accountVM.Actived = GetTypeFunction<bool>(row[i].ToString(), i);
                            break;
                    }
                    #endregion Convert data to import
                }
            }
            catch (FormatException ex)
            {
                accountVM.Error = string.Format(LanguageResource.Validation_ImportCastValid, fieldName, index) + ex.Message;
            }
            catch (InvalidCastException ex)
            {
                accountVM.Error = string.Format(LanguageResource.Validation_ImportCastValid, fieldName, index) + ex.Message;
            }
            catch (Exception ex)
            {
                accountVM.Error = string.Format(LanguageResource.Validate_ImportException, fieldName, index) + ex.Message;
            }
            return accountVM;
        }
        #endregion
        #endregion
    }
}