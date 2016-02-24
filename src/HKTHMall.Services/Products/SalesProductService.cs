using HKTHMall.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKTHMall.Domain.AdminModel.Models.Products;
using BrCms.Framework.Collections;
using Simple.Data;

namespace HKTHMall.Services.Products
{
    public class SalesProductService : BaseService, ISalesProductService
    {
        /// <summary>
        /// 通过Id查询广告促销商品对象
        /// </summary>
        /// <param name="id">主键Id</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-8</remarks>
        public ResultModel GetSalesProductById(long id)
        {
            var salesProduct = _database.Db.SalesProduct;
            var productRule = _database.Db.ProductRule;
            dynamic pr;
            var query = salesProduct.FindAll(salesProduct.SalesProductId == id).
                LeftJoin(productRule, out pr).On(pr.ProductId == salesProduct.ProductId).
                Select(
                pr.Discount,
                salesProduct.SalesProductId,
                salesProduct.productId,
                salesProduct.PlaceCode,
                salesProduct.IdentityStatus,
                salesProduct.Sorts,
                salesProduct.PicAddress,
                salesProduct.CreateBy,
                salesProduct.CreateDT,
                salesProduct.UpdateBy,
                salesProduct.UpdateDT
                ).ToList<SalesProductModel>();
            var result = new ResultModel { Data = query };
            return result;
        }

        /// <summary>
        ///广告促销商品分页查询
        /// </summary>
        /// <param name="model">输入查询参数对象</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-8</remarks>
        public ResultModel Select(SearchSalesProductModel model)
        {
            var salesProduct = _database.Db.SalesProduct;
            var productlang = _database.Db.Product_Lang;
            var product = _database.Db.Product;
            var productRule = _database.Db.ProductRule;

            var whereParam = new SimpleExpression(1, 1, SimpleExpressionType.Equal);
            dynamic pl;
            dynamic pd;
            dynamic pr;
            var query = salesProduct.Query().
                LeftJoin(productlang, out pl).On(pl.ProductId == salesProduct.ProductId).
                Where(pl.LanguageID == model.LanguageID).
                LeftJoin(product, out pd).On(pd.ProductId == salesProduct.ProductId).
                LeftJoin(productRule, out pr).On(pr.ProductId == salesProduct.ProductId).
                Select(
                pl.ProductName.Distinct(),
                salesProduct.SalesProductId,
                salesProduct.productId,
                salesProduct.PlaceCode,
                salesProduct.IdentityStatus,
                (pd.HKPrice * pr.Discount).As("SalePrice"),
                pd.HKPrice,
                pd.Status,
                pr.SalesRuleId,
                pr.Discount,
                pr.StarDate,
                pr.EndDate,
                salesProduct.Sorts,
                salesProduct.PicAddress,
                salesProduct.CreateBy,
                salesProduct.CreateDT,
                salesProduct.UpdateBy,
                salesProduct.UpdateDT).
                OrderBy(salesProduct.Sorts).
                Where(pl.ProductName.Like("%" + model.ProductName + "%"));
            var result = new ResultModel
            {
                Data =
                    new SimpleDataPagedList<SalesProductModel>(query,
                        model.PagedIndex, model.PagedSize)
            };
            return result;
        }

        /// <summary>
        /// 添加广告促销商品
        /// </summary>
        /// <param name="model">广告促销商品对象</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-8</remarks>
        public ResultModel Add(SalesProductModel model)
        {
            var result = new ResultModel();
            var prodcut = _database.Db.ProductRule.Find(_database.Db.ProductRule.ProductId == model.productId && _database.Db.ProductRule.SalesRuleId == 2);
            if (prodcut == null)
            {
                result.IsValid = false;
                result.Messages = new List<string>() { "The promotion is not there, please fill in the number of other promotional items" };//该促销商品不存在,请填写别的促销商品编号
            }
            else
            {
                if (_database.Db.SalesProduct.Find(_database.Db.SalesProduct.ProductId == model.productId) != null)
                {
                    result.IsValid = false;
                    result.Messages = new List<string>() { "The promotion product already exists, can not repeat the addition!" };//该促销商品已经存在,不能重复添加！
                }
                else
                {
                    result.Data = _database.Db.SalesProduct.Insert(model);
                }
            };
            return result;
        }

        /// <summary>
        /// 通过Id删除广告促销商品
        /// </summary>
        /// <param name="id">系统参数id</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-8</remarks>
        public ResultModel Delete(long id)
        {
            var result = new ResultModel { Data = _database.Db.SalesProduct.DeleteBySalesProductId(id) };
            return result;
        }

        /// <summary>
        /// 更新广告促销商品
        /// </summary>
        /// <param name="model">广告促销商品对象</param>
        /// <returns>返回true时,表示更新成功；反之,表示更新失败</returns>
        /// <remarks>added by jimmy,2015-7-8</remarks>
        public ResultModel Update(SalesProductModel model)
        {
            var result = new ResultModel();
            var prodcut = _database.Db.ProductRule.FindByProductId(model.productId);
            var sp = _database.Db.SalesProduct;
            if (prodcut == null)
            {
                result.IsValid = false;
                result.Messages = new List<string>() { "The promotion is not there, please fill in the number of other promotional items" };//该促销商品不存在,请填写别的促销商品编号
            }
            else
            {
                result.Data = sp.UpdateBySalesProductId(SalesProductId: model.SalesProductId, productId: model.productId, PlaceCode: model.PlaceCode, IdentityStatus: model.IdentityStatus,
                PicAddress: model.PicAddress, UpdateBy: model.UpdateBy, UpdateDT: model.UpdateDT);

            };
            return result;
        }

        /// <summary>
        /// 更新排序位置
        /// </summary>
        /// <param name="SalesProductId">广告促销商品Id</param>
        /// <param name="place">位置</param>
        /// <returns>返回true时,表示更新成功；反之,表示更新失败</returns>
        /// <remarks>added by jimmy,2015-7-13</remarks>
        public ResultModel UpdatePlace(long SalesProductId, int place)
        {
            var result = new ResultModel();
            var fc = this._database.Db.SalesProduct;
            dynamic record = new SimpleRecord();
            record.SalesProductId = SalesProductId;
            record.Sorts = place;
            result.Data = fc.UpdateBySalesProductId(record);
            return result;
        }
    }
}
