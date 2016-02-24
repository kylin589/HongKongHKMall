using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Schema;

namespace HKTHMall.WebApi.Models.Request
{
    public class RequestGoodsInfoModel
    {
        public RequestGoodsInfoModel()
        {
            this.ProductInfos = new List<ProductInfo>();
            this.SkuNumberInfos = new List<SkuNumberInfo>();
        }

        /// <summary>
        ///     商品Ids
        /// </summary>
        public List<string> StrGoodsIds
        {
            get { return this.ProductInfos.Select(m => m.ProductId).ToList(); }
            set
            {
                value.ForEach(m =>
                {
                    this.ProductInfos.Add(new ProductInfo
                    {
                        ProductId = m
                    });
                });
            }
        }

        /// <summary>
        ///     sku码s
        /// </summary>
        public List<string> StrSkuNumber
        {
            get { return this.SkuNumberInfos.Select(m => m.SkuNumber).ToList(); }
            set
            {
                value.ForEach(m =>
                {
                    this.SkuNumberInfos.Add(new SkuNumberInfo
                    {
                        SkuNumber = m
                    });
                });
            }
        }

        /// <summary>
        /// 商品数量
        /// </summary>
        public List<int> StrCounts
        {
            get { return this.CountInfos.Select(m => m.Count).ToList(); }
            set
            {
                value.ForEach(m =>
                {
                    this.CountInfos.Add(new CountInfo()
                    {
                        Count = m
                    });
                });
            }
        }

        /// <summary>
        ///     商品信息
        /// </summary>
        public List<ProductInfo> ProductInfos { get; set; }

        /// <summary>
        ///     SkuNumberIds
        /// </summary>
        public List<SkuNumberInfo> SkuNumberInfos { get; set; }

        /// <summary>
        /// 商品数量
        /// </summary>
        public List<CountInfo> CountInfos { get; set; }

        /// <summary>
        ///     用户Id
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        ///     语言
        /// </summary>
        public int Lang { get; set; }

        /// <summary>
        /// 用户收货地址Id
        /// </summary>
        public long? UserAddressId { get; set; }

        public class ProductInfo
        {
            /// <summary>
            ///     商品Id
            /// </summary>
            public string ProductId { get; set; }
        }

        public class SkuNumberInfo
        {
            /// <summary>
            ///     SkuId
            /// </summary>
            public string SkuNumber { get; set; }
        }

        /// <summary>
        /// 商品数量
        /// </summary>
        public class CountInfo
        {
            /// <summary>
            /// 商品数量
            /// </summary>
            public int Count { get; set; }
        }
    }
}