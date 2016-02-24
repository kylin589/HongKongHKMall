namespace HKTHMall.Domain.WebModel.Models.Product
{
    /// <summary>
    /// 订单评论
    /// </summary>
    public class SearchOrderProductCommentView
    {

        public SearchOrderProductCommentView()
        {
            this.pageSize = 10;
            this.page = 1;
        }

        public string OrderID { get; set; }
        public long? UserID { get; set; }
        public int? OrderStatus { get; set; }
        public int? LanguageID { get; set; }

        public int? Iscomment { get; set; }

        private int _page;

        public int page
        {
            get { return _page; }
            set
            {

                _page = value == 0 ? 1 : value;
            }
        }

        public int pageSize { get; set; }


    }
}
