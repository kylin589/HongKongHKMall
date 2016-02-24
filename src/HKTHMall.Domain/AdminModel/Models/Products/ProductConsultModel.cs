using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.AdminModel
{
    /// <summary>
    /// 产品咨询
    /// zhoub 20150827
    /// </summary>
    public class ProductConsultModel
    {
        public long ProductConsultId { get; set; }

        public long ProductId { get; set; }

        public long UserID { get; set; }
        public string ConsultContent { get; set; }

        public DateTime ConsultDT { get; set; }
        public int IsAnonymous { get; set; }
        public int CheckStatus { get; set; }
        public string CheckBy { get; set; }
        public DateTime CheckDT { get; set; }
        public string ReplyBy { get; set; }
        public DateTime? ReplyDT { get; set; }
        public string ReplyContent { get; set; }

        public string ProductName { get;set; }
        public string Account { get; set; }
        public string Phone { get; set; }
        public string NickName { get; set; }
        /// <summary>
        /// 咨询类别.add by liujc
        /// </summary>
        public int ConsultType { get; set; }
        public int IsGood { get; set; }
        public int NotGood { get; set; }
    }

    public class ProductConsult 
    {
        public long ProductConsultId { get; set; }
        public long ProductId { get; set; }
        public long UserID { get; set; }
        public string ConsultContent { get; set; }
        public DateTime ConsultDT { get; set; }
        public int IsAnonymous { get; set; }
        public int CheckStatus { get; set; }
        public string CheckBy { get; set; }
        public Nullable<System.DateTime> CheckDT { get; set; }
        public string ReplyBy { get; set; }
        public Nullable<System.DateTime> ReplyDT { get; set; }
        public string ReplyContent { get; set; }
        /// <summary>
        /// 咨询类别.add by liujc
        /// </summary>
        public int ConsultType { get; set; }

        public int IsGood { get; set; }
        public int NotGood { get; set; }
    }

    /// <summary>
    /// 用户点赞,add by liujc
    /// </summary>
    public class UserConsult
    {
        /// <summary>
        /// 标识列
        /// </summary>
        public long ID { get; set; }
        /// <summary>
        /// 咨询ID
        /// </summary>
        public long ConsultID { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public long UserID { get; set; }
        /// <summary>
        /// 1:满意 -1：不满意
        /// </summary>
        public int IsGood { get; set; }
    }

    public class SearchModelBase
    {
        public SearchModelBase()
        {          
            this.PageSize = 5; //默认每页5
        }
        private int pageIndex = 1;
        public int Page
        {
            get { return pageIndex <= 0 ? 1 : pageIndex; }
            set { pageIndex = value <= 0 ? 1 : value; }
        }
        private int pageSize = 1;
        public int PageSize
        {
            get
            {
                return pageSize <= 0 ? 1 : pageSize;
            }
            set { pageSize = value <= 0 ? 1 : value; }
        }
        public int AllCount { get; set; }
    }


    public class SearchConsle : SearchModelBase
    {
        public SearchConsle()
        { }

        public SearchConsle(int _contype,int count)
        {
            contype = _contype;
            AllCount = count;
        }

        public int languageID { get; set; }
        public long? productId { get; set; }
        /// <summary>
        /// 评价类别。add by liujc
        /// </summary>
        public int contype { get; set; }
    }

    /// <summary>
    /// add by liujc
    /// </summary>
    public class GroupModel
    {
        public int contype { get; set; }
        public int AllCount { get; set; }
    }
}
