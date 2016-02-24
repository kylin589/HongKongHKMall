using BrCms.Framework.Collections;
using HKTHMall.Domain.AdminModel;
using HKTHMall.Domain.AdminModel.Products;
using HKTHMall.Domain.Models;
using Simple.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simple.Data.RawSql;
using HKTHMall.Core.Extensions;

namespace HKTHMall.Services.Products
{
    public class ProductConsultService : BaseService, IProductConsultService
    {
        /// <summary>
        /// 分页获取商品咨询信息.update by liujc
        /// zhoub 20150827
        /// </summary>
        /// <param name="model">用户搜索模型</param>
        /// <returns>用户列表数据</returns>
        public ResultModel GetPagingProductConsult(SearchProductConsultModel model, int languageID)
        {
            var pro = base._database.Db.ProductConsult;
            var where = new SimpleExpression(1, 1, SimpleExpressionType.Equal);

            if (model.UserID != 0)
            {
                //用户ID
                where = new SimpleExpression(where, pro.UserID == model.UserID, SimpleExpressionType.And);
            }
            if (model.ProductId != 0)
            {
                //商品id
                where = new SimpleExpression(where, pro.ProductId == model.ProductId, SimpleExpressionType.And);
            }
            if (model.ProductConsultId != 0)
            {
                where = new SimpleExpression(where, pro.ProductConsultId == model.ProductConsultId, SimpleExpressionType.And);
            }
            if (!string.IsNullOrEmpty(model.Phone))
            {
                where = new SimpleExpression(where, _database.Db.YH_User.Phone == model.Phone.Trim(), SimpleExpressionType.And);
            }

            dynamic cl;
            dynamic pc;

            var query = pro.Query()
                .LeftJoin(_database.Db.Product_Lang, out cl).On(_database.Db.Product_Lang.ProductId == pro.ProductId && _database.Db.Product_Lang.LanguageID == languageID)
                .LeftJoin(_database.Db.YH_User, out pc).On(_database.Db.YH_User.UserID == pro.UserID)
                .Select(pro.ProductConsultId,
                    pro.ProductId,
                    pro.UserID,
                    pro.ConsultContent,
                    pro.ConsultDT,
                    pro.ReplyBy,
                    pro.ReplyDT,
                    pro.ReplyContent,
                    cl.ProductName,
                    pc.Phone,
                    pc.Account,
                    pro.ConsultType
                )
                .Where(where)
                .OrderByConsultDTDescending();

            var result = new ResultModel()
            {
                Data = new SimpleDataPagedList<ProductConsultModel>(query, model.PagedIndex, model.PagedSize)

            };
            return result;
        }       

        /// <summary>
        /// 商品咨询回复
        /// zhoub 20150827
        /// </summary>
        /// <param name="model"></param>
        /// <param name="languageID"></param>
        /// <returns></returns>
        public ResultModel ReplyProductConsult(ProductConsultModel model, int languageID)
        {
            ResultModel result=new ResultModel();
            result.Data=_database.Db.ProductConsult.UpdateByProductConsultId(ProductConsultId:model.ProductConsultId,ReplyBy:model.ReplyBy,ReplyDT:model.ReplyDT,ReplyContent:model.ReplyContent);
            if (result.Data > 0)
            {
                result.IsValid = true;
            }
            else
            {
                result.IsValid = false;
            }
            return result;
        }

        /// <summary>
        /// 商品咨询删除
        /// zhoub 20150827
        /// </summary>
        /// <param name="productConsultId"></param>
        /// <param name="languageID"></param>
        /// <returns></returns>
        public ResultModel DeleteProductConsult(long productConsultId, int languageID)
        {
            var result = new ResultModel()
            {
                Data = base._database.Db.ProductConsult.Delete(ProductConsultId: productConsultId)
            };
            result.IsValid = result.Data > 0 ? true : false;
            return result;
        }

        /// <summary>
        /// 添加商品咨询
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ResultModel AddConsult(ProductConsult model)
        {
            var result = new ResultModel
            {
                Data = _database.Db.ProductConsult.Insert(model)
            };
            return result;
        }

        /// <summary>
        /// 商品问题咨询列表.update by liujc
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ResultModel GetConsulList(SearchConsle model, out int count)
        {
            var pro = base._database.Db.ProductConsult;    
             ResultModel result = new ResultModel();
             int PageIndex = model.Page;
             int PageSize = model.PageSize;
             dynamic cl;
             dynamic pc;
             var query = pro.Query()
                  .LeftJoin(_database.Db.Product_Lang, out cl).On(_database.Db.Product_Lang.ProductId == pro.ProductId && _database.Db.Product_Lang.LanguageID == model.languageID)
                 .LeftJoin(_database.Db.YH_User, out pc).On(_database.Db.YH_User.UserID == pro.UserID)     
                .Select(pro.ProductConsultId,
                    pro.ProductId,
                    pro.UserID,
                    pro.ConsultContent,
                    pro.ConsultDT,
                    pro.ReplyBy,
                    pro.ReplyDT,
                    pro.ReplyContent,
                    pro.IsAnonymous,
                    pc.Phone,
                    pc.Account,
                    pc.NickName,
                    pro.ConsultType
                ).Where(pro.ProductId == model.productId).OrderByConsultDTDescending();

             count = query.ToList<ProductConsultModel>().Count;
             if (PageIndex == 1)
             {
                 result.Data = query.Take(PageSize).ToList<ProductConsultModel>();
             }
             else
             {
                 result.Data = query.Skip((PageIndex - 1) * PageSize).Take(PageSize).ToList<ProductConsultModel>();
             }
            return result;
        }


        /// <summary>
        /// 获取咨询总数.update by liujc
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public ResultModel GetConsultCount(long productId)
        {
            var pro = base._database.Db.ProductConsult;    
            ResultModel result=new ResultModel();
            result.Data = pro.Query().Select(pro.ProductConsultId,
                pro.ProductId,
                pro.UserID,
                pro.ConsultContent,
                pro.ConsultDT,
                pro.ReplyBy,
                pro.ReplyDT,
                pro.ReplyContent,
                pro.IsAnonymous,
                pro.ConsultType).
                Where(pro.ProductId == productId).ToList().Count;
             
            return result;
        }

        /// <summary>
        /// 获取咨询总数.add by liujc
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public ResultModel GetConsultCountGroup(long productId)
        {
            ResultModel result = new ResultModel();

            var sql = @"select ConsultType as contype,count(*) AllCount from ProductConsult WHERE ProductId=" + productId.ToString() + " group by ConsultType order by ConsultType asc";
            List<dynamic> res = this._database.RunSqlQuery(x => x.ToResultSets(sql))[0];
            List<GroupModel> list = res.ToEntity<GroupModel>();
            result.Data = list;
            return result;
        }

        /// <summary>
        /// 商品问题咨询列表.add by liujc
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ResultModel GetConsulList(SearchConsle model)
        {
            StringBuilder builder = new StringBuilder(" where pro.ProductId=" + model.productId.ToString());
            if(model.contype>1 && model.contype<7)
            {
                builder.Append(" and pro.ConsultType=" + model.contype.ToString());
            }
            else if (model.contype == 7)
            {
                builder.Append(" and pro.ConsultType=7");
            }
            else { }

            string sql = @"select ProductConsultId,ProductId,UserID,ConsultContent,ConsultDT,ReplyBy,ReplyDT,ReplyContent,IsAnonymous,Phone,Account,NickName,ConsultType,
                        (select count(*) from  UserConsult where ConsultId=x.ProductConsultId and IsGood=1) as IsGood,
                        (select count(*) from  UserConsult where ConsultId=x.ProductConsultId and IsGood=-1) as NotGood
                    from (select ROW_NUMBER()over(order by pro.ConsultDT desc) as row,pro.ProductConsultId,pro.ProductId,pro.UserID,pro.ConsultContent,pro.ConsultDT,pro.ReplyBy,pro.ReplyDT,pro.ReplyContent,pro.IsAnonymous,pc.Phone,pc.Account,pc.NickName,pro.ConsultType 
                    from ProductConsult pro left join Product_Lang c1 on pro.ProductId=c1.ProductId and c1.LanguageID=" + model.languageID +
                    " left join YH_User pc on pc.UserID=pro.UserID" + builder.ToString() + ") as x" +
                    " where row between " + ((model.Page - 1) * model.PageSize + 1).ToString() + " and " + (model.Page * model.PageSize).ToString() +
                    " order by ConsultDT desc";

            ResultModel result = new ResultModel();
            List<dynamic> res = this._database.RunSqlQuery(x => x.ToResultSets(sql))[0];
            List<ProductConsultModel> list = res.ToEntity<ProductConsultModel>();
            result.Data = list;
            return result;


            //var pro = base._database.Db.ProductConsult;
            //ResultModel result = new ResultModel();
            //int PageIndex = model.Page;
            //int PageSize = model.PageSize;
            //dynamic cl;
            //dynamic pc;

            //var where = new SimpleExpression(1, 1, SimpleExpressionType.Equal);
            //where = new SimpleExpression(where, pro.ProductId == model.productId, SimpleExpressionType.And);
            //if(model.contype>1 && model.contype<7)
            //{
            //    where = new SimpleExpression(where, pro.ConsultType == model.contype, SimpleExpressionType.And);
            //}
            //else if (model.contype == 7)
            //{
            //    where = new SimpleExpression(where, pro.ConsultType <= 1, SimpleExpressionType.And);
            //    where = new SimpleExpression(where, pro.ConsultType >= 7, SimpleExpressionType.And);
            //}
            //else { }
            //var query = pro.Query()
            //     .LeftJoin(_database.Db.Product_Lang, out cl).On(_database.Db.Product_Lang.ProductId == pro.ProductId && _database.Db.Product_Lang.LanguageID == model.languageID)
            //    .LeftJoin(_database.Db.YH_User, out pc).On(_database.Db.YH_User.UserID == pro.UserID)
            //   .Select(pro.ProductConsultId,
            //       pro.ProductId,
            //       pro.UserID,
            //       pro.ConsultContent,
            //       pro.ConsultDT,
            //       pro.ReplyBy,
            //       pro.ReplyDT,
            //       pro.ReplyContent,
            //       pro.IsAnonymous,
            //       pc.Phone,
            //       pc.Account,
            //       pc.NickName,
            //       pro.ConsultType
            //   ).Where(where).OrderByConsultDTDescending();

            //if (PageIndex == 1)
            //{
            //    result.Data = query.Take(PageSize).ToList<ProductConsultModel>();
            //}
            //else
            //{
            //    result.Data = query.Skip((PageIndex - 1) * PageSize).Take(PageSize).ToList<ProductConsultModel>();
            //}
            //return result;
        }


        /// <summary>
        /// 添加商品咨询点赞,add by liujc
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ResultModel AddUserConsult(UserConsult model)
        {
            var result = new ResultModel();
            var uc = _database.Db.UserConsult.FindBy(ConsultID: model.ConsultID, UserID: model.UserID);
            result.Status = model.IsGood;
            if (uc != null)
            {
                if(uc.IsGood == model.IsGood)
                {
                    result.Status = 0;//不更改
                    return result;
                }

                model.ID = uc.ID;
                result.Data = _database.Db.UserConsult.Update(model);
            }
            else
            {
                result.Data = _database.Db.UserConsult.Insert(model);
            }
            return result;
        }
    }
}
