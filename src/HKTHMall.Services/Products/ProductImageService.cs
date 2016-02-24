using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKTHMall.Domain.Entities;
using HKTHMall.Domain.Models;
using BrCms.Framework.Collections;
using HKTHMall.Domain.AdminModel.Models.Products;
using Simple.Data;


namespace HKTHMall.Services.Products
{
    public class ProductImageService : BaseService, IProductImageService
    {
        /// <summary>
        /// 通过Id查询产品图对象
        /// </summary>
        /// <param name="id">主键Id</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-27</remarks>
        public ResultModel GetProductImageById(long id)
        {
            var result = new ResultModel() { Data = base._database.Db.ProductImage.FindByProductImageId(id) };
            return result;
        }

        /// <summary>
        /// 产品图对象分布查询
        /// </summary>
        /// <param name="model">输入查询参数对象</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-27</remarks>
        public ResultModel Select(SearchProductImageModel model)
        {
            //查询参数条件
            var whereParam = new SimpleExpression(1, 1, SimpleExpressionType.Equal);
            //产品名称
            if (!string.IsNullOrEmpty(model.ProductName))
            {
                whereParam = new SimpleExpression(whereParam, base._database.Db.ProductImage.ProductName.Like("%" + model.ProductName + "%"), SimpleExpressionType.And);
            }
            //产品类型
            if (model.ImageType != 0)
            {
                whereParam = new SimpleExpression(whereParam, base._database.Db.ProductImage.ImageType == model.ImageType, SimpleExpressionType.And);
            }

            var result = new ResultModel()
            {
                Data = new SimpleDataPagedList<ProductImageModel>(base._database.Db.ProductImage.FindAll(whereParam).OrderByplace().ThenByCreateDTDescending(),
                            model.PagedIndex, model.PagedSize)
            };
            return result;
        }

        /// <summary>
        /// 添加产品图
        /// </summary>
        /// <param name="model">产品图对象</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-27</remarks>
        public ResultModel Add(ProductImageModel model)
        {

            var result = new ResultModel { Data = _database.Db.ProductImage.Insert(model) };
            return result;
        }

        /// <summary>
        /// 通过Id删除产品图
        /// </summary>
        /// <param name="id">产品图id</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-27</remarks>
        public ResultModel Delete(long id)
        {
            var result = new ResultModel() { Data = base._database.Db.ProductImage.DeleteByProductImageId(id) };
            return result;
        }

        /// <summary>
        /// 更新产品图
        /// </summary>
        /// <param name="model">产品图对象</param>
        /// <returns>返回true时,表示更新成功；反之,表示更新失败</returns>
        /// <remarks>added by jimmy,2015-7-27</remarks>
        public ResultModel Update(ProductImageModel model)
        {
            dynamic record = new SimpleRecord();
            record.ProductImageId = model.ProductImageId;
            record.ProductName = model.ProductName;
            record.ImageUrl = model.ImageUrl;
            record.ImageType = model.ImageType;
            record.linkUrl = model.linkUrl;
            record.UpdateBy = model.UpdateBy;
            record.UpdateDT = model.UpdateDT;
            var result = new ResultModel() { Data = this._database.Db.ProductImage.UpdateByProductImageId(record) };
            return result;
        }

        /// <summary>
        /// 更新排序位置
        /// </summary>
        /// <param name="floorKeywordId">关键字管理Id</param>
        /// <param name="place">位置</param>
        /// <returns>返回true时,表示更新成功；反之,表示更新失败</returns>
        /// <remarks>added by jimmy,2015-7-27</remarks>
        public ResultModel UpdatePlace(long productImageId, int place)
        {
            var result = new ResultModel();
            var fc = this._database.Db.ProductImage;
            dynamic record = new SimpleRecord();
            record.productImageId = productImageId;
            record.place = place;
            result.Data = fc.UpdateByProductImageId(record);
            return result;
        }
    }
}
