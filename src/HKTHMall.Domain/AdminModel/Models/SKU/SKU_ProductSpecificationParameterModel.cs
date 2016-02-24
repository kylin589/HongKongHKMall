using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.AdminModel.Models.SKU
{
    public class SKU_ProductSpecificationParameterModel
    {
        /// <summary>
        /// 商品详情
        /// </summary>
        public string introduction { get; set; }
        /// <summary>
        /// 规格数组
        /// </summary>
        public List<SKU_AttributeArrayModel> attributeArray { get; set; }
    }
    public class SKU_AttributeArrayModel
    {
        /// <summary>
        /// 属性项ID
        /// </summary>
        public string attributeId { get; set; }
        /// <summary>
        /// 属性项名称
        /// </summary>
        public string attributeName { get; set; }
        /// <summary>
        /// 规格类型0文字,1图片
        /// </summary>
        public int attributeType { get; set; }
        /// <summary>
        /// 选择框模式,0:复选框,1:单选框,2:下拉框,3:输入框
        /// </summary>
        public int usageMode { get; set; }
        /// <summary>
        /// 子集合
        /// </summary>
        public List<SKU_ChildAttributeArrayModel> childAttributeArray { get; set; }

    }
    public class SKU_ChildAttributeArrayModel
    {
        /// <summary>
        /// 展示文字
        /// </summary>
        public string valuestr { get; set; }
        /// <summary>
        /// 展示图片地址
        /// </summary>
        public string imageurl { get; set; }
        /// <summary>
        /// 分组
        /// </summary>
        public int group { get; set; }

    }
}
