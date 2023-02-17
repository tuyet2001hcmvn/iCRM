using System.Web;
using System.Web.Optimization;

namespace ISD.Admin
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jqwidgets/jqxcore.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            // Lib Script
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/underscore-min.js",
                      "~/Scripts/ScriptExtensions.js"
                      ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/font-awesome.css",
                      "~/Scripts/Ionicons/css/ionicons.min.css",
                      "~/Scripts/AdminLTE-2.4.3/build/less/AdminLTE.css",
                      //"~/Scripts/AdminLTE-2.4.3/build/less/skins/_all-skins.css"
                      "~/Content/skin-strong-blue.css"
                      ));
            bundles.Add(new StyleBundle("~/Content/custom").Include(
                      "~/Content/site.css",
                      "~/Content/site-component.css"));
            //Common
            bundles.Add(new ScriptBundle("~/scripts/common/js").Include(
             "~/Scripts/Common/ISD.Common.js"
             //,"~/Scripts/Common/ISD.Print.js"
             //,"~/Scripts/Common/ISD.Select2Custom.js"
             ));
            //Sử dụng trong trang layout
            bundles.Add(new ScriptBundle("~/scripts/common/shared").Include(
             "~/Scripts/Common/ISD.Shared.js"
             ));

            //Script Customer/Profile
            bundles.Add(new ScriptBundle("~/areas/customer/scripts/js").Include(
               "~/Areas/Customer/Scripts/Profile_Edit.js",
               //"~/Areas/Customer/Scripts/ProfileGroup.js",
               "~/Areas/Customer/Scripts/AddressBook.js",
               "~/Areas/Customer/Scripts/CertificateAC.js",
               "~/Areas/Customer/Scripts/Spons.js",
               "~/Areas/Customer/Scripts/ProductPromotion.js",
               "~/Areas/Customer/Scripts/Appointment.js",
               "~/Areas/Customer/Scripts/PersonInCharge.js",
               "~/Areas/Customer/Scripts/RoleInCharge.js",
               //"~/Areas/Customer/Scripts/Partner.js",
               "~/Areas/Customer/Scripts/FileAttachment.js",
               "~/Areas/Customer/Scripts/ProfileContact.js",
               "~/Areas/Customer/Scripts/Investor.js",
               "~/Areas/Customer/Scripts/Design.js",
               "~/Areas/Customer/Scripts/Contractor.js",
               "~/Areas/Customer/Scripts/Construction.js",
               "~/Areas/Customer/Scripts/Activities.js",
               "~/Areas/Customer/Scripts/Competitor.js",
               "~/Areas/Customer/Scripts/Consulting.js"
               ));

            //Script Task
            bundles.Add(new ScriptBundle("~/Areas/Customer/Scripts/Profile_Create").Include(
               "~/Areas/Customer/Scripts/Profile_Create.js",
               "~/Areas/Customer/Scripts/ProfileContact.js"
               ));

            //Script Task
            bundles.Add(new ScriptBundle("~/Areas/Work/Scripts/CreateTask").Include(
               "~/Areas/Work/Scripts/CreateTask.js"
               ));
            bundles.Add(new ScriptBundle("~/Areas/Work/Scripts/EditTask").Include(
               "~/Areas/Work/Scripts/EditTask.js"
               ));
#if !DEBUG
            BundleTable.EnableOptimizations = true;
#endif
        }
    }
}
