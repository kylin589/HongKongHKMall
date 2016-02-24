using HKTHMall.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Services.Products
{
    public class FavoritesService : BaseService, IFavoritesService
    {

        public ResultModel IsExistFavorites(long userId, long productId)
        {
            ResultModel result = new ResultModel();
            result.IsValid = false;
            var list = _database.Db.Favorites.All().Select(_database.Db.Favorites.UserID).Where(_database.Db.Favorites.FavoritesID == productId).ToArray();
            if (list.Length > 0)
            {
                result.IsValid = true;
                result.Messages.Add("此商品在收藏夹已存在");
                return result;
            }
            result.Messages.Add("此商品不存在");
            return result;
        }

    }
}
