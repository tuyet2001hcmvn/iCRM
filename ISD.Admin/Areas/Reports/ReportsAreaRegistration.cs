using System.Web.Mvc;

namespace ISD.Admin.Areas.Reports
{
    public class ReportsAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Reports";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Reports_default",
                "Reports/{controller}/{action}/{id}",
                new { controller = "Reports", action = "Index", id = UrlParameter.Optional },
                new string[] { "Reports.Controllers" }
            );
        }
    }
}