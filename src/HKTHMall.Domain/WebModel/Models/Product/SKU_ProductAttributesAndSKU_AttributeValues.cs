using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.WebModel.Models.Product
{
    public class SKU_ProductAttributesAndSKU_AttributeValues
    {

        /// <summary>
        /// 商品所有规格的值
        /// </summary>
        public int SKU_ProductAttributesId { get; set; }
        /// <summary>
        /// 产品id
        /// </summary>
        public long ProductId { get; set; } 
        /// <summary>
        /// 显示图片
        /// </summary>
        public string ImageUrl { get; set; }

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
        /// 显示文字
        /// </summary>
        public string ValueStr { get; set; }
       
    }
}
