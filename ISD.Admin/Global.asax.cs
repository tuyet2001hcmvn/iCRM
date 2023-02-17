using AutoMapper;
using ISD.Admin.App_Start;
using ISD.Admin.Controllers;
using ISD.Core;
using ISD.EntityModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace ISD.Admin
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimsIdentity.DefaultNameClaimType;

            // Bug using IRunAtInit => Use later
            //var tasks = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes())
            //    .Where(x => typeof(IRunAtInit).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
            //    .Select(x => (IRunAtInit)Activator.CreateInstance(x)).ToList();

            //foreach (var r in tasks)
            //{
            //    r.Execute();
            //}
            Mapper.Initialize(cfg =>
            {

            });

        }

        public void Application_BeginRequest(Object sender, EventArgs e)
        {
        }

        public void Application_Error(Object sender, EventArgs e)
        {
            //if (User.Identity.IsAuthenticated)
            //{
            //    Exception exception = Server.GetLastError();
            //    Server.ClearError();

            //    var routeData = new RouteData();
            //    routeData.Values.Add("controller", "Error");
            //    routeData.Values.Add("action", "Error");
            //    routeData.Values.Add("exception", exception);

            //    if (exception.GetType() == typeof(HttpException))
            //    {
            //        routeData.Values.Add("statusCode", ((HttpException)exception).GetHttpCode());
            //    }
            //    else
            //    {
            //        routeData.Values.Add("statusCode", 500);
            //    }

            //    Response.TrySkipIisCustomErrors = true;
            //    IController controller = new ErrorController();
            //    controller.Execute(new RequestContext(new HttpContextWrapper(Context), routeData));
            //    Response.End();
            //}
        }

        public void Application_EndRequest(Object sender, EventArgs e)
        {

        }
    }
}
