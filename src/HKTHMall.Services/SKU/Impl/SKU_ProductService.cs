using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKTHMall.Domain.Entities;
using HKTHMall.Domain.Models;
using HKTHMall.Domain.WebModel.Models.Product;

namespace HKTHMall.Services.SKU.Impl
{
    public class SKU_ProductService : BaseService, ISKU_ProductService
    {

        /// <summary>
        /// 查询库存数据
        /// </summary>
        /// <param name="productId">商品ID</param>
        /// <returns></returns>
        public ResultModel GetSKU_ProductById(long productId)
        {
            var result = new ResultModel
            {
                Data = _database.Db.SKU_Product.Query().Where(_database.Db.SKU_Product.ProductId == productId ).ToList<SKU_ProductModel>()
            };
            return result;
        }

        /// <summary>
        /// 生成库存更新Sql语句
        /// </summary>
        /// <param name="view">库存实体</param>
        /// <returns>Sql语句</returns>
        internal string GenerateUpdateStockSql(Domain.WebModel.Models.SKU.SKU_ProductView view)
        {
            string sql = string.Format(" UPDATE SKU_Product SET Stock=Stock+{0} WHERE SKU_ProducId={1}", view.Stock, view.SKU_ProducId);
            return sql;
        }
    }
}
