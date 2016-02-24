using System.Collections.Generic;
using FluentValidation.Attributes;
using HKTHMall.Domain.AdminModel.Validators.Shipment;
using System;

namespace HKTHMall.Domain.AdminModel.Models.Shipment
{
    /// <summary>
    ///     主模板
    /// </summary>
    //[Validator(typeof(ShipmentValidator))]
    public class YF_FareTemplateModel
    {
        public YF_FareTemplateModel()
        {
            this.YF_FareTemplateAreaCountryModels = new List<YF_FareTemplateAreaCountryModel>();
        }

        /// <summary>
        ///     模板Id
        /// </summary>
        public int FareTemplateID { get; set; }

        /// <summary>
        ///     商家Id
        /// </summary>
        public long MerchantID { get; set; }

        /// <summary>
        ///     商家名称
        /// </summary>
        public string MerchantName { get; set; }

        /// <summary>
        ///     模板名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        ///  多少天内发货
        /// </summary>
        public int ShipmentDate { get; set; }
        /// <summary>
        /// 计算方式
        /// </summary>
        public int ComputeMode { get; set; }
        /// <summary>
        /// 是否默认
        /// </summary>
        public int IsDefault { get; set; }
        /// <summary>
        /// 区域ID
        /// </summary>
        public int THAreaID { get; set; }
        /// <summary>
        /// 区域级别
        /// </summary>
        public int AreaType { get; set; }
        /// <summary>
        /// 是否设置不满价格 0.否 1.是
        /// </summary>
        public int InsufficientType { get; set; }
        /// <summary>
        ///不满价格(超过包邮)
        /// </summary>
        public decimal? InsufficientPrice { get; set; }
        /// <summary>
        /// 几件内  几KG内
        /// </summary>
        public int Number1 { get; set; }

        /// <summary>
        ///  几件内  几KG内 价格
        /// </summary>
        public decimal? Price1 { get; set; }

        /// <summary>
        /// 每几件，每几KG
        /// </summary>
        public decimal? Number2 { get; set; }
        /// <summary>
        /// 每几件，每几KG价格
        /// </summary>
        public decimal? Price2 { get; set; }

        /// <summary>
        ///     价格区间3
        /// </summary>
        public decimal? Price3 { get; set; }

        /// <summary>
        ///  价格区间4
        /// </summary>
        public decimal? Price4 { get; set; }

        /// <summary>
        /// 价格区间5
        /// zhoub 20150930
        /// </summary>
        public decimal? Price5 { get; set; }

        /// <summary>
        /// 价格区间6
        /// zhoub 20150930
        /// </summary>
        public decimal? Price6 { get; set; }


        /// <summary>
        ///超过重量每kg价格
        /// </summary>
        public decimal? Price7 { get; set; }

 

       
        /// <summary>
        ///     运费模板
        /// </summary>
        public IList<YF_FareTemplateAreaCountryModel> YF_FareTemplateAreaCountryModels { get; set; }
    }
}