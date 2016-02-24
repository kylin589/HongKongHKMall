using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKTHMall.Domain.Models;
using HKTHMall.Domain.WebModel.Models.Product;

namespace HKTHMall.Services.WebProducts.Impl
{
    public class ProductPicSercive : BaseService, IProductPicSercive
    {
        /// <summary>
        /// 根据商品ID获取图片
        /// </summary>
        /// <param name="productId"> 商品ID</param>
        /// <param name="orderby">排序 1:ASC;2:DESC</param>
        /// <returns></returns>
        public ResultModel GetImageListByProductIdNoPage(long productId, int orderby)
        {

            var tb = _database.Db.ProductPic;
            var result = new ResultModel();
            if (orderby==1)
            {
                result.Data = tb.FindAllByProductID(productId)
                    .OrderBy(tb.ProductPic.sort)
                    .ToList<ProductPicModel>();
            }
            else
            {
                result.Data = tb.FindAllByProductID(productId)
                    .OrderByDescending(tb.ProductPic.sort)
                    .ToList<ProductPicModel>();
            }
            if (result.Data.Count == 0)
            {
                result.IsValid = false;
                result.Messages = new List<string>() { "商品图片不存在." };
            }
            return result;
        }

        /// <summary>
        /// 根据商品ID获取商品主图Flag(1主图，0非主图)
        /// </summary>
        /// <param name="productId"> 商品ID</param>
        /// <returns></returns>
        public ResultModel GetFlagImageByProductId(long productId)
        {
            var tb = _database.Db.ProductPic;
            var result = new ResultModel()
            {
                Data = tb.All()
                .Where(tb.ProductID == productId && tb.Flag == 1)
                .ToList<ProductPicModel>()
            };
            return result;
        }
    }
}
