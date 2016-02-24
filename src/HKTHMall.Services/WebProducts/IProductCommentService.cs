using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrCms.Core.Infrastructure;
using HKTHMall.Domain.Models;
using HKTHMall.Domain.Models.Categoreis;

namespace HKTHMall.Services.WebProducts
{
    public interface IProductCommentService : IDependency
    {
        /// <summary>
        /// 获取产品评价总数、好评数、中评数、差评数
        /// </summary>
        /// <param name="key">产品ID</param>
        /// <returns></returns>
        ResultModel GetCount(long key);

        /// <summary>
        /// 获取产品平均评星
        /// </summary>
        /// <param name="productId">产品ID</param>
        /// <returns></returns>
        decimal GetProductCommentAvgRate(long productId);

        /// <summary>
        /// 获取商品评论列表
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns>商品评论列表</returns>
        ResultModel GetProductCommentList(SearchSP_ProductCommentModel model);

        /// <summary>
        /// 获取商品评论列表
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns>商品评论列表</returns>
        ResultModel GetProductCommentList(SearchSP_ProductCommentModel model,out int TotalCnt);

        /// <summary>
        /// 返回产品的印象标签汇总
        /// </summary>
        /// <param name="productid"></param>
        /// <returns></returns>
        List<HKTHMall.Domain.WebModel.Models.Product.CommentLablesGroup> GroupLabelsByProduct(long productid);

    }
}
