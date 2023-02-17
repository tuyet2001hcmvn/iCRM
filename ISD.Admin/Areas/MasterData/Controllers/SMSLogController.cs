using ISD.Core;
using ISD.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MasterData.Controllers
{
    public class SMSLogController : BaseController
    {
        // GET: SMSLog
        [ISDAuthorizationAttribute]
        public ActionResult Index()
        {
            ViewBag.PageId = GetPageId("/MasterData/SMSLog");
            return View();
        }
        public ActionResult _Search(DateTime? FromDate, DateTime? ToDate, string PhoneNumber, string BrandName)
        {
            var result = (from p in _context.SMSModel
                          where (FromDate == null || p.CreateDate >= FromDate)
                          && (ToDate == null || p.CreateDate <= ToDate)
                          //Search by phone
                          && (PhoneNumber == "" || p.SMSTo == PhoneNumber)
                          //Search by brand
                          && (BrandName == "" || p.BrandName.Contains(BrandName))
                          orderby p.CreateDate descending
                          select p).ToList();
            return PartialView(result);
        }
    }
}