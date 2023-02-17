using BotDetect.Web.Mvc;
using ISD.Constant;
using ISD.Core;
using ISD.EntityModels;
using ISD.Repositories;
using ISD.Resources;
using ISD.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Web.WebPages;
using Warranty.Models;

namespace Warranty.Controllers
{
    public class ActivedWarrantyController : BaseController
    {
        private CatalogRepository _catalogRepository;

        public ActivedWarrantyController()
        {
            _catalogRepository = new CatalogRepository(_context);
        }

        // GET: ActivedWarranty
        public ActionResult Index()
        {
            return View();
        }

        #region Check Captcha và số Serial/số OD

        [HttpPost]
        [CaptchaValidationActionFilter("CaptchaCodeCheck", "regWarCaptcha", "Mã xác nhận không đúng!")]
        public ActionResult _CheckWarranty(RegWarrantyCheckModel regWarranty)
        {
            if (ModelState.IsValid)
            {
                //Check bằng số serial
                if (regWarranty.WarrantyType == "SerialNo")
                {
                    if (string.IsNullOrEmpty(regWarranty.SerialCheck))
                    {
                        ModelState.AddModelError("SerialCheck", "Số serial không được bỏ trống");
                        MvcCaptcha.ResetCaptcha("regWarCaptcha");
                        return ValidationInvalid();
                    }
                    //Tách serial
                    var serialList = regWarranty.SerialCheck.Split(',');

                    var productWarrantyExist = new List<ProductWarrantyModel>();
                    //Kiểm tra xem có sản phẩm đã kích hoạt BH chưa?
                    // res != null => đã có sp kích hoạt rồi => báo lỗi và ko tiến hành kích hoạt.
                    foreach (var item in serialList)
                    {
                        var serial = item.Trim();
                        if (!string.IsNullOrEmpty(serial))
                        {
                            if (serial.Length > 18)
                            {
                                return Json(new
                                {
                                    Code = HttpStatusCode.BadRequest,
                                    Success = false,
                                    Data = "Serial không hợp lệ. Vui lòng kiểm tra lại!"
                                });
                            }
                            var res = (from p in _context.ProductWarrantyModel
                                       where p.SerriNo == serial
                                       select p).FirstOrDefault();
                            if (res != null)
                            {
                                productWarrantyExist.Add(res);
                            }
                        }
                        
                    }
                    MvcCaptcha.ResetCaptcha("regWarCaptcha");

                    //Chưa có mã đăng ký
                    if (productWarrantyExist == null || productWarrantyExist.Count == 0)
                    {
                        //Kiểm tra serial
                        var response = new ResponseViewModel();
                        foreach (var item in serialList)
                        {
                            var serial = item.Trim();
                            if (!string.IsNullOrEmpty(serial))
                            {
                                //var model = _unitOfWork.WarrantyRepository.GetSerialInfor(serial);
                                var result = _unitOfWork.WarrantyRepository.CheckSerialReady(serial);
                                //Nếu trong các serial gửi lên không hợp lệ thì return luôn.
                                if (result == false)
                                {
                                    response.Code = HttpStatusCode.BadRequest;
                                    response.Success = false;
                                    response.Data = "Có số serial không hợp lệ. Vui lòng kiểm tra lại!";

                                    return Json(response, JsonRequestBehavior.AllowGet);
                                }
                            }
                        }
                        //Các serial gửi lên đều ok
                        response.Code = HttpStatusCode.OK;
                        response.Success = true;
                        return Json(response, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        //Đã có mã đăng ký
                        var message = string.Empty;
                        foreach (var item in productWarrantyExist)
                        {
                            message += "Sản phẩm " + item.ProductModel.ProductName + " đã được đăng ký bảo hành vào ngày: " + item.FromDate.ToString("dd/MM/yyyy") + ".<br/>";
                        }
                        return Json(new
                        {
                            Code = HttpStatusCode.OK,
                            Success = false,
                            Data = message + "Vui lòng kiểm tra và xoá serial đã kích hoạt ra khỏi danh sách serial!"
                        });
                    }
                }
                //Check bằng số OD
                else if (regWarranty.WarrantyType == "OrderDelivery")
                {
                    if (string.IsNullOrEmpty(regWarranty.OrderDelivery))
                    {
                        ModelState.AddModelError("OrderDelivery", "Số OD không được bỏ trống");
                        MvcCaptcha.ResetCaptcha("regWarCaptcha");
                        return ValidationInvalid();
                    }
                    MvcCaptcha.ResetCaptcha("regWarCaptcha");

                    var productList = _unitOfWork.WarrantyRepository.GetOrderDetails(regWarranty.OrderDelivery);
                    if (productList != null && productList.Count > 0)
                    {
                        var response = new ResponseViewModel();
                        //Các serial gửi lên đều ok
                        response.Code = HttpStatusCode.OK;
                        response.Success = true;
                        return Json(response, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        var dataTable = _unitOfWork.WarrantyRepository.GetOrderInfo(regWarranty.OrderDelivery);
                        if (dataTable == null || dataTable.Rows.Count == 0)
                        {
                            return Json(new
                            {
                                Code = HttpStatusCode.OK,
                                Success = false,
                                Data = "OD-Giao hàng: " + regWarranty.OrderDelivery + " không có mặt hàng để kích hoạt bảo hành. Xin kiểm tra lại!"
                            });
                        }
                        else
                        {
                            return Json(new
                            {
                                Code = HttpStatusCode.OK,
                                Success = false,
                                Data = "Tất cả sản phẩm của phiếu giao hàng " + regWarranty.OrderDelivery + " đã được kích hoạt bảo hành!"
                            });
                        }
                            
                    }
                }
                return Json(null, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return ValidationInvalid();
            }
        }

        #endregion Check Captcha và số Serial/số OD

        #region Tạo khách hàng mới và phiếu Đăng ký bảo hành

        public ActionResult Create(string Serial = null, string OrderDelivery = null)
        {
            WarrantyCustomerViewModel profile = new WarrantyCustomerViewModel();
            var productActivedList = new List<ProductActivedViewModel>();
            //Nếu tạo bằng số serial
            if (!string.IsNullOrEmpty(Serial))
            {
                if (!Serial.Contains(","))
                {
                    if (Serial.Contains(" "))
                    {
                        Serial = Serial.Replace(" ", ", ");
                    }
                }
                ViewBag.Serial = Serial;
                ViewBag.Title = LanguageResource.Actived_Warranty + " Serial: " + Serial;
            }
            //Nếu tạo bằng số OD
            else if (!string.IsNullOrEmpty(OrderDelivery))
            {
                ViewBag.OrderDelivery = OrderDelivery;
                ViewBag.Title = LanguageResource.Actived_Warranty + " OD: " + OrderDelivery;

                var productList = _unitOfWork.WarrantyRepository.GetOrderDetails(OrderDelivery);
                if (productList != null && productList.Count > 0)
                {
                    var existProfile = productList.FirstOrDefault();
                    profile.ProfileName = existProfile.ProfileName;
                    profile.Address = existProfile.ProfileAddress;
                    profile.Phone = existProfile.Phone;
                    foreach (var item in productList)
                    {
                        var productActived = new ProductActivedViewModel
                        {
                            ERPProductCode = item.ProductCode,
                            ProductName = item.ProductName,
                            SerialNumber = item.SerialNo,
                            CompanyCode = item.CompanyCode,
                            ProfileForeignCode = item.ProfileForeignCode,
                            DocumentDate = item.DocumentDate,
                            PostDate = item.PostDate,
                            SaleOrderCode = item.SaleOrderCode,
                            WarrantyCode = item.WarrantyCode,
                            Quantity = item.Quantity,
                            Unit = item.Unit
                        };
                        productActivedList.Add(productActived);
                    }
                }
            }
            ViewBag.ProductActivedList = productActivedList;
            CreateViewBag();
            return View(profile);
        }

        [HttpPost]
        public ActionResult Create(ProfileViewModel profileViewModel, List<ProductActivedViewModel> productActivedList, string Serial = null, string OrderDelivery = null)
        {
            return ExecuteContainer(() =>
            {
                var isValidationFailed = false;
                var Message = "";
                #region ValidateField

                if (profileViewModel.CustomerTypeCode == ConstCustomerType.Bussiness)
                {
                    if (string.IsNullOrEmpty(profileViewModel.CompanyNumber))
                    {
                        ModelState.AddModelError("CompanyNumber", "Vui lòng nhập số điện thoại công ty");
                        isValidationFailed = true;
                    }
                    if (profileViewModel.ProvinceId == null || profileViewModel.ProvinceId == Guid.Empty)
                    {
                        ModelState.AddModelError("ProvinceId", "Vui lòng nhập thông tin Tỉnh/Thành phố");
                        isValidationFailed = true;
                    }
                    if (string.IsNullOrEmpty(profileViewModel.ContactName))
                    {
                        ModelState.AddModelError("ContactName", "Vui lòng nhập thông tin liên hệ");
                        isValidationFailed = true;
                    }
                    if (string.IsNullOrEmpty(profileViewModel.PhoneBusiness))
                    {
                        ModelState.AddModelError("PhoneBusiness", "Vui lòng nhập thông tin số điện thoại liên hệ");
                        isValidationFailed = true;
                    }
                }
                else if (profileViewModel.CustomerTypeCode == ConstCustomerType.Customer)
                {
                    if (string.IsNullOrEmpty(profileViewModel.Phone))
                    {
                        ModelState.AddModelError("Phone", "Vui lòng nhập thông tin số điện thoại liên hệ");
                        isValidationFailed = true;
                    }
                    if (profileViewModel.ProvinceId == null || profileViewModel.ProvinceId == Guid.Empty)
                    {
                        ModelState.AddModelError("ProvinceId", "Vui lòng nhập thông tin Tỉnh/Thành phố");
                        isValidationFailed = true;
                    }
                }

                if (isValidationFailed)
                {
                    return ValidationInvalid();
                }

                #endregion ValidateField

                var CompanyCode = ConfigUtilities.GetSysConfigAppSetting("CompanyCode");
                var company = _context.CompanyModel.FirstOrDefault(p => p.CompanyCode == CompanyCode);

                bool isReady = true;
                List<string> serialList = new List<string>();
                List<ProductRegWarrantyViewModel> productList = new List<ProductRegWarrantyViewModel>();

                #region Check serial | OD hợp lệ

                var OD_Serial = string.Empty;
                if (!string.IsNullOrEmpty(Serial))
                {
                    //- Sau khi các input hợp lệ
                    //- Tách serial (Distinct gộp trùng lặp)
                    //- Check lại serial lần nữa
                    serialList = Serial.Split(',').Distinct().ToList();
                    Serial = string.Empty;

                    //- Kiểm tra danh sách serial nếu có mã ko hợp lệ thì response lỗi
                    //- Check xem serial đó đã đăng ký bảo hành chưa thêm lần nữa
                    //- Trả về 1 biến isready: tất cả các serial với 2 điều kiện đều ok.
                    foreach (var item in serialList)
                    {
                        if (!string.IsNullOrEmpty(item))
                        {
                            var serialClean = item.Trim();
                            //Kiểm tra Serial hợp lệ
                            var serialResult = _unitOfWork.WarrantyRepository.CheckSerialReady(serialClean);
                            //Kiểm tra có serial kích hoạt chưa. WarrantyExist = null => ok
                            var WarrantyExist = _context.ProductWarrantyModel.FirstOrDefault(p => p.SerriNo == serialClean);
                            //Có serial không hợp lệ
                            if (WarrantyExist != null || serialResult == false)
                            {
                                isReady = false;
                                break;
                            }
                            Serial += serialClean + ", ";
                        }
                    }
                }
                else if (!string.IsNullOrEmpty(OrderDelivery))
                {
                    //Lấy tất cả sản phẩm trong OD
                    //var products = _unitOfWork.WarrantyRepository.GetOrderDetails(OrderDelivery);

                    if (productActivedList != null && productActivedList.Count > 0)
                    {
                        foreach (var item in productActivedList)
                        {
                            if (item.isActivedWarranty)
                            {
                                //Nếu có serial thì kiểm tra số serial có hợp lệ hay không
                                if (!string.IsNullOrEmpty(item.SerialNumber))
                                {
                                    var serialResult = _unitOfWork.WarrantyRepository.CheckSerialReady(item.SerialNumber);
                                    if (serialResult == false)
                                    {
                                        isReady = false;
                                        break;
                                    }
                                    OD_Serial += item.SerialNumber + ", ";
                                }
                                //Check xem số lượng kích hoạt có vượt quá số lượng hiện tại không
                                if (item.ActivationQuantity > item.Quantity)
                                {
                                    isReady = false;
                                    break;
                                }

                                var product = new ProductRegWarrantyViewModel
                                {
                                    ProductCode = item.ERPProductCode,
                                    ProductName = item.ProductName,
                                    Serial = item.SerialNumber,
                                    ProfileForeignCode = item.ProfileForeignCode,
                                    Duration = item.WarrantyCode,
                                    SaleOrder = item.SaleOrderCode,
                                    ActivatedQuantity = item.ActivationQuantity,
                                    DocumentDate = item.DocumentDate,
                                    PostDate = item.PostDate
                                };
                                productList.Add(product);
                            }
                        }
                    }
                }

                #endregion Check serial | OD hợp lệ

                if (isReady)
                {
                    var productRepository = new ProductRepository(_context);
                    //SYSTEM
                    var accountCreateId = _context.AccountModel.FirstOrDefault(p => p.UserName == "SYSTEM").AccountId;

                    #region Thêm mới khách hàng

                    var profileId = Guid.NewGuid();
                    ProfileModel existProfile = new ProfileModel();

                    //Nếu kích hoạt bằng Serial, kiểm tra theo SĐT
                    if (!string.IsNullOrEmpty(Serial))
                    {
                        var profilePhone = RepositoryLibrary.ConvertToNoSpecialCharacters(profileViewModel.Phone.Trim().Replace(" ", ""));
                        existProfile = _context.ProfileModel.Where(p => p.Phone == profilePhone).FirstOrDefault();
                    }
                    //Nếu kích hoạt bằng số OD, kiểm tra theo mã SAP khách hàng
                    else if (!string.IsNullOrEmpty(OrderDelivery))
                    {
                        if (productList != null && productList.Count > 0)
                        {
                            var ProfileForeignCode = productList.Select(p => p.ProfileForeignCode).FirstOrDefault();
                            existProfile = _context.ProfileModel.FirstOrDefault(p => p.ProfileForeignCode == ProfileForeignCode);
                        }
                    }

                    //Nếu chưa có KH trong db => thêm mới
                    if (existProfile == null)
                    {
                        profileViewModel.ProfileForeignCode = productList.Select(p => p.ProfileForeignCode).FirstOrDefault();
                        //Lấy mặc định là khách Tiêu dùng
                        if (string.IsNullOrEmpty(profileViewModel.CustomerTypeCode))
                        {
                            profileViewModel.CustomerTypeCode = ConstCustomerType.Customer;
                        }
                        //Lấy chi nhánh mặc định theo cty
                        profileViewModel.CreateAtSaleOrg = CompanyCode;
                        //Lấy khu vực theo tỉnh thành
                        var province = _context.ProvinceModel.FirstOrDefault(p => p.ProvinceId == profileViewModel.ProvinceId);
                        if (province != null)
                        {
                            profileViewModel.SaleOfficeCode = province.Area.ToString();
                        }
                        profileViewModel.ProfileTypeCode = ConstCustomerType.Account;
                        var profileGroupList = new List<ProfileGroupCreateViewModel>();
                        var personInChargeList = new List<PersonInChargeViewModel>();
                        //Ngồn khách hàng: Mặc định là khác
                        profileViewModel.CustomerSourceCode = "OTHER";

                        //Nếu là Doanh nghiệp => Lưu thông tin Loại địa chỉ, Nhóm khách hàng & Ngành nghề
                        if (profileViewModel.CustomerTypeCode == ConstCustomerType.Bussiness)
                        {
                            profileViewModel.Phone = RepositoryLibrary.ConvertToNoSpecialCharacters(profileViewModel.CompanyNumber);
                            profileViewModel.AddressTypeCode = "GH";
                            profileViewModel.CustomerCareerCode = "9999";
                            profileGroupList.Add(new ProfileGroupCreateViewModel { ProfileGroupCode = "99" });
                        }
                        //Nếu là Tiêu dùng => Lưu Loại địa chỉ
                        else if (profileViewModel.CustomerTypeCode == ConstCustomerType.Customer)
                        {
                            profileViewModel.AddressTypeCode = "NR";
                        }

                        //NV kinh doanh
                        personInChargeList.Add(new PersonInChargeViewModel
                        {
                            ProfileId = profileId,
                            SalesEmployeeCode = ConstCommon.ChuaXacDinh,
                            CompanyCode = CompanyCode
                        });

                        List<string> errorList = new List<string>();
                        ProfileModel modelAdd = new ProfileModel()
                        {
                            ProfileId = profileId
                        };
                        _unitOfWork.ProfileRepository.CreateProfile(profileViewModel, modelAdd, null, null, null, null, personInChargeList, null, profileGroupList, accountCreateId, CompanyCode, null, out errorList);

                        //Return errors
                        if (errorList != null && errorList.Count > 0)
                        {
                            return Json(new
                            {
                                Code = HttpStatusCode.BadRequest,
                                Success = false,
                                Data = errorList
                            });
                        }
                    }
                    else
                    {
                        profileId = existProfile.ProfileId;
                    }

                    #endregion Thêm mới khách hàng

                    #region Thêm phiếu Đăng ký bảo hành theo từng sản phẩm

                    //Thời hạn bảo hành mặc định (12 tháng)
                    var warrantyId = _context.WarrantyModel.FirstOrDefault(p => p.WarrantyCode == "BH12T").WarrantyId;

                    #region Theo Serial

                    if (!string.IsNullOrEmpty(Serial))
                    {
                        //Foreach các serial và tạo phiếu đăng ký bảo hành theo serial
                        //Đăng ký bảo hành
                        //Default BH 12T
                        foreach (var item in serialList)
                        {
                            if (!string.IsNullOrEmpty(item))
                            {
                                ProductWarrantyModel productWarrantyNew = new ProductWarrantyModel();
                                productWarrantyNew.ProductWarrantyId = Guid.NewGuid();
                                productWarrantyNew.ProfileId = profileId;
                                productWarrantyNew.FromDate = DateTime.Now;
                                productWarrantyNew.CreateTime = DateTime.Now;
                                productWarrantyNew.Actived = true;
                                //Thông tin khách hàng
                                productWarrantyNew.ProfileName = profileViewModel.ProfileName;
                                productWarrantyNew.ProfileShortName = profileViewModel.ProfileShortName;
                                productWarrantyNew.Age = profileViewModel.Age;
                                productWarrantyNew.Phone = profileViewModel.Phone;
                                productWarrantyNew.Email = profileViewModel.Email;
                                productWarrantyNew.ProvinceId = profileViewModel.ProvinceId;
                                productWarrantyNew.DistrictId = profileViewModel.DistrictId;
                                productWarrantyNew.WardId = profileViewModel.WardId;
                                productWarrantyNew.Address = profileViewModel.Address;
                                productWarrantyNew.Note = profileViewModel.Note;

                                var serialClean = item.Trim();
                                //Tìm sản phẩm trong CRM theo mã SAP. 
                                var productId = _unitOfWork.WarrantyRepository.GetProductIdBySerial(serialClean);
                                if (productId == null || productId == Guid.Empty)
                                {

                                    return Json(new ResponseViewModel
                                    {
                                        Code = HttpStatusCode.BadRequest,
                                        Success = false,
                                        Data = "Không tìm thấy thông tin sản phẩm!"
                                    }, JsonRequestBehavior.AllowGet);
                                }
                                var proWarranty = _context.ProductModel.FirstOrDefault(p => p.ProductId == productId)?.WarrantyId;
                                //Nếu SP có thông tin thời hạn bảo hành => lấy time bảo hành theo SP <> Mặc định 12 tháng
                                if (proWarranty != null && proWarranty != Guid.Empty)
                                {
                                    warrantyId = (Guid)proWarranty;
                                }
                                productWarrantyNew.WarrantyId = warrantyId;
                                productWarrantyNew.ProductId = productId;
                                productWarrantyNew.SerriNo = serialClean;
                                productWarrantyNew.ToDate = GetToDateTime(warrantyId, DateTime.Now);
                                productWarrantyNew.CompanyId = company.CompanyId;
                                _context.Entry(productWarrantyNew).State = EntityState.Added;
                            }
                        }
                    }

                    #endregion Theo Serial

                    #region Theo OD

                    else if (!string.IsNullOrEmpty(OrderDelivery))
                    {
                        if (productList != null && productList.Count > 0)
                        {
                            foreach (var item in productList)
                            {
                                ProductWarrantyModel productWarrantyNew = new ProductWarrantyModel();
                                productWarrantyNew.ProductWarrantyId = Guid.NewGuid();
                                productWarrantyNew.ProfileId = profileId;

                                if (item.PostDate != null)
                                {
                                    productWarrantyNew.FromDate = (DateTime)item.PostDate;
                                }
                                else
                                {
                                    productWarrantyNew.FromDate = DateTime.Now;
                                }
                                productWarrantyNew.CreateTime = DateTime.Now;
                                productWarrantyNew.Actived = true;
                                //Thông tin khách hàng
                                productWarrantyNew.ProfileName = profileViewModel.ProfileName;
                                productWarrantyNew.ProfileShortName = profileViewModel.ProfileShortName;
                                productWarrantyNew.Age = profileViewModel.Age;
                                productWarrantyNew.Phone = profileViewModel.Phone;
                                productWarrantyNew.Email = profileViewModel.Email;
                                productWarrantyNew.ProvinceId = profileViewModel.ProvinceId;
                                productWarrantyNew.DistrictId = profileViewModel.DistrictId;
                                productWarrantyNew.WardId = profileViewModel.WardId;
                                productWarrantyNew.Address = profileViewModel.Address;
                                productWarrantyNew.Note = profileViewModel.Note;
                                productWarrantyNew.CompanyId = company.CompanyId;


                                var productId = Guid.Empty;
                                var matType = "Z" + item.ProductCode.Substring(0, 2);
                                //AConcept k có số serial
                                if (string.IsNullOrEmpty(item.Serial))
                                {
                                    productId = _unitOfWork.WarrantyRepository.GetProductIdByERPCode(item.ProductCode, CompanyCode);
                                    if (productId == null || productId == Guid.Empty)
                                    {
                                        productId = productRepository.SyncMaterial(CompanyCode, 1, matType, item.ProductCode);
                                        if (productId == Guid.Empty)
                                        {
                                            return Json(new
                                            {
                                                Code = HttpStatusCode.InternalServerError,
                                                Success = false,
                                                Data = "Kích hoạt không thành công.<br/> Có lỗi trong quá trình đồng bộ sản phẩm mã: " + item.ProductCode + "!"
                                            });
                                        }
                                    }
                                }
                                else
                                {
                                 
                                    productId = _unitOfWork.WarrantyRepository.GetProductIdBySerial(item.Serial);
                                   if (productId == null || productId == Guid.Empty)
                                    {
                                        productId = productRepository.SyncMaterial(CompanyCode, 1, matType, item.ProductCode);
                                        if (productId == Guid.Empty)
                                        {
                                            return Json(new
                                            {
                                                Code = HttpStatusCode.InternalServerError,
                                                Success = false,
                                                Data = "Kích hoạt không thành công.<br/> Có lỗi trong quá trình đồng bộ sản phẩm mã: " + item.ProductCode + "!"
                                            });
                                        }

                                    }
                                }

                                // var proWarranty = _context.ProductModel.FirstOrDefault(p => p.ProductId == productId).WarrantyId;

                                //if (proWarranty == null || proWarranty == Guid.Empty)
                                //{
                                if (!string.IsNullOrEmpty(item.Duration)) //Nếu không có Duration trả về hợp lệ => Không kích hoạt bảo hành cho SP này
                                {
                                    if (item.Duration.IsInt())
                                    {
                                        var Duration = Convert.ToInt32(item.Duration);

                                        var proWarranty = _context.WarrantyModel.FirstOrDefault(p => p.Duration == Duration);

                                        if (proWarranty != null)
                                        {
                                            warrantyId = proWarranty.WarrantyId;
                                            productWarrantyNew.WarrantyId = warrantyId;
                                            productWarrantyNew.ProductId = productId;
                                            productWarrantyNew.SerriNo = item.Serial;
                                            productWarrantyNew.OrderDelivery = OrderDelivery;
                                            productWarrantyNew.ERPProductCode = item.ProductCode;
                                            productWarrantyNew.SaleOrder = item.SaleOrder;
                                            productWarrantyNew.ToDate = GetToDateTime(warrantyId, productWarrantyNew.FromDate);
                                            productWarrantyNew.ActivatedQuantity = item.ActivatedQuantity; //Số lượng kích sản phẩm trong 1 lần kích hoạt
                                            _context.Entry(productWarrantyNew).State = EntityState.Added;
                                        }
                                        else
                                        {
                                            return Json(new
                                            {
                                                Code = HttpStatusCode.InternalServerError,
                                                Success = false,
                                                Data = "Kích hoạt không thành công.<br/> Chưa có số tháng bảo hành: '" + Duration + "' trong hệ thống!"
                                            });
                                        }
                                    }
                                }
                                //}
                                //else
                                //{
                                //    warrantyId = (Guid)proWarranty;
                                //}
                            }
                        }
                    }

                    #endregion Theo OD

                    #endregion Thêm phiếu Đăng ký bảo hành theo từng sản phẩm

                    _context.SaveChanges();

                    //Save success => send sms

                    #region Send SMS

                    try
                    {
                        bool isSentSMS = ConstDomain.isSentSMS;
                        var phone = string.Empty;
                        if (profileViewModel.CustomerTypeCode == ConstCustomerType.Bussiness)
                        {
                            phone = profileViewModel.PhoneBusiness;
                        }
                        else
                        {
                            phone = profileViewModel.Phone;
                        }
                        if (!string.IsNullOrEmpty(phone) && isSentSMS == true)
                        {
                            SendSMSViewModel smsViewModel = new SendSMSViewModel();
                            smsViewModel.PhoneNumber = phone;

                            string brandName = string.Empty;
                            string tokenSMS = string.Empty;
                            string message = string.Empty;

                            //Company lấy theo từ web.config
                            switch (CompanyCode)
                            {
                                case ConstCompanyCode.AnCuong:
                                    brandName = ConstCompanyCode.BrandName_AnCuong;
                                    tokenSMS = ConstDomain.TokenAnCuong;
                                    break;

                                case ConstCompanyCode.Malloca:
                                    brandName = ConstCompanyCode.BrandName_Malloca;
                                    tokenSMS = ConstDomain.TokenMalloca;
                                    break;

                                case ConstCompanyCode.Aconcept:
                                    brandName = ConstCompanyCode.BrandName_Aconcept;
                                    tokenSMS = ConstDomain.TokenAconcept;
                                    break;

                                default:
                                    brandName = ConstCompanyCode.BrandName_AnCuong;
                                    tokenSMS = ConstDomain.TokenAnCuong;
                                    break;
                            }
                            //brand name
                            smsViewModel.BrandName = brandName;

                            //token
                            smsViewModel.Token = tokenSMS;

                            //message
                            var smsTemplateCode = "BH_" + company.SMSTemplateCode;
                            message = _context.CatalogModel.Where(p => p.CatalogTypeCode == ConstCatalogType.SMSTemplate
                                                                    && p.CatalogCode == smsTemplateCode
                                                                    && p.Actived == true)
                                              .Select(p => p.CatalogText_vi).FirstOrDefault();

                            bool isSent = false;
                            if (message != null)
                            {
                                isSent = true;
                                var fromDate = DateTime.Now;
                                var toDate = GetToDateTime(warrantyId, DateTime.Now);
                                //Nếu theo OD là gửi time bao hành theo san pham cuoi cung trong danh sach
                                var warrantyTime = "tu " + fromDate.ToString("dd/MM/yyyy") + " den " + toDate.ToString("dd/MM/yyyy");

                                if (!string.IsNullOrEmpty(Serial))
                                {
                                    smsViewModel.Message = message.Replace("[Serial]", Serial).Replace("[WarrantyTime]", warrantyTime);
                                }
                                else if (!string.IsNullOrEmpty(OrderDelivery))
                                {
                                    smsViewModel.Message = message.Replace("[Serial]", OD_Serial).Replace("[WarrantyTime]", warrantyTime);
                                }
                            }
                            else
                            {
                                isSent = false;
                            }
                            //Chỉ kích hoạt bằng serial mới gửi tin nhắn
                            if (isSent == true && !string.IsNullOrEmpty(Serial))
                            {
                                _unitOfWork.SendSMSRepository.SendSMSToCustomer(smsViewModel);
                                _context.SaveChanges();
                            }
                        }
                    }
                    catch (Exception)
                    {
                        return Json(new
                        {
                            Code = HttpStatusCode.InternalServerError,
                            Success = true,
                            Data = "Kích hoạt thành công. Hệ thống tin nhắn bận, không thể gửi tin nhắn ngay lúc này!"
                        });
                    }

                    #endregion Send SMS

                    #region Return

                    var mess = string.Empty;
                    if (!string.IsNullOrEmpty(Serial))
                    {
                        mess = "Sản phẩm " + Serial + " của Quý khách đã được đăng ký bảo hành điện tử thành công. Quý khách cần thêm thông tin vui lòng liên hệ " + company.TelService;
                    }
                    else if (!string.IsNullOrEmpty(OrderDelivery))
                    {
                        mess = "Các sản phẩm thuộc phiếu giao hàng " + OrderDelivery + " của Quý khách đã được đăng ký bảo hành điện tử thành công. Quý khách cần thêm thông tin vui lòng liên hệ " + company.TelService;
                    }
                    var response = new ResponseViewModel
                    {
                        Code = HttpStatusCode.OK,
                        Success = true,
                        Data = mess
                    };
                    return Json(response, JsonRequestBehavior.AllowGet);

                    #endregion Return
                }
                else
                {
                    var response = new ResponseViewModel
                    {
                        Code = HttpStatusCode.BadRequest,
                        Success = false,
                        Data = "Có số serial không hợp lệ hoặc các sản phẩm đã được kích hoạt bảo hành! Vui lòng kiểm tra lại."
                    };
                    return Json(response, JsonRequestBehavior.AllowGet);
                }
            });
        }

        #endregion Tạo khách hàng mới và phiếu Đăng ký bảo hành

        #region ViewBag

        private void CreateViewBag(string Age = null, Guid? ProvinceId = null, Guid? DistrictId = null, Guid? WardId = null)
        {
            #region //CustomerTypeCode

            //var catalogList = _context.CatalogModel.Where(
            //    p => p.CatalogTypeCode == ConstCatalogType.CustomerType
            //    && p.CatalogCode != ConstCustomerType.Contact
            //    && p.Actived == true).OrderBy(p => p.OrderIndex).ToList();
            //ViewBag.CustomerTypeCode = new SelectList(catalogList, "CatalogCode", "CatalogText_vi", "C");

            ViewBag.CustomerTypeCode = "C";

            #endregion //CustomerTypeCode

            #region //Title

            var titleLst = _catalogRepository.GetBy(ConstCatalogType.Title)
                                             .Where(p => p.CatalogCode != ConstTitle.Company);
            ViewBag.CustomerTitle = new SelectList(titleLst, "CatalogCode", "CatalogText_vi");

            #endregion //Title

            #region //Get list Age (Độ tuổi)

            var ageList = _catalogRepository.GetBy(ConstCatalogType.Age);
            ViewBag.Age = new SelectList(ageList, "CatalogCode", "CatalogText_vi", Age);
            #endregion //Get list Age (Độ tuổi)



            string SaleOfficeCode = string.Empty;

            #region //Get list Province (Tỉnh/Thành phố)

            var _provinceRepository = new ProvinceRepository(_context);
            var provinceList = _provinceRepository.GetAll();
            ViewBag.ProvinceId = new SelectList(provinceList, "ProvinceId", "ProvinceName", ProvinceId);
            //ViewBag.ProvinceIdSearchList = new SelectList(provinceList, "ProvinceId", "ProvinceName");

            #endregion //Get list Province (Tỉnh/Thành phố)

            if (ProvinceId == null)
            {
                ProvinceId = Guid.Empty;
            }

            #region //Get list District (Quận/Huyện)

            var _districtRepository = new DistrictRepository(_context);
            var districtList = _districtRepository.GetBy(ProvinceId);
            ViewBag.DistrictId = new SelectList(districtList, "DistrictId", "DistrictName", DistrictId);

            #endregion //Get list District (Quận/Huyện)

            if (DistrictId == null)
            {
                DistrictId = Guid.Empty;
            }

            #region //Get list Ward (Phường/Xã)

            var _wardRepository = new WardRepository(_context);
            var wardList = _wardRepository.GetBy(DistrictId);
            ViewBag.WardId = new SelectList(wardList, "WardId", "WardName", WardId);

            #endregion //Get list Ward (Phường/Xã)

            #region Position (Chức vụ)

            var positionList = _catalogRepository.GetBy(ConstCatalogType.Position);
            //ViewBag.ProfileContactPosition = new SelectList(positionList, "CatalogCode", "CatalogText_vi");
            ViewBag.PositionB = new SelectList(positionList, "CatalogCode", "CatalogText_vi");

            #endregion Position (Chức vụ)

            #region Department (Phòng ban)

            var departmentList = _catalogRepository.GetBy(ConstCatalogType.Department);
            ViewBag.DepartmentCode = new SelectList(departmentList, "CatalogCode", "CatalogText_vi");

            #endregion Department (Phòng ban)
        }

        #endregion ViewBag

        #region Get district by province

        public ActionResult GetDistrictByProvince(Guid? ProvinceId)
        {
            DistrictRepository repo = new DistrictRepository(_context);
            var districtList = repo.GetBy(ProvinceId);
            var lst = new SelectList(districtList, "DistrictId", "DistrictName");
            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        #endregion Get district by province

        #region Get ward by district

        public ActionResult GetWardByDistrict(Guid? DistrictId)
        {
            WardRepository repo = new WardRepository(_context);
            var wardList = repo.GetBy(DistrictId);
            var lst = new SelectList(wardList, "WardId", "WardName");
            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        #endregion Get ward by district

        #region Tính ngày hết hạn bảo hành

        /// <summary>
        /// Tính ngày hết hạn bảo hành
        /// </summary>
        /// <param name="WarrantyId">WarrantyId</param>
        /// <param name="FromDate">FromDate</param>
        /// <returns>ToDate</returns>
        private DateTime GetToDateTime(Guid WarrantyId, DateTime FromDate)
        {
            var warranty = _unitOfWork.WarrantyRepository.GetWarranty(WarrantyId);
            int duration = warranty.Duration;
            var toDate = FromDate.AddMonths(duration);
            return toDate;
        }

        #endregion Tính ngày hết hạn bảo hành
    }
}