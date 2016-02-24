using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace HKTHMall.WebApi.Models.Result
{
    [DataContract]
    public class ResponseGetCapitalRecordDetails
    {
        /// <summary>
        ///账单Id
        /// </summary>
        [DataMember(Name = "recordId")]
        public int RecordId { get; set; }

        /// <summary>
        ///异动类型（1:充值,2:转账,3:消费,4提现,5:退款,6:营收,7省级代理消费收益,8:市级代理消费收益,9:区级代理消费收益,10:感恩[一级分销商]粉丝消费收益,11:感动[二级分销商]粉丝消费收益,12:感谢[三级分销商]粉丝消费收益),13:省级代理加盟收益,14:市级代理加盟收益,15:区级代理加盟收益,16:感恩[一级分销商]粉丝加盟收益,17:感动[二级分销商]粉丝加盟收益,18:感谢[三级分销商]粉丝加盟收益)
        /// </summary>
        [DataMember(Name = "addOrCutType")]
        public int AddOrCutType { get; set; }

        /// <summary>
        ///操作账号
        /// </summary>
        [DataMember(Name = "operationAccount")]
        public string OperationAccount { get; set; }

        /// <summary>
        ///操作金额
        /// </summary>
        [DataMember(Name = "operationAmount")]
        public decimal OperationAmount { get; set; }

        /// <summary>
        ///订单号
        /// </summary>
        [DataMember(Name = "orderNumber")]
        public string OrderNumber { get; set; }

        /// <summary>
        ///商品图片
        /// </summary>
        [DataMember(Name = "imageUrl")]
        public string ImageUrl { get; set; }

        /// <summary>
        ///创建时间
        /// </summary>
        [DataMember(Name = "createDt")]
        public long CreateDt { get; set; }

        /// <summary>
        ///流水号
        /// </summary>
        [DataMember(Name = "serialNumber")]
        public string SerialNumber { get; set; }

        /// <summary>
        ///商品名称
        /// </summary>
        [DataMember(Name = "shopName")]
        public string ShopName { get; set; }

        /// <summary>
        ///惠粉类型 (1感恩惠粉 2感动惠粉 3感谢惠粉)
        /// </summary>
        [DataMember(Name = "gsType")]
        public int GsType { get; set; }

        /// <summary>
        ///收益比率
        /// </summary>
        [DataMember(Name = "incomeRate")]
        public decimal IncomeRate { get; set; }

        /// <summary>
        ///支付方式
        /// </summary>
        [DataMember(Name = "paymentWay")]
        public string PaymentWay { get; set; }

        /// <summary>
        ///收益渠道
        /// </summary>
        [DataMember(Name = "incomeWay")]
        public string IncomeWay { get; set; }

        /// <summary>
        ///代理类型 1区级代理 2市级代理 3省级代理 4全球代理
        /// </summary>
        [DataMember(Name = "proxyType")]
        public int ProxyType { get; set; }
    }
}