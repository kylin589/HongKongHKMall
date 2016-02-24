namespace HKTHMall.Domain.AdminModel.Models.Shipment
{
    public class AddShipmentTemplateModel
    {
        /// <summary>
        ///     区域Id
        /// </summary>
        public long THAreaID { get; set; }

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
        ///     超过重量每kg价格
        /// </summary>
        public decimal? Price5 { get; set; }

        /// <summary>
        ///     城市Ids
        /// </summary>
        public string CityIds { get; set; }

        /// <summary>
        ///     运费模板Id
        /// </summary>
        public int FareTemplateID { get; set; }
    }
}