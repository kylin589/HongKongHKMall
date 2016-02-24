using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace HKTHMall.WebApi.Models.Result
{
    [DataContract]
    public class ResponseGetCapRecordList
    {
        /// <summary>
        ///账单Id
        /// </summary>
        [DataMember(Name = "recordId")]
        public int RecordId { get; set; }
        /// <summary>
        ///订单编号(流水号)
        /// </summary>
        [DataMember(Name = "orderNumber")]
        public string OrderNumber { get; set; }

        /// <summary>
        ///资金异动类型（1:充值 2:提现 3:购物  4:惠粉加盟收益 5:惠粉消费收益 6: 惠粉月度分红收益 7: 营收 8:团购 9:手机话费充值 10:惠卡买单  11:后台充值 12:订单结算货款 13:退款 14:返现)
        /// </summary>
        [DataMember(Name = "addOrCutType")]
        public int AddOrCutType { get; set; }

        /// <summary>
        /// 自己详细备注
        /// </summary>
        [DataMember(Name = "remark")]
        public string Remark { get; set; }

        /// <summary>
        ///异动金额
        /// </summary>
        [DataMember(Name = "addOrCutAmount")]
        public decimal AddOrCutAmount { get; set; }

        /// <summary>
        ///增或减 1 增 0减
        /// </summary>
        [DataMember(Name = "isAddOrCut")]
        public int IsAddOrCut { get; set; }

        /// <summary>
        ///商品名称
        /// </summary>
        [DataMember(Name = "shopName")]
        public string ShopName { get; set; }

        /// <summary>
        /// 创建时间(时间戳)
        /// </summary>
        [DataMember(Name = "createDt")]
        public long CreateDt { get; set; }

        /// <summary>
        ///资金异动类型返回结果 1:成功 2:失败
        /// </summary>
        [DataMember(Name = "addOrCutResult")]
        public int AddOrCutResult { get; set; }

    }
}