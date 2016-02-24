using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace HKTHMall.WebApi.Models.Result
{
    [DataContract]
    public class TotalPostagePriceResult
    {
        [DataMember(Name = "comInfos")]
        public List<PostagePrice> comInfos { get; set; }

        [DataMember(Name = "totalExpressMoney")]
        public decimal TotalExpressMoney { get; set; }

    }
    [DataContract]
    public class PostagePrice
    {
        /// <summary>商家Id.</summary>          
        [DataMember(Name = "comId")]
        public String ComId { get; set; }

        /// <summary>
        /// 运费
        /// </summary>
        [DataMember(Name = "expressMoney")]
        public decimal ExpressMoney { get; set; }
    }

}