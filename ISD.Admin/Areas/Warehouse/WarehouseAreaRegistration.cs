using System.Web.Mvc;

namespace ISD.Admin.Areas.Warehouse
{
    public class WarehouseAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Warehouse";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Warehouse_default",
                "Warehouse/{controller}/{action}/{id}",
                new { controller = "Warehouse", action = "Index", id = UrlParameter.Optional },
                new string[] { "Warehouse.Controllers" }
            );
        }
    }
}