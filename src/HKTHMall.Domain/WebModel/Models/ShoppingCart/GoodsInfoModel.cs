using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.WebModel.Models.ShoppingCart
{
    /// <summary>
    /// 购物车商品信息
    /// </summary>
    [DataContract]
    public class GoodsInfoModel
    {
        /// <summary>购物车ID.</summary>
        /// <remarks></remarks>
        /// <author>ywd 2015-7-16 00:53:25</author>
        /// <value>The ShoopingCarts units.</value>
        [DataMember(Name = "cartsId")]
        public long CartsId { get; set; }

        [DataMember(Name = "goodsId")]
        public String GoodsId { get; set; }

        [DataMember(Name = "addToShoppingCartTime")]
        public String AddToShoppingCartTime { get; set; }

        [DataMember(Name = "CartDate")]
        public System.DateTime CartDate { get; set; }

        [DataMember(Name = "count")]
        public Int32 Count { get; set; }

        [DataMember(Name = "isChecked")]
        public String IsChecked { get; set; }

        [DataMember(Name = "pic")]
        public String Pic { get; set; }
        //商品主图

        [DataMember(Name = "productActiveImage")]
        public String ProductActiveImage { get; set; }

        [DataMember(Name = "goodsName")]
        public String GoodsName { get; set; }

        /// <summary>惠卡价.</summary>
        /// <remarks></remarks>
        /// <author>PanYun HX1501345 2015-04-24 17:19:45</author>
        /// <value>The goods units.</value>
        [DataMember(Name = "goodsUnits")]
        public Decimal GoodsUnits { get; set; }

        [DataMember(Name = "comId")]
        public String ComId { get; set; }

        [DataMember(Name = "comName")]
        public String ComName { get; set; }

        /// <summary>是否失效；商品库存为0 or 下架为失效  (API:0为正常,1为库存为零,2 为下架失效)</summary>
        /// <remarks></remarks>
        /// <author>PanYun HX1501345 2015-04-21 15:46:01</author>
        /// <value>The is disabled.</value>
        [DataMember(Name = "isDisabled")]
        public Int32 IsDisabled { get; set; }

        /// <summary>商品状态:未提交=1,待审核=2,审核不通过=3,已上架=4,已下架=5</summary>
        /// <remarks></remarks>
        /// <value>The goods Status.</value>
        [DataMember(Name = "status")]
        public Int32 Status { get; set; }

        /// <summary>库存数量</summary>
        /// <remarks></remarks>
        /// <author>PanYun HX1501345 2015-04-23 08:56:03</author>
        /// <value>The stock quantity.</value>
        [DataMember(Name = "stockQuantity")]
        public Int32 StockQuantity { get; set; }

        /// <summary>邮费</summary>
        /// <remarks></remarks>
        /// <author>PanYun HX1501345 2015-04-23 08:56:51</author>
        /// <value>The postage price.</value>
        [DataMember(Name = "postagePrice")]
        public Decimal PostagePrice { get; set; }
        /// <summary>
        /// 邮费模板
        /// </summary>
        public long FareTemplateID { get; set; }
        /// <summary>是否提供发票</summary>
        /// <remarks></remarks>
        /// <author>PanYun HX1501345 2015-04-23 10:45:41</author>
        /// <value>The is provide invoices.</value>
        [DataMember(Name = "isProvideInvoices")]
        public Int32 IsProvideInvoices { get; set; }

        /// <summary>市场价</summary>
        /// <remarks></remarks>
        /// <author>PanYun HX1501345 2015-04-24 17:19:35</author>
        /// <value>The market price.</value>
        [DataMember(Name = "marketPrice")]
        public Decimal MarketPrice { get; set; }

        /// <summary>
        /// 成本价
        /// </summary>
        [DataMember(Name = "purchasePrice")]
        public decimal PurchasePrice { get; set; }

        /// <summary>
        /// Ryan SkuNumber Sku编码
        /// </summary>
        [DataMember(Name = "skuNumber")]
        public String SkuNumber { get; set; }

        /// <summary>
        /// Ryan 商家ID
        /// </summary>
        [DataMember(Name = "merchantID")]
        public long MerchantID { get; set; }

        /// <summary>
        /// Sku属性名称,如:规格
        /// </summary>
        [DataMember(Name = "attributeName")]
        public String AttributeName { get; set; }

        /// <summary>
        /// Sku属性值,如:22尺
        /// </summary>
        [DataMember(Name = "valueStr")]
        public String ValueStr { get; set; }

        /// <summary>
        /// 供应商Id
        /// </summary>
        [DataMember(Name = "supplierId")]
        public long SupplierId { get; set; }

        /// <summary>
        /// 重量
        /// </summary>
        [DataMember(Name = "weight")]
        public decimal Weight { get; set; }
        /// <summary>
        /// 体积
        /// </summary>
        public decimal Volume { get; set; }
        
        /// <summary>
        /// 是否免运费
        /// </summary>
        [DataMember(Name = "freeShipping")]
        public int FreeShipping { get; set; }
        /// <summary>
        /// 返现天数
        /// </summary>
        public int RebateDays { get; set; }
        /// <summary>
        /// 返现比例
        /// </summary>
        public decimal RebateRatio { get; set; }
    }
}
