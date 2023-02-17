using ISD.Constant;
using ISD.Core;
using ISD.EntityModels;
using ISD.Extensions;
using ISD.Resources;
using ISD.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MasterData.Controllers
{
    public class EmailTemplateConfigController : BaseController
    {
        // GET: EmailTemplateConfig
        #region Index
        [ISDAuthorizationAttribute]
        public ActionResult Index()
        {
            CreateViewBag();
            return View();
        }
        [HttpPost]
        public ActionResult _Search(EmailTemplateConfigSearchViewModel searchViewModel)
        {
            return ExecuteSearch(() =>
            {
                var lst = (from p in _context.EmailTemplateConfigModel
                           join cTemp in _context.CatalogModel on new { CatalogCode = p.EmailTemplateType, CatalogTypeCode = ConstCatalogType.EmailTemplateConfig } equals new { CatalogCode = cTemp.CatalogCode, CatalogTypeCode = cTemp.CatalogTypeCode } into cList
                           from c in cList.DefaultIfEmpty()
                           where (searchViewModel.EmailTemplateType == null || p.EmailTemplateType.Contains(searchViewModel.EmailTemplateType))
                           && (searchViewModel.Actived == null || p.Actived == searchViewModel.Actived)
                           orderby p.CreateTime
                           select new EmailTemplateConfigViewModel{
                               EmailTemplateConfigId = p.EmailTemplateConfigId,
                               EmailTemplateType = p.EmailTemplateType,
                               EmailTypeName = c.CatalogText_vi,
                               FromEmail = p.FromEmail,
                               ToEmail = p.ToEmail,
                               Subject = p.Subject,
                               Content = p.Content,
                               EmailFrom = p.EmailFrom,
                               EmailSender = p.EmailSender,
                               EmailHost = p.EmailHost,
                               EmailPort = p.EmailPort,
                               EmailEnableSsl = p.EmailEnableSsl,
                               EmailAccount = p.EmailAccount,
                               EmailPassword = p.EmailPassword,
                               Actived = p.Actived,
                               CreateBy = p.CreateBy,
                               CreateTime = p.CreateTime,
                               LastEditBy = p.LastEditBy,
                               LastEditTime = p.LastEditTime

                           }).ToList();
                CreateViewBag(searchViewModel.EmailTemplateType);
                return PartialView(lst);
            });
        }

        #endregion

        // GET: ProfileConfig/Create
        #region Create
        [ISDAuthorizationAttribute]
        public ActionResult Create()
        {
            CreateViewBag();
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(EmailTemplateConfigModel model)
        {
            return ExecuteContainer(() =>
            {
                if (_unitOfWork.EmailTemplateConfigRepository.GetByType(model.EmailTemplateType) != null)
                {
                    return Json(new
                    {
                        Code = HttpStatusCode.NotModified,
                        Success = false,
                        Data = string.Format("Loại cấu hình này đã tồn tại!"),
                    });
                }
                model.EmailTemplateConfigId = Guid.NewGuid();
                model.CreateBy = CurrentUser.AccountId;
                model.CreateTime = DateTime.Now;
                _context.Entry(model).State = EntityState.Added;
                _context.SaveChanges();

                return Json(new
                {
                    Code = HttpStatusCode.Created,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Create_Success, LanguageResource.CustomerType.ToLower()),
                });
            });
        }
        #endregion

        // GET: ProfileConfig/Edit
        #region Edit
        [ISDAuthorizationAttribute]
        public ActionResult Edit(Guid? id)
        {
            var emailConfig = (from p in _context.EmailTemplateConfigModel
                            where p.EmailTemplateConfigId == id
                            select new EmailTemplateConfigViewModel
                            {
                                EmailTemplateConfigId = p.EmailTemplateConfigId,
                                EmailTemplateType = p.EmailTemplateType,
                                FromEmail = p.FromEmail,
                                ToEmail = p.ToEmail,
                                Subject = p.Subject,
                                Content = p.Content,
                                EmailFrom = p.EmailFrom,
                                EmailSender = p.EmailSender,
                                EmailHost = p.EmailHost,
                                EmailPort = p.EmailPort,
                                EmailEnableSsl = p.EmailEnableSsl,
                                EmailAccount = p.EmailAccount,
                                EmailPassword = p.EmailPassword,
                                Actived = p.Actived,
                                CreateBy = p.CreateBy,
                                CreateTime = p.CreateTime,
                                LastEditBy = p.LastEditBy,
                                LastEditTime = p.LastEditTime
                            }).FirstOrDefault();
            CreateViewBag(emailConfig.EmailTemplateType);
            return View(emailConfig);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(EmailTemplateConfigViewModel viewModel)
        {
            return ExecuteContainer(() =>
            {
                var existEmailTemplateConfig = _context.EmailTemplateConfigModel
                                            .Where(p => p.EmailTemplateConfigId == viewModel.EmailTemplateConfigId).FirstOrDefault();

                var checkExsistEmailType = _unitOfWork.EmailTemplateConfigRepository.GetByType(viewModel.EmailTemplateType);
                if (checkExsistEmailType != null && existEmailTemplateConfig.EmailTemplateConfigId != checkExsistEmailType.EmailTemplateConfigId)
                {
                    return Json(new
                    {
                        Code = HttpStatusCode.NotModified,
                        Success = false,
                        Data = string.Format("Loại cấu hình này đã tồn tại!"),
                    });
                }

                if (existEmailTemplateConfig != null)
                {
                    existEmailTemplateConfig.EmailTemplateType = viewModel.EmailTemplateType;
                    existEmailTemplateConfig.FromEmail = viewModel.FromEmail;
                    existEmailTemplateConfig.ToEmail = viewModel.ToEmail;
                    existEmailTemplateConfig.Subject = viewModel.Subject;
                    existEmailTemplateConfig.Content = viewModel.Content;
                    existEmailTemplateConfig.EmailFrom = viewModel.EmailFrom;
                    existEmailTemplateConfig.EmailSender = viewModel.EmailSender;
                    existEmailTemplateConfig.EmailHost = viewModel.EmailHost;
                    existEmailTemplateConfig.EmailPort = viewModel.EmailPort;
                    existEmailTemplateConfig.EmailEnableSsl = viewModel.EmailEnableSsl;
                    existEmailTemplateConfig.EmailAccount = viewModel.EmailAccount;
                    existEmailTemplateConfig.EmailPassword = viewModel.EmailPassword;
                    existEmailTemplateConfig.Actived = viewModel.Actived;
                    existEmailTemplateConfig.LastEditBy = CurrentUser.AccountId;
                    existEmailTemplateConfig.LastEditTime = DateTime.Now;
                }
                _context.SaveChanges();

                return Json(new
                {
                    Code = HttpStatusCode.Created,
                    Success = true,
                    Data = string.Format(LanguageResource.Alert_Edit_Success, LanguageResource.CustomerType.ToLower()),
                });
            });
        }
        #endregion

        #region ViewBag
        private void CreateViewBag(string EmailTemplateType = null)
        {
            var emailTemplateType = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.EmailTemplateConfig);
            ViewBag.EmailTemplateType = new SelectList(emailTemplateType, "CatalogCode", "CatalogText_vi", EmailTemplateType);

        }
        #endregion
    }
}