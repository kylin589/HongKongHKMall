using BrCms.Framework.Collections;
using HKTHMall.Domain.Enum;
using HKTHMall.Domain.Models;
using HKTHMall.Domain.Models.banner;
using Simple.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Services.Banner
{
    public class bannerProductService : BaseService, IbannerProductService
    {
        /// <summary>
        /// 添加banner图片
        /// </summary>
        /// <param name="model">用banner图片模型</param>
        /// <returns>是否成功</returns>

        public ResultModel AddBannerProduct(bannerProductModel model)
        {

            var result = new ResultModel()
            {
                Data = base._database.Db.bannerProduct.Insert(model)
            };

            return result;
        }

        /// <summary>
        /// 根据banner图片id获取banner图片
        /// </summary>
        /// <param name="id">banner图片id</param>
        /// <returns>banner图片模型</returns>
        public ResultModel GetBannerProductById(int bannerProductId)
        {

            var result = new ResultModel()
            {
                Data = base._database.Db.bannerProduct.Get(bannerProductId)
            };

            return result;
        }

        /// <summary>
        /// 测试用的
        /// </summary>
        /// <param name="bannerProductId"></param>
        /// <returns></returns>
        public ResultModel GetBannerProductAndByProductId(int bannerProductId)
        {
            var result = new ResultModel();
            var query = base._database.Db.BannerProduct.All().With(base._database.Db.Brand.Brand_Lang).Where(base._database.Db.BannerProduct.bannerProductId == bannerProductId);
            result.Data = query.ToList<bannerProductModel>();
            return result;

        }



        /// <summary>
        /// 获取banner图片列表
        /// </summary>
        /// <returns>banner图片列表</returns>
        public ResultModel GetBannerProduct(SearchbannerProductModel model)
        {

            var tb = base._database.Db.bannerProduct;
            var where = new SimpleExpression(1, 1, SimpleExpressionType.Equal);
            //商品状态
            where = new SimpleExpression(where, _database.Db.Product.Status == 4, SimpleExpressionType.And);

            if (model.PlaceCode > 0)
            {
                where = new SimpleExpression(where, tb.PlaceCode == model.PlaceCode, SimpleExpressionType.And);//位置（分类）（代表页面里的某个部位）
            }

            if (model.IdentityStatus > 0)
            {
                //标识ID （代表页面）
                where = new SimpleExpression(where, tb.IdentityStatus == model.IdentityStatus, SimpleExpressionType.And);
            }

            var result = new ResultModel();

            dynamic pc;

            var query = _database.Db.bannerProduct
                .Query()
                .LeftJoin(_database.Db.Product, out pc)
                .On(_database.Db.Product.ProductId == _database.Db.bannerProduct.ProductId)
                .Select(
                    tb.productId,
                    tb.PicAddress,
                    tb.Sorts,
                    tb.PlaceCode,
                    tb.IdentityStatus,
                    tb.CreateBy,
                    tb.CreateDT,
                    tb.UpdateBy,
                    tb.UpdateDT,
                    tb.bannerProductId,

                    pc.HKPrice,
                    pc.CategoryId

                )

                //.Where(where);
                //.Where(cl.languageId == null || cl.languageId == model.LanguageID)
                .Where(where)
                .OrderBySorts()
                .ToList<bannerProductModel>()
                ;

            result.Data = query;

            //var result = new ResultModel()
            //{
            //    Data = new SimpleDataPagedList<bannerProductModel>(base._database.Db.bannerProduct.FindAll(where).OrderBySorts(), model.PagedIndex, model.PagedSize)

            //};

            return result;
        }

        /// <summary>
        /// 根据Sorts查询的上一条或者下一条
        /// </summary>
        /// <param name="Sorts"></param>
        /// <param name="sx"></param>
        /// <returns></returns>
        public ResultModel GetBannerProduct(long Sorts, int sx, int IdentityStatus, long bannerId, int LanguageID, int BannerPlaceCode)
        {
            var tb = base._database.Db.bannerProduct;
            var where = new SimpleExpression(1, 1, SimpleExpressionType.Equal);
            where = new SimpleExpression(where, tb.IdentityStatus == IdentityStatus, SimpleExpressionType.And);
            where = new SimpleExpression(where, tb.bannerProductId != bannerId, SimpleExpressionType.And);
            //1上移动,2下移动
            if (sx == 1)
            {
                where = new SimpleExpression(where, tb.Sorts >= Sorts, SimpleExpressionType.And);
            }
            else
            {
                where = new SimpleExpression(where, tb.Sorts <= Sorts, SimpleExpressionType.And);
            }

            if (IdentityStatus > 1)
            {
                //只有多楼层才需要有这个条件,没有楼层区分的不需要
                where = new SimpleExpression(where, tb.PlaceCode == BannerPlaceCode, SimpleExpressionType.And);
            }

            if (LanguageID != null && LanguageID > 0)
            {
                where = new SimpleExpression(where, _database.Db.Product_Lang.languageId == LanguageID, SimpleExpressionType.And);
            }
            else
            {
                where = new SimpleExpression(where, _database.Db.Product_Lang.languageId == 1, SimpleExpressionType.And);
            }

            var result = new ResultModel();
            dynamic cl;
            dynamic pc;

            var query = _database.Db.bannerProduct
                //.FindAllBybannerProductId(1215865440)
                .Query()
                //.FindAll()
                .LeftJoin(_database.Db.Product_Lang, out cl)
                .On(_database.Db.Product_Lang.ProductId == _database.Db.bannerProduct.ProductId)
                .LeftJoin(_database.Db.Product, out pc)
                .On(_database.Db.Product.ProductId == _database.Db.bannerProduct.ProductId)
                .Select(
                    tb.productId,
                    tb.PicAddress,
                    tb.Sorts,
                    tb.PlaceCode,
                    tb.IdentityStatus,
                    tb.CreateBy,
                    tb.CreateDT,
                    tb.UpdateBy,
                    tb.UpdateDT,
                    tb.bannerProductId,
                    cl.ProductName,
                    cl.LanguageID,
                    pc.HKPrice,
                    pc.CategoryId

                )

                //.Where(where);
                //.Where(cl.languageId == null || cl.languageId == model.LanguageID)
                .Where(where)
                .OrderBySorts()
                .ToList<bannerProductModel>()
                ;
            //query = query.Skip(model.PagedIndex * model.PagedSize).Take(model.PagedSize).ToList<bannerProductModel>();
            //total = query.Count;


            result.Data = query;


            //var result = new Result()
            //{
            //    Data = new SimpleDataPagedList<bannerProductModel>(base._database.Db.bannerProduct.FindAll(where).OrderBySortsDescending(), 0, 10)

            //};

            return result;
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns>列表</returns>
        public ResultModel GetBannerProduct(SearchbannerProductModel model, out int total)
        {
            dynamic cl;
            dynamic pc;
            var tb = base._database.Db.bannerProduct;
            var where = new SimpleExpression(1, 1, SimpleExpressionType.Equal);
            //商品状态
            where = new SimpleExpression(where, _database.Db.Product.Status == 4, SimpleExpressionType.And);
            var result = new ResultModel();
            total = 0;
            if (model.PlaceCode > 0)
            {
                where = new SimpleExpression(where, tb.PlaceCode == model.PlaceCode, SimpleExpressionType.And);//位置（分类）（代表页面里的某个部位）
            }

            if (model.IdentityStatus > 0)
            {
                //标识ID （代表页面）
                where = new SimpleExpression(where, tb.IdentityStatus == model.IdentityStatus, SimpleExpressionType.And);
            }
            if (model.LanguageID > 0)
            {
                //选择语言
                where = new SimpleExpression(where, _database.Db.Product_Lang.languageId == model.LanguageID, SimpleExpressionType.And);
            }
            else
            {
                if (model.IdentityStatus > 1)
                {
                    //1不是楼层
                    where = new SimpleExpression(where, _database.Db.Product_Lang.languageId == 1, SimpleExpressionType.And);
                }

            }
            if (model.productId > 0)
            {
                where = new SimpleExpression(where, _database.Db.Product.productId == model.productId, SimpleExpressionType.And);
            }
            if (model.bannerProductId > 0)
            {
                where = new SimpleExpression(where, tb.bannerProductId == model.bannerProductId, SimpleExpressionType.And);
            }

            var query = _database.Db.bannerProduct
                //.FindAllBybannerProductId(1215865440)
                .Query()
                //.FindAll()
                .LeftJoin(_database.Db.Product_Lang, out cl)
                .On(_database.Db.Product_Lang.ProductId == _database.Db.bannerProduct.ProductId)
                .LeftJoin(_database.Db.Product, out pc)
                .On(_database.Db.Product.ProductId == _database.Db.bannerProduct.ProductId)
                .Select(
                    tb.productId,
                    tb.PicAddress,
                    tb.Sorts,
                    tb.PlaceCode,
                    tb.IdentityStatus,
                    tb.CreateBy,
                    tb.CreateDT,
                    tb.UpdateBy,
                    tb.UpdateDT,
                    tb.bannerProductId,
                    cl.ProductName,
                    cl.LanguageID,
                    pc.HKPrice,
                    pc.CategoryId,
                    pc.Status

                )

                //.Where(where);
                //.Where(cl.languageId == null || cl.languageId == model.LanguageID)
                .Where(where)
                .OrderBySorts()
                .ToList<bannerProductModel>()
                ;
            //query = query.Skip(model.PagedIndex * model.PagedSize).Take(model.PagedSize).ToList<bannerProductModel>();
            //total = query.Count;


            result.Data = query;
            //result.Data = new SimpleDataPagedList<bannerProductModel>(base._database.Db.bannerProduct.FindAll(where).OrderBySortsDescending(), model.PagedIndex, model.PagedSize);



            return result;
        }

        /// <summary>
        /// 更新banner图片
        /// </summary>
        /// <param name="model">后banner图片模型</param>
        /// <returns>是否修改成功</returns>
        public ResultModel UpdateBannerProduct(bannerProductModel model)
        {

            var result = new ResultModel();

            if (model.IdentityStatus == 2)
            {
                //修改PlaceCode 位置
                result.Data = base._database.Db.bannerProduct.UpdateBybannerProductId(bannerProductId: model.bannerProductId, productId: model.productId, PlaceCode: model.PlaceCode, PicAddress: model.PicAddress, UpdateBy: model.UpdateBy, UpdateDT: model.UpdateDT);
            }
            else
            {
                result.Data = base._database.Db.bannerProduct.UpdateBybannerProductId(bannerProductId: model.bannerProductId, productId: model.productId, PicAddress: model.PicAddress, UpdateBy: model.UpdateBy, UpdateDT: model.UpdateDT);
            }



            result.IsValid = result.Data > 0 ? true : false;
            return result;
        }

        /// <summary>
        /// 更新banner的Sorts
        /// </summary>
        /// <param name="model">需要上下移动的model</param>
        /// <param name="model">需要上下移动的model对应的上下行model</param>
        /// <returns>是否修改成功</returns>
        public ResultModel UpdateSorts(bannerProductModel model, bannerProductModel model1)
        {


            var result = new ResultModel();
            using (var tx = _database.Db.BeginTransaction())
            {
                try
                {
                    tx.bannerProduct.UpdateBybannerProductId(bannerProductId: model.bannerProductId, Sorts: model.Sorts, UpdateBy: model.UpdateBy, UpdateDT: model.UpdateDT);
                    tx.bannerProduct.UpdateBybannerProductId(bannerProductId: model1.bannerProductId, Sorts: model1.Sorts, UpdateBy: model1.UpdateBy, UpdateDT: model1.UpdateDT);
                    tx.Commit();
                }
                catch (Exception ex)
                {
                    tx.Rollback();

                    result.IsValid = false;
                    result.Messages.Add(ex.Message);
                }
            }
            return result;
        }

        /// <summary>
        /// 删除banner图片
        /// </summary>
        /// <param name="model">banner图片模型</param>
        /// <returns>是否删除成功</returns>
        public ResultModel DeleteBannerProduct(bannerProductModel model)
        {

            var result = new ResultModel()
            {
                Data = base._database.Db.bannerProduct.Delete(bannerProductId: model.bannerProductId)
            };

            result.IsValid = result.Data > 0 ? true : false;
            return result;
        }

        public ResultModel GetTopBanner(int topCount, int identityStatus, int placeCode,int Lang=4)
        {
            var tb = base._database.Db.bannerProduct;
            var tb2 = base._database.Db.Product;
            var tb3 = base._database.Db.Product_Lang;
            var where = new SimpleExpression(1, 1, SimpleExpressionType.Equal);
            where = new SimpleExpression(where, tb.IdentityStatus == identityStatus, SimpleExpressionType.And);
            where = new SimpleExpression(where, tb.PlaceCode == placeCode, SimpleExpressionType.And);
            where = new SimpleExpression(where, tb2.Status == ProductStatus.HasUpShelves, SimpleExpressionType.And);
            where = new SimpleExpression(where, tb3.LanguageID == Lang, SimpleExpressionType.And);
            var result = new ResultModel()
            {
                //Data = base._database.Db.bannerProduct.FindAll(where).Take(topCount).OrderBySortsDescending().ToList<bannerProductModel>()
                Data = base._database.Db.bannerProduct.FindAll(where)
                .Join(tb2)
                .On(tb2.ProductId == tb.ProductId)
                .Join(tb3)
                .On(tb3.ProductId == tb.ProductId)
                .Select(tb.productId, tb.PicAddress, tb2.HKPrice, tb3.ProductName)
                .OrderByCreateDTDescending().Take(topCount).ToList<bannerProductModel>()//刘宏文改
            };

            return result;
        }
        public ResultModel GetTopBannerForApi(int topCount, int identityStatus, int placeCode, int languageID)
        {
            var result = new ResultModel();
            dynamic pl;
            dynamic pc;
            var tb = base._database.Db.bannerProduct;
            var where = new SimpleExpression(1, 1, SimpleExpressionType.Equal);
            where = new SimpleExpression(where, tb.IdentityStatus == identityStatus, SimpleExpressionType.And);
            where = new SimpleExpression(where, tb.PlaceCode == placeCode, SimpleExpressionType.And);
            where = new SimpleExpression(where, _database.Db.Product.Status == 4, SimpleExpressionType.And);
            where = new SimpleExpression(where, _database.Db.Product_Lang.LanguageID == languageID, SimpleExpressionType.And);
            var query = _database.Db.bannerProduct
               .Query()
               .LeftJoin(_database.Db.Product_Lang, out pl)
               .On(_database.Db.Product_Lang.ProductId == _database.Db.bannerProduct.ProductId)
                .LeftJoin(_database.Db.Product, out pc)
                 .On(pc.ProductId == tb.ProductId)
               .Select(
                   tb.productId,
                   tb.PicAddress,
                   tb.Sorts,
                   tb.PlaceCode,
                   tb.IdentityStatus,
                   tb.CreateBy,
                   tb.CreateDT,
                   tb.UpdateBy,
                   tb.UpdateDT,
                   tb.bannerProductId,
                   pl.ProductName,
                   pl.LanguageID
               ).Where(where)
               .OrderBySorts().Take(topCount)
               .ToList<bannerProductModel>();
            result.Data = query;
            return result;
        }
    }
}