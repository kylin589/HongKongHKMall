using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrCms.Core.Infrastructure;
using HKTHMall.Domain.AdminModel.Models.Products;
using HKTHMall.Domain.Models;
using HKTHMall.Domain.WebModel.Models.SKU;

namespace HKTHMall.Services.SKU
{
    public interface ISKU_ProductService : IDependency
    {

        /// <summary>
        /// 查询库存数据
        /// </summary>
        /// <param name="productId">商品ID</param>
        /// <returns></returns>
        ResultModel GetSKU_ProductById(long productId);

    }
}
