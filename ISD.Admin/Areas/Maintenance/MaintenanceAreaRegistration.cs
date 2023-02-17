using System.Web.Mvc;

namespace ISD.Admin.Areas.Maintenance
{
    public class MaintenanceAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Maintenance";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Maintenance_default",
                "Maintenance/{controller}/{action}/{id}",
                new { controller = "Maintenance", action = "Index", id = UrlParameter.Optional },
                new string[] { "Maintenance.Controllers" }
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