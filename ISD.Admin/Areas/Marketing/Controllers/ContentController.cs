using ISD.Constant;
using ISD.Core;
using ISD.EntityModels;
using ISD.Resources;
using ISD.ViewModels.Marketing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Marketing.Controllers
{
    public class ContentController : BaseController
    {
        // GET: Content
        public ActionResult Index(string Type)
        {
            string pageUrl = "/Marketing/Content";
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
        public ActionResult Create(string Type)
        {
            var viewModel = new ContentCreateViewModel()
            {
                Type = Type,
            };
            var companyList = _context.CompanyModel.Where(p => p.Actived == true).OrderBy(p => p.CompanyCode).ToList().Select(s => new CompanyModel
            {
                CompanyCode = s.CompanyCode,
                CompanyName = s.CompanyCode + " | " + s.CompanyName
            });
            ViewBag.CompanyCode = new SelectList(companyList, "CompanyCode", "CompanyName");
            var saleOrg = _context.StoreModel.Where(p => p.Actived == true).OrderBy(p => p.SaleOrgCode).ToList().Select(s => new StoreModel
            {
                SaleOrgCode = s.SaleOrgCode,
                StoreName = s.SaleOrgCode + " | " + s.StoreName
            });
            ViewBag.SaleOrg = new SelectList(saleOrg, "SaleOrgCode", "StoreName");
            CreateViewBag(Type: Type);
            return View(viewModel);
        }

        public ActionResult Edit(Guid Id, string Type)
        {
            ViewBag.Id = Id;
            var companyList = _context.CompanyModel.Where(p => p.Actived == true).OrderBy(p => p.CompanyCode).ToList().Select(s => new CompanyModel
            {
                CompanyCode = s.CompanyCode,
                CompanyName = s.CompanyCode + " | " + s.CompanyName
            });
            ViewBag.CompanyCode = new SelectList(companyList, "CompanyCode", "CompanyName");
            var saleOrg = _context.StoreModel.Where(p => p.Actived == true).OrderBy(p => p.SaleOrgCode).ToList().Select(s => new StoreModel
            {
                SaleOrgCode = s.SaleOrgCode,
                StoreName = s.SaleOrgCode + " | " + s.StoreName
            });
            ViewBag.SaleOrg = new SelectList(saleOrg, "SaleOrgCode", "StoreName");
            CreateViewBag(Type: Type);
            return View();
        }

        public void CreateViewBag(string Type = null)
        {
            ViewBag.Type = Type;
            var title = (from p in _context.PageModel
                         where p.PageUrl == "/Marketing/Content"
                         && p.Parameter.Contains(Type)
                         select p.PageName).FirstOrDefault();
            ViewBag.Title = title;

            var ContentTypeList = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.Marketing_Content);
            ViewBag.CatalogCode = new SelectList(ContentTypeList, "CatalogCode", "CatalogText_vi");

            var EmailTypeList = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.Marketing_Content_Email_Type);
            ViewBag.EmailType = new SelectList(EmailTypeList, "CatalogCode", "CatalogText_vi");

            var BrandNameList = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.SMSConfig);
            ViewBag.SentFrom = new SelectList(BrandNameList, "CatalogCode", "CatalogText_vi");

            var message = _unitOfWork.CatalogRepository.GetBy(ConstCatalogType.SMSTemplate);
            ViewBag.Content = new SelectList(message, "CatalogText_vi", "CatalogText_vi");
        }
    }
}