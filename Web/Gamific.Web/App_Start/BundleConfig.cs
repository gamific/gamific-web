using System;
using System.Web;
using System.Web.Optimization;

namespace Vlast.Gamific.Web
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {

            string isMinify = Util.Parameter.ParameterCache.Get("MINIFY_COMPONENTES");

            if (true)
            {
                bundles.Add(new ScriptBundle("~/bundles/LayoutScripts").Include(
                            "~/Content/Js/lib/jquery-1.11.1.min.js",
                            "~/Content/Js/lib/bootstrap.min.js ",
                            "~/Content/Js/lib/jquery.dataTables.min.js",
                            "~/Content/Js/lib/dataTables.bootstrap.min.js",
                            "~/Content/Js/lib/dataTables.responsive.min.js",
                            "~/Content/Js/lib/bootstrap-dropdown-multilevel.min.js",
                            "~/Content/Js/lib/jquery.mmenu.min.js",
                            "~/Content/Js/lib/jquery.sparkline.min.js",
                            "~/Content/Js/lib/jquery.nicescroll.min.js",
                            "~/Content/Js/lib/jquery.animateNumber.min.js",
                            "~/Content/Js/lib/jquery.videobackground.min.js",
                            "~/Content/Js/lib/jquery.blockUI.min.js",
                            "~/Content/Js/lib/bootstrap-tabdrop.min.js",
                            "~/Content/Js/lib/chosen.jquery.min.js",
                            "~/Content/Js/lib/parsley.min.js",
                            "~/Content/Js/lib/jquery.bootstrap.wizard.min.js",
                            "~/Content/Js/lib/minimal.min.js",
                            "~/Content/Js/lib/vanilla-masker.min.js",
                            "~/Content/Js/lib/plugin.min.js",
                            "~/Content/Js/lib/custom-colored.min.js",
                            "~/Content/Js/lib/Treant.min.js",
                            "~/Content/Js/lib/jquery.flot.min.js",
                            "~/Content/Js/lib/jquery.flot.time.min.js",
                            "~/Content/Js/lib/jquery.flot.selection.min.js",
                            "~/Content/Js/lib/jquery.flot.animator.min.js",
                            "~/Content/Js/lib/jquery.flot.orderBars.min.js",
                            "~/Content/Js/lib/jquery.easypiechart.min.js",
                            "~/Content/Js/lib/raphael-min.js",
                            "~/Content/Js/lib/d3.v2.min.js",
                            "~/Content/Js/lib/rickshaw.min.js",
                            "~/Content/Js/lib/morris.min.js",
                            "~/Content/Js/lib/summernote.min.js",
                            "~/Content/Js/lib/jquery.unobtrusive-ajax.min.js",
                            "~/Content/Js/lib/tiny-mce.min.js",
                            "~/Content/Js/lib/moment.min.js",
                            "~/Content/Js/lib/jquery.animateNumbers.js",
                            "~/Content/Js/app/gamific.min.js",
                            "~/Content/Js/lib/progressbar.min.js",
                            "~/Content/Js/lib/bootstrap-datepicker.js",
                            "~/Content/Js/lib/bootstrap-dialog.min.js"
                            ));
            }
            else
            {
                bundles.Add(new ScriptBundle("~/bundles/LayoutScripts").Include(
                        "~/Content/Js/lib/jquery-1.11.1.min.js",
                        "~/Content/Js/lib/bootstrap.min.js ",
                        "~/Content/Js/lib/jquery.dataTables.js",
                        "~/Content/Js/lib/dataTables.bootstrap.js",
                        "~/Content/Js/lib/dataTables.responsive.min.js",
                        "~/Content/Js/lib/bootstrap-dropdown-multilevel.js",
                        "~/Content/Js/lib/jquery.mmenu.min.js",
                        "~/Content/Js/lib/jquery.sparkline.min.js",
                        "~/Content/Js/lib/jquery.nicescroll.min.js",
                        "~/Content/Js/lib/jquery.animateNumber.js",
                        "~/Content/Js/lib/jquery.videobackground.js",
                        "~/Content/Js/lib/jquery.blockUI.js",
                        "~/Content/Js/lib/bootstrap-tabdrop.min.js",
                        "~/Content/Js/lib/chosen.jquery.min.js",
                        "~/Content/Js/lib/parsley.min.js",
                        "~/Content/Js/lib/jquery.bootstrap.wizard.min.js",
                        "~/Content/Js/lib/minimal.min.js",
                        "~/Content/Js/lib/vanilla-masker.min.js",
                        "~/Content/Js/lib/bootstrap-datepicker.js",
                        "~/Content/Js/lib/plugin.js",
                        "~/Content/Js/lib/custom-colored.js",
                        "~/Content/Js/lib/Treant.js",
                        "~/Content/Js/lib/d3.v2.js",
                        "~/Content/Js/lib/jquery.easypiechart.min.js",
                        "~/Content/Js/lib/jquery.flot.animator.min.js",
                        "~/Content/Js/lib/jquery.flot.min.js",
                        "~/Content/Js/lib/jquery.flot.orderBars.js",
                        "~/Content/Js/lib/jquery.flot.selection.min.js",
                        "~/Content/Js/lib/jquery.flot.time.min.js",
                        "~/Content/Js/lib/morris.min.js",
                        "~/Content/Js/lib/raphael-min.js",
                        "~/Content/Js/lib/rickshaw.min.js",
                        "~/Content/Js/lib/summernote.min.js",
                        "~/Content/Js/lib/jquery.unobtrusive-ajax.js",
                        "~/Content/Js/lib/tiny-mce.js",
                        "~/Content/Js/lib/moment.js",
                        "~/Content/Js/app/gamific.js"
                        ));
            }
            bundles.Add(new StyleBundle("~/Content/LayoutStyle").Include(
                        "~/Content/css/bootstrap.min.css", new CssRewriteUrlTransform()).Include(
                        "~/Content/css/datepicker.css", new CssRewriteUrlTransform()).Include(
                        "~/Content/css/animate.min.css", new CssRewriteUrlTransform()).Include(
                        "~/Content/css/dataTables.responsive.css", new CssRewriteUrlTransform()).Include(
                        "~/Content/css/dataTables.bootstrap.css", new CssRewriteUrlTransform()).Include(
                        "~/Content/css/animate.css", new CssRewriteUrlTransform()).Include(
                        "~/Content/css/jquery.mmenu.all.css", new CssRewriteUrlTransform()).Include(
                        "~/Content/css/bootstrap-checkbox.css", new CssRewriteUrlTransform()).Include(
                        "~/Content/css/bootstrap-dropdown-multilevel.css", new CssRewriteUrlTransform()).Include(
                        "~/Content/css/chosen.min.css", new CssRewriteUrlTransform()).Include(
                        "~/Content/css/chosen-bootstrap.css", new CssRewriteUrlTransform()).Include(
                        "~/Content/css/tabdrop.css", new CssRewriteUrlTransform()).Include(
                        "~/Content/css/minimal.css", new CssRewriteUrlTransform()).Include(
                        "~/Content/css/Treant.css", new CssRewriteUrlTransform()).Include(
                        "~/Content/css/morris.css", new CssRewriteUrlTransform()).Include(
                        "~/Content/css/rickshaw.min.css", new CssRewriteUrlTransform()).Include(
                        "~/Content/css/gamific.css", new CssRewriteUrlTransform()).Include(
                        "~/Content/css/bootstrap-dialog.min.css", new CssRewriteUrlTransform()
                        ));

            bundles.Add(new StyleBundle("~/Content/EmptyLayoutStyle").Include(
                    "~/Content/css/bootstrap.min.css", new CssRewriteUrlTransform()).Include(
                    "~/Content/css/bootstrap-checkbox.css", new CssRewriteUrlTransform()).Include(
                    "~/Content/css/login-load.css", new CssRewriteUrlTransform()).Include(
                    "~/Content/css/bootstrap-dropdown-multilevel.css", new CssRewriteUrlTransform()).Include(
                    "~/Content/css/datepicker2.css", new CssRewriteUrlTransform()).Include(
                    "~/Content/css/minimal.css", new CssRewriteUrlTransform()
                    ));
        }
    }
}