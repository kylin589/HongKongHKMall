using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrCms.Core.Infrastructure;
using HKTHMall.Domain.Models;

namespace HKTHMall.Services.SKU
{
    public interface ISKU_ProductAttributesService : IDependency
    {

        /// <summary>
        /// 获取产品规格清单列表
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        ResultModel GetSKU_ProductAttributesById(long productId);


        /// <summary>
        /// 查询产品规格值列表
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        ResultModel GetSKU_ProductAttributesAndSKU_AttributeValuesById(long productId);

    }
}
