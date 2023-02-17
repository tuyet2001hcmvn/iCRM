using ISD.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Warranty.Models;
using BotDetect.Web.Mvc;
using ISD.Repositories;
using System.Text;
using System.Drawing;
using System.IO;
using ZXing;
using ISD.Constant;

namespace Warranty.Controllers
{
    public class HomeController : BaseController
    {
        private new readonly UnitOfWork _unitOfWork;
        public HomeController()
        {
            _unitOfWork = new UnitOfWork(_context);
        }
        public ActionResult Index()
        {

            ViewBag.NoWarranty = false;
            return View();
        }
        [HttpPost]
        [CaptchaValidationActionFilter("CaptchaCode", "checkWarCaptcha", "Mã xác nhận không đúng!")]
        public ActionResult Index(CheckWarrantySearchModel checkWarrantySearch)
        {
            if (ModelState.IsValid)
            {
                var result = new List<WarrantyResultViewModel>();
                //Theo sdt || serial
                if (checkWarrantySearch.SearchType == "Serial")
                {
                    if (string.IsNullOrEmpty(checkWarrantySearch.SearchSeriNo))
                    {
                        ModelState.AddModelError("SearchSeriNo", "Vui lòng nhập số serial!");
                        return View("Index");
                    }
                    result = (from p in _context.ProductWarrantyModel
                              where p.SerriNo == checkWarrantySearch.SearchSeriNo
                              orderby p.ProductWarrantyCode
                              select new WarrantyResultViewModel
                              {
                                  ProductWarrantyId = p.ProductWarrantyId,
                                  ProductWarrantyCode = p.ProductWarrantyCode,
                                  ProfileId = p.ProfileId,
                                  ProfileName = p.ProfileModel.ProfileName,
                                  ProfilePhoneNumber = p.ProfileModel.Phone,
                                  SerriNo = p.SerriNo,
                                  ProductWarrantyNo = p.ProductWarrantyNo,
                                  ProductId = p.ProductId,
                                  ProductName = p.ProductModel.ProductName,
                                  WarrantyName = p.WarrantyModel.WarrantyName,
                                  FromDate = p.FromDate,
                                  ToDate = (DateTime)p.ToDate
                              }).ToList();
                }
                else if (checkWarrantySearch.SearchType == "OrderDelivery")
                {
                    if (string.IsNullOrEmpty(checkWarrantySearch.SearchOrderDelivery))
                    {
                        ModelState.AddModelError("OrderDelivery", "Vui lòng nhập số OD");
                        //ViewBag.Tabs = "Phone";
                        return View("Index");
                    }
                    result = (from p in _context.ProductWarrantyModel
                              join cus in _context.ProfileModel on p.ProfileId equals cus.ProfileId
                              where p.OrderDelivery == checkWarrantySearch.SearchOrderDelivery
                              orderby p.ProductWarrantyCode
                              select new WarrantyResultViewModel
                              {
                                  ProductWarrantyId = p.ProductWarrantyId,
                                  ProductWarrantyCode = p.ProductWarrantyCode,
                                  ProfileId = p.ProfileId,
                                  ProfileName = p.ProfileModel.ProfileName,
                                  ProfilePhoneNumber = p.ProfileModel.Phone,
                                  SerriNo = p.SerriNo,
                                  ProductWarrantyNo = p.ProductWarrantyNo,
                                  ProductId = p.ProductId,
                                  ProductName = p.ProductModel.ProductName,
                                  WarrantyName = p.WarrantyModel.WarrantyName,
                                  FromDate = p.FromDate,
                                  ToDate = (DateTime)p.ToDate
                              }).ToList();
                }
                else
                {
                    if (string.IsNullOrEmpty(checkWarrantySearch.SearchPhone))
                    {
                        ModelState.AddModelError("SearchPhone", "Vui lòng nhập số điện thoại");
                        //ViewBag.Tabs = "Phone";
                        return View("Index");
                    }
                    var phone = checkWarrantySearch.SearchPhone.Trim();
                    result = (from p in _context.ProductWarrantyModel
                              join cus in _context.ProfileModel on p.ProfileId equals cus.ProfileId
                              where cus.Phone == phone
                              orderby p.ProductWarrantyCode
                              select new WarrantyResultViewModel
                              {
                                  ProductWarrantyId = p.ProductWarrantyId,
                                  ProductWarrantyCode = p.ProductWarrantyCode,
                                  ProfileId = p.ProfileId,
                                  ProfileName = p.ProfileModel.ProfileName,
                                  ProfilePhoneNumber = p.ProfileModel.Phone,
                                  SerriNo = p.SerriNo,
                                  ProductWarrantyNo = p.ProductWarrantyNo,
                                  ProductId = p.ProductId,
                                  ProductName = p.ProductModel.ProductName,
                                  WarrantyName = p.WarrantyModel.WarrantyName,
                                  FromDate = p.FromDate,
                                  ToDate = (DateTime)p.ToDate
                              }).ToList();
                }

                MvcCaptcha.ResetCaptcha("checkWarCaptcha");
                //Set thông báo khi không tìm thấy
                if (result == null || result.Count == 0)
                {
                    ViewBag.NoWarranty = true;
                    ViewBag.Serrial = checkWarrantySearch.SearchSeriNo;
                    ViewBag.OrderDelivery = checkWarrantySearch.SearchOrderDelivery;
                    ViewBag.PhoneSearch = checkWarrantySearch.SearchPhone;
                }
                else
                {
                    //Ẩn số dt
                    if (!string.IsNullOrEmpty(result[0].ProfilePhoneNumber))
                    {
                        var phoneNew = EnCodePhone(result[0].ProfilePhoneNumber);
                        result[0].ProfilePhoneNumber = phoneNew;
                    }
                }

                ViewBag.WarrantyDetail = result;
                return View(checkWarrantySearch);
            }
            else
            {
                return View(checkWarrantySearch);
            }
        }
        public ActionResult UserManual()
        {
            return View();
        }

        #region Not used
        /*
        [HttpPost]
        [CaptchaValidationActionFilter("CaptchaCode", "checkWarCaptcha", "Mã xác nhận không đúng!")]
        public ActionResult _CheckBySerial(CheckWarrantySearchModel checkWarrantySearch)
        {
            if (ModelState.IsValid)
            {

                var result = (from p in _context.ProductWarrantyModel
                              where p.SerriNo == checkWarrantySearch.SearchSeriNo
                              select new WarrantyResultViewModel
                              {
                                  ProductWarrantyId = p.ProductWarrantyId,
                                  ProductWarrantyCode = p.ProductWarrantyCode,
                                  ProfileName = p.ProfileModel.ProfileName,
                                  ProfilePhoneNumber = p.ProfileModel.Phone,
                                  SerriNo = p.SerriNo,
                                  ProductWarrantyNo = p.ProductWarrantyNo,
                                  ProductId = p.ProductId,
                                  ProductName = p.ProductModel.ProductName,
                                  WarrantyName = p.WarrantyModel.WarrantyName,
                                  FromDate = p.FromDate,
                                  ToDate = (DateTime)p.ToDate
                              }).ToList();
                MvcCaptcha.ResetCaptcha("checkWarCaptcha");
                //Set thông báo khi không tìm thấy
                if (result == null || result.Count == 0)
                {
                    ViewBag.NoWarranty = true;
                    ViewBag.Serrial = checkWarrantySearch.SearchSeriNo;
                    ViewBag.PhoneSearch = checkWarrantySearch.SearchPhone;
                }
                else
                {
                    //Ẩn số dt
                    if (!string.IsNullOrEmpty(result[0].ProfilePhoneNumber))
                    {
                        var phoneNew = EnCodePhone(result[0].ProfilePhoneNumber);
                        result[0].ProfilePhoneNumber = phoneNew;
                    }
                }

                ViewBag.WarrantyDetail = result;
                return View("Index", checkWarrantySearch);
            }
            else
            {
                return View(checkWarrantySearch);
            }
        }

        [HttpPost]
        [CaptchaValidationActionFilter("CaptchaCode", "checkWarCaptcha", "Mã xác nhận không đúng!")]
        public ActionResult _CheckByPhone(CheckWarrantySearchModel checkWarrantySearch)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(checkWarrantySearch.SearchPhone))
                {
                    ModelState.AddModelError("SearchPhone", "Vui lòng nhập số điện thoại");
                    ViewBag.Tabs = "Phone";
                    return View("Index");
                }
                //var sessionOTP = Session["CurrentOTP"];
                //var phoneOTP = Session["CurrentPhone"];
                //var otp = string.Empty;
                //var otpPhone = string.Empty;
                //if (sessionOTP != null && phoneOTP != null)
                //{
                //    otp = sessionOTP.ToString();
                //    otpPhone = phoneOTP.ToString();
                //}
                //if (checkWarrantySearch.VerifyOTP == otp && checkWarrantySearch.SearchPhone == otpPhone)
                //{
                var result = (from p in _context.ProductWarrantyModel
                              join cus in _context.ProfileModel on p.ProfileId equals cus.ProfileId
                              where cus.Phone == checkWarrantySearch.SearchPhone
                              select new WarrantyResultViewModel
                              {
                                  ProductWarrantyId = p.ProductWarrantyId,
                                  ProductWarrantyCode = p.ProductWarrantyCode,
                                  ProfileName = p.ProfileModel.ProfileName,
                                  ProfilePhoneNumber = p.ProfileModel.Phone,
                                  SerriNo = p.SerriNo,
                                  ProductWarrantyNo = p.ProductWarrantyNo,
                                  ProductId = p.ProductId,
                                  ProductName = p.ProductModel.ProductName,
                                  WarrantyName = p.WarrantyModel.WarrantyName,
                                  FromDate = p.FromDate,
                                  ToDate = (DateTime)p.ToDate
                              }).ToList();
                MvcCaptcha.ResetCaptcha("checkWarCaptcha");
                if (result == null || result.Count == 0)
                {
                    ViewBag.NoWarranty = true;
                    ViewBag.Serrial = checkWarrantySearch.SearchSeriNo;
                    ViewBag.PhoneSearch = checkWarrantySearch.SearchPhone;
                }
                else
                {
                    if (!string.IsNullOrEmpty(result[0].ProfilePhoneNumber))
                    {
                        var phoneNew = EnCodePhone(result[0].ProfilePhoneNumber);
                        result[0].ProfilePhoneNumber = phoneNew;
                    }
                }

                ViewBag.WarrantyDetail = result;
                ViewBag.Tabs = "Phone";
                return View("Index", checkWarrantySearch);
                //}
                //else
                //{
                //    ModelState.AddModelError("VerifyOTP", "Mã OTP không chính xác");
                //    ViewBag.Tabs = "Phone";
                //    return View("Index");
                //}
            }
            else
            {
                ViewBag.Tabs = "Phone";
                return View("Index");
            }
        }
        
        [HttpPost]
        public JsonResult SendOTP(string phoneNumber)
        {
            if (!string.IsNullOrEmpty(phoneNumber))
            {
                //Thông tin sdt da dang ky bao hanh
                var warrantyProfile = (from pro in _context.ProfileModel
                                       join war in _context.ProductWarrantyModel on pro.ProfileId equals war.ProfileId
                                       where pro.Phone == phoneNumber
                                       orderby war.CreateTime descending
                                       select pro).FirstOrDefault();
                if (warrantyProfile == null || warrantyProfile.Phone != phoneNumber)
                {
                    return Json(new
                    {
                        code = HttpStatusCode.BadRequest,
                        success = false,
                        message = "Số điện thoại chưa được đăng ký bảo hành"
                    });
                }
                int otpValue = new Random().Next(100000, 999999);
                bool isSentSMS = ConstDomain.isSentSMS;
                var CompanyCode = ConfigUtilities.GetSysConfigAppSetting("CompanyCode");
                #region SendSMS
                SendSMSViewModel smsViewModel = new SendSMSViewModel();
                smsViewModel.PhoneNumber = phoneNumber;
                string brandName = string.Empty;
                string tokenSMS = string.Empty;
                string message = string.Empty;

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
                var company = _context.CompanyModel.FirstOrDefault(p => p.CompanyCode == CompanyCode);
                var smsTemplateCode = "OTP_" + company.SMSTemplateCode;
                message = _context.CatalogModel.Where(p => p.CatalogTypeCode == ConstCatalogType.SMSTemplate && p.CatalogCode == smsTemplateCode && p.Actived == true).Select(p => p.CatalogText_vi).FirstOrDefault();

                bool isSent = false;
                if (message != null)
                {
                    isSent = true;
                    smsViewModel.Message = message.Replace("[OTPvalue]", otpValue.ToString());
                }

                if (isSent == true)
                {
                    //_unitOfWork.SendSMSRepository.SendSMSToCustomer(smsViewModel);
                    //_context.SaveChanges();
                    Session["CurrentOTP"] = otpValue;
                    Session["CurrentPhone"] = phoneNumber;
                    return Json(new
                    {
                        code = HttpStatusCode.OK,
                        success = true,
                        message = "Mã OTP đã được gửi đến SĐT của bạn!"
                    });
                }
                else
                {
                    return Json(new
                    {
                        code = HttpStatusCode.InternalServerError,
                        success = false,
                        message = "Đã xảy ra lỗi, vui lòng thử lại!"
                    });
                }
                #endregion
            }
            else
            {
                return Json(new
                {
                    code = HttpStatusCode.BadRequest,
                    success = false,
                    message = "Vui lòng nhập số điện thoại"
                });
            }

        }
        */
        #endregion

        [HttpPost]
        public ActionResult GetSerialNo(string image)
        {
            try
            {
                StringBuilder send = new StringBuilder();
                // Lock by send variable 
                lock (send)
                {
                    // Convert base64 string from the client side into byte array
                    byte[] bitmapArrayOfBytes = Convert.FromBase64String(image);
                    Bitmap bmp;
                    using (var ms = new MemoryStream(bitmapArrayOfBytes))
                    {
                        bmp = new Bitmap(ms);
                    }
                    BarcodeReader reader = new BarcodeReader();
                    var result = reader.Decode(bmp);
                    var serial = string.Empty;
                    if (result != null)
                    {
                        var index = result.Text.IndexOf("<T10>") + 5;
                        serial = result.Text.Substring(index, 18);
                        // Return the output string as JSON
                        return Json(new { d = serial });
                    }
                    return Json(new { d = "NONE" });
                }
            }
            catch (Exception ex)
            {
                // return the exception instead
                return Json(new { d = ex.Message + "\r\n" + ex.StackTrace });
            }
        }

        [HttpPost]
        public ActionResult GetOrderDelivery(string image)
        {
            try
            {
                StringBuilder send = new StringBuilder();
                // Lock by send variable 
                lock (send)
                {
                    // Convert base64 string from the client side into byte array
                    byte[] bitmapArrayOfBytes = Convert.FromBase64String(image);
                    Bitmap bmp;
                    using (var ms = new MemoryStream(bitmapArrayOfBytes))
                    {
                        bmp = new Bitmap(ms);
                    }
                    BarcodeReader reader = new BarcodeReader();
                    var result = reader.Decode(bmp);
                    var OrderDelivery = string.Empty;
                    if (result != null)
                    {
                        WarrantyRepository _warrantyRepository = new WarrantyRepository(_context);
                        OrderDelivery = _warrantyRepository.ConvertQRCodeToOrderDelivery(result.Text);

                        return Json(new { d = OrderDelivery });
                    }
                    return Json(new { d = "NONE" });
                }
            }
            catch (Exception ex)
            {
                // return the exception instead
                return Json(new { d = ex.Message + "\r\n" + ex.StackTrace });
            }
        }

        [HttpGet]
        public JsonResult GetTaskTicket(Guid productId)
        {
            var taskList = (from p in _context.TaskProductModel
                            join t in _context.TaskModel on p.TaskId equals t.TaskId
                            where p.ProductId == productId
                            select new TaskTicketViewModel
                            {
                                TaskId = t.TaskId,
                                Summary = t.Summary,
                                TaskCode = t.TaskCode,
                                CreateTime = t.CreateTime
                            }).ToList();
            return Json(taskList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ViewRepuest(Guid productId, Guid profileId)
        {
            var taskList = (from p in _context.TaskProductModel
                            join t in _context.TaskModel on p.TaskId equals t.TaskId
                            where p.ProductId == productId && t.ProfileId == profileId
                            group t by new { t.TaskId, t.Summary, t.TaskCode, t.CreateTime } into task
                            orderby task.Key.CreateTime descending
                            select new TaskTicketViewModel
                            {
                                TaskId = task.Key.TaskId,
                                Summary = task.Key.Summary,
                                TaskCode = task.Key.TaskCode,
                                CreateTime = task.Key.CreateTime
                            }).ToList();
            return PartialView(taskList);
        }
    }
}