using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrCms.Core.Infrastructure;
using HKTHMall.Domain.Models;

namespace HKTHMall.Services.SKU
{
    public interface ISKU_SKUItems : IDependency
    {
        /// <summary>
        /// 获取扩展属性表,用于显示使用
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        ResultModel GetSKU_SKUItemsAndSKU_AttributeValuesById(long productId);
    }
}
