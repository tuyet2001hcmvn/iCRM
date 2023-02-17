using ISD.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Warehouse.Controllers
{
    public class WarehouseController : BaseController
    {
        // GET: Warehouse
        public ActionResult Index()
        {
            return View();
        }
    }
}