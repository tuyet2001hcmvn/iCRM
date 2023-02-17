using ISD.Constant;
using ISD.Core;
using ISD.EntityModels;
using ISD.Resources;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MasterData.Controllers
{
    [AllowAnonymous]
    public class EmployeeRatingsController : BaseController
    {
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Index(Guid AccountId)
        {
            ViewBag.AccountId = AccountId;
            if (_unitOfWork.AccountRepository.GetBy(AccountId) == null)
            {
                return RedirectToAction("ErrorText", "Error", new { area = "", statusCode = 404, exception = string.Format(LanguageResource.Error_NotExist, LanguageResource.AccountId.ToLower()) });
            }
            return View();
        }

        [HttpPost]
        public JsonResult Index(Guid AccountId, string Ratings, string Reviews, string FullName, string PhoneNumber, string Email, bool? isHasOtherReviews)
        {
            return ExecuteContainer(() =>
            {
                var currentDate = DateTime.Now;
                if (isHasOtherReviews.HasValue && isHasOtherReviews == true)
                {
                    Ratings = (from p in _context.CatalogModel
                               where p.CatalogCode == "EmployeeRatings6"
                               select p.CatalogCode).FirstOrDefault();
                    if (string.IsNullOrEmpty(Reviews))
                    {
                        return Json(new
                        {
                            Code = System.Net.HttpStatusCode.NotModified,
                            Success = false,
                            Data = string.Format("Quý khách vui lòng nhập ý kiến khác trước khi bấm nút GỬI Ý KIẾN")
                        });
                    }

                    //Gửi mail
                    /*
                        Mail nhận: infoacc@ancuong.com; tuyenhtn@ancuong.com
                        ======================================================
                        Subject: HỘP THƯ GÓP Ý-ĐÁNH GIÁ CỦA KHÁCH HÀNG 
                        ======================================================
                        Content:
                        ------------------------------
                        Họ và Tên:
                        SDT:
                        Email:
                        Nội dung góp ý:
                        User gửi: <lấy user đang tiếp khách>
                        Thời gian gửi: <ngày giờ khách hàng gửi ý kiến>
                        ------------------------------
                        - Về From email: thì lấy info@ancuong.com
                        - Về To email: thì cho anh chỗ thiết lập, để sau này có thay đổi email thì vào update lại
                        - Cập nhật (22/03/2022) : Anh Phước yêu cầu cấu hình gửi mail theo công ty(1000,2000,3000)
                    */
                    //User
                    var user = _context.AccountModel.Where(p => p.AccountId == AccountId).FirstOrDefault();
                    //GET email account
                    EmailAccountModel emailAccount = _context.EmailAccountModel.Where(s => s.SenderName == "Gỗ An Cường" && s.IsSender == true).FirstOrDefault();
                    //get mail server provider
                    MailServerProviderModel provider = _context.MailServerProviderModel.Where(s => s.Id == emailAccount.ServerProviderId).FirstOrDefault();
                    var emailConfigLst = _context.CatalogModel.Where(p => p.CatalogTypeCode == ConstCatalogType.FeedbackEmailConfig).ToList();
                    //FromEmail
                    string FromEmail = emailConfigLst.Where(p => p.CatalogCode == "Feedback_FromEmail").Select(p => p.CatalogText_vi).FirstOrDefault();

                    //ToEmail
                    string ToEmail = "";
                    //Gửi mail theo chi nhánh
                    foreach (var item in user.StoreModel.Where(x=>x.Actived == true))
                    {
                        ToEmail += item.Email;
                    }
                    //Gửi mail theo công ty
                    foreach (var item in user.StoreModel.Where(x => x.Actived == true).Select(x=>x.CompanyModel.CompanyCode).Distinct())
                    {
                        if (!string.IsNullOrEmpty(ToEmail))
                        {
                            ToEmail += ";";
                        }
                        ToEmail += emailConfigLst.Where(p => p.CatalogCode == item).Select(p => p.CatalogText_vi).FirstOrDefault();
                    }
                    //Subject
                    string Subject = "HỘP THƯ GÓP Ý-ĐÁNH GIÁ CỦA KHÁCH HÀNG";
                    //Content
                    string EmailContent = string.Empty;
                    EmailContent += "Họ và Tên: [FullName]";
                    EmailContent += "<br />";
                    EmailContent += "SDT: [PhoneNumber]";
                    EmailContent += "<br />";
                    EmailContent += "Email: [Email]";
                    EmailContent += "<br />";
                    EmailContent += "Nội dung góp ý: [Reviews]";
                    EmailContent += "<br />";
                    EmailContent += "User gửi: [UserName]";
                    EmailContent += "<br />";
                    EmailContent += "Thời gian gửi: [CurrentDate]";
                    EmailContent += "<br />";

                    EmailContent = EmailContent.Replace("[FullName]", FullName)
                                                .Replace("[PhoneNumber]", PhoneNumber)
                                                .Replace("[Email]", Email)
                                                .Replace("[Reviews]", Reviews)
                                                .Replace("[UserName]", string.IsNullOrEmpty(user.FullName) ? user.UserName : user.FullName)
                                                .Replace("[CurrentDate]", string.Format("{0:dd/MM/yyyy HH:mm:ss}", currentDate))
                                               ;

                    _unitOfWork.EmployeeRatingsRepository.SendMail(EmailContent, Subject, FromEmail, emailAccount.Account, emailAccount.Password, provider.OutgoingHost, provider.OutgoingPort, emailAccount.EnableSsl ?? false, ToEmail);
                }
                //Ratings
                RatingModel rating = new RatingModel();
                rating.RatingId = Guid.NewGuid();
                rating.RatingTypeCode = ConstCatalogType.EmployeeRatings;
                rating.ReferenceId = AccountId;
                rating.Ratings = Ratings;
                rating.Reviews = Reviews;
                rating.FullName = FullName;
                rating.PhoneNumber = PhoneNumber;
                rating.Email = Email;
                rating.CreateTime = currentDate;

                _context.Entry(rating).State = EntityState.Added;
                _context.SaveChanges();

                return Json(new
                {
                    Code = System.Net.HttpStatusCode.Created,
                    Success = true,
                    Data = string.Format("Cảm ơn quý khách! \n Đánh giá của quý khách giúp chúng tôi cải thiện chất lượng phục vụ tốt hơn.")
                });
            });
        }

    }
}