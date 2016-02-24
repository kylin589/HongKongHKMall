using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKTHMall.Domain.Entities;
using HKTHMall.Domain.Validators.SKU;
using HKTHMall.Domain.WebModel.Models.Index;

namespace HKTHMall.Domain.WebModel.Models.Product
{

    /// <summary>
    /// 推荐显示商品信息
    /// </summary>
    public class ProductInfo
    {
        /// <summary>
        /// 唯一识别码
        /// </summary>
        public string Guid { get; set; }

        /// <summary>
        /// 商品ID
        /// </summary>
        public long ProductId { get; set; }


        /// <summary>
        /// 显示图片
        /// </summary>
        public string PicUrl { get; set; }

        /// <summary>
        /// 惠卡价格
        /// </summary>
        public decimal HKPrice { get; set; }

        /// <summary>
        /// 市场价
        /// </summary>
        public decimal MarketPrice { get; set; }

        /// <summary>
        /// 库存数量
        /// </summary>
        public int StockQuantity { get; set; }

        /// <summary>
        /// 商品名称描述
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 副标题
        /// </summary>
        public string Subheading { get; set; }

        /// <summary>
        /// 产品描述
        /// </summary>
        public string Introduction { get; set; }

        /// <summary>
        /// 产品图片地址
        /// </summary>
        public string BrandUrl { get; set; }

        /// <summary>
        /// 商品類型Id
        /// </summary>
        public string CategoryId { get; set; }

        /// <summary>
        /// 商品類型名称描述
        /// </summary>
        public string CategoryName { get; set; }


        /// <summary>
        /// 获取产品截图
        /// </summary>
        public List<ProductPicModel> Imglist { get; set; }
        /// <summary>
        /// 产品评论数
        /// </summary>
        public CommentCount GetCommentCount { get; set; }

        /// <summary>
        /// 评论平均评数
        /// </summary>
        public decimal AvgCommentRate { get; set; }



        /// <summary>
        /// 获取库存总数值
        /// </summary>
        public List<SKU_ProductModel> SKU_ProductList { get; set; }

        /// <summary>
        /// 产品规格清单
        /// </summary>
        public List<SKU_ProductAttributes> SKU_ProductAttributesList { get; set; }

        /// <summary>
        /// 产品规格值清单
        /// </summary>
        public List<SKU_ProductAttributesAndSKU_AttributeValues> SKU_ProductAttributesAndSKU_AttributeValuesList { get; set; }


        /// <summary>
        /// 产品扩展属性值
        /// </summary>
        public List<SKU_SKUItemsAndSKU_AttributeValues> SKU_SKUItemsAndSKU_AttributeValuesList { get; set; }

        /// <summary>
        /// 产品类型规格参数值
        /// </summary>
        public List<ProductCategoryTypeForSKU_Attributes> ProductCategoryTypeForSKU_AttributesList { get; set; }

        /// <summary>
        /// 产品规格参数
        /// </summary>
        public List<ProductParametersModel> ProductParametersList { get; set; }

        /// <summary>
        /// 是否为促销产品
        /// </summary>
        public List<IndexExplosion> IndexExplosionList { get; set; }

        /// <summary>
        /// 商品状态
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDT { get; set; }

        /// <summary>
        /// 商品销售量
        /// </summary>
        public int SaleCount { get; set; }

        /// <summary>
        /// 促销类型 1:无促销,2:打折
        /// </summary>
        public int SalesRuleId { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StarDate { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// 折扣率
        /// </summary>
        public decimal Discount { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public long UserID { get; set; }


        /// <summary>
        /// 选择的销售商品属性
        /// </summary>
        public string Keys { get; set; }


        /// <summary>
        /// 库存数据
        /// </summary>
        public string Data { get; set; }



    }
}
