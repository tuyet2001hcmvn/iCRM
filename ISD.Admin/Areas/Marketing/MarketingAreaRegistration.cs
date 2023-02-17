using System.Web.Mvc;

namespace ISD.Admin.Areas.Marketing
{
    public class MarketingAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Marketing";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Marketing_default",
                "Marketing/{controller}/{action}/{id}",
                new { controller = "Marketing", action = "Index", id = UrlParameter.Optional },
                new string[] { "Marketing.Controllers" }
            );
        }
    }
}