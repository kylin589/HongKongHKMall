using System.Web;
using System.Web.Optimization;

namespace HKTHMall.Web
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            //bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
            //            "~/Content/js/jquery-1.11.3.min.js"));

            ////bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
            ////            "~/Scripts/jquery.validate*"));

            //// 使用要用于开发和学习的 Modernizr 的开发版本。然后,当你做好
            //// 生产准备时,请使用 http://modernizr.com 上的生成工具来仅选择所需的测试。
            //bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
            //            "~/Content/js/modernizr-*"));

            //bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
            //          "~/Content/bootstrap-3.3.5-dist/js/bootstrap.js", "~/Content/js/bootstrap-table.min.js"));

            //bundles.Add(new StyleBundle("~/Content/css").Include(
            //          "~/Content/bootstrap-3.3.5-dist/css/bootstrap.css",
            //          "~/Content/css/site.css"));

            bundles.Add(new StyleBundle("~/Content/css/zh-CN.css").Include("~/Content/newcss/css/css.css"));

            bundles.Add(new StyleBundle("~/Content/css/th-TH.css").Include("~/Content/newcss/css/css-th.css"));

            bundles.Add(new StyleBundle("~/Content/css/en-US.css").Include("~/Content/newcss/css/css-En.css"));

            bundles.Add(new StyleBundle("~/Content/css/zh-HK.css").Include("~/Content/newcss/css/css-HK.css"));

            //启用压缩 合并
           // BundleTable.EnableOptimizations = true;

        }
    }
}
