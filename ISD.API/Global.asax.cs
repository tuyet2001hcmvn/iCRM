using ISD.EntityModels;
using ISD.ViewModels;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace ISD.API
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //Mapper.Initialize(cfg =>
            //{
            //    cfg.CreateMap<MobileScreenModel, MobileScreenViewModel>();
            //    cfg.CreateMap<MobileScreenViewModel, MobileScreenModel>();
            //});
        }
    }
}
