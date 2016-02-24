using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace HKTHMall.WebApi.Models.Result
{
    [DataContract]
    public class ResponseGetUserBalance
    {
        /// <summary>
        /// 我的余额
        /// </summary>
        [DataMember(Name = "balance")]
        public decimal Balance { get; set; }
        /// <summary>
        /// 销售收益
        /// </summary>
        [DataMember(Name = "sellIncome")]
        public decimal SellIncome { get; set; }
        /// <summary>
        /// 财富界面提示
        /// </summary>
        [DataMember(Name = "remark")]
        public List<Prompt> Remark { get; set; }
    }
    [DataContract]
    public class Prompt
    {

        [DataMember(Name = "content")]
        public string Content { get; set; }
    }
}