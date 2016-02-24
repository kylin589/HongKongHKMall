using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrCms.Core.Infrastructure;
using HKTHMall.Domain.Models;
using HKTHMall.Domain.WebModel.Models.Collection;

namespace HKTHMall.Services.YHUser
{

    public interface IMyCollectionService : IDependency
    {
        /// <summary>
        /// 加入收藏
        /// </summary>
        /// <param name="model">收藏模型</param>
        ResultModel Add(FavoritesModel model);

        /// <summary>
        /// 刷新收藏
        /// </summary>
        /// <param name="model">收藏模型</param>
        ResultModel Find(FavoritesModel model);

        #region APP接口
        /// <summary>
        /// 添加收藏
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="productId">商品ID</param>
        /// <param name="favoritesID">收藏ID</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-8-7</remarks>
        ResultModel AddFavorites(long userId, long productId,out long favoritesID);
        #endregion

    }
}
