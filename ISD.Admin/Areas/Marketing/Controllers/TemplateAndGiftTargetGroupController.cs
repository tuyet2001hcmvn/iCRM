using ISD.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Marketing.Controllers
{
    public class TemplateAndGiftTargetGroupController : BaseController
    {
        // GET: TemplateAndGiftTargetGroup
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Create()
        {
            return View();
        }
        public ActionResult Edit(Guid Id)
        {
            ViewBag.Id = Id;
            return View();
        }
    }
}