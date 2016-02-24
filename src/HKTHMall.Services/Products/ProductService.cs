using System;
using System.Collections.Generic;
using System.Linq;
using BrCms.Framework.Collections;
using HKTHMall.Domain.AdminModel.Models.Products;
using HKTHMall.Domain.Enum;
using HKTHMall.Domain.Models;
using HKTHMall.Domain.Models.Categoreis;
using HKTHMall.Domain.WebModel.Models.Product;
using HKTHMall.Domain.WebModel.Models.Search;
using Simple.Data.RawSql;
using HKTHMall.Core.Extensions;
using System.Text;
using HKTHMall.Domain.WebModel.Models.ShoppingCart;
using HKTHMall.Core;

namespace HKTHMall.Services.Products
{
    public class ProductService : BaseService, IProductService
    {
        public ResultModel Search(SearchProductModel model)
        {
            var result = new ResultModel();

            var q = this._database.Db.Product
                .Query()
                .LeftJoin(this._database.Db.Product_Lang.As("Product_LangModels"),
                    ProductId: this._database.Db.Product.ProductId)
                .LeftJoin(this._database.Db.Category, CategoryId: this._database.Db.Product.CategoryId)
                .LeftJoin(this._database.Db.Category_Lang.As("CategoryLanguageModel"),
                    CategoryId: this._database.Db.Category.CategoryId)
                .LeftJoin(this._database.Db.Brand, BrandID: this._database.Db.Product.BrandID)
                .LeftJoin(this._database.Db.Brand_Lang.As("Brand_LangModel"), BrandID: this._database.Db.Brand.BrandID)
                .LeftJoin(this._database.Db.Suppliers, SupplierId: this._database.Db.Product.SupplierId)
                ;

            if (model.LanguageId.HasValue)
            {
                q = q
                    .Select(
                        this._database.Db.Product.ProductId
                        , q.CategoryLanguageModel.CategoryName
                        , q.Product_LangModels.ProductName
                        , q.Product_LangModels.LanguageId
                        , q.Brand_LangModel.BrandName
                        , this._database.Db.Product.ArtNo
                        , this._database.Db.Product.PostagePrice
                        , this._database.Db.Product.StockQuantity
                        , this._database.Db.Product.AllowCustomerReviews
                        , this._database.Db.Product.AllowBackInStockSubscriptions
                        , this._database.Db.Product.IsProvideInvoices
                        , this._database.Db.Product.Weight
                        , this._database.Db.Product.HKPrice
                        , this._database.Db.Product.ProductParameter
                        , this._database.Db.Product.PackingList
                        , this._database.Db.Product.MarketPrice
                        , this._database.Db.Product.PurchasePrice
                        , this._database.Db.Product.ActivityBottomPrice
                        , this._database.Db.Product.SaleCount
                        , this._database.Db.Product.NotifyAdminForQuantityBelow
                        , this._database.Db.Product.PutawayDT
                        , this._database.Db.Product.Status
                        , this._database.Db.Product.RecommendSort
                        , this._database.Db.Product.IsDelete
                        , this._database.Db.Product.ExtensionPropertiesText
                        , this._database.Db.Product.Volume
                        , this._database.Db.Product.CreateBy
                        , this._database.Db.Product.CreateDT
                        , this._database.Db.Product.UpdateBy
                        , this._database.Db.Product.UpdateDT
                        , this._database.Db.Suppliers.SupplierName
                        , q.Product_LangModels.Introduction //商品详情页 商品描述
                    )
                    .Where(q.Product_LangModels.LanguageId == null ||
                           q.Product_LangModels.LanguageId == model.LanguageId.Value)
                    .Where(q.CategoryLanguageModel.LanguageId == null ||
                           q.CategoryLanguageModel.LanguageId == model.LanguageId.Value)
                    .Where(q.Brand_LangModel.LanguageId == null ||
                           q.Brand_LangModel.LanguageId == model.LanguageId.Value)
                    ;
            }
            else
            {
                q = q
                    .WithMany(q.Product_LangModels)
                    .WithOne(q.Brand_LangModel)
                    .WithOne(q.CategoryLanguageModel)
                    ;
            }

            q = q.Where(this._database.Db.Product.IsDelete == false);
            if (model.Status.HasValue)
            {
                q = q.Where(this._database.Db.Product.Status == (int)model.Status.Value);
            }
            if (model.CategoryId.HasValue)
            {
                q = q.Where(this._database.Db.Product.CategoryId == model.CategoryId);
            }

             if (model.CategoryId3.HasValue && model.CategoryId3 > 0)
            {
                q = q.Where(this._database.Db.Product.CategoryId == model.CategoryId3.Value);
            }
             else if (model.CategoryId2.HasValue && model.CategoryId2 > 0)
             {
                 List<dynamic> cids = this._database.Db.Category.Query()
                     .Where(this._database.Db.Category.parentId == model.CategoryId2.Value)
                     .Select(this._database.Db.Category.CategoryId);

                 if (cids.Count > 0)
                 {
                     var newList = new List<long>();
                     foreach (var cid in cids)
                     {
                         newList.Add(cid.CategoryId ?? 0);
                     }
                     q = q.Where(this._database.Db.Product.CategoryId == newList);
                 }
                 else
                 {
                     q = q.Where(this._database.Db.Product.CategoryId == 0);
                 }
             }

            else if (model.CategoryId1.HasValue && model.CategoryId1>0)
            {
                dynamic category1;
                List<dynamic> cids = this._database.Db.Category.Query()
                    .LeftJoin(this._database.Db.Category.As("cateogry1"), out category1)
                    .On(category1.parentId == this._database.Db.Category.CategoryId)
                    .Where(this._database.Db.Category.parentId == model.CategoryId1.Value)
                    .Select(category1.CategoryId);

                if (cids.Count > 0)
                {
                    var newList = new List<long>();
                    foreach (var cid in cids)
                    {
                        newList.Add(cid.CategoryId ?? 0);
                    }
                    q = q.Where(this._database.Db.Product.CategoryId == newList);
                }
                else
                {
                    q = q.Where(this._database.Db.Product.CategoryId == 0);
                }
            }
            
            



            if (model.BrandId.HasValue && model.BrandId > 0)
            {
                q = q.Where(this._database.Db.Brand.BrandID == model.BrandId);
            }
            if (!string.IsNullOrEmpty(model.ProductName))
            {
                q = q.Where(q.Product_LangModels.ProductName.Like('%' + model.ProductName + '%'));
            }

            if (!string.IsNullOrEmpty(model.CategoryName))
            {
                q = q.Where(q.CategoryLanguageModel.CategoryName.Like('%' + model.CategoryName + '%'));
            }
            if (!string.IsNullOrEmpty(model.BrandName))
            {
                q = q.Where(q.Brand_LangModel.BrandName.Like('%' + model.BrandName + '%'));
            }
            if (model.ProductId.HasValue)
            {
                q = q.Where(this._database.Db.Product.ProductId == model.ProductId.Value);
            }     
            if (!string.IsNullOrEmpty(model.SortName))
            {
                switch (model.SortName)
                {
                    case "SupplierName":  
                        if (model.SortOrder == "desc")
                        {
                            q = q.OrderByDescending(this._database.Db.Suppliers.SupplierName);
                        }
                        else
                        {
                            q = q.OrderBy(this._database.Db.Suppliers.SupplierName);                           
                        }  
                        break;
                    case "CategoryName":
                        if (model.SortOrder == "desc")
                        {
                            q = q.OrderByDescending(q.CategoryLanguageModel.CategoryName);
                        }
                        else
                        {
                            q = q.OrderBy(q.CategoryLanguageModel.CategoryName);
                        } 
                        break;
                    case "BrandName":
                        if (model.SortOrder == "desc")
                        {
                            q = q.OrderByDescending(q.Brand_LangModel.BrandName);
                        }
                        else
                        {
                            q = q.OrderBy(q.Brand_LangModel.BrandName);
                        }
                        break;
                    case "ArtNo":
                        if (model.SortOrder == "desc")
                        {
                            q = q.OrderByDescending(this._database.Db.Product.ArtNo);
                        }
                        else
                        {
                            q = q.OrderBy(this._database.Db.Product.ArtNo);
                        }
                        break;
                    case "PostagePrice":
                        if (model.SortOrder == "desc")
                        {
                            q = q.OrderByDescending(this._database.Db.Product.PostagePrice);
                        }
                        else
                        {
                            q = q.OrderBy(this._database.Db.Product.PostagePrice);
                        }
                        break;
                    case "MarketPrice":
                        if (model.SortOrder == "desc")
                        {
                            q = q.OrderByDescending(this._database.Db.Product.MarketPrice);
                        }
                        else
                        {
                            q = q.OrderBy(this._database.Db.Product.MarketPrice);
                        }
                        break;
                    case "HKPrice":
                        if (model.SortOrder == "desc")
                        {
                            q = q.OrderByDescending(this._database.Db.Product.HKPrice);
                        }
                        else
                        {
                            q = q.OrderBy(this._database.Db.Product.HKPrice);
                        }
                        break;
                    case "StockQuantity":
                        if (model.SortOrder == "desc")
                        {
                            q = q.OrderByDescending(this._database.Db.Product.StockQuantity);
                        }
                        else
                        {
                            q = q.OrderBy(this._database.Db.Product.StockQuantity);
                        }
                        break;
                    case "SaleCount":
                        if (model.SortOrder == "desc")
                        {
                            q = q.OrderByDescending(this._database.Db.Product.SaleCount);
                        }
                        else
                        {
                            q = q.OrderBy(this._database.Db.Product.SaleCount);
                        }
                        break;
                    case "Status":
                        if (model.SortOrder == "desc")
                        {
                            q = q.OrderByDescending(this._database.Db.Product.Status);
                        }
                        else
                        {
                            q = q.OrderBy(this._database.Db.Product.Status);
                        }
                        break;
                    case "CreateBy":
                        if (model.SortOrder == "desc")
                        {
                            q = q.OrderByDescending(this._database.Db.Product.CreateBy);
                        }
                        else
                        {
                            q = q.OrderBy(this._database.Db.Product.CreateBy);
                        }
                        break;
                    case "CreateDT":
                        if (model.SortOrder == "desc")
                        {
                            q = q.OrderByDescending(this._database.Db.Product.CreateDT);
                        }
                        else
                        {
                            q = q.OrderBy(this._database.Db.Product.CreateDT);
                        }
                        break;
                    case "PutawayDT":
                        if (model.SortOrder == "desc")
                        {
                            q = q.OrderByDescending(this._database.Db.Product.PutawayDT);
                        }
                        else
                        {
                            q = q.OrderBy(this._database.Db.Product.PutawayDT);
                        }
                        break;
                    case "UpdateBy":
                        if (model.SortOrder == "desc")
                        {
                            q = q.OrderByDescending(this._database.Db.Product.UpdateBy);
                        }
                        else
                        {
                            q = q.OrderBy(this._database.Db.Product.UpdateBy);
                        }
                        break;
                    case "UpdateDT":
                        if (model.SortOrder == "desc")
                        {
                            q = q.OrderByDescending(this._database.Db.Product.UpdateDT);
                        }
                        else
                        {
                            q = q.OrderBy(this._database.Db.Product.UpdateDT);
                        }
                        break;
                    default:
                        q = q.OrderByDescending(this._database.Db.Product.CreateDT);
                        break;
                }
            }
            else
            {
                q = q.OrderByDescending(this._database.Db.Product.CreateDT);
            }



            result.Data = new SimpleDataPagedList<SearchResultModel>(q, model.PagedIndex, model.PagedSize);

            return result;
        }

        /// <summary>
        /// 查询分类品牌产品
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ResultModel SearchCategoryBrandProduct(SearchBrandProductModel model,ref int totalCount)
        {
            var result = new ResultModel();
            string sqlWhere = "";
            string sort = "CreateDT desc";
            #region 查询条件

            if (model.categoryId > 0)
            {
                if (model.level==1)
                {
                    sqlWhere += " and a.CategoryId in(select CategoryId from Category where parentId in(select CategoryId from Category where parentId=" + model.categoryId + "))";
                }
                else if (model.level == 2)
                {
                    sqlWhere += " and  a.CategoryId in (select CategoryId from Category where parentId=" + model.categoryId + ")";
                }
                else if (model.level == 3)
                {
                    sqlWhere += " and  a.CategoryId=" + model.categoryId.ToString();
                }
               
            }           
            if (model.status > 0)
            {
                sqlWhere += " and  a.status=" + model.status;
            }
            if (model.brandId > 0)
            {
                sqlWhere += " and  a.BrandID=" + model.brandId;
            }
            if (model.startPrice > 0 && model.endPrice > 0)
            {
                sqlWhere += " and  a.HKPrice>" + model.startPrice + " and a.HKPrice<=" + model.endPrice;
            }
            else if (model.startPrice > 0)
            {
                sqlWhere += " and  a.HKPrice>" + model.startPrice;
            }
            else if (model.endPrice > 0)
            {
                sqlWhere += " and a.HKPrice<=" + model.endPrice;
            }
            if (!string.IsNullOrEmpty(model.ProductName))
            {
                sqlWhere += " and ProductName like'%" + model.ProductName + "%'";
            }

            if (!string.IsNullOrEmpty(model.startDate) && !string.IsNullOrEmpty(model.endDate))
            {
                sqlWhere += " and  a.CreateDT>='" + model.startDate + "' and a.CreateDT<='" + model.endDate + "'";
            }           
            else if (!string.IsNullOrEmpty(model.startDate))
            {
                sqlWhere += " and  a.CreateDT>='" + model.startDate + "'";
            }
            else if (!string.IsNullOrEmpty(model.endDate))
            {
                sqlWhere += " and  a.CreateDT<='" + model.endDate + "'";
            }
            if (model.sortPrice == 1)
            {
                sort = " HKPrice";
            }
            else if (model.sortPrice == 2)
            {
                sort = " HKPrice desc";
            }
            else if (model.sortRebateDays == 1)
            {
                sort = " RebateDays";
            }
            else if (model.sortRebateDays == 2)
            {
                sort = " RebateDays desc";
            }
            //销量同等的最新发布时间靠前
            else if (model.sortSell == 1)
            {
                sort = "CreateDT";
            }
            else if (model.sortSell == 2)
            {
                sort = "CreateDT desc";
            }           
            #endregion
            if (totalCount == 0)
            {
                string strSqlCount = "select Count(a.productid)sumnumber from Product a inner join Product_Lang b on a.ProductId=b.ProductId inner join ProductPic c on a.ProductId=c.ProductID where a.IsDelete = 0 and c.Flag=1 and b.LanguageID=" + model.langId + sqlWhere;
                decimal soure = _database.RunSqlQuery(x => x.ToScalar(strSqlCount));
                totalCount = (int)soure;

            }
            string strSql = "select a.ProductID,b.LanguageID,c.PicUrl,b.ProductName,a.HKPrice,a.RebateDays,a.CreateDT from Product a inner join Product_Lang b on a.ProductId=b.ProductId inner join ProductPic c on a.ProductId=c.ProductID where  a.IsDelete = 0 and c.Flag=1 and b.LanguageID=" + model.langId + sqlWhere;           
            strSql = "select * from (select row_number() over (order by "+sort+") as RowNumber,* from ("+strSql+")aa)as temp where RowNumber >" + ((model.pageIndex-1)*model.pageSize) + " and RowNumber<=" + (model.pageIndex*model.pageSize);
            List<dynamic> sources = _database.RunSqlQuery(x => x.ToRows(strSql));
            result.Data = sources.ToEntity<ProductInfo>();
            return result; ;
        }
        /// <summary>
        ///     审核
        /// </summary>
        /// <param name="productId">商品Id</param>
        /// <param name="status">状态</param>
        /// <returns></returns>
        public ResultModel Check(long productId, ProductStatus status)
        {
            var result = new ResultModel();
            if (ProductStatus.HasUpShelves == status)
            {
                this._database.Db.Product.UpdateByProductId(ProductId: productId, Status: (int)status,
                    PutawayDT: DateTime.Now);
            }
            else
            {
                this._database.Db.Product.UpdateByProductId(ProductId: productId, Status: (int)status);
            }

            return result;
        }
        /// <summary>
        ///     审核
        /// </summary>
        /// <param name="productId">商品Id</param>
        /// <param name="status">状态</param>
        /// <returns></returns>
        public ResultModel Check(long[] productIds, ProductStatus status)
        {
            var result = new ResultModel();
            using (var tx = this._database.Db.BeginTransaction())
            {
                try
                {
                    if (ProductStatus.HasUpShelves == status)
                    {
                        foreach (long id in productIds)
                        {
                            tx.Product.UpdateByProductId(ProductId: id, Status: (int)status,
                                PutawayDT: DateTime.Now);
                        }
                    }
                    else
                    {
                        foreach (long id in productIds)
                        {
                            tx.Product.UpdateByProductId(ProductId: id, Status: (int)status);
                        }
                    }
                    tx.Commit();
                    result.IsValid = true;
                }
                catch
                {
                    tx.Rollback();
                    result.IsValid = false;
                    result.Messages.Add(string.Format("check product {0} fails", string.Join(",", productIds)));
                }
            }
            return result;
        }
        /// <summary>
        /// 获取热销列表
        /// </summary>
        /// <param name="TopCount"></param>
        /// <param name="Lang"></param>
        /// <returns></returns>
        public ResultModel GetSellingList(int TopCount,int Lang=4)
        {
            var tbProduct=  base._database.Db.Product;
            var tbProImg = base._database.Db.ProductPic;
            var tbProLang=base._database.Db.Product_Lang;
            var result = new ResultModel
            {
                Data = tbProduct.Query()
                .Join(tbProImg)
                .On(tbProduct.ProductId == tbProImg.ProductId)
                .Join(tbProLang)
                .On(tbProduct.ProductId == tbProLang.ProductId)
                .Where(tbProduct.StockQuantity != 0 && tbProduct.IsDelete == false && tbProduct.Status==4
                && tbProImg.Flag == 1 && tbProLang.LanguageID==Lang)
                .OrderByDescending(tbProduct.SaleCount).ThenBy(tbProduct.PutawayDT)
                .Select(tbProduct.ProductId, tbProImg.PicUrl, tbProLang.ProductName, tbProLang.Subheading, tbProduct.HKPrice)
                .Take(TopCount)
            };
            return result;
        }

        public ResultModel GetProductById(long id, int luanguageId)
        {
            var result = new ResultModel();

            result.Data = this._database.RunQuery(db =>
            {
                var model = new AddProductModel();

                model = db.Product.get(id);

                model.Product_LangModels = db.Product_Lang.FindAll(db.Product_Lang.ProductId == id)
                    .ToList<ProductModel.Product_LangModel>();

                model.Brand_LangModel =
                    db.Brand_Lang.Find(db.Brand_Lang.BrandID == model.BrandID && db.Brand_Lang.LanguageID == luanguageId);

                model.CategoryLanguageModel = db.Category_Lang.Find(db.Category_Lang.CategoryId == model.CategoryId &&
                                                                    db.Category_Lang.LanguageID == luanguageId);

                model.SKU_ProductAttributesModels =
                    db.SKU_ProductAttributes.FindAll(db.SKU_ProductAttributes.ProductId == id)
                        .ToList<AddSKU_ProductAttributesModel>();

                model.SKU_SKUItemsModels =
                    db.SKU_SKUItems.FindAll(db.SKU_SKUItems.ProductId == id).ToList<AddSKU_SKUItemsModel>();

                model.SKU_ProductModels =
                    db.SKU_Product.FindAll(db.SKU_Product.ProductId == id).ToList<ProductModel.AddSKU_ProductModel>();

                model.ProductPicModels =
                    db.ProductPic.FindAll(db.ProductPic.ProductId == id).ToList<ProductModel.ProductPicModel>();

                if (model.SupplierId.HasValue && model.SupplierId != 0)
                {
                    model.SupplierName = db.Suppliers.Get(model.SupplierId).SupplierName;
                }

                model.ProductParametersModels = db.ProductParameters.FindAllByProductId(model.ProductId).ToList<ProductModel.ProductParameters>();

                return model;

                ////SKU_ProductModels
                //var q = db.Product
                //    .Query()
                //    .LeftJoin(db.Product_Lang.As("Product_LangModels"), ProductId: db.Product.ProductId)
                //    //.LeftJoin(db.Category, CategoryId: db.Product.CategoryId)
                //    .LeftJoin(db.Category_Lang.As("CategoryLanguageModel"), CategoryId: db.Product.CategoryId)
                //    //.LeftJoin(db.Brand, BrandID: db.Product.BrandID)
                //    .LeftJoin(db.Brand_Lang.As("Brand_LangModel"), BrandID: db.Product.BrandID)
                //    .LeftJoin(db.SKU_ProductAttributes.As("SKU_ProductAttributesModels"),
                //        ProductId: db.Product.ProductId)
                //    .LeftJoin(db.SKU_Product.As("SKU_ProductModels"), ProductId: db.Product.ProductId)
                //    .LeftJoin(db.SKU_SKUItems.As("SKU_SKUItemsModels"), ProductId: db.Product.ProductId)
                //    .LeftJoin(db.ProductPic.As("ProductPicModels"), ProductID: db.Product.ProductId)
                //    ;

                //q = q
                //    .WithMany(q.Product_LangModels)
                //    .WithOne(q.Brand_LangModel)
                //    .WithOne(q.CategoryLanguageModel)
                //    .WithMany(q.SKU_ProductAttributesModels)
                //    .WithMany(q.SKU_ProductModels)
                //    .WithMany(q.ProductPicModels)
                //    .WithMany(q.SKU_SKUItemsModels)
                //    .Where(db.Product.IsDelete == false)
                //    ;
                //return q.Where(db.Product.ProductId == id).FirstOrDefault();
            });

            return result;
        }
        /// <summary>
        /// 复制商品
        /// </summary>
        /// <param name="id">商品id</param>
        /// <returns></returns>
        public ResultModel CopyProductById(long id)
        {
            var result = new ResultModel();
            #region 获取该商品信息
            var model = new AddProductModel();
            dynamic db = this._database.Db;
            model = db.Product.get(id);         
           
            model.Product_LangModels = db.Product_Lang.FindAll(db.Product_Lang.ProductId == id)
                .ToList<ProductModel.Product_LangModel>();
          
            model.PrdoctRuleModels =
                db.ProductRule.FindAll(db.ProductRule.ProductId == id)
                .ToList<ProdctRuleModel>();

          
            model.SKU_ProductAttributesModels =
                db.SKU_ProductAttributes.FindAll(db.SKU_ProductAttributes.ProductId == id)
                    .ToList<AddSKU_ProductAttributesModel>();

            model.SKU_SKUItemsModels =
                db.SKU_SKUItems.FindAll(db.SKU_SKUItems.ProductId == id).ToList<AddSKU_SKUItemsModel>();

            model.SKU_ProductModels =
                db.SKU_Product.FindAll(db.SKU_Product.ProductId == id).ToList<ProductModel.AddSKU_ProductModel>();

            model.ProductPicModels =
                db.ProductPic.FindAll(db.ProductPic.ProductId == id).ToList<ProductModel.ProductPicModel>();

            if (model.SupplierId.HasValue && model.SupplierId != 0)
            {
                model.SupplierName = db.Suppliers.Get(model.SupplierId).SupplierName;
            }

            model.ProductParametersModels = db.ProductParameters.FindAllByProductId(model.ProductId).ToList<ProductModel.ProductParameters>();

            #endregion
            #region 复制商品
            using (var tx = this._database.Db.BeginTransaction())
            {
                try
                {
                    var product = new NewProductModel
                    {
                        ProductId = MemCacheFactory.GetCurrentMemCache().Increment("logId")
                        ,
                        CategoryId = model.CategoryId
                        ,
                        MerchantID = model.MerchantID
                        ,
                        FareTemplateID = model.FareTemplateID
                        ,
                        ArtNo = model.ArtNo
                        ,
                        ProductBarcode = model.ProductBarcode
                        ,
                        Status = model.Status
                        ,
                        PostagePrice = model.PostagePrice
                        ,
                        StockQuantity = model.StockQuantity
                        ,
                        AllowCustomerReviews = model.AllowCustomerReviews
                        ,
                        AllowBackInStockSubscriptions = model.AllowBackInStockSubscriptions
                        ,
                        IsProvideInvoices = model.IsProvideInvoices
                        ,
                        Weight = model.Weight
                        ,
                        ProductParameter = model.ProductParameter
                        ,
                        PackingList = model.PackingList
                        ,
                        HKPrice = model.HKPrice
                        ,
                        RebateDays = model.RebateDays
                        ,
                        RebateRatio = model.RebateRatio
                        ,
                        MarketPrice = model.MarketPrice
                        ,
                        PurchasePrice = model.PurchasePrice
                        ,
                        ActivityBottomPrice = model.ActivityBottomPrice
                        ,
                        SaleCount = model.SaleCount
                        ,
                        NotifyAdminForQuantityBelow = model.NotifyAdminForQuantityBelow
                        ,
                        PutawayDT = model.PutawayDT
                        ,
                        IsRecommend = model.IsRecommend
                        ,
                        RecommendSort = model.RecommendSort
                        ,
                        ExtensionPropertiesText = model.ExtensionPropertiesText
                        ,
                        Volume = model.Volume
                        ,
                        CreateBy = model.CreateBy
                        ,
                        CreateDT = DateTime.Now
                        ,
                        BrandID = model.BrandID
                        ,
                        SupplierId = model.SupplierId.GetValueOrDefault()
                        ,
                        FreeShipping = model.FreeShipping
                    };

                    tx.Product.Insert(product);
                    if (model.PrdoctRuleModels != null)
                    {
                        var PrdoctRuleModels = model.PrdoctRuleModels;
                        foreach (var lang in PrdoctRuleModels)
                        {
                            lang.ProductRuleId = MemCacheFactory.GetCurrentMemCache().Increment("logId");
                            lang.ProductId = product.ProductId;
                        }
                        if (PrdoctRuleModels.Count > 0)
                        {
                            tx.ProductRule.Insert(PrdoctRuleModels);
                        }
                    }

                    if (model.Product_LangModels != null)
                    {
                        var Product_LangModels = model.Product_LangModels;

                        foreach (var lang in Product_LangModels)
                        {
                            lang.ProductId = product.ProductId;  
                        }
                        if (Product_LangModels.Count > 0)
                        {
                            tx.Product_Lang.Insert(Product_LangModels);
                        }
                    }

                    if (model.SKU_ProductModels != null)
                    {
                        var SKU_ProductModels = model.SKU_ProductModels;
                        foreach (var lang in SKU_ProductModels)
                        {
                            lang.ProductId = product.ProductId;
                            lang.CreateDT = DateTime.Now;
                            lang.SKU_ProducId = MemCacheFactory.GetCurrentMemCache().Increment("logId");
                        }
                        if (SKU_ProductModels.Count > 0)
                        {
                            tx.SKU_Product.Insert(SKU_ProductModels);
                        }
                    }

                    if (model.SKU_ProductAttributesModels != null)
                    {
                        var SKU_ProductAttributesModels = model.SKU_ProductAttributesModels;
                        foreach (var lang in SKU_ProductAttributesModels)
                        {
                            lang.ProductId = product.ProductId;
                            lang.CreateDT = DateTime.Now;
                            lang.SKU_ProductAttributesId = MemCacheFactory.GetCurrentMemCache().Increment("logId");
                        }
                        if (SKU_ProductAttributesModels.Count > 0)
                        {
                            tx.SKU_ProductAttributes.Insert(SKU_ProductAttributesModels);
                        }
                    }
                    if (model.SKU_SKUItemsModels != null)
                    {
                        var SKU_SKUItemsModels = model.SKU_SKUItemsModels;
                        foreach (var lang in SKU_SKUItemsModels)
                        {
                            lang.ProductId = product.ProductId;
                            lang.CreateDT = DateTime.Now;
                            lang.SKU_SKUItemsId = MemCacheFactory.GetCurrentMemCache().Increment("logId");
                        }
                        if (SKU_SKUItemsModels.Count > 0)
                        {
                            tx.SKU_SKUItems.Insert(SKU_SKUItemsModels);
                        }
                    }

                    if (model.ProductPicModels != null)
                    {
                        var ProductPicModels = model.ProductPicModels;
                        foreach (var lang in ProductPicModels)
                        {
                            lang.ProductID = product.ProductId;                           
                            lang.ProductPicId = MemCacheFactory.GetCurrentMemCache().Increment("logId");
                        }
                        if (ProductPicModels.Count > 0)
                        {
                            tx.ProductPic.Insert(ProductPicModels);
                        }
                    }
                    if (model.ProductParametersModels != null)
                    {
                        var ProductParametersModels = model.ProductParametersModels;
                        foreach (var lang in ProductParametersModels)
                        {
                            lang.ProductId = product.ProductId;
                            lang.ParametersId = MemCacheFactory.GetCurrentMemCache().Increment("logId");
                        }
                        if (model.ProductParametersModels.Count > 0)
                        {
                            tx.ProductParameters.Insert(ProductParametersModels);
                        }
                    }

                    tx.Commit();
                    result.IsValid = true;
                }
                catch(Exception ex)
                {
                    tx.Rollback();
                    result.IsValid = false;
                    result.Messages = new List<string>() { " copy faild" };
                }
            }
            #endregion
            return result;
        }
        /// <summary>
        ///     添加产品
        /// </summary>
        /// <param name="model">AddProductModel</param>
        /// <returns>结果</returns>
        public ResultModel Add(AddProductModel model)
        {
            var result = new ResultModel();
            using (var tx = this._database.Db.BeginTransaction())
            {
                try
                {
                    var product = new NewProductModel
                    {
                        ProductId = model.ProductId ?? 0
                        ,
                        CategoryId = model.CategoryId
                        ,
                        MerchantID = model.MerchantID
                        ,
                        FareTemplateID = model.FareTemplateID
                        ,
                        ArtNo = model.ArtNo
                        ,
                        ProductBarcode = model.ProductBarcode
                        ,
                        Status = model.Status
                        ,
                        PostagePrice = model.PostagePrice
                        ,
                        StockQuantity = model.StockQuantity
                        ,
                        AllowCustomerReviews = model.AllowCustomerReviews
                        ,
                        AllowBackInStockSubscriptions = model.AllowBackInStockSubscriptions
                        ,
                        IsProvideInvoices = model.IsProvideInvoices
                        ,
                        Weight = model.Weight
                        ,
                        ProductParameter = model.ProductParameter
                        ,
                        PackingList = model.PackingList
                        ,
                        HKPrice = model.HKPrice
                        ,
                        RebateDays = model.RebateDays
                        ,
                        RebateRatio = model.RebateRatio
                        ,
                        MarketPrice = model.MarketPrice
                        ,
                        PurchasePrice = model.PurchasePrice
                        ,
                        ActivityBottomPrice = model.ActivityBottomPrice
                        ,
                        SaleCount = model.SaleCount
                        ,
                        NotifyAdminForQuantityBelow = model.NotifyAdminForQuantityBelow
                        ,
                        PutawayDT = model.PutawayDT
                        ,
                        IsRecommend = model.IsRecommend
                        ,
                        RecommendSort = model.RecommendSort
                        ,
                        ExtensionPropertiesText = model.ExtensionPropertiesText
                        ,
                        Volume = model.Volume
                        ,
                        CreateBy = model.CreateBy
                        ,
                        CreateDT = model.CreateDT
                        ,
                        BrandID = model.BrandID
                        ,
                        SupplierId = model.SupplierId.GetValueOrDefault()
                        ,
                        FreeShipping = model.FreeShipping
                    };

                    tx.Product.Insert(product);

                    tx.ProductRule.Insert(model.ProdctRuleModel);

                    if (model.Product_LangModels != null)
                    {
                        var Product_LangModels = model.Product_LangModels;

                        foreach (var lang in Product_LangModels)
                        {
                            if (model.ProductId.HasValue)
                                lang.ProductId = model.ProductId.Value;
                        }
                        if (Product_LangModels.Count > 0)
                        {
                            tx.Product_Lang.Insert(Product_LangModels);
                        }
                    }

                    if (model.SKU_ProductModels != null)
                    {
                        var SKU_ProductModels = model.SKU_ProductModels;
                        if (SKU_ProductModels.Count > 0)
                        {
                            tx.SKU_Product.Insert(SKU_ProductModels);
                        }
                    }

                    if (model.SKU_ProductAttributesModels != null)
                    {
                        var SKU_ProductAttributesModels = model.SKU_ProductAttributesModels;
                        if (SKU_ProductAttributesModels.Count > 0)
                        {
                            tx.SKU_ProductAttributes.Insert(SKU_ProductAttributesModels);
                        }
                    }
                    if (model.SKU_SKUItemsModels != null)
                    {
                        var SKU_SKUItemsModels = model.SKU_SKUItemsModels;
                        if (SKU_SKUItemsModels.Count > 0)
                        {
                            tx.SKU_SKUItems.Insert(SKU_SKUItemsModels);
                        }
                    }

                    if (model.ProductPicModels != null)
                    {
                        var ProductPicModels = model.ProductPicModels;
                        if (ProductPicModels.Count > 0)
                        {
                            tx.ProductPic.Insert(ProductPicModels);
                        }
                    }
                    if (model.ProductParametersModels != null)
                    {
                        if (model.ProductParametersModels.Count > 0)
                        {
                            tx.ProductParameters.Insert(model.ProductParametersModels);
                        }
                    }

                    tx.Commit();
                }
                catch
                {
                    tx.Rollback();
                    throw;
                }
            }


            return result;
        }

        /// <summary>
        ///     更新产品
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ResultModel Update(UpdateProductModel model)
        {
            var result = new ResultModel();

            IList<ProductModel.UpdateSKU_ProductModel> oldskuProducts = this._database.Db.SKU_Product.All()
                .Where(this._database.Db.SKU_Product.ProductId == model.ProductId)
                .ToList<ProductModel.UpdateSKU_ProductModel>();

            IList<UpdateSKU_ProductAttributesModel> oldskuProductAttrs = this._database.Db.SKU_ProductAttributes.All()
                .Where(this._database.Db.SKU_ProductAttributes.ProductId == model.ProductId)
                .ToList<UpdateSKU_ProductAttributesModel>();

            IList<UpdateSKU_SKUItemsModel> oldskuItems = this._database.Db.SKU_SKUItems.All()
                .Where(this._database.Db.SKU_SKUItems.ProductId == model.ProductId)
                .ToList<UpdateSKU_SKUItemsModel>();

            IList<ProductPicModel> oldpics =
                this._database.Db.ProductPic.All().Where(this._database.Db.ProductPic.ProductId == model.ProductId)
                    .ToList<ProductPicModel>();

            using (var tx = this._database.Db.BeginTransaction())
            {
                try
                {
                    dynamic productModel = new
                    {
                        ProductId = model.ProductId ?? 0
                        ,
                        model.CategoryId
                        ,
                        model.FareTemplateID
                        ,
                        model.ArtNo
                        ,
                        model.ProductBarcode
                        ,
                        model.Status
                        ,
                        model.PostagePrice
                        ,
                        model.StockQuantity
                        ,
                        model.AllowCustomerReviews
                        ,
                        model.AllowBackInStockSubscriptions
                        ,
                        model.IsProvideInvoices
                        ,
                        model.Weight
                        ,
                        model.ProductParameter
                        ,
                        model.PackingList
                        ,
                        model.HKPrice
                        ,
                        model.MarketPrice
                        ,
                        model.PurchasePrice
                        ,
                        model.ActivityBottomPrice
                        ,
                        //model.SaleCount
                        //,
                        model.NotifyAdminForQuantityBelow
                        ,
                        model.PutawayDT
                        ,
                        model.IsRecommend
                        ,
                        model.RecommendSort
                        ,
                        model.ExtensionPropertiesText
                        ,
                        model.Volume
                        ,
                        model.UpdateBy
                        ,
                        model.UpdateDT
                        ,
                        model.BrandID
                        ,
                        model.SupplierId
                        ,
                        model.FreeShipping,
                        model.RebateDays
                    };

                    tx.Product.UpdateByProductId(productModel);

                    if (model.Product_LangModels != null)
                    {
                        foreach (var productLangModel in model.Product_LangModels)
                        {
                            if (model.ProductId != null) productLangModel.ProductId = model.ProductId.Value;
                            if (productLangModel.Id != 0)
                            {
                                tx.Product_Lang.Update(productLangModel);
                            }
                            else
                            {
                                tx.Product_Lang.Insert(productLangModel);
                            }
                        }
                    }

                    #region SKU_Product

                    foreach (var item in oldskuProducts)
                    {
                        if (model.SKU_ProductModels.All(m => m.SKU_ProducId != item.SKU_ProducId))
                        {
                            tx.SKU_Product.Delete(SKU_ProducId: item.SKU_ProducId);
                        }
                    }

                    foreach (var item in model.SKU_ProductModels)
                    {
                        if (tx.SKU_Product.Any(tx.SKU_Product.SKU_ProducId == item.SKU_ProducId))
                        {
                            tx.SKU_Product.Update(item);
                        }
                        else
                        {
                            tx.SKU_Product.Insert(item);
                        }
                    }

                    #endregion

                    #region SKUProductAttr

                    foreach (var item in oldskuProductAttrs)
                    {
                        if (
                            model.SKU_ProductAttributesModels.All(
                                m => m.SKU_ProductAttributesId != item.SKU_ProductAttributesId))
                        {
                            tx.SKU_ProductAttributes.Delete(SKU_ProductAttributesId: item.SKU_ProductAttributesId);
                        }
                    }
                    foreach (var item in model.SKU_ProductAttributesModels)
                    {
                        if (
                            tx.SKU_ProductAttributes.Any(tx.SKU_ProductAttributes.SKU_ProductAttributesId ==
                                                         item.SKU_ProductAttributesId))
                        {
                            tx.SKU_ProductAttributes.Update(item);
                        }
                        else
                        {
                            tx.SKU_ProductAttributes.Insert(item);
                        }
                    }

                    #endregion

                    #region SKU_Items

                    foreach (var item in oldskuItems)
                    {
                        if (model.SKU_SKUItemsModels.All(m => m.SKU_SKUItemsId != item.SKU_SKUItemsId))
                        {
                            tx.SKU_SKUItems.Delete(SKU_SKUItemsId: item.SKU_SKUItemsId);
                        }
                    }
                    foreach (var item in model.SKU_SKUItemsModels)
                    {
                        if (tx.SKU_SKUItems.Any(tx.SKU_SKUItems.SKU_SKUItemsId == item.SKU_SKUItemsId))
                        {
                            tx.SKU_SKUItems.Update(item);
                        }
                        else
                        {
                            tx.SKU_SKUItems.Insert(item);
                        }
                    }

                    #endregion

                    #region ProductPic

                    foreach (var item in oldpics)
                    {
                        if (model.ProductPicModels.All(m => m.ProductPicId != item.ProductPicId))
                        {
                            tx.ProductPic.Delete(ProductPicId: item.ProductPicId);
                        }
                    }
                    foreach (var item in model.ProductPicModels)
                    {
                        if (tx.ProductPic.Any(tx.ProductPic.ProductPicId == item.ProductPicId))
                        {
                            tx.ProductPic.Update(item);
                        }
                        else
                        {
                            tx.ProductPic.Insert(item);
                        }
                    }

                    if (model.ProductId.HasValue)
                    {
                        tx.ProductParameters.DeleteAll(tx.ProductParameters.ProductId == model.ProductId.Value);
                    }

                    if (model.ProductParametersModels != null)
                    {
                        if (model.ProductParametersModels.Count > 0)
                        {
                            tx.ProductParameters.Insert(model.ProductParametersModels);
                        }
                    }

                    #endregion

                    tx.Commit();
                }
                catch
                {
                    result.IsValid = false;
                    tx.Rollback();
                    throw;
                }
            }

            result.Data = model;

            return result;
        }

        /// <summary>
        ///     批量删除
        /// </summary>
        /// <returns></returns>
        public ResultModel DeleteList(IList<long> ids)
        {
            var result = new ResultModel();

            result.Data = this._database.Db.Product.UpdateByProductId(ProductId: ids, IsDelete: true,
                Status: (int)ProductStatus.HasUnderShelves);

            return result;
        }

        /// <summary>
        ///     获取惠卡推荐数据
        /// </summary>
        /// <param name="languageid">显示的语言</param>
        /// <param name="userId">用户ID</param>
        /// <param name="tJCount">推荐条数</param>
        /// <returns></returns>
        public ResultModel GetTopRecommend(int languageid = 3, long userId = 0, int tJCount=100)
        {
            var result = new ResultModel();
            var tb = this._database.Db.Product;
            var tb1 = this._database.Db.ProductPic;
            var tb2 = this._database.Db.Product_Lang;
            var tb3 = this._database.Db.ProductRule;
            var tb4 = this._database.Db.Favorites;
            if (userId == 0)
            {
                result.Data = tb
                    .Join(tb2, ProductId: tb.ProductId)
                    .LeftJoin(tb1, ProductId: tb.ProductId)
                    .LeftJoin(tb3, ProductId: tb.ProductId)
                    .Select(tb.ProductId, tb1.PicUrl
                        , tb.HKPrice, tb.MarketPrice
                        , tb2.ProductName, tb2.Subheading
                        , tb3.SalesRuleId, tb3.StarDate, tb3.EndDate, tb3.Discount)
                    .Take(tJCount)
                    .Where(tb2.LanguageID == languageid && tb1.FLAG == 1 &&
                           tb.STATUS == 4 && tb.STOCKQUANTITY > 0 &&
                           tb.ISDELETE == 0)
                    .ToList<ProductInfo>();
            }
            else
            {
                result.Data = tb
                    .Join(tb2, ProductId: tb.ProductId)
                    .LeftJoin(tb1, ProductId: tb.ProductId)
                    .LeftJoin(tb3, ProductId: tb.ProductId)
                    .LeftJoin(tb4).On(tb4.ProductId == tb.ProductId && tb4.UserID == userId)
                    .Select(tb.ProductId, tb1.PicUrl
                        , tb.HKPrice, tb.MarketPrice
                        , tb2.ProductName, tb2.Subheading
                        , tb3.SalesRuleId, tb3.StarDate, tb3.EndDate, tb3.Discount
                        , tb4.UserID)
                    .Take(tJCount)
                    .Where(tb2.LanguageID == languageid && tb1.FLAG == 1 &&
                           tb.STATUS == 4 && tb.STOCKQUANTITY > 0 &&
                           tb.ISDELETE == 0)
                    .ToList<ProductInfo>();
            }

            return result;
        }


        ///// <summary>
        /////     获取惠卡推荐数据
        ///// </summary>
        ///// <param name="languageid">显示的语言</param>
        ///// <returns></returns>
        //public ResultModel GetTopRecommend(long productId, int top = 6, int languageid = 4, long userId = 0)
        //{
        //    var result = new ResultModel();
        //    var tb = this._database.Db.Product;
        //    var pc = this._database.Db.Category;
        //    var tb1 = this._database.Db.ProductPic;
        //    var tb2 = this._database.Db.Product_Lang;
        //    var tb3 = this._database.Db.ProductRule;
        //    var tb4 = this._database.Db.Favorites;

        //    if (userId == 0)
        //    {
        //        result.Data = tb
        //            .Join(pc, CategoryId: tb.CategoryId)
        //            .Join(tb2, ProductId: tb.ProductId)
        //            .LeftJoin(tb1, ProductId: tb.ProductId)
        //            .LeftJoin(tb3, ProductId: tb.ProductId)
        //            .Select(tb.ProductId
        //                , tb1.PicUrl
        //                , tb.HKPrice
        //                , tb.MarketPrice
        //                , tb2.ProductName
        //                , tb2.Subheading
        //                , tb3.SalesRuleId
        //                , tb3.StarDate
        //                , tb3.EndDate
        //                , tb3.Discount)
        //            .Where(tb2.LanguageID == languageid
        //            && tb1.FLAG == 1
        //            && tb.STATUS == 4
        //            && tb.STOCKQUANTITY > 0
        //            && tb.ISDELETE == 0
        //            && tb.ProductId != productId
        //            && tb.CategoryId == pc.CategoryId).OrderByDescending(tb.CategoryId).ThenByDescending(tb.PutawayDT).Take(top).ToList<ProductInfo>();
        //    }
        //    else
        //    {
        //        result.Data = tb.Join(pc, CategoryId: tb.CategoryId)
        //            .Join(tb2, ProductId: tb.ProductId)
        //            .LeftJoin(tb1, ProductId: tb.ProductId)
        //            .LeftJoin(tb3, ProductId: tb.ProductId)
        //            .LeftJoin(tb4).On(tb4.ProductId == tb.ProductId && tb4.UserID == userId)
        //            .Select(tb.ProductId
        //                , tb1.PicUrl
        //                , tb.HKPrice
        //                , tb.MarketPrice
        //                , tb2.ProductName
        //                , tb2.Subheading
        //                , tb3.SalesRuleId
        //                , tb3.StarDate
        //                , tb3.EndDate
        //                , tb3.Discount)
        //            .Where(tb2.LanguageID == languageid && tb1.FLAG == 1 &&
        //                   tb.STATUS == 4
        //                   && tb.STOCKQUANTITY > 0
        //                   && tb.ISDELETE == 0
        //                  && tb.ProductId != productId
        //            && tb.CategoryId == pc.CategoryId).OrderByDescending(tb.CategoryId).ThenByDescending(tb.PutawayDT).Take(top).ToList<ProductInfo>();
        //    }
        //    return result;
        //}


        /// <summary>
        ///     获取惠卡推荐数据(其他类型的，不排序 随意推荐)
        /// </summary>
        /// <param name="languageid">显示的语言</param>
        /// <returns></returns>
        public ResultModel GetTopRecommend(long productId, int top = 6, int languageid = 4)
        {
            var result = new ResultModel();
            var pc = this._database.Db.Product.FindByProductId(productId);
            var tb = this._database.Db.Product;
            var tb1 = this._database.Db.ProductPic;
            var tb2 = this._database.Db.Product_Lang;
            var tb3 = this._database.Db.ProductRule;

                result.Data = tb
                    .Join(tb2, ProductId: tb.ProductId)
                    .LeftJoin(tb1, ProductId: tb.ProductId)
                    .LeftJoin(tb3, ProductId: tb.ProductId)
                    .Select(tb.ProductId
                        , tb1.PicUrl
                        , tb.HKPrice
                        , tb.MarketPrice
                        , tb2.ProductName
                        , tb2.Subheading
                        , tb3.SalesRuleId
                        , tb3.StarDate
                        , tb3.EndDate
                        , tb3.Discount)
                    .Where(tb2.LanguageID == languageid && tb1.FLAG == 1 &&
                           tb.STATUS == 4
                           && tb.STOCKQUANTITY > 0
                           && tb.ISDELETE == 0
                           && tb.ProductId != pc.CategoryId).Take(top).ToList<ProductInfo>();  
            return result;
        }

        /// <summary>
        ///     获取惠卡推荐数据
        /// </summary>
        /// <param name="languageid">显示的语言</param>
        /// <returns></returns>
        public ResultModel GetTopRecommend(long productId, int top = 6, int languageid = 4, long userId = 0)
        {
            var result = new ResultModel();
            var tb = this._database.Db.Product;
            var pc = this._database.Db.Category;
            var tb1 = this._database.Db.ProductPic;
            var tb2 = this._database.Db.Product_Lang;
            var tb3 = this._database.Db.ProductRule;
            var tb4 = this._database.Db.Favorites;

            if (userId == 0)
            {
                result.Data = tb
                    .Join(pc, CategoryId: tb.CategoryId)
                    .Join(tb2, ProductId: tb.ProductId)
                    .LeftJoin(tb1, ProductId: tb.ProductId)
                    .LeftJoin(tb3, ProductId: tb.ProductId)
                    .Select(tb.ProductId
                        , tb1.PicUrl
                        , tb.HKPrice
                        , tb.MarketPrice
                        , tb2.ProductName
                        , tb2.Subheading
                        , tb3.SalesRuleId
                        , tb3.StarDate
                        , tb3.EndDate
                        , tb3.Discount)
                    .Where(tb2.LanguageID == languageid
                    && tb1.FLAG == 1
                    && tb.STATUS == 4
                    && tb.STOCKQUANTITY > 0
                    && tb.ISDELETE == 0
                    && tb.ProductId != productId
                    && tb.CategoryId == pc.CategoryId).OrderByDescending(tb.CategoryId).ThenByDescending(tb.PutawayDT).Take(top).ToList<ProductInfo>();
            }
            else
            {
                result.Data = tb.Join(pc, CategoryId: tb.CategoryId)
                    .Join(tb2, ProductId: tb.ProductId)
                    .LeftJoin(tb1, ProductId: tb.ProductId)
                    .LeftJoin(tb3, ProductId: tb.ProductId)
                    .LeftJoin(tb4).On(tb4.ProductId == tb.ProductId && tb4.UserID == userId)
                    .Select(tb.ProductId
                        , tb1.PicUrl
                        , tb.HKPrice
                        , tb.MarketPrice
                        , tb2.ProductName
                        , tb2.Subheading
                        , tb3.SalesRuleId
                        , tb3.StarDate
                        , tb3.EndDate
                        , tb3.Discount)
                    .Where(tb2.LanguageID == languageid && tb1.FLAG == 1 &&
                           tb.STATUS == 4
                           && tb.STOCKQUANTITY > 0
                           && tb.ISDELETE == 0
                          && tb.ProductId != productId
                    && tb.CategoryId == pc.CategoryId).OrderByDescending(tb.CategoryId).ThenByDescending(tb.PutawayDT).Take(top).ToList<ProductInfo>();
            }
            return result;
        }


        /// <summary>
        ///     获取惠卡推荐数据
        /// </summary>
        /// <param name="languageid">显示的语言</param>
        /// <returns></returns>
        public ResultModel GetTopRecommendForApi(int languageid = 3)
        {
            var result = new ResultModel();

            result.Data = this._database.Db.Product.ALL()
                .Select(this._database.Db.Product.ProductId, this._database.Db.ProductPic.PicUrl
                    , this._database.Db.Product.HKPrice, this._database.Db.Product.MarketPrice
                    , this._database.Db.Product_Lang.ProductName, this._database.Db.Product_Lang.Subheading
                    , this._database.Db.ProductRule.Discount, this._database.Db.ProductRule.StarDate
                    , this._database.Db.ProductRule.EndDate)
                .Join(this._database.Db.Product_Lang, ProductId: this._database.Db.Product.ProductId)
                .LeftJoin(this._database.Db.ProductPic, ProductId: this._database.Db.Product.ProductId)
                .LeftJoin(this._database.Db.ProductRule, ProductId: this._database.Db.Product.ProductId)
                .Take(100)
                .Where(this._database.Db.Product_Lang.LanguageID == languageid && this._database.Db.ProductPic.FLAG == 1 &&
                       this._database.Db.Product.STATUS == 4 && this._database.Db.Product.STOCKQUANTITY > 0 &&
                       this._database.Db.Product.ISDELETE == 0).ToList();

            return result;
        }

        /// <summary>
        ///     wuyf 是为推荐商品那验证用的 bannerProduct
        /// </summary>
        /// <param name="ProductId"></param>
        /// <returns></returns>
        public ResultModel GetBannerProductByProductId(long ProductId)
        {
            var product = this._database.Db.Product;
            var result = new ResultModel
            {
                Data =
                    this._database.Db.Product.FindAll(product.ProductId == ProductId && product.Status == 4)
                        .ToList<ProductModel>()
            };

            return result;
        }

        /// <summary>
        ///     取得商品信息
        /// </summary>
        /// <param name="id">商品Id</param>
        /// <param name="LanguageID">语言ID</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-16</remarks>
        public ResultModel GetProduct(long id, int LanguageID)
        {
            var result = new ResultModel();
            var product = this._database.Db.Product;
            var prodctLand = this._database.Db.Product_Lang;
            dynamic pl;
            var query = product.FindAll(product.ProductId == id && product.IsDelete == 0).
                LeftJoin(prodctLand, out pl).On(pl.ProductId == product.ProductId).
                Where(pl.LanguageID == LanguageID).
                Select(
                    product.ProductId,
                    product.HKPrice,
                    pl.ProductName,
                    product.Status
                );
            result = query.FirstOrDefault();
            return result;
        }

        /// <summary>
        ///     获取商品的分类规格数据
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public ResultModel GetProductCategoryTypeForSKU_Attributes(long productId)
        {
            var tb1 = this._database.Db.Product;
            var tb2 = this._database.Db.CategoryType;
            var tb3 = this._database.Db.SKU_ProductTypeAttribute;
            var tb4 = this._database.Db.SKU_Attributes;
            var result = new ResultModel();
            result.Data = tb1
                .Join(tb2, CategoryId: tb1.CategoryId)
                .Join(tb3, SkuTypeId: tb2.SkuTypeId)
                .Join(tb4, AttributeId: tb3.AttributeId)
                .Select(tb2.CategoryTypeId, tb2.CategoryId,
                    tb3.SKU_ProductTypeAttributeId, tb3.SkuTypeId,
                    tb3.AttributeType.As("SKU_P_AttributeType"), tb3.DisplaySequence,
                    tb4.AttributeId, tb4.AttributeName, tb4.AttributeType.As("SKU_AttributeType"),
                    tb4.IsSKU, tb4.UsageMode)
                .Where(tb1.ProductId == productId)
                .ToList<ProductCategoryTypeForSKU_Attributes>();
            return result;
        }

        /// <summary>
        /// 获取产品规格参数
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public ResultModel GetProductSpecifications(long productId)
        {
            var tb1 = this._database.Db.ProductParameters;
            var result = new ResultModel();
            result.Data = tb1.FindAll(tb1.ProductId == productId).
                Select(
                    tb1.ParametersId,
                    tb1.ProductId,
                    tb1.PName,
                    tb1.PValue,
                    tb1.GroupName,
                    tb1.Sort
                )
                .OrderBySort()
                .ToList<ProductParametersModel>();
            return result;

        }


        /// <summary>
        ///     根据产品id获取商品库存信息
        /// </summary>
        /// <param name="id">商品Id</param>
        /// <param name="languageId"></param>
        /// <returns></returns>
        public ResultModel GetSKU_ProductById(long id, int languageId)
        {
            var result = new ResultModel();

            result.Data = this._database.RunQuery(db =>
            {
                AddProductModel model = db.Product.get(id);
                if (model == null)
                {
                    model = new AddProductModel();
                }
                var pl =
                    db.Product_Lang.Find(db.Product_Lang.ProductId == id && db.Product_Lang.LanguageID == languageId);
                model.ProductName = pl != null ? pl.ProductName : string.Empty;
                var pr = db.ProductRule.Find(db.ProductRule.ProductId == id);
                model.Discount = pr != null ? pr.Discount : 0;
                model.SKU_ProductModels =
                    db.SKU_Product.FindAll(db.SKU_Product.ProductId == id).ToList<ProductModel.AddSKU_ProductModel>();
                return model;
            });
            return result;
        }

        /// <summary>
        ///     根据产品id获取商品信息（前台方法）
        /// </summary>
        /// <param name="model">商品Id</param>
        /// <returns></returns>
        public ResultModel SearchProduct(SearchProductModel model)
        {
            var result = new ResultModel();

            var q = this._database.Db.Product
                .Query()
                .LeftJoin(this._database.Db.Product_Lang.As("Product_LangModels"),
                    ProductId: this._database.Db.Product.ProductId)
                .LeftJoin(this._database.Db.Category, CategoryId: this._database.Db.Product.CategoryId)
                .LeftJoin(this._database.Db.Category_Lang.As("CategoryLanguageModel"),
                    CategoryId: this._database.Db.Category.CategoryId)
                .LeftJoin(this._database.Db.Brand, BrandID: this._database.Db.Product.BrandID)
                .LeftJoin(this._database.Db.Brand_Lang.As("Brand_LangModel"), BrandID: this._database.Db.Brand.BrandID)
                ;

            if (model.LanguageId.HasValue)
            {
                q = q
                    .Select(
                        this._database.Db.Product.ProductId
                        ,this._database.Db.Product.CategoryId
                        , q.CategoryLanguageModel.CategoryName
                        , q.Product_LangModels.ProductName
                        , q.Product_LangModels.Subheading
                        , q.Product_LangModels.LanguageId
                        , q.Brand_LangModel.BrandName
                        , this._database.Db.Product.ArtNo
                        , this._database.Db.Product.PostagePrice
                        , this._database.Db.Product.StockQuantity
                        , this._database.Db.Product.AllowCustomerReviews
                        , this._database.Db.Product.AllowBackInStockSubscriptions
                        , this._database.Db.Product.IsProvideInvoices
                        , this._database.Db.Product.Weight
                        , this._database.Db.Product.HKPrice
                        , this._database.Db.Product.ProductParameter
                        , this._database.Db.Product.PackingList
                        , this._database.Db.Product.MarketPrice
                        , this._database.Db.Product.PurchasePrice
                        , this._database.Db.Product.ActivityBottomPrice
                        , this._database.Db.Product.SaleCount
                        , this._database.Db.Product.NotifyAdminForQuantityBelow
                        , this._database.Db.Product.PutawayDT
                        , this._database.Db.Product.IsRecommend
                        , this._database.Db.Product.RecommendSort
                        , this._database.Db.Product.IsDelete
                        , this._database.Db.Product.ExtensionPropertiesText
                        , this._database.Db.Product.Volume
                        , this._database.Db.Product.CreateBy
                        , this._database.Db.Product.CreateDT
                        , this._database.Db.Product.UpdateBy
                        , this._database.Db.Product.UpdateDT
                        , this._database.Db.Product.Status
                        , this._database.Db.Brand.BrandUrl
                        , q.Product_LangModels.Introduction //商品详情页 商品描述
                    )
                    .Where(q.Product_LangModels.LanguageId == null ||
                           q.Product_LangModels.LanguageId == model.LanguageId.Value)
                    .Where(q.CategoryLanguageModel.LanguageId == null ||
                           q.CategoryLanguageModel.LanguageId == model.LanguageId.Value)
                    .Where(q.Brand_LangModel.LanguageId == null ||
                           q.Brand_LangModel.LanguageId == model.LanguageId.Value)
                    ;
            }
            else
            {
                q = q
                    .WithMany(q.Product_LangModels)
                    .WithOne(q.Brand_LangModel)
                    .WithOne(q.CategoryLanguageModel)
                    ;
            }

            q = q.Where(this._database.Db.Product.IsDelete == false);
            if (model.Status.HasValue)
            {
                q = q.Where(this._database.Db.Product.Status == model.Status.Value);
            }
            if (!string.IsNullOrEmpty(model.ProductName))
            {
                q = q.Where(q.Product_LangModels.ProductName.Like('%' + model.ProductName + '%'));
            }

            if (!string.IsNullOrEmpty(model.CategoryName))
            {
                q = q.Where(q.CategoryLanguageModel.CategoryName.Like('%' + model.CategoryName + '%'));
            }
            if (!string.IsNullOrEmpty(model.BrandName))
            {
                q = q.Where(q.Brand_LangModel.BrandName.Like('%' + model.BrandName + '%'));
            }
            if (model.ProductId.HasValue)
            {
                q = q.Where(this._database.Db.Product.ProductId == model.ProductId.Value);
            }

            result.Data = model.ProductId.HasValue
                ? q.Where(this._database.Db.Product.ProductId == model.ProductId.Value).FirstOrDefault()
                : new SimpleDataPagedList<ProductInfo>(q, model.PagedIndex, model.PagedSize);

            return result;
        }

        public ResultModel SearchProductShow(SearchProductModel model)
        {

            var result = new ResultModel();

            var q = this._database.Db.Product
                .Query()
                .LeftJoin(this._database.Db.Product_Lang.As("Product_LangModels"),
                    ProductId: this._database.Db.Product.ProductId)
                .LeftJoin(this._database.Db.Category, CategoryId: this._database.Db.Product.CategoryId)
                .LeftJoin(this._database.Db.Category_Lang.As("CategoryLanguageModel"),
                    CategoryId: this._database.Db.Category.CategoryId)
                .LeftJoin(this._database.Db.Brand, BrandID: this._database.Db.Product.BrandID)
                .LeftJoin(this._database.Db.Brand_Lang.As("Brand_LangModel"), BrandID: this._database.Db.Brand.BrandID)
                ;

            if (model.LanguageId.HasValue)
            {
                q = q
                    .Select(
                        this._database.Db.Product.ProductId
                        , this._database.Db.Product.CategoryId
                        , q.CategoryLanguageModel.CategoryName
                        , q.Product_LangModels.ProductName
                        , q.Product_LangModels.Subheading
                        , q.Product_LangModels.LanguageId
                        , q.Brand_LangModel.BrandName
                        , this._database.Db.Product.ArtNo
                        , this._database.Db.Product.PostagePrice
                        , this._database.Db.Product.StockQuantity
                        , this._database.Db.Product.AllowCustomerReviews
                        , this._database.Db.Product.AllowBackInStockSubscriptions
                        , this._database.Db.Product.IsProvideInvoices
                        , this._database.Db.Product.Weight
                        , this._database.Db.Product.HKPrice
                        , this._database.Db.Product.ProductParameter
                        , this._database.Db.Product.PackingList
                        , this._database.Db.Product.MarketPrice
                        , this._database.Db.Product.PurchasePrice
                        , this._database.Db.Product.ActivityBottomPrice
                        , this._database.Db.Product.SaleCount
                        , this._database.Db.Product.NotifyAdminForQuantityBelow
                        , this._database.Db.Product.PutawayDT
                        , this._database.Db.Product.IsRecommend
                        , this._database.Db.Product.RecommendSort
                        , this._database.Db.Product.IsDelete
                        , this._database.Db.Product.ExtensionPropertiesText
                        , this._database.Db.Product.Volume
                        , this._database.Db.Product.CreateBy
                        , this._database.Db.Product.CreateDT
                        , this._database.Db.Product.UpdateBy
                        , this._database.Db.Product.UpdateDT
                        , this._database.Db.Product.Status
                        , this._database.Db.Brand.BrandUrl
                        , q.Product_LangModels.Introduction //商品详情页 商品描述
                    )
                    .Where(q.Product_LangModels.LanguageId == null ||
                           q.Product_LangModels.LanguageId == model.LanguageId.Value)
                    .Where(q.CategoryLanguageModel.LanguageId == null ||
                           q.CategoryLanguageModel.LanguageId == model.LanguageId.Value)
                    .Where(q.Brand_LangModel.LanguageId == null ||
                           q.Brand_LangModel.LanguageId == model.LanguageId.Value)
                    ;
            }
            else
            {
                q = q
                    .WithMany(q.Product_LangModels)
                    .WithOne(q.Brand_LangModel)
                    .WithOne(q.CategoryLanguageModel)
                    ;
            }

            q = q.Where(this._database.Db.Product.IsDelete == false);
            //if (model.Status.HasValue)
            //{
            //    q = q.Where(this._database.Db.Product.Status == model.Status.Value);
            //}
            if (!string.IsNullOrEmpty(model.ProductName))
            {
                q = q.Where(q.Product_LangModels.ProductName.Like('%' + model.ProductName + '%'));
            }

            if (!string.IsNullOrEmpty(model.CategoryName))
            {
                q = q.Where(q.CategoryLanguageModel.CategoryName.Like('%' + model.CategoryName + '%'));
            }
            if (!string.IsNullOrEmpty(model.BrandName))
            {
                q = q.Where(q.Brand_LangModel.BrandName.Like('%' + model.BrandName + '%'));
            }
            if (model.ProductId.HasValue)
            {
                q = q.Where(this._database.Db.Product.ProductId == model.ProductId.Value);
            }

            result.Data = model.ProductId.HasValue
                ? q.Where(this._database.Db.Product.ProductId == model.ProductId.Value).FirstOrDefault()
                : new SimpleDataPagedList<ProductInfo>(q, model.PagedIndex, model.PagedSize);

            return result;
        }
        /// <summary>
        ///     生成商品库存更新Sql语句
        /// </summary>
        /// <param name="view">商品实体</param>
        /// <returns>Sql语句</returns>
        internal string GenerateUpdateStockQuantitySql(ProductView view)
        {
            string sql = string.Empty;
            if (view.SaleCount > 0)
            {
                sql =
                    string.Format(
                        " UPDATE Product SET StockQuantity=StockQuantity+{0},SaleCount=SaleCount+{1} WHERE ProductId={2}",
                        view.StockQuantity, view.SaleCount, view.ProductId);
            }
            else
            {
                sql =
                    string.Format(
                        " UPDATE Product SET StockQuantity=StockQuantity+{0} WHERE ProductId={1}",
                        view.StockQuantity, view.ProductId);
            }

            return sql;
        }

        /// <summary>
        ///     生成销售数量更新Sql语句
        /// </summary>
        /// <param name="view">商品实体</param>
        /// <returns>Sql语句</returns>
        internal string GenerateUpdateSaleCountSql(ProductView view)
        {
            var sql = string.Format(" UPDATE Product SET SaleCount=SaleCount+{0} WHERE ProductId={1}",
                view.SaleCount, view.ProductId);

            return sql;
        }
        /// <summary>
        /// 获取产品详情页的面包屑导航
        /// </summary>
        /// <param name="ProductId">产品ID</param>
        /// <param name="Lang">语言ID</param>
        /// <returns></returns>
        public ResultModel GetProductPath(long ProductId, int Lang = 4)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT P.[ProductId],PL.ProductName,CL1.CategoryId AS 'CategoryId1',CL1.CategoryName AS 'CategoryName1', ");
            sb.Append("CL2.CategoryId AS 'CategoryId2',CL2.CategoryName AS 'CategoryName2',CL3.CategoryId AS 'CategoryId3',CL3.CategoryName AS 'CategoryName3' ");
            sb.Append("FROM [Product] P JOIN [Category] C1 ON P.CategoryId=C1.CategoryId JOIN [Category] C2  ON C1.parentId=C2.CategoryId ");
            sb.Append("JOIN [Category] C3 ON C2.parentId=C3.CategoryId JOIN [Category_Lang] CL1 ON C1.CategoryId=CL1.CategoryId ");
            sb.Append("JOIN [Category_Lang] CL2 ON C2.CategoryId=CL2.CategoryId JOIN [Category_Lang] CL3 ON C3.CategoryId=CL3.CategoryId ");
            sb.Append("JOIN [Product_Lang] PL ON P.ProductId=PL.ProductId ");
            sb.Append("WHERE P.ProductId={0} AND CL1.LanguageID={1} AND CL2.LanguageID={2} AND CL3.LanguageID={3} AND PL.LanguageID={4}");
            string sql = string.Format(sb.ToString(),ProductId,Lang,Lang,Lang,Lang);
            List<dynamic> list = _database.RunSqlQuery(x => x.ToResultSets(sql))[0];
            List<ProductPath> datalist = list.ToEntity<ProductPath>();
            ResultModel result = new ResultModel();
            result.Data = datalist;
            result.IsValid = true;
            return result;
        }

        /// <summary>
        /// 根据产品id获取购物车商品列表
        /// </summary>
        /// <param name="userid">用户id</param>
        /// <param name="languageId">语言id</param>
        /// <returns>购物车商品模型</returns>
        public ResultModel GetProductListByPrdouctId(string productIds, string productSkuIds, int languageId)
        {
            StringBuilder sb = new StringBuilder();
            ResultModel result = new ResultModel { IsValid = false };
            string sql = @"SELECT 0 as 'CartsId',p.FareTemplateID,p.Volume,sp.PurchasePrice,m.IsProvideInvoices,sp.Stock AS 'StockQuantity',p.PostagePrice,p.SupplierId,
                            p.ProductID AS 'GoodsId',p.FreeShipping,p.Weight,p.RebateDays,p.RebateRatio,0 As 'Count',pp.PicUrl As 'Pic',pl.ProductName AS 'GoodsName',
                            Case When (pr.StarDate<=GETDATE() and pr.EndDate>=GETDATE() and pr.SalesRuleId=2 and pr.Discount>0) Then sp.HKPrice*pr.Discount Else sp.HKPrice End As 'GoodsUnits',
                            sp.MarketPrice,p.MerchantID AS 'ComId',m.ShopName AS 'ComName',p.Status,p.MerchantID,(SELECT  reverse(stuff(reverse((
                                                      SELECT DISTINCT BBB.AttributeName+',' 
                                                      FROM dbo.SKU_AttributeValues AAA  
                                                      JOIN dbo.SKU_Attributes BBB ON AAA.AttributeId=BBB.AttributeId
                                                      WHERE CHARINDEX(convert(varchar,AAA.ValueId)+'_',sp.SKUStr)>0
                                                            OR CHARINDEX('_'+convert(varchar,AAA.ValueId),sp.SKUStr)>0
                                                            OR sp.SKUStr=convert(varchar,AAA.ValueId) FOR XML PATH(''))),1,1,''))) As 'AttributeName',
                            sp.SkuName As 'ValueStr',sp.SKU_ProducId As 'SkuNumber'
                            From Product p 
                            LEFT JOIN ProductPic pp on p.ProductID = pp.ProductId  
                            LEFT JOIN Product_Lang pl ON p.ProductID = pl.ProductId 
                            LEFT JOIN YH_MerchantInfo m ON p.MerchantID=m.MerchantID 
                            LEFT JOIN SKU_Product sp on sp.ProductId = p.ProductId 
                            LEFT JOIN ProductRule pr ON p.ProductID = pr.ProductId 
                            WHERE pp.Flag = 1 
                            AND pl.LanguageID ={0} 
                            AND p.ProductId IN ({1})
                            AND sp.SKU_ProducId IN ({2})";
            sb.AppendFormat(sql, languageId, productIds, productSkuIds);
            var data = _database.RunSqlQuery(x => x.ToResultSets(sb.ToString()));
            List<dynamic> sources = data[0];
           var resultData = sources.ToEntity<GoodsInfoModel>();
            foreach (var prodcut in resultData)
            {
                //prodcut.Pic = GetConfig.FullPath() + prodcut.Pic;
                prodcut.Pic = HtmlExtensions.GetImagesUrl(prodcut.Pic, 72, 72);
                prodcut.AddToShoppingCartTime = DateTimeExtensions.DateTimeToString(prodcut.CartDate);
            }

            // 构造以商家分组的数据结构
            var lstAllComId = resultData.Select(m => m.ComId).Distinct();
            var lstRslt = lstAllComId.Select(comId =>
            {
                return new ComInfo
                {
                    ComId = comId,
                    ComName =
                        resultData.FirstOrDefault(m => String.Equals(m.ComId, comId, StringComparison.OrdinalIgnoreCase))
                            .ComName,
                    Goods =
                        resultData.Where(m => String.Equals(m.ComId, comId, StringComparison.OrdinalIgnoreCase)).ToList()
                };
            }).ToList();

            // 商品倒序
            lstRslt.ForEach(c => c.Goods = c.Goods.OrderByDescending(g => g.AddToShoppingCartTime).ToList());
            // 商家倒序
            lstRslt = lstRslt.OrderByDescending(c => c.Goods.Max(g => g.AddToShoppingCartTime)).ToList();
            result.Data = lstRslt;
            result.IsValid = true;
            return result;

        }

    }
}