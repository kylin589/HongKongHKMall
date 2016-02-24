using HKTHMall.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKTHMall.Domain.AdminModel.Models.Products;
using BrCms.Framework.Collections;
using HKTHMall.Domain.WebModel.Models.Index;
using Simple.Data;
using HKTHMall.Domain.Entities;
using HKTHMall.Domain.Enum;


namespace HKTHMall.Services.Products
{
    public class ProductRuleService : BaseService, IProductRuleService
    {
        /// <summary>
        /// 通过Id查询商品促销信息对象
        /// </summary>
        /// <param name="id">主键Id</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-10</remarks>
        public ResultModel GetProductRuleById(long id)
        {
            var result = new ResultModel { Data = _database.Db.ProductRule.FindByProductRuleId(id) };
            return result;
        }

        /// <summary>
        ///商品促销信息分页查询
        /// </summary>
        /// <param name="model">输入查询参数对象</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-10</remarks>
        public ResultModel Select(SearchProductRuleModel model)
        {
            var sr = _database.Db.ProductRule;//商品促销
            var product = _database.Db.Product;//订单对象
            var salesRule = _database.Db.SalesRule;//促销规则
            var productlang = _database.Db.Product_Lang;
            var whereParam = new SimpleExpression(1, 1, SimpleExpressionType.Equal);
            if (model.ProductId != 0)
            {
                whereParam = new SimpleExpression(whereParam, sr.ProductId == model.ProductId, SimpleExpressionType.And);
            }
            if (model.SalesRuleId != 0)
            {
                whereParam = new SimpleExpression(whereParam, sr.SalesRuleId == model.SalesRuleId, SimpleExpressionType.And);
            }
            dynamic pl;
            dynamic _srule;
            dynamic pd;
            var sd = sr.Query().
                LeftJoin(product, out pd).On(pd.ProductId == sr.ProductId).
                LeftJoin(salesRule, out _srule).On(_srule.SalesRuleId == sr.SalesRuleId).
                LeftJoin(productlang, out pl).On(pl.ProductId == sr.ProductId).
                Where(pl.LanguageID == model.LanguageID).
                Select(
                sr.ProductRuleId,
                pl.ProductName,
                sr.ProductId,
                salesRule.RuleName,
                salesRule.SalesRuleId,
                sr.PrdoctRuleName,
                sr.BuyQty,
                sr.SendQty,
                sr.Discount,
                pd.HKPrice,
                (pd.HKPrice * sr.Discount).As("SalePrice"),
                sr.Price,
                pd.Status,
                sr.StarDate,
                sr.EndDate).
                Where(sr.SalesRuleId > 1).
                Where(whereParam).
                OrderByProductRuleIdDescending();
            var result = new ResultModel
            {
                Data = new SimpleDataPagedList<ProductRuleModel>(sd,
                        model.PagedIndex, model.PagedSize)

            };
            return result;
        }

        /// <summary>
        /// 添加商品促销信息
        /// </summary>
        /// <param name="model">商品促销信息对象</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-10</remarks>
        public ResultModel Add(ProductRuleModel model)
        {
            var result = new ResultModel();
            var prodcut = _database.Db.Product.Find(_database.Db.Product.ProductId == model.ProductId && _database.Db.Product.IsDelete == 0);
            if (prodcut == null)
            {
                result.IsValid = false;
                result.Messages = new List<string>() { "The product information is not there, please fill in the other item number" };//该商品信息不存在,请填写别的商品编号
            }
            else if (prodcut.Status == (int)ProductStatus.ExaminationNotThrough)
            {
                result.IsValid = false;
                result.Messages = new List<string>() { "This product is not approved, and can not be used as a commodity promotion!" };//该商品没有审核通过,不能作为商品促销!
            }
            else if (prodcut.Status == (int)ProductStatus.HasUnderShelves)
            {
                result.IsValid = false;
                result.Messages = new List<string>() { "The goods have been under the shelf, can not be used as a commodity promotion!" };//该商品已下架,不能作为商品促销!
            }
            else if (prodcut.Status == (int)ProductStatus.Submitting)
            {
                result.IsValid = false;
                result.Messages = new List<string>() { "The commodity is in the state of the audit, not as a commodity promotion!" };//该商品处在待审核状态,不能作为商品促销!
            }
            else if (prodcut.Status == (int)ProductStatus.Uncommitted)
            {
                result.IsValid = false;
                result.Messages = new List<string>() { "The goods are not submitted, can not be used as a commodity promotion!" };//该商品未提交,不能作为商品促销!
            }
            else
            {
                if (_database.Db.ProductRule.Find(_database.Db.ProductRule.ProductId == model.ProductId && _database.Db.ProductRule.SalesRuleId == 2) != null)
                {
                    result.IsValid = false;
                    result.Messages = new List<string>() { "The product promotion information already exists, can not repeat to add!" };//该商品促销信息已经存在,不能重复添加！
                }
                else
                {
                    try
                    {
                        var pr = _database.Db.ProductRule.Find(_database.Db.ProductRule.ProductId == model.ProductId && _database.Db.ProductRule.SalesRuleId == 1);
                        if (pr != null)
                        {
                            result.Data = _database.Db.ProductRule.UpdateByProductRuleId(ProductRuleId: pr.ProductRuleId, SalesRuleId: model.SalesRuleId, StarDate: model.StarDate, EndDate: model.EndDate, Discount:model.Discount);
                        }
                        else
                        {
                            result.Data = _database.Db.ProductRule.Insert(model);
                        }

                    }
                    catch (Exception ex)
                    {
                        //todo错误日志
                        throw;
                    }

                }
            }
            return result;
        }

        /// <summary>
        /// 通过Id删除商品促销信息
        /// </summary>
        /// <param name="id">系统参数id</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-10</remarks>
        public ResultModel Delete(long id)
        {
            var result = new ResultModel { Data = _database.Db.ProductRule.UpdateByProductRuleId(ProductRuleId: id, SalesRuleId: 1) };
            return result;
        }

        /// <summary>
        /// 更新商品促销信息
        /// </summary>
        /// <param name="model">商品促销信息对象</param>
        /// <returns>返回true时,表示更新成功；反之,表示更新失败</returns>
        /// <remarks>added by jimmy,2015-7-10</remarks>
        public ResultModel Update(ProductRuleModel model)
        {
            var result = new ResultModel();
            var prodcut = _database.Db.Product.FindByProductId(model.ProductId);
            var sp = _database.Db.ProductRule;
            if (prodcut == null)
            {
                result.IsValid = false;
                result.Messages = new List<string>() { "Product not exist, cannot be promotion product" };
            }
            else
            {
                result.Data = sp.UpdateByProductRuleId(model);
            }
            return result;
        }

        /// <summary>
        /// 通过Id查询商品信息
        /// </summary>
        /// <param name="id">主键Id</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-10</remarks>
        public ResultModel GetProductById(long id)
        {
            return new ResultModel { Data = _database.Db.Product.FindByProductId(id) };
        }


        /// <summary>
        /// 获取首页显示爆款产品
        /// </summary> 
        /// <param name="languageID">加载的语言</param>
        /// <param name="topCount">取前面多少条数据</param>
        /// <param name="isToday">true:今天 false:预售</param>
        /// <returns></returns>
        public ResultModel GetIndexExplosion(int languageID, int topCount, bool isToday)
        {
            var result = new ResultModel();
            var tb = base._database.Db.ProductRule;
            var where = new SimpleExpression(1, 1, SimpleExpressionType.Equal);
            
            #region del by liujc
            /*注释掉。del by liujc
             * //    where = new SimpleExpression(where, tb.StarDate <= DateTime.Now, SimpleExpressionType.And);
            where = new SimpleExpression(where, tb.EndDate >= DateTime.Now, SimpleExpressionType.And);
            where = new SimpleExpression(where, base._database.Db.Product_Lang.LanguageID == languageID, SimpleExpressionType.And);
            where = new SimpleExpression(where, base._database.Db.Product.STATUS == 4, SimpleExpressionType.And);
            where = new SimpleExpression(where, base._database.Db.ProductRule.SalesRuleId > 1, SimpleExpressionType.And);//此句刘宏文加Take(10)  -- or .Top(10)

            result.Data = base._database.Db.ProductRule.ALL()
                .Join(base._database.Db.Product, ProductId: base._database.Db.ProductRule.ProductId)
                .Join(base._database.Db.Product_Lang, ProductId: base._database.Db.ProductRule.ProductId)
                .Join(base._database.Db.SalesProduct)
                .On(base._database.Db.SalesProduct.ProductId == base._database.Db.ProductRule.ProductId)
                .Where(where)
                .Select(base._database.Db.Product_Lang.ProductId, base._database.Db.Product_Lang.ProductName,
                        base._database.Db.Product.HKPrice, base._database.Db.Product.MarketPrice, base._database.Db.SalesProduct.PicAddress,
                        base._database.Db.ProductRule.Discount, base._database.Db.ProductRule.StarDate,
                        base._database.Db.ProductRule.EndDate, base._database.Db.SalesProduct.Sorts)
                .OrderBy(base._database.Db.SalesProduct.Sorts).Take(topCount)////Taketop(Count) 此句刘宏文加Take(10)  -- or .Top(10)
                .ToList<IndexExplosion>();
            
            if (explosionList.Count > 0)
            {
                ///计算时间间隔差距,以及活动价格
                explosionList.ForEach(a =>
                {
                    if (a.StarDate <= DateTime.Now)
                    {
                        a.Intervalsecond = (int)Math.Round((a.EndDate - DateTime.Now).TotalSeconds);
                        a.StartStatus = 1;
                    }
                    else
                    {
                        a.Intervalsecond = (int)Math.Round((a.StarDate - DateTime.Now).TotalSeconds);
                        a.StartStatus = 0;
                    } 
                    a.ActivityPrice = (a.HKPrice * a.Discount);
                });
            }
            */
            #endregion

            #region add by liujc

            where = new SimpleExpression(where, base._database.Db.Product_Lang.LanguageID == languageID, SimpleExpressionType.And);
            where = new SimpleExpression(where, base._database.Db.Product.STATUS == 4, SimpleExpressionType.And);
            if(isToday)
            {
                where = new SimpleExpression(where, tb.StarDate <= DateTime.Now, SimpleExpressionType.And);
                result.Data = base._database.Db.ProductRule.ALL()
                .Join(base._database.Db.Product, ProductId: base._database.Db.ProductRule.ProductId)
                .Join(base._database.Db.Product_Lang, ProductId: base._database.Db.ProductRule.ProductId)
                .Join(base._database.Db.SalesProduct)
                .On(base._database.Db.SalesProduct.ProductId == base._database.Db.ProductRule.ProductId)
                .Where(where)
                .Select(base._database.Db.Product_Lang.ProductId, base._database.Db.Product_Lang.ProductName,
                        base._database.Db.Product.HKPrice, base._database.Db.Product.MarketPrice, base._database.Db.SalesProduct.PicAddress,
                        base._database.Db.ProductRule.Discount, base._database.Db.ProductRule.StarDate,
                        base._database.Db.ProductRule.EndDate, base._database.Db.SalesProduct.Sorts,
                        base._database.Db.Product.RebateDays)
                .OrderBy(base._database.Db.ProductRule.SalesRuleId, OrderByDirection.Descending)
                .OrderBy(base._database.Db.ProductRule.StarDate, OrderByDirection.Descending)
                .OrderBy(base._database.Db.SalesProduct.Sorts).Take(topCount)
                .ToList<IndexExplosion>();
            }
            else
            {
                where = new SimpleExpression(where, tb.StarDate >= DateTime.Now, SimpleExpressionType.And);
                where = new SimpleExpression(where, base._database.Db.ProductRule.SalesRuleId > 1, SimpleExpressionType.And);
                result.Data = base._database.Db.ProductRule.ALL()
                .Join(base._database.Db.Product, ProductId: base._database.Db.ProductRule.ProductId)
                .Join(base._database.Db.Product_Lang, ProductId: base._database.Db.ProductRule.ProductId)
                .Join(base._database.Db.SalesProduct)
                .On(base._database.Db.SalesProduct.ProductId == base._database.Db.ProductRule.ProductId)
                .Where(where)
                .Select(base._database.Db.Product_Lang.ProductId, base._database.Db.Product_Lang.ProductName,
                        base._database.Db.Product.HKPrice, base._database.Db.Product.MarketPrice,  base._database.Db.SalesProduct.PicAddress,
                        base._database.Db.ProductRule.Discount, base._database.Db.ProductRule.StarDate,
                        base._database.Db.ProductRule.EndDate, base._database.Db.SalesProduct.Sorts,
                        base._database.Db.Product.RebateDays)
                .OrderBy(base._database.Db.ProductRule.StarDate)
                .OrderBy(base._database.Db.SalesProduct.Sorts).Take(topCount)
                .ToList<IndexExplosion>();
            }
            

            #endregion

            List<IndexExplosion> explosionList = result.Data;

            if (explosionList.Count > 0)
            {
                explosionList.ForEach(a =>
                {
                    if(a.EndDate<=DateTime.Now)
                    {
                        a.Intervalsecond = 0;
                        a.StartStatus = -1;//已结束
                    }
                    else if(a.StarDate>DateTime.Now)
                    {
                        a.Intervalsecond = (int)Math.Round((a.StarDate - DateTime.Now).TotalSeconds);
                        a.StartStatus = 0;//未开始
                    }
                    else
                    {
                        a.Intervalsecond = (int)Math.Round((a.EndDate - DateTime.Now).TotalSeconds);
                        a.StartStatus = 1;//爆款中
                    }
                    a.ActivityPrice = (a.HKPrice * a.Discount);
                });
            }

            result.Data = explosionList;
            return result;
        }

        /// <summary>
        /// 获取首页显示爆款产品
        /// </summary> 
        /// <param name="languageID">加载的语言</param>       
        /// <returns></returns>
        public ResultModel GetIndexExplosionForApi(int languageID)
        {
            var result = new ResultModel();
            var tb = base._database.Db.ProductRule;
            var where = new SimpleExpression(1, 1, SimpleExpressionType.Equal);
            //where = new SimpleExpression(where, tb.StarDate <= DateTime.Now, SimpleExpressionType.And);
            where = new SimpleExpression(where, tb.EndDate >= DateTime.Now, SimpleExpressionType.And);
            where = new SimpleExpression(where, base._database.Db.Product_Lang.LanguageID == languageID, SimpleExpressionType.And);
            where = new SimpleExpression(where, base._database.Db.Product.STATUS == 4, SimpleExpressionType.And);
            where = new SimpleExpression(where, base._database.Db.ProductRule.SalesRuleId > 1, SimpleExpressionType.And);


            result.Data = base._database.Db.ProductRule.ALL()
                .Join(base._database.Db.Product, ProductId: base._database.Db.ProductRule.ProductId)
                .Join(base._database.Db.Product_Lang, ProductId: base._database.Db.ProductRule.ProductId)
                .Join(base._database.Db.SalesProduct)
                .On(base._database.Db.SalesProduct.ProductId == base._database.Db.ProductRule.ProductId)
                .Where(where)
                .Select(base._database.Db.Product_Lang.ProductId, base._database.Db.Product_Lang.ProductName,
                        base._database.Db.Product.HKPrice, base._database.Db.Product.MarketPrice, base._database.Db.SalesProduct.PicAddress,
                        base._database.Db.ProductRule.Discount, base._database.Db.ProductRule.StarDate,
                        base._database.Db.ProductRule.EndDate)
                .OrderBy(base._database.Db.SalesProduct.Sorts)
                .ToList<IndexExplosion>();


            List<IndexExplosion> explosionList = result.Data;
            if (explosionList.Count > 0)
            {
                ///计算时间间隔差距,以及活动价格
                explosionList.ForEach(a =>
                {
                   // a.Intervalsecond = (int)Math.Round((a.EndDate - DateTime.Now).TotalSeconds);
                    a.ActivityPrice = (a.HKPrice * a.Discount);
                });
            }
            result.Data = explosionList;
            return result;
        }
        /// <summary>
        /// 获取产品爆款信息数据
        /// </summary>
        /// <param name="productid">产品Id</param>
        /// <param name="languageID">语言版本</param>
        /// <returns></returns>
        public ResultModel GetPromotionProductForId(long productid, int languageID)
        {
            var result = new ResultModel();
            var tb = base._database.Db.ProductRule;
            var tb2 = base._database.Db.Product;
            var tb3 = base._database.Db.Product_Lang;
            var tb4 = base._database.Db.SalesProduct;
            var where = new SimpleExpression(1, 1, SimpleExpressionType.Equal);
            where = new SimpleExpression(where, tb.SalesRuleId > 1, SimpleExpressionType.And);
            where = new SimpleExpression(where, tb.StarDate <= DateTime.Now, SimpleExpressionType.And);
            where = new SimpleExpression(where, tb.EndDate >= DateTime.Now, SimpleExpressionType.And);
            where = new SimpleExpression(where, tb3.LanguageID == languageID, SimpleExpressionType.And);
            where = new SimpleExpression(where, tb2.STATUS == 4, SimpleExpressionType.And);
            where = new SimpleExpression(where, tb2.ProductId == productid, SimpleExpressionType.And);

            result.Data = tb.Query()
                .Join(tb2, ProductId: tb.ProductId)
                .Join(tb3, ProductId: tb.ProductId)
                .LeftJoin(tb4, ProductId: tb.ProductId)
                .Where(where)
                .Select(tb3.ProductId, tb3.ProductName, tb2.HKPrice, tb2.MarketPrice, tb4.PicAddress,
                        tb.Discount, tb.StarDate, tb.EndDate)
                .ToList<IndexExplosion>();
            List<IndexExplosion> explosionList = result.Data;
            if (explosionList.Count > 0)
            {
                ///计算时间间隔差距,以及活动价格
                explosionList.ForEach(a =>
                {
                    a.Intervalsecond = (int)Math.Round((a.EndDate - DateTime.Now).TotalSeconds);
                    a.ActivityPrice = (decimal)Math.Round(a.HKPrice * a.Discount);
                });
            }
            result.Data = explosionList;
            return result;
        }
    }
}
