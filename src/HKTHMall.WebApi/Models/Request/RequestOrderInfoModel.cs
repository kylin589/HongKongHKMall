namespace HKTHMall.WebApi.Models.Request
{
    /// <summary>
    ///     获取订单详情
    /// </summary>
    public class RequestOrderInfoModel
    {
        public RequestOrderInfoModel()
        {
            this.lang = 1;
        }

        /// <summary>
        ///     用户Id
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 用户地址Id
        /// </summary>
        public long? UserAddressId { get; set; }

        /// <summary>
        ///     语言Id
        /// </summary>
        public int lang { get; set; }
    }
}