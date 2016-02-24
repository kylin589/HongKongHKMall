using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrCms.Core.Infrastructure;
using HKTHMall.Domain.Models;

namespace HKTHMall.Services.WebProducts
{
    public interface IProductPicSercive : IDependency
    {
        /// <summary>
        /// 根据商品ID获取图片
        /// </summary>
        /// <param name="productId"> 商品ID</param>
        /// <param name="orderby">排序 1:ASC;2:DESC</param>
        /// <returns></returns>
        ResultModel GetImageListByProductIdNoPage(long productId, int orderby);

        /// <summary>
        /// 根据商品ID获取商品主图Flag(1主图，0非主图)
        /// </summary>
        /// <param name="productId"> 商品ID</param>
        /// <returns></returns>
        ResultModel GetFlagImageByProductId(long productId);
    }
}
