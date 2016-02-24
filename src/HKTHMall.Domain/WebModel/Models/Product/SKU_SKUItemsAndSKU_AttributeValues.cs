using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.WebModel.Models.Product
{
    public class SKU_SKUItemsAndSKU_AttributeValues
    {
        /// <summary>
        /// 商品扩展属性
        /// </summary>
        public int SKU_SKUItemsId { get; set; }
        /// <summary>
        /// 产品id
        /// </summary>
        public long ProductId { get; set; }

        /// <summary>
        ///所有值的数据参数
        /// </summary>
        ///
        /// 值Id
        /// 
        public long ValueId { get; set; }
        /// <summary>
        /// 规格id
        /// </summary>
        public long AttributeId { get; set; }
        /// <summary>
        /// 显示顺序
        /// </summary>
        public int DisplaySequence { get; set; }
        /// <summary>
        /// 自定义显示文字
        /// </summary>
        public string CustomValueStr { get; set; }
        /// <summary>
        /// 显示文字
        /// </summary>
        public string ValueStr { get; set; }
    }
}
