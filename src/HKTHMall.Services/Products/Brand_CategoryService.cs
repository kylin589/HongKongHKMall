using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKTHMall.Domain.Entities;
using HKTHMall.Domain.Models;
using BrCms.Framework.Collections;
using HKTHMall.Domain.Models.Bra;
using HKTHMall.Domain.Models.Categoreis;
using Simple.Data;


namespace HKTHMall.Services.Products
{
    public class Brand_CategoryService : BaseService, IBrand_CategoryService
    {
        /// <summary>
        /// 品牌分类关联添加
        /// zhoub 20150709
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ResultModel AddBrand_Category(Brand_CategoryModel model)
        {
            ResultModel result = new ResultModel();
            var brand_Category = base._database.Db.Brand_Category.Find(base._database.Db.Brand_Category.BrandID == model.BrandID && base._database.Db.Brand_Category.CategoryId == model.CategoryId);
            if (brand_Category == null)
            { 
                result.IsValid = true;
                result.Data = base._database.Db.Brand_Category.Insert(model);
            }
            else
            {
                result.IsValid = false;
            }
            return result;
        }

        /// <summary>
        /// 品牌关联分页
        /// zhoub 20150709
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ResultModel GetPagingBrand_Category(SearchBrandModel model)
        {
            var whereExpr = base._database.Db.Brand_Category.BrandID == model.BrandID;
            whereExpr = new SimpleExpression(whereExpr, base._database.Db.Category_Lang.LanguageID == model.LanguageID, SimpleExpressionType.And);

            var result = new ResultModel()
            {
                Data =
                    new SimpleDataPagedList<Brand_Category>(base._database.Db.Brand_Category.All().Join(base._database.Db.Category_Lang).On(base._database.Db.Category_Lang.CategoryId == base._database.Db.Brand_Category.CategoryId).Select(base._database.Db.Brand_Category.Brand_CategoryId, base._database.Db.Category_Lang.CategoryName.As("AddUser")).Where(whereExpr),
                        model.PagedIndex, model.PagedSize)
            };
            return result;
        }


        /// <summary>
        /// 品牌关联删除
        /// zhoub 20150709
        /// </summary>
        /// <param name="brand_CategoryId"></param>
        /// <returns></returns>
        public ResultModel DeleteBrand_Category(string brand_CategoryId, string brandID)
        {
            ResultModel result = new ResultModel();
            var brand_Category = _database.Db.Brand_Category.FindByBrand_CategoryId(brand_CategoryId);
            if (brand_Category != null)
            {
                var product = _database.Db.Product.Find(_database.Db.Product.CategoryId == brand_Category.CategoryId && _database.Db.Product.BrandID == brandID);
                if (product == null)
                {
                    base._database.Db.Brand_Category.DeleteByBrand_CategoryId(brand_CategoryId);
                    result.IsValid = true;
                    result.Messages = new List<string>() { "Delete the success of brand classification" };//删除品牌分类关联成功
                }
                else
                {
                    result.IsValid = false;
                    result.Messages = new List<string>() { "To remove the brand name association failed, please remove the product from the brand." };//删除品牌分类关联失败,请先移除品牌下的商品.
                }
            }
            else {
                result.IsValid = false;
                result.Messages = new List<string>() { "Brand ID error." };//品牌分类ID错误.
            }
            return result;
        }
    }
}
