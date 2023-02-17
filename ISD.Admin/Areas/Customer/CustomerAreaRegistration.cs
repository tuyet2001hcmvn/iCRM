using System.Web.Mvc;

namespace ISD.Admin.Areas.Customer
{
    public class CustomerAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Customer";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Customer_default",
                "Customer/{controller}/{action}/{id}",
                new { controller = "Customer", action = "Index", id = UrlParameter.Optional },
                new string[] { "Customer.Controllers" }
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