using BrCms.Core.Infrastructure;
using HKTHMall.Domain.Models;
using HKTHMall.Domain.Models.Categoreis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKTHMall.Domain.WebModel.Models.Product;

namespace HKTHMall.Services.Products
{
    public interface ISP_ProductCommentService : IDependency
    {
        /// <summary>
        /// 添加商品评论
        /// </summary>
        /// <param name="model">商品评论</param>
        /// <returns>是否成功</returns>
        ResultModel AddSP_ProductComment(SP_ProductCommentModel model);

        /// <summary>
        /// 根据商品评论id获取商品评论
        /// </summary>
        /// <param name="id">商品评论id</param>
        /// <returns>商品评论模型</returns>
        ResultModel GetSP_ProductCommentById(long? ProductCommentId);

        /// <summary>
        /// 获取商品评论列表
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns>商品评论列表</returns>
        ResultModel GetSP_ProductCommentList(SearchSP_ProductCommentModel model);

        /// <summary>
        /// 更新商品评论
        /// </summary>
        /// <param name="model">商品评论模型</param>
        /// <returns>是否修改成功</returns>
        ResultModel UpdateSP_ProductComment(SP_ProductCommentModel model);

        /// <summary>
        /// 更新商品评论状态
        /// </summary>
        /// <param name="model">商品评论模型</param>
        /// <returns>是否修改成功</returns>
        ResultModel UpdateSP_ProductCommentCheckStatus(SP_ProductCommentModel model);

        /// <summary>
        /// 删除商品评论
        /// </summary>
        /// <param name="model">商品评论模型</param>
        /// <returns>是否删除成功</returns>
        ResultModel DeleteSP_ProductComment(SP_ProductCommentModel model);

        /// <summary>
        /// 批量添加商品评论
        /// </summary>
        /// <param name="models">商品评论</param>
        /// <returns>评论列表</returns>
        ResultModel BatchAddSP_ProductComment(List<SP_ProductCommentModel> models);


        /// <summary>
        /// 获取单个订单的商品评论
        /// </summary>
        /// <param name="model">查询模型</param>
        /// <returns>单个订单的商品评论</returns>
        ResultModel GetOrderProductComments(SearchOrderProductCommentView model);

         /// <summary>
        /// 分页获取用户商品评论
        /// </summary>
        /// <param name="model">查询模型</param>
        /// <returns>分页获取用户商品评论</returns>
        ResultModel GetPaingCommentsIntoWeb(SearchOrderProductCommentView model);

         /// <summary>
        /// 根据商品ID和返回类型返回（好、中、差评总数）wuyf
        /// </summary>
        /// <param name="ProductId"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        ResultModel GetPaingCommentCount(long ProductId, int type);
    }
}
