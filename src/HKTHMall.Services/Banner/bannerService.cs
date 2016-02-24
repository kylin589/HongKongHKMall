using System.Runtime.InteropServices;
using BrCms.Framework.Collections;
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
    public class bannerService : BaseService, IbannerService
    {
        /// <summary>
        /// 添加banner图片
        /// </summary>
        /// <param name="model">用banner图片模型</param>
        /// <returns>是否成功</returns>

        public ResultModel AddBanner(bannerModel model)
        {

            var result = new ResultModel()
            {
                Data = base._database.Db.banner.Insert(model)
            };

            return result;
        }

        /// <summary>
        /// 根据banner图片id获取banner图片
        /// </summary>
        /// <param name="id">banner图片id</param>
        /// <returns>banner图片模型</returns>
        public ResultModel GetBannerById(long bannerId)
        {

            var result = new ResultModel()
            {
                Data = base._database.Db.banner.Get(bannerId)
            };

            return result;
        }



        /// <summary>
        /// 获取banner图片列表
        /// </summary>
        /// <returns>banner图片列表</returns>
        public ResultModel GetBanner(SearchbannerModel model)
        {

            var tb = base._database.Db.banner;
            var where = new SimpleExpression(1, 1, SimpleExpressionType.Equal);

            if (model.PlaceCode > 0)
            {
                where = new SimpleExpression(where, tb.PlaceCode == model.PlaceCode, SimpleExpressionType.And);//位置（分类）（代表页面里的某个部位）
            }

            if (model.IdentityStatus > 0)
            {
                //标识ID （代表页面）
                where = new SimpleExpression(where, tb.IdentityStatus == model.IdentityStatus, SimpleExpressionType.And);
            }



            var result = new ResultModel()
            {
                //OrderByDescending(降序)-OrderBy(升序)
                Data = new SimpleDataPagedList<bannerModel>(base._database.Db.banner.FindAll(where).OrderBySorts(), model.PagedIndex, model.PagedSize)

            };

            return result;
        }

        /// <summary>
        /// 根据Sorts查询的上一条或者下一条
        /// </summary>
        /// <param name="Sorts"></param>
        /// <param name="sx"></param>
        /// <returns></returns>
        public ResultModel GetBanner(long Sorts, int sx, int IdentityStatus, long bannerId, int BannerPlaceCode)
        {
            var tb = base._database.Db.banner;
            var where = new SimpleExpression(1, 1, SimpleExpressionType.Equal);
            where = new SimpleExpression(where, tb.IdentityStatus == IdentityStatus, SimpleExpressionType.And);
            where = new SimpleExpression(where, tb.bannerId != bannerId, SimpleExpressionType.And);
            if (IdentityStatus > 1)
            {
                //没有楼层需求的,是不需要此条件,只有楼层才需要此条件
                where = new SimpleExpression(where, tb.PlaceCode == BannerPlaceCode, SimpleExpressionType.And);
            }
            //2上移动,1下移动
            if (sx == 1)
            {
                where = new SimpleExpression(where, tb.Sorts >= Sorts, SimpleExpressionType.And);
            }
            else
            {
                where = new SimpleExpression(where, tb.Sorts <= Sorts, SimpleExpressionType.And);
            }




            var result = new ResultModel()
            {
                Data = new SimpleDataPagedList<bannerModel>(base._database.Db.banner.FindAll(where).OrderBySorts(), 0, 1000)

            };

            return result;
        }

        /// <summary>
        /// 更新banner图片
        /// </summary>
        /// <param name="model">后banner图片模型</param>
        /// <returns>是否修改成功</returns>
        public ResultModel UpdateBanner(bannerModel model)
        {

            var result = new ResultModel()
            {
                Data = base._database.Db.banner.UpdateBybannerId(bannerId: model.bannerId, bannerName: model.bannerName, bannerPic: model.bannerPic, bannerUrl: model.bannerUrl, ProductId: model.ProductId, UpdateBy: model.UpdateBy, UpdateDT: model.UpdateDT)
            };

            result.IsValid = result.Data > 0 ? true : false;
            return result;
        }

        /// <summary>
        /// 更新banner图片
        /// </summary>
        /// <param name="model">后banner图片模型</param>
        /// <returns>是否修改成功</returns>
        public ResultModel UpdateThreeBanner(bannerModel model)
        {

            var result = new ResultModel()
            {
                Data = base._database.Db.banner.UpdateBybannerId(bannerId: model.bannerId, bannerName: model.bannerName, bannerPic: model.bannerPic, bannerUrl: model.bannerUrl, ProductId: model.ProductId, UpdateBy: model.UpdateBy, UpdateDT: model.UpdateDT,IdentityStatus: model.IdentityStatus)
            };

            result.IsValid = result.Data > 0 ? true : false;
            return result;
        }
        

        /// <summary>
        /// 更新banner的Sorts
        /// </summary>
        /// <param name="model">需要上下移动的model</param>
        /// <param name="model">需要上下移动的model对应的上下行model</param>
        /// <returns>是否修改成功</returns>
        public ResultModel UpdateSorts(bannerModel model, bannerModel model1)
        {


            var result = new ResultModel();
            using (var tx = _database.Db.BeginTransaction())
            {
                try
                {
                    tx.banner.UpdateBybannerId(bannerId: model.bannerId, Sorts: model.Sorts, UpdateBy: model.UpdateBy, UpdateDT: model.UpdateDT);
                    tx.banner.UpdateBybannerId(bannerId: model1.bannerId, Sorts: model1.Sorts, UpdateBy: model1.UpdateBy, UpdateDT: model1.UpdateDT);
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

        //public Result AddCategory(AddCategoryModel model)
        //{
        //    var result = new Result();

        //    using (var tx = _database.Db.BeginTransaction())
        //    {
        //        try
        //        {
        //            var category = tx.Category.Insert(model);
        //            foreach (var lang in model.CategoryLanguageModels)
        //            {
        //                lang.CategoryId = category.CategoryId;
        //            }

        //            tx.Category_Lang.Insert(model.CategoryLanguageModels);
        //            tx.Commit();
        //        }
        //        catch (Exception ex)
        //        {
        //            tx.Rollback();

        //            result.IsValid = false;
        //            result.Messages.Add(ex.Message);
        //        }
        //    }

        //    return result;
        //}


        /// <summary>
        /// 删除banner图片
        /// </summary>
        /// <param name="model">banner图片模型</param>
        /// <returns>是否删除成功</returns>
        public ResultModel DeleteBanner(bannerModel model)
        {

            var result = new ResultModel()
            {
                Data = base._database.Db.banner.Delete(bannerId: model.bannerId)
            };

            result.IsValid = result.Data > 0 ? true : false;
            return result;
        }


        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="topCount"></param>
        /// <returns></returns>
        public ResultModel GetTopBanner(int topCount, int identityStatus, int placeCode)
        {
            var tb = base._database.Db.banner;
            var where = new SimpleExpression(1, 1, SimpleExpressionType.Equal);
            where = new SimpleExpression(where, tb.IdentityStatus == identityStatus, SimpleExpressionType.And);
            where = new SimpleExpression(where, tb.PlaceCode == placeCode, SimpleExpressionType.And);
            var result = new ResultModel()
            {
                Data = base._database.Db.banner.FindAll(where).Take(topCount).OrderBySorts().ToList<bannerModel>()
            };

            return result;
        }
        /// <summary>
        /// 获取banner按时间倒序
        /// 黄主霞 2015-01-14
        /// </summary>
        /// <param name="topCount">获取前N个Banner</param>
        /// <param name="identityStatus">类别：1首页轮播banner，2首页楼层banner,3 分类频道轮播banner,4分类频道楼层banner</param>
        /// <param name="placeCode">分类ID</param>
        /// <returns></returns>
        public ResultModel GetBannerByTimeDesc(int? topCount, int identityStatus, int placeCode)
        {
            var tb = base._database.Db.banner;
            var where = new SimpleExpression(1, 1, SimpleExpressionType.Equal);
            where = new SimpleExpression(where, tb.IdentityStatus == identityStatus, SimpleExpressionType.And);
            where = new SimpleExpression(where, tb.PlaceCode == placeCode, SimpleExpressionType.And);
            var result = new ResultModel()
            {
                Data = (topCount.HasValue?
                base._database.Db.banner.FindAll(where).Take(topCount.Value).OrderByDescending(tb.CreateDT).ToList<bannerModel>()
                : base._database.Db.banner.FindAll(where).OrderByDescending(tb.CreateDT).ToList<bannerModel>())
            };

            return result;
        }
        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="topCount"></param>
        /// <returns></returns>
        public ResultModel GetTopBannerForApp(int topCount, int identityStatus, int placeCode)
        {
            dynamic pc;
            var tb = base._database.Db.banner;
            var where = new SimpleExpression(1, 1, SimpleExpressionType.Equal);
            where = new SimpleExpression(where, tb.IdentityStatus == identityStatus, SimpleExpressionType.And);
            where = new SimpleExpression(where, tb.PlaceCode == placeCode, SimpleExpressionType.And);           
            var result = new ResultModel();
            var query = _database.Db.banner
                .Query()
                .LeftJoin(_database.Db.Product, out pc)
                 .On(pc.ProductId == tb.ProductId)
                .Select(tb.bannerId, tb.ProductId, tb.bannerName, tb.bannerUrl, tb.bannerPic, tb.PlaceCode, tb.IdentityStatus
                , tb.Sorts, tb.CreateBy, tb.CreateDT, tb.UpdateBy, tb.UpdateDT, pc.Status)
                .Where(where).               
                OrderBySorts().
                ToList<bannerMidModel>();
             List<bannerModel>  list=new List<bannerModel>  ();
            foreach (bannerMidModel a in query)
            {
                if (a.ProductId == 0 || (a.ProductId != 0 && a.Status == 4))
                {
                    bannerModel b = new bannerModel();
                    b.bannerId = a.bannerId;
                    b.bannerName = a.bannerName;
                    b.bannerPic = a.bannerPic;
                    b.bannerUrl = a.bannerUrl;
                    b.CreateBy = a.CreateBy;
                    b.CreateDT = a.CreateDT;
                    b.IdentityStatus = a.IdentityStatus;
                    b.PlaceCode = a.PlaceCode;
                    b.ProductId=a.ProductId;
                    b.UpdateBy=a.UpdateBy;
                    b.UpdateDT=a.UpdateDT;
                    list.Add(b);
                }
            }

            result.Data = list.Take(topCount).ToList<bannerModel>();
            return result;
        }
    }
}
