using System.Web.Mvc;
using ISD.Core;

namespace ISD.Admin.Controllers
{
    public class AppController : BaseController
    {
        public ActionResult Index()
        {
            return Redirect("itms-services://?action=download-manifest&amp;url=https://giahoamobile.citek.vn/app.plist");
        }

    }
}