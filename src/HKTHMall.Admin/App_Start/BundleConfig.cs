using System.Web;
using System.Web.Optimization;

namespace HKTHMall.Admin
{
    public class BundleConfig
    {
        // 有关绑定的详细信息,请访问 http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Content/js/jquery-1.11.3.min.js"));

            //bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
            //            "~/Scripts/jquery.validate*"));

            // 使用要用于开发和学习的 Modernizr 的开发版本。然后,当你做好
            // 生产准备时,请使用 http://modernizr.com 上的生成工具来仅选择所需的测试。
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Content/js/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Content/bootstrap-3.3.5-dist/js/bootstrap.js", "~/Content/js/bootstrap-table.min.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap-3.3.5-dist/css/bootstrap.css",
                      "~/Content/css/site.css"));
        }
    }
}
