using BrCms.Framework.Collections;
using HKTHMall.Core.Extensions;
using HKTHMall.Core.Sql;
using HKTHMall.Domain.Models;
using HKTHMall.Domain.Models.Categoreis;
using HKTHMall.Domain.WebModel.Models.Orders;
using HKTHMall.Domain.WebModel.Models.Product;
using Simple.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simple.Data.RawSql;

namespace HKTHMall.Services.Products
{
    public class SP_ProductCommentService : BaseService, ISP_ProductCommentService
    {
        /// <summary>
        /// 添加商品评论
        /// </summary>
        /// <param name="model">商品评论</param>
        /// <returns>是否成功</returns>
        public ResultModel AddSP_ProductComment(SP_ProductCommentModel model)
        {

            var result = new ResultModel
            {
                Data = base._database.Db.SP_ProductComment.Insert(model)
            };
            return result;
        }

      

        /// <summary>
        /// 根据商品评论id获取商品评论
        /// </summary>
        /// <param name="id">商品评论id</param>
        /// <returns>商品评论模型</returns>
        public ResultModel GetSP_ProductCommentById(long? ProductCommentId)
        {
            var result = new ResultModel()
            {
                Data = base._database.Db.SP_ProductComment.Get(ProductCommentId)
            };

            return result;
        }

        /// <summary>
        /// 获取商品评论列表
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns>商品评论列表</returns>
        public ResultModel GetSP_ProductCommentList(SearchSP_ProductCommentModel model)
        {
            var tb = base._database.Db.SP_ProductComment;
            var where = new SimpleExpression(1, 1, SimpleExpressionType.Equal);

            if (model.OrderId != 0)
            {
                //订单ID
                where = new SimpleExpression(where, tb.OrderId == model.OrderId, SimpleExpressionType.And);
            }
            if (model.UserID != 0)
            {
                //用户ID
                where = new SimpleExpression(where, tb.UserID == model.UserID, SimpleExpressionType.And);
            }
            if (model.ProductId != null && model.ProductId != 0)
            {
                //商品id
                where = new SimpleExpression(where, tb.ProductId == model.ProductId, SimpleExpressionType.And);
            }
            if (model.CheckBy != null && !string.IsNullOrEmpty(model.CheckBy.Trim()))
            {
                //审核人名称
                where = new SimpleExpression(where, tb.CheckBy.Like("%" + model.CheckBy + "%"), SimpleExpressionType.And);
            }
            if (model.Phone != null && !string.IsNullOrEmpty(model.Phone))
            {
                //手机
                where = new SimpleExpression(where, _database.Db.YH_User.Phone.Like("%" + model.Phone + "%"), SimpleExpressionType.And);
            }

            if (model.Email != null && !string.IsNullOrEmpty(model.Email.Trim()))
            {
                //Email
                where = new SimpleExpression(where, _database.Db.YH_User.Email.Like("%" + model.Email + "%"), SimpleExpressionType.And);
            }

            if (model.BeginCommentDT != null)
            {
                //评论开始时间
                where = new SimpleExpression(where, tb.CommentDT >= model.BeginCommentDT, SimpleExpressionType.And);
            }

            if (model.EndCommentDT != null)
            {
                //评论结束 时间
                where = new SimpleExpression(where, tb.CommentDT < model.EndCommentDT, SimpleExpressionType.And);
            }

            if (model.BeginCheckDT != null)
            {
                //开始审核时间
                where = new SimpleExpression(where, tb.CheckDT >= model.BeginCheckDT, SimpleExpressionType.And);
            }

            if (model.EndCheckDT != null)
            {
                //结束审核时间
                where = new SimpleExpression(where, tb.CheckDT < model.EndCheckDT, SimpleExpressionType.And);
            }

            if (model.CheckStatus != 0)
            {
                //审核状态
                where = new SimpleExpression(where, tb.CheckStatus == model.CheckStatus, SimpleExpressionType.And);
            }

            if (model.LanguageID > 0)
            {
                //语言
                where = new SimpleExpression(where, _database.Db.Product_Lang.LanguageID == model.LanguageID, SimpleExpressionType.And);
            }

            if (model.ProductCommentId > 0)
            {
                //评论ID
                where = new SimpleExpression(where, tb.ProductCommentId == model.ProductCommentId, SimpleExpressionType.And);
            }
            // 1：好评；2：中评；3：差评  
            if (model.typeLevel != null && model.typeLevel.HasValue)
            {
                switch (model.typeLevel.Value)
                {
                    case 1:
                        where = new SimpleExpression(where, tb.CommentLevel >= 4, SimpleExpressionType.And);
                        break;
                    case 2:
                        where = new SimpleExpression(where, tb.CommentLevel >= 2 && tb.CommentLevel < 4, SimpleExpressionType.And);
                        break;
                    case 3:
                        where = new SimpleExpression(where, tb.CommentLevel >= 0 && tb.CommentLevel < 2, SimpleExpressionType.And);
                        break;
                    default:
                        break;
                }
            }

            dynamic cl;
            dynamic pc;

            var query = tb
                .Query()
                .LeftJoin(_database.Db.Product_Lang, out cl)
                .On(_database.Db.Product_Lang.ProductId == tb.ProductId)
                .LeftJoin(_database.Db.YH_User, out pc)
                .On(_database.Db.YH_User.UserID == tb.UserID)
                .Select(
                    tb.ProductCommentId,
                    tb.OrderId,
                    tb.UserID,
                    tb.ProductId,
                    tb.CommentLevel,
                    tb.CommentContent,
                    tb.CommentDT,
                    tb.IsAnonymous,
                    tb.ReplyBy,
                    tb.ReplyDT,
                    tb.ReplyContent,
                    tb.CheckBy,
                    tb.CheckStatus,

                    tb.CheckDT,


                    cl.ProductName,

                    pc.Account,
                    pc.NickName,
                    pc.Phone, //商品评价人显示手机号
                    pc.Email,
                    pc.HeadImageUrl
                )
                .Where(where)
                .OrderByCommentDTDescending();

            var result = new ResultModel()
            {
                Data = new SimpleDataPagedList<SP_ProductCommentModel>(query, model.PagedIndex, model.PagedSize)

            };

            return result;

        }

        /// <summary>
        /// 更新商品评论
        /// </summary>
        /// <param name="model">商品评论模型</param>
        /// <returns>是否修改成功</returns>
        public ResultModel UpdateSP_ProductComment(SP_ProductCommentModel model)
        {

            var result = new ResultModel()
            {
                Data = base._database.Db.SP_ProductComment.Update(model)

            };
            result.IsValid = result.Data > 0 ? true : false;
            return result;
        }

        /// <summary>
        /// 更新商品评论状态
        /// </summary>
        /// <param name="model">商品评论模型</param>
        /// <returns>是否修改成功</returns>
        public ResultModel UpdateSP_ProductCommentCheckStatus(SP_ProductCommentModel model)
        {

            var result = new ResultModel()
            {
                Data = base._database.Db.SP_ProductComment.UpdateByProductCommentId(ProductCommentId: model.ProductCommentId, CheckStatus: model.CheckStatus, CheckBy: model.CheckBy, CheckDT: model.CheckDT)

            };
            result.IsValid = result.Data > 0 ? true : false;
            return result;
        }

        /// <summary>
        /// 删除商品评论
        /// </summary>
        /// <param name="model">商品评论模型</param>
        /// <returns>是否删除成功</returns>
        public ResultModel DeleteSP_ProductComment(SP_ProductCommentModel model)
        {

            var result = new ResultModel()
            {
                Data = base._database.Db.SP_ProductComment.Delete(ProductCommentId: model.ProductCommentId)
            };
            result.IsValid = result.Data > 0 ? true : false;
            return result;
        }

        /// <summary>
        /// 批量添加商品评论
        /// </summary>
        /// <param name="models">商品评论</param>
        /// <returns>评论列表</returns>
        public ResultModel BatchAddSP_ProductComment(List<SP_ProductCommentModel> models)
        {
            var result = new ResultModel();

            //List<SP_ProductCommentModel> entities = new List<SP_ProductCommentModel>();
            if (models == null || models.Count == 0)
            {
                result.IsValid = false;
                result.Messages.Add("请输入评论内容");
            }
            else
            {
                int count = _database.Db.Order.GetCount(_database.Db.Order.OrderID == models[0].OrderId && _database.Db.Order.UserID == models[0].UserID);
                if (count == 0)
                {
                    result.IsValid = false;
                    result.Messages.Add("参数非法,该订单不是您的订单");
                }
                else
                {

                    //获取订单详情
                    List<OrderDetailsViewT1> orderDetails = _database.Db.OrderDetails.FindAll(_database.Db.OrderDetails.OrderID ==
                                                               models.Select(x => x.OrderId).ToArray() &&
                                                               _database.Db.OrderDetails.SKU_ProducId ==
                                                               models.Select(x => x.SKU_ProducId).ToArray() &&
                                                               _database.Db.OrderDetails.Iscomment == 0);
                    //开启事务
                    var trans = _database.Db.BeginTransaction();

                    try
                    {
                        foreach (var model in models)
                        {

                            var orderDetail =
                                orderDetails.FirstOrDefault(x => x.OrderID == model.OrderId.ToString() && x.ProductId == model.ProductId && x.SKU_ProducId == model.SKU_ProducId);

                            if (orderDetail != null && orderDetail.Iscomment == 0)
                            {
                                List<CommentLablesList> _list = CommentLablesList.ReturnList(model.LablesStr);
                                SP_ProductCommentModel tempModel = trans.SP_ProductComment.Insert(model);
                                //是否评价成功
                                if (tempModel.ProductCommentId > 0)
                                {
                                    foreach(CommentLablesList x in _list)
                                    {
                                        trans.SP_ProductComment_Labels.Insert(ProductCommentId: tempModel.ProductCommentId, UserID: model.UserID, Labels: x.Labels);
                                    }
                                    if (trans.OrderDetails.UpdateByOrderDetailsID(OrderDetailsID: orderDetail.OrderDetailsID, Iscomment: 1) == 0)
                                    {
                                        //entities.Add(tempModel);
                                        trans.Rollback();
                                    }
                                }
                                else
                                {
                                    trans.Rollback();
                                }

                            }
                        }
                        trans.Commit();
                        result.IsValid = true;
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        throw ex;
                    }
                    //result.Data = entities;
                }
            }
            return result;
        }

        /// <summary>
        /// 获取单个订单的商品评论
        /// </summary>
        /// <param name="model">查询模型</param>
        /// <returns>单个订单的商品评论</returns>
        public ResultModel GetOrderProductComments(SearchOrderProductCommentView model)
        {
            var result = new ResultModel() { IsValid = false };
            string sql = string.Format(@"SELECT a.SKU_ProducId,a.Iscomment,a.OrderID,a.ProductId,a.SkuName,c.PicUrl,d.ProductName,e.CommentContent,e.CommentDT,e.CommentLevel,e.ProductCommentId,
                            stuff((select ','+convert(varchar(10),Labels) from SP_ProductComment_Labels where ProductCommentId = e.ProductCommentId for xml path('')),1,1,'') as LablesStr
                            FROM OrderDetails AS a
                            INNER JOIN dbo.[Order] AS f ON a.OrderID=f.OrderID
                            INNER JOIN dbo.Product AS b ON a.ProductId =b.ProductId
                            LEFT JOIN(SELECT * FROM dbo.ProductPic WHERE Flag=1) AS c
                            ON b.ProductId=c.ProductID
                            LEFT JOIN(SELECT * FROM dbo.Product_Lang WHERE LanguageID={0}) AS d
                            ON b.ProductId=d.ProductId
                            LEFT JOIN dbo.SP_ProductComment AS e ON a.SKU_ProducId=e.SKU_ProducId AND a.OrderID=e.OrderId
                            WHERE a.OrderID='{1}' AND f.UserID={2}", model.LanguageID.Value, SqlFilterUtil.ReplaceSqlChar(model.OrderID), model.UserID);

            if (model.Iscomment.HasValue)
            {
                sql += string.Format(" AND a.Iscomment={0}", model.Iscomment.Value);
            }

            List<dynamic> sources = _database.RunSqlQuery(x => x.ToResultSets(sql))[0];
            result.Data = sources.ToEntity<OrderProductCommentView>();
            result.IsValid = true;
            return result;
        }

        /// <summary>
        /// 分页获取用户商品评论
        /// </summary>
        /// <param name="model">查询模型</param>
        /// <returns>分页获取用户商品评论</returns>
        public ResultModel GetPaingCommentsIntoWeb(SearchOrderProductCommentView model)
        {
            var result = new ResultModel() { IsValid = false };

            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.AppendLine(" SELECT COUNT(*) AS [Count] ");
            sqlBuilder.AppendLine(" FROM OrderDetails AS a ");
            sqlBuilder.AppendLine(" INNER JOIN dbo.[Order] AS f ON a.OrderID=f.OrderID ");
            sqlBuilder.AppendLine(" INNER JOIN dbo.Product AS b ON a.ProductId =b.ProductId ");
            sqlBuilder.AppendLine(" LEFT JOIN(SELECT * FROM dbo.ProductPic WHERE Flag=1) AS c ON b.ProductId=c.ProductID  ");
            sqlBuilder.AppendLine(" LEFT JOIN(SELECT * FROM dbo.Product_Lang WHERE LanguageID=1) AS d ON b.ProductId=d.ProductId ");
            sqlBuilder.AppendLine(" INNER JOIN dbo.YH_MerchantInfo AS g ON f.MerchantID=g.MerchantID ");
            sqlBuilder.AppendLine(" INNER JOIN dbo.SP_ProductComment AS e ON a.ProductId=e.ProductId AND a.OrderID=e.OrderId ");
            sqlBuilder.AppendFormat(" WHERE f.UserID={0}; ", model.UserID);

            sqlBuilder.AppendLine(" WITH tempTable AS ( ");
            sqlBuilder.AppendLine(" SELECT a.SKU_ProducId,a.Iscomment,a.OrderID,a.ProductId,a.SkuName,c.PicUrl,d.ProductName,e.CommentContent,e.CommentDT,e.CommentLevel,e.ProductCommentId,f.MerchantID,g.ShopName ");
            sqlBuilder.AppendLine(" FROM OrderDetails AS a ");
            sqlBuilder.AppendLine(" INNER JOIN dbo.[Order] AS f ON a.OrderID=f.OrderID ");
            sqlBuilder.AppendLine(" INNER JOIN dbo.Product AS b ON a.ProductId =b.ProductId ");
            sqlBuilder.AppendLine(" LEFT JOIN(SELECT * FROM dbo.ProductPic WHERE Flag=1) AS c ON b.ProductId=c.ProductID ");
            sqlBuilder.AppendFormat(" LEFT JOIN(SELECT * FROM dbo.Product_Lang WHERE LanguageID={0}) AS d ON b.ProductId=d.ProductId ", model.LanguageID);
            sqlBuilder.AppendLine(" INNER JOIN dbo.YH_MerchantInfo AS g ON f.MerchantID=g.MerchantID ");
            sqlBuilder.AppendLine(" INNER JOIN dbo.SP_ProductComment AS e ON a.SKU_ProducId=e.SKU_ProducId AND a.OrderID=e.OrderId ");
            sqlBuilder.AppendFormat(" WHERE f.UserID={0}) ", model.UserID);
            sqlBuilder.AppendLine(" SELECT Iscomment,OrderID,ProductId,SkuName,PicUrl,ProductName,CommentContent,CommentDT,CommentLevel,ProductCommentId ,MerchantID,ShopName FROM ( SELECT  ");
            sqlBuilder.AppendFormat(" *,ROW_NUMBER() OVER(ORDER BY CommentDT DESC) AS rid FROM tempTable)AS a WHERE a.rid BETWEEN {0} AND {1} ", (model.page - 1) * model.pageSize + 1, model.page * model.pageSize);


            var queryResult = _database.RunSqlQuery(x => x.ToResultSets(sqlBuilder.ToString()));
            dynamic source = queryResult[0][0];
            List<dynamic> sources = queryResult[1];
            result.Data = new SimpleDataPagedList<OrderProductCommentView>(sources.ToEntity<OrderProductCommentView>(), model.page, model.pageSize, source.Count);
            result.IsValid = true;
            return result;
        }

        /// <summary>
        /// 根据商品ID和返回类型返回（好、中、差评总数）wuyf
        /// </summary>
        /// <param name="ProductId"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public ResultModel GetPaingCommentCount(long ProductId,int type)
        {
            var result = new ResultModel();
            var tb=base._database.Db.SP_ProductComment;
            if (type==1)
            {
                //好评总数
                result.Data = tb.GetCount(tb.ProductId == ProductId && tb.CheckStatus ==2 && tb.CommentLevel >= 4);
            }
            else if (type == 2)
            {
                //中评总数
                result.Data = tb.GetCount(tb.ProductId == ProductId && tb.CheckStatus == 2 && tb.CommentLevel >= 2 && tb.CommentLevel < 4);
            }
            else if (type == 3)
            {
                //差评总数
                result.Data = tb.GetCount(tb.ProductId == ProductId && tb.CheckStatus == 2 && tb.CommentLevel >= 0 && tb.CommentLevel < 2);
            }
            else
            {
                result.Data = tb.GetCount(tb.ProductId == ProductId && tb.CheckStatus == 2);
            }
           

            return result;
            

            
        }

    }
}
