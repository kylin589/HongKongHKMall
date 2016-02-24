using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrCms.Framework.Collections;
using HKTHMall.Core.Extensions;
using HKTHMall.Domain.Models;
using HKTHMall.Domain.Models.Categoreis;
using HKTHMall.Domain.WebModel.Models.Product;
using Simple.Data;
using Simple.Data.RawSql;

namespace HKTHMall.Services.WebProducts.Impl
{
    public class ProductCommentService : BaseService, IProductCommentService
    {
        /// <summary>
        /// 获取产品评价总数、好评数、中评数、差评数
        /// </summary>
        /// <param name="key">产品ID</param>
        /// <returns></returns>
        public ResultModel GetCount(long key)
        {
            StringBuilder sb = new StringBuilder();
            ResultModel result = new ResultModel { IsValid = false };
            ////获取产品评价总数
            //sb.AppendFormat("select count(1) as 'CommentTotal' from SP_ProductComment where CheckStatus=2 and ProductId={0};", key);
            ////获取产品差评数
            //sb.AppendFormat("select count(1) as 'CommentChaPing' from SP_ProductComment where CommentLevel <= 1 and CheckStatus=2 and ProductId={0};", key);
            ////获取产品中评数
            //sb.AppendFormat("select count(1) as 'CommentZhongPing' from SP_ProductComment where CommentLevel between 2 and 3 and CheckStatus=2 and ProductId={0};", key);
            ////获取产品好评数
            //sb.AppendFormat("select count(1) as 'CommentHaoPing' from SP_ProductComment where CommentLevel > 3 and CheckStatus=2 and ProductId={0};", key);
            string sql = @"select * from (select count(1) as 'CommentTotal' from SP_ProductComment where  ProductId={0} ) AS A,
(select count(1) as 'CommentChaPing' from SP_ProductComment where CommentLevel <= 1 and ProductId={0} ) AS B,
(select count(1) as 'CommentZhongPing' from SP_ProductComment where CommentLevel BETWEEN 2 AND 3 and ProductId={0}) AS C,
(select count(1) as 'CommentHaoPing' from SP_ProductComment where CommentLevel > 3 and ProductId={0}) AS D;";
            sb.AppendFormat(sql, key);
            var data = _database.RunSqlQuery(x => x.ToResultSets(sb.ToString()));
            List<dynamic> sources = data[0];
            result.Data = sources.ToEntity<CommentCount>();
            result.IsValid = true;
            return result;
        }


        /// <summary>
        /// 获取产品平均评星
        /// </summary>
        /// <param name="productId">产品ID</param>
        /// <returns></returns>
        public decimal GetProductCommentAvgRate(long productId)
        {
            try
            {
                var pc = base._database.Db.SP_ProductComment;
                var query = pc.All().
                    Select(pc.CommentLevel.Sum().As("TotalScore"),
                    pc.ProductCommentId.Count().As("TotalCount")).
                    Where(pc.ProductId == productId);
                var result = query.ToList<dynamic>()[0];

                return result["TotalScore"] / Convert.ToDecimal(result["TotalCount"]);
            }
            catch (Exception ex)
            {
                return Convert.ToDecimal(0);
            }
        }


        /// <summary>
        /// 获取商品评论列表
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns>商品评论列表</returns>
        public ResultModel GetProductCommentList(SearchSP_ProductCommentModel model)
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(" where 1=1");
            if (model.OrderId != 0) { builder.Append(" and tb.OrderId=" + model.OrderId.ToString()); }
            if (model.UserID != 0) { builder.Append(" and tb.UserID=" + model.UserID.ToString()); }
            if (model.ProductId != null && model.ProductId != 0) { builder.Append(" and tb.ProductId=" + model.ProductId.ToString()); }
            if (!string.IsNullOrEmpty(model.CheckBy)) { builder.Append(" and tb.CheckBy like '%" + model.CheckBy.Replace("'", "''") + "%'"); }
            if (model.BeginCommentDT != null) { builder.Append(" and tb.CommentDT>='" + model.BeginCommentDT.Value.ToString("yyyy-MM-dd") + "'"); }
            if (model.EndCommentDT != null) { builder.Append(" and tb.CommentDT<'" + model.EndCommentDT.Value.ToString("yyyy-MM-dd") + "'"); }
            if (model.BeginCheckDT != null) { builder.Append(" and tb.CheckDT>='" + model.BeginCheckDT.Value.ToString("yyyy-MM-dd") + "'"); }
            if (model.EndCheckDT != null) { builder.Append(" and tb.CheckDT<'" + model.EndCheckDT.Value.ToString("yyyy-MM-dd") + "'"); }
            if (model.CheckStatus != 0) { builder.Append(" and tb.CheckStatus=" + model.CheckStatus.ToString()); }
            if (model.LanguageID > 0) { builder.Append(" and c1.LanguageID=" + model.LanguageID.ToString()); }
            if (model.ProductCommentId > 0) { builder.Append(" and tb.ProductCommentId=" + model.ProductCommentId.ToString()); }
            if (model.CommentLevel > 0)
            {
                if (model.CommentLevel == 1) { builder.Append(" and tb.CommentLevel > 3"); }
                if (model.CommentLevel == 2) { builder.Append(" and tb.CommentLevel <= 3 and tb.CommentLevel >= 2"); }
                if (model.CommentLevel == 3) { builder.Append(" and tb.CommentLevel <= 1"); }
            }

            string startindex = (model.PagedIndex * model.PagedSize + 1).ToString();
            string endindex = ((model.PagedIndex + 1) * model.PagedSize).ToString();

            string sql = @"select ProductCommentId,OrderId,UserID,ProductId,CommentLevel,CommentContent,IsAnonymous,ReplyBy,ReplyDT,ReplyContent,CheckBy,Email,HeadImageUrl,
                            CheckStatus,CheckDT,CommentDT,ProductName,Account,Phone,PicUrl,(case when row between " + startindex + " and " + endindex + " then " +
                            @"stuff((select ','+convert(varchar(10),Labels) from SP_ProductComment_Labels where ProductCommentId = x.ProductCommentId for xml path('')),1,1,'') else 0 end) as LablesStr
                           from (select row_number() over(order by tb.CommentDT desc) as row,tb.*,c1.ProductName,pc.Account,pc.Phone,pp.PicUrl,pc.Email,pc.HeadImageUrl from 
                          SP_ProductComment tb left join Product_Lang c1 on c1.ProductId = tb.ProductId left join ProductPic pp on pp.ProductId = tb.ProductId and pp.Flag=1 left join YH_User pc on pc.UserID = tb.UserID " +
                         builder.ToString() + ") x";

            List<dynamic> res = this._database.RunSqlQuery(x => x.ToResultSets(sql))[0];
            List<ProductCommentModel> list = res.Skip(model.PagedIndex * model.PagedSize).Take(model.PagedSize).ToEntity<ProductCommentModel>();

            return new ResultModel()
            {
                Data = list
            };

            #region del by liujc
            /*
            var query = tb
                .Query()
                .LeftJoin(cl)
                .On(cl.ProductId == tb.ProductId)
                .LeftJoin(pp)
                .On(pp.ProductId == tb.ProductId)
                .LeftJoin(pc)
                .On(pc.UserID == tb.UserID)
                .Select(
                    tb.ProductCommentId,tb.OrderId,tb.UserID,tb.ProductId,tb.CommentLevel,tb.CommentContent,tb.IsAnonymous,tb.ReplyBy,tb.ReplyDT,tb.ReplyContent,tb.CheckBy,
                    tb.CheckStatus,tb.CheckDT,tb.CommentDT,cl.ProductName,pc.Account,pc.Phone, //商品评价人显示手机号
                    pp.PicUrl
                ).
                With(base._database.Db.SP_ProductComment.SP_ProductComment_Labels).
                Where(base._database.Db.SP_ProductComment.SP_ProductComment_Labels.ProductCommentId == base._database.Db.SP_ProductComment.ProductId)
                .Where(where)
                .OrderByCommentDTDescending();


            var tb = base._database.Db.SP_ProductComment;
            var cl = base._database.Db.Product_Lang;
            var pp = base._database.Db.ProductPic;
            var pc = base._database.Db.YH_User;
            //var where = new SimpleExpression(1, 1, SimpleExpressionType.Equal);

            //图片
            var where = new SimpleExpression(pp.Flag, 1, SimpleExpressionType.Equal);
            //dynamic cl;
            //dynamic pc;
            //dynamic pp;

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
            if (model.CheckBy != null && string.IsNullOrEmpty(model.CheckBy))
            {
                //审核人名称
                where = new SimpleExpression(where, tb.CheckBy.Like("%" + model.CheckBy + "%"), SimpleExpressionType.And);
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
                where = new SimpleExpression(where, cl.LanguageID == model.LanguageID, SimpleExpressionType.And);
            }



            if (model.ProductCommentId > 0)
            {
                //评论ID
                where = new SimpleExpression(where, tb.ProductCommentId == model.ProductCommentId, SimpleExpressionType.And);
            }

            if (model.CommentLevel > 0)
            {
                //评价等级（0全部,1好评,2中评,3差评）
                if (model.CommentLevel == 1)
                {
                    where = new SimpleExpression(where, tb.CommentLevel > 3, SimpleExpressionType.And);
                }
                if (model.CommentLevel == 2)
                {
                    where = new SimpleExpression(where, tb.CommentLevel <= 3, SimpleExpressionType.And);
                    where = new SimpleExpression(where, tb.CommentLevel >= 2, SimpleExpressionType.And);
                }
                if (model.CommentLevel == 3)
                {
                    where = new SimpleExpression(where, tb.CommentLevel <= 1, SimpleExpressionType.And);
                }
            }



            var query = tb
                .Query()
                .LeftJoin(cl)
                .On(cl.ProductId == tb.ProductId)
                .LeftJoin(pp)
                .On(pp.ProductId == tb.ProductId)
                .LeftJoin(pc)
                .On(pc.UserID == tb.UserID)
                .Select(
                    tb.ProductCommentId,tb.OrderId,tb.UserID,tb.ProductId,tb.CommentLevel,tb.CommentContent,tb.IsAnonymous,tb.ReplyBy,tb.ReplyDT,tb.ReplyContent,tb.CheckBy,
                    tb.CheckStatus,tb.CheckDT,tb.CommentDT,cl.ProductName,pc.Account,pc.Phone, //商品评价人显示手机号
                    pp.PicUrl
                ).
                .Where(where)
                .OrderByCommentDTDescending();

            var result = new ResultModel()
            {
                Data = new SimpleDataPagedList<ProductCommentModel>(query, model.PagedIndex, model.PagedSize)

            };

            return result;
             * */
            #endregion
        }

        /// <summary>
        /// 获取商品评论列表
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns>商品评论列表</returns>
        public ResultModel GetProductCommentList(SearchSP_ProductCommentModel model, out int TotalCnt)
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(" where 1=1");
            if (model.OrderId != 0) { builder.Append(" and tb.OrderId=" + model.OrderId.ToString()); }
            if (model.UserID != 0) { builder.Append(" and tb.UserID=" + model.UserID.ToString()); }
            if (model.ProductId != null && model.ProductId != 0) { builder.Append(" and tb.ProductId=" + model.ProductId.ToString()); }
            if (!string.IsNullOrEmpty(model.CheckBy)) { builder.Append(" and tb.CheckBy like '%" + model.CheckBy.Replace("'", "''") + "%'"); }
            if (model.BeginCommentDT != null) { builder.Append(" and tb.CommentDT>='" + model.BeginCommentDT.Value.ToString("yyyy-MM-dd") + "'"); }
            if (model.EndCommentDT != null) { builder.Append(" and tb.CommentDT<'" + model.EndCommentDT.Value.ToString("yyyy-MM-dd") + "'"); }
            if (model.BeginCheckDT != null) { builder.Append(" and tb.CheckDT>='" + model.BeginCheckDT.Value.ToString("yyyy-MM-dd") + "'"); }
            if (model.EndCheckDT != null) { builder.Append(" and tb.CheckDT<'" + model.EndCheckDT.Value.ToString("yyyy-MM-dd") + "'"); }
            if (model.CheckStatus != 0) { builder.Append(" and tb.CheckStatus=" + model.CheckStatus.ToString()); }
            if (model.LanguageID > 0) { builder.Append(" and c1.LanguageID=" + model.LanguageID.ToString()); }
            if (model.ProductCommentId > 0) { builder.Append(" and tb.ProductCommentId=" + model.ProductCommentId.ToString()); }
            if (model.CommentLevel > 0)
            {
                if (model.CommentLevel == 1) { builder.Append(" and tb.CommentLevel > 3"); }
                if (model.CommentLevel == 2) { builder.Append(" and tb.CommentLevel <= 3 and tb.CommentLevel >= 2"); }
                if (model.CommentLevel == 3) { builder.Append(" and tb.CommentLevel <= 1"); }
            }

            string startindex = (model.PagedIndex * model.PagedSize + 1).ToString();
            string endindex = ((model.PagedIndex + 1) * model.PagedSize).ToString();

            string sql = @"select ProductCommentId,OrderId,UserID,ProductId,CommentLevel,CommentContent,IsAnonymous,ReplyBy,ReplyDT,ReplyContent,CheckBy,Email,HeadImageUrl,
                            CheckStatus,CheckDT,CommentDT,ProductName,Account,Phone,PicUrl,(case when row between " + startindex + " and " + endindex + " then " +
                            @"stuff((select ','+convert(varchar(10),Labels) from SP_ProductComment_Labels where ProductCommentId = x.ProductCommentId for xml path('')),1,1,'') else null end) as LablesStr
                           from (select row_number() over(order by tb.CommentDT desc) as row,tb.*,c1.ProductName,pc.Account,pc.Phone,pp.PicUrl,pc.Email,pc.HeadImageUrl from 
                          SP_ProductComment tb left join Product_Lang c1 on c1.ProductId = tb.ProductId left join ProductPic pp on pp.ProductId = tb.ProductId and pp.Flag=1 left join YH_User pc on pc.UserID = tb.UserID " +
                         builder.ToString() + ") x";

            List<dynamic> res = this._database.RunSqlQuery(x => x.ToResultSets(sql))[0];
            List<ProductCommentModel> list = res.Skip(model.PagedIndex * model.PagedSize).Take(model.PagedSize).ToEntity<ProductCommentModel>();

            TotalCnt = res.Count;

            return new ResultModel()
            {
                Data = list
            };
        }

        /// <summary>
        /// 返回产品的印象标签汇总
        /// </summary>
        /// <param name="productid"></param>
        /// <returns></returns>
        public List<CommentLablesGroup> GroupLabelsByProduct(long productid)
        {
            var sql = "select y.Labels,COUNT(*) totalcnt from SP_ProductComment x inner join SP_ProductComment_Labels y on x.ProductCommentId=y.ProductCommentId where x.ProductId=" + productid.ToString() + " group by y.Labels";
            List<dynamic> res = this._database.RunSqlQuery(x => x.ToResultSets(sql))[0];
            List<CommentLablesGroup> list = res.ToEntity<CommentLablesGroup>();
            return list;
        }
    }
}
