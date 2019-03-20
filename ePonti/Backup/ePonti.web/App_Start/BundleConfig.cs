using System.Web;
using System.Web.Optimization;

namespace ePonti.web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            BundleTable.EnableOptimizations = true;

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js", "~/Scripts/jquery-ui.min.js"));

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
                      "~/Content/jquery-ui.min.css",
                      "~/Content/site.css"));

            bundles.Add(new ScriptBundle(Bundles.Js.ePontiV9Prototype).Include("~/Scripts/ePontiV9Prototype.js"));
            bundles.Add(new ScriptBundle(Bundles.Js.CommonOptions).Include("~/Scripts/pages/options.js"));

#if DEBUG
            foreach (var bundle in BundleTable.Bundles)
            {
                //Clear it, so that bundles are not minified. Should be remove when moving to production
                bundle.Transforms.Clear();
            }
#endif
        }
    }

    public static class Bundles
    {
        public static class Js
        {
            public const string ePontiV9Prototype = "~/bundles/ePontiV9Prototype_js";
            public const string CommonOptions = "~/bundles/commonoptions_js";
        }

        public static class Css
        {
            public const string css = "~/bundles/css_css";
        }
    }
}
