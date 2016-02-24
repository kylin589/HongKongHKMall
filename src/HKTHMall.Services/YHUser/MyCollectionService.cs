using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKTHMall.Domain.Models;
using HKTHMall.Domain.WebModel.Models.Collection;
using HKTHMall.Core;

namespace HKTHMall.Services.YHUser
{
    public class MyCollectionService : BaseService, IMyCollectionService
    {
        /// <summary>
        /// 加入收藏
        /// </summary>
        /// <param name="model">收藏模型</param>
        public ResultModel Add(FavoritesModel model)
        {
            var result = new ResultModel();
            var favorites = _database.Db.Favorites.Find(_database.Db.Favorites.UserID == model.UserID &&
                _database.Db.Favorites.ProductId == model.ProductId);
            if (favorites != null)
            {
                result.IsValid = false;
                result.Messages.Add("已经收藏过这商品,不能再收藏");
                result.Data = favorites.FavoritesID;
            }
            else
            {
                try
                {
                    result.Data = _database.Db.Favorites.Insert(model);
                }
                catch (Exception ex)
                {
                    //todo 错误日志记录
                    throw;
                }
            }
            return result;
        }

        /// <summary>
        /// 刷新收藏
        /// </summary>
        /// <param name="model">收藏模型</param>
        public ResultModel Find(FavoritesModel model)
        {
            var result = new ResultModel
            {
                Data = _database.Db.Favorites.All()
                .Where(_database.Db.Favorites.UserID == model.UserID && _database.Db.Favorites.ProductId == model.ProductId)
                .ToList<FavoritesModel>()
            };
            result.IsValid = result.Data.Count > 0;
            return result;
        }



        #region APP接口
        /// <summary>
        /// 添加收藏
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="productId">商品ID</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-8-7</remarks>
        public ResultModel AddFavorites(long userId, long productId, out long favoritesID)
        {
            FavoritesModel model = new FavoritesModel();
            model.UserID = userId;
            model.ProductId = productId;
            model.FavoritesDate = DateTime.Now;
            model.FavoritesID = MemCacheFactory.GetCurrentMemCache().Increment("commonId");
            var result = Add(model);
            favoritesID = !result.IsValid ? result.Data : model.FavoritesID;
            return result;
        }
        #endregion
    }
}
