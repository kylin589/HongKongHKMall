using BrCms.Framework.Collections;
using HKTHMall.Domain.Enum;
using HKTHMall.Domain.Models;
using HKTHMall.Domain.WebModel.Models.Collection;
using HKTHMall.Domain.WebModel.Models.Search;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKTHMall.Core.Extensions;
namespace HKTHMall.Services.Products
{
    public class ProductSearchListService : BaseService, IProductSearchListService
    {
        /// <summary>
        /// 关键字搜素结果
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ResultModel GetProductSearchList(KeyWordsSearch model, out int count)
        {
            ResultModel result = new ResultModel();

            int PageIndex = model.Page;
            int PageSize = model.PageSize;
          
            var q = base._database.Db.Product
             .Query()
             .LeftJoin(base._database.Db.Product_Lang, ProductId: base._database.Db.Product.ProductId)
             .LeftJoin(base._database.Db.ProductPic, ProductID: base._database.Db.Product.ProductId)
             .LeftJoin(base._database.Db.ProductRule, ProductId: base._database.Db.Product.ProductId)
             ;

            q = q.Select(
              base._database.Db.Product.ProductId
              , q.Product_Lang.ProductName
              , q.Product_Lang.LanguageId
              , q.ProductPic.PicUrl
              , q.ProductPic.Flag
              , base._database.Db.Product.HKPrice
              , base._database.Db.Product.MarketPrice
              , base._database.Db.Product.IsDelete
              , base._database.Db.Product.SaleCount
              , base._database.Db.Product.CreateDT
              , base._database.Db.Product.stockquantity
              , base._database.Db.ProductRule.SalesRuleId
              , base._database.Db.ProductRule.StarDate
              , base._database.Db.ProductRule.EndDate
              , base._database.Db.ProductRule.Discount
              );

            q = q.Where(q.Product_Lang.ProductName.Like("%" + model.k + "%")).Where(q.ProductPic.Flag == 1).Where(q.Product_Lang.LanguageId==model.languageId)
                .Where(base._database.Db.Product.IsDelete == 0).Where(base._database.Db.Product.Status==ProductStatus.HasUpShelves.GetHashCode());
            q = q.Where(base._database.Db.Product.stockquantity >0);//搜索时过滤无库存的商品 朱志容
            if(model.st==SearchType.ZongHe)
            {
                q = q.OrderByDescending(base._database.Db.Product.CreateDT);
            }
            else if(model.st==SearchType.Sales)
            {
                q = q.OrderByDescending(base._database.Db.Product.SaleCount);
            }
            else if(model.st==SearchType.PriceAsc)
            {
                 q = q.OrderBy(base._database.Db.Product.HKPrice);
            }
            else if(model.st==SearchType.PriceDesc)
            {
                 q = q.OrderByDescending(base._database.Db.Product.HKPrice);
            }

          //  result.Data =q.ToList<SearchModel>();
            count = q.ToList<SearchModel>().Count;
            if (PageIndex == 1)
            {
                result.Data = q.Take(PageSize).ToList<SearchModel>();
            }
            else
            {
                result.Data = q.Skip((PageIndex - 1) * PageSize).Take(PageSize).ToList<SearchModel>();
            }
            return result;

        }
        /// <summary>
        /// 分类搜索列表
        /// </summary>
        /// <param name="model"></param>
        /// <param name="ilist">分类Id</param>
        /// <returns></returns>
        public ResultModel GetProductSearchList(KeyWordsSearch model, int[] ilist, out int count)
        {
            ResultModel result = new ResultModel();

            int PageIndex = model.Page;
            int PageSize = model.PageSize;

            var q = base._database.Db.Product
             .Query()
             .LeftJoin(base._database.Db.Product_Lang, ProductId: base._database.Db.Product.ProductId)
             .LeftJoin(base._database.Db.ProductPic, ProductID: base._database.Db.Product.ProductId)
             .LeftJoin(base._database.Db.ProductRule, ProductId: base._database.Db.Product.ProductId)
             ;

            q = q.Select(
              base._database.Db.Product.ProductId
              , q.Product_Lang.ProductName
              , q.Product_Lang.LanguageId
              , q.ProductPic.PicUrl
              , q.ProductPic.Flag
              , base._database.Db.Product.CategoryId
              , base._database.Db.Product.HKPrice
              , base._database.Db.Product.MarketPrice
              , base._database.Db.Product.IsDelete
              , base._database.Db.Product.SaleCount
              , base._database.Db.Product.Status
              , base._database.Db.Product.CreateDT
              , base._database.Db.ProductRule.SalesRuleId
              , base._database.Db.ProductRule.StarDate
              , base._database.Db.ProductRule.EndDate
              , base._database.Db.ProductRule.Discount
              );

            q = q.Where(q.ProductPic.Flag == 1).Where(q.Product_Lang.LanguageId == model.languageId)
                .Where(base._database.Db.Product.CategoryId == ilist).Where(base._database.Db.Product.IsDelete == 0).
                Where(base._database.Db.Product.Status == ProductStatus.HasUpShelves.GetHashCode());

            if (model.st == SearchType.ZongHe)
            {
                q = q.OrderByDescending(base._database.Db.Product.CreateDT);
            }
            else if (model.st == SearchType.Sales)
            {
                q = q.OrderByDescending(base._database.Db.Product.SaleCount);
            }
            else if (model.st == SearchType.PriceAsc)
            {
                q = q.OrderBy(base._database.Db.Product.HKPrice);
            }
            else if (model.st == SearchType.PriceDesc)
            {
                q = q.OrderByDescending(base._database.Db.Product.HKPrice);
            }

            count = q.ToList<SearchModel>().Count;
            if (PageIndex == 1)
            {
                result.Data = q.Take(PageSize).ToList<SearchModel>();
            }
            else
            {
                result.Data = q.Skip((PageIndex - 1) * PageSize).Take(PageSize).ToList<SearchModel>();
            }
            return result;

        }

        /// <summary>
        /// 我的收藏
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="languageId"></param>
        /// <returns></returns>
        public ResultModel GetMyCollectionList(long userId, KeyWordsSearch model, out int count)
        {
            ResultModel result = new ResultModel();

            int PageIndex = model.Page;
            int PageSize = model.PageSize;

            var z = base._database.Db.Favorites
             .Query()
             .LeftJoin(base._database.Db.Product, ProductId: base._database.Db.Favorites.ProductId)
             .LeftJoin(base._database.Db.Product_Lang, ProductId: base._database.Db.Product.ProductId)
             .LeftJoin(base._database.Db.ProductPic, ProductID: base._database.Db.Product.ProductId)
             ;

            z = z.Select(base._database.Db.Favorites.FavoritesID,
             base._database.Db.Favorites.ProductId, base._database.Db.Favorites.UserID, base._database.Db.Favorites.FavoritesDate
             , z.Product_Lang.ProductName, z.Product_Lang.LanguageId, z.ProductPic.PicUrl, z.ProductPic.Flag, z.Product.HKPrice
             , z.Db.Product.MarketPrice, z.Product.IsDelete, z.Db.Product.SaleCount);

            z = z.Where(z.ProductPic.Flag == 1).Where(z.Product_Lang.LanguageId ==model.languageId ).
                Where(base._database.Db.Favorites.UserID == userId).Where(z.Product.IsDelete == 0);
            z = z.OrderByDescending(base._database.Db.Favorites.FavoritesDate);

            count = z.ToList<MyCollectionModel>().Count;
            if(PageIndex==1)
            {
                result.Data = z.Take(PageSize).ToList<MyCollectionModel>();
            }
            else
            {
                result.Data = z.Skip((PageIndex-1) * PageSize).Take(PageSize).ToList<MyCollectionModel>();
            }
                         
           //  new SimpleDataPagedList<MyCollectionModel>(z, PageIndex, PageSize);

           // result.Data = z.ToList<MyCollectionModel>();

            return result;

        }
        /// <summary>
        /// 我的收藏
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="languageId"></param>
        /// <returns></returns>
        public ResultModel GetMyCollectionListForApi(long userId, KeyWordsSearch model, out int count)
        {
            ResultModel result = new ResultModel();

            int PageIndex = model.Page;
            int PageSize = model.PageSize;

            var z = base._database.Db.Favorites
             .Query()
             .LeftJoin(base._database.Db.Product, ProductId: base._database.Db.Favorites.ProductId)
             .LeftJoin(base._database.Db.Product_Lang, ProductId: base._database.Db.Product.ProductId)
             .LeftJoin(base._database.Db.ProductPic, ProductID: base._database.Db.Product.ProductId)
             .LeftJoin(base._database.Db.ProductRule, ProductId: base._database.Db.Product.ProductId)
             ;

            z = z.Select(base._database.Db.Favorites.FavoritesID,
             base._database.Db.Favorites.ProductId, base._database.Db.Favorites.UserID, base._database.Db.Favorites.FavoritesDate
             , z.Product_Lang.ProductName, z.Product_Lang.LanguageId, z.ProductPic.PicUrl, z.ProductPic.Flag, z.Product.HKPrice
             , z.Db.Product.MarketPrice, z.Product.IsDelete, z.Db.Product.SaleCount, base._database.Db.ProductRule.Discount, 
             base._database.Db.ProductRule.StarDate,base._database.Db.ProductRule.EndDate);

            z = z.Where(z.ProductPic.Flag == 1).Where(z.Product_Lang.LanguageId == model.languageId).
                Where(base._database.Db.Favorites.UserID == userId).Where(z.Product.IsDelete == 0); ;
            z = z.OrderByDescending(base._database.Db.Favorites.FavoritesDate);

            count = z.ToList<MyCollectionModel>().Count;
            if (PageIndex == 1)
            {
                result.Data = z.Take(PageSize).ToList<MyCollectionModel>();
            }
            else
            {
                result.Data = z.Skip((PageIndex - 1) * PageSize).Take(PageSize).ToList<MyCollectionModel>();
            }
            //转换数据
            List<MyCollectionModel> explosionList = result.Data;
            if (explosionList.Count > 0)
            {
                ///计算时间间隔差距,以及活动价格
                explosionList.ForEach(a =>
                {
                    a.isActivity = (a.EndDate > DateTime.Now && a.Discount != 0) ? true : false;
                    a.activityPrice = (a.HKPrice * a.Discount);
                });
            }
            result.Data = explosionList;
            //  new SimpleDataPagedList<MyCollectionModel>(z, PageIndex, PageSize);

            // result.Data = z.ToList<MyCollectionModel>();

            return result;

        }
        /// <summary>
        /// 我的收藏删除
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="collectionId"></param>
        /// <returns></returns>
        public ResultModel DeleteMyCollection(long userId, long collectionId)
        {
            ResultModel result = new ResultModel();
            result.IsValid = true;
            var user = _database.Db.Favorites.All()
                .Select(_database.Db.Favorites.UserID)
                .Where(_database.Db.Favorites.FavoritesID == collectionId).ToArray();
            if (user.Length <= 0)
            {
                result.IsValid = false;
                result.Messages = new List<string>() { "删除失败,收藏记录不存在！" };
                return result;
            }
            if ((long)user[0].UserID != userId)
            {
                result.IsValid = false;
                result.Messages = new List<string>() { "删除失败, 该收藏不属于你！" };
                return result;
            }
            result.Data = _database.Db.Favorites.DeleteByFavoritesID(collectionId);
            result.Messages = new List<string>() { "删除成功！" };
            return result;
        }
        /// <summary>
        /// 分类搜索列表
        /// </summary>
        /// <param name="model"></param>
        /// <param name="ilist"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public ResultModel GetAllSearchList(KeyWordsSearch model, int[] ilist, long userId, out int count)
        {
            ResultModel result = new ResultModel();

            int PageIndex = model.Page;
            int PageSize = model.PageSize;
            dynamic q;

            if (userId == 0)
            {
                q = base._database.Db.Product
                 .Query()
                 .LeftJoin(base._database.Db.Product_Lang, ProductId: base._database.Db.Product.ProductId)
                 .LeftJoin(base._database.Db.ProductPic, ProductID: base._database.Db.Product.ProductId)
                 .LeftJoin(base._database.Db.ProductRule, ProductId: base._database.Db.Product.ProductId);
            }
            else
            {
                q = base._database.Db.Product
                 .Query()
                 .LeftJoin(base._database.Db.Product_Lang, ProductId: base._database.Db.Product.ProductId)
                 .LeftJoin(base._database.Db.ProductPic, ProductID: base._database.Db.Product.ProductId)
                 .LeftJoin(base._database.Db.ProductRule, ProductId: base._database.Db.Product.ProductId)
                 .LeftJoin(base._database.Db.Favorites).On(base._database.Db.Favorites.ProductId == base._database.Db.Product.ProductId && base._database.Db.Favorites.UserID==userId);
            }
 
            q = q.Select(
              base._database.Db.Product.ProductId
              , q.Product_Lang.ProductName
              , q.Product_Lang.LanguageId
              , q.ProductPic.PicUrl
              , q.ProductPic.Flag
              , q.Favorites.UserID
              , base._database.Db.Product.CategoryId
              , base._database.Db.Product.HKPrice
              , base._database.Db.Product.MarketPrice
              , base._database.Db.Product.IsDelete
              , base._database.Db.Product.SaleCount
              , base._database.Db.Product.Status
              , base._database.Db.Product.CreateDT
              , base._database.Db.ProductRule.SalesRuleId
              , base._database.Db.ProductRule.StarDate
              , base._database.Db.ProductRule.EndDate
              , base._database.Db.ProductRule.Discount
              );


            q = q.Where(q.ProductPic.Flag == 1).Where(q.Product_Lang.LanguageId == model.languageId)
            .Where(base._database.Db.Product.CategoryId == ilist).Where(base._database.Db.Product.IsDelete == 0).
            Where(base._database.Db.Product.Status == ProductStatus.HasUpShelves.GetHashCode());
  
            count = q.ToList<SearchModel>().Count;

            result.Data = q.ToList<SearchModel>();

            List<SearchModel> productlist = result.Data;
            if (productlist.Count > 0)
            {
                productlist.ForEach(a =>
                {
                    //处理价格显示促销中的
                    if (a.SalesRuleId > 1 & a.StarDate <= DateTime.Now & a.EndDate >= DateTime.Now)
                    {
                        var price = a.HKPrice;
                        a.MarketPrice = a.HKPrice;
                        a.HKPrice = a.Discount == 0 ? price : price * a.Discount;
                    }
                    a.CreateDT =Convert.ToDateTime(a.CreateDT).DateTimeToString();
                });
            }
            List<SearchModel> searchData = null;
            if (model.sf == SearchField.ZongHe && model.AscOrDesc == AscOrDescType.DESC)
            {
                searchData = productlist.OrderByDescending(a => a.CreateDT).ToList();
            }
            if (model.sf == SearchField.Sales && model.AscOrDesc == AscOrDescType.ASC)
            {
                searchData = productlist.OrderBy(a => a.SaleCount).ToList();
            }
            if (model.sf == SearchField.Sales && model.AscOrDesc == AscOrDescType.DESC)
            {
                searchData = productlist.OrderByDescending(a => a.SaleCount).ToList();
            }
            if (model.sf == SearchField.Price && model.AscOrDesc == AscOrDescType.ASC)
            {
                searchData = productlist.OrderBy(a => a.HKPrice).ToList();
            }
            if (model.sf == SearchField.Price && model.AscOrDesc == AscOrDescType.DESC)
            {
                searchData = productlist.OrderByDescending(a => a.HKPrice).ToList();
            }
           
            if (PageIndex == 1)
            {
                result.Data = searchData.Take(PageSize).ToList();
            }
            else
            {
                result.Data = searchData.Skip((PageIndex - 1) * PageSize).Take(PageSize).ToList();
            }
            return result;

        }

        /// <summary>
        /// 关键字搜素结果
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ResultModel GetProductSearchListNew(KeyWordsSearch model, long userId, out int count)
        {
            ResultModel result = new ResultModel();

            int PageIndex = model.Page;
            int PageSize = model.PageSize;
            dynamic q;
            if (userId == 0)
            {
                q = base._database.Db.Product
                 .Query()
                 .LeftJoin(base._database.Db.Product_Lang, ProductId: base._database.Db.Product.ProductId)
                 .LeftJoin(base._database.Db.ProductPic, ProductID: base._database.Db.Product.ProductId)
                 .LeftJoin(base._database.Db.ProductRule, ProductId: base._database.Db.Product.ProductId);
            }
            else
            {
                q = base._database.Db.Product
                 .Query()
                 .LeftJoin(base._database.Db.Product_Lang, ProductId: base._database.Db.Product.ProductId)
                 .LeftJoin(base._database.Db.ProductPic, ProductID: base._database.Db.Product.ProductId)
                 .LeftJoin(base._database.Db.ProductRule, ProductId: base._database.Db.Product.ProductId)
                 .LeftJoin(base._database.Db.Favorites).On(base._database.Db.Favorites.ProductId == base._database.Db.Product.ProductId && base._database.Db.Favorites.UserID == userId);
            }


            q = q.Select(
              base._database.Db.Product.ProductId
              , q.Product_Lang.ProductName
              , q.Product_Lang.LanguageId
              , q.ProductPic.PicUrl
              , q.ProductPic.Flag
              , q.Favorites.UserID
              , base._database.Db.Product.HKPrice
              , base._database.Db.Product.MarketPrice
              , base._database.Db.Product.IsDelete
              , base._database.Db.Product.SaleCount
              , base._database.Db.Product.CreateDT
              , base._database.Db.Product.stockquantity
              , base._database.Db.ProductRule.SalesRuleId
              , base._database.Db.ProductRule.StarDate
              , base._database.Db.ProductRule.EndDate
              , base._database.Db.ProductRule.Discount
              );

            q = q.Where(q.Product_Lang.ProductName.Like("%" + model.k + "%")).Where(q.ProductPic.Flag == 1).Where(q.Product_Lang.LanguageId == model.languageId)
                .Where(base._database.Db.Product.IsDelete == 0).Where(base._database.Db.Product.Status == ProductStatus.HasUpShelves.GetHashCode());
            q = q.Where(base._database.Db.Product.stockquantity > 0);

            count = q.ToList<SearchModel>().Count;

            result.Data = q.ToList<SearchModel>();

            List<SearchModel> productlist = result.Data;
            if (productlist.Count > 0)
            {
                productlist.ForEach(a =>
                {
                    //处理价格显示促销中的
                    if (a.SalesRuleId > 1 & a.StarDate <= DateTime.Now & a.EndDate >= DateTime.Now)
                    {
                        var price = a.HKPrice;
                        a.MarketPrice = a.HKPrice;
                        a.HKPrice = a.Discount == 0 ? price : price * a.Discount;
                    }
                    a.CreateDT = Convert.ToDateTime(a.CreateDT).DateTimeToString();
                });
            }
            List<SearchModel> searchData = null;
            if (model.sf == SearchField.ZongHe && model.AscOrDesc == AscOrDescType.DESC)
            {
                searchData = productlist.OrderByDescending(a => a.CreateDT).ToList();
            }
            if (model.sf == SearchField.Sales && model.AscOrDesc == AscOrDescType.ASC)
            {
                searchData = productlist.OrderBy(a => a.SaleCount).ToList();
            }
            if (model.sf == SearchField.Sales && model.AscOrDesc == AscOrDescType.DESC)
            {
                searchData = productlist.OrderByDescending(a => a.SaleCount).ToList();
            }
            if (model.sf == SearchField.Price && model.AscOrDesc == AscOrDescType.ASC)
            {
                searchData = productlist.OrderBy(a => a.HKPrice).ToList();
            }
            if (model.sf == SearchField.Price && model.AscOrDesc == AscOrDescType.DESC)
            {
                searchData = productlist.OrderByDescending(a => a.HKPrice).ToList();
            }

            if (PageIndex == 1)
            {
                result.Data = searchData.Take(PageSize).ToList();
            }
            else
            {
                result.Data = searchData.Skip((PageIndex - 1) * PageSize).Take(PageSize).ToList();
            }
            return result;

        }
    }
}
