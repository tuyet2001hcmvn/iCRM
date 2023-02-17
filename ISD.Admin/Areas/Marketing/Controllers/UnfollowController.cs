using ISD.Core;
using ISD.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Marketing.Controllers
{
    public class UnfollowController : BaseController
    {
        // GET: Unfollow
        public ActionResult Index()
        {
            var companyList = _context.CompanyModel.Where(p => p.Actived == true).OrderBy(p => p.CompanyCode).ToList().Select(s => new CompanyModel
            {
                CompanyCode = s.CompanyCode,
                CompanyName = s.CompanyCode + " | " + s.CompanyName
            });
            ViewBag.CompanyCode = new SelectList(companyList, "CompanyCode", "CompanyName");
            return View();
        }
        public ActionResult Create()
        {
            var companyList = _context.CompanyModel.Where(p => p.Actived == true).OrderBy(p => p.CompanyCode).ToList().Select(s => new CompanyModel
            {
                CompanyCode = s.CompanyCode,
                CompanyName = s.CompanyCode + " | " + s.CompanyName
            });
            ViewBag.CompanyCode = new SelectList(companyList, "CompanyCode", "CompanyName");
            return View();
        }
    }
}