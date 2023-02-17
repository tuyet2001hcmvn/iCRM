using BotDetect.Web.Mvc;
using ISD.Constant;
using ISD.Core;
using ISD.EntityModels;
using ISD.Repositories;
using ISD.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Warranty.Models;

namespace Warranty.Controllers
{
    public class WarrantyRequestController : BaseController
    {
        // GET: WarrantyRequest
        public ActionResult Index()
        {
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

        #region check Old
        /*
        [HttpPost]
        [CaptchaValidationActionFilter("CaptchaCode", "checkWarCaptcha", "Mã xác nhận không đúng!")]
        public ActionResult _CheckBySerial(CheckWarrantySearchModel checkWarrantySearch)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(checkWarrantySearch.SearchSeriNo))
                {
                    ModelState.AddModelError("SearchSeriNo", "Vui lòng nhập số serial!");
                    return View("Index");
                }
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
                return View("Index", checkWarrantySearch);
            }
            else
            {
                return View("Index");
            }
        }

        [HttpPost]
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
                var sessionOTP = Session["CurrentOTP"];
                var phoneOTP = Session["CurrentPhone"];
                var otp = string.Empty;
                var otpPhone = string.Empty;
                if (sessionOTP != null && phoneOTP != null)
                {
                    otp = sessionOTP.ToString();
                    otpPhone = phoneOTP.ToString();
                }
                if (checkWarrantySearch.VerifyOTP == otp && checkWarrantySearch.SearchPhone == otpPhone)
                {
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
                }
                else
                {
                    ModelState.AddModelError("VerifyOTP", "Mã OTP không chính xác");
                    ViewBag.Tabs = "Phone";
                    return View("Index");
                }
            }
            else
            {
                ViewBag.Tabs = "Phone";
                return View("Index");
            }
        }
        */
        #endregion

        [HttpPost]
        public ActionResult Create(List<Guid> ItemsId)
        {
            if (ItemsId != null && ItemsId.Count > 0)
            {
                //Truyền id qua view
                ViewBag.warrantyIdList = ItemsId;
                var id = ItemsId[0];
                var result = (from p in _context.ProductWarrantyModel
                              where p.ProductWarrantyId == id
                              select new WarrantyResultViewModel
                              {
                                  ProductWarrantyId = p.ProductWarrantyId,
                                  ProductName = p.ProductModel.ProductName,
                                  ProductWarrantyCode = p.ProductWarrantyCode,
                                  ProfileName = p.ProfileModel.ProfileName,
                                  ProfilePhoneNumber = p.ProfileModel.Phone,
                                  Address = p.ProfileModel.Address,
                                  SerriNo = p.SerriNo,
                                  ProfileId = p.ProfileModel.ProfileId,
                              }).FirstOrDefault();

                var productNames = string.Empty;
                foreach (var item in ItemsId)
                {
                    var name = _context.ProductWarrantyModel.FirstOrDefault(p => p.ProductWarrantyId == item).ProductModel.ProductName;
                    if (!string.IsNullOrEmpty(name))
                    {
                        productNames += name + " | ";
                    }
                }

                ViewBag.ProductName = productNames;
                var viewModel = new RequestWarrantyViewModel
                {
                    //ProductWarrantyId = result.ProductWarrantyId,
                    ProfileName = result.ProfileName,
                    PhoneNumber = result.ProfilePhoneNumber,
                    Address = result.Address,
                    ProfileId = result.ProfileId
                };
                return View(viewModel);
            }
            else
            {
                SetAlert("Không có sản phẩm nào được chọn. Vui lòng kiểm tra lại!", "warning");
                return View("Index");
                //return Json(new
                //{
                //    Code = HttpStatusCode.BadRequest,
                //    Success = false,
                //    Data = "Không có sản phẩm nào được chọn. Vui lòng kiểm tra lại!"
                //});
            }
        }

        [HttpPost]
        public ActionResult SaveTicket(RequestWarrantyViewModel requestWarranty, List<Guid> ListIdWarranty)
        {
            //if (ModelState.IsValid)
            //{
            return ExecuteContainer(() =>
            {
                //AccountId SYSTEM
                var accountId = _context.AccountModel.FirstOrDefault(p => p.UserName == "SYSTEM").AccountId;
                bool isReady = true;
                var serialInfor = new SerialViewModel();
                bool isSerial = bool.Parse(ConfigurationManager.AppSettings["isSerial"]);
                //Nếu cty theo serial thì mới kiểm tra lại serial có hợp lệ hay không
                if (isSerial)
                {
                    foreach (var item in ListIdWarranty)
                    {
                        var ProductWarranty = _context.ProductWarrantyModel.FirstOrDefault(p => p.ProductWarrantyId == item);
                        serialInfor = _unitOfWork.WarrantyRepository.GetSerialInfor(ProductWarranty.SerriNo);
                        //Có serial không hợp lệ
                        if (serialInfor.SerialNo == null || ProductWarranty == null)
                        {
                            isReady = false;
                            break;
                        }

                    }
                }
                //var ProductWarranty = _context.ProductWarrantyModel.FirstOrDefault(p => p.ProductWarrantyId == requestWarranty.ProductWarrantyId);
                //var serialInfor = _unitOfWork.WarrantyRepository.GetSerialInfor(ProductWarranty.SerriNo);


                if (isReady)
                {
                    //Loại Ticket bảo hành
                    var workflowCode = ConfigurationManager.AppSettings["WorkflowCode"];
                    var workflowId = _context.WorkFlowModel.FirstOrDefault(p => p.WorkFlowCode == workflowCode).WorkFlowId;
                    //Company lấy từ web.config
                    var CompanyCode = ConfigUtilities.GetSysConfigAppSetting("CompanyCode");
                    var company = _context.CompanyModel.FirstOrDefault(p => p.CompanyCode == CompanyCode);
                    //Trạng thái: mặc định "Chờ duyệt"
                    var defaultStatus = ConfigurationManager.AppSettings["defaultStatusCode"];
                    var taskStatusId = _context.TaskStatusModel.FirstOrDefault(p => p.WorkFlowId == workflowId && p.TaskStatusCode == defaultStatus).TaskStatusId;

                    var taskId = Guid.NewGuid();
                    var taskNew = new TaskModel();
                    taskNew.TaskId = taskId;
                    //Tiêu đề
                    taskNew.Summary = requestWarranty.Summary;
                    taskNew.ProfileId = requestWarranty.ProfileId;

                    #region Profile/ Khách hàng
                    //Lưu khách hàng vào bảng task reference
                    TaskReferenceModel referenceAccount = new TaskReferenceModel();
                    referenceAccount.ObjectId = requestWarranty.ProfileId;
                    referenceAccount.Type = ConstTaskReference.Account;
                    referenceAccount.TaskId = taskId;
                    //referenceAccount.CreateBy = CurrentUser.AccountId;
                    _unitOfWork.TaskRepository.CreateTaskReference(referenceAccount);
                    #endregion

                    //Mức độ
                    taskNew.PriorityCode = ConstPriotityCode.NORMAL;
                    taskNew.WorkFlowId = workflowId;//Id work Bảo hành
                    taskNew.TaskStatusId = taskStatusId; //Id taskStatus tiếp nhận
                                                         //ID Model đăng ký bảo hành
                    taskNew.ProductWarrantyId = ListIdWarranty[0];

                    //Công ty
                    taskNew.CompanyId = company.CompanyId;
                    taskNew.DateKey = _unitOfWork.UtilitiesRepository.ConvertDateTimeToInt(DateTime.Now);
                    taskNew.ReceiveDate = DateTime.Now; //Ngày tiếp nhận
                                                        //Reporter
                    taskNew.Reporter = "10009999"; //Admin
                    taskNew.CreateBy = accountId;
                    taskNew.CreateTime = DateTime.Now;
                    taskNew.Actived = true;
                    taskNew.isPrivate = false;

                    //Sản phẩm phụ kiện
                    _context.Entry(taskNew).State = EntityState.Added;
                    foreach (var item in ListIdWarranty)
                    {
                        var ProductWarranty = _context.ProductWarrantyModel.FirstOrDefault(p => p.ProductWarrantyId == item);
                        TaskProductModel model = new TaskProductModel();
                        model.TaskProductId = Guid.NewGuid();
                        model.TaskId = taskId;
                        model.ProductId = ProductWarranty.ProductId;
                        model.Qty = 1;
                        model.CreateBy = accountId;
                        model.CreateTime = DateTime.Now;
                        //model.ErrorTypeCode = taskProductVM.ErrorTypeCode;
                        //model.ErrorCode = taskProductVM.ErrorCode;
                        //model.ProductLevelCode = taskProductVM.ProductLevelCode;
                        //model.ProductColorCode = taskProductVM.ProductColorCode;
                        //model.UsualErrorCode = taskProductVM.UsualErrorCode;
                        //model.ProductCategoryCode = taskProductVM.ProductCategoryCode;
                        _context.Entry(model).State = EntityState.Added;
                    }
                      _context.SaveChanges();


                    //Save success => send sms
                    #region Send SMS
                    bool isSentSMS = ConstDomain.isSentSMS;
                    var phone = requestWarranty.PhoneNumber;
                    if (!string.IsNullOrEmpty(phone) && isSentSMS == true)
                    {
                        SendSMSViewModel smsViewModel = new SendSMSViewModel();
                        smsViewModel.PhoneNumber = phone;

                        string brandName = string.Empty;
                        string tokenSMS = string.Empty;
                        string message = string.Empty;

                        //string companyCode = ConfigUtilities.GetSysConfigAppSetting("CompanyCode");
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
                        var smsTemplateCode = "YCBH_" + company.SMSTemplateCode;
                        message = _context.CatalogModel.Where(p => p.CatalogTypeCode == ConstCatalogType.SMSTemplate && p.CatalogCode == smsTemplateCode && p.Actived == true).Select(p => p.CatalogText_vi).FirstOrDefault();
                        //smsViewModel.Message = string.Format("Cam on quy khach da den tham quan {0}", store.StoreName);
                        bool isSent = false;
                        if (message != null)
                        {
                            isSent = true;
                            smsViewModel.Message = message.Replace("[time]", "02 gio");
                        }
                        else
                        {
                            isSent = false;
                        }

                        if (isSent == true)
                        {
                             _unitOfWork.SendSMSRepository.SendSMSToCustomer(smsViewModel);
                             _context.SaveChanges();
                        }

                    }
                    #endregion
                    // SetAlert("Yêu cầu bảo hành thành công. " + company.CompanyShortName + " sẽ liên hệ với quý khách trong vòng 2 giờ. Cảm ơn quý khách đã sử dụng sản phẩm của " + company.CompanyShortName, "success");
                    //return RedirectToAction("Index", "Home");
                    return Json(new
                    {
                        Code = HttpStatusCode.OK,
                        Success = true,
                        Data = "Yêu cầu bảo hành thành công. " + company.CompanyShortName + " sẽ liên hệ với quý khách trong vòng 2 giờ. Cảm ơn quý khách đã sử dụng sản phẩm của " + company.CompanyShortName
                    });
                }
                //ViewBag.warrantyIdList = ListIdWarranty;
                //SetAlert("Serial không đúng với bất kỳ sản phẩm nào, vui lòng kiểm tra lại!", "error");
                //return View("Create", requestWarranty);
                //return Json(new
                //{
                //    Code = HttpStatusCode.BadRequest,
                //    Success = false,
                //    Data = "Serial không đúng với bất kỳ sản phẩm nào, vui lòng kiểm tra lại!"
                //});

                //ViewBag.warrantyIdList = ListIdWarranty;
                //SetAlert("Hệ thống bận, vui lòng thử lại sau!", "warning");
                //return View("Create", requestWarranty);
                return Json(new
                {
                    Code = HttpStatusCode.BadRequest,
                    Success = false,
                    Data = "Hệ thống bận, vui lòng thử lại sau!"
                });
            });
        }
    }
}