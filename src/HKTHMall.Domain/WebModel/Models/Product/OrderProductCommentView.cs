using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.WebModel.Models.Product
{
    public class OrderProductCommentView
    {
        public string OrderId { get; set; }

        public long ProductId { get; set; }

        public string ProductName { get; set; }

        public string PicUrl { get; set; }

        public int Iscomment { get; set; }

        public string CommentContent { get; set; }

        public DateTime CommentDT { get; set; }

        public int CommentLevel { get; set; }

        public int ProductCommentId { get; set; }

        public long MerchantID { get; set; }

        public string ShopName { get; set; }

        public string SkuName { get; set; }

        public long SKU_ProducId { get; set; }

        /// <summary>
        /// add by liujc 印象标签
        /// </summary>
        public string LablesStr { get; set; }

        public List<CommentLablesList> LablesList
        {
            get
            {
                return CommentLablesList.ReturnList(LablesStr);
            }
        }
    }
}
