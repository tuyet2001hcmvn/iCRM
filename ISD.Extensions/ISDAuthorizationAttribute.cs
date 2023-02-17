using ISD.ViewModels;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ISD.Extensions
{
    public class ISDAuthorizationAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string actionName = filterContext.ActionDescriptor.ActionName;
            string controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            string areaName = "";

            if (filterContext.RouteData.DataTokens["area"] != null)
            {
                areaName = filterContext.RouteData.DataTokens["area"].ToString();
            }
            if (!CheckAccessRight(areaName, actionName, controllerName))
            {
                filterContext.Result = new HttpStatusCodeResult(404);
            }
            else
            {
                base.OnActionExecuting(filterContext);
            }
        }

        public static bool CheckAccessRight(string Area, string Action, string Controller)
        {
            if (HttpContext.Current.Session["Menu"] != null)
            {
                string pageUrl = "";
                //Get PageUrl from user input
                if (!string.IsNullOrEmpty(Area))
                {
                    pageUrl = string.Format("/{0}/{1}", Area, Controller);
                }
                else
                {
                    pageUrl = string.Format("/{0}/{1}", Controller, Action);
                }
                //Get PageUrl from Session["Menu"]
                PermissionViewModel permission = (PermissionViewModel)HttpContext.Current.Session["Menu"];
                var pageId = permission.PageModel.Where(p => p.PageUrl == pageUrl)
                                                .Select(p => p.PageId)
                                                .FirstOrDefault();
                //Compare with PageId in PagePermission
                var pagePermission = permission.PagePermissionModel.Where(p => p.PageId == pageId && p.FunctionId == Action.ToUpper()).FirstOrDefault();
                if (pagePermission != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}
