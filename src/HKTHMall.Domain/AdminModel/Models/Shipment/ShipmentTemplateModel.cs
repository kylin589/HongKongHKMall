using FluentValidation.Attributes;
using HKTHMall.Domain.AdminModel.Validators.Shipment;

namespace HKTHMall.Domain.AdminModel.Models.Shipment
{
    /// <summary>
    ///     区域运费模板
    /// </summary>
    [Validator(typeof(ShipmentTemplateValidator))]
    public class ShipmentTemplateModel
    {
        /// <summary>
        ///     区域运费模板Id
        /// </summary>
        public int ShipmentTemplateId { get; set; }

        /// <summary>
        ///     区域Id
        /// </summary>
        public long THAreaID { get; set; }

        /// <summary>
        ///     区域代码
        /// </summary>
        public string AreaCode { get; set; }

        /// <summary>
        ///     价格区间1
        /// </summary>
        public decimal? Price1 { get; set; }

        /// <summary>
        ///     价格区间2
        /// </summary>
        public decimal? Price2 { get; set; }

        /// <summary>
        ///     价格区间3
        /// </summary>
        public decimal? Price3 { get; set; }

        /// <summary>
        ///     价格区间4
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
        ///     超过重量每kg价格
        /// </summary>
        public decimal? Price7 { get; set; }

        /// <summary>
        ///     城市Ids
        /// </summary>
        public string CityIds { get; set; }

        /// <summary>
        ///     城市名称
        /// </summary>
        public string CityNames { get; set; }

        /// <summary>
        ///     运费模板Id
        /// </summary>
        public int FareTemplateID { get; set; }

        /// <summary>
        ///不满价格
        /// </summary>
        public decimal? InsufficientPrice { get; set; }
    }
}