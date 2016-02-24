using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKTHMall.Domain.Models;
using HKTHMall.Domain.AdminModel.Models.Categoreis;
using BrCms.Framework.Collections;
using Simple.Data;

namespace HKTHMall.Services.Products
{
    public class FloorCategoryService : BaseService, IFloorCategoryService
    {
        /// <summary>
        /// 通过Id查询楼层显示分类对象
        /// </summary>
        /// <param name="id">主键Id</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-13</remarks>
        public ResultModel GetFloorCategoryById(long id)
        {
            var result = new ResultModel { Data = _database.Db.FloorCategory.FindByFloorCategoryId(id) };
            return result;
        }

        /// <summary>
        ///楼层显示分类分页查询
        /// </summary>
        /// <param name="model">输入查询参数对象</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-13</remarks>
        public ResultModel Select(SearchFloorCategoryModel model)
        {
            var floorCategory = _database.Db.FloorCategory;
            var categoryLang = _database.Db.Category_Lang;
            var category = _database.Db.Category;
            var whereParam = new SimpleExpression(1, 1, SimpleExpressionType.Equal);
            if (model.ParentID != 0)
            {
                whereParam = new SimpleExpression(whereParam, floorCategory.CategoryId == model.ParentID || floorCategory.DCategoryId == model.ParentID, SimpleExpressionType.And);
            }
            dynamic c;
            dynamic cl;
            dynamic cl3;
            var query = floorCategory.All().
                Join(categoryLang.As("categoryLang1"), out cl).On(floorCategory.DCategoryId == cl.CategoryId && cl.LanguageID == model.LanguageID).
                Join(categoryLang.As("categoryLang3"), out cl3).On(floorCategory.CategoryId == cl3.CategoryId && cl.LanguageID == model.LanguageID).
                 Join(category.As("c"), out c).On(floorCategory.CategoryId == c.CategoryId).
                Where(cl.LanguageID == model.LanguageID).
                Where(cl3.LanguageID == model.LanguageID).
                Where(c.AuditState == true).
                Select(floorCategory.FloorCategoryId, cl.CategoryName.As("CategoryNameFirst"), cl3.CategoryName.As("CategoryNameThree"), floorCategory.Place, floorCategory.AddUsers,
                floorCategory.AddTime).OrderByPlace().Where(whereParam);
            var result = new ResultModel
            {
                Data =
                    new SimpleDataPagedList<FloorCategoryModel>(query,
                        model.PagedIndex, model.PagedSize)
            };

            return result;
        }

        /// <summary>
        /// 添加楼层显示分类
        /// </summary>
        /// <param name="model">楼层显示分类对象</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-13</remarks>
        public ResultModel Add(FloorCategoryModel model)
        {
            var result = new ResultModel();
            var fc = _database.Db.FloorCategory.Find(_database.Db.FloorCategory.CategoryId == model.CategoryId);
            if (fc != null)
            {
                result.IsValid = false;
                result.Messages = new List<string>() { "This information already exists and cannot be repeated!" };
            }
            else
            {
                result.Data = _database.Db.FloorCategory.Insert(model);
            }
           
            return result;
        }

        /// <summary>
        /// 通过Id删除楼层显示分类
        /// </summary>
        /// <param name="id">系统参数id</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-13</remarks>
        public ResultModel Delete(long id)
        {
            var result = new ResultModel { Data = _database.Db.FloorCategory.DeleteByFloorCategoryId(id) };
            return result;
        }

        /// <summary>
        /// 更新楼层显示分类
        /// </summary>
        /// <param name="model">楼层显示分类对象</param>
        /// <returns>返回true时,表示更新成功；反之,表示更新失败</returns>
        /// <remarks>added by jimmy,2015-7-13</remarks>
        public ResultModel Update(FloorCategoryModel model)
        {
            var result = new ResultModel();
            var fc = this._database.Db.FloorCategory;
            dynamic record = new SimpleRecord();
            record.FloorCategoryId = model.FloorCategoryId;
            record.CategoryId = model.CategoryId;
            record.DCategoryId = model.DCategoryId;
            result.Data = fc.UpdateByFloorCategoryId(record);
            return result;
        }

        /// <summary>
        /// 更新排序位置
        /// </summary>
        /// <param name="floorCategoryId">楼层显示分类Id</param>
        /// <param name="place">位置</param>
        /// <returns>返回true时,表示更新成功；反之,表示更新失败</returns>
        /// <remarks>added by jimmy,2015-7-13</remarks>
        public ResultModel UpdatePlace(long floorCategoryId, int place)
        {
            var result = new ResultModel();
            var fc = this._database.Db.FloorCategory;
            dynamic record = new SimpleRecord();
            record.FloorCategoryId = floorCategoryId;
            record.Place = place;
            result.Data = fc.UpdateByFloorCategoryId(record);
            return result;
        }

        /// <summary>
        ///首页导航分页查询
        /// </summary>
        /// <param name="model">输入查询参数对象</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-15</remarks>
        public ResultModel GetFloorCategoryList(SearchFloorCategoryModel model)
        {
            var floorCategory = _database.Db.FloorCategory;
            var categoryLang = _database.Db.Category_Lang;
            var whereParam = new SimpleExpression(1, 1, SimpleExpressionType.Equal);
            if (model.ParentID != 0)
            {
                whereParam = new SimpleExpression(whereParam, floorCategory.CategoryId == model.ParentID || floorCategory.DCategoryId == model.ParentID, SimpleExpressionType.And);
            }

            dynamic cl;
           // var query = floorCategory.FindAll(floorCategory.DCategoryId == 0).OrderByPlace();

            var query = floorCategory.
                Query().
                LeftJoin(categoryLang, out cl).
                On(floorCategory.CategoryId == cl.CategoryId).
                Where(cl.LanguageID == model.LanguageID && floorCategory.DCategoryId == 0).
                Select(
                floorCategory.FloorCategoryId,
                cl.CategoryName.As("navigationName"),
                floorCategory.Place,
                floorCategory.AddUsers,
                floorCategory.AddTime).
                Where(whereParam).
                OrderByPlace();
            var result = new ResultModel
            {
                Data =
                    new SimpleDataPagedList<FloorCategoryModel>(query,
                        model.PagedIndex, model.PagedSize)
            };

            return result;
        }
    }
}
