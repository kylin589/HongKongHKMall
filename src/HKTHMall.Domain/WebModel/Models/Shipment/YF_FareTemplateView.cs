using System.Collections.Generic;
using FluentValidation.Attributes;

namespace HKTHMall.Domain.WebModel.Models.Shipment
{
    /// <summary>
    ///     主模板
    /// </summary>
    public class YF_FareTemplateView
    {
        public YF_FareTemplateView()
        {
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

        public int ComputeMode { get; set; }
        public int IsDefault { get; set; }

        /// <summary>
        ///价格区间1
        /// </summary>
        public decimal? Price1 { get; set; }

        /// <summary>
        ///价格区间2
        /// </summary>
        public decimal? Price2 { get; set; }

        /// <summary>
        ///价格区间3
        /// </summary>
        public decimal? Price3 { get; set; }

        /// <summary>
        ///价格区间4
        /// </summary>
        public decimal? Price4 { get; set; }

        /// <summary>
        ///价格区间5
        /// </summary>
        public decimal? Price5 { get; set; }

        /// <summary>
        ///     价格区间6
        /// </summary>
        public decimal? Price6 { get; set; }

        /// <summary>
        ///价格区间7
        /// </summary>
        public decimal? Price7 { get; set; }


    }
}