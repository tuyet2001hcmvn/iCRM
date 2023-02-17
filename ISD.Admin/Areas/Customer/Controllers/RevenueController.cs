using ISD.Extensions;
using ISD.Repositories;
using ISD.Resources;
using ISD.Core;
using System;
using System.Linq;
using System.Web.Mvc;
using ISD.ViewModels;
using System.Collections.Generic;

namespace Customer.Controllers
{
    public class RevenueController : BaseController
    {
        // GET: Revenue
        #region Index
        [ISDAuthorizationAttribute]
        public ActionResult Index()
        {
            ViewBag.PageId = GetPageId("/Customer/Revenue");
            CreateViewBag();
            return View();
        }
        public ActionResult _Search(Guid? CompanyId = null, string CustomerCode = "", string PhoneNumber = "", decimal? FromLimit = null, decimal? ToLimit = null, int? NumberOfRows = 100)
        {
            return ExecuteSearch(() =>
            {
                if (CompanyId == null || FromLimit == null || ToLimit == null)
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.InternalServerError,
                        Success = false,
                        Data = LanguageResource.Required_Validation
                    });
                }
                if (FromLimit > ToLimit)
                {
                    return Json(new
                    {
                        Code = System.Net.HttpStatusCode.InternalServerError,
                        Success = false,
                        Data = LanguageResource.Revenue_LimitError
                    });
                }
                var lst = _unitOfWork.RevenueRepository.Search(CompanyId, PhoneNumber, CustomerCode, FromLimit, ToLimit, NumberOfRows);
                return PartialView(lst);
            });
        }
        #endregion

        public void CreateViewBag(Guid? CompanyId = null)
        {
            var companyLst = _context.CompanyModel.Where(p => p.Actived == true)
                                                  .OrderBy(p => p.CompanyCode).ToList();
            ViewBag.CompanyIdList = new SelectList(companyLst, "CompanyId", "CompanyName", CompanyId);
        }

        public ActionResult _ProfileRevenue(Guid? id, string Year, string CompanyCode)
        {
            //var result = _unitOfWork.RevenueRepository.GetProfileRevenue(id);
            //var result = _unitOfWork.RevenueRepository.GetProfileRevenueBy(id, Year);
            if (string.IsNullOrEmpty(CompanyCode))
            {
                CompanyCode = CurrentUser.CompanyCode;
            }
            var result = new List<RevenueViewModel>();
            result = _unitOfWork.RevenueRepository.GetProfileRevenueBy(id, Year, CompanyCode);

            #region ViewBag
            //Năm xem doanh số
            List<ISDSelectStringItem> year = new List<ISDSelectStringItem>();
            int numberOfYear = DateTime.Now.Year - 2017;
            if (numberOfYear > 0)
            {
                for (int i = numberOfYear; i >= 0; i--)
                {
                    year.Add(new ISDSelectStringItem() { id = DateTime.Now.AddYears(-i).Year.ToString(), name = DateTime.Now.AddYears(-i).Year.ToString() });
                }
            }
            ViewBag.Year = new SelectList(year, "id", "name", Year);

            //Nếu không lọc điều kiện thì mới hiển thị tổng cộng
            if (string.IsNullOrEmpty(Year))
            {
                ViewBag.IsSearchAll = true;
            }

            //Danh sách công ty
            var companyLst = _context.CompanyModel.Where(p => p.Actived == true && (p.CompanyCode.Contains("1000") || p.CompanyCode.Contains("2000") || p.CompanyCode.Contains("3000")))
                                                 .OrderBy(p => p.CompanyCode).ToList();
            ViewBag.CompanyCode = new SelectList(companyLst, "CompanyCode", "CompanyName", CompanyCode);
            #endregion ViewBag

            return PartialView(result);
        }
    }
}