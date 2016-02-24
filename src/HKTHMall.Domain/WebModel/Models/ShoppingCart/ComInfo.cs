using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.WebModel.Models.ShoppingCart
{
    /// <summary>商家,购物车用</summary>
    /// <remarks></remarks>
    /// <author>PanYun HX1501345 2015-04-20 09:53:16</author>
    [DataContract]
    public class ComInfo
    {
        /// <summary>商家Id.</summary>
        /// <remarks></remarks>
        /// <author>PanYun HX1501345 2015-04-20 13:46:52</author>
        /// <value>The COM identifier.</value>
        [DataMember(Name = "comId")]
        public String ComId { get; set; }

        /// <summary>商家名称</summary>
        /// <remarks></remarks>
        /// <author>PanYun HX1501345 2015-04-20 13:46:55</author>
        /// <value>The name of the COM.</value>
        [DataMember(Name = "comName")]
        public String ComName { get; set; }

        /// <summary>
        /// 商品集合
        /// </summary>
        [DataMember(Name = "goods")]
        public List<GoodsInfoModel> Goods { get; set; }

        /// <summary>
        /// 运费
        /// </summary>
        [DataMember(Name = "expressMoney")]
        public decimal ExpressMoney { get; set; }
    }
}
