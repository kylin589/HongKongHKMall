using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKTHMall.Domain.Entities;
using HKTHMall.Domain.Models;
using BrCms.Framework.Collections;
using HKTHMall.Domain.Models.Bra;
using Simple.Data;
using Simple.Data.RawSql;
using HKTHMall.Core.Extensions;

namespace HKTHMall.Services.Products
{
    public class BrandService : BaseService, IBrandService
    {
        /// <summary>
        /// 通过Id查询商品品牌对象
        /// </summary>
        /// <param name="id">主键Id</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-7</remarks>
        public ResultModel GetBrandById(int id)
        {
            var query = base._database.Db.Brand.All().With(base._database.Db.Brand.Brand_Lang).Where(base._database.Db.Brand.BrandID == id);
            var result = new ResultModel() { Data = query.ToList<BrandModel>() };
            return result;
        }

        /// <summary>
        /// 商品品牌对象分布查询
        /// </summary>
        /// <param name="model">输入查询参数对象</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-7</remarks>
        public ResultModel Select(SearchBrandModel model)
        {
            var whereParam = new SimpleExpression(1, 1, SimpleExpressionType.Equal);
            if (model.BrandState != 0)
            {
                whereParam = new SimpleExpression(whereParam, _database.Db.Brand.BrandState == model.BrandState, SimpleExpressionType.And);
            }
            if (!string.IsNullOrEmpty(model.BrandName))
            {
                whereParam = new SimpleExpression(whereParam,_database.Db.Brand.Brand_Lang.BrandName.Like("%" + model.BrandName + "%"), SimpleExpressionType.And);
            }
            var query = base._database.Db.Brand.All().
                With(base._database.Db.Brand.Brand_Lang).
                Where(base._database.Db.Brand.Brand_Lang.LanguageID == model.LanguageID).
                Where(whereParam).
                OrderByDescending(base._database.Db.Brand.AddTime);
            var result = new ResultModel()
            {
                Data = new SimpleDataPagedList<BrandModel>(query, model.PagedIndex, model.PagedSize)
            };
            return result;
        }

        /// <summary>
        /// 获取品牌列表
        /// </summary>
        /// <param name="languageId">语言</param>
        /// <returns></returns>
        public ResultModel GetAll(int languageId)
        {
            var result = new ResultModel();
            result.Data = base._database.RunQuery(db => db.Brand.All()
                .LeftJoin(db.Brand_Lang)
                .On(db.Brand_Lang.BrandID == db.Brand.BrandID && db.Brand_Lang.LanguageID == languageId)
                .Select(
                    db.Brand.BrandId,
                    db.Brand_Lang.BrandName
                ).ToList<BrandModel>());
            return result;
        }

        /// <summary>
        /// 添加商品品牌
        /// </summary>
        /// <param name="model">商品品牌对象</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-7</remarks>
        public ResultModel Add(BrandModel model)
        {
            var result = new ResultModel();
            using (var bt = this._database.Db.BeginTransaction())
            {
                try
                {
                    var brand = bt.Brand.Insert(model);
                    if (model.Brand_Lang.Any())
                    {
                        foreach (var item in model.Brand_Lang)
                        {
                            item.BrandID = brand.BrandID;
                        }
                        bt.Brand_Lang.Insert(model.Brand_Lang);
                    }
                    bt.Commit();
                    result.Data = brand;
                }
                catch (Exception ex)
                {
                    bt.Rollback();
                    result.IsValid = false;
                    result.Messages.Add(ex.Message);
                    throw;
                }
            }
            return result;
        }

        /// <summary>
        /// 通过Id删除商品品牌
        /// </summary>
        /// <param name="id">商品品牌id</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-7</remarks>
        public ResultModel Delete(int id)
        {
            var result = new ResultModel();
            using (var bt = this._database.Db.BeginTransaction())
            {
                try
                {
                    bt.Brand_Lang.DeleteByBrandID(id);
                    var brand = bt.Brand.DeleteByBrandID(id);
                    bt.Commit();
                    result.Data = brand;
                }
                catch (Exception ex)
                {
                    bt.Rollback();
                    result.IsValid = false;
                    result.Messages.Add(ex.Message);
                    throw;
                }
            }
            return result;
        }

        /// <summary>
        /// 根据一级分类ID和品牌ID，获取该品牌下的三级分类ID
        /// </summary>
        /// <param name="categoryId">一级分类ID</param>
        /// <param name="brandId">品牌ID</param>
        /// <returns></returns>
        public ResultModel GetThirdCategoryId(int categoryId, int brandId)
        {
            ResultModel model = new ResultModel();
            string sql = "select CategoryId from Brand_Category where BrandID=" + brandId + " and CategoryId in(select CategoryId from Category where parentId in(select CategoryId from Category where parentId=" + categoryId + "))";
            decimal cateId=_database.RunSqlQuery(x => x.ToScalar(sql));
            model.Data = cateId;
            model.IsValid = true;
            return model;
        }
        /// <summary>
        /// 更新商品品牌
        /// </summary>
        /// <param name="model">商品品牌对象</param>
        /// <returns>返回true时,表示更新成功；反之,表示更新失败</returns>
        /// <remarks>added by jimmy,2015-7-7</remarks>
        public ResultModel Update(BrandModel model)
        {
            var result = new ResultModel();
            using (var bt = this._database.Db.BeginTransaction())
            {
                try
                {
                    var brand = bt.Brand.UpdateByBrandID(BrandID: model.BrandID, BrandUrl: model.BrandUrl, Remark: model.Remark,
                        BrandState: model.BrandState, UpdateBy: model.UpdateBy, UpdateDT: model.UpdateDT);
                    var brandLang = bt.Brand_Lang.FindAll(bt.Brand_Lang.BrandID == model.BrandID).ToList<Brand_LangModel>();
                    if (model.Brand_Lang != null && brandLang != null)
                    {
                        for (int i = 0; i < brandLang.Count; i++)
                        {
                            for (int j = 0; j < model.Brand_Lang.Count; j++)
                            {
                                if (brandLang[i].LanguageID == model.Brand_Lang[j].LanguageID)
                                {
                                    brandLang[i].BrandName = model.Brand_Lang[j].BrandName;
                                    bt.Brand_Lang.UpdateById(brandLang[i]);
                                }
                            }
                        }
                    }
                    bt.Commit();
                    result.Data = brand;
                }
                catch (Exception ex)
                {
                    bt.Rollback();
                    result.IsValid = false;
                    result.Messages.Add(ex.Message);
                    throw;
                }
            }
            return result;
        }

        /// 更新商品品牌状态
        /// </summary>
        /// <param name="brandID">商品品牌ID</param>
        /// <param name="brandState">状态</param>
        /// <returns>返回true时,表示更新成功；反之,表示更新失败</returns>
        /// <remarks>added by jimmy,2015-7-7</remarks>
        public ResultModel UpdateState(int brandID, int brandState)
        {
            var result = new ResultModel();
            var brand = this._database.Db.Brand;
            dynamic record = new SimpleRecord();
            record.BrandID = brandID;
            record.BrandState = brandState;
            result.Data = brand.UpdateByBrandID(record);
            return result;
        }

        /// <summary>
        /// 根据分类Id获取品牌
        /// </summary>
        /// <param name="id">分类Id</param>
        /// <param name="languageId">语言Id</param>
        /// <returns>品牌列表</returns>
        public ResultModel GetBrandByCategoryId(int id, int languageId, int level = 3)
        {
            var result = new ResultModel();
            string str = id.ToString();
            if (level == 1)
            {
                str = "select CategoryId from Category where parentId in(select CategoryId from Category where parentId=" + id + ") ";
            }
            else if (level == 2)
            {
                str = "select CategoryId from Category where parentId=" + id;
            }
            string sql = "select d.BrandID,d.BrandUrl,d.BrandState,c.BrandName from(select a.BrandID,b.LanguageID,b.BrandName from Brand_Category a inner join Brand_Lang b on(a.BrandID=b.BrandID)where b.LanguageID="+languageId+" and CategoryId in(" + str + ")group by a.BrandID,b.LanguageID,b.BrandName)c inner join Brand d on c.BrandID=d.BrandID where d.BrandState=1";

            List<dynamic> sources = _database.RunSqlQuery(x => x.ToRows(sql));
            result.Data = sources.ToEntity<BrandModel>();

            //result.Data = this._database.RunQuery(db =>
            //{
            //    dynamic bc, bl;
            //    return db.Brand
            //        .Query()
            //        .Join(db.Brand_Category, out bc)
            //        .On(bc.BrandID == db.Brand.BrandID && bc.CategoryId == id)
            //        .LeftJoin(db.Brand_Lang.As("Brand_LangModel"), out bl)
            //        .On(bc.BrandID == bl.BrandID && bl.LanguageID == languageId)
            //        .WithOne(bl)
            //        .ToList<BrandModel>();
            //    ;
            //});

            return result;
        }
        /// <summary>
        /// 根据分类Id获取品牌
        /// </summary>
        /// <param name="id">分类Id</param>
        /// <param name="languageId">语言Id</param>
        /// <returns>品牌列表</returns>
        public ResultModel GetBrandByCategoryId(int id, int languageId)
        {
            var result = new ResultModel();
            result.Data = this._database.RunQuery(db =>
            {
                dynamic bc, bl;
                return db.Brand
                    .Query()
                    .Join(db.Brand_Category, out bc)
                    .On(bc.BrandID == db.Brand.BrandID && bc.CategoryId == id)
                    .LeftJoin(db.Brand_Lang.As("Brand_LangModel"), out bl)
                    .On(bc.BrandID == bl.BrandID && bl.LanguageID == languageId)
                    .WithOne(bl)
                    .ToList<BrandModel>();
                
            });
            return result;
        }
        /// <summary>
        /// 获取首页的楼层品牌（按添加时间倒序）
        /// 黄主霞 2016-01-15
        /// </summary>
        /// <param name="TopCount">前N个(null表示获取所有)</param>
        /// <param name="CategoryId">分类ID</param>
        /// <param name="Lang">语言：默认繁体</param>
        /// <returns></returns>
        public ResultModel GetTopBrandTimeDesc(int? TopCount, int[] CategoryIds, int Lang = 4)
        {
            var tbBrand = base._database.Db.Brand;
            var tbBrandCategory = base._database.Db.Brand_Category;
            var tbBrandLang = base._database.Db.Brand_Lang;
            var tbCategory = base._database.Db.Category;
            CategoryIds = CategoryIds.Length == 0 ? new int[] { -1 } : CategoryIds;
            ResultModel result = new ResultModel
            {
                Data = TopCount.HasValue ? tbBrand.Query().Join(tbBrandCategory).On(tbBrandCategory.BrandID == tbBrand.BrandID)
                .Join(tbBrandLang).On(tbBrandLang.BrandID == tbBrand.BrandID)
                .Where(tbBrand.BrandState == 1 && tbBrandLang.LanguageID == Lang && tbBrandCategory.CategoryId == CategoryIds).OrderByDescending(tbBrand.AddTime).Take(TopCount.Value)
                .Select(tbBrand.BrandID, tbBrandLang.BrandName, tbBrand.BrandUrl.As("PicUrl")).ToList<BrandAdvertiseModel>()
                :tbBrand.Query().Join(tbBrandCategory).On(tbBrandCategory.BrandID == tbBrand.BrandID)
                .Join(tbBrandLang).On(tbBrandLang.BrandID == tbBrand.BrandID)
                .Where(tbBrand.BrandState == 1 && tbBrandLang.LanguageID == Lang && tbBrandCategory.CategoryId == CategoryIds).OrderByDescending(tbBrand.AddTime)
                .Select(tbBrand.BrandID, tbBrandLang.BrandName, tbBrand.BrandUrl.As("PicUrl")).ToList<BrandAdvertiseModel>()
            };
            return result;
        }
    }
}
