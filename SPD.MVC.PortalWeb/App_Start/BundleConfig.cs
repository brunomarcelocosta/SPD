using System.Web;
using System.Web.Optimization;

namespace SPD.MVC.PortalWeb
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                     "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                      "~/Scripts/jquery.validate*",
                      "~/Scripts/jquery-{version}.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                      "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/bootstrap-select.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/sweetalert").Include(
                     "~/Scripts/sweetalert.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/signalr").Include(
                      "~/Scripts/jquery.signalR-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/site").Include(
                      "~/Scripts/sitebase.js",
                      "~/Scripts/site.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquerymask").Include(
                "~/Scripts/jquery.mask*",
                "~/Scripts/Mascaras.js"));

            //bundles.Add(new ScriptBundle("~/bundles/webcam").Include(
            //          "~/Scripts/webcam.js",
            //          "~/Scripts/webcam.min.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/Styles/bootstrap.css",
                      "~/Content/bootstrap-select.css",
                      "~/Content/Styles/site.css"));

            bundles.Add(new StyleBundle("~/Content/multileveldropdown").Include(
                      "~/Content/Styles/multileveldropdown.css"));

            //bundles.Add(new StyleBundle("~/Content/datatables").Include(
            //                 "~/Content/DataTables/css/"));
        }
    }
}
