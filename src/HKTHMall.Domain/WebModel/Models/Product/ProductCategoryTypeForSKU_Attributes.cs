using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.WebModel.Models.Product
{
    public class ProductCategoryTypeForSKU_Attributes
    {

        //============================================================
        //CategoryType 
        public long CategoryTypeId { get; set; } 
        /// <summary>
        /// 分类ID
        /// </summary>
        public int CategoryId { get; set; }
 
        //============================================================
        //SKU_ProductTypeAttribute 
        //============================================================
        public long SKU_ProductTypeAttributeId { get; set; }
        /// <summary>
        /// 商品类型ID
        /// </summary>
        public long SkuTypeId { get; set; }
        /// <summary>
        /// 属性项类型0:SKU属性,1:扩展属性,2:参数表,3:扩展属性+参数表
        /// </summary>
        public int SKU_P_AttributeType { get; set; }
        /// <summary>
        /// 属性项ID排序
        /// </summary>
        public long DisplaySequence { get; set; } 

        //============================================================
        //SKU_Attributes 
        //============================================================

        /// <summary>
        /// 属性项ID
        /// </summary>
        public long AttributeId { get; set; }
        /// <summary>
        /// 属性项名称
        /// </summary>
        public string AttributeName { get; set; }
        /// <summary>
        /// 规格类型,0文字,1图片
        /// </summary>
        public int SKU_AttributeType { get; set; }
        //是否规格属性
        public int IsSKU { get; set; }
        /// <summary>
        /// 选择框模式,0:复选框,1:单选框,2:下拉框,3:输入框
        /// </summary>
        public int UsageMode { get; set; }
 
     
    }
}
