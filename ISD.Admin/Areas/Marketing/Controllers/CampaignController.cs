using ISD.Constant;
using ISD.Core;
using ISD.EntityModels;
using ISD.Repositories;
using ISD.Resources;
using ISD.ViewModels;
using ISD.ViewModels.Marketing;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Marketing.Controllers
{
    public class CampaignController : BaseController
    {
        // GET: Campaign
        public ActionResult Index(string Type)
        {
            string pageUrl = "/Marketing/Campaign";
            var parameter = string.Empty;
            if (Type == ConstMarketingType.Marketing)
            {
                parameter = "?Type=Marketing";
            }
            else if (Type == ConstMarketingType.Event)
            {
                parameter = "?Type=Event";
            }
            ViewBag.PageId = GetPageId(pageUrl, parameter);
            CreateViewBag(Type: Type);
            return View();
        }
        public ActionResult Edit(Guid Id, string Type)
        {
            var saleOrg = _context.StoreModel.Where(p => p.Actived == true).OrderBy(p => p.SaleOrgCode).ToList().Select(s => new StoreModel
            {
                SaleOrgCode = s.SaleOrgCode,
                StoreName = s.SaleOrgCode + " | " + s.StoreName
            });
            ViewBag.SaleOrg = new SelectList(saleOrg, "SaleOrgCode", "StoreName");
            ViewBag.Id = Id;
            ViewBag.TypeContent = _unitOfWork.ContentRepository.GetContentByCampaign(Id)?.CatalogCode;
            CreateViewBag(Type: Type);
            return View();
        }
        public ActionResult Create(string Type)
        {
            var saleOrg = _context.StoreModel.Where(p => p.Actived == true).OrderBy(p => p.SaleOrgCode).ToList().Select(s => new StoreModel
            {
                SaleOrgCode = s.SaleOrgCode,
                StoreName = s.SaleOrgCode + " | " + s.StoreName
            });
            ViewBag.SaleOrg = new SelectList(saleOrg, "SaleOrgCode", "StoreName");
            ViewBag.Type = Type;
            return View();
        }

        public ActionResult ReportById()
        {      
            return PartialView();
        }
        [AllowAnonymous]
        public ActionResult Unsubscribe(Guid id)
        {
            ViewBag.Id = id;
            return View();
        }

        [AllowAnonymous]
        public ActionResult ConfirmEmail(Guid id, string Type)
        {
            var sendcalendar = (from s in _context.SendMailCalendarModel
                                join cp in _context.CampaignModel on s.CampaignId equals cp.Id
                                where s.Id == id
                                select new
                                {
                                    Name = s.Param,
                                    Content = cp.CampaignName
                                }).FirstOrDefault();
            if (sendcalendar == null || (Type != "Confirm" && Type != "Reject"))
            {
                return RedirectToAction("ErrorText", "Error", new { area = "", statusCode = 404, exception = "Trang không tồn tại!" });
            }
            #region Confirm email
            new EmailsRepository(_context).ConfirmEmail(id, Type);
            #endregion
            ViewBag.Type = Type;
            ViewBag.Name = sendcalendar.Name;
            ViewBag.Content = sendcalendar.Content;
            return View();
        }
        public void CreateViewBag(string Type = null)
        {
            ViewBag.Type = Type;
            var title = (from p in _context.PageModel
                         where p.PageUrl == "/Marketing/Campaign"
                         && p.Parameter.Contains(Type)
                         select p.PageName).FirstOrDefault();
            ViewBag.Title = title;
            var roleHasSendSMSPermission = (from acc in _context.AccountModel
                                            from accRole in acc.RolesModel
                                            join r in _context.RolesModel on accRole.RolesId equals r.RolesId
                                            where r.isSendSMSPermission == true && acc.AccountId == CurrentUser.AccountId
                                            select acc).FirstOrDefault();
            ViewBag.roleHasSendSMSPermission = roleHasSendSMSPermission;


        }

        [HttpPost]
        public ActionResult SendSMS(Guid? Id)
        {
            return ExecuteContainer(() =>
            { //Check if role is has send SMS permission
                var roleHasSendSMSPermission = (from acc in _context.AccountModel
                                                from accRole in acc.RolesModel
                                                join r in _context.RolesModel on accRole.RolesId equals r.RolesId
                                                where r.isSendSMSPermission == true && acc.AccountId == CurrentUser.AccountId
                                                select acc).FirstOrDefault();
                if (roleHasSendSMSPermission == null)
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.NotFound,
                        Success = false,
                        Data = "Không có quyền gửi SMS. Vui lòng liên hệ quản trị viên!"
                    });
                }
                var profileList = new List<MemberOfTargetGroupInCampaignViewModel>();
                string SMSContent = string.Empty;
                var Campaign = _unitOfWork.CampaignRepository.GetBy(Id);
                if (Campaign != null)
                {
                    var content = _unitOfWork.ContentRepository.GetBy(Campaign.ContentId);
                    if (content == null || content.CatalogCode != ConstCatalogType.SMS)
                    {
                        return Json(new
                        {
                            Code = System.Net.HttpStatusCode.NotFound,
                            Success = false,
                            Data = "Nội dung này không thể gửi SMS"
                        });
                    }
                    #region Phone needs to be sent
                    //Using table: SendMailCalendarModel
                    profileList = (from sendMailCalendar in _context.SendMailCalendarModel
                                     join campaign in _context.CampaignModel on sendMailCalendar.CampaignId equals campaign.Id
                                     where sendMailCalendar.IsSend == false //chưa gửi
                                           && sendMailCalendar.isError != true // chưa có lỗi
                                           && campaign.Id == Id
                                     orderby campaign.CampaignCode
                                     select new MemberOfTargetGroupInCampaignViewModel
                                     {
                                         SendMailId = sendMailCalendar.Id,
                                         ProfileId = sendMailCalendar.ToProfileId,
                                     }).ToList();
                    //Lấy danh sách phone
                    if (profileList != null && profileList.Count > 0)
                    {
                        foreach (var item in profileList)
                        {
                            var totalInternalMember = (from c in _context.CampaignModel
                                                       join m in _context.MemberOfTargetGroupModel on c.TargetGroupId equals m.TargetGroupId
                                                       where c.Id == Id && m.ProfileId == item.ProfileId
                                                       select m).FirstOrDefault();
                            if (totalInternalMember != null)
                            {
                                var profile = (from p in _context.ProfileModel
                                               where p.ProfileId == totalInternalMember.ProfileId
                                               select p).FirstOrDefault();
                                if (profile != null)
                                {
                                    item.Phone = profile.Phone;
                                    item.ProfileName = profile.ProfileShortName;
                                }
                            }
                            else
                            {
                                var totalExternalMember = (from c in _context.CampaignModel
                                                           join m in _context.MemberOfExternalProfileTargetGroupModel on c.TargetGroupId equals m.TargetGroupId
                                                           where c.Id == Id && m.ExternalProfileTargetGroupId == item.ProfileId
                                                           select m).FirstOrDefault();

                                if (totalExternalMember != null)
                                {
                                    item.Phone = totalExternalMember.Phone;
                                    item.ProfileName = totalExternalMember.FullName;
                                }
                            }
                        }
                    }
                    else
                    {
                        return Json(new
                        {
                            Code = System.Net.HttpStatusCode.NotFound,
                            Success = false,
                            Data = "Không tìm thấy SĐT nào để gửi!"
                        });
                    }
                    #endregion
                    if (profileList != null && profileList.Count > 0)
                    {
                        foreach (var item in profileList)
                        {
                            SendSMSViewModel smsViewModel = new SendSMSViewModel();
                            smsViewModel.PhoneNumber = item.Phone;
                            //Lấy config trong danh mục dùng chung (Content: CatalogCode: SMS)
                            var config = _unitOfWork.CatalogRepository.GetBy(content.SentFrom, ConstCatalogType.SMSConfig);
                            //CatalogText_vi: BrandName
                            //CatalogText_en: Token
                            string brandName = config.CatalogText_vi;
                            string tokenSMS = config.CatalogText_en;
                            string message = string.Empty;
                            //brand name
                            smsViewModel.BrandName = brandName;
                            //token
                            smsViewModel.Token = tokenSMS;

                            message = content.Content.Replace("##FullName##", item.ProfileName);
                            //smsViewModel.Message = string.Format("Cam on quy khach da den tham quan {0}", store.StoreName);
                            bool isSent = false;
                            if (message != null)
                            {
                                isSent = true;
                                smsViewModel.Message = message;
                                SMSContent = message;
                            }
                            else
                            {
                                isSent = false;
                            }

                            //Nếu đủ điều kiện gửi SMS thì mới tiến hành gửi
                            if (isSent == true)
                            {
                                var response = _unitOfWork.SendSMSRepository.SendSMSToCustomer(smsViewModel);
                                if (response != null)
                                {
                                    if (response.isSent == false)
                                    {
                                        var send = _context.SendMailCalendarModel.FirstOrDefault(s => s.Id == item.SendMailId);
                                        send.IsBounce = true;
                                        send.isError = true;
                                        send.ErrorMessage = response.ErrorMessage;
                                        //return Json(new
                                        //{
                                        //    Code = System.Net.HttpStatusCode.NotFound,
                                        //    Success = false,
                                        //    Data = "Đã xảy ra lỗi khi gửi tin: " + response.ErrorMessage
                                        //});
                                    }
                                    else
                                    {   
                                        var send = _context.SendMailCalendarModel.FirstOrDefault(s => s.Id == item.SendMailId);
                                        send.IsSend = true;
                                        send.SendTime = DateTime.Now;
                                        _context.Entry(send).State = EntityState.Modified;
                                    }
                                    _context.SaveChanges();

                                }
                            }
                        }
                        //Danh sách đã gửi
                        var SentList = _context.SendMailCalendarModel.Where(x => x.CampaignId == Id && x.IsSend == true);
                        //Danh sách tổng
                        var AllList = _context.SendMailCalendarModel.Where(x => x.CampaignId == Id && x.IsBounce == false);
                        if (SentList == null || SentList.Count() == 0)
                        {
                            Campaign.Status = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.Campaign_Planned, ConstCatalogType.CampaignStatus).CatalogId;
                        }
                        else if(SentList.Count() >= AllList.Count())
                        {
                            Campaign.Status = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.Campaign_Finnished,ConstCatalogType.CampaignStatus).CatalogId;
                        }
                        else if(SentList.Count() < AllList.Count())
                        {
                            Campaign.Status = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.Campaign_Actived, ConstCatalogType.CampaignStatus).CatalogId;
                        }
                        _context.SaveChanges();
                    }
                    else
                    {
                        return Json(new
                        {
                            Code = System.Net.HttpStatusCode.NotFound,
                            Success = false,
                            Data = "Không tìm thấy SĐT nào để gửi!"
                        });
                    }
                }

                return Json(new
                {
                    Code = System.Net.HttpStatusCode.Accepted,
                    Success = true,
                    Data = string.Format("Đã gửi thành công SMS tới {0} SĐT", profileList.Count)
                });
            });
        }
    }
}