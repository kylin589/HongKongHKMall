using HKTHMall.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.Models.Categoreis
{
    [DataContract]
    public class SP_ProductCommentModel : IResult
    {
        /// <summary>
        /// 评论ID
        /// </summary>
        //[DataMember]
        public long ProductCommentId { get; set; }

        /// <summary>
        /// 商品ID
        /// </summary>
        //[DataMember]
        public Nullable<long> ProductId { get; set; }

        /// <summary>
        /// 订单ID
        /// </summary>
        //[DataMember]
        public long OrderId { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        [DataMember]
        public long UserID { get; set; }

        /// <summary>
        /// 评论星级
        /// </summary>
        [DataMember]
        public byte CommentLevel { get; set; }

        /// <summary>
        /// 评论内容
        /// </summary>
        [DataMember]
        public string CommentContent { get; set; }

        /// <summary>
        /// 评论时间
        /// </summary>
        [DataMember]
        public Nullable<System.DateTime> CommentDT { get; set; }

        /// <summary>
        /// 是否匿名
        /// </summary>
        [DataMember]
        public Nullable<byte> IsAnonymous { get; set; }

        /// <summary>
        /// 审核状态(1:待审；2:通过；3:拒绝）
        /// </summary>
        //[DataMember]
        public byte CheckStatus { get; set; }

        /// <summary>
        /// 审核人
        /// </summary>
        //[DataMember]
        public string CheckBy { get; set; }

        /// <summary>
        /// 审核时间
        /// </summary>
        //[DataMember]
        public Nullable<System.DateTime> CheckDT { get; set; }

        /// <summary>
        /// 回复人
        /// </summary>
        //[DataMember]
        public string ReplyBy { get; set; }

        /// <summary>
        /// 回复时间
        /// </summary>
        //[DataMember]
        public Nullable<System.DateTime> ReplyDT { get; set; }

        /// <summary>
        /// 回复内容
        /// </summary>
        //[DataMember]
        public string ReplyContent { get; set; }

        /// <summary>
        /// 商品model
        /// </summary>
        //[DataMember]
        public virtual Product Product { get; set; }

        /// <summary>
        /// 用户名（用户表）
        /// </summary>
        //[DataMember]
        public string Account { get; set; }

        /// <summary>
        /// 商品名称（Product_Lang商品多语言扩展表）
        /// </summary>
        //[DataMember]
        public string ProductName { get; set; }

        /// <summary>
        /// 用户手机号（用户表）
        /// </summary>
        [DataMember]
        public string Phone { get; set; }

        /// <summary>
        /// 用户Email（用户表）
        /// </summary>

        public string Email { get; set; }

        /// <summary>
        /// 用户手昵称（用户表）
        /// </summary>
        [DataMember]
        public string NickName { get; set; }

            /// <summary>
        /// 用户头像（用户表）
        /// </summary>
        [DataMember]
        public string HeadImageUrl { get; set; }

        //[DataMember]
        public long SKU_ProducId { get; set; }

        public string LablesStr { get; set; }

    }

    
}
