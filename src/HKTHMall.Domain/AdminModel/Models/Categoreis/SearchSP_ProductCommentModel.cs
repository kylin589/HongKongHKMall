using BrCms.Framework.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.Models.Categoreis
{
    public class SearchSP_ProductCommentModel : Paged
    {
        public long ProductCommentId { get; set; }
        /// <summary>
        /// 商品ID
        /// </summary>
        public Nullable<long> ProductId { get; set; }

        /// <summary>
        /// 订单ID
        /// </summary>
        public long OrderId { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public long UserID { get; set; }

        /// <summary>
        /// 开始评论时间
        /// </summary>
        public Nullable<System.DateTime> BeginCommentDT { get; set; }

        /// <summary>
        /// 结束评论时间
        /// </summary>
        public Nullable<System.DateTime> EndCommentDT { get; set; }

        /// <summary>
        /// 审核状态(0:全部,1:待审；2:通过；3:拒绝）
        /// </summary>
        public byte CheckStatus { get; set; }

        /// <summary>
        /// 审核人
        /// </summary>
        public string CheckBy { get; set; }

        /// <summary>
        /// 开始审核时间
        /// </summary>
        public Nullable<System.DateTime> BeginCheckDT { get; set; }

        /// <summary>
        /// 结束审核时间
        /// </summary>
        public Nullable<System.DateTime> EndCheckDT { get; set; }

        /// <summary>
        /// 多语言版本 1中文,2英语,3泰语（默认）
        /// </summary>
        public int LanguageID { get; set; }

        /// <summary>
        /// 评价等级（0全部,1好评,2中评,3差评）
        /// </summary>
        public int CommentLevel { get; set; }

        /// <summary>
        /// 1：好评；2：中评；3：差评  
        /// <remarks>added by jimmy,2015-9-11</remarks>
        /// </summary>
        public int? typeLevel { get; set; }

        /// <summary>
        /// 用户手机号（用户表）
        /// </summary>
        
        public string Phone { get; set; }

            /// <summary>
        /// 用户Email（用户表）
        /// </summary>

        public string Email { get; set; }
    }
}
