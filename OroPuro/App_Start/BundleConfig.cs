using System.Web;
using System.Web.Optimization;

namespace OroPuro
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-2.2.3.min.js",
                        "~/Scripts/jquery-ui.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/panelcss").Include(
                      "~/Content/AdminLTE.min.css",
                      "~/Content/bootstrap.css",
                      "~/Content/skins/_all-skins.min.css",
                      "~/Content/alt/AdminLTE-bootstrap-social.css",
                      "~/Content/alt/AdminLTE-fullcalendar.css",
                      "~/Content/alt/AdminLTE-select2.css",
                      "~/Content/alt/slider.css",
                      "~/Content/bootstrap.css"));

            bundles.Add(new ScriptBundle("~/bundles/paneljs").Include(
                      "~/Scripts/bootstrap-slider.js",
                      "~/Scripts/Chart.min.js",
                      "~/Scripts/lodash.js",
                      "~/Scripts/app.js"));
        }
    }
}
