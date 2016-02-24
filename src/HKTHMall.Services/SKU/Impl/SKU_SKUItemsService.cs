using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKTHMall.Domain.Models;
using HKTHMall.Domain.WebModel.Models.Product;

namespace HKTHMall.Services.SKU.Impl
{
    public class SKU_SKUItemsService : BaseService, ISKU_SKUItems
    {
        /// <summary>
        /// 查询产品规格值列表
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public ResultModel GetSKU_SKUItemsAndSKU_AttributeValuesById(long productId)
        {
            var tb1 = _database.Db.SKU_SKUItems;
            var tb2 = _database.Db.SKU_AttributeValues;
            var result = new ResultModel
            {

                Data = tb1.Query()
                        .Join(tb2, ValueId: tb1.ValueId)
                        .Select(tb1.SKU_SKUItemsId, tb1.ProductId,tb1.ValueStr.As("CustomValueStr"),
                                 tb2.ValueId, tb2.AttributeId, tb2.DisplaySequence, tb2.ValueStr)
                        .Where(tb1.ProductId == productId)
                        .ToList<SKU_SKUItemsAndSKU_AttributeValues>()
            };
            return result;
        }

    }
}
