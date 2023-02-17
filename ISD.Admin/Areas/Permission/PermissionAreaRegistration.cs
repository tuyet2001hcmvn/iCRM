﻿using System.Web.Mvc;

namespace ISD.Admin.Areas.Permission
{
    public class PermissionAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Permission";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Permission_default",
                "Permission/{controller}/{action}/{id}",
                new { controller = "Permission", action = "Index", id = UrlParameter.Optional },
                new string[] { "Permission.Controllers" }
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