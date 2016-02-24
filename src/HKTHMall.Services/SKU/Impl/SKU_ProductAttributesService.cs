using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKTHMall.Domain.AdminModel.Models.SKU;
using HKTHMall.Domain.Entities;
using HKTHMall.Domain.Models;
using HKTHMall.Domain.WebModel.Models.Product;

namespace HKTHMall.Services.SKU.Impl
{
    public class SKU_ProductAttributesService : BaseService, ISKU_ProductAttributesService
    {


        /// <summary>
        /// 查询产品规格列表
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public ResultModel GetSKU_ProductAttributesById(long productId)
        {
            var result = new ResultModel
            {
                Data = _database.Db.SKU_ProductAttributes.Query().Where(_database.Db.SKU_ProductAttributes.ProductId == productId).ToList<SKU_ProductAttributes>()
            };
            return result;
        }

        /// <summary>
        /// 查询产品规格值列表
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public ResultModel GetSKU_ProductAttributesAndSKU_AttributeValuesById(long productId)
        {
            var tb1 = _database.Db.SKU_ProductAttributes;
            var tb2 = _database.Db.SKU_AttributeValues;
            var result = new ResultModel
            {

                Data = tb1.Query()
                        .Join(tb2, ValueId: tb1.ValueId)
                        .Select(tb1.SKU_ProductAttributesId, tb1.ProductId, tb1.ImageUrl
                                , tb2.ValueId, tb2.AttributeId, tb2.DisplaySequence, tb2.ValueStr)
                        .Where(tb1.ProductId == productId)
                        .ToList<SKU_ProductAttributesAndSKU_AttributeValues>()
            };
            return result;
        }
    }
}
