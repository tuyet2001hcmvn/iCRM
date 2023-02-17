using System.Web.Mvc;

namespace ISD.Admin.Areas.MasterData
{
    public class MasterDataAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "MasterData";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "MasterData_default",
                "MasterData/{controller}/{action}/{id}",
                new { controller = "MasterData", action = "Index", id = UrlParameter.Optional },
                new string[] { "MasterData.Controllers" }
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