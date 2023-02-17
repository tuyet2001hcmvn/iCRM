using ISD.EntityModels;
using ISD.Resources;
using ISD.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ISD.Core;
using ISD.Extensions;

namespace Utilities.Controllers
{
    public class SettingController : BaseController
    {
        // GET: Setting
        #region Setting
        //public ActionResult Index()
        //{
        //    return View();
        //}

        //public ActionResult _Search(string ResourceKey, string ResourceValue)
        //{
        //    return ExecuteSearch(() =>
        //    {
        //        var ResourceKeyIsNullOrEmpty = string.IsNullOrEmpty(ResourceKey);
        //        var ResourceValueIsNullOrEmpty = string.IsNullOrEmpty(ResourceValue);
        //        var resourceList = _context.ResourceModel.Where(p => ResourceKeyIsNullOrEmpty || p.ResourceKey.ToLower().Contains(ResourceKey.ToLower())
        //                                                             && ResourceValueIsNullOrEmpty || p.ResourceValue.ToLower().Contains(ResourceValue.ToLower()))
        //                                                    .Select(p => new ResourceViewModel()
        //                                                    {
        //                                                        ResourceKey = p.ResourceKey,
        //                                                        ResourceValue = p.ResourceValue,
        //                                                        ResourceComment = p.ResourceComment
        //                                                    })
        //                                                    .OrderBy(p => p.ResourceKey)
        //                                                    .ToList();
        //        return PartialView(resourceList);
        //    });
        //}
        [ISDAuthorizationAttribute]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult _Search(string ResourceKey, string ResourceValue)
        {
            return ExecuteSearch(() =>
            {
                var ResourceKeyIsNullOrEmpty = string.IsNullOrEmpty(ResourceKey);
                var ResourceValueIsNullOrEmpty = string.IsNullOrEmpty(ResourceValue);
                var resourceList = _context.ResourceModel.Where(p => ResourceKeyIsNullOrEmpty || p.ResourceKey.ToLower().Contains(ResourceKey.ToLower())
                                                                     && ResourceValueIsNullOrEmpty || p.ResourceValue.ToLower().Contains(ResourceValue.ToLower()))
                                                            .Select(p => new ResourceViewModel()
                                                            {
                                                                ResourceKey = p.ResourceKey,
                                                                ResourceValue = p.ResourceValue,
                                                                ResourceComment = p.ResourceComment
                                                            })
                                                            .OrderBy(p => p.ResourceKey)
                                                            .ToList();
                return PartialView(resourceList);
            });
        }

        [ISDAuthorizationAttribute]
        public ActionResult Create()
        {
            return View();
        }
        //POST: Create
        [HttpPost]
        [ValidateAjax]
        [ISDAuthorizationAttribute]
        public JsonResult Create(ResourceModel model)
        {
            return ExecuteContainer(() =>
            {
                _context.Entry(model).State = EntityState.Added;
                _context.SaveChanges();

                return Json(new
                {
                    Code = System.Net.HttpStatusCode.Created,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Create_Success, LanguageResource.Utilities_Setting.ToLower())
                });
            });
        }

        [ISDAuthorizationAttribute]
        public ActionResult Edit(string id)
        {
            var resource = (from p in _context.ResourceModel
                            where p.ResourceKey == id
                            select new ResourceViewModel()
                            {
                                ResourceKey = p.ResourceKey,
                                ResourceValue = p.ResourceValue,
                                ResourceComment = p.ResourceComment
                            }).FirstOrDefault();
            if (resource == null)
            {
                return RedirectToAction("ErrorText", "Error", new { area = "", statusCode = 404, exception = string.Format(LanguageResource.Error_NotExist, LanguageResource.MasterData_Province.ToLower()) });
            }

            return View(resource);
        }
        //POST: Edit
        [HttpPost]
        [ISDAuthorizationAttribute]
        public JsonResult Edit(ResourceModel model)
        {
            return ExecuteContainer(() =>
            {
                _context.Entry(model).State = EntityState.Modified;
                _context.SaveChanges();

                return Json(new
                {
                    Code = System.Net.HttpStatusCode.Created,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Edit_Success, LanguageResource.Utilities_Setting.ToLower())
                });
            });
        }
        #endregion

        #region Configuration

        #region Configuration
        //GET
        public ActionResult Configuration()
        {
            var about = _context.AboutModel.FirstOrDefault();
            var contact = _context.ContactModel.FirstOrDefault();
            var api = _context.APIModel.FirstOrDefault();
            var emailConfig = _context.EmailConfig.FirstOrDefault();

            ConfigurationViewModel model = new ConfigurationViewModel();
            if (about != null)
            {
                model.AboutTitle = about.AboutTitle;
                model.AboutDescription = about.AboutDescription;
            }
            if (contact != null)
            {
                model.ContactDescription = contact.ContactDescription;
                model.ReviewDescription = contact.ReviewDescription;
            }
            if (api != null)
            {
                model.Token = api.Token;
                model.Key = api.Key;
                model.isAllowedToBooking = api.isAllowedToBooking;
                model.isRequiredLogin = api.isRequiredLogin;
            }
            if (emailConfig != null)
            {
                model.Email = emailConfig.Email;
                model.SmtpServer = emailConfig.SmtpServer;
                model.SmtpPort = emailConfig.SmtpPort;
                model.EnableSsl = emailConfig.EnableSsl;
                model.SmtpMailFrom = emailConfig.SmtpMailFrom;
                model.SmtpUser = emailConfig.SmtpUser;
                model.SmtpPassword = emailConfig.SmtpPassword;
                model.ToEmail = emailConfig.ToEmail;
                model.CCMail = emailConfig.CCMail;
                model.BCCMail = emailConfig.BCCMail;
                model.EmailTitle = emailConfig.EmailTitle;
                model.EmailContent = emailConfig.EmailContent;
            }
            return View(model);
        }
        //POST
        [HttpPost]
        [ValidateInput(false)] //need when using ckeditor, do not delete
        public ActionResult Configuration(ConfigurationViewModel model, HttpPostedFileBase Logo, HttpPostedFileBase Icon)
        {
            try
            {
                //Logo and Icon
                if (Logo != null)
                {
                    getFileName(Logo);
                }
                if (Icon != null)
                {
                    getFileName(Icon);
                }

                //API token and Key
                var api = _context.APIModel.FirstOrDefault();
                if (api == null)
                {
                    APIModel apiModel = new APIModel();
                    apiModel.Token = _unitOfWork.RepositoryLibrary.GetMd5Sum("TienThu");
                    apiModel.Key = Guid.NewGuid().ToString();
                    apiModel.isAllowedToBooking = model.isAllowedToBooking;
                    api.isRequiredLogin = model.isRequiredLogin;
                    _context.Entry(apiModel).State = EntityState.Added;
                }
                else
                {
                    api.Key = model.Key;
                    api.isAllowedToBooking = model.isAllowedToBooking;
                    api.isRequiredLogin = model.isRequiredLogin;
                    _context.Entry(api).State = EntityState.Modified;
                }

                //About
                var about = _context.AboutModel.FirstOrDefault();
                if (about == null)
                {
                    AboutModel aboutModel = new AboutModel();
                    aboutModel.AboutId = Guid.NewGuid();
                    aboutModel.AboutTitle = model.AboutTitle;
                    aboutModel.AboutDescription = model.AboutDescription;
                    _context.Entry(aboutModel).State = EntityState.Added;
                }
                else
                {
                    about.AboutTitle = model.AboutTitle;
                    about.AboutDescription = model.AboutDescription;
                    _context.Entry(about).State = EntityState.Modified;
                }

                //Contact
                var contact = _context.ContactModel.FirstOrDefault();
                if (contact == null)
                {
                    ContactModel contactModel = new ContactModel();
                    contactModel.ContactId = Guid.NewGuid();
                    contactModel.ReviewDescription = model.ReviewDescription;
                    contactModel.ContactDescription = model.ContactDescription;
                    _context.Entry(contactModel).State = EntityState.Added;
                }
                else
                {
                    contact.ReviewDescription = model.ReviewDescription;
                    contact.ContactDescription = model.ContactDescription;
                    _context.Entry(contact).State = EntityState.Modified;
                }

                //SMTP
                var emailConfig = _context.EmailConfig.FirstOrDefault();
                if (emailConfig == null)
                {
                    EmailConfig emailConfigModel = new EmailConfig();
                    emailConfigModel.EmailConfigId = Guid.NewGuid();
                    emailConfigModel.Email = model.Email;
                    emailConfigModel.SmtpServer = model.SmtpServer;
                    emailConfigModel.SmtpPort = model.SmtpPort;
                    emailConfigModel.EnableSsl = model.EnableSsl;
                    emailConfigModel.SmtpMailFrom = model.SmtpMailFrom;
                    emailConfigModel.SmtpUser = model.SmtpUser;
                    emailConfigModel.SmtpPassword = model.SmtpPassword;
                    emailConfigModel.ToEmail = model.ToEmail;
                    emailConfigModel.CCMail = model.CCMail;
                    emailConfigModel.BCCMail = model.BCCMail;
                    emailConfigModel.EmailTitle = model.EmailTitle;
                    emailConfigModel.EmailContent = model.EmailContent;
                    _context.Entry(emailConfigModel).State = EntityState.Added;
                }
                else
                {
                    emailConfig.Email = model.Email;
                    emailConfig.SmtpServer = model.SmtpServer;
                    emailConfig.SmtpPort = model.SmtpPort;
                    emailConfig.EnableSsl = model.EnableSsl;
                    emailConfig.SmtpMailFrom = model.SmtpMailFrom;
                    emailConfig.SmtpUser = model.SmtpUser;
                    emailConfig.SmtpPassword = model.SmtpPassword;
                    emailConfig.ToEmail = model.ToEmail;
                    emailConfig.CCMail = model.CCMail;
                    emailConfig.BCCMail = model.BCCMail;
                    emailConfig.EmailTitle = model.EmailTitle;
                    emailConfig.EmailContent = model.EmailContent;
                    _context.Entry(emailConfig).State = EntityState.Modified;
                }

                _context.SaveChanges();

                return Json(new
                {
                    Code = System.Net.HttpStatusCode.Created,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Edit_Success, LanguageResource.Configuration.ToLower())
                });
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.Created,
                        Success = false,
                        Data = ex.InnerException.Message
                    });
                }
                else
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.Created,
                        Success = false,
                        Data = ex.Message
                    });
                }
            }
        }
        #endregion

        public string getFileName(HttpPostedFileBase file)
        {
            var fileName = Path.GetFileName(file.FileName);

            //Create dynamically folder to save file
            var existPath = Server.MapPath("~/Upload/");
            Directory.CreateDirectory(existPath);
            var path = Path.Combine(existPath, fileName);

            file.SaveAs(path);

            return fileName;
        }

        #region ContactDetail
        public ActionResult _ContactDetail()
        {
            var detailList = _context.ContactDetailModel.Select(p =>
                                                        new ContactDetailViewModel()
                                                        {
                                                            ContactDetailId = p.ContactDetailId,
                                                            Title = p.Title,
                                                            DisplayPhone = p.DisplayPhone,
                                                            Phone = p.Phone,
                                                            OrderIndex = p.OrderIndex
                                                        }).OrderBy(p => p.OrderIndex).ToList();
            if (detailList == null)
            {
                detailList = new List<ContactDetailViewModel>();
            }
            return PartialView(detailList);
        }
        //show list source
        public ActionResult _ContactDetailInner(List<ContactDetailViewModel> detailList = null)
        {
            if (detailList == null)
            {
                detailList = new List<ContactDetailViewModel>();
            }
            return PartialView(detailList);
        }
        //insert new source
        public ActionResult InsertContactDetail(string Title, string DisplayPhone, string Phone, int? OrderIndex)
        {
            ContactDetailModel model = new ContactDetailModel();
            model.ContactDetailId = Guid.NewGuid();
            model.Title = Title;
            model.DisplayPhone = DisplayPhone;
            model.Phone = Phone;
            model.OrderIndex = OrderIndex;

            _context.Entry(model).State = EntityState.Added;
            _context.SaveChanges();

            var detailList = _context.ContactDetailModel.Select(p =>
                                                        new ContactDetailViewModel()
                                                        {
                                                            ContactDetailId = p.ContactDetailId,
                                                            Title = p.Title,
                                                            DisplayPhone = p.DisplayPhone,
                                                            Phone = p.Phone,
                                                            OrderIndex = p.OrderIndex
                                                        }).OrderBy(p => p.OrderIndex).ToList();
            return PartialView("_ContactDetailInner", detailList);
        }
        //delete source
        public ActionResult DeleteContactDetail(Guid id)
        {
            var detail = _context.ContactDetailModel.FirstOrDefault(p => p.ContactDetailId == id);
            if (detail != null)
            {
                _context.Entry(detail).State = EntityState.Deleted;
                _context.SaveChanges();
            }
            var detailList = _context.ContactDetailModel.Select(p =>
                                                        new ContactDetailViewModel()
                                                        {
                                                            ContactDetailId = p.ContactDetailId,
                                                            Title = p.Title,
                                                            DisplayPhone = p.DisplayPhone,
                                                            Phone = p.Phone,
                                                            OrderIndex = p.OrderIndex
                                                        }).OrderBy(p => p.OrderIndex).ToList();
            return PartialView("_ContactDetailInner", detailList);
        }
        #endregion ContactDetail

        #region Banner
        public ActionResult _Banner()
        {
            var bannerList = _context.BannerModel.Select(p =>
                                                        new ConfigurationViewModel()
                                                        {
                                                            BannerId = p.BannerId,
                                                            ImageUrl = p.ImageUrl
                                                        }).ToList();
            if (bannerList == null)
            {
                bannerList = new List<ConfigurationViewModel>();
            }
            return PartialView(bannerList);
        }
        //show list source
        public ActionResult _BannerInner(List<ConfigurationViewModel> bannerList = null)
        {
            if (bannerList == null)
            {
                bannerList = new List<ConfigurationViewModel>();
            }
            return PartialView(bannerList);
        }
        //insert new source
        public ActionResult InsertBanner(HttpPostedFileBase ImageUrl)
        {
            BannerModel model = new BannerModel();
            model.BannerId = Guid.NewGuid();
            model.ImageUrl = Upload(ImageUrl, "Banner");
            model.CreatedTime = DateTime.Now;

            _context.Entry(model).State = EntityState.Added;
            _context.SaveChanges();

            var bannerList = _context.BannerModel.Select(p =>
                                                        new ConfigurationViewModel()
                                                        {
                                                            BannerId = p.BannerId,
                                                            ImageUrl = p.ImageUrl
                                                        }).ToList();
            return PartialView("_BannerInner", bannerList);
        }
        //delete source
        public ActionResult DeleteBanner(Guid id)
        {
            var banner = _context.BannerModel.FirstOrDefault(p => p.BannerId == id);
            if (banner != null)
            {
                _context.Entry(banner).State = EntityState.Deleted;
                _context.SaveChanges();
            }
            var bannerList = _context.BannerModel.Select(p =>
                                                        new ConfigurationViewModel()
                                                        {
                                                            BannerId = p.BannerId,
                                                            ImageUrl = p.ImageUrl
                                                        }).ToList();
            return PartialView("_BannerInner", bannerList);
        }
        #endregion Source

        #endregion Configuration
    }
}