using ISD.Constant;
using ISD.EntityModels;
using ISD.Extensions;
using ISD.Repositories;
using ISD.Resources;
using ISD.ViewModels;
using ISD.ViewModels.API;
using ISD.ViewModels.API.Mobile;
using ISD.ViewModels.API.MobileBooking;
using ISD.ViewModels.Permission;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Script.Serialization;

namespace MasterData.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [AllowAnonymous]
    public class APIController : BaseController
    {
        public const string tokenConst = "454FC8F419313554549E2DED09B9AF94";
        public const string keyConst = "77f430e1-66fd-48dc-8057-77935e53be20";

        // GET: API
        #region API Login Mobile
        public ActionResult CheckLoginMobile(string UserName, string Password, string DeviceId, string token, string key)
        {
            LoginMobileViewModel result = new LoginMobileViewModel();
            result.IsSuccess = false;
            try
            {
                if (token == tokenConst && key == keyConst)
                {

                    Password = RepositoryLibrary.GetMd5Sum(Password);
                    var account = _context.AccountModel.Where(p => p.UserName == UserName && p.Password == Password)
                                                       .Select(p => p).FirstOrDefault();
                    if (account != null)
                    {
                        #region Kiểm tra đăng nhập trong giờ làm
                        var employeeRole = (from p in _context.AccountModel
                                            from r in p.RolesModel
                                            where p.AccountId == account.AccountId
                                            select r.RolesCode).FirstOrDefault();
                        if (employeeRole == ConstRoles.Sale || employeeRole == ConstRoles.Service)
                        {
                            bool isHasPermission;
                            CheckLoginPermissionByTime(account.UserName, out isHasPermission);
                            if (isHasPermission == false)
                            {
                                result.IsSuccess = false;
                                result.Error = "Hiện tại đã quá thời gian làm việc. Vui lòng đăng nhập trong giờ làm.";
                                return Json(result, JsonRequestBehavior.AllowGet);
                            }
                        }
                        #endregion
                        if (account.Actived != true)
                        {
                            result.Error = LanguageResource.Account_Locked;
                        }
                        else
                        {
                            //Lưu DeviceId khi user đăng nhập 
                            if (!string.IsNullOrEmpty(DeviceId))
                            {
                                try
                                {
                                    account.DeviceId = DeviceId;
                                    account.LastLogin = DateTime.Now;
                                    _context.Entry(account).State = EntityState.Modified;
                                    _context.SaveChanges();
                                }
                                catch (Exception ex)
                                {

                                }
                            }
                            result.AccountId = account.AccountId;
                            result.UserName = account.UserName;
                            result.Password = account.Password;
                            result.EmployeeCode = account.EmployeeCode;
                            result.Actived = account.Actived;
                            result.RoleCodeList = (from p in _context.AccountModel
                                                   from r in p.RolesModel
                                                   where p.AccountId == account.AccountId
                                                   select r.RolesCode).ToList();
                            result.StoreList = (from p in _context.AccountModel
                                                from r in p.StoreModel
                                                where p.AccountId == account.AccountId
                                                orderby r.OrderIndex
                                                select r.StoreCode
                                                   ).ToList();
                            result.IsSuccess = true;

                            //check customer authorized
                            var customerRole = (from p in _context.AccountModel
                                                from r in p.RolesModel
                                                where p.AccountId == account.AccountId
                                                select r.RolesCode).FirstOrDefault();
                            if (customerRole == ConstRoles.Customer)
                            {
                                var customerActived = (from acc in _context.AccountModel
                                                       join cus in _context.CustomerModel on acc.AccountId equals cus.AccountId
                                                       where acc.UserName == account.UserName
                                                       select new { cus.FirstName, cus.SMSConfirm, cus.CustomerCode, cus.CustomerId, cus.Phone }).FirstOrDefault();
                                result.CustomerId = customerActived.CustomerId;
                                result.CustomerCode = customerActived.CustomerCode;
                                result.FullName = customerActived.FirstName;
                                result.Phone = customerActived.Phone;
                                if (customerActived.SMSConfirm != true)
                                {
                                    result.IsSuccess = false;
                                    result.Error = LanguageResource.Account_NotAuthorized;
                                }
                            }
                        }
                    }
                    else
                    {
                        result.Error = LanguageResource.Account_Confirm;
                    }
                }
                else
                {
                    //access denied
                    result.Error = LanguageResource.Account_AccessDenied;
                }
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                if (ex.InnerException != null)
                {
                    result.Error = ex.InnerException.Message;
                }
                else
                {
                    result.Error = ex.Message;
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion API Login Mobile

        #region Update Price Product
        public ActionResult UpdatePriceProduct(PriceProductViewModel model)
        {
            try
            {
                var updateModel = (from p in _context.PriceProductModel
                                   join pr in _context.ProductModel on p.ProductId equals pr.ProductId
                                   join s in _context.StyleModel on p.StyleId equals s.StyleId into sg
                                   from s1 in sg.DefaultIfEmpty()
                                   join c in _context.ColorModel on p.MainColorId equals c.ColorId
                                   //PriceProductCode
                                   where p.PriceProductCode == model.PriceProductCode
                                   //Product
                                   && pr.ProductCode == model.ProductCode
                                   //Style
                                   && s1.StyleCode == model.StyleCode
                                   //Color
                                   && c.ColorShortName == model.MainColorCode
                                   select p).FirstOrDefault();

                //Update
                if (updateModel != null)
                {
                    updateModel.Price = model.Price;
                    updateModel.PostDate = model.PostDate;
                    updateModel.PostTime = model.PostTime;
                    updateModel.UserPost = model.UserPost;

                    _context.Entry(updateModel).State = EntityState.Modified;
                    _context.SaveChanges();
                    return Json(new
                    {
                        Status = "Success",
                        Message = "Cập nhật giá sản phẩm thành công!"
                    }, JsonRequestBehavior.AllowGet);
                }
                //Insert
                else
                {
                    PriceProductModel addModel = new PriceProductModel();
                    addModel.PriceProductId = Guid.NewGuid();

                    //Product
                    var product = _context.ProductModel.FirstOrDefault(p => p.ProductCode == model.ProductCode);
                    if (product != null)
                    {
                        addModel.ProductId = product.ProductId;
                    }
                    else
                    {
                        return Json(new
                        {
                            Status = "Fail",
                            Message = "Mã sản phẩm chưa chính xác!"
                        }, JsonRequestBehavior.AllowGet);
                    }
                    //Style
                    if (model.StyleCode != null)
                    {
                        var style = _context.StyleModel.FirstOrDefault(p => p.StyleCode == model.StyleCode);
                        if (style != null)
                        {
                            addModel.StyleId = style.StyleId;
                        }
                        else
                        {
                            return Json(new
                            {
                                Status = "Fail",
                                Message = "Kiểu dáng xe chưa chính xác!"
                            }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    //MainColor
                    var color = _context.ColorModel.FirstOrDefault(p => p.ColorShortName == model.MainColorCode);
                    if (color != null)
                    {
                        addModel.MainColorId = color.ColorId;
                    }
                    else
                    {
                        return Json(new
                        {
                            Status = "Fail",
                            Message = "Màu sản phẩm chưa chính xác!"
                        }, JsonRequestBehavior.AllowGet);
                    }
                    addModel.PriceProductCode = model.PriceProductCode;
                    addModel.Price = model.Price;
                    addModel.PostDate = model.PostDate;
                    addModel.PostTime = model.PostTime;
                    addModel.UserPost = model.UserPost;
                    _context.Entry(addModel).State = EntityState.Added;
                    _context.SaveChanges();

                    return Json(new
                    {
                        Status = "Success",
                        Message = "Cập nhật giá sản phẩm thành công!"
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    return Json(new
                    {
                        Status = "Fail",
                        Message = ex.InnerException.Message
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        Status = "Fail",
                        Message = ex.Message
                    }, JsonRequestBehavior.AllowGet);
                }
            }
        }
        #endregion Update Price Product

        #region Update Warehouse Stock
        public ActionResult UpdateWarehouseStock(WarehouseProductViewModel model)
        {
            try
            {
                var updateModel = (from p in _context.WarehouseProductModel
                                   join w in _context.WarehouseModel on p.WarehouseId equals w.WarehouseId
                                   join pr in _context.ProductModel on p.ProductId equals pr.ProductId
                                   join s in _context.StyleModel on p.StyleId equals s.StyleId into sg
                                   from s1 in sg.DefaultIfEmpty()
                                   join c in _context.ColorModel on p.MainColorId equals c.ColorId
                                   //Product
                                   where pr.ProductCode == model.ProductCode
                                   //Warehouse
                                   && w.WarehouseCode == model.WarehouseCode
                                   //Style
                                   && s1.StyleCode == model.StyleCode
                                   //Color
                                   && c.ColorShortName == model.MainColorCode
                                   select p).FirstOrDefault();

                //Update
                if (updateModel != null)
                {
                    updateModel.Quantity = model.Quantity;
                    updateModel.PostDate = model.PostDate;
                    updateModel.PostTime = model.PostTime;
                    updateModel.UserPost = model.UserPost;

                    _context.Entry(updateModel).State = EntityState.Modified;
                    _context.SaveChanges();

                    return Json(new
                    {
                        Status = "Success",
                        Message = "Cập nhật tồn kho thành công!"
                    }, JsonRequestBehavior.AllowGet);
                }
                //Insert
                else
                {
                    WarehouseProductModel addModel = new WarehouseProductModel();
                    addModel.WarehouseProductId = Guid.NewGuid();

                    //Product
                    var product = _context.ProductModel.FirstOrDefault(p => p.ProductCode == model.ProductCode);
                    if (product != null)
                    {
                        addModel.ProductId = product.ProductId;
                    }
                    else
                    {
                        return Json(new
                        {
                            Status = "Fail",
                            Message = "Mã sản phẩm chưa chính xác!"
                        }, JsonRequestBehavior.AllowGet);
                    }
                    //Warehouse
                    var warehouse = _context.WarehouseModel.FirstOrDefault(p => p.WarehouseCode == model.WarehouseCode);
                    if (warehouse != null)
                    {
                        addModel.WarehouseId = warehouse.WarehouseId;
                    }
                    else
                    {
                        return Json(new
                        {
                            Status = "Fail",
                            Message = "Kho chưa chính xác!"
                        }, JsonRequestBehavior.AllowGet);
                    }
                    //Style
                    if (model.StyleCode != null)
                    {
                        var style = _context.StyleModel.FirstOrDefault(p => p.StyleCode == model.StyleCode);
                        if (style != null)
                        {
                            addModel.StyleId = style.StyleId;
                        }
                        else
                        {
                            return Json(new
                            {
                                Status = "Fail",
                                Message = "Kiểu dáng xe chưa chính xác!"
                            }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    //MainColor
                    var color = _context.ColorModel.FirstOrDefault(p => p.ColorShortName == model.MainColorCode);
                    if (color != null)
                    {
                        addModel.MainColorId = color.ColorId;
                    }
                    else
                    {
                        return Json(new
                        {
                            Status = "Fail",
                            Message = "Màu sản phẩm chưa chính xác!"
                        }, JsonRequestBehavior.AllowGet);
                    }
                    addModel.Quantity = model.Quantity;
                    addModel.PostDate = model.PostDate;
                    addModel.PostTime = model.PostTime;
                    addModel.UserPost = model.UserPost;
                    _context.Entry(addModel).State = EntityState.Added;
                    _context.SaveChanges();

                    return Json(new
                    {
                        Status = "Success",
                        Message = "Cập nhật tồn kho thành công!"
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    return Json(new
                    {
                        Status = "Fail",
                        Message = ex.InnerException.Message
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        Status = "Fail",
                        Message = ex.Message
                    }, JsonRequestBehavior.AllowGet);
                }
            }
        }
        #endregion Update Warehouse Stock

        #region Get About
        public ActionResult GetAbout(string token, string key)
        {
            var result = new AboutMobileViewModel();
            if (token == tokenConst && key == keyConst)
            {
                result = _context.AboutModel
                                .Select(p => new AboutMobileViewModel()
                                {
                                    AboutDescription = p.AboutDescription
                                }).FirstOrDefault();
            }
            else
            {
                //access denied
                result.Error = LanguageResource.Account_AccessDenied;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Get Contact
        public ActionResult GetContact(string token, string key)
        {
            var result = new AboutMobileViewModel();
            if (token == tokenConst && key == keyConst)
            {
                result = _context.ContactModel
                                .Select(p => new AboutMobileViewModel()
                                {
                                    ContactDescription = p.ContactDescription
                                }).FirstOrDefault();
            }
            else
            {
                //access denied
                result.Error = LanguageResource.Account_AccessDenied;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Get Banner
        public ActionResult GetBanner(string token, string key)
        {
            var result = new List<BannerMobileViewModel>();
            string Error = "";
            if (token == tokenConst && key == keyConst)
            {
                result = GetBanner();
            }
            else
            {
                //access denied
                Error = LanguageResource.Account_AccessDenied;
            }
            return Json(new { result, Error }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Get Customer Promotion
        //Display in home page
        public ActionResult GetHomePromotion(string token, string key)
        {
            var result = new List<CustomerPromotionViewModel>();
            string Error = "";
            if (token == tokenConst && key == keyConst)
            {
                result = GetHomePromotions();
            }
            else
            {
                //access denied
                Error = LanguageResource.Account_AccessDenied;
            }
            return Json(new { result, Error }, JsonRequestBehavior.AllowGet);
        }



        //Get all promotion
        public ActionResult GetAllPromotion(string token, string key, Guid? CustomerId)
        {
            var customerPromotion = new List<CustomerPromotionViewModel>();
            var customerGift = new List<CustomerGiftAPIViewModel>();
            string Error = "";
            bool IsNotFound = false;
            if (token == tokenConst && key == keyConst)
            {
                //1.1. "Chương trình khuyến mãi" dành cho khách hàng
                customerPromotion = _context.CustomerPromotionModel
                                .Where(p => p.EffectToDate != null && p.EffectFromDate != null)
                                .Select(p => new CustomerPromotionViewModel()
                                {
                                    PromotionCode = p.PromotionCode,
                                    ImageUrlTemp = p.ImageUrl,
                                    PromotionName = p.PromotionName,
                                    EffectToDateTemp = p.EffectToDate,
                                    Description = p.Description,
                                }).OrderByDescending(p => p.EffectToDateTemp).ToList();

                if (CustomerId != null)
                {
                    //1.2. "Chương trình quà tặng" riêng cho từng khách hàng
                    customerGift = (from gift in _context.CustomerGiftModel
                                    join detail in _context.CustomerGiftDetailModel on gift.GiftId equals detail.GiftId
                                    join cus in _context.CustomerModel on detail.CustomerId equals cus.CustomerId
                                    where cus.CustomerId == CustomerId
                                    && gift.EffectToDate != null && gift.EffectFromDate != null
                                    orderby gift.EffectToDate descending
                                    select new CustomerGiftAPIViewModel()
                                    {
                                        PromotionCode = gift.GiftCode,
                                        ImageUrlTemp = gift.ImageUrl,
                                        PromotionName = gift.GiftName,
                                        EffectToDateTemp = gift.EffectToDate,
                                        Description = gift.Description,
                                        isRead = detail.isRead
                                    }).ToList();
                }

                if (customerPromotion.Count == 0 || customerGift.Count == 0)
                {
                    Error = LanguageResource.Mobile_NotFound;
                    IsNotFound = true;
                }
            }
            else
            {
                //access denied
                Error = LanguageResource.Account_AccessDenied;
            }
            return Json(new { IsNotFound, customerPromotion, customerGift, Error }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Get Category
        public ActionResult GetCategory(string token, string key)
        {
            string Error = "";
            var result = new List<CategoryMobileDetailViewModel>();
            if (token == tokenConst && key == keyConst)
            {
                // Get all category
                var allCategory = _context.CategoryModel.Where(p => p.Actived == true)
                                                    .Select(p => new CategoryMobileViewModel()
                                                    {
                                                        CategoryId = p.CategoryId,
                                                        ParentCategoryId = p.ParentCategoryId,
                                                        CategoryCode = p.CategoryCode,
                                                        CategoryName = p.CategoryName.ToUpper(),
                                                        OrderIndex = p.OrderIndex,
                                                        ImageUrlTemp = p.ImageUrl,
                                                    })
                                                    .OrderByDescending(p => p.OrderIndex.HasValue).ThenBy(p => p.OrderIndex)
                                                    .ToList();


                // Get all category parent name
                var parentCat = allCategory
                    .Where(d => d.ParentCategoryId == null) // Get parent category (parent not have parent)
                                                            //.Select(d => d.TPGRPNM) // Lấy tên 
                                                            //.Distinct()
                    .ToList();

                // Get all category parent object
                //var parentCat = allCategory.Where(d => parentCatName.Contains(d.TPGRPNM)).OrderBy(o => o.OrderIndex).ToList();



                foreach (var parent in parentCat)
                {
                    var categoryVM = new CategoryMobileDetailViewModel();
                    var parentId = parent.CategoryId;
                    var children = allCategory.Where(d => d.ParentCategoryId != null && d.ParentCategoryId == parentId).Take(6)
                        .ToList();
                    categoryVM.Parent = parent;
                    categoryVM.Children = children;

                    result.Add(categoryVM);
                }
            }
            else
            {
                //access denied
                Error = LanguageResource.Account_AccessDenied;
            }
            return Json(new { result, Error }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCategoryByCode(string ParentCategoryCode, int? orderbyMode, decimal? fromPrice, decimal? toPrice, string token, string key)
        {
            string Error = "";
            var result = new List<CategoryMobileViewModel>();
            string ParentCategoryImageUrl = "";
            if (token == tokenConst && key == keyConst)
            {
                //Get ParentCategoryId
                var parentCategory = _context.CategoryModel.Where(p => p.CategoryCode == ParentCategoryCode)
                                                            .Select(p => new CategoryMobileViewModel()
                                                            {
                                                                CategoryId = p.CategoryId,
                                                                ImageUrlTemp = p.ImageUrl,
                                                                ProductTypeId = p.ProductTypeId
                                                            }).FirstOrDefault();
                if (parentCategory != null)
                {
                    // Get all category by ParentCategoryCode
                    result = _context.CategoryModel.Where(p => p.ParentCategoryId != null && p.ParentCategoryId == parentCategory.CategoryId && p.Actived == true)
                                                        .Select(p => new CategoryMobileViewModel()
                                                        {
                                                            CategoryId = p.CategoryId,
                                                            ParentCategoryId = p.ParentCategoryId,
                                                            CategoryCode = p.CategoryCode,
                                                            CategoryName = p.CategoryName.ToUpper(),
                                                            OrderIndex = p.OrderIndex,
                                                            ImageUrlTemp = p.ImageUrl,
                                                            ProductTypeId = p.ProductTypeId
                                                        }).OrderByDescending(p => p.OrderIndex.HasValue).ThenBy(p => p.OrderIndex).ToList();
                    ParentCategoryImageUrl = parentCategory.ImageUrl;
                }
                else
                {
                    Error = LanguageResource.Mobile_NotFound;
                }
            }
            else
            {
                //access denied
                Error = LanguageResource.Account_AccessDenied;
            }
            return Json(new { result, ParentCategoryImageUrl, Error }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetChildCategory(string token, string key)
        {
            string Error = "";
            var result = new List<ISDSelectItem>();
            if (token == tokenConst && key == keyConst)
            {
                // Get all category by ParentCategoryCode
                result = _context.CategoryModel.Where(p => p.ParentCategoryId != null && p.Actived == true)
                                                    .Select(p => new ISDSelectItem()
                                                    {
                                                        value = p.CategoryId,
                                                        text = p.CategoryName,
                                                    }).OrderBy(p => p.text).ToList();
            }
            else
            {
                //access denied
                Error = LanguageResource.Account_AccessDenied;
            }
            return Json(new { result, Error }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Get Hot Products
        public ActionResult GetHotProducts(string token, string key)
        {
            var result = new List<ProductMobileViewModel>();
            string Error = "";
            if (token == tokenConst && key == keyConst)
            {
                result = GetHotProducts();
            }
            else
            {
                //access denied
                Error = LanguageResource.Account_AccessDenied;
            }
            return Json(new { result, Error }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAllHotProducts(string token, string key)
        {
            var result = new List<ProductMobileViewModel>();
            string Error = "";
            if (token == tokenConst && key == keyConst)
            {
                result = _context.ProductModel
                                            .Where(p => p.isHot == true)
                                            .Select(p => new ProductMobileViewModel()
                                            {
                                                ProductId = p.ProductId,
                                                CategoryId = p.CategoryId,
                                                ProductName = p.ProductName,
                                            }).OrderBy(p => p.ProductName).ToList();

                foreach (var product in result)
                {
                    var image = _context.ImageProductModel.Where(p => p.ProductId == product.ProductId && p.isDefault == true).Select(p => p.ImageUrl).FirstOrDefault();
                    product.ImageUrl = string.Format("{0}/{1}", ConstDomain.DomainImageProduct,
                                                                image != null ? image : ConstImageUrl.noImage);
                }
            }
            else
            {
                //access denied
                Error = LanguageResource.Account_AccessDenied;
            }
            return Json(new { result, Error }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Get Product By Category
        public ActionResult GetProductByCategory(Guid categoryId, int? orderbyMode, decimal? fromPrice, decimal? toPrice, string token, string key)
        {
            string Error = "";
            var result = new List<ProductMobileViewModel>();
            if (token == tokenConst && key == keyConst)
            {

                result = (from p in _context.ProductModel
                              //Loại xe
                          join ca in _context.CategoryModel on p.CategoryId equals ca.CategoryId into caList
                          from cat in caList.DefaultIfEmpty()
                              //Xe ga xe số
                          join pt in _context.ProductTypeModel on cat.ProductTypeId equals pt.ProductTypeId into ptList
                          from producttype in ptList.DefaultIfEmpty()
                          where p.CategoryId == categoryId &&
                                  p.Actived == true
                          orderby p.OrderIndex.HasValue descending, p.OrderIndex
                          select new ProductMobileViewModel()
                          {
                              ProductId = p.ProductId,
                              CategoryId = p.CategoryId,
                              ProductName = p.ProductName,
                              ProductTypeId = producttype.ProductTypeId,
                              ImageUrl = p.ImageUrl,
                          }).ToList();
                foreach (var product in result)
                {
                    //var image = _context.ImageProductModel.Where(p => p.ProductId == product.ProductId && p.isDefault == true).Select(p => p.ImageUrl).FirstOrDefault();
                    //product.ImageUrl = string.Format("{0}/{1}", ConstDomain.DomainImageProduct,
                    //                                            image != null ? image : ConstImageUrl.noImage);
                    product.ImageUrl = string.Format("{0}/{1}", ConstDomain.DomainImageDefaultProduct,
                                                                product.ImageUrl != null ? product.ImageUrl : ConstImageUrl.noImage);
                }

            }
            else
            {
                //access denied
                Error = LanguageResource.Account_AccessDenied;
            }
            return Json(new { result, Error }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Get token
        public string GetAccessToken()
        {
            try
            {
                var username = "tienthuapi@tienthu.vn";
                var password = "Qq>a$5rWTf";
                string encodedUserCredentials = Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes("user:password"));
                string userData = string.Format("username={0}&password={1}&grant_type=password", username, password);

                string url = string.Format("{0}/oauth/token", ConstDomain.DomainAPI);
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.Headers.Add("Authorization", "Basic " + encodedUserCredentials);
                httpWebRequest.Method = "POST";

                StreamWriter requestWriter = new StreamWriter(httpWebRequest.GetRequestStream());
                requestWriter.Write(userData);
                requestWriter.Close();

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    //Get access token from result string
                    return SplitParameter(result, "access_token");
                }
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }
        #endregion Get token

        #region Create Customer
        public ActionResult CreateCustomer(CustomerMobileViewModel model)
        {
            var IsSuccess = false;
            var ErrorCode = "";
            try
            {
                string url = string.Format("{0}/customers/saveandcreateso", ConstDomain.DomainAPI);
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                var access_token = GetAccessToken();
                httpWebRequest.Headers.Add("Authorization", "Bearer " + access_token);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.MediaType = "application/json";
                httpWebRequest.Accept = "application/json";
                httpWebRequest.Method = "POST";

                //get ProvinceCode, DistrictCode
                var province = _context.ProvinceModel.Where(p => p.ProvinceId == model.Province)
                                                     .Select(p => p.ProvinceCode).FirstOrDefault();
                var district = _context.DistrictModel.Where(p => p.DistrictId == model.District)
                                                     .Select(p => p.DistrictCode).FirstOrDefault();
                //LastName, MiddleName, FirstName => CustomerName 
                //Truyền lên ERP: FirstName = CustomerName
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json = new JavaScriptSerializer().Serialize(new
                    {
                        //LastName = model.LastName,
                        //FirstName = model.FirstName,
                        //MiddleName = model.MiddleName,
                        FirstName = model.CustomerName,
                        Gender = model.Gender == null ? "" : (model.Gender == true ? ConstGender.Male_String : ConstGender.Female_String),
                        DateOfBirth = model.DateOfBirth == null ? "" : model.DateOfBirth,
                        CMND = model.CMND == null ? "" : model.CMND,
                        CustomerAddress = model.CustomerAddress == null ? "" : model.CustomerAddress,
                        Province = province == null ? "" : province,
                        District = district == null ? "" : district,
                        Phone = model.Phone,
                        EmailAddress = model.EmailAddress == null ? "" : model.EmailAddress,
                        Fax = model.Fax == null ? "" : model.Fax
                    });
                    streamWriter.Write(json);
                    streamWriter.Flush();
                    streamWriter.Close();
                }



                var result = "";
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();
                }
                //Get status from result string
                ErrorCode = SplitParameter(result, "status");
                //return message
                if (ErrorCode != "false")
                {
                    IsSuccess = true;

                    var data = SplitParameter(result, "data");
                    if (data != "")
                    {
                        model.CustomerCode = data;
                        //insert customer into ISD db
                        //Thread thread = new Thread(new ThreadStart(() => TaskCreateCustomer(model)));
                        //thread.Start();
                        result = string.Format(LanguageResource.Alert_Create_Success, LanguageResource.MasterData_Customer.ToLower());
                    }
                    else
                    {
                        result = LanguageResource.Mobile_ErrorNotFound;
                    }

                }
                else
                {
                    result = SplitParameter(result, "data");
                }
                string FullName = string.Format("{0} {1} {2}", model.LastName, model.MiddleName, model.FirstName);
                if (!string.IsNullOrEmpty(FullName))
                {
                    FullName.Replace("  ", " ");
                    FullName = FullName.Trim();
                }
                return Json(new { IsSuccess = IsSuccess, Message = result, CustomerCode = model.CustomerCode, FullName = FullName }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                if (ex.GetType() == typeof(HttpException))
                {
                    ErrorCode = ((HttpException)ex).GetHttpCode().ToString();
                }
                else
                {
                    ErrorCode = "500";
                }
                IsSuccess = false;
                if (ex.InnerException != null)
                {
                    return Json(new { IsSuccess = IsSuccess, ErrorCode = ErrorCode, Message = ex.InnerException.Message }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { IsSuccess = IsSuccess, ErrorCode = ErrorCode, Message = ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
                }
            }
        }

        //save to db ISD
        public void TaskCreateCustomer(CustomerMobileViewModel viewModel)
        {
            try
            {
                using (EntityDataContext _ctx = new EntityDataContext())
                {
                    CustomerModel model = new CustomerModel();
                    model.CustomerId = Guid.NewGuid();
                    model.CustomerCode = viewModel.CustomerCode;
                    model.LastName = viewModel.LastName;
                    model.FirstName = viewModel.FirstName;
                    model.MiddleName = viewModel.MiddleName;
                    model.Gender = viewModel.Gender;
                    model.DateOfBirth = RepositoryLibrary.VNStringToDateTime(viewModel.DateOfBirth);
                    model.IdentityNumber = viewModel.CMND;
                    model.CustomerAddress = viewModel.CustomerAddress;
                    model.ProvinceId = viewModel.Province;
                    model.DistrictId = viewModel.District;
                    model.Phone = viewModel.Phone;
                    model.EmailAddress = viewModel.EmailAddress;
                    model.Fax = viewModel.Fax;
                    _ctx.Entry(model).State = EntityState.Added;
                    _ctx.SaveChanges();
                }
            }
            catch (Exception ex)
            { }
        }
        #endregion Create Customer

        #region Home
        public ActionResult HomeScreen(Guid? CustomerId, string token, string key)
        {
            //return Json( "", JsonRequestBehavior.AllowGet);

            var banner = new List<BannerMobileViewModel>();
            var hotProducts = new List<ProductMobileViewModel>();
            var promotions = new List<CustomerPromotionViewModel>();
            int notifCount = 0;
            string Error = "";
            if (token == tokenConst && key == keyConst)
            {
                banner = GetBanner();
                hotProducts = GetHotProducts();
                promotions = GetHomePromotions();
                int promotionNotifCount = 0;
                int checkingTimesNotifCount = 0;
                //Nếu là KH thì hiển thị số lượng thông báo chưa đọc.
                if (CustomerId != null)
                {
                    promotionNotifCount = (from gift in _context.CustomerGiftModel
                                           join detail in _context.CustomerGiftDetailModel on gift.GiftId equals detail.GiftId
                                           join cus in _context.CustomerModel on detail.CustomerId equals cus.CustomerId
                                           where cus.CustomerId == CustomerId
                                           && detail.isRead != true
                                           orderby gift.EffectToDate descending
                                           select new CustomerGiftAPIViewModel()
                                           {
                                               PromotionCode = gift.GiftCode,
                                               ImageUrlTemp = gift.ImageUrl,
                                               PromotionName = gift.GiftName,
                                               EffectToDateTemp = gift.EffectToDate,
                                               Description = gift.Description,
                                               isRead = detail.isRead
                                           }).Count();

                    #region Lấy thông tin kiểm tra định kỳ trong DB
                    checkingTimesNotifCount = (from cus in _context.CustomerModel
                                               join noti in _context.CheckingTimesNotificationModel on cus.CustomerCode equals noti.CustomerCode
                                               where cus.CustomerId == CustomerId && noti.isRead != true
                                               select new CheckingTimesNotificationViewModel()
                                               {
                                                   CheckingTimesId = noti.CheckingTimesId,
                                                   CheckingTimesDescription = noti.CheckingTimesDescription,
                                                   isRead = noti.isRead,
                                               }).Count();
                    #endregion Lấy thông tin kiểm tra định kỳ trong DB

                    notifCount = promotionNotifCount + checkingTimesNotifCount;
                }
            }
            else
            {
                //access denied
                Error = LanguageResource.Account_AccessDenied;
            }
            return Json(new { banner, hotProducts, promotions, notifCount, Error }, JsonRequestBehavior.AllowGet);
        }

        private List<BannerMobileViewModel> GetBanner()
        {
            return _context.BannerModel
                .Take(8)
                            .Select(p => new BannerMobileViewModel()
                            {
                                ImageUrlTemp = p.ImageUrl,
                                CreatedTime = p.CreatedTime,
                            }).OrderBy(p => p.CreatedTime).ToList();
        }
        private List<ProductMobileViewModel> GetHotProducts()
        {
            var hotProducts = _context.ProductModel
                                            .Where(p => p.Actived == true && p.isHot == true)
                                            .Take(6)
                                            .Select(p => new ProductMobileViewModel()
                                            {
                                                ProductId = p.ProductId,
                                                CategoryId = p.CategoryId,
                                                ProductName = p.ProductName,
                                                ImageUrl = p.ImageUrl,
                                            }).OrderBy(p => p.ProductName).ToList();

            foreach (var product in hotProducts)
            {
                //var image = _context.ImageProductModel.Where(p => p.ProductId == product.ProductId && p.isDefault == true).Select(p => p.ImageUrl).FirstOrDefault();
                product.ImageUrl = string.Format("{0}/{1}", ConstDomain.DomainImageDefaultProduct,
                                                            product.ImageUrl != null ? product.ImageUrl : ConstImageUrl.noImage);
            }

            return hotProducts;

        }
        private List<CustomerPromotionViewModel> GetHomePromotions()
        {
            return _context.CustomerPromotionModel
                            .Take(6)
                            .Select(p => new CustomerPromotionViewModel()
                            {
                                PromotionCode = p.PromotionCode,
                                ImageUrlTemp = p.ImageUrl,
                                PromotionName = p.PromotionName,
                                Description = p.Description,
                            }).ToList();
        }
        #endregion

        #region Get Province
        public ActionResult GetProvince(string token, string key)
        {
            var result = new List<ISDSelectItem>();
            string Error = "";
            if (token == tokenConst && key == keyConst)
            {
                result = _context.ProvinceModel.Select(p => new ISDSelectItem()
                {
                    value = p.ProvinceId,
                    text = p.ProvinceName,
                    area = p.Area,
                }).OrderBy(p => p.area).ThenBy(p => p.text).ToList();
            }
            else
            {
                //access denied
                Error = LanguageResource.Account_AccessDenied;
            }
            return Json(new { result, Error }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Sign Up
        public ActionResult SignUp(string Phone, string Password, string token, string key)
        {
            try
            {
                if (token == tokenConst && key == keyConst)
                {
                    //Username is exist 
                    var usernameIsExist = (from acc in _context.AccountModel
                                           join customer in _context.CustomerModel on acc.AccountId equals customer.AccountId
                                           where acc.UserName == Phone &&
                                           customer.SMSConfirm == true
                                           select acc
                                          ).FirstOrDefault();

                    //Bắt lỗi user có tồn tại trong hệ thống
                    if (usernameIsExist != null)
                    {
                        return Json(new { IsSuccess = false, Message = LanguageResource.Mobile_UserIsExist }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        //get OTP code from TienThu API - Register
                        MobileRegisterViewModel resultRegister = SendSMS(Phone, 2);
                        if (resultRegister.Result)
                        {
                            var usernameIsExistNotConfirm = (from acc in _context.AccountModel
                                                             join customer in _context.CustomerModel on acc.AccountId equals customer.AccountId
                                                             where acc.UserName == Phone &&
                                                             customer.SMSConfirm == false
                                                             select acc
                                          ).FirstOrDefault();

                            if (usernameIsExistNotConfirm == null)
                            {
                                #region Add Account Model
                                AccountModel model = new AccountModel();
                                model.AccountId = Guid.NewGuid();
                                model.UserName = Phone;
                                model.Password = RepositoryLibrary.GetMd5Sum(Password); ;
                                model.Actived = false;
                                var roleCustomer = _context.RolesModel.Where(p => p.RolesCode == ConstRoles.Customer).FirstOrDefault();
                                model.RolesModel.Add(roleCustomer);
                                _context.Entry(model).State = EntityState.Added;
                                #endregion

                                #region Add Customer infomation
                                CustomerModel cusModel = new CustomerModel();
                                cusModel.CustomerId = Guid.NewGuid();
                                cusModel.AccountId = model.AccountId;
                                cusModel.Phone = Phone;
                                cusModel.FirstName = resultRegister.FullName;
                                cusModel.SMSConfirm = false;
                                cusModel.OTPCode = resultRegister.OTPCode;
                                cusModel.OTPCodeExp = DateTime.Now.AddHours(1);
                                cusModel.CustomerCode = resultRegister.CustomerCode;
                                _context.Entry(cusModel).State = EntityState.Added;
                                _context.SaveChanges();
                                #endregion
                            }
                            else
                            {
                                //gửi lại
                                //Update password

                                var accModel = (from acc in _context.AccountModel
                                                where acc.UserName == Phone
                                                select acc).FirstOrDefault();
                                accModel.Password = RepositoryLibrary.GetMd5Sum(Password);
                                _context.Entry(accModel).State = EntityState.Modified;
                                //Update lại OTP
                                var mcModel = (from acc in _context.AccountModel
                                               join customer in _context.CustomerModel on acc.AccountId equals customer.AccountId
                                               where acc.UserName == Phone
                                               select customer).FirstOrDefault();
                                mcModel.OTPCode = resultRegister.OTPCode;
                                mcModel.OTPCodeExp = DateTime.Now.AddHours(1);
                                _context.Entry(mcModel).State = EntityState.Modified;
                                _context.SaveChanges();
                            }

                            return Json(new { IsSuccess = true, Message = LanguageResource.SignUp_IsSuccess }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json(new { IsSuccess = false, Message = resultRegister.ErrorMessage }, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
                else
                {
                    return Json(new { IsSuccess = false, Message = LanguageResource.Account_AccessDenied }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    return Json(new { IsSuccess = false, Message = ex.InnerException.Message }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { IsSuccess = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
                }
            }
        }
        //Get OTP code from TienThu API
        public MobileRegisterViewModel SendSMS(string Phone, int isforget)
        {
            var resultRegister = new MobileRegisterViewModel();
            resultRegister.Result = false;
            try
            {
                //request tới api của tiến thu
                string url = string.Format("{0}/customers/register?phonenumber={1}&isforget={2}",
                                        ConstDomain.DomainAPI,
                                        Phone,
                                        isforget);
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                var access_token = GetAccessToken();
                httpWebRequest.Headers.Add("Authorization", "Bearer " + access_token);
                httpWebRequest.Method = "GET";
                //Kết quả trả về
                var result = "";
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();
                }
                //Get status from result string
                var status = SplitParameter(result, "statusCode");

                //return message
                if (status == "200")
                {
                    var data = (JObject)JsonConvert.DeserializeObject(result);
                    var list = data["data"].Children();

                    if (list.Count() > 0)
                    {
                        var firstData = list.First().ToString();
                        var part = firstData.Split(new string[] { ";" }, StringSplitOptions.None);
                        resultRegister.CustomerCode = part[0].ToString();
                        resultRegister.FullName = part[1].ToString();
                        resultRegister.OTPCode = part[2].ToString();
                        resultRegister.Result = true;
                        resultRegister.ErrorMessage = string.Empty;
                    }
                    else
                    {
                        resultRegister.OTPCode = string.Empty;
                        resultRegister.ErrorMessage = LanguageResource.Mobile_Register_PhoneNotFound;
                    }
                }
                else
                {
                    resultRegister.OTPCode = string.Empty;
                    resultRegister.ErrorMessage = SplitParameter(result, "statusText");
                }
            }
            catch (Exception ex)
            {
                resultRegister.OTPCode = string.Empty;
                resultRegister.ErrorMessage = ex.Message;
            }
            return resultRegister;
        }
        //SignUpAuthorized
        public ActionResult SignUpAuthorized(string Phone, string OTPCode, string token, string key)
        {
            var IsSuccess = false;
            var Message = "";
            try
            {
                if (token == tokenConst && key == keyConst)
                {
                    #region Account
                    var accModel = (from acc in _context.AccountModel
                                    where acc.UserName == Phone
                                    select acc).FirstOrDefault();
                    if (accModel != null)
                    {
                        accModel.Actived = true;
                        _context.Entry(accModel).State = EntityState.Modified;
                    }
                    #endregion

                    #region customer Info
                    var customer = (from acc in _context.AccountModel
                                    join cus in _context.CustomerModel on acc.AccountId equals cus.AccountId
                                    where acc.UserName == Phone && cus.OTPCode == OTPCode
                                    select cus).FirstOrDefault();
                    if (accModel != null && customer != null)
                    {
                        customer.SMSConfirm = true;
                        _context.Entry(customer).State = EntityState.Modified;
                        _context.SaveChanges();

                        IsSuccess = true;
                        Message = LanguageResource.SignUp_IsAuthorized;
                    }
                    else
                    {
                        IsSuccess = false;
                        Message = LanguageResource.SiginUp_OTPFalse;
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    return Json(new { IsSuccess = IsSuccess, Message = ex.InnerException.Message }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { IsSuccess = IsSuccess, Message = ex.Message }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { IsSuccess = IsSuccess, Message = Message, Phone = Phone }, JsonRequestBehavior.AllowGet);
        }
        #endregion Sign Up

        #region Forgot password
        public ActionResult ForgotPassword(string Phone, string token, string key)
        {
            try
            {
                if (token == tokenConst && key == keyConst)
                {
                    //Username is exist 
                    var forgotCustomerUser = (from acc in _context.AccountModel
                                              join customer in _context.CustomerModel on acc.AccountId equals customer.AccountId
                                              where acc.UserName == Phone &&
                                                     customer.SMSConfirm == true
                                              select customer
                                                ).FirstOrDefault();

                    //Bắt lỗi user không tồn tại trong hệ thống
                    if (forgotCustomerUser == null)
                    {
                        return Json(new { IsSuccess = false, Message = LanguageResource.Mobile_UserIsNotExist }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        //get OTP code from TienThu API
                        MobileRegisterViewModel SMSResult = SendSMS(Phone, 0);
                        if (SMSResult.Result)
                        {
                            //Gửi SMS thành công
                            //1. Đánh dấu đang phục hồi password
                            //2. Cập nhật OTP Code phục hồi password
                            //3. Gửi thông báo đã gửi tin nhắn phục hồi cho khách hàng
                            forgotCustomerUser.OTPCode = SMSResult.OTPCode;
                            forgotCustomerUser.OTPCodeExp = DateTime.Now.AddHours(1);
                            _context.Entry(forgotCustomerUser).State = EntityState.Modified;
                            _context.SaveChanges();
                            return Json(new { IsSuccess = true, Message = LanguageResource.ForgotPassword_SMSHasBeenSent }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            //Gửi SMS Thất bại
                            //1. Thông báo không thể gửi tin nhắn.
                            return Json(new { IsSuccess = false, Message = SMSResult.ErrorMessage }, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
                else
                {
                    return Json(new { IsSuccess = false, Message = LanguageResource.Account_AccessDenied }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    return Json(new { IsSuccess = false, Message = ex.InnerException.Message }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { IsSuccess = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
                }
            }
        }

        public ActionResult ForgotChangePassword(string Phone, string newPassword, string OTPCode, string token, string key)
        {
            try
            {
                if (token == tokenConst && key == keyConst)
                {
                    //find customer user 
                    var forgotCustomerUser = (from acc in _context.AccountModel
                                              join customer in _context.CustomerModel on acc.AccountId equals customer.AccountId
                                              where acc.UserName == Phone &&
                                                     customer.SMSConfirm == true
                                              select customer
                                        ).FirstOrDefault();
                    if (forgotCustomerUser == null)
                    {
                        return Json(new { IsSuccess = false, Message = LanguageResource.Mobile_UserIsNotExist }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        //1. Kiểm tra "OTPCode" và "OTPCodeExp" của mã OTP được gửi trước đó
                        //2. Cập nhật mật khẩu mới cho user
                        if (forgotCustomerUser.OTPCode == OTPCode && DateTime.Now <= forgotCustomerUser.OTPCodeExp)
                        {
                            var account = _context.AccountModel.Find(forgotCustomerUser.AccountId);
                            if (account != null)
                            {
                                account.Password = RepositoryLibrary.GetMd5Sum(newPassword);
                                _context.Entry(account).State = EntityState.Modified;
                                _context.SaveChanges();
                                return Json(new { IsSuccess = true, Message = LanguageResource.ForgotPassword_PasswordHasBeenChanged }, JsonRequestBehavior.AllowGet);
                            }
                            else
                            {
                                //Không tìm thấy tài khoản để cập nhật
                                return Json(new { IsSuccess = false, Message = LanguageResource.ForgotPassword_AccountNotFound }, JsonRequestBehavior.AllowGet);
                            }
                        }
                        else
                        {
                            //Sai mã OTP hoặc mã OTP đã hết hạn
                            return Json(new { IsSuccess = false, Message = LanguageResource.ForgotPassword_OTP_WrongOrExpired }, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
                else
                {
                    return Json(new { IsSuccess = false, Message = LanguageResource.Account_AccessDenied }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    return Json(new { IsSuccess = false, Message = ex.InnerException.Message }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { IsSuccess = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
                }
            }
        }

        #endregion

        #region Get District By Province
        public ActionResult GetDistrictBy(Guid ProvinceId, string token, string key)
        {
            var result = new List<ISDSelectItem>();
            string Error = "";
            if (token == tokenConst && key == keyConst)
            {
                result = _context.DistrictModel.Where(p => p.ProvinceId == ProvinceId).Select(p => new ISDSelectItem()
                {
                    value = p.DistrictId,
                    text = p.Appellation + " " + p.DistrictName,
                }).OrderBy(p => p.text).ToList();
            }
            else
            {
                //access denied
                Error = LanguageResource.Account_AccessDenied;
            }
            return Json(new { result, Error }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Search Customer
        public ActionResult SearchCustomers(string searchtext, string searchtype)
        {
            var IsSuccess = false;
            var ErrorCode = "";
            var ErrorText = "";
            List<SearchCustomerViewModel> searchList = new List<SearchCustomerViewModel>();
            try
            {
                string url = string.Format("{0}/customers/searchcustomers?searchtext={1}&searchtype={2}",
                                        ConstDomain.DomainAPI,
                                        searchtext,
                                        searchtype);
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                var access_token = GetAccessToken();
                httpWebRequest.Headers.Add("Authorization", "Bearer " + access_token);
                httpWebRequest.Method = "GET";

                var result = "";
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();
                }
                //Get status from result string
                ErrorCode = SplitParameter(result, "statusCode");
                //return message
                if (ErrorCode == "200")
                {
                    IsSuccess = true;
                    var data = (JObject)JsonConvert.DeserializeObject(result);
                    var list = data["data"].Children();

                    if (list.Count() > 0)
                    {
                        result = "";
                        foreach (var item in list)
                        {
                            string value = item.ToObject<string>();
                            if (value != "")
                            {
                                var part = value.Split(new string[] { ";" }, StringSplitOptions.None);
                                SearchCustomerViewModel viewModel = new SearchCustomerViewModel();
                                //CustomerCode
                                viewModel.CustomerCode = part[0].ToString();
                                //FullName
                                viewModel.FullName = part[1].ToString();
                                //Address
                                var address = part[2].ToString();
                                if (address != "")
                                {
                                    var addressArray = address.Split(new string[] { "\n" }, StringSplitOptions.None);
                                    var Street = addressArray[0].ToString();
                                    //district
                                    var District = "";
                                    if (addressArray.Length > 1)
                                    {
                                        var districtTemp = addressArray[1].ToString();
                                        District = _context.DistrictModel.Where(p => p.DistrictCode == districtTemp)
                                                           .Select(p => ", " + p.Appellation + " " + p.DistrictName)
                                                           .FirstOrDefault();
                                    }
                                    //province
                                    var Province = "";
                                    if (addressArray.Length > 2)
                                    {
                                        var provinceTemp = addressArray[2].ToString();
                                        Province = _context.ProvinceModel.Where(p => p.ProvinceCode == provinceTemp)
                                                           .Select(p => ", " + p.ProvinceName).FirstOrDefault();
                                    }
                                    viewModel.CustomerAddress = string.Format("{0}{1}{2}", Street, District, Province);
                                }
                                //Phone
                                viewModel.Phone = part[3].ToString();
                                //Plate
                                viewModel.Plate = part[4].ToString();
                                //Times
                                viewModel.Times = part[5].ToString();
                                searchList.Add(viewModel);
                            }
                        }
                    }
                    else
                    {
                        ErrorText = LanguageResource.SearchCustomer_NotFound;
                    }

                }
                else
                {
                    ErrorText = SplitParameter(result, "statusText");
                }
                return Json(new { IsSuccess = IsSuccess, ErrorCode = ErrorCode, Message = ErrorText, Data = searchList }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                if (ex.GetType() == typeof(HttpException))
                {
                    ErrorCode = ((HttpException)ex).GetHttpCode().ToString();
                }
                else
                {
                    ErrorCode = "500";
                }
                IsSuccess = false;
                if (ex.InnerException != null)
                {
                    return Json(new { IsSuccess = IsSuccess, ErrorCode = ErrorCode, Message = ex.InnerException.Message }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { IsSuccess = IsSuccess, ErrorCode = ErrorCode, Message = ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
                }
            }
        }
        #endregion Search Customer

        #region Create Prospect
        public ActionResult CreateProspect(ProspectCustomerViewModel model)
        {
            var IsSuccess = false;
            var result = "";
            var statusProspect = "";
            var statusOpportunities = "";
            var ErrorCode = "";
            try
            {
                var access_token = GetAccessToken();
                //get ProvinceCode, DistrictCode
                var province = _context.ProvinceModel.Where(p => p.ProvinceId == model.Province)
                                                     .Select(p => p.ProvinceCode).FirstOrDefault();
                var district = _context.DistrictModel.Where(p => p.DistrictId == model.District)
                                                     .Select(p => p.DistrictCode).FirstOrDefault();

                //Tạo "Cơ hội bán hàng": createopportunities
                //Tạo "Khách hàng tiềm năng": createprospect
                if (model.isOpportunities == true)
                {
                    #region Create Opportunities
                    string urlOpportunities = string.Format("{0}/customers/createopportunities", ConstDomain.DomainAPI);
                    var httpWebRequestOpportunities = (HttpWebRequest)WebRequest.Create(urlOpportunities);
                    httpWebRequestOpportunities.Headers.Add("Authorization", "Bearer " + access_token);
                    httpWebRequestOpportunities.ContentType = "application/json";
                    httpWebRequestOpportunities.MediaType = "application/json";
                    httpWebRequestOpportunities.Accept = "application/json";
                    httpWebRequestOpportunities.Method = "POST";

                    //opportunities
                    var prognosis = _context.PrognosisModel.Where(p => p.PrognosisId == model.Prognosis)
                                                           .Select(p => p.PrognosisDescription).FirstOrDefault();
                    var saleUnit = _context.SaleUnitModel.Where(p => p.SaleUnitId == model.SalesUnit)
                                                           .Select(p => p.SaleUnitDescription).FirstOrDefault();
                    var saleProcess = _context.SaleProcessModel.Where(p => p.SaleProcessId == model.SalesProcess)
                                                           .Select(p => p.SaleProcessDescription).FirstOrDefault();
                    var source = _context.SourceModel.Where(p => p.SourceId == model.SourceId)
                                                           .Select(p => p.SourceDescription).FirstOrDefault();

                    using (var streamWriter = new StreamWriter(httpWebRequestOpportunities.GetRequestStream()))
                    {
                        string json = new JavaScriptSerializer().Serialize(new
                        {
                            //LastName = model.LastName,
                            //FirstName = model.FirstName,
                            //MiddleName = model.MiddleName,
                            FirstName = model.CustomerName,
                            MailAddress = model.EmailAddress == null ? "" : model.EmailAddress,
                            Gender = model.Gender == null ? "" : (model.Gender == true ? ConstGender.Male_String : ConstGender.Female_String),
                            DateOfBirth = model.DateOfBirth == null ? "" : model.DateOfBirth,
                            CMND = model.CMND == null ? "" : model.CMND,
                            CustomerAddress = model.CustomerAddress == null ? "" : model.CustomerAddress,
                            Province = province == null ? "" : province,
                            District = district == null ? "" : district,
                            Phone = model.Phone,
                            EmailAddress = model.EmailAddress == null ? "" : model.EmailAddress,
                            Subject = model.Subject,
                            HcmPersonnelNumberId = model.HcmPersonnelNumberId,
                            Name = model.Name == null ? "" : model.Name,
                            Status = model.Status == null ? "" : model.Status,
                            Owner = model.Owner == null ? "" : model.Owner,
                            Prognosis = prognosis == null ? "" : prognosis,
                            Probability = model.Probability == null ? "" : model.Probability,
                            SalesUnit = saleUnit == null ? "" : saleUnit,
                            SalesProcess = saleProcess == null ? "" : saleProcess,
                            EstimatedRevenue = model.EstimatedRevenue == null ? "" : model.EstimatedRevenue,
                            SourceId = source == null ? "" : source
                        });
                        streamWriter.Write(json);
                        streamWriter.Flush();
                        streamWriter.Close();
                    }
                    var resultOpportunities = "";
                    var httpResponseOpportunities = (HttpWebResponse)httpWebRequestOpportunities.GetResponse();
                    using (var streamReader = new StreamReader(httpResponseOpportunities.GetResponseStream()))
                    {
                        resultOpportunities = streamReader.ReadToEnd();
                    }
                    statusOpportunities = SplitParameter(resultOpportunities, "status");

                    if (statusOpportunities == "true")
                    {
                        IsSuccess = true;
                        result = string.Format(LanguageResource.Alert_Create_Success, LanguageResource.Opportunities);
                    }
                    else
                    {
                        //result = LanguageResource.ProspectCustomer_OpportunitiesError;
                        result = SplitParameter(resultOpportunities, "data");
                    }
                    #endregion Create Opportunities
                }
                else
                {
                    #region Create Prospect
                    string url = string.Format("{0}/customers/createprospect", ConstDomain.DomainAPI);
                    var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                    httpWebRequest.Headers.Add("Authorization", "Bearer " + access_token);
                    httpWebRequest.ContentType = "application/json";
                    httpWebRequest.MediaType = "application/json";
                    httpWebRequest.Accept = "application/json";
                    httpWebRequest.Method = "POST";

                    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                    {
                        string json = new JavaScriptSerializer().Serialize(new
                        {
                            //LastName = model.LastName,
                            //FirstName = model.FirstName,
                            //MiddleName = model.MiddleName,
                            FirstName = model.CustomerName,
                            MailAddress = model.EmailAddress == null ? "" : model.EmailAddress,
                            Gender = model.Gender == null ? "" : (model.Gender == true ? ConstGender.Male_String : ConstGender.Female_String),
                            DateOfBirth = model.DateOfBirth == null ? "" : model.DateOfBirth,
                            CMND = model.CMND == null ? "" : model.CMND,
                            CustomerAddress = model.CustomerAddress == null ? "" : model.CustomerAddress,
                            Province = province == null ? "" : province,
                            District = district == null ? "" : district,
                            Phone = model.Phone,
                            EmailAddress = model.EmailAddress == null ? "" : model.EmailAddress,
                        });
                        streamWriter.Write(json);
                        streamWriter.Flush();
                        streamWriter.Close();
                    }
                    var resultProspect = "";
                    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        resultProspect = streamReader.ReadToEnd();
                    }
                    statusProspect = SplitParameter(resultProspect, "status");

                    if (statusProspect == "true")
                    {
                        IsSuccess = true;
                        result = string.Format(LanguageResource.Alert_Create_Success, LanguageResource.ProspectCustomer);
                    }
                    else
                    {
                        //result = LanguageResource.ProspectCustomer_ProspectError;
                        result = SplitParameter(resultProspect, "data");
                    }

                    #endregion Create Prospect
                }
                return Json(new { IsSuccess = IsSuccess, Message = result }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                if (ex.GetType() == typeof(HttpException))
                {
                    ErrorCode = ((HttpException)ex).GetHttpCode().ToString();
                }
                else
                {
                    ErrorCode = "500";
                }
                IsSuccess = false;
                if (ex.InnerException != null)
                {
                    return Json(new { IsSuccess = IsSuccess, ErrorCode = ErrorCode, Message = ex.InnerException.Message }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { IsSuccess = IsSuccess, ErrorCode = ErrorCode, Message = ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
                }
            }
        }
        #endregion Create Prospect

        #region Create Sales Order
        public ActionResult CreateSalesOrder(SaleOrderViewModel model)
        {
            var IsSuccess = false;
            var ErrorCode = "";
            try
            {
                string url = string.Format("{0}/customers/createsalesorder", ConstDomain.DomainAPI);
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                var access_token = GetAccessToken();
                httpWebRequest.Headers.Add("Authorization", "Bearer " + access_token);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.MediaType = "application/json";
                httpWebRequest.Accept = "application/json";
                httpWebRequest.Method = "POST";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json = new JavaScriptSerializer().Serialize(new
                    {
                        CustAccount = model.CustAccount,
                        DimensionValue = model.DimensionValue == null ? "" : model.DimensionValue,
                        DlvDate = model.DlvDate == null ? "" : model.DlvDate,
                        CustRef = model.Note == null ? "" : model.Note,
                        PersonnalNumberId = model.PersonnalNumberId == null ? "" : model.PersonnalNumberId,
                        SubTotal = model.SubTotal == null ? 0 : model.SubTotal,
                        Total = model.Total == null ? 0 : model.Total
                    });
                    streamWriter.Write(json);
                    streamWriter.Flush();
                    streamWriter.Close();
                }

                var result = "";
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();
                }
                //Get status from result string
                var statusCode = SplitParameter(result, "statusCode");
                //return message
                if (statusCode == "200")
                {
                    //insert sale order into ISD db
                    var SaleOrderCode = SplitParameter(result, "data");
                    model.SaleOrderCode = SaleOrderCode;
                    Thread thread = new Thread(new ThreadStart(() => TaskCreateSaleOrder(model)));
                    thread.Start();

                    IsSuccess = true;
                    result = string.Format(LanguageResource.Alert_Create_Success, LanguageResource.SaleOrder.ToLower());
                }
                else
                {
                    result = SplitParameter(result, "statusText");
                }
                return Json(new { IsSuccess = IsSuccess, Message = result }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                if (ex.GetType() == typeof(HttpException))
                {
                    ErrorCode = ((HttpException)ex).GetHttpCode().ToString();
                }
                else
                {
                    ErrorCode = "500";
                }
                IsSuccess = false;
                if (ex.InnerException != null)
                {
                    return Json(new { IsSuccess = IsSuccess, ErrorCode = ErrorCode, Message = ex.InnerException.Message }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { IsSuccess = IsSuccess, ErrorCode = ErrorCode, Message = ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
                }
            }
        }

        //save to db ISD
        public void TaskCreateSaleOrder(SaleOrderViewModel viewModel)
        {
            try
            {
                using (EntityDataContext _ctx = new EntityDataContext())
                {
                    SaleOrderModel model = new SaleOrderModel();
                    model.SaleOrderId = Guid.NewGuid();
                    model.SaleOrderCode = viewModel.SaleOrderCode;
                    model.CustomerCode = viewModel.CustAccount;
                    model.CustomerName = viewModel.CustomerName;
                    model.Category = viewModel.Category;
                    model.Color = viewModel.Color;
                    model.SerialNumber = viewModel.SerialNumber;
                    model.EngineNumber = viewModel.EngineNumber;
                    model.isPlateService = viewModel.isPlateService;
                    model.PlateCost = viewModel.PlateCost;
                    model.DownPayment = viewModel.DownPayment;
                    model.BalanceDue = viewModel.BalanceDue;
                    model.PersonnalNumberId = viewModel.PersonnalNumberId;
                    model.Note = viewModel.Note;
                    //additional field
                    model.CreatedDate = DateTime.Now;

                    _ctx.Entry(model).State = EntityState.Added;
                    _ctx.SaveChanges();
                }
            }
            catch (Exception ex)
            { }
        }
        #endregion Create Sales Order

        #region Create Plate
        public ActionResult CreatePlate(PlateMobileViewModel model)
        {
            var IsSuccess = false;
            var ErrorCode = "";
            try
            {
                string url = string.Format("{0}/customers/createplate", ConstDomain.DomainAPI);
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                var access_token = GetAccessToken();
                httpWebRequest.Headers.Add("Authorization", "Bearer " + access_token);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.MediaType = "application/json";
                httpWebRequest.Accept = "application/json";
                httpWebRequest.Method = "POST";

                //loại xe
                var categoryCode = _context.CategoryModel.Where(p => p.CategoryId == model.CategoryName).Select(p => p.CategoryCode).FirstOrDefault();
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json = new JavaScriptSerializer().Serialize(new
                    {
                        LicensePlate = model.LicensePlate,
                        CategoryName = categoryCode == null ? "" : categoryCode,
                        InvoceDate = model.InvoceDate,
                        SerialNumber = model.SerialNumber == null ? "" : model.SerialNumber,
                        EngineNumber = model.EngineNumber == null ? "" : model.EngineNumber,
                        //Barcode = model.Barcode == null ? "" : model.Barcode,
                        CustomerName = model.CustomerName == null ? "" : model.CustomerName,
                        CustomerPhone = model.CustomerPhone == null ? "" : model.CustomerPhone,
                        UserId = model.UserId == null ? "" : model.UserId
                    });
                    streamWriter.Write(json);
                    streamWriter.Flush();
                    streamWriter.Close();
                }

                var result = "";
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();
                }
                //Get status from result string
                var status = SplitParameter(result, "status");
                //return message
                if (status == "true")
                {
                    IsSuccess = true;
                    result = string.Format(LanguageResource.Alert_Create_Success, LanguageResource.VehicleInformation);
                }
                else
                {
                    result = SplitParameter(result, "data");
                }
                return Json(new { IsSuccess = IsSuccess, Message = result }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                if (ex.GetType() == typeof(HttpException))
                {
                    ErrorCode = ((HttpException)ex).GetHttpCode().ToString();
                }
                else
                {
                    ErrorCode = "500";
                }
                IsSuccess = false;
                if (ex.InnerException != null)
                {
                    return Json(new { IsSuccess = IsSuccess, ErrorCode = ErrorCode, Message = ex.InnerException.Message }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { IsSuccess = IsSuccess, ErrorCode = ErrorCode, Message = ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
                }
            }
        }
        #endregion Create Plate

        #region Create Service Order
        public ActionResult CreateServiceOrder(ServiceOrderViewModel model)
        {
            var IsSuccess = false;
            var ErrorCode = "";
            try
            {
                string url = string.Format("{0}/customers/createserviceorder", ConstDomain.DomainAPI);
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                var access_token = GetAccessToken();
                httpWebRequest.Headers.Add("Authorization", "Bearer " + access_token);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.MediaType = "application/json";
                httpWebRequest.Accept = "application/json";
                httpWebRequest.Method = "POST";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json = new JavaScriptSerializer().Serialize(new
                    {
                        CustAccount = model.CustAccount,
                        PlateId = model.PlateId == null ? "" : model.PlateId,
                        CurrentKm = model.CurrentKm == null ? 0 : model.CurrentKm,
                        ServicePool = model.ServicePool == null ? "" : model.ServicePool,
                        DimensionStore = model.DimensionStore == null ? "" : model.DimensionStore,
                        CustRef = model.Note == null ? "" : model.Note,
                        PersonnelNumberId = model.PersonnelNumberId == null ? "" : model.PersonnelNumberId,
                        EngineId = model.EngineId == null ? "" : model.EngineId,
                        InventSerialId = model.InventSerialId == null ? "" : model.InventSerialId,
                    });
                    streamWriter.Write(json);
                    streamWriter.Flush();
                    streamWriter.Close();
                }

                var result = "";
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();
                }
                //Get status from result string
                var statusCode = SplitParameter(result, "statusCode");
                //return message
                if (statusCode == "200")
                {
                    //Cập nhật bảng "BookingModel" khi đã tạo phiếu dịch vụ thành công
                    var booking = _context.BookingModel.Where(p => p.BookingCode == model.BookingCode).FirstOrDefault();
                    if (booking != null)
                    {
                        booking.IsCreatedServiceOrder = true;
                        _context.Entry(booking).State = EntityState.Modified;
                        _context.SaveChanges();

                    }
                    //insert service order into ISD db
                    var ServiceOrderCode = SplitParameter(result, "data");
                    model.ServiceOrderCode = ServiceOrderCode;
                    Thread thread = new Thread(new ThreadStart(() => TaskCreateServiceOrder(model)));
                    thread.Start();

                    IsSuccess = true;
                    result = string.Format(LanguageResource.Alert_Create_Success, LanguageResource.ServiceOrder.ToLower());
                }
                else
                {
                    result = SplitParameter(result, "statusText");
                }
                return Json(new { IsSuccess = IsSuccess, Message = result }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                if (ex.GetType() == typeof(HttpException))
                {
                    ErrorCode = ((HttpException)ex).GetHttpCode().ToString();
                }
                else
                {
                    ErrorCode = "500";
                }
                IsSuccess = false;
                if (ex.InnerException != null)
                {
                    return Json(new { IsSuccess = IsSuccess, ErrorCode = ErrorCode, Message = ex.InnerException.Message }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { IsSuccess = IsSuccess, ErrorCode = ErrorCode, Message = ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
                }
            }
        }

        //save to db ISD
        public void TaskCreateServiceOrder(ServiceOrderViewModel viewModel)
        {
            try
            {
                using (EntityDataContext _ctx = new EntityDataContext())
                {
                    ServiceOrderModel model = new ServiceOrderModel();
                    model.ServiceOrderId = Guid.NewGuid();
                    model.ServiceOrderCode = viewModel.ServiceOrderCode;
                    model.CustomerCode = viewModel.CustAccount;
                    model.CustomerName = viewModel.CustomerName;
                    model.CustomerPhone = viewModel.CustomerPhone;
                    model.LicensePlate = viewModel.PlateId;
                    model.Category = viewModel.Category;
                    model.SerialNumber = viewModel.InventSerialId;
                    model.EngineNumber = viewModel.EngineId;
                    model.ServicePool = viewModel.ServicePool.ToString();
                    model.CurrentKilometers = viewModel.CurrentKm;
                    model.PersonnalNumberId = viewModel.PersonnelNumberId;
                    model.Note = viewModel.Note;
                    //additional field
                    model.CreatedDate = DateTime.Now;

                    _ctx.Entry(model).State = EntityState.Added;
                    _ctx.SaveChanges();
                }
            }
            catch (Exception ex)
            { }
        }
        #endregion Create Service Order

        #region Search Order header
        public ActionResult SearchSaleOrderHeader(string custaccount)
        {
            var IsSuccess = false;
            var ErrorCode = "";
            List<SearchSaleOrderHeaderViewModel> searchList = new List<SearchSaleOrderHeaderViewModel>();
            try
            {
                string url = string.Format("{0}/customers/searchorderheader?custaccount={1}",
                                        ConstDomain.DomainAPI,
                                        custaccount);
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                var access_token = GetAccessToken();
                httpWebRequest.Headers.Add("Authorization", "Bearer " + access_token);
                httpWebRequest.Method = "GET";

                var result = "";
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();
                }
                //Get status from result string
                var status = SplitParameter(result, "statusCode");

                //return message
                if (status == "200")
                {
                    IsSuccess = true;
                    var data = (JObject)JsonConvert.DeserializeObject(result);
                    var list = data["data"].Children();

                    if (list.Count() > 0)
                    {
                        result = "";
                        foreach (var item in list)
                        {
                            string value = item.ToObject<string>();
                            if (value != "")
                            {
                                var part = value.Split(new string[] { ";" }, StringSplitOptions.None);
                                SearchSaleOrderHeaderViewModel viewModel = new SearchSaleOrderHeaderViewModel();
                                //CustomerCode
                                viewModel.CustomerCode = part[0].ToString();
                                //FullName
                                viewModel.FullName = part[1].ToString();
                                //SaleOrderCode
                                viewModel.SaleOrderCode = part[2].ToString();
                                //CreatedDate
                                viewModel.CreatedDate = part[4].ToString();
                                //Total
                                viewModel.TotalTmp = part[5].ToString();
                                //StoreName
                                viewModel.StoreName = part[6].ToString();
                                //SaleOrderType
                                viewModel.SaleOrderType = part[7].ToString();
                                searchList.Add(viewModel);
                            }
                        }
                    }
                    else
                    {
                        result = LanguageResource.SearchSaleOrder_NotFound;
                    }
                }
                else
                {
                    result = SplitParameter(result, "statusText");
                }
                return Json(new { IsSuccess = IsSuccess, ErrorCode = status, Message = result, Data = searchList.OrderByDescending(p => p.CreatedDate) }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                if (ex.GetType() == typeof(HttpException))
                {
                    ErrorCode = ((HttpException)ex).GetHttpCode().ToString();
                }
                else
                {
                    ErrorCode = "500";
                }
                IsSuccess = false;
                if (ex.InnerException != null)
                {
                    return Json(new { IsSuccess = IsSuccess, ErrorCode = ErrorCode, Message = ex.InnerException.Message }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { IsSuccess = IsSuccess, ErrorCode = ErrorCode, Message = ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
                }
            }
        }
        #endregion Search Order header

        #region Search Order detail
        public ActionResult SearchSaleOrderDetail(string custaccount, string voucher, string transdateDate, string transtype)
        {
            var IsSuccess = false;
            var ErrorCode = "";
            DateTime? TransdateDate = RepositoryLibrary.VNStringToDateTime(transdateDate);
            List<SearchSaleOrderDetailViewModel> searchList = new List<SearchSaleOrderDetailViewModel>();
            try
            {
                var transdate = "";
                if (TransdateDate.HasValue)
                {
                    transdate = TransdateDate.Value.ToString("yyyy-MM-dd'T'HH:mm:ss");
                }
                string url = string.Format("{0}/customers/searchorderdetails?custaccount={1}&voucher={2}&transdate={3}&transtype={4}",
                                        ConstDomain.DomainAPI,
                                        custaccount,
                                        voucher,
                                        transdate,
                                        transtype);
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                var access_token = GetAccessToken();
                httpWebRequest.Headers.Add("Authorization", "Bearer " + access_token);
                httpWebRequest.Method = "GET";

                var result = "";
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();
                }
                //Get status from result string
                var status = SplitParameter(result, "statusCode");

                if (status == "200")
                {
                    IsSuccess = true;
                    var data = (JObject)JsonConvert.DeserializeObject(result);
                    var list = data["data"].Children();

                    if (list.Count() > 0)
                    {
                        result = "";
                        foreach (var item in list)
                        {
                            string value = item.ToObject<string>();
                            if (value != "")
                            {
                                var part = value.Split(new string[] { ";" }, StringSplitOptions.None);
                                SearchSaleOrderDetailViewModel viewModel = new SearchSaleOrderDetailViewModel();
                                //CustomerCode
                                viewModel.CustomerCode = part[0].ToString();
                                //SaleOrderCode
                                viewModel.SaleOrderCode = part[1].ToString();
                                //CreatedDate
                                viewModel.CreatedDate = part[2].ToString();
                                //Quantity
                                //viewModel.Quantity = part[3].ToString();
                                //Product
                                viewModel.Description = part[4].ToString();
                                var productCode = viewModel.Description.Substring(viewModel.Description.LastIndexOf("/") + 1)
                                                           .Trim().ToString();
                                var product = _context.ProductModel.FirstOrDefault(p => p.ProductCode == productCode);
                                if (product != null)
                                {
                                    viewModel.Product = product.ProductName;
                                }
                                //TransDate
                                viewModel.TransDateTmp = part[5].ToString();
                                //AmountMST
                                viewModel.AmountMSTTmp = part[6].ToString();
                                //StoreName
                                viewModel.StoreName = part[7].ToString();
                                //TransType
                                viewModel.TransType = part[8].ToString();
                                //SaleOrderType
                                viewModel.SaleOrderType = part[9].ToString();
                                searchList.Add(viewModel);
                            }
                        }
                    }
                    else
                    {
                        result = LanguageResource.SearchCustomer_NotFound;
                    }
                }
                else
                {
                    result = SplitParameter(result, "statusText");
                }
                return Json(new { IsSuccess = IsSuccess, ErrorCode = status, Message = result, Data = searchList }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                if (ex.GetType() == typeof(HttpException))
                {
                    ErrorCode = ((HttpException)ex).GetHttpCode().ToString();
                }
                else
                {
                    ErrorCode = "500";
                }
                IsSuccess = false;
                if (ex.InnerException != null)
                {
                    return Json(new { IsSuccess = IsSuccess, ErrorCode = ErrorCode, Message = ex.InnerException.Message }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { IsSuccess = IsSuccess, ErrorCode = ErrorCode, Message = ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
                }
            }
        }
        #endregion Search Order detail

        #region Update Sale Order
        public ActionResult UpdateSaleOrder(SaleOrderUpdateViewModel viewModel, List<SaleOrderUpdateDetailViewModel> detailList)
        {
            try
            {
                if (viewModel.SaleOrderCode != null)
                {
                    //Master
                    SaleOrderModel model = new SaleOrderModel();
                    model.SaleOrderId = Guid.NewGuid();
                    model.SaleOrderCode = viewModel.SaleOrderCode;
                    model.CustomerCode = viewModel.CustomerCode;
                    model.CustomerName = viewModel.CustomerName;
                    model.CustomerPhone = viewModel.CustomerPhone;
                    model.StoreName = viewModel.StoreName;
                    model.CreatedDate = viewModel.CreatedDate;
                    model.PaidDate = viewModel.PaidDate;
                    model.SubTotal = viewModel.SubTotal;
                    model.Total = viewModel.Total;
                    model.Note = viewModel.Note;
                    model.isUpdatedFromERP = true;
                    model.UpdatedFromERPTime = DateTime.Now;
                    model.SystemNote = viewModel.SystemNote;
                    _context.Entry(model).State = EntityState.Added;

                    //Detail
                    if (detailList != null && detailList.Count > 0)
                    {
                        foreach (var item in detailList)
                        {
                            SaleOrderDetailModel detailModel = new SaleOrderDetailModel();
                            detailModel.SaleOrderDetailId = Guid.NewGuid();
                            detailModel.SaleOrderId = model.SaleOrderId;
                            //product
                            var product = _context.ProductModel.FirstOrDefault(p => p.ProductCode == item.ProductCode);
                            if (product != null)
                            {
                                detailModel.ProductId = product.ProductId;
                            }
                            detailModel.Description = item.Description;
                            detailModel.SerialNumber = item.SerialNumber;
                            detailModel.EngineNumber = item.EngineNumber;
                            detailModel.Price = item.Price;
                            detailModel.Quantity = item.Quantity;
                            //1: discount by percent (%)
                            //0: discount by unit price ($)
                            detailModel.DiscountType = item.DiscountType;
                            detailModel.Discount = item.Discount;
                            detailModel.UnitPrice = item.UnitPrice;
                            _context.Entry(detailModel).State = EntityState.Added;
                        }
                    }
                    _context.SaveChanges();

                    return Json(new
                    {
                        Status = "Success",
                        Message = "Cập nhật đơn hàng thành công!"
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        Status = "Fail",
                        Message = "Bắt buộc truyền field SaleOrderCode!"
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    return Json(new
                    {
                        Status = "Fail",
                        Message = ex.InnerException.Message
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        Status = "Fail",
                        Message = ex.Message
                    }, JsonRequestBehavior.AllowGet);
                }
            }
        }
        #endregion Update Sale Order

        #region Update Service Order
        public ActionResult UpdateServiceOrder(ServiceOrderUpdateViewModel viewModel, List<ServiceOrderUpdateDetailViewModel> detailList)
        {
            try
            {
                if (viewModel.ServiceOrderCode != null)
                {
                    //Master
                    ServiceOrderModel model = new ServiceOrderModel();
                    model.ServiceOrderId = Guid.NewGuid();
                    model.ServiceOrderCode = viewModel.ServiceOrderCode;
                    model.CustomerCode = viewModel.CustomerCode;
                    model.CustomerName = viewModel.CustomerName;
                    model.CustomerPhone = viewModel.CustomerPhone;
                    model.StoreName = viewModel.StoreName;
                    model.CreatedDate = viewModel.CreatedDate;
                    model.Category = viewModel.Category;
                    model.ServicePool = viewModel.ServicePool;
                    model.LicensePlate = viewModel.LicensePlate;
                    model.SerialNumber = viewModel.SerialNumber;
                    model.EngineNumber = viewModel.EngineNumber;
                    model.CurrentKilometers = viewModel.CurrentKilometers;
                    model.PersonnalNumberId = viewModel.PersonnalNumberId;
                    model.Quantity = viewModel.Quantity;
                    model.Total = viewModel.Total;
                    model.Note = viewModel.Note;
                    model.isUpdatedFromERP = true;
                    model.UpdatedFromERPTime = DateTime.Now;
                    model.SystemNote = viewModel.SystemNote;
                    _context.Entry(model).State = EntityState.Added;

                    //Detail
                    if (detailList != null && detailList.Count > 0)
                    {
                        foreach (var item in detailList)
                        {
                            ServiceOrderDetailModel detailModel = new ServiceOrderDetailModel();
                            detailModel.ServiceOrderDetailId = Guid.NewGuid();
                            detailModel.ServiceOrderId = model.ServiceOrderId;
                            detailModel.Description = item.Description;
                            detailModel.Price = item.Price;
                            detailModel.Quantity = item.Quantity;
                            //1: discount by percent (%)
                            //0: discount by unit price ($)
                            detailModel.DiscountType = item.DiscountType;
                            detailModel.Discount = item.Discount;
                            detailModel.UnitPrice = item.UnitPrice;
                            _context.Entry(detailModel).State = EntityState.Added;
                        }
                    }
                    _context.SaveChanges();
                    return Json(new
                    {
                        Status = "Success",
                        Message = "Cập nhật đơn hàng dịch vụ thành công!"
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        Status = "Fail",
                        Message = "Bắt buộc truyền field ServiceOrderCode!"
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    return Json(new
                    {
                        Status = "Fail",
                        Message = ex.InnerException.Message
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        Status = "Fail",
                        Message = ex.Message
                    }, JsonRequestBehavior.AllowGet);
                }
            }
        }
        #endregion Update Service Order

        #region Checking Times
        public ActionResult CheckingTimes(string custaccount, string transdateDate, Guid? CustomerId)
        {
            var IsSuccess = false;
            var IsNotFound = false;
            var ErrorCode = "";
            var ErrorText = "";
            DateTime? TransdateDate = RepositoryLibrary.VNStringToDateTime(transdateDate);
            List<CheckingTimesViewModel> searchList = new List<CheckingTimesViewModel>();
            NotificationViewModel notification = new NotificationViewModel();
            try
            {
                #region API của Tiến Thu
                //var transdate = "";
                //if (TransdateDate.HasValue)
                //{
                //    transdate = TransdateDate.Value.ToString("yyyy-MM-dd'T'HH:mm:ss");
                //}
                //string url = string.Format("{0}/customers/checkingtimes?custaccount={1}&transdate={2}",
                //                        ConstDomain.DomainAPI,
                //                        custaccount,
                //                        transdate);
                //var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                //var access_token = GetAccessToken();
                //httpWebRequest.Headers.Add("Authorization", "Bearer " + access_token);
                //httpWebRequest.Method = "GET";

                //var result = "";
                //var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                //using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                //{
                //    result = streamReader.ReadToEnd();
                //}
                ////Get status from result string
                //ErrorCode = SplitParameter(result, "statusCode");
                //if (ErrorCode == "200")
                //{
                //    IsSuccess = true;
                //    var data = (JObject)JsonConvert.DeserializeObject(result);
                //    var list = data["data"].Children();

                //    if (list.Count() > 0)
                //    {
                //        ErrorText = "";
                //        foreach (var item in list)
                //        {
                //            string value = item.ToObject<string>();
                //            if (value != "")
                //            {
                //                var part = value.Split(new string[] { ";" }, StringSplitOptions.None);
                //                CheckingTimesViewModel viewModel = new CheckingTimesViewModel();
                //                //Plate
                //                viewModel.Plate = part[0].ToString();
                //                //Configuration
                //                viewModel.Configuration = part[1].ToString();
                //                //Description
                //                viewModel.Description = part[2].ToString();
                //                searchList.Add(viewModel);
                //            }
                //        }
                //        notification.CheckingTimes = searchList;
                //    }
                //    //else
                //    //{
                //    //    ErrorText = LanguageResource.SearchCustomer_NotFound;
                //    //}
                //}
                //else
                //{
                //    ErrorText = SplitParameter(result, "statusText");
                //}
                #endregion API của Tiến Thu=

                var currentDate = DateTime.Now.Date;
                //"Chương trình quà tặng" riêng cho từng khách hàng
                if (CustomerId != null)
                {
                    IsSuccess = true;
                    var customerGift = (from gift in _context.CustomerGiftModel
                                        join detail in _context.CustomerGiftDetailModel on gift.GiftId equals detail.GiftId
                                        join cus in _context.CustomerModel on detail.CustomerId equals cus.CustomerId
                                        where cus.CustomerId == CustomerId
                                        //&& ((gift.EffectFromDate <= currentDate)
                                        //&& (currentDate <= gift.EffectToDate))
                                        orderby gift.EffectToDate descending
                                        select new CustomerGiftAPIViewModel()
                                        {
                                            GiftId = gift.GiftId,
                                            PromotionCode = gift.GiftCode,
                                            ImageUrlTemp = gift.ImageUrl,
                                            PromotionName = gift.GiftName,
                                            EffectToDateTemp = gift.EffectToDate,
                                            Description = gift.Description,
                                            isRead = detail.isRead
                                        }).ToList();
                    notification.Gifts = customerGift;

                    #region Lấy thông tin kiểm tra định kỳ trong DB
                    var checkingTimesList = (from cus in _context.CustomerModel
                                             join noti in _context.CheckingTimesNotificationModel on cus.CustomerCode equals noti.CustomerCode
                                             where cus.CustomerId == CustomerId
                                             select new CheckingTimesNotificationViewModel()
                                             {
                                                 CheckingTimesId = noti.CheckingTimesId,
                                                 CheckingTimesDescription = noti.CheckingTimesDescription,
                                                 isRead = noti.isRead,
                                             }).ToList();
                    notification.CheckingTimes = checkingTimesList;
                    #endregion Lấy thông tin kiểm tra định kỳ trong DB
                }

                if (notification.CheckingTimes == null && notification.Gifts == null)
                {
                    IsNotFound = true;
                    ErrorText = LanguageResource.Mobile_NotificationNotFound;
                }
                return Json(new { IsSuccess = IsSuccess, ErrorCode = ErrorCode, Message = ErrorText, Data = notification, IsNotFound = IsNotFound }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                if (ex.GetType() == typeof(HttpException))
                {
                    ErrorCode = ((HttpException)ex).GetHttpCode().ToString();
                }
                else
                {
                    ErrorCode = "500";
                }
                IsSuccess = false;
                if (ex.InnerException != null)
                {
                    return Json(new { IsSuccess = IsSuccess, ErrorCode = ErrorCode, Message = ex.InnerException.Message }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { IsSuccess = IsSuccess, ErrorCode = ErrorCode, Message = ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
                }
            }
        }
        #endregion Checking Times

        #region Helper
        public string SplitParameter(string result, string parameter)
        {
            var entries = result.TrimStart('{').TrimEnd('}').Replace("\"", String.Empty).Split(',');
            var status = "";
            foreach (var entry in entries)
            {
                if (entry.Split(':')[0] == parameter)
                {
                    status = entry.Split(':')[1];
                }
            }
            return status;
        }
        //split data
        public List<string> SplitDataParameter(string result, string parameter)
        {
            List<string> dataList = new List<string>();

            var entries = result.TrimStart('{').TrimEnd('}').Replace("\"", String.Empty).Split(',');
            foreach (var entry in entries)
            {
                if (entry.Split(':')[0] == parameter)
                {
                    if (entry.Split(':')[1] != "[]")
                    {
                        dataList.Add(entry.Split(':')[1].Replace("[", "").Replace("]", ""));
                    }
                }
                if (!entry.Contains(":"))
                {
                    dataList.Add(entry.Replace("[", "").Replace("]", ""));
                }
            }
            return dataList;
        }
        #endregion Helper

        #region Opportunity
        public ActionResult GetOpportunitySource(string token, string key)
        {
            var prognosis = new List<ISDSelectItem>();
            var saleUnit = new List<ISDSelectItem>();
            var saleProcess = new List<ISDSelectItem>();
            var source = new List<ISDSelectItem>();
            string Error = "";
            if (token == tokenConst && key == keyConst)
            {
                prognosis = GetPrognosis();
                saleUnit = GetSaleUnit();
                saleProcess = GetSaleProcess();
                source = GetSource();
            }
            else
            {
                //access denied
                Error = LanguageResource.Account_AccessDenied;
            }
            return Json(new { prognosis = prognosis, saleUnit = saleUnit, saleProcess = saleProcess, source = source, Error }, JsonRequestBehavior.AllowGet);
        }

        private List<ISDSelectItem> GetPrognosis()
        {
            return _context.PrognosisModel
                            .Select(p => new ISDSelectItem()
                            {
                                value = p.PrognosisId,
                                text = p.PrognosisDescription,
                            }).ToList();
        }
        private List<ISDSelectItem> GetSaleUnit()
        {
            return _context.SaleUnitModel
                            .Select(p => new ISDSelectItem()
                            {
                                value = p.SaleUnitId,
                                text = p.SaleUnitDescription,
                            }).ToList();
        }
        private List<ISDSelectItem> GetSaleProcess()
        {
            return _context.SaleProcessModel
                           .Select(p => new ISDSelectItem()
                           {
                               value = p.SaleProcessId,
                               text = p.SaleProcessDescription,
                           }).ToList();
        }
        private List<ISDSelectItem> GetSource()
        {
            return _context.SourceModel
                           .Select(p => new ISDSelectItem()
                           {
                               value = p.SourceId,
                               text = p.SourceDescription,
                           }).ToList();
        }
        #endregion

        #region Service Pool
        public ActionResult GetServicePool(string token, string key)
        {
            var result = new List<ISDSelectItem2>();
            string Error = "";
            if (token == tokenConst && key == keyConst)
            {
                result = _context.ServiceCategoryModel.Where(p => p.Actived == true).Select(p => new ISDSelectItem2()
                {
                    value = p.ServiceCategoryCode,
                    text = p.ServiceCategoryName,
                    orderIndex = p.OrderIndex
                }).OrderBy(p => p.orderIndex).ToList();
            }
            else
            {
                //access denied
                Error = LanguageResource.Account_AccessDenied;
            }
            return Json(new { result, Error }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Get Product Details
        //Test: ProductId=2221094b-0e4a-480e-a89c-067107dfd675
        //GET
        //MasterData/API/GetProductDetails?ProductId=2221094b-0e4a-480e-a89c-067107dfd675&token=454FC8F419313554549E2DED09B9AF94&key=77f430e1-66fd-48dc-8057-77935e53be20
        public ActionResult GetProductDetails(Guid ProductId, string token, string key, string UserName = "")
        {
            string Error = "";
            var result = new ProductMobileViewModel();
            if (token == tokenConst && key == keyConst)
            {
                //1.1 Tìm sản phẩm
                //Default
                //1.1 Thông số mặc định cho "Phiên bản này"
                var product = (from p in _context.ProductModel
                               where p.ProductId == ProductId && p.Actived == true
                               select new ProductMobileViewModel()
                               {
                                   ProductId = p.ProductId,
                                   ProductName = p.ProductName,
                                   CategoryId = p.CategoryId,
                                   ImageUrl = p.ImageUrl,
                               }).FirstOrDefault();

                if (product == null)
                {
                    //Báo lỗi
                }

                //1. Còn trong kho
                bool isInstock;
                bool isViewTotal;
                bool isViewByWarehouse;


                //Hình đại diện: Nếu không có hình đại diện trong sản phẩm thì lấy hình mặc định trong danh sách hình ảnh sản phẩm
                if (product.ImageUrl == null)
                {
                    var image = _context.ImageProductModel.Where(p => p.ProductId == product.ProductId && p.isDefault == true).Select(p => p.ImageUrl).FirstOrDefault();
                    product.ImageUrl = image != null ? image : ConstImageUrl.noImage;
                }


                //1.2 Danh sách "Phiên bản" của "Loại xe" này
                var sameCategoryList = (from p in _context.ProductModel
                                        where p.CategoryId == product.CategoryId && p.Actived == true
                                        orderby p.OrderIndex.HasValue descending, p.OrderIndex
                                        select new SameCategoryViewModel()
                                        {
                                            ProductId = p.ProductId,
                                            ProductName = p.ProductName
                                        }).ToList();

                #region //Check tồn kho
                TT_InventoryCheckingRepository _InventoryRepository = new TT_InventoryCheckingRepository(_context);
                //Truyền thêm Account vào
                _InventoryRepository.GetStock(ProductId, UserName, sameCategoryList, out isInstock, out isViewTotal, out isViewByWarehouse);
                #endregion

                //Color
                //1.3 Danh sách màu sắc của "Phiên bản" này
                var colorList = (from color in _context.ColorModel
                                 join cp in _context.ColorProductModel on color.ColorId equals cp.MainColorId
                                 where cp.ProductId == ProductId
                                 orderby color.OrderIndex
                                 select new ColorMobileViewModel()
                                 {
                                     ColorId = color.ColorId,
                                     ColorCode = color.ColorCode,
                                     ColorName = color.ColorName
                                 }).Distinct().ToList();

                //Style 
                //1.4 Danh sách các "Kiểu Dáng" Theo "Phiên bản" này với thông số "ColorId"
                //Có thêm dữ liệu màu sắc

                Guid? DefaultColorId = null;
                Guid? DefaultStyleId = new Guid();
                //Guid defaultId = new Guid();

                var styleList = (from colorOfProduct in _context.ColorProductModel
                                 join style in _context.StyleModel on colorOfProduct.StyleId equals style.StyleId into sg
                                 from styleGroup in sg.DefaultIfEmpty()
                                 where colorOfProduct.ProductId == ProductId
                                 orderby /*color.OrderIndex,*/ styleGroup.OrderIndex
                                 select new
                                 {
                                     ColorId = colorOfProduct.MainColorId,
                                     StyleId = colorOfProduct.StyleId != null ? colorOfProduct.StyleId : DefaultStyleId,
                                     StyleName = styleGroup.StyleName != null ? styleGroup.StyleName : LanguageResource.DefaultStyle
                                 }).ToList();
                if (styleList != null && styleList.Count > 0)
                {
                    foreach (var color in colorList)
                    {
                        var StyleId = styleList.Where(p => p.ColorId == color.ColorId).Select(p => p.StyleId).FirstOrDefault();
                        if (StyleId != null)
                        {
                            color.DefaultStyleId = StyleId;
                        }
                        else
                        {
                            color.DefaultStyleId = DefaultStyleId;
                        }
                    }
                }

                //Image
                //1.5 Danh sách "Hình ảnh" sản phẩm theo "Phiên bản" này với thông số "Màu sắc", "Kiểu dáng"
                var imageList = (from img in _context.ImageProductModel
                                 join colorOfProduct in _context.ColorProductModel on img.ColorProductId equals colorOfProduct.ColorProductId
                                 where colorOfProduct.ProductId == ProductId
                                 select new ImageListViewModel()
                                 {
                                     ColorId = colorOfProduct.MainColorId,
                                     StyleId = colorOfProduct.StyleId != null ? colorOfProduct.StyleId : DefaultStyleId,
                                     ImageUrl = img.ImageUrl
                                 }).ToList();
                //Default data
                if (colorList != null && colorList.Count > 0)
                {
                    DefaultColorId = colorList.First().ColorId;
                }
                //Trường hợp có "Kiểu dáng" => lấy "Hình ảnh" theo "Kiểu dáng" đầu tiên
                if (styleList != null && styleList.Count > 0 && imageList != null)
                {
                    var defaultStyle = styleList.Where(p => p.ColorId == DefaultColorId).FirstOrDefault();
                    if (defaultStyle != null)
                    {
                        DefaultStyleId = defaultStyle.StyleId;
                    }
                }
                var defaultGuid = new Guid();
                //Price
                //1.6 
                var priceList = (from pr in _context.PriceProductModel
                                 where pr.ProductId == ProductId
                                 select new PriceListViewModel()
                                 {
                                     ColorId = pr.MainColorId,
                                     StyleId = pr.StyleId != null ? pr.StyleId : defaultGuid,
                                     Price = pr.Price,
                                     PostDate = pr.PostDate,
                                     PostTime = pr.PostTime
                                 }).ToList();
                var defaultPrice = priceList.Where(p => p.ColorId == DefaultColorId && p.StyleId == DefaultStyleId).FirstOrDefault();
                if (defaultPrice != null)
                {
                    product.DefaultPrice = defaultPrice.PriceWithFormat;
                    product.DefaultApplyTime = defaultPrice.ApplyTime;
                }

                product.DefaultColorId = DefaultColorId;
                product.DefaultStyleId = DefaultStyleId;

                //Specifications: "Thông số kỹ thuật" theo "Phiên bản" này
                //1.7
                var specificationsList = (from specs in _context.SpecificationsModel
                                          join specsProduct in _context.SpecificationsProductModel on specs.SpecificationsId equals specsProduct.SpecificationsId
                                          where specsProduct.ProductId == ProductId
                                          orderby specs.OrderIndex
                                          select new SpecificationsListViewModel()
                                          {
                                              SpecificationsName = specs.SpecificationsName,
                                              Description = specsProduct.Description,
                                          }).ToList();

                //Accessory: "Phụ kiện"
                //1.8
                //Danh sách "Nhóm phụ kiện"
                var accessoryCategoryList = (from ac in _context.AccessoryCategoryModel
                                             select new AccessorCategoryListViewModel()
                                             {
                                                 AccessoryCategoryId = ac.AccessoryCategoryId,
                                                 AccessoryCategoryName = ac.AccessoryCategoryName,
                                             }).ToList()
                                             .Select(p => new
                                             {
                                                 AccessoryCategoryId = p.AccessoryCategoryId,
                                                 AccessoryCategoryName = p.AccessoryCategoryName,
                                                 ImageUrl = p.ImageUrl
                                             }).ToList();

                //Danh sách "Phụ kiện" theo "Phiên bản" này
                var accessoryList = (from a in _context.AccessoryModel
                                     join ac in _context.AccessoryCategoryModel on a.AccessoryCategoryId equals ac.AccessoryCategoryId
                                     join ap in _context.AccessoryProductModel on a.AccessoryId equals ap.AccessoryId
                                     where ap.ProductId == ProductId
                                     //orderby a.OrderIndex
                                     select new AccessoryListViewModel
                                     {
                                         AccessoryCategoryId = a.AccessoryCategoryId,
                                         AccessoryId = a.AccessoryId,
                                         AccessoryCategoryCode = ac.AccessoryCategoryCode,
                                         AccessoryName = a.AccessoryName,
                                         Price = ap.Price,
                                         //isHelmet = a.isHelmet,
                                         //isHelmetAdult = a.isHelmetAdult,
                                         //Size = a.Size,
                                         ImageUrlTemp = a.ImageUrl,
                                     }).ToList();

                //Properties: "Tính năng"
                //1.10

                var propertiesList = (from p in _context.PropertiesProductModel
                                      join pr in _context.ProductModel on p.ProductId equals pr.ProductId
                                      where p.ProductId == ProductId
                                      select new PropertiesViewModel
                                      {
                                          PropertiesId = p.PropertiesId,
                                          X = p.X,
                                          Y = p.Y,
                                          Subject = p.Subject,
                                          Description = p.Description,
                                          Image = p.Image
                                      }).ToList();
                //Ẩn hiện icon quà tặng
                // 1.11
                IQueryable<PromotionAPIViewModel> query = GetPromotion(ProductId);
                var IsHasPromotion = false;
                if (query.FirstOrDefault() != null)
                {
                    IsHasPromotion = true;
                }

                return Json(new
                {
                    status = true,
                    //1.1
                    product = product,
                    //1.2
                    samecategorylist = sameCategoryList,
                    //1.3
                    colorlist = colorList,
                    //1.4
                    stylelist = styleList,
                    //1.5
                    imagelist = imageList,
                    //1.6
                    pricelist = priceList,
                    //1.7
                    specificationslist = specificationsList,
                    //1.8
                    accessorycategorylist = accessoryCategoryList,
                    accessorylist = accessoryList,
                    //1.9 isInstock
                    isinstock = isInstock,
                    isviewtotal = isViewTotal,
                    isviewbywarehouse = isViewByWarehouse,
                    //1.10
                    propertieslist = propertiesList,
                    //1.11
                    isHasPromotion = IsHasPromotion,

                    //1.100 Default domain
                    domainimageurl = ConstDomain.DomainImageProduct,
                    //1.101 Accessory domain
                    domainaccessory = ConstDomain.DomainImageAccessory,

                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                //access denied
                Error = LanguageResource.Account_AccessDenied;
                return Json(new
                {
                    status = false,
                    Error = Error
                }, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult GetProductDetails2(Guid? ProductId, Guid? CategoryId, string token, string key, string UserName = "")
        {
            string Error = "";
            var result = new ProductMobileViewModel();
            if (token == tokenConst && key == keyConst)
            {
                //1.1 Tìm sản phẩm
                //Nếu xem "Thông tin sản phẩm" => ProductId != null => chọn "Phiên bản" = ProductId
                //Nếu xem "Thông tin sản phẩm" từ "Loại xe" => CategoryId != null => Chọn "Phiên bản" theo "Thứ tự thể hiện" (default)
                //1.1 Thông số mặc định cho "Phiên bản này"
                ProductMobileViewModel product = new ProductMobileViewModel();
                if (ProductId != null)
                {
                    product = (from p in _context.ProductModel
                               where p.Actived == true && p.ProductId == ProductId
                               select new ProductMobileViewModel()
                               {
                                   ProductId = p.ProductId,
                                   ProductName = p.ProductName,
                                   CategoryId = p.CategoryId,
                                   ImageUrl = p.ImageUrl,
                               }).FirstOrDefault();
                }
                else if (CategoryId != null)
                {
                    product = (from p in _context.ProductModel
                               where p.Actived == true && p.CategoryId == CategoryId
                               orderby p.OrderIndex.HasValue descending, p.OrderIndex
                               select new ProductMobileViewModel()
                               {
                                   ProductId = p.ProductId,
                                   ProductName = p.ProductName,
                                   CategoryId = p.CategoryId,
                                   ImageUrl = p.ImageUrl,
                               }).FirstOrDefault();
                }

                if (product == null)
                {
                    //Báo lỗi
                    return Json(new
                    {
                        status = false,
                        Error = "Đã có lỗi xảy ra. Vui lòng thử lại sau."

                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    //1. Còn trong kho
                    bool isInstock;
                    bool isViewTotal;
                    bool isViewByWarehouse;


                    //Hình đại diện: Nếu không có hình đại diện trong sản phẩm thì lấy hình mặc định trong danh sách hình ảnh sản phẩm
                    if (product.ImageUrl == null)
                    {
                        var image = _context.ImageProductModel.Where(p => p.ProductId == product.ProductId && p.isDefault == true).Select(p => p.ImageUrl).FirstOrDefault();
                        product.ImageUrl = image != null ? image : ConstImageUrl.noImage;
                    }


                    //1.2 Danh sách "Phiên bản" của "Loại xe" này
                    var sameCategoryList = (from p in _context.ProductModel
                                            where p.CategoryId == product.CategoryId && p.Actived == true
                                            orderby p.OrderIndex.HasValue descending, p.OrderIndex
                                            select new SameCategoryViewModel()
                                            {
                                                ProductId = p.ProductId,
                                                ProductName = p.ProductName
                                            }).ToList();

                    #region //Check tồn kho
                    TT_InventoryCheckingRepository _InventoryRepository = new TT_InventoryCheckingRepository(_context);
                    //Truyền thêm Account vào
                    _InventoryRepository.GetStock(product.ProductId, UserName, sameCategoryList, out isInstock, out isViewTotal, out isViewByWarehouse);
                    #endregion

                    //Color
                    //1.3 Danh sách màu sắc của "Phiên bản" này
                    var colorList = (from color in _context.ColorModel
                                     join cp in _context.ColorProductModel on color.ColorId equals cp.MainColorId
                                     where cp.ProductId == product.ProductId
                                     orderby color.OrderIndex
                                     select new ColorMobileViewModel()
                                     {
                                         ColorId = color.ColorId,
                                         ColorCode = color.ColorCode,
                                         ColorName = color.ColorName
                                     }).Distinct().ToList();

                    //Style 
                    //1.4 Danh sách các "Kiểu Dáng" Theo "Phiên bản" này với thông số "ColorId"
                    //Có thêm dữ liệu màu sắc

                    Guid? DefaultColorId = null;
                    Guid? DefaultStyleId = new Guid();
                    //Guid defaultId = new Guid();

                    var styleList = (from colorOfProduct in _context.ColorProductModel
                                     join style in _context.StyleModel on colorOfProduct.StyleId equals style.StyleId into sg
                                     from styleGroup in sg.DefaultIfEmpty()
                                     where colorOfProduct.ProductId == product.ProductId
                                     orderby /*color.OrderIndex,*/ styleGroup.OrderIndex
                                     select new
                                     {
                                         ColorId = colorOfProduct.MainColorId,
                                         StyleId = colorOfProduct.StyleId != null ? colorOfProduct.StyleId : DefaultStyleId,
                                         StyleName = styleGroup.StyleName != null ? styleGroup.StyleName : LanguageResource.DefaultStyle
                                     }).ToList();
                    if (styleList != null && styleList.Count > 0)
                    {
                        foreach (var color in colorList)
                        {
                            var StyleId = styleList.Where(p => p.ColorId == color.ColorId).Select(p => p.StyleId).FirstOrDefault();
                            if (StyleId != null)
                            {
                                color.DefaultStyleId = StyleId;
                            }
                            else
                            {
                                color.DefaultStyleId = DefaultStyleId;
                            }
                        }
                    }

                    //Image
                    //1.5 Danh sách "Hình ảnh" sản phẩm theo "Phiên bản" này với thông số "Màu sắc", "Kiểu dáng"
                    var imageList = (from img in _context.ImageProductModel
                                     join colorOfProduct in _context.ColorProductModel on img.ColorProductId equals colorOfProduct.ColorProductId
                                     where colorOfProduct.ProductId == product.ProductId
                                     select new ImageListViewModel()
                                     {
                                         ColorId = colorOfProduct.MainColorId,
                                         StyleId = colorOfProduct.StyleId != null ? colorOfProduct.StyleId : DefaultStyleId,
                                         ImageUrl = img.ImageUrl
                                     }).ToList();
                    //Default data
                    if (colorList != null && colorList.Count > 0)
                    {
                        DefaultColorId = colorList.First().ColorId;
                    }
                    //Trường hợp có "Kiểu dáng" => lấy "Hình ảnh" theo "Kiểu dáng" đầu tiên
                    if (styleList != null && styleList.Count > 0 && imageList != null)
                    {
                        var defaultStyle = styleList.Where(p => p.ColorId == DefaultColorId).FirstOrDefault();
                        if (defaultStyle != null)
                        {
                            DefaultStyleId = defaultStyle.StyleId;
                        }
                    }
                    var defaultGuid = new Guid();
                    //Price
                    //1.6 
                    var priceList = (from pr in _context.PriceProductModel
                                     where pr.ProductId == product.ProductId
                                     select new PriceListViewModel()
                                     {
                                         ColorId = pr.MainColorId,
                                         StyleId = pr.StyleId != null ? pr.StyleId : defaultGuid,
                                         Price = pr.Price,
                                         PostDate = pr.PostDate,
                                         PostTime = pr.PostTime
                                     }).ToList();
                    var defaultPrice = priceList.Where(p => p.ColorId == DefaultColorId && p.StyleId == DefaultStyleId).FirstOrDefault();
                    if (defaultPrice != null)
                    {
                        product.DefaultPrice = defaultPrice.PriceWithFormat;
                        product.DefaultApplyTime = defaultPrice.ApplyTime;
                    }

                    product.DefaultColorId = DefaultColorId;
                    product.DefaultStyleId = DefaultStyleId;

                    //Specifications: "Thông số kỹ thuật" theo "Phiên bản" này
                    //1.7
                    var specificationsList = (from specs in _context.SpecificationsModel
                                              join specsProduct in _context.SpecificationsProductModel on specs.SpecificationsId equals specsProduct.SpecificationsId
                                              where specsProduct.ProductId == product.ProductId
                                              orderby specs.OrderIndex
                                              select new SpecificationsListViewModel()
                                              {
                                                  SpecificationsName = specs.SpecificationsName,
                                                  Description = specsProduct.Description,
                                              }).ToList();

                    //Accessory: "Phụ kiện"
                    //1.8
                    //Danh sách "Nhóm phụ kiện"
                    var accessoryCategoryList = (from ac in _context.AccessoryCategoryModel
                                                 select new AccessorCategoryListViewModel()
                                                 {
                                                     AccessoryCategoryId = ac.AccessoryCategoryId,
                                                     AccessoryCategoryName = ac.AccessoryCategoryName,
                                                 }).ToList()
                                                 .Select(p => new
                                                 {
                                                     AccessoryCategoryId = p.AccessoryCategoryId,
                                                     AccessoryCategoryName = p.AccessoryCategoryName,
                                                     ImageUrl = p.ImageUrl
                                                 }).ToList();

                    //Danh sách "Phụ kiện" theo "Phiên bản" này
                    var accessoryList = (from a in _context.AccessoryModel
                                         join ac in _context.AccessoryCategoryModel on a.AccessoryCategoryId equals ac.AccessoryCategoryId
                                         join ap in _context.AccessoryProductModel on a.AccessoryId equals ap.AccessoryId
                                         where ap.ProductId == product.ProductId
                                         select new AccessoryListViewModel
                                         {
                                             AccessoryCategoryId = a.AccessoryCategoryId,
                                             AccessoryId = a.AccessoryId,
                                             AccessoryCategoryCode = ac.AccessoryCategoryCode,
                                             AccessoryName = a.AccessoryName,
                                             Price = ap.Price,
                                             //isHelmet = a.isHelmet,
                                             //isHelmetAdult = a.isHelmetAdult,
                                             //Size = a.Size,
                                             ImageUrlTemp = a.ImageUrl,
                                         }).ToList();

                    //Properties: "Tính năng"
                    //1.10

                    var propertiesList = (from p in _context.PropertiesProductModel
                                          join pr in _context.ProductModel on p.ProductId equals pr.ProductId
                                          where p.ProductId == product.ProductId
                                          select new PropertiesViewModel
                                          {
                                              PropertiesId = p.PropertiesId,
                                              X = p.X,
                                              Y = p.Y,
                                              Subject = p.Subject,
                                              Description = p.Description,
                                              Image = p.Image
                                          }).ToList();
                    //Ẩn hiện icon quà tặng
                    // 1.11
                    IQueryable<PromotionAPIViewModel> query = GetPromotion(product.ProductId);
                    var IsHasPromotion = false;
                    if (query.FirstOrDefault() != null)
                    {
                        IsHasPromotion = true;
                    }

                    return Json(new
                    {
                        status = true,
                        //1.1
                        product = product,
                        //1.2
                        samecategorylist = sameCategoryList,
                        //1.3
                        colorlist = colorList,
                        //1.4
                        stylelist = styleList,
                        //1.5
                        imagelist = imageList,
                        //1.6
                        pricelist = priceList,
                        //1.7
                        specificationslist = specificationsList,
                        //1.8
                        accessorycategorylist = accessoryCategoryList,
                        accessorylist = accessoryList,
                        //1.9 isInstock
                        isinstock = isInstock,
                        isviewtotal = isViewTotal,
                        isviewbywarehouse = isViewByWarehouse,
                        //1.10
                        propertieslist = propertiesList,
                        //1.11
                        isHasPromotion = IsHasPromotion,

                        //1.100 Default domain
                        domainimageurl = ConstDomain.DomainImageProduct,
                        //1.101 Accessory domain
                        domainaccessory = ConstDomain.DomainImageAccessory,

                    }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                //access denied
                Error = LanguageResource.Account_AccessDenied;
                return Json(new
                {
                    status = false,
                    Error = Error
                }, JsonRequestBehavior.AllowGet);
            }

        }
        #endregion Get Product Details

        #region Get quantity in stock
        public ActionResult GetStockDetail(Guid ProductId, string token, string key, string UserName = "")
        {
            if (token == tokenConst && key == keyConst)
            {
                TT_InventoryCheckingRepository _inventoryRepository = new TT_InventoryCheckingRepository(_context);
                var result = _inventoryRepository.GetStockDetail(ProductId, UserName);
                return Json(new
                {
                    status = true,
                    data = result,
                    Error = ""
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                //access denied
                return Json(new
                {
                    status = false,
                    Error = LanguageResource.Account_AccessDenied
                }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Account Information
        /// <summary>
        /// Thông tin xe của khách hàng
        /// </summary>
        /// <param name="ProductKeySearch">Thông tin sản phẩm</param>
        ///<returns>
        ///1. - Biển số xe
        ///2. - Tên xe
        ///3. - Ngày mua xe
        ///</returns>
        public ActionResult AccountInfomation(string custAccount, string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                string CustomerLevel = string.Empty;
                string CustomerPoint = string.Empty;
                #region Thông tin xe
                string status = "";
                var PlateOfCustomerList = new List<PlateOfCustomerViewModel>();

                string url = string.Format("{0}/customers/plateofcustomer?custaccount={1}",
                                        ConstDomain.DomainAPI,
                                        custAccount);
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                var access_token = GetAccessToken();
                httpWebRequest.Headers.Add("Authorization", "Bearer " + access_token);
                httpWebRequest.Method = "GET";

                var result = "";
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();
                }
                //Get status from result string
                status = SplitParameter(result, "statusCode");
                if (status == "200")
                {
                    var data = (JObject)JsonConvert.DeserializeObject(result);
                    var list = data["data"].Children();

                    if (list.Count() > 0)
                    {
                        foreach (var item in list)
                        {
                            string value = item.ToObject<string>();
                            if (value != "")
                            {
                                var part = value.Split(new string[] { ";" }, StringSplitOptions.None);
                                PlateOfCustomerViewModel viewModel = new PlateOfCustomerViewModel();
                                //Plate
                                viewModel.Plate = part[0].ToString();
                                //Configuration
                                viewModel.ProductName = part[1].ToString();
                                //Description
                                viewModel.CreatedDate = part[2].ToString();
                                CustomerLevel = part[3].ToString();
                                CustomerPoint = string.Format("{0:n0}", decimal.Parse(part[4].ToString()));
                                PlateOfCustomerList.Add(viewModel);
                            }
                        }
                    }

                    if (PlateOfCustomerList != null && PlateOfCustomerList.Count > 0)
                    {
                        //Thông tin xe
                        //return _APISuccess(ret);
                    }
                    else
                    {
                        return _APIError("Không tìm thấy thông tin xe.");
                    }
                }
                else
                {
                    return _APIError(SplitParameter(result, "statusText"));
                }
                #endregion
                #region
                AccountInfomationViewModel retAccountInfomation = new AccountInfomationViewModel();
                List<SearchSaleOrderHeaderViewModel> searchList = new List<SearchSaleOrderHeaderViewModel>();
                url = string.Format("{0}/customers/searchorderheader?custaccount={1}",
                                        ConstDomain.DomainAPI,
                                        custAccount);
                httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.Headers.Add("Authorization", "Bearer " + access_token);
                httpWebRequest.Method = "GET";

                httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();
                }
                //Get status from result string
                status = SplitParameter(result, "statusCode");

                //return message
                if (status == "200")
                {
                    var data = (JObject)JsonConvert.DeserializeObject(result);
                    var list = data["data"].Children();

                    if (list.Count() > 0)
                    {
                        result = "";
                        foreach (var item in list)
                        {
                            string value = item.ToObject<string>();
                            if (value != "")
                            {
                                var part = value.Split(new string[] { ";" }, StringSplitOptions.None);
                                SearchSaleOrderHeaderViewModel viewModel = new SearchSaleOrderHeaderViewModel();
                                viewModel.SaleOrderType = part[7].ToString();
                                searchList.Add(viewModel);
                            }
                        }
                    }
                    else
                    {
                        result = LanguageResource.SearchSaleOrder_NotFound;
                    }
                }
                else
                {
                    result = SplitParameter(result, "statusText");
                }
                if (searchList != null && searchList.Count > 0)
                {
                    retAccountInfomation.TotalSaleOrder = searchList.Where(p => p.SaleOrderType == "Đơn hàng mua").Count();
                    retAccountInfomation.TotalServiceOrder = searchList.Count - retAccountInfomation.TotalSaleOrder;
                }

                return _APISuccess(new
                {
                    retAccountInfomation.TotalSaleOrder,
                    retAccountInfomation.TotalServiceOrder,
                    PlateOfCustomerList,
                    CustomerLevel,
                    CustomerPoint,
                });

                #endregion
            });
        }

        public ActionResult GetPlate(string custAccount, string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                if (string.IsNullOrEmpty(custAccount))
                {
                    return _APIError("Đặt lịch sửa chữa chỉ dành cho khách hàng");
                }
                else
                {
                    #region Thông tin xe
                    string status = "";
                    var PlateOfCustomerList = new List<PlateOfCustomerViewModel>();

                    string url = string.Format("{0}/customers/plateofcustomer?custaccount={1}",
                                            ConstDomain.DomainAPI,
                                            custAccount);
                    var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                    var access_token = GetAccessToken();
                    httpWebRequest.Headers.Add("Authorization", "Bearer " + access_token);
                    httpWebRequest.Method = "GET";

                    var result = "";
                    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        result = streamReader.ReadToEnd();
                    }
                    //Get status from result string
                    status = SplitParameter(result, "statusCode");
                    if (status == "200")
                    {
                        var data = (JObject)JsonConvert.DeserializeObject(result);
                        var list = data["data"].Children();

                        if (list.Count() > 0)
                        {
                            foreach (var item in list)
                            {
                                string value = item.ToObject<string>();
                                if (value != "")
                                {
                                    var part = value.Split(new string[] { ";" }, StringSplitOptions.None);
                                    PlateOfCustomerViewModel viewModel = new PlateOfCustomerViewModel();
                                    //Plate
                                    viewModel.Plate = part[0].ToString();
                                    //Configuration
                                    viewModel.ProductName = part[1].ToString();
                                    //Description
                                    viewModel.CreatedDate = part[2].ToString();
                                    PlateOfCustomerList.Add(viewModel);
                                }
                            }
                        }

                        if (PlateOfCustomerList != null && PlateOfCustomerList.Count > 0)
                        {
                            //Thông tin xe
                            return _APISuccess(PlateOfCustomerList);
                        }
                        else
                        {
                            return _APIError("Bạn chưa có xe nào, vui lòng chọn xe khác!");
                        }
                    }
                    else
                    {
                        return _APIError(SplitParameter(result, "statusText"));
                    }
                    #endregion
                }
            });
        }
        #endregion

        #region  ChangePassword
        public ActionResult ChangePassword(string username, string oldPassword, string newPassword, string token, string key, bool isCustomer)
        {
            if (token == tokenConst && key == keyConst)
            {
                //Nếu là khách hàng thì tìm trong bảng "CustomerModel"
                //Nếu là nhân viên thì tìm trong bảng "AccountModel"

                //1. Tìm account
                // => Nếu không thấy báo lôi
                #region Tìm account
                AccountModel account = new AccountModel();
                if (isCustomer == true)
                {
                    account = (from acc in _context.AccountModel
                               join customer in _context.CustomerModel on acc.AccountId equals customer.AccountId
                               where acc.UserName == username &&
                               customer.SMSConfirm == true
                               select acc).FirstOrDefault();
                }
                else
                {
                    account = (from acc in _context.AccountModel
                               where acc.UserName == username
                               select acc).FirstOrDefault();
                }

                //Bắt lỗi user không tồn tại trong hệ thống
                if (account == null)
                {
                    return Json(new { IsSuccess = false, Message = LanguageResource.Mobile_UserIsNotExist }, JsonRequestBehavior.AllowGet);
                }
                #endregion

                else
                {
                    //2. So sánh với mật khẩu cũ
                    // => Không đúng báo lỗi
                    #region
                    var md5OldPassword = RepositoryLibrary.GetMd5Sum(oldPassword);
                    if (md5OldPassword != account.Password)
                    {
                        return Json(new
                        {
                            IsSuccess = false,
                            Message = LanguageResource.ChangePassword_OldPasswordNotMatch
                        }, JsonRequestBehavior.AllowGet);
                    }
                    #endregion

                    //3. Cập nhật mật khẩu mới
                    #region
                    else
                    {
                        account.Password = RepositoryLibrary.GetMd5Sum(newPassword);
                        _context.Entry(account).State = EntityState.Modified;
                        _context.SaveChanges();

                        return Json(new
                        {
                            IsSuccess = true,
                            Message = LanguageResource.ChangePassword_Success
                        }, JsonRequestBehavior.AllowGet);
                    }
                    #endregion
                }
            }
            else
            {
                //access denied
                return Json(new
                {
                    IsSuccess = false,
                    Message = LanguageResource.Account_AccessDenied
                }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region SearchBooking
        public ActionResult SearchBooking(long? BookingCode, string FullName, DateTime? BookingDate, string Phone, string StoreCode, string token, string key)
        {
            if (token == tokenConst && key == keyConst)
            {
                //Tìm kiếm lịch hẹn trong bảng "[tService].BookingModel" với điều kiện là chưa tạo "Phiếu dịch vụ" và lọc theo 4 field: 
                //1. Mã phiếu đặt: BookingCode
                //2. Họ tên khách hàng: FullName
                //3. Ngày đặt: BookingDate
                //4. Số điện thoại: Phone

                //Nhân viên chưa được cấu hình cửa hàng trên mobile
                if (string.IsNullOrEmpty(StoreCode))
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        Message = LanguageResource.Mobile_NotConfigStore
                    }, JsonRequestBehavior.AllowGet);
                }
                //Cửa hàng được đặt
                var storeId = _context.StoreModel.Where(p => p.StoreCode == StoreCode).Select(p => p.StoreId).FirstOrDefault();

                var bookingDate = BookingDate.Value.Date;

                var bookingList = (from booking in _context.BookingModel
                                   where (BookingCode == null || booking.BookingCode == BookingCode)
                                   && (string.IsNullOrEmpty(FullName) || booking.FullName.Contains(FullName))
                                   && (BookingDate == null || booking.BookingDate == bookingDate)
                                   && (string.IsNullOrEmpty(Phone) || booking.Phone == Phone)
                                   && booking.StoreId == storeId
                                   && booking.Active == true
                                   && booking.IsCreatedServiceOrder != true
                                   join customer in _context.CustomerModel on booking.CustomerId equals customer.CustomerId
                                   orderby booking.BookingTime
                                   select new BookingListViewModel()
                                   {
                                       BookingCode = booking.BookingCode,
                                       BookingDate = booking.BookingDate,
                                       BookingTimeTemp = booking.BookingTime,
                                       CustomerCode = customer.CustomerCode,
                                       FullName = booking.FullName,
                                       Phone = booking.Phone,
                                       Plate = booking.Plate,
                                   }).ToList();

                if (bookingList == null || bookingList.Count == 0)
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        Message = LanguageResource.SearchCustomer_NotFound
                    }, JsonRequestBehavior.AllowGet);
                }

                return Json(new
                {
                    IsSuccess = true,
                    result = bookingList
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                //access denied
                return Json(new
                {
                    IsSuccess = false,
                    Message = LanguageResource.Account_AccessDenied
                }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region AccessoriesEstimates
        public ActionResult AccessoriesEstimates(string custAccount, string estimatedDate, string plate, decimal currentkm, string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {

                #region Dự toán phụ tùng
                string status = "";
                var SparePartList = new List<AccessoriesEstimatesViewModel>();

                string url = string.Format("{0}/customers/checkingspareparts?custcccount={1}&plateid={2}&currentkm={3}",
                                        ConstDomain.DomainAPI,
                                        custAccount,
                                        plate,
                                        currentkm);
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                var access_token = GetAccessToken();
                httpWebRequest.Headers.Add("Authorization", "Bearer " + access_token);
                httpWebRequest.Method = "GET";

                var result = "";
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();
                }
                //Get status from result string
                status = SplitParameter(result, "statusCode");
                if (status == "200")
                {
                    var data = (JObject)JsonConvert.DeserializeObject(result);
                    var list = data["data"].Children();

                    if (list.Count() > 0)
                    {
                        foreach (var item in list)
                        {
                            string value = item.ToObject<string>();
                            if (!string.IsNullOrEmpty(value))
                            {
                                var part = value.Split(new string[] { ";" }, StringSplitOptions.None);
                                AccessoriesEstimatesViewModel viewModel = new AccessoriesEstimatesViewModel();
                                //Plate
                                viewModel.Title = part[0].ToString();
                                //Configuration
                                viewModel.Description = part[1].ToString();
                                //Description
                                viewModel.StoreName = part[2].ToString();
                                SparePartList.Add(viewModel);
                            }
                        }
                    }

                    if (SparePartList != null && SparePartList.Count > 0)
                    {
                        //Thông tin xe
                        return _APISuccess(new
                        {
                            SparePartList,
                            Message1 = "Dưới đây là những phụ tùng, linh kiện cần thay thế, bảo dưỡng: ",
                            Message2 = "Vui lòng đến cửa hàng của Tiến Thu gần nhất để được thay thế bảo dưỡng!"
                        });
                    }
                    else
                    {
                        return _APIError("Hiện tại xe của bạn chưa cần phải: Bảo dưỡng, sửa chữa.");
                    }
                }
                else
                {
                    return _APIError(SplitParameter(result, "statusText"));
                }
                #endregion
            });
        }
        #endregion

        #region Get Stores List
        public ActionResult GetStore(string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                //Danh sách cửa hàng chỉ hiển thị dạng thông tin
                List<StoreListViewModel> result = (from store in _context.StoreModel
                                                   join st in _context.StoreTypeModel on store.StoreTypeId equals st.StoreTypeId into stGroup
                                                   from storeType in stGroup.DefaultIfEmpty()
                                                   orderby store.OrderIndex
                                                   select new StoreListViewModel()
                                                   {
                                                       StoreId = store.StoreId,
                                                       ImageUrlTemp = store.ImageUrl,
                                                       LogoUrlTemp = store.LogoUrl,
                                                       StoreName = store.StoreName,
                                                       StoreTypeName = storeType.StoreTypeName,
                                                       StoreAddress = store.StoreAddress,
                                                       TelProductTemp = store.TelProduct,
                                                       TelServiceTemp = store.TelService,
                                                       ProvinceId = store.ProvinceId,
                                                   }).ToList();

                //Danh sách tỉnh thành có cửa hàng của Tiến Thu
                var provinceList = (from store in _context.StoreModel
                                    join province in _context.ProvinceModel on store.ProvinceId equals province.ProvinceId
                                    where store.Actived == true
                                    select new
                                    {
                                        ProvinceId = province.ProvinceId,
                                        ProvinceName = province.ProvinceName,
                                    }).Distinct().ToList();
                if (result == null || result.Count == 0)
                {
                    return _APIError(LanguageResource.Mobile_NotFound);
                }
                return _APISuccess(new
                {
                    StoreList = result,
                    ProvinceList = provinceList,
                    IsAllowedToBooking = _context.APIModel.Select(p => p.isAllowedToBooking).FirstOrDefault() == true
                });
            });
        }
        #endregion

        #region Get Periodically Checking
        //Test
        //ProductId=50aa56e3-e954-4f73-80c0-1f5ec3a07a8b&token=454FC8F419313554549E2DED09B9AF94
        //MasterData/api/GetPeriodicallyCheckingBy?ProductId=50aa56e3-e954-4f73-80c0-1f5ec3a07a8b&token=454FC8F419313554549E2DED09B9AF94&key=77f430e1-66fd-48dc-8057-77935e53be20
        public ActionResult GetPeriodicallyCheckingBy(Guid ProductId, string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                //Danh sách kiểm tra định kỳ theo mã sản phẩm
                var query = (from p in _context.PeriodicallyCheckingModel
                             from pr in p.ProductModel
                             where pr.ProductId == ProductId && p.Actived == true
                             select new PeriodicallyCheckingViewModel()
                             {
                                 PeriodicallyCheckingId = p.PeriodicallyCheckingId,
                                 PeriodicallyCheckingCode = p.PeriodicallyCheckingCode,
                                 PeriodicallyCheckingName = p.PeriodicallyCheckingName,
                                 Description = p.Description,
                                 FileUrl = ConstDomain.DomainPeriodicallyCheckingAPI + p.FileUrl
                             });

                var PeriodicallyChecking = query.FirstOrDefault();
                if (PeriodicallyChecking == null)
                {
                    return _APIError(LanguageResource.Mobile_NotFound);
                }
                //có nhiều hơn một chương trình
                string mess = string.Empty;
                if (query.Count() > 1)
                {
                    mess = LanguageResource.API_HasManyRow;
                }

                return _APISuccess(PeriodicallyChecking, mess);
            });
        }
        #endregion

        #region Get Plate Fee
        //Test: ProductId=33120e45-fdf1-49ce-8eeb-021bf077b6ab
        //GET
        //MasterData/api/GetPlateFeeBy?ProductId=33120e45-fdf1-49ce-8eeb-021bf077b6ab&token=454FC8F419313554549E2DED09B9AF94&key=77f430e1-66fd-48dc-8057-77935e53be20
        public ActionResult GetPlateFeeBy(Guid ProductId, string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                //Danh sách phí biển số theo mã sản phẩm
                var query = (from p in _context.PlateFeeModel
                             from pr in p.ProductModel
                             where pr.ProductId == ProductId && p.Actived == true
                             select p);
                var PlateFee = query.FirstOrDefault();
                if (PlateFee == null)
                {
                    return _APIError(LanguageResource.Mobile_NotFound);
                }
                //có nhiều hơn một chương trình
                string mess = string.Empty;
                if (query.Count() > 1)
                {
                    mess = LanguageResource.API_HasManyRow;
                }

                //Chi tiết các mức giá theo khu vực
                var detailList = (from d in _context.PlateFeeDetailModel
                                  where d.PlateFeeId == PlateFee.PlateFeeId
                                  select new PlateFeeDetailAPIViewModel()
                                  {
                                      Province = d.Province,
                                      PriceTmp = d.Price
                                  }).ToList();

                return _APISuccess(new
                {
                    PlateFeeList = detailList,
                    Description = PlateFee.Description
                }, mess);
            });
        }
        #endregion

        #region Get Promotion By Product
        //Test
        ///MasterData/api/GetPromotionBy?ProductId=844e5f95-e735-44e7-917b-1faad4c9f109&token=454FC8F419313554549E2DED09B9AF94&key=77f430e1-66fd-48dc-8057-77935e53be20
        public ActionResult GetPromotionBy(Guid ProductId, string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                //Query get promotion
                IQueryable<PromotionAPIViewModel> query = GetPromotion(ProductId);

                var Promotion = query.FirstOrDefault();
                if (Promotion == null)
                {
                    return _APIError(LanguageResource.Mobile_NotFound);
                }
                //có nhiều hơn một chương trình
                string mess = string.Empty;
                if (query.Count() > 1)
                {
                    mess = LanguageResource.API_HasManyRow;
                }

                return _APISuccess(Promotion, mess);
            });
        }

        private IQueryable<PromotionAPIViewModel> GetPromotion(Guid ProductId)
        {
            //Danh sách CTKM theo mã sản phẩm
            var currentDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            var query = (from p in _context.PromotionModel
                         from pr in p.ProductModel
                         where pr.ProductId == ProductId
                         //hiệu lực từ - đến
                         && p.EffectFromDate <= currentDate && currentDate <= p.EffectToDate
                         select new PromotionAPIViewModel()
                         {
                             PromotionCode = p.PromotionCode,
                             PromotionName = p.PromotionName,
                             Description = p.Description,
                         });
            return query;
        }
        #endregion

        #region Get Booking Time By Date And Store
        /// <summary>
        /// GetBookingTimeByDateAndStore
        /// Author: Tien
        /// </summary>
        /// <param name="BookingDate">24/12/2018</param>
        /// <param name="StoreId">2F28E30E-2603-4190-AB22-36ECBF959673</param>
        /// <returns></returns>
        public JsonResult GetBookingTimeByDateAndStore(string bookingDate, Guid? storeId, string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                #region
                //Ngày đặt
                DateTime? BookingDate = RepositoryLibrary.VNStringToDateTime(bookingDate);
                //Cửa hàng
                var Store = _context.StoreModel.Find(storeId);
                if (BookingDate == null || Store == null)
                {
                    //Dữ liệu không hợp lệ
                    return _APIError(LanguageResource.GetBookingTimeByDateAndStore_InputDataError);
                }
                else
                {
                    //Nam - 2018-12-19
                    //1. Nếu ngày nghỉ => Thông báo hết chỗ
                    //  Check trong table:
                    //2. Nếu là ngày làm việc
                    //  2.1 Kiểm tra múi giờ đặt lịch => Lấy danh sách "Giờ đặt", "Hạn mức tối đa"
                    //  2.2 Kiểm tra booking đã đặt ở từng múi giờ => update số lượng
                    #region  //1. Nếu ngày nghỉ => Thông báo hết chỗ
                    var dayoff = _context.WorkingDateModel.Where(p => p.DayOff == BookingDate && p.StoreId == storeId).FirstOrDefault();
                    if (dayoff != null)
                    {
                        return _APIError(LanguageResource.FullLoad);
                    }
                    #endregion
                    #region //2. Nếu là ngày làm việc
                    #region 2.1 Kiểm tra múi giờ đặt lịch => Lấy danh sách "Giờ đặt", "Hạn mức tối đa"
                    //Ngày đặt
                    var bookingDayOfWeek = (int)BookingDate.Value.DayOfWeek;
                    var workingTimeId = _context.WorkingTimeModel
                                        .Where(p => p.DayOfWeek == bookingDayOfWeek)
                                        .Select(p => p.WorkingTimeId).First();
                    var bookingTimeList = (from config in _context.WorkingTimeDetailModel
                                           where
                                           //Cửa hàng
                                           config.StoreId == storeId &&
                                           //Thứ của ngày đặt
                                           config.WorkingTimeId == workingTimeId &&
                                           //Chỉ lấy số lượng > 0
                                            config.Amount > 0

                                           select new BookingTimeViewModel()
                                           {
                                               TimeSpan = config.TimeFrameFrom,
                                               Number = config.Amount ?? 0
                                           }).ToList();
                    #endregion
                    #region 2.2 Kiểm tra booking đã đặt ở từng múi giờ => Update số lượng
                    //Số lượng đã đặt
                    var bookingList = (from booking in _context.BookingModel
                                       where booking.BookingDate == BookingDate
                                       group booking by booking.BookingTime into g
                                       select new
                                       {
                                           BookingTime = (from row2 in g select row2.BookingTime).FirstOrDefault(),
                                           TotalBooking = g.Count()
                                       }).ToList();
                    //Update số lượng
                    if (bookingList != null && bookingList.Count > 0)
                    {
                        foreach (var item in bookingTimeList)
                        {
                            var booked = bookingList
                                          .Where(p => string.Format("{0:D2}:{1:D2}", p.BookingTime.Value.Hours, p.BookingTime.Value.Minutes) == item.Time)
                                          .FirstOrDefault();
                            if (booked != null && booked.TotalBooking > 0)
                            {
                                item.Number = item.Number - booked.TotalBooking;
                            }
                        }
                    }
                    #endregion
                    #endregion
                    string Message = string.Empty;
                    if (bookingTimeList == null || bookingTimeList.Count == 0)
                    {
                        return _APIError(LanguageResource.FullLoad);
                    }
                    else if (bookingTimeList.Sum(p => p.Number) == 0)
                    {
                        return _APIError(LanguageResource.FullLoad);
                    }
                    return _APISuccess(bookingTimeList.OrderBy(a => a.Time), Message);
                }
                #endregion

            });
        }
        #endregion

        #region Perform Booking
        /// <summary>
        /// Tiến hành đă chổ
        /// Author: Tien
        /// </summary>
        /// <param name="model">Thông tin được gửi từ Mobile</param>
        /// 
        ///1. - Mã cửa hàng
        ///2. - Mã KH
        ///3. - Biển số xe
        ///4. - Loại xe
        ///5. - Tên KH
        ///6. - Số ĐT
        ///7. - Ngày đặt
        ///8. - Giờ đặt
        /// <returns></returns>
        public ActionResult PerformBooking(PerformBookingViewModel viewModel, string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                //Nếu là nhân viên không được đặt
                if (viewModel.CustomerId == new Guid("00000000-0000-0000-0000-000000000000"))
                {
                    return _APIError("Đăng nhập tài khoản khách hàng để đặt chổ!");
                }

                // TODO: SAVE data
                var modelAdd = new BookingModel();
                modelAdd.BookingModelId = Guid.NewGuid();
                //1. - Mã cửa hàng
                modelAdd.StoreId = viewModel.StoreId;
                //2. - Mã KH
                modelAdd.CustomerId = viewModel.CustomerId;
                //3. - Biển số xe
                modelAdd.Plate = viewModel.Plate;
                //4. - Loại xe
                modelAdd.BikeModel = viewModel.BikeModel;
                //5. - Tên KH
                modelAdd.FullName = viewModel.FullName;
                //6. - Số ĐT
                modelAdd.Phone = viewModel.Phone;
                //7. - Ngày đặt
                modelAdd.BookingDate = RepositoryLibrary.VNStringToDateTime(viewModel.BookingDate);
                //8. - Giờ đặt
                modelAdd.BookingTime = RepositoryLibrary.VNStringToTimeSpan(viewModel.BookingTime);
                //9. - Active
                modelAdd.Active = true;
                _context.Entry(modelAdd).State = EntityState.Added;
                _context.SaveChanges();

                #region Gửi thông báo đặt lịch hẹn sửa chữa tới nhân viên được cấu hình trong tài khoản
                //1. Cấu hình "Nhận thông báo đặt lịch":
                //1.1. Nhận thông báo trong ngày
                //      => Nếu đặt lịch vào ngày hiện tại => thông báo cho cửa hàng
                //1.2. Nhận toàn bộ thông báo
                //      => Luôn luôn gửi thông báo tới cửa hàng
                //2. Gửi thông báo tới cửa hàng
                //2.1. Lấy danh sách nhân viên nhận thông báo
                //2.2. Gửi notification cho toàn bộ nhân viên trên

                //isSentNotificationToStore: Có gửi thông báo
                var isSentNotificationToStore = false;

                //Cấu hình "Nhận thông báo đặt lịch"
                var config = _context.APIModel.FirstOrDefault();
                if (config != null && config.isReceiveInCurrentDay == true)
                {
                    // 1. Nhận thông báo trong ngày
                    if (modelAdd.BookingDate != null &&
                        modelAdd.BookingDate.HasValue &&
                        modelAdd.BookingDate.Value.Date.CompareTo(DateTime.Now.Date) == 0)
                    {
                        // => Nếu đặt lịch vào ngày hiện tại => thông báo cho cửa hàng
                        isSentNotificationToStore = true;
                    }
                }
                else
                {
                    // 2. Nhận toàn bộ thông báo
                    //  => Luôn luôn gửi thông báo tới cửa hàng
                    isSentNotificationToStore = true;
                }
                //2. Gửi thông báo tới cửa hàng
                if (isSentNotificationToStore)
                {
                    //2.1. Lấy danh sách nhân viên nhận thông báo
                    var deviceList = (from ac in _context.AccountModel
                                      where ac.isReceiveNotification == true &&
                                      ac.StoreModel.Any(r => r.StoreId == modelAdd.StoreId)
                                      select ac.DeviceId
                                       ).ToArray();

                    //2.2. Gửi notification cho toàn bộ nhân viên trên
                    if (deviceList != null && deviceList.Length > 0)
                    {
                        PushNotification(deviceList);
                    }
                }
                #endregion

                viewModel.BookingCode = modelAdd.BookingCode;
                return _APISuccess(JsonConvert.SerializeObject(viewModel), string.Format("Mã đặt chỗ của bạn: {0}", viewModel.BookingCode));
            });
        }
        #endregion

        #region Get Booked
        /// <summary>
        /// Lấy thông tin đã đặt lịch sửa chữa của khách hàng
        /// </summary>
        /// <param name="CustomerId">Mã khách hàng</param>
        ///<returns>
        ///1. - Mã phiếu đặt
        ///2. - Biển số xe
        ///3. - Ngày đặt
        ///4. - Giờ đặt
        ///5. - Trạng thái
        ///     5.1. Tên trạng thái
        ///     5.2. Loại trạng thái
        ///6. - Cửa hàng
        ///</returns>
        public JsonResult GetBooked(Guid customerId, string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                List<BookedResultViewModel> ret = new List<BookedResultViewModel>();
                ret = (from booking in _context.BookingModel
                       join store in _context.StoreModel on booking.StoreId equals store.StoreId
                       select new BookedResultViewModel()
                       {
                           ///1. - Mã phiếu đặt
                           BookingCode = booking.BookingCode,
                           ///2. - Biển số xe
                           Plate = booking.Plate,
                           ///3. - Ngày đặt
                           BookingDateTmp = booking.BookingDate,
                           ///4. - Giờ đặt
                           BookingTimeTmp = booking.BookingTime,
                           ///5. - Trạng thái
                           ///     5.1. Tên trạng thái
                           StatusName = booking.IsCreatedServiceOrder == true ? LanguageResource.BookingStatusComplted : LanguageResource.BookingStatusCreated,
                           ///     5.2. Loại trạng thái
                           Status = booking.IsCreatedServiceOrder == true,
                           ///6. - Cửa hàng
                           StoreName = store.StoreName

                       }).ToList();

                if (ret != null && ret.Count > 0)
                {
                    return _APISuccess(ret);
                }
                else
                {
                    return _APIError("Hiện bạn chưa đặt lịch sửa chữa lần nào!");
                }
            });
        }
        #endregion

        #region HomeContact
        public JsonResult GetContactList(string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                List<MobileContactList> contactList = (from c in _context.ContactDetailModel
                                                       orderby c.OrderIndex
                                                       select new MobileContactList()
                                                       {
                                                           Title = c.Title,
                                                           Phone = c.Phone,
                                                           PhoneWithFormat = c.DisplayPhone
                                                       }).ToList();

                if (contactList != null && contactList.Count > 0)
                {
                    var contact = _context.ContactModel.FirstOrDefault();

                    string ContactDescription = string.Empty;
                    string ReviewDescription = string.Empty;
                    if (contact != null)
                    {
                        ContactDescription = contact.ContactDescription;
                        ReviewDescription = contact.ReviewDescription;
                    }
                    var returnData = new
                    {
                        BeginDescription = ContactDescription,
                        ContactList = contactList,
                        EndDescription = ReviewDescription
                    };
                    return _APISuccess(returnData);
                }
                else
                {
                    return _APIError("Chưa có cấu hình!");
                }
            });
        }
        #endregion

        #region SearchProduct
        /// <summary>
        /// Tìm kiếm thông tin "Sản phẩm"
        /// </summary>
        /// <param name="ProductKeySearch">Thông tin sản phẩm</param>
        ///<returns>
        ///1. - Tên sản phẩm
        ///2. - Hình sản phẩm
        ///</returns>
        public JsonResult SearchProduct(string ProductKeySearch, string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                List<ProductMobileViewModel> result = new List<ProductMobileViewModel>();
                result = (from p in _context.ProductModel
                          where p.Actived == true
                          && (p.ERPProductCode.Contains(ProductKeySearch) || p.ProductCode.Contains(ProductKeySearch) || p.ProductName.Contains(ProductKeySearch))
                          orderby p.ProductName
                          select new ProductMobileViewModel()
                          {
                              ProductId = p.ProductId,
                              CategoryId = p.CategoryId,
                              ProductName = p.ProductName,
                          }
                          ).ToList();

                foreach (var product in result)
                {
                    var image = _context.ImageProductModel.Where(p => p.ProductId == product.ProductId && p.isDefault == true).Select(p => p.ImageUrl).FirstOrDefault();
                    product.ImageUrl = string.Format("{0}/{1}", ConstDomain.DomainImageProduct,
                                                                image != null ? image : ConstImageUrl.noImage);
                }

                if (result != null && result.Count > 0)
                {
                    return _APISuccess(result);
                }
                else
                {
                    return _APIError("Không tìm thấy sản phẩm. Vui lòng thử lại sau.");
                }
            });
        }
        #endregion SearchProduct

        #region SendMail
        public ActionResult SendMail(string FullName, string Phone, string Description, string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                try
                {

                    // TODO TIEN: Su dung email nay de gui mail (xemaytienthu.email@gmail.com)

                    //MailMessage mail = new MailMessage();
                    //mail.To.Add("xuan.tien@hotmail.com");

                    //mail.Subject = "Tien Test";
                    //string Body = "<p>Day la email test cua Xe May Tien Thu</p>";
                    //mail.Body = Body;
                    //mail.IsBodyHtml = true;

                    //SmtpClient smtp = new SmtpClient();
                    //mail.From = new MailAddress("xemaytienthu.email@gmail.com");
                    //smtp.Host = "smtp.gmail.com";
                    //smtp.Port = 587;
                    //smtp.UseDefaultCredentials = false;
                    ////Enter seders User name and password
                    //smtp.Credentials = new System.Net.NetworkCredential("xemaytienthu.email@gmail.com", "123@abcd");
                    //smtp.EnableSsl = true;
                    //smtp.Send(mail);


                    // TODO TIEN: tienthu.tk SendGrid (100 emails/day | 40,000 emails/month)
                    /// Sử dụng domain tienthu.tk
                    //MailMessage mail = new MailMessage();
                    //mail.To.Add("xuan.tien@hotmail.com");

                    //mail.Subject = "Tien Test";
                    //string Body = "<p>Day la email test cua Xe May Tien Thu</p>";
                    //mail.Body = Body;
                    //mail.IsBodyHtml = true;

                    //SmtpClient smtp = new SmtpClient();
                    //mail.From = new MailAddress("support@tienthu.tk");
                    //smtp.Host = "smtp.sendgrid.net";
                    //smtp.Port = 587;
                    //smtp.UseDefaultCredentials = false;
                    ////Enter seders User name and password
                    //smtp.Credentials = new System.Net.NetworkCredential("apikey", "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");
                    //smtp.EnableSsl = false;
                    //smtp.Send(mail);

                    // TODO TIEN: tienthu.tk SparkPost (25,000 emails/day | 100,000 emails/month)
                    /// Sử dụng domain tienthu.tk
                    //MailMessage mail = new MailMessage();
                    //mail.To.Add("xuan.tien@hotmail.com");

                    //mail.Subject = "Tien Test";
                    //string Body = "<p>Day la email test cua Xe May Tien Thu</p>";
                    //mail.Body = Body;
                    //mail.IsBodyHtml = true;

                    //SmtpClient smtp = new SmtpClient();
                    //mail.From = new MailAddress("support@tienthu.tk");
                    //smtp.Host = "smtp.sparkpostmail.com";
                    //smtp.Port = 587;
                    //smtp.UseDefaultCredentials = false;
                    ////Enter seders User name and password
                    //smtp.Credentials = new System.Net.NetworkCredential("SMTP_Injection", "e65286290753d1b5635a3a5a52f84fdc7cd31082");
                    //smtp.EnableSsl = true;
                    //smtp.Send(mail);


                    var configEmail = _context.EmailConfig.FirstOrDefault();
                    if (configEmail != null)
                    {
                        MailMessage mail = new MailMessage();
                        mail.To.Add(configEmail.ToEmail);
                        if (!string.IsNullOrEmpty(configEmail.CCMail))
                        {
                            mail.CC.Add(configEmail.CCMail);
                        }
                        if (!string.IsNullOrEmpty(configEmail.BCCMail))
                        {
                            mail.Bcc.Add(configEmail.BCCMail);
                        }
                        mail.Subject = configEmail.EmailTitle;
                        string Body = configEmail.EmailContent.Replace("{FullName}", FullName)
                                                              .Replace("{Phone}", Phone)
                                                              .Replace("{Description}", Description);
                        mail.Body = Body;
                        mail.IsBodyHtml = true;

                        SmtpClient smtp = new SmtpClient();
                        mail.From = new MailAddress(configEmail.SmtpMailFrom);
                        smtp.Host = configEmail.SmtpServer;
                        smtp.Port = Int32.Parse(configEmail.SmtpPort);
                        smtp.UseDefaultCredentials = false;
                        //Enter seders User name and password
                        smtp.Credentials = new System.Net.NetworkCredential(configEmail.SmtpUser, configEmail.SmtpPassword);
                        smtp.EnableSsl = (bool)configEmail.EnableSsl;
                        smtp.Send(mail);

                        //Trả về
                        return _APISuccess(new
                        {
                            Message = "Cám ơn bạn đã đóng góp ý kiến.",
                        });
                    }
                    else
                    {
                        return _APIError("Hệ thống đang bảo trì. Vui lòng thử lại sau.");
                    }
                }
                catch (Exception ex)
                {
                    return _APIError("Hệ thống đang bảo trì. Vui lòng thử lại sau.");
                }
            });
        }
        #endregion

        #region GetConfig
        public ActionResult GetConfig(string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                try
                {
                    var config = (from a in _context.APIModel
                                  select new
                                  {
                                      isRequiredLogin = a.isRequiredLogin
                                  }).FirstOrDefault();

                    if (config != null)
                    {
                        //Trả về
                        return _APISuccess(config);
                    }
                    else
                    {
                        return _APIError("Hệ thống đang bảo trì. Vui lòng thử lại sau.");
                    }
                }
                catch (Exception ex)
                {
                    return _APIError("Hệ thống đang bảo trì. Vui lòng thử lại sau.");
                }
            });
        }
        #endregion

        #region LogOut
        public ActionResult LogOut(string UserName, string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                try
                {
                    var account = _context.AccountModel.Where(p => p.UserName == UserName).FirstOrDefault();
                    account.DeviceId = null;
                    _context.Entry(account).State = EntityState.Modified;
                    _context.SaveChanges();
                    return _APISuccess(account);
                }
                catch (Exception ex)
                {
                    return _APIError("Hệ thống đang bảo trì. Vui lòng thử lại sau.");
                }
            });
        }
        #endregion

        #region GetProductListByCategory
        public ActionResult GetProductListByCategory(Guid CategoryId, string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                try
                {
                    var product = (from p in _context.ProductModel
                                   where p.Actived == true && p.CategoryId == CategoryId
                                   select new
                                   {
                                       ProductId = p.ProductId
                                   }).FirstOrDefault();

                    if (product != null)
                    {
                        //Trả về
                        return _APISuccess(product);
                    }
                    else
                    {
                        return _APIError("Đang cập nhật");
                    }
                }
                catch (Exception ex)
                {
                    return _APIError("Hệ thống đang bảo trì. Vui lòng thử lại sau.");
                }
            });
        }
        #endregion

        #region GetAllRouteForDebug

        [AllowAnonymous]
        public JsonResult GetAllRoute()
        {
            var listRoutes = GetAllRoutes().ToList();

            return Json(listRoutes, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        public JsonResult GetAllRouteUrl()
        {
            var listRoutes = GetAllRoutes().ToList();

            return Json(listRoutes.Select(l => l.Url).ToList(), JsonRequestBehavior.AllowGet);
        }

        private static IEnumerable<Route> GetAllRoutes()
        {
            //Get the executing assembly
            var assembly = Assembly.GetExecutingAssembly();

            //Get all classes that inherit from the Controller class that are public and not abstract
            //Replace Controller with ApiController for Web Api
            var types = assembly.GetTypes()
            .Where(t => t.IsSubclassOf(typeof(Controller)) && t.IsPublic && !t.IsAbstract);
            foreach (var type in types)
            {
                //Get the Controller Name minus the word Controller
                string controllerName = type.Name
                .Substring(0, type.Name.IndexOf("Controller", System.StringComparison.InvariantCulture));

                //Get all Methods within the inherited class
                var methods = type.GetMethods()
                .Where(x => x.IsPublic && x.DeclaringType.Equals(type));
                foreach (var method in methods)
                {
                    //Construct the initial url pattern which will contain the Controller and Method name.
                    string url = string.Format("{0}/{1}", controllerName, method.Name);

                    //Create a new Dictionary and add the controller name and method name
                    var routeDictionary = new Dictionary<string, object>();
                    routeDictionary.Add("controller", controllerName);
                    routeDictionary.Add("action", method.Name);

                    // Get all method parameters and add them to the dictionary
                    var paramInfo = method.GetParameters();
                    if (paramInfo.Count() > 0)
                    {
                        foreach (var parameter in paramInfo)
                        {
                            //Append the url format with the parameter name
                            url += "/{" + parameter.Name + "}";

                            //Check if parameter is optional and add the name and value to the dictionary
                            if (parameter.IsOptional)
                                routeDictionary.Add(parameter.Name, UrlParameter.Optional);
                            else
                                routeDictionary.Add(parameter.Name, parameter.Name);
                        }
                    }
                    yield return new Route(url, new RouteValueDictionary(routeDictionary), new MvcRouteHandler());
                }
            }
        }

        #endregion

        #region Helper
        protected JsonResult ExecuteAPIContainer(string token, string key, Func<JsonResult> codeToExecute)
        {
            if (token == tokenConst && key == keyConst)
            {
                try
                {
                    // All code will run here
                    // Usage: return ExecuteContainer(() => { ALL RUNNING CODE HERE, remember to return });
                    return codeToExecute.Invoke();
                }
                catch (Exception ex)
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        Data = "",
                        Error = ex.Message
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new
                {
                    IsSuccess = false,
                    Data = "",
                    Error = LanguageResource.Account_AccessDenied
                }, JsonRequestBehavior.AllowGet);
            }
        }
        protected JsonResult _APIError(string errorMessage, object data = null)
        {
            return Json(new
            {
                IsSuccess = false,
                Data = data,
                Error = errorMessage
            }, JsonRequestBehavior.AllowGet);
        }
        protected JsonResult _APISuccess(object data, string message = "")
        {
            return Json(new
            {
                IsSuccess = true,
                Data = data,
                Error = message
            }, JsonRequestBehavior.AllowGet);
        }
        #endregion Helper

        #region Gửi thông báo
        private void PushNotification(string[] playerIds)
        {
            var request = WebRequest.Create(ConstPushNotification.PushNotificationUrl) as HttpWebRequest;

            request.KeepAlive = true;
            request.Method = "POST";
            request.ContentType = "application/json; charset=utf-8";

            request.Headers.Add("authorization", "Basic " + ConstPushNotification.RESTAPIKey);

            var serializer = new JavaScriptSerializer();
            var obj = new
            {
                app_id = ConstPushNotification.AppId,
                // OneSignal uses English as the default language so the field must be filled. 
                // However if you only want to send your message in one language you can place it under "en"
                headings = new { en = LanguageResource.Notification_NewBooking },
                contents = new { en = LanguageResource.Notification_BookingContent },
                data = new { openAction = ConstPushNotification.DatLich },
                include_player_ids = playerIds
            };
            var param = serializer.Serialize(obj);
            byte[] byteArray = Encoding.UTF8.GetBytes(param);

            string responseContent = null;
            string message = null;

            try
            {
                using (var writer = request.GetRequestStream())
                {
                    writer.Write(byteArray, 0, byteArray.Length);
                }

                using (var response = request.GetResponse() as HttpWebResponse)
                {
                    using (var reader = new StreamReader(response.GetResponseStream()))
                    {
                        responseContent = reader.ReadToEnd();
                    }
                }
            }
            catch (WebException ex)
            {
                message = ex.Message;
            }
        }
        #endregion

        #region Cập nhật trạng thái đọc thông báo 
        public ActionResult UpdateIsReadNotification(Guid CustomerId, Guid NotificationId, string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                try
                {
                    //Ưu đãi của bạn
                    var detail = _context.CustomerGiftDetailModel.Where(p => p.CustomerId == CustomerId && p.GiftId == NotificationId).FirstOrDefault();
                    if (detail != null)
                    {
                        detail.isRead = true;
                        _context.Entry(detail).State = EntityState.Modified;

                    }
                    //Kiểm tra định kỳ
                    var checkingTimes = (from cus in _context.CustomerModel
                                         join noti in _context.CheckingTimesNotificationModel on cus.CustomerCode equals noti.CustomerCode
                                         where cus.CustomerId == CustomerId && noti.CheckingTimesId == NotificationId
                                         select noti
                                         ).FirstOrDefault();
                    if (checkingTimes != null)
                    {
                        checkingTimes.isRead = true;
                        _context.Entry(checkingTimes).State = EntityState.Modified;
                    }

                    _context.SaveChanges();
                    return _APISuccess(null);
                }
                catch (Exception ex)
                {
                    return _APIError(ex.Message);
                }
            });
        }
        #endregion  Cập nhật trạng thái đọc thông báo 

        #region Kiểm tra nhân viên đăng nhập trong giờ làm việc
        public ActionResult CheckLoginPermission(string UserName, string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                try
                {
                    bool isHasPermission;
                    CheckLoginPermissionByTime(UserName, out isHasPermission);
                    return _APISuccess(isHasPermission);
                }
                catch (Exception ex)
                {
                    return _APIError(ex.Message);
                }
            });
        }

        private void CheckLoginPermissionByTime(string UserName, out bool isHasPermission)
        {
            isHasPermission = false;
            //Nếu tài khoản có thiết lập phân quyền đăng nhập thì kiểm tra thời gian được phép đăng nhập
            var account = (from p in _context.AccountModel
                           from r in p.RolesModel
                           where p.UserName == UserName
                           && r.isCheckLoginByTime == true
                           select r).FirstOrDefault();

            if (account != null)
            {
                DateTime currentDate = DateTime.Now;
                DateTime WorkingTimeFrom = (DateTime)account.WorkingTimeFrom;
                DateTime WorkingTimeTo = (DateTime)account.WorkingTimeTo;
                if (TimeSpan.Compare(currentDate.TimeOfDay, WorkingTimeFrom.TimeOfDay) > -1
                        && TimeSpan.Compare(currentDate.TimeOfDay, WorkingTimeTo.TimeOfDay) < 1)
                {
                    isHasPermission = true;
                }
            }
            else
            {
                isHasPermission = true;
            }
        }
        #endregion Kiểm tra nhân viên đăng nhập trong giờ làm việc

        #region Kiểm tra định kỳ
        //Linh 2019-03-13
        //API gửi thông báo kiểm tra định kỳ cho mobile app
        //Cung cấp cho Tiến Thu sử dụng
        //Input: Mã KH, Nội dung thông báo
        //Output:  + IsSuccess: true | false
        //         + Message: 
        public ActionResult UpdateCheckTimesNotification(List<CheckingTimesNotificationViewModel> model, string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                try
                {
                    List<string> errorList = new List<string>();
                    List<string> playerIdList = new List<string>();
                    if (model != null && model.Count > 0)
                    {
                        foreach (var item in model)
                        {
                            var customer = _context.CustomerModel.Where(p => p.CustomerCode == item.CustomerCode).FirstOrDefault();
                            if (customer != null)
                            {
                                CheckingTimesNotificationModel addModel = new CheckingTimesNotificationModel();
                                addModel.CheckingTimesId = Guid.NewGuid();
                                addModel.AccountId = customer.AccountId;
                                addModel.CustomerCode = item.CustomerCode;
                                addModel.CheckingTimesDescription = item.CheckingTimesDescription;
                                addModel.isRead = false;

                                _context.Entry(addModel).State = EntityState.Added;
                                _context.SaveChanges();

                                var deviceId = _context.AccountModel.Where(p => p.AccountId == customer.AccountId).Select(p => p.DeviceId).FirstOrDefault();
                                playerIdList.Add(deviceId);
                            }
                            else
                            {
                                errorList.Add(string.Format("Mã KH \"{0\" không tồn tại trong hệ thống", item.CustomerCode));
                            }
                        }

                        //Gửi thông báo
                        var deviceList = playerIdList.ToArray();
                        PushNotification(deviceList);

                        if (errorList != null && errorList.Count > 0)
                        {
                            return Json(new
                            {
                                Status = "Fail",
                                Message = errorList,
                            }, JsonRequestBehavior.AllowGet);
                        }

                        return Json(new
                        {
                            Status = "Success",
                            Message = "Cập nhật thông báo kiểm tra định kỳ thành công!"
                        }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new
                        {
                            Status = "Fail",
                            Message = "Không tìm thấy dữ liệu!"
                        }, JsonRequestBehavior.AllowGet);
                    }
                }
                catch (Exception ex)
                {
                    return Json(new
                    {
                        Status = "Fail",
                        Message = ex.Message
                    }, JsonRequestBehavior.AllowGet);
                }
            });
        }
        #endregion Kiểm tra định kỳ

        #region Kiểm tra tài khoản đăng nhập chỉ trên 1 thiết bị
        public ActionResult CheckLoginDevice(string UserName, string DeviceId, string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                try
                {
                    bool isLogOut = false;
                    //So sánh DeviceId trên mobile và trong dữ liệu
                    //Nếu giống nhau => vẫn đăng nhập bình thường
                    //Nếu khác nhau => đăng xuất khỏi thiết bị
                    var account = _context.AccountModel.Where(p => p.UserName == UserName).FirstOrDefault();
                    if (account != null)
                    {
                        if (account.DeviceId != null && account.DeviceId != DeviceId)
                        {
                            isLogOut = true;
                        }
                    }
                    return _APISuccess(isLogOut);
                }
                catch (Exception ex)
                {
                    return _APIError(ex.Message);
                }
            });
        }
        #endregion Kiểm tra tài khoản đăng nhập chỉ trên 1 thiết bị

        #region Lấy Url hiển thị báo cáo trên mobile 
        public ActionResult GetUrlReport(string type, string token, string key)
        {
            return ExecuteAPIContainer(token, key, () =>
            {
                try
                {
                    string url = string.Empty;
                    if (type == ConstReport.ServiceHistoryReport)
                    {
                        url = string.Format("{0}/Reports/ServiceHistory", ConstDomain.Domain);
                        //url = "http://tienthu.seagleyes.com/Reports/ServiceHistory";
                    }
                    else
                    {
                        url = string.Format("{0}/Reports/Customer", ConstDomain.Domain);
                        //url = "http://tienthu.seagleyes.com/Reports/Customer";
                    }

                    return _APISuccess(url);
                }
                catch (Exception ex)
                {
                    return _APIError(ex.Message);
                }
            });
        }
        #endregion Lấy Url hiển thị báo cáo trên mobile
    }
}