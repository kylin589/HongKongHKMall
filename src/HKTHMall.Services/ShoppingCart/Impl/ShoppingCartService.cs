using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKTHMall.Core;
using HKTHMall.Core.Config;
using HKTHMall.Core.Extensions;
using HKTHMall.Core.Sql;
using HKTHMall.Domain.Entities;
using HKTHMall.Domain.Models;
using HKTHMall.Domain.WebModel.Models.ShoppingCart;
using Simple.Data;
using Simple.Data.RawSql;
using BrCms.Framework.Collections;

namespace HKTHMall.Services.ShoppingCart.Impl
{
    public class ShoppingCartService : BaseService, IShoppingCartService
    {
        /// <summary>
        /// 加入购物车
        /// </summary>
        /// <param name="model">购物车模型</param>
        public ResultModel AddShoppingCart(ShoppingCartModel model)
        {
            var result = new ResultModel
            {
                Data = _database.Db.ShoppingCart.Insert(model)
            };
            return result;
        }

        /// <summary>
        /// 查询购物车商品数量（吴育富）
        /// </summary>
        /// <param name="UserID">用户ID</param>
        /// <param name="ProductID">商品ID</param>
        /// <param name="SKU_ProducId">商品SKU</param>
        /// <returns></returns>
        public ResultModel GetShoppingCartUserID(long UserID, long ProductID, long SKU_ProducId)
        {
            var tb = base._database.Db.ShoppingCart;
            var where = new SimpleExpression(1, 1, SimpleExpressionType.Equal);

            if (UserID > 1)
            {

                where = new SimpleExpression(where, tb.UserID == UserID, SimpleExpressionType.And);
            }
            if (ProductID > 1)
            {

                where = new SimpleExpression(where, tb.ProductID == ProductID, SimpleExpressionType.And);
            }
            if (SKU_ProducId > 1)
            {

                where = new SimpleExpression(where, tb.SKU_ProducId == SKU_ProducId, SimpleExpressionType.And);
            }





            var result = new ResultModel()
            {
                Data = new SimpleDataPagedList<ShoppingCartModel>(base._database.Db.ShoppingCart.FindAll(where).OrderByUserID(), 0, 1000)

            };

            return result;
        }

        /// <summary>
        /// 根据商品ID和商品SKU_ProductID查询产品数量
        /// </summary>
        /// <param name="ProductId"></param>
        /// <param name="SKU_ProductID"></param>
        /// <returns></returns>
        public ResultModel GetSKU_Product(long ProductId, long SKU_ProductID)
        {
            var result = new ResultModel();

            result.Data = _database.Db.SKU_Product.Find(_database.Db.SKU_Product.ProductId == ProductId && _database.Db.SKU_Product.SKU_ProducId == SKU_ProductID);

            return result;
        }

        /// <summary>
        /// 查询购物车总数量（吴育富）
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public ResultModel GetShoppingCartUserIDSum(long UserID)
        {
            var sr = _database.Db.ShoppingCart;//商品促销
            var product = _database.Db.Product;//订单对象

            var whereParam = new SimpleExpression(1, 1, SimpleExpressionType.Equal);
            whereParam = new SimpleExpression(whereParam, _database.Db.Product.Status == 4, SimpleExpressionType.And);
            whereParam = new SimpleExpression(whereParam, _database.Db.Product.StockQuantity > 0, SimpleExpressionType.And);
            if (UserID != 0)
            {
                whereParam = new SimpleExpression(whereParam, sr.UserID == UserID, SimpleExpressionType.And);
            }




            dynamic pd;
            var sd = sr.Query().
                LeftJoin(product, out pd).On(pd.ProductId == sr.ProductID)
                .Select(
                    sr.ShoppingCartId
                    , sr.ProductID
                    , sr.SKU_ProducId
                    , sr.UserID
                    , sr.Quantity
                    , sr.CartDate
                    , sr.IsCheck
               )
               .Where(whereParam);

            var result = new ResultModel
            {
                Data = new SimpleDataPagedList<ShoppingCartModel>(sd,
                        0, 1000)

            };
            return result;
        }

        /// <summary>添加商品到购物车.</summary>
        /// <remarks></remarks>
        /// <param name="lstGoodsIdToAdd">商品ID.</param>
        /// <param name="lstSkuNumber">商品SKUID.</param>
        /// <param name="lstCount">数量.</param>
        /// <param name="userid">用户ID.</param>
        /// <returns>System.Object.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public ResultModel AddToShoppingCart(List<string> lstGoodsIdToAdd, List<string> lstSkuNumber, List<string> lstCount, string userid = null)
        {
            // 获取数据库购物车中已有商品
            // 存在则修改数量,不存在则添加
            // 更新至数据库
            var lstFromDb = this.GetShoppingCartInfo(userid);
            List<ShoppingCartModel> lstshoppingcart = lstFromDb.Data;
            // 获取数据库购物车中已有商品
            // 存在则修改数量,不存在则添加
            // 更新至数据库
            var result = new ResultModel();
            var Index = 0;
            foreach (var id in lstGoodsIdToAdd)
            {
                var SkuNumber = lstSkuNumber[Index];
                if (Convert.ToInt32(lstCount[Index]) <= 0)
                {
                    Index++;
                    continue;
                }
                ////取SKUID(加入购物车传参SKU字串时使用)
                //var sKU_ProducId = _database.Db.SKU_Product.All().Select(_database.Db.SKU_Product.SKU_ProducId).Where(_database.Db.SKU_Product.SKUStr == SkuNumber && _database.Db.SKU_Product.ProductId == id).ToArray()[0].SKU_ProducId;
                //var memInDb = lstshoppingcart.FirstOrDefault(m => String.Equals(m.ProductID.ToString(), id, StringComparison.OrdinalIgnoreCase) && String.Equals(m.SKU_ProducId.ToString(), sKU_ProducId.ToString(), StringComparison.OrdinalIgnoreCase));
                var memInDb = lstshoppingcart.FirstOrDefault(m => String.Equals(m.ProductID.ToString(), id, StringComparison.OrdinalIgnoreCase) && String.Equals(m.SKU_ProducId.ToString(), SkuNumber, StringComparison.OrdinalIgnoreCase));
                if (memInDb != null)
                {
                    //库存取SKU库存SKU_Product（Stock）
                    var productEntity = _database.Db.SKU_Product.FindBySKU_ProducId(memInDb.SKU_ProducId);

                    //var productEntity = _database.Db.Product.FindByProductId(id);
                    if (productEntity == null ||
                        (memInDb.Quantity + 1) > productEntity.Stock && productEntity.Stock != -1)
                    {
                        Index++;
                        continue;
                    }
                    memInDb.Quantity += Convert.ToInt32(lstCount[Index]);
                }
                else
                {
                    ////库存取SKU库存SKU_Product（Stock）
                    //var productEntity = _database.Db.SKU_Product.FindBySKU_ProducId(Convert.ToInt64(SkuNumber));
                    //if (productEntity == null ||
                    //    (Convert.ToInt32(lstCount[Index]) + 1) > productEntity.Stock && productEntity.Stock != -1)
                    //{
                    //    Index++;
                    //    continue;
                    //}
                    lstshoppingcart.Add(new ShoppingCartModel
                    {
                        ProductID = Convert.ToInt64(id),
                        CartDate = DateTime.Now,
                        Quantity = Convert.ToInt32(lstCount[Index]),
                        ////取SKUID(加入购物车传参SKU字串时使用)
                        //SKU_ProducId = sKU_ProducId,
                        SKU_ProducId = Convert.ToInt64(SkuNumber),
                        IsCheck = 1
                    });
                }
                Index++;
            }

            result.IsValid = this.updateCartInDb(lstshoppingcart, userid);

            return result;
        }

        /// <summary>获取当前用户的数据库购物车.</summary>
        /// <remarks></remarks>
        /// <returns>List{GoodsInfo}.</returns>
        /// <exception cref="System.Exception">未登录用户</exception>
        public List<GoodsInfoModel> getCartFromDb(int languageId, string userid = null, YH_User u = null)
        {
            //if (u == null)
            //{
            //    u = SessionHelper.getCurUserFromSession();
            //}
            //if (string.IsNullOrEmpty(userid) && u == null)
            //{
            //    throw new Exception("未登录用户");
            //}
            //if (!string.IsNullOrEmpty(userid))
            //{
            ResultModel resul = GetShoppingCartByUserId(userid, languageId);
            return resul.Data;
            //}
            //else
            //{
            //    ResultModel resul = GetShoppingCartByUserId(u.UserID.ToString(), languageId);
            //    return resul.Data;
            //}
        }

        /// <summary>更新当前用户的数据库购物车.</summary>
        /// <remarks></remarks>
        /// <author>Yun 2015-04-19 10:16:35</author>
        /// <param name="lstGoodsInfoNew">The LST goods information new.</param>
        /// <returns>Boolean.</returns>
        public Boolean updateCartInDb(List<ShoppingCartModel> lstGoodsInfoNew, string userid = null)
        {
            lstGoodsInfoNew = lstGoodsInfoNew.Distinct(new GoodsInfoEqualityComparer()).ToList();
            //YH_User u = _database.Db.YH_User.FindByUserID(userid);
            var Index = 0;
            var result = new ResultModel();
            foreach (var newGood in lstGoodsInfoNew)
            {
                using (var transaction = _database.Db.BeginTransaction())
                {
                    var resultq = new ResultModel
                    {
                        Data =
                        _database.Db.ShoppingCart.All()
                        .Select(_database.Db.ShoppingCart.ShoppingCartId)
                        .Where(_database.Db.ShoppingCart.ProductID == newGood.ProductID && _database.Db.ShoppingCart.SKU_ProducId == newGood.SKU_ProducId && _database.Db.ShoppingCart.UserID == userid)
                        .ToList<ShoppingCartModel>()
                    };

                    var id = resultq.Data.Count > 0 ? resultq.Data[0].ShoppingCartId : null;

                    if (id != null)
                    {

                        // 更新
                        dynamic updateRecord = new SimpleRecord();
                        updateRecord.ShoppingCartId = id;
                        updateRecord.ProductID = newGood.ProductID;
                        updateRecord.SKU_ProducId = newGood.SKU_ProducId;
                        updateRecord.UserID = userid;
                        updateRecord.Quantity = newGood.Quantity;
                        updateRecord.IsCheck = newGood.IsCheck;
                        transaction.ShoppingCart.UpdateByShoppingCartId(updateRecord);
                    }
                    else
                    {
                        //添加
                        dynamic insertRecord = new SimpleRecord();
                        insertRecord.ShoppingCartId = MemCacheFactory.GetCurrentMemCache().Increment("commonId");
                        insertRecord.ProductID = newGood.ProductID;
                        insertRecord.SKU_ProducId = newGood.SKU_ProducId;
                        insertRecord.UserID = userid;
                        insertRecord.Quantity = newGood.Quantity;
                        insertRecord.CartDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        insertRecord.IsCheck = newGood.IsCheck;
                        transaction.ShoppingCart.Insert(insertRecord);
                    }
                    transaction.Commit();
                }
            }
            return result.IsValid;
        }

        /// <summary>
        /// 获取当前用户的数据库购物车
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="u"></param>
        /// <returns></returns>
        private ResultModel GetShoppingCartInfo(string userid = null, YH_User u = null)
        {
            var sc = _database.Db.ShoppingCart; //购物车表
            ResultModel result = new ResultModel
            {

                Data = sc.FindAllByUserID(userid)
                    .Select(
                        sc.ShoppingCartId,
                        sc.ProductID,
                        sc.SKU_ProducId,
                        sc.Quantity,
                        sc.UserID,
                        sc.IsCheck
                    ).ToList<ShoppingCartModel>()
            };
            return result;
        }

        /// <summary>
        /// 根据用户id获取购物车商品
        /// </summary>
        /// <param name="userid">用户id</param>
        /// <param name="languageId">语言id</param>
        /// <returns>购物车商品模型</returns>
        public ResultModel GetShoppingCartByUserId(string userid, int languageId)
        {
            StringBuilder sb = new StringBuilder();
            ResultModel result = new ResultModel { IsValid = false };
            string sql = @"Select p.FareTemplateID,p.Volume,sp.PurchasePrice,sc.ShoppingCartId As 'CartsId',m.IsProvideInvoices,sp.Stock AS 'StockQuantity',p.PostagePrice,p.SupplierId,
                            p.ProductID AS 'GoodsId',p.FreeShipping,p.Weight,p.RebateDays,p.RebateRatio,sc.Quantity As 'Count',pp.PicUrl As 'Pic',pl.ProductName AS 'GoodsName',
                            Case When (pr.StarDate<=GETDATE() and pr.EndDate>=GETDATE() and pr.SalesRuleId=2 and pr.Discount>0) Then sp.HKPrice*pr.Discount Else sp.HKPrice End As 'GoodsUnits',
                            sp.MarketPrice,p.MerchantID AS 'ComId',m.ShopName AS 'ComName',p.Status,p.MerchantID,(SELECT  reverse(stuff(reverse((
                                                              SELECT DISTINCT BBB.AttributeName+',' FROM dbo.SKU_AttributeValues AAA  JOIN dbo.SKU_Attributes BBB ON AAA.AttributeId=BBB.AttributeId
                                                              WHERE CHARINDEX(convert(varchar,AAA.ValueId)+'_',sp.SKUStr)>0
                                                                    OR CHARINDEX('_'+convert(varchar,AAA.ValueId),sp.SKUStr)>0
                                                                    OR sp.SKUStr=convert(varchar,AAA.ValueId) FOR XML PATH(''))),1,1,''))) As 'AttributeName',
                            sp.SkuName As 'ValueStr',sp.SKU_ProducId As 'SkuNumber',sc.IsCheck As 'IsChecked',sc.CartDate
                            From ShoppingCart sc  LEFT JOIN Product p on sc.ProductID = p.ProductId  
                            LEFT JOIN ProductPic pp on p.ProductID = pp.ProductId  LEFT JOIN Product_Lang pl ON p.ProductID = pl.ProductId 
                            LEFT JOIN YH_MerchantInfo m ON p.MerchantID=m.MerchantID LEFT JOIN SKU_Product sp on sc.SKU_ProducId = sp.SKU_ProducId 
                            LEFT JOIN ProductRule pr ON sc.ProductID = pr.ProductId Where pp.Flag = 1 And pl.LanguageID ={0} And sc.UserID ={1};";
            sb.AppendFormat(sql, languageId, userid);
            var data = _database.RunSqlQuery(x => x.ToResultSets(sb.ToString()));
            List<dynamic> sources = data[0];
            result.Data = sources.ToEntity<GoodsInfoModel>();
            foreach (var prodcut in result.Data)
            {
                //prodcut.Pic = GetConfig.FullPath() + prodcut.Pic;
                prodcut.Pic = HtmlExtensions.GetImagesUrl(prodcut.Pic, 72, 72);
                prodcut.AddToShoppingCartTime = DateTimeExtensions.DateTimeToString(prodcut.CartDate);
            }
            result.IsValid = true;
            return result;

        }




        /// <summary>   
        /// 购物车商品单个商品数量修改 （2015.08.08 戴勇军修改）
        /// </summary> 
        /// <param name="userId	">用户ID</param>
        ///  <param name="ShoppingCartId">购物车ID</param>
        /// <param name="productId">商品编号ID</param>
        /// <param name="quantity">修改后的数量</param>
        /// <returns></returns>
        public ResultModel UpdateShoppingCartByCountData(string userid, long ShoppingCartId, int productId, int quantity)
        {
            var lstFromDb = this.GetShoppingCartInfo(userid); //查找用户购物车已有商品
            var result = new ResultModel();

            for (var i = 0; i < lstFromDb.Data.Count; i++)
            {
                var inDb = lstFromDb.Data[i];
                if (inDb.ProductID == productId)
                {
                    if (inDb.ShoppingCartId == ShoppingCartId)
                    {
                        inDb.Quantity = quantity; //要更新的购买数量
                        break;
                    }
                }
            };
            result.IsValid = this.updateCartInDb(lstFromDb.Data, userid);
            return result;
        }

        /// <summary>删除购物车中的指定商品.</summary>
        /// <remarks></remarks>
        /// <author>Yun 2015-04-19 09:51:04</author>
        /// <param name="lstGoodsId">The LST goods identifier.</param>
        /// <returns>System.Object.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public ResultModel deleteGoodsInShoppingCart(List<string> lstCartsId, string userid = null)
        {
            // 获取数据库购物车中已有商品
            // 查找要删除的,并删除
            // 更新至数据库
            var result = new ResultModel();
            using (var transaction = _database.Db.BeginTransaction())
            {
                for (int i = 0; i < lstCartsId.Count; i++)
                {
                    result.Data += transaction.ShoppingCart.DeleteByShoppingCartId(lstCartsId[i]);
                }
                transaction.Commit();

            }
            return result;
        }

        /// <summary>获取以商家分组的商品信息.</summary>
        /// <remarks></remarks>
        /// <author>ywd</author>
        /// <param name="getChecked">是否只获取已选中的商品</param>
        /// <param name="userid">用户ID</param>
        /// <param name="languageId">语言ID</param>
        /// <returns>List{ComInfo}.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public ResultModel getGoodsGroupByCom(String getChecked, int languageId, string userid = null)
        {
            var result = new ResultModel();
            var lstElse = new List<GoodsInfoModel>();

            lstElse = this.getCartFromDb(languageId, userid);

            if (String.Equals(getChecked, "1")) lstElse = lstElse.Where(m => m.IsChecked == "1").ToList();

            // 构造以商家分组的数据结构
            var lstAllComId = lstElse.Select(m => m.ComId).Distinct();
            var lstRslt = lstAllComId.Select(comId =>
            {
                return new ComInfo
                {
                    ComId = comId,
                    ComName =
                        lstElse.FirstOrDefault(m => String.Equals(m.ComId, comId, StringComparison.OrdinalIgnoreCase))
                            .ComName,
                    Goods =
                        lstElse.Where(m => String.Equals(m.ComId, comId, StringComparison.OrdinalIgnoreCase)).ToList()
                };
            }).ToList();

            // 商品倒序
            lstRslt.ForEach(c => c.Goods = c.Goods.OrderByDescending(g => g.AddToShoppingCartTime).ToList());
            // 商家倒序
            lstRslt = lstRslt.OrderByDescending(c => c.Goods.Max(g => g.AddToShoppingCartTime)).ToList();

            result.Data = lstRslt;

            return result;
        }

        /// <summary>获取以商家分组的商品信息.</summary>
        /// <remarks>主要用APP 接口</remarks>
        /// <author>戴勇军 2015-08-12 修改</author>
        /// <param name="getChecked">是否只获取已选中的商品</param>
        /// <param name="userid">用户ID</param>
        /// <param name="languageId">语言ID</param>
        /// <returns>List{ComInfo}.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public List<ComInfo> getGoodsGroupByComList(String getChecked, int languageId, string userid = null)
        {
            var result = new ResultModel();
            var lstElse = new List<GoodsInfoModel>();

            lstElse = this.getCartFromDb(languageId, userid);
            if (String.Equals(getChecked, "1")) lstElse = lstElse.Where(m => m.IsChecked == "1").ToList();
            // 构造以商家分组的数据结构
            var lstAllComId = lstElse.Select(m => m.ComId).Distinct();
            var lstRslt = lstAllComId.Select(comId =>
            {
                return new ComInfo
                {
                    ComId = comId,
                    ComName =
                        lstElse.FirstOrDefault(m => String.Equals(m.ComId, comId, StringComparison.OrdinalIgnoreCase))
                            .ComName,
                    Goods =
                        lstElse.Where(m => String.Equals(m.ComId, comId, StringComparison.OrdinalIgnoreCase)).ToList()
                };
            }).ToList();
            // 商品倒序
            lstRslt.ForEach(c => c.Goods = c.Goods.OrderByDescending(g => g.AddToShoppingCartTime).ToList());
            // 商家倒序
            // lstRslt = lstRslt.OrderByDescending(c => c.Goods.Max(g => g.AddToShoppingCartTime)).ToList();

            // 商家升序
            lstRslt = lstRslt.OrderBy(c => c.Goods.Max(g => g.AddToShoppingCartTime)).ToList();
            return lstRslt;
        }

        //public ResultModel GetClearingInfo()
        //{

        //}

        public List<GoodsInfoModel> SetGoodsByCartIdArr(List<CartIdArr> cartIdArr)
        {
            List<GoodsInfoModel> result = new List<GoodsInfoModel>();
            foreach (var item in cartIdArr)
            {
                GoodsInfoModel model = new GoodsInfoModel();
                model.GoodsId = item.ProductId;
                model.SkuNumber = item.SKUId;
                model.Count = Convert.ToInt32(item.BuyNum);
                model.AddToShoppingCartTime = DateTime.Now.ToString();
                model.IsChecked = "1";
                result.Add(model);
            }

            return result;
        }

        public ResultModel updateGoodsCount(List<GoodsInfoModel> lstGoodsInfo, int languageId, string userid = null)
        {
            // 获取数据库购物车中已有商品
            // 查找并更新数量
            // 更新至数据库
            var lstFromDb = this.GetShoppingCartInfo(userid);

            var result = new ResultModel();

            foreach (var mToUpdate in lstGoodsInfo)
            {
                for (var i = 0; i < lstFromDb.Data.Count; i++)
                {
                    var inDb = lstFromDb.Data[i];

                    if (String.Equals(inDb.ProductID.ToString(), mToUpdate.GoodsId, StringComparison.OrdinalIgnoreCase) && String.Equals(inDb.SKU_ProducId.ToString(), mToUpdate.SkuNumber, StringComparison.OrdinalIgnoreCase))
                    {
                        inDb.Quantity = mToUpdate.Count;
                    }
                };

            }
            result.IsValid = this.updateCartInDb(lstFromDb.Data, userid);

            return result;
        }

        /// <summary>更新传入的商品为选中状态,购物车中不在入参中的商品都更新为非选中状态</summary>
        /// <remarks></remarks>
        /// <param name="lstCartsId">The LST carts identifier.</param>
        /// <returns>System.Object.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public ResultModel updateGoodsChecked(List<string> lstCartsId, string userid = null)
        {
            var result = new ResultModel();
            var lstFromDb = this.GetShoppingCartInfo(userid);
            for (var i = 0; i < lstFromDb.Data.Count; i++)
            {
                lstFromDb.Data[i].IsCheck = lstCartsId.Contains(lstFromDb.Data[i].ShoppingCartId.ToString()) ? 1 : 0; //lstGoodsId.Contains(lstFromDb[i].GoodsId) ? "1" : "0";
            }
            result.IsValid = this.updateCartInDb(lstFromDb.Data, userid);
            return result;
        }

        /// <summary>更新传入的商品ID集合为选中状态,购物车中不在入参中的商品都更新为非选中状态(吴育富)</summary>
        /// <remarks></remarks>
        /// <param name="lstCartsId">The LST carts identifier.</param>
        /// <returns>System.Object.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public ResultModel updateGoodsCheckedss(List<string> lstCartsId, string userid = null)
        {
            var result = new ResultModel();
            var lstFromDb = this.GetShoppingCartInfo(userid);
            for (var i = 0; i < lstFromDb.Data.Count; i++)
            {
                lstFromDb.Data[i].IsCheck = lstCartsId.Contains(lstFromDb.Data[i].ProductID.ToString()) ? 1 : 0; //lstGoodsId.Contains(lstFromDb[i].GoodsId) ? "1" : "0";
            }
            result.IsValid = this.updateCartInDb(lstFromDb.Data, userid);
            return result;
        }

        /// <summary>
        /// 根据商品Id,sku码Id获取商品信息
        /// </summary>
        /// <param name="lstGoodsId">商品Id集合</param>
        /// <param name="lstSkuProductIds">sku码Id集合</param>
        /// <param name="languageId">语言代码</param>
        /// <returns></returns>
        public ResultModel GetGoodsInfo(List<String> lstGoodsId, List<String> lstSkuProductIds, int languageId)
        {
            ResultModel result = new ResultModel { IsValid = false };
            if (lstGoodsId == null || lstGoodsId.Count == 0)
            {
                result.Messages.Add("参数异常,请稍候再试");
            }
            else
            {
                var goodsidattr = string.Join(",", lstGoodsId.Select(m => "'" + SqlFilterUtil.ReplaceSqlChar(m) + "'").ToArray());
                var skuattr = string.Join(",", lstSkuProductIds.Select(m => "'" + SqlFilterUtil.ReplaceSqlChar(m) + "'").ToArray());

                StringBuilder sb = new StringBuilder();

                string sql = @"Select p.FareTemplateID,p.Volume,sp.PurchasePrice,sp.Stock AS 'StockQuantity',sp.MarketPrice,sp.SkuName As 'ValueStr',sp.SKU_ProducId As 'SkuNumber',p.RebateDays,p.RebateRatio
                                ,p.PostagePrice,p.SupplierId, p.ProductID AS 'GoodsId',p.FreeShipping,p.Weight,p.MerchantID AS 'ComId',p.Status,p.MerchantID,
                              (SELECT  reverse(stuff(reverse((
                                  SELECT DISTINCT BBB.AttributeName+',' FROM dbo.SKU_AttributeValues AAA  JOIN dbo.SKU_Attributes BBB ON AAA.AttributeId=BBB.AttributeId
                                  WHERE CHARINDEX(convert(varchar,AAA.ValueId)+'_',sp.SKUStr)>0
                                        OR CHARINDEX('_'+convert(varchar,AAA.ValueId),sp.SKUStr)>0
                                        OR sp.SKUStr=convert(varchar,AAA.ValueId) FOR XML PATH(''))),1,1,''))) As 'AttributeName'
                                ,pp.PicUrl As 'Pic',pl.ProductName AS 'GoodsName',
                                    Case When (pr.StarDate<=GETDATE() and pr.EndDate>=GETDATE() and pr.SalesRuleId=2 and pr.Discount>0) Then sp.HKPrice*pr.Discount 
                                         Else sp.HKPrice End As 'GoodsUnits'
                                ,m.ShopName AS 'ComName',m.IsProvideInvoices
                                ,0 As 'Count',0 As 'IsChecked'  From  Product p
                               LEFT JOIN ProductPic pp on p.ProductID = pp.ProductId  LEFT JOIN Product_Lang pl ON p.ProductID = pl.ProductId 
                               LEFT JOIN YH_MerchantInfo m ON p.MerchantID=m.MerchantID LEFT JOIN SKU_Product sp on p.ProductId = sp.ProductId
                               LEFT JOIN ProductRule pr ON p.ProductID = pr.ProductId 
                                    Where pp.Flag = 1 And pl.LanguageID ={0} AND sp.SKU_ProducId IN ({1}) AND  p.ProductId IN ({2});";
                sb.AppendFormat(sql, languageId, skuattr, goodsidattr);
                var data = _database.RunSqlQuery(x => x.ToResultSets(sb.ToString()));
                List<dynamic> sources = data[0];
                result.Data = sources.ToEntity<GoodsInfoModel>();
                foreach (var prodcut in result.Data)
                {
                    prodcut.Pic = GetConfig.FullPath() + prodcut.Pic;
                }
                result.IsValid = true;
            }
            return result;
        }

        ///<summary>
        /// 生成购物车商品删除Sql
        /// </summary>
        /// <param name="view">购物车商品实体</param>
        /// <returns>Sql语句</returns>
        public string GenerateDeleteSql(ShoppingCartModel view)
        {
            string sql = string.Format("DELETE dbo.ShoppingCart WHERE UserID={0} AND SKU_ProducId={1}", view.UserID,
                view.SKU_ProducId);
            return sql;
        }
    }

    public class CartIdArr
    {
        public string ProductId { get; set; }
        public string SKUId { get; set; }
        public string BuyNum { get; set; }
    }
}
