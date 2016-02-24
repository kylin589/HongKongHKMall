using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Autofac;
using AutoMapper;
using BrCms.Framework.Infrastructure;
using HKTHMall.Core.Security;
using HKTHMall.Domain.Enum;
using HKTHMall.Domain.WebModel.Models.Orders;

namespace HKTHMall.WebApi.Models.Request
{
    [DataContract]
    public class RequestOrderInfoViewModel
    {
        public RequestOrderInfoViewModel()
        {
            Mapper.CreateMap<RequestOrderInfoViewModel, AddOrderInfoView>()
                .ForMember(d => d.UserId, o =>
                {
                    o.MapFrom(m =>
                        Convert.ToInt64(BrEngineContext.Current.Resolve<IEncryptionService>().RSADecrypt(m.UserId))
                        );
                })
                .ForMember(d => d.LanguageId, o =>
                {
                    o.MapFrom(m => m.lang);
                });
            Mapper.CreateMap<RequestOrderInfoViewModel.MerchantView, AddOrderInfoView.MerchantView>();
            Mapper.CreateMap<RequestOrderInfoViewModel.GoodsView, AddOrderInfoView.GoodsView>();

            this.lang = 1;
        }

        /// <summary>
        ///     收货地址
        /// </summary>
        [DataMember]
        public long ReceiverAddressId { get; set; }

        /// <summary>
        ///     支付类型
        /// </summary>
        [DataMember]
        public int PayType { get; set; }

        /// <summary>
        ///     支付单类型
        /// </summary>
        [DataMember]
        public int PaidType { get; set; }

        /// <summary>
        ///     语言代码
        /// </summary>
        [DataMember]
        public int lang { get; set; }

        /// <summary>
        ///     用户Id
        /// </summary>
        [DataMember]
        public string UserId { get; set; }

        /// <summary>
        ///     订单来源
        /// </summary>
        [DataMember]
        public int OrderSource { get; set; }

        /// <summary>
        ///     支付通道
        /// </summary>
        [DataMember]
        public int PayChannel { get; set; }

        /// <summary>
        ///     购买类型
        /// </summary>
        [DataMember]
        public int PurchaseType { get; set; }

        /// <summary>
        ///     支付单号
        /// </summary>
        [DataMember]
        public string PaymentOrderId { get; set; }

        [DataMember]
        public List<MerchantView> MerchantViews { get; set; }

        [DataContract]
        public class MerchantView
        {
            [DataMember]
            public string MerchantID { get; set; }

            [DataMember]
            public string Remark { get; set; }

            [DataMember]
            public List<GoodsView> Goods { get; set; }
        }

        [DataContract]
        public class GoodsView
        {
            [DataMember]
            public string ProductID { get; set; }

            [DataMember]
            public string SkuNumber { get; set; }

            [DataMember]
            public string ProductNumber { get; set; }
        }


        public T To<T>() where T : class
        {
            return Mapper.Map<RequestOrderInfoViewModel, T>(this);
        }
    }
}