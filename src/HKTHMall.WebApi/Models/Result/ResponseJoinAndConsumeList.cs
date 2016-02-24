using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace HKTHMall.WebApi.Models.Result
{
    public class ResponseJoinAndConsumeList
    {
        /// <summary>
        /// 类型（1.感恩惠粉,2.感动惠粉,3.感谢惠粉,4其他惠粉）
        /// </summary>
        [DataMember(Name = "gtype")]
        public int gtype { get; set; }
        /// <summary>
        /// 总金额
        /// </summary>
        [DataMember(Name = "totalAmount")]
        public decimal totalAmount { get; set; }
        /// <summary>
        /// 感恩惠粉金额
        /// </summary>
        [DataMember(Name = "gnTotal")]
        public decimal gnTotal { get; set; }
        /// <summary>
        /// 感动惠粉金额
        /// </summary>
        [DataMember(Name = "gdTotal")]
        public decimal gdTotal { get; set; }
        /// <summary>
        /// 感谢惠粉金额
        /// </summary>
        [DataMember(Name = "gxTotal")]
        public decimal gxTotal { get; set; }
        /// <summary>
        /// 外围惠粉金额
        /// </summary>
        [DataMember(Name = "periphery")]
        public decimal periphery { get; set; }
        /// <summary>
        /// data集合
        /// </summary>
        [DataMember(Name = "data")]
        public ExtInfo[] data { get; set; }
        public class ExtInfo
        {
            /// <summary>
            /// 用户Id
            /// </summary>
            [DataMember(Name = "userId")]
            public long userId { get; set; }
            /// <summary>
            /// 昵称
            /// </summary>
            [DataMember(Name = "nickName")]
            public string nickName { get; set; }
            /// <summary>
            /// 惠信号
            /// </summary>
            [DataMember(Name = "jcPerson")]
            public string jcPerson { get; set; }
            /// <summary>
            /// 金额
            /// </summary>
            [DataMember(Name = "jcAmount")]
            public string jcAmount { get; set; }
            /// <summary>
            /// 消费时间(最新) 时间戳
            /// </summary>
            [DataMember(Name = "jsDT")]
            public long jsDT { get; set; }
            /// <summary>
            /// 用户头像
            /// </summary>
            [DataMember(Name = "imageUrl")]
            public string imageUrl { get; set; }
        }

    }
}