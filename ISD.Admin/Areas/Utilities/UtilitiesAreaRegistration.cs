using System.Web.Mvc;

namespace ISD.Admin.Areas.Utilities
{
    public class UtilitiesAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Utilities";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Utilities_default",
                "Utilities/{controller}/{action}/{id}",
                new { controller = "Utilities", action = "Index", id = UrlParameter.Optional },
                new string[] { "Utilities.Controllers" }
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