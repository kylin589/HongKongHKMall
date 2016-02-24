using BrCms.Core.Infrastructure;
using HKTHMall.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Services.Products
{
    public interface IFavoritesService : IDependency
    {
        /// <summary>
        /// 商品是否存在
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="productId">商品ID</param>
        /// <returns></returns>
        ResultModel IsExistFavorites(long userId, long productId);
    }
}
