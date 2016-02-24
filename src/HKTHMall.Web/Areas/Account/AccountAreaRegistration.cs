using System.Web.Mvc;

namespace HKTHMall.Web.Areas.Account
{
    public class AccountAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Account";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {

            context.MapRoute("orderdetail", "order/detail.html", new { controller = "my", action = "orderdetail" });
            context.MapRoute("orderlist", "order/list.html", new { controller = "my", action = "order" });
            context.MapRoute("refund", "order/refund.html", new { controller = "my", action = "RefundProcess" });
            context.MapRoute("refundprocess", "order/refundprocess.html", new { controller = "my", action = "applyrefund" });
            context.MapRoute("tradecomment", "trade/comment.html", new { controller = "my", action = "tradeComment" });
            context.MapRoute("tradecomments", "trade/comments.html", new { controller = "my", action = "tradeComments" });
            

            context.MapRoute(
                "Account_default",
                "Account/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}