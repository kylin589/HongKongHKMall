using System;
using System.Web.Http;
using BrCms.Framework.Logging;
using HKTHMall.Core.Security;
using HKTHMall.Domain.WebModel.Models.Login;
using HKTHMall.Services.ShoppingCart;
using HKTHMall.Services.Users;
using HKTHMall.WebApi.Models;

using HKTHMall.WebApi.Models.Result;
using HKTHMall.WebApi.Models.Request;
using System.Net.Http;
using HKTHMall.Domain.Models;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Text;
using HKSJ.Common;
using HKTHMall.WebApi.Models.Result.Cart;
using HKTHMall.Core;
using HKTHMall.Domain.WebModel.Models.ShoppingCart;
using HKTHMall.WebApi.Models.Result.Order;
using HKTHMall.Services.Orders;
using HKTHMall.Services.Products;
using HKTHMall.Services.Common.MultiLangKeys;

namespace HKTHMall.WebApi.Controllers
{
    /// <summary>
    /// 6.	购物车相关接口
    /// </summary>
    public class ShoppingCartController : ApiController
    {
        private readonly IEncryptionService _enctyptionService;
        private readonly ILogger _logger;
        private readonly IOrderService _orderService;
        private readonly IShoppingCartService _shoppingCartService;
        private readonly IUserAddressService _userAddressService;
        private readonly IFavoritesService _favoritesService;

        public ShoppingCartController(
            IOrderService orderService
            , IShoppingCartService shoppingCartService
            , IEncryptionService enctyptionService
            , IUserAddressService userAddressService
            , ILogger logger
            , IFavoritesService favoritesService)
        {
            this._orderService = orderService;
            this._shoppingCartService = shoppingCartService;
            this._enctyptionService = enctyptionService;
            this._userAddressService = userAddressService;
            this._logger = logger;
            this._favoritesService = favoritesService;
        }

        #region  6.1.加入购物车（吴育富）
        /// <summary>
        ///  6.1.加入购物车（吴育富）
        /// </summary>
        /// <param name="reqestModel"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AddShoppingCart(RequestShoppingCartModel reqestModel)
        {
            var responseModel = new ResponseShoppingCartModel();
            responseModel.flag = 0;
            responseModel.msg = "失败";
            var remodel = new ResultModel();
            try
            {
                //var us = _enctyptionService.RSAEncrypt("6600000000");
                if (IsNullRequestShoppingCartModel(reqestModel))
                {
                    //商品ID list
                    var lstGoodsIdToAdd = new List<string> { reqestModel.productId.ToString().Trim() };
                    //商品SKU list
                    var lstSkuNumber = new List<string> { reqestModel.product_SKU.ToString().Trim() };

                    //商品数量 list
                    var lstCount = new List<string> { reqestModel.quantity.ToString().Trim() };

                    //用户ID
                    var uId=this._enctyptionService.RSADecrypt(reqestModel.userId);

                    HKTHMall.Domain.AdminModel.Models.Products.ProductModel.AddSKU_ProductModel SKU_ProductModel = this._shoppingCartService.GetSKU_Product(reqestModel.productId, reqestModel.product_SKU).Data;

                    Dictionary<string, int> count1 = new Dictionary<string, int>();//返回购物车选中的总数量,库存量不等0
                    var num = 0;

                    if (SKU_ProductModel!=null&&SKU_ProductModel.Stock>0)//商品库存量不等0允许加入
                    {
                        //加入购物车（这里只做了单条的）
                        remodel = this._shoppingCartService.AddToShoppingCart(lstGoodsIdToAdd, lstSkuNumber, lstCount,
                           uId);
                        responseModel.flag = remodel.IsValid ? 1 : 0;
                        
                    }
                    else
                    {
                        remodel.IsValid = false;//用于返回描述
                        responseModel.flag = 0;
                        
                    }
                    #region 戴勇军 写的方法


                    var carlist = this._shoppingCartService.getGoodsGroupByComList("0", reqestModel.lang, uId);
                    foreach (var v in carlist)
                    {
                        var ps = new CartListResult.ProductShop();
                        ps.merchantID = v.ComId;
                        ps.shopName = v.ComName;

                        var productlist = new List<CartListResult.Productobj>();
                        foreach (var g in v.Goods)
                        {
                            if (g.Status == 4 && g.StockQuantity > 0)
                            {
                                //continue;
                                num++;
                            }

                        }
                    }
                    count1.Add("count", num);
                    #endregion

                    responseModel.rs = count1;

                    switch (reqestModel.lang)
                    {
                        case 1:
                            responseModel.msg = remodel.IsValid ? "购物车添加成功" : "失败";
                            break;
                        case 2:
                            responseModel.msg = remodel.IsValid ? "Shopping cart add success" : "Failure";
                            break;
                        case 3:
                            responseModel.msg = remodel.IsValid ? "เพิ่มในรถเข็นช้อปปิ้งที่ประสบความสําเร็จ" : "ล้มเหลว";
                            break;
                        default:
                            responseModel.msg = remodel.IsValid ? "购物车添加成功" : "失败";
                            break;
                    }

                }
                else
                {
                    switch (reqestModel.lang)
                    {
                        case 1:
                            responseModel.msg = "请输入正确的参数";
                            break;
                        case 2:
                            responseModel.msg = "Please enter the correct parameters";
                            break;
                        case 3:
                            responseModel.msg = "โปรดระบุอาร์กิวเมนต์ที่ถูกต้อง";
                            break;
                        default:
                            responseModel.msg = "请输入正确的参数";
                            break;
                    }

                }
            }
            catch (Exception ex)
            {
                responseModel.msg = ex.ToString();
            }
            //var jsonStr = JsonConvert.SerializeObject(responseModel);
            //return new HttpResponseMessage { Content = new StringContent(jsonStr, Encoding.UTF8, "application/json") };
            return this.Ok(responseModel);
        }
        #endregion

        #region  6.2.购物车查询 (戴勇军)
        /// <summary>
        ///  6.2.购物车查询 (戴勇军)
        /// </summary>
        /// <param name="rePara"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetShoppingCart(RequersCarttModel rePara)
        {
            var er = new ResponseCartListResultModel();
            er.flag = 0;
            er.msg = CultureHelper.GetAPPLangSgring("USER_SEARCH_FAILED", rePara.lang); //查询失败
            try
            {
                if (rePara == null)
                {
                    //logger.Warn(JsonHelper.SerializeObject(requestParam));
                    er.flag = 0;
                    er.msg = CultureHelper.GetAPPLangSgring("PARAMETER_NOT_EMPTY", 3); //参数不能为空
                    return this.Ok(er);
                }
                long uid = 0;
                long.TryParse(this._enctyptionService.RSADecrypt(rePara.userId), out uid);
                if (uid == 0)
                {
                    er.flag = 0;
                    er.msg = CultureHelper.GetAPPLangSgring("USER_ID_IllEGAL", rePara.lang); //用户ID不合法
                    return this.Ok(er);
                }
                er = this.carlistErr(uid.ToString(), rePara.lang);
            }
            catch (Exception ex)
            {
                er.msg = ex.ToString();
            }
            return this.Ok(er);
        }

        /// <summary>
        ///     <param name="uid">用户ID</param>
        ///     <param name="lang">语言:1:中文,2:英文,3:泰文</param>
        ///     <returns> ResponseCartListResultModel Object </returns>
        /// </summary>
        private ResponseCartListResultModel carlistErr(string uid, int lang)
        {
            var carlistResult = new ResponseCartListResultModel();
            try
            {
                var carlist = this._shoppingCartService.getGoodsGroupByComList("0", lang, uid);
                var WebSite = ConfigHelper.GetConfigString("domain");
                var ImagePath = ConfigHelper.GetConfigString("ImagePath");
                carlistResult.flag = 1;
                var j = 0;
                var rs = new List<CartListResult.ProductShop>();

                foreach (var v in carlist)
                {
                    var ps = new CartListResult.ProductShop();

                    ps.merchantID = v.ComId;
                    ps.shopName = v.ComName;
                    j = 0;
                    var productlist = new List<CartListResult.Productobj>();

                    foreach (var g in v.Goods)
                    {
                        if (g.Status == 5 || g.StockQuantity <= 0)
                        {
                            continue;
                        }
                        var product = new CartListResult.Productobj();

                        product.merchantID = ps.merchantID;
                        product.productId = g.GoodsId;
                        product.productName = g.GoodsName;
                        product.sku = g.SkuNumber;
                        product.imageUrl = g.Pic.IndexOf(ImagePath) >= 0 ? g.Pic : WebSite + g.Pic;
                        product.buyNum = g.Count;
                        product.shoppingCartId = g.CartsId.ToString();
                        string strProValueStr = string.IsNullOrEmpty(g.ValueStr) ? "" : g.ValueStr;

                        //if (g.GoodsUnits > 0)
                        {
                            product.tradePrice = g.GoodsUnits.ToString();
                        }
                        // else
                        {
                            product.marketPrice = g.MarketPrice.ToString();
                        }
                        product.stock = g.StockQuantity.ToString();

                        var attributeArr = g.AttributeName.Split('_');
                        var valueArr = strProValueStr.Split(',');
                        var ValueStr = "";
                        var ValueStr1 = new List<string>();


                        //for (var i = 0; i < attributeArr.Length; i++)
                        //{
                        //    ValueStr += attributeArr[i] + ":" + valueArr[i] + " ";
                        //    //ValueStr1.Add(attributeArr[i] + ":" + valueArr[i]);
                        //}

                        product.skuText = strProValueStr.Replace(","," ");// ValueStr.Trim();
                        //string.Join(" ", ValueStr1.OrderBy(o => o).ToArray());// ValueStr.Trim();

                        //
                        var result = this._favoritesService.IsExistFavorites(Convert.ToInt64(uid),
                            Convert.ToInt64(product.productId));
                        if (result.IsValid)
                        {
                            product.isFavorites = "1";
                        }
                        else
                        {
                            product.isFavorites = "0";
                        }

                        #region  加入爆款商品SKU库存判断 (暂不是需要)

                        /*
                                  //加入爆款商品SKU库存判断
                                //爆款产品表,根据产品ID查找
                                var explosionProductxFill = _IProductRuleService.GetPromotionProductForId(Convert.ToInt64(product.productId), lang);
                                var explosionProduct = explosionProductxFill.Data;
                                //SP_ExplosionProduct.GetExplosionByProductID(Convert.ToInt32(product.productId));
                                if (explosionProduct != null) //爆款中
                                {
                                    //if (explosionProduct.Status > 1) //爆款中
                                    {
                                        j++;
                                    }
                                    //爆款产品SKU表,根据产品ID查找
                                    //var spBao = SP_ExplosionSKU.GetExplosionBySKUID(explosionProduct.ExplosionProductID, product.sku);
                                    //if (spBao != null && spBao.LimitStock > 0)
                                    //{
                                    //    productlist.Add(product);
                                    //}
                                    //else
                                    {
                                        productlist.Add(product);
                                    }
                                }
                                else
                                {
                                    productlist.Add(product);
                                } 
                                 */

                        #endregion

                        productlist.Add(product);
                    }

                    if (productlist.Count != 0)
                    {
                        ps.Products = productlist;
                        rs.Add(ps);
                    }
                    if (j > 0)
                    {
                        ps.IsHot = "1";
                    }
                    else
                    {
                        ps.IsHot = "0";
                    }
                }
                carlistResult.msg = CultureHelper.GetAPPLangSgring("USERINFO_OP_SUCCESS", lang);
                carlistResult.rs = rs;

                return carlistResult;
            }
            catch (Exception ex)
            {
                //logger.Error(ex);
                carlistResult.flag = 0;
                carlistResult.msg = CultureHelper.GetAPPLangSgring("SYSTEM_ERROR", lang);
                return carlistResult;
            }
        }

        #endregion

        #region 6.3.购物车商品数量修改  (戴勇军)
        /// <summary>
        /// 6.3.购物车商品数量修改  (戴勇军)
        /// </summary>
        /// <param name="rePara"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult UpdateShoppingCart(RequersCarttModel rePara)
        {
            var er = new ResponseShoppingCartModel();
            er.flag = 0;
            er.msg = CultureHelper.GetAPPLangSgring("UPDATE_Failure", rePara.lang);// 更新失败 
            try
            {
                if (rePara == null)
                {
                    //logger.Warn(JsonHelper.SerializeObject(requestParam));
                    er.flag = 0;
                    er.msg = CultureHelper.GetAPPLangSgring("PARAMETER_NOT_EMPTY", 3); //参数不能为空
                    return this.Ok(er);
                }

                long uid = 0;
                long.TryParse(rePara.userId, out uid);
                if (uid == 0)
                {
                    er.flag = 0;
                    er.msg = CultureHelper.GetAPPLangSgring("USER_ID_IllEGAL", rePara.lang); //用户ID不合法
                    return this.Ok(er);
                }
                long ShCartId = 0;
                long.TryParse(this._enctyptionService.RSADecrypt(rePara.shoppingCartId), out ShCartId);
                if (ShCartId == 0)
                {
                    er.flag = 0;
                    er.msg = CultureHelper.GetAPPLangSgring("SHOPPINGCART_ID_IllEGAL", rePara.lang);//购物车ID不合法
                    return this.Ok(er);
                }
                long prId = 0;
                long.TryParse(this._enctyptionService.RSADecrypt(rePara.ProductId), out prId);
                if (prId == 0)
                {
                    er.flag = 0;
                    er.msg = CultureHelper.GetAPPLangSgring("GOODS_ID_IllEGAL", rePara.lang);//商品ID不合法
                    return this.Ok(er);
                }
                long quantity = 0;
                long.TryParse(rePara.buyNum, out quantity);
                if (quantity <= 0)
                {
                    er.flag = 0;
                    er.msg = CultureHelper.GetAPPLangSgring("PURCHASE_QUANTITY_IllEGAL", rePara.lang);//购买数量不合法
                    return this.Ok(er);
                }
                var remodel = this._shoppingCartService.UpdateShoppingCartByCountData(uid.ToString(), ShCartId,
                    (int)prId,
                    (int)quantity);
                er.flag = remodel.IsValid ? 1 : 0;
                er.msg = remodel.IsValid ? CultureHelper.GetAPPLangSgring("UPDATE_SUCCESS", rePara.lang) : CultureHelper.GetAPPLangSgring("UPDATE_Failure", rePara.lang);
            }
            catch (Exception ex)
            {
                er.msg = ex.ToString();
            }
            //var jsonStr = JsonConvert.SerializeObject(er);
            //return new HttpResponseMessage { Content = new StringContent(jsonStr, Encoding.UTF8, "application/json") };
            return this.Ok(er);
        }
        #endregion

        #region 6.4.购物车商品移除  (戴勇军)
        /// <summary>
        /// 6.4.购物车商品移除  (戴勇军)
        /// </summary>
        /// <param name="rePara"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult DeleteShoppingCart(RequersCarttModel rePara)
        {
            var er = new ResponseShoppingCartModel();
            er.flag = 0;
            er.msg = CultureHelper.GetAPPLangSgring("DELETE_Failure", rePara.lang); //删除失败
            try
            {
                if (rePara == null)
                {
                    //logger.Warn(JsonHelper.SerializeObject(requestParam));
                    er.flag = 0;
                    er.msg = CultureHelper.GetAPPLangSgring("PARAMETER_NOT_EMPTY", 3); //参数不能为空
                    return this.Ok(er);
                }
                long uid = 0;
                long.TryParse(this._enctyptionService.RSADecrypt(rePara.userId), out uid);
                if (uid == 0)
                {
                    er.flag = 0;
                    er.msg = CultureHelper.GetAPPLangSgring("USER_ID_IllEGAL", rePara.lang); //用户ID不合法
                    return this.Ok(er);
                }
                long ShCartId = 0;
                long.TryParse(this._enctyptionService.RSADecrypt(rePara.shoppingCartId), out ShCartId);
                if (ShCartId == 0)
                {
                    er.flag = 0;
                    er.msg = CultureHelper.GetAPPLangSgring("SHOPPINGCART_ID_IllEGAL", rePara.lang);//购物车ID不合法
                    return this.Ok(er);
                }
                var prList = new List<string>();
                prList.Add(ShCartId.ToString());
                var remodel = this._shoppingCartService.deleteGoodsInShoppingCart(prList, uid.ToString());
                er.flag = remodel.IsValid ? 1 : 0;
                er.msg = remodel.IsValid ? CultureHelper.GetAPPLangSgring("DELETE_SUCCESS", rePara.lang) : CultureHelper.GetAPPLangSgring("DELETE_Failure", rePara.lang);
            }
            catch (Exception ex)
            {
                er.msg = ex.ToString();
            }
            //var jsonStr = JsonConvert.SerializeObject(er);
            //return new HttpResponseMessage { Content = new StringContent(jsonStr, Encoding.UTF8, "application/json") };
            return this.Ok(er);
        }
        #endregion

        #region 6.5.获取购物车总条数 (戴勇军)
        /// <summary>
        /// 6.5.获取购物车总条数 (戴勇军)
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetCartNum(RequersCarttModel para)
        {
            var er = new GetCartNumResult.GetCartNum();
            var result = new GetCartNumResult.ResultM();
            try
            {
                if (para == null)
                {
                    er.Flag = 0;
                    er.Msg = CultureHelper.GetAPPLangSgring("PARAMETER_NOT_EMPTY", 3); //参数不能为空
                    return this.Ok(er);
                }
                long userId;
                if (!long.TryParse(this._enctyptionService.RSADecrypt(para.userId), out userId))
                {
                    er.Flag = 0;
                    er.Msg = CultureHelper.GetAPPLangSgring("USER_ID_IllEGAL", para.lang); //用户ID不合法
                    return this.Ok(er);
                }

                var num = 0;

                var carlist = this._shoppingCartService.getGoodsGroupByComList("0", para.lang, userId.ToString());
                foreach (var v in carlist)
                {
                    var ps = new CartListResult.ProductShop();
                    ps.merchantID = v.ComId;
                    ps.shopName = v.ComName;

                    var productlist = new List<CartListResult.Productobj>();
                    foreach (var g in v.Goods)
                    {
                        if (g.Status == 5 || g.StockQuantity <= 0)
                        {
                            continue;
                        }
                        num++;
                    }
                }
                result.Number = num; //购物车总条数 
                er.Rs = result;
                er.Flag = 1;
                er.Msg = CultureHelper.GetAPPLangSgring("USERINFO_OP_SUCCESS", para.lang); //操作成功
                return this.Ok(er);
            }
            catch (Exception ex)
            {
                er.Flag = 0;
                er.Msg = CultureHelper.GetAPPLangSgring("SYSTEM_ERROR", para.lang) + ex.Message; //系统异常,请稍后再试
                return this.Ok(er);
            }
        }
        #endregion

        #region 6.6.编辑购物车  (戴勇军)
        /// <summary>
        /// 6.6.编辑购物车  (戴勇军)
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult EditCart(RequersCarttModel para)
        {
            try
            {
                if (para == null)
                {
                    return this.Ok(new ApiResultModel { flag = 0, msg = CultureHelper.GetAPPLangSgring("PARAMETER_NOT_EMPTY", 3) });
                }
                var userid = this._enctyptionService.RSADecrypt(para.userId);
                var productid = string.IsNullOrEmpty(para.Product) ? para.productId : para.Product;
                var v_shoppingCartId = string.IsNullOrEmpty(para.shoppingCartId) ? null : para.shoppingCartId;
                if (v_shoppingCartId == null)
                {
                    return this.Ok(new ApiResultModel { flag = 1, msg = CultureHelper.GetAPPLangSgring("SHOPPINGCART_ID_IllEGAL", para.lang) });//购物车ID不合法
                }
                var sku = para.sku;
                var buyNum = para.buyNum;
                var action = para.Action;
                //YH_ShoppingCartBll cartBll = new YH_ShoppingCartBll();
                var v_lstGoodsId = StringUtility.DeserializeObject<List<string>>(this.ChangeString(v_shoppingCartId));
                var lstGoodsId = StringUtility.DeserializeObject<List<string>>(this.ChangeString(productid));
                var lstSkuNumber = StringUtility.DeserializeObject<List<string>>(this.ChangeString(sku));
                var blb = new ResultModel();
                if (para.Action == 2)
                {
                    // bl = cartBll.deleteGoodsInShoppingCart(lstGoodsId, lstSkuNumber, userid);
                    blb = this._shoppingCartService.deleteGoodsInShoppingCart(v_lstGoodsId, userid);
                }
                else if (para.Action == 1)
                {
                    var list = new List<GoodsInfoModel>();

                    var skuArr = sku.Split(',');
                    var productArr = productid.Split(',');
                    var v_productArr = v_shoppingCartId.Split(',');
                    var CountArr = buyNum.Split(',');
                    var i = 0;
                    foreach (var item in skuArr)
                    {
                        var goodsInfo = new GoodsInfoModel();
                        goodsInfo.SkuNumber = item;
                        goodsInfo.GoodsId = productArr[i];
                        goodsInfo.Count = Convert.ToInt32(CountArr[i]);
                        list.Add(goodsInfo);
                        i++;
                    }
                    //  bl = new YH_ShoppingCartBll().updateGoodsCount(list, userid);
                    blb = this._shoppingCartService.updateGoodsCount(list, para.lang, userid);
                }
                else
                {
                    return this.Ok(new ApiResultModel { flag = 0, msg = CultureHelper.GetAPPLangSgring("PARAMETER_ERROR", para.lang) }); //PARAMETER_ERROR
                }
                if (blb.IsValid)
                {
                    return this.Ok(new ApiResultModel { flag = 1, msg = CultureHelper.GetAPPLangSgring("USERINFO_OP_SUCCESS", para.lang) });//操作成功
                }
                return this.Ok(new ApiResultModel { flag = 0, msg = CultureHelper.GetAPPLangSgring("SYSTEM_ERROR", para.lang) });//系统异常,请稍后再试
            }
            catch (Exception ex)
            {
                var carlistResult = new ApiResultModel();
                //logger.Error(ex);
                carlistResult.flag = 0;
                carlistResult.msg = CultureHelper.GetAPPLangSgring("SYSTEM_ERROR", para.lang);//系统异常,请稍后再试
                return this.Ok(carlistResult);
            }
        }
        #endregion

        #region 6.7.根据传入的购物车ID集合和用户ID修改商品选中,购物车中不在入参中的商品都更新为非选中状态  (吴育富---新增)
        /// <summary>
        /// 6.7.根据传入的购物车ID集合和用户ID修改商品选中,购物车中不在入参中的商品都更新为非选中状态  (吴育富---新增)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult UpdateGoodsChecked(RequestUpdateGoodsCheckedModel model)
        {
            try
            {
                var result = this._shoppingCartService.updateGoodsChecked(
                model.GoodsIDs,
                this._enctyptionService.RSADecrypt(model.UserId)
                );

                return this.Ok(result);
            }
            catch (Exception)
            {
                return this.Ok(new ApiResultModel()
                {
                    flag = 0,
                    msg = CultureHelper.GetAPPLangSgring("SYSTEM_ERROR", model.Lang) //系统异常,请稍后再试
                });
            }
        }
        #endregion

        #region 私有方法
        /// <summary>
        /// 验证模型参数（吴育富）
        /// </summary>
        /// <param name="reqestModel"></param>
        /// <returns></returns>
        private bool IsNullRequestShoppingCartModel(RequestShoppingCartModel reqestModel)
        {
            var bl = true;
            if (reqestModel == null)
            {
                return bl = false;
            }
            if (reqestModel.userId == null)
            {
                bl = false;
            }
            else if (reqestModel.product_SKU.ToString().Trim() == "" || reqestModel.productId.ToString().Trim() == "" || reqestModel.quantity.ToString().Trim() == "" || reqestModel.userId.ToString().Trim() == "")
            {
                bl = false;
            }

            return bl;
        }

        
        private string ChangeString(string str)
        {
            var allSplit = str.Split(',');

            var strConnect = "[";
            foreach (var v in allSplit)
            {
                strConnect += "'" + v + "',";
            }
            strConnect = strConnect.Remove(strConnect.Length - 1);
            strConnect += "]";
            return strConnect;
        }

        #endregion

    }
}