using System.Web.Mvc;

namespace ISD.Admin.Areas.Work
{
    public class WorkAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Work";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Work_default",
                "Work/{controller}/{action}/{id}",
                new { controller = "Work", action = "Index", id = UrlParameter.Optional },
                new string[] { "Work.Controllers" }
            );

            //context.MapRoute(
            //    "Permission_Account",
            //    "Permission/Account/{action}/{id}",
            //    new { controller = "Account", action = "Index", id = UrlParameter.Optional },
            //    new string[] { "Permission.Controllers" }
            //);
        }
    }
}