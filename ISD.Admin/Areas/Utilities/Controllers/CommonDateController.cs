using ISD.Extensions;
using ISD.Repositories;
using System;
using System.Web.Mvc;
using ISD.Core;

namespace Utilities.Controllers
{
    public class CommonDateController : BaseController
    {
        // GET: CommonDate
        public ActionResult Get(string CommonDate)
        {
            DateTime? fromDate;
            DateTime? toDate;

            CommonDateRepository _commonDateRepository = new CommonDateRepository(_context);
            _commonDateRepository.GetDateBy(CommonDate, out fromDate, out toDate);

            return Json(new
            {
                FromDate = string.Format("{0:yyyy-MM-dd}", fromDate),
                ToDate = string.Format("{0:yyyy-MM-dd}", toDate)
            }, JsonRequestBehavior.AllowGet);
        }
    }
}