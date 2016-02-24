using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace HKTHMall.WebApi.Models.Result
{ 
     
    [DataContract]
    public class ResponseProductInfoModel : ApiResultModel
    {
        /// <summary>
        /// 结果
        /// </summary>
        [DataMember(Name = "rs")]
        public ResultProduct Results { get; set; }

    }
         

    [DataContract]
    public class ResultProduct
    {
        /// <summary>
        /// 商品ID
        /// </summary>
        [DataMember(Name = "productId")]
        public string ProductId { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        [DataMember(Name = "productname")]
        public string ProductName { get; set; }

        /// <summary>
        /// 商品副标题
        /// </summary>
        [DataMember(Name = "productSubName")]
        public string Subheading { get; set; }

        ///// <summary>
        ///// 商品详情描述
        ///// </summary>
        //[DataMember(Name = "introduction")]
        //public string Introduction { get; set; }

        /// <summary>
        /// 惠卡价7
        /// </summary>
        [DataMember(Name = "hkprice")]
        public decimal HkPrice { get; set; }

        /// <summary>
        /// 市场价
        /// </summary>
        [DataMember(Name = "marketprice")]
        public decimal MarketPrice { get; set; }

        /// <summary>
        /// 商家LOGOUrl(150*150)
        /// </summary>
        [DataMember(Name = "companyurl")]
        public string CompanyUrl { get; set; }
        /// <summary>
        /// 商家名称
        /// </summary>
        [DataMember(Name = "companyname")]
        public string CompanyName  { get; set; }

        /// <summary>
        /// 商家电话
        /// </summary>
        [DataMember(Name = "companytelephone")]
        public string CompanyTelephone { get; set; }

        /// <summary>
        /// 库存数量
        /// </summary>
        [DataMember(Name = "stockquantity")]
        public int StockQuanTity { get; set; }

        /// <summary>
        /// 商品状态(0-正常,1已下架)
        /// </summary>
        [DataMember(Name = "statustype")]
        public int StatusType { get; set; }

        /// <summary>
        ///  邮费  0包邮,大于0为邮费
        /// </summary>
        [DataMember(Name = "ispostage")]
        public decimal IsPostage { get; set; }

        /// <summary>
        ///  销售数量
        /// </summary>
        [DataMember(Name = "buyCount")]
        public int BuyCount { get; set; }

        /// <summary>
        /// 商品图片列表
        /// </summary>
        [DataMember(Name = "productImageArray")]
        public List<ImageUrl> ProductImageArray { get; set; }

        /// <summary>
        /// 商品图片
        /// </summary>
        [DataMember(Name = "imageUrl")]
        public string ImageUrl { get; set; }

        /// <summary>
        /// 大商品图片
        /// </summary>
        [DataMember(Name = "bigImageUrl")]
        public string bigImageUrl { get; set; }

        /// <summary>
        ///往期评价总数量
        /// </summary>
        [DataMember(Name = "commentCount")]
        public string CommentCount { get; set; }

        /// <summary>
        /// 咨询总数
        /// </summary>
        [DataMember(Name = "consultCount")]
        public int ConsultCount { get; set; }

        /// <summary>
        /// sku属性集
        /// </summary>
        [DataMember(Name = "skuItems")]
        public List<SkuItems> SkuItemses { get; set; }

        /// <summary>
        /// 库存价格
        /// </summary>
        [DataMember(Name = "skuStock")]
        public List<SkuStock> SkuStockList { get; set; }

        /// <summary>
        /// 创建时间（时间戳）
        /// </summary>
        [DataMember(Name = "createDt")]
        public long CreateDt { get; set; }

        /// <summary>
        /// 爆款开始时间
        /// </summary>
        [DataMember(Name = "startDt")]
        public long StartDt { get; set; }

        /// <summary>
        /// 拼购结束时间 (时间戳)
        /// </summary>
        [DataMember(Name = "endDt")]
        public long EndDt { get; set; }

        /// <summary>
        /// 服务器当前时间 (时间戳)
        /// </summary>
        [DataMember(Name = "serviceDt")]
        public long ServiceDt { get; set; }

        /// <summary>
        /// 是否特价 1是 0不是
        /// </summary>
        [DataMember(Name = "isSpecial")]
        public int IsSpecial { get; set; }
          
        /// <summary>
        /// 爆款折扣
        /// </summary>
        [DataMember(Name = "explosionrebate")]
        public decimal ExplosionRebate { get; set; }


        /// <summary>
        /// 是否收藏
        /// </summary>
        [DataMember(Name = "isFavorites")]
        public long IsFavorites { get; set; }



        


    }

    [DataContract]
    public class SkuStock
    {

        /// <summary>
        /// SKU编号
        /// </summary>
        [DataMember(Name = "skuid")]
        public string SkuProducId { get; set; }

        /// <summary>
        /// SKU编码
        /// </summary>
        [DataMember(Name = "sku")]
        public string SkuNumber { get; set; }


        /// <summary>
        /// SKU名称
        /// </summary>
        [DataMember(Name = "skuname")]
        public string SkuName { get; set; }

        /// <summary>
        /// 库存
        /// </summary>
        [DataMember(Name = "stock")]
        public int Stock { get; set; }

        ///// <summary>
        ///// 价格
        ///// </summary>
        //[DataMember(Name = "price")]
        //public decimal Price { get; set; }

        /// <summary>
        /// 市场价
        /// </summary>
        [DataMember(Name = "marketPrice")]
        public decimal MarketPrice { get; set; }

        /// <summary>
        /// 惠卡价
        /// </summary>
        [DataMember(Name = "hkPrice")]
        public decimal hkPrice { get; set; }

    }
     


    [DataContract]
    public class SkuItems
    {
        /// <summary>
        /// 属性ID
        /// </summary>
        [DataMember(Name = "attributeId")]
        public string AttributeId { get; set; }

        /// <summary>
        /// 属性名称
        /// </summary>
        [DataMember(Name = "attributeName")]
        public string AttributeName { get; set; }

        /// <summary>
        /// 图片或文字 true图片,false文字
        /// </summary>
        [DataMember(Name = "isRelationImage")]
        public bool IsRelationImage { get; set; }

        /// <summary>
        /// 属性项值
        /// </summary>
        [DataMember(Name = "values")]
        public List<Values> PropertyValues { get; set; }
    }

    [DataContract]
    public class Values
    {

        /// <summary>
        /// 属性值ID
        /// </summary>
        [DataMember(Name = "valueId")]
        public string ValueId { get; set; }

        /// <summary>
        /// 属性ID
        /// </summary>
        [DataMember(Name = "attributeId")]
        public string AttributeId { get; set; }

        /// <summary>
        /// 展示文字
        /// </summary>
        [DataMember(Name = "valueStr")]
        public string ValueStr { get; set; }

        /// <summary>
        /// 展示图片地址
        /// </summary>
        [DataMember(Name = "imgUrl")]
        public string ImgUrl { get; set; }
        /// <summary>
        /// SKU编码
        /// </summary>
        [DataMember(Name = "sku")]
        public string Sku { get; set; }

    }

    /// <summary>
    /// 商品图片
    /// </summary>
    [DataContract]
    public class ImageUrl
    {
        /// <summary>
        /// 商品图片
        /// </summary>
        [DataMember(Name = "imageUrl")]
        public string ImageUrls { get; set; }

        /// <summary>
        /// 大商品图片
        /// </summary>
        [DataMember(Name = "bigImageUrl")]
        public string BigimageUrls { get; set; }
    }

    /// <summary>
    /// 商品图片
    /// </summary>
    [DataContract]
    public class Merchant
    {
        /// <summary>
        /// 商家ID
        /// </summary>
        [DataMember(Name = "merchantId")]
        public string MerchantId { get; set; }
        /// <summary>
        /// 商家名称
        /// </summary>
        [DataMember(Name = "shopName")]
        public string ShopName { get; set; }

        /// <summary>
        /// 商家地址
        /// </summary>
        [DataMember(Name = "shopAddress")]
        public string ShopAddress { get; set; }

        /// <summary>
        /// 商家电话
        /// </summary>
        [DataMember(Name = "shopPhone")]
        public string ShopPhone { get; set; }

        /// <summary>
        /// 商家logo图片
        /// </summary>
        [DataMember(Name = "shopImg")]
        public string ShopImg { get; set; }
    }

    [DataContract]
    public class ProductCommentArray
    {
        /// <summary>
        /// 当前评论内容
        /// </summary>
        [DataMember(Name = "commentContent")]
        public string CommentContent { get; set; }

        /// <summary>
        /// 当前评论用户昵称(没有昵称显示电话号码)
        /// </summary>
        [DataMember(Name = "commentUser")]
        public string CommentUser { get; set; }
        /// <summary>
        /// 当前评论星级 1-5
        /// </summary>
        [DataMember(Name = "commentLevel")]
        public string CommentLevel { get; set; }

        /// <summary>
        /// 当前评论时间(时间戳)
        /// </summary>
        [DataMember(Name = "commentDt")]
        public string CommentDt { get; set; }

        /// <summary>
        /// 当前评论是否匿名	 1-是 0-否
        /// </summary>
        [DataMember(Name = "isAnonymous")]
        public string IsAnonymous { get; set; }
    }
   
} 