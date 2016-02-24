using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace HKTHMall.WebApi.Models.Result.Order
{
    public class GetCartNumResult
    {
        public class GetCartNum : ExResult
        {
            /// <summary>
            ///返回结果
            /// </summary>
            [DataMember(Name = "rs")]
            public ResultM Rs { set; get; }
        }

        [DataContract]
        /// <summary>
        /// 返回结果
        /// </summary>
        public class ResultM
        {
            /// <summary>
            /// 数量
            /// </summary>
            [DataMember(Name = "number")]
            public int Number { get; set; }
        }
    }
}