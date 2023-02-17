using ISD.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MasterData.Controllers
{
    public class FavoriteReportController : BaseController
    {
        // GET: FavoriteReport
        public ActionResult Index()
        {           
            return View();
        }
    }
}