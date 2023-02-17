using ISD.EntityModels;
using System;
using System.Web;
using System.Web.Mvc;
using ISD.Core;
using ISD.ViewModels;
using System.Linq;

namespace ISD.Admin.Controllers
{
    public class ModuleController : BaseController
    {
        // GET: Module/ModuleId
        public ActionResult SelectedModule(Guid ModuleId)
        {
            ModuleModel module = _context.ModuleModel.Find(ModuleId);
            if (module == null)
            {
                return HttpNotFound();
            }
            //Save selected module to cookie
            HttpCookie selectedModule = new HttpCookie("selected_module");
            selectedModule["ModuleId"] = ModuleId.ToString();
            selectedModule["ModuleName"] = module.ModuleName;
            selectedModule.Expires = DateTime.Now.AddDays(30);
            Response.Cookies.Add(selectedModule);
            ViewBag.SelectedModuleId = module.ModuleId;
            ViewBag.SelectedModuleName = module.ModuleName;
            //Get By Module
            PermissionViewModel lst = (PermissionViewModel)Session["Menu"];
            var ret = new PermissionViewModel();
            //Get Page By Module
            ret.PageModel = lst.PageModel.Where(p => p.ModuleId == module.ModuleId).ToList();
            ViewBag.PageModel = ret.PageModel;
            //Get menu by Page
            ret.MenuModel = (from m in lst.MenuModel
                             join p in ret.PageModel on m.MenuId equals p.MenuId
                             select m).Distinct().ToList();
            ViewBag.MenuModel = ret.MenuModel;
            return View(module);
        }
    }
}