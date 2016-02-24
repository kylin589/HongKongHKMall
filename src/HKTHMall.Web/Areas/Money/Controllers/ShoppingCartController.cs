using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HKTHMall.Core;
using HKTHMall.Domain.Entities;
using HKTHMall.Domain.WebModel.Models.ShoppingCart;
using HKTHMall.Services.ShoppingCart;
using HKTHMall.Web.Controllers;
using HKTHMall.Domain.WebModel.Models.Login;
using HKTHMall.Services.Common.MultiLangKeys;


namespace HKTHMall.Web.Areas.Money.Controllers
{
    /// <summary>购物车</summary>
    /// <remarks></remarks>
    /// <author>ywd 2015-7-9 14:48:10</author>
    public class ShoppingCartController : BaseController
    {
        /// <summary>
        /// 购物车服务类
        /// </summary>
        private readonly IShoppingCartService _ShoppingCartService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="shoppingCartService"></param>
        public ShoppingCartController(IShoppingCartService shoppingCartService)
        {
            _ShoppingCartService = shoppingCartService;
        }

        // GET: Money/ShoppingCart
        /// <summary>我的购物车界面</summary>
        /// <remarks></remarks>
        /// <author>ywd 2015-7-9 14:49:01</author>
        /// <returns>ActionResult</returns>
        public ActionResult MyShoppingCart()
        {
            return View();
        }

        /// <summary>根据用户id获取商品信息</summary>
        /// <remarks></remarks>
        /// <author>ywd 2015-7-9 14:49:53</author>
        /// <param name="userId">用户ID</param>
        /// <returns>ActionResult.</returns>
        [Authorize]
        public JsonResult getGoodsInfoByUserId(String userId)
        {
            var prodcutList = _ShoppingCartService.GetShoppingCartByUserId(base.UserID.ToString(), CultureHelper.GetLanguageID());
            var data = new
            {
                rows = prodcutList.Data,
                total = prodcutList.Data.Count
            };
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        /// <summary>添加商品到购物车.</summary>
        /// <remarks></remarks>
        /// <author>Yun 2015-04-19 09:45:41</author>
        /// <param name="strGoodsIds">The string goods ids.</param>
        /// <returns>ActionResult.</returns>
        [Authorize]
        public ActionResult addToShoppingCart(String strGoodsIds, String strSkuNumber, String strCount)
        {
            var lstGoodsId = StringUtility.DeserializeObject<List<String>>(strGoodsIds);
            var lstSkuNumber = StringUtility.DeserializeObject<List<String>>(strSkuNumber.Replace("'", ""));
            var lstCount = StringUtility.DeserializeObject<List<String>>(strCount);

            var msg = "";
            var tag = false;

            if (strSkuNumber == null || strCount == null)
            {
                tag = true;
                msg = CultureHelper.GetLangString("SHOPPINGCART_PLS_CHOOSE_PRODUCTS_ATTRIBUTE");//请选择商品属性

                return new JsonResult { Data = new { Data = false, Msg = msg } };
            }

            var index = 0;
            List<GoodsInfoModel> list = new List<GoodsInfoModel>();
            lstGoodsId.ForEach(l =>
            {
                list.Add(new GoodsInfoModel()
                {
                    GoodsId = l,
                    SkuNumber = lstSkuNumber[index],
                    Count = Convert.ToInt32(lstCount[index])
                });
                index++;
            });

            //删除里面有为空的
            list.ForEach(l =>
            {
                if (l.GoodsId == "null")//确保当前产品的所有SkuNumber
                {
                    var lstIndex = lstSkuNumber.FindIndex(p => p == l.SkuNumber);//一删一
                    lstSkuNumber.Remove(l.SkuNumber);
                    lstCount.RemoveAt(lstIndex);
                    lstGoodsId.RemoveAt(lstIndex);
                }
            });
            return Json(new { Data = _ShoppingCartService.AddToShoppingCart(lstGoodsId, lstSkuNumber, lstCount, base.UserID.ToString()).IsValid, Msg = msg }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>删除购物车中的指定商品.</summary>
        /// <remarks></remarks>
        /// <author>Yun 2015-04-19 09:45:54</author>
        /// <param name="strGoodsIds">The string goods ids.</param>
        /// <returns>ActionResult.</returns>
        [Authorize]
        public ActionResult deleteGoodsInShoppingCart(String strCartsIds)
        {
            var lstCartsId = StringUtility.DeserializeObject<List<String>>(strCartsIds);

            return Json(_ShoppingCartService.deleteGoodsInShoppingCart(lstCartsId).IsValid, JsonRequestBehavior.AllowGet);
        }

        /// <summary>获取以商家分组的商品信息.</summary>
        /// <remarks></remarks>
        /// <author>Yun 2015-04-19 09:46:04</author>
        /// <param name="getChecked">The string goods ids.</param>
        /// <returns>ActionResult.</returns>
        [Authorize]
        public ActionResult getGoodsGroupByCom(String getChecked)
        {
            return Json(_ShoppingCartService.getGoodsGroupByCom(getChecked, CultureHelper.GetLanguageID(), base.UserID.ToString()).Data, JsonRequestBehavior.AllowGet);
        }

        /// <summary>更新指定商品的数量.</summary>
        /// <remarks></remarks>
        /// <author>Yun 2015-04-19 09:46:20</author>
        /// <param name="strGoodsIds">The string goods ids.</param>
        /// <returns>ActionResult.</returns>
        [Authorize]
        public ActionResult updateGoodsCount(String arrGoodsCountString)
        {
            var lstGoodsInfo = StringUtility.DeserializeObject<List<GoodsInfoModel>>(arrGoodsCountString);

            return Json(_ShoppingCartService.updateGoodsCount(lstGoodsInfo, CultureHelper.GetLanguageID(), base.UserID.ToString()).IsValid, JsonRequestBehavior.AllowGet);
        }

        /// <summary>更新传入的商品为选中状态,购物车中不在入参中的商品都更新为非选中状态</summary>
        /// <remarks></remarks>
        /// <param name="strGoodsIds">The string goods ids.</param>
        /// <returns>ActionResult.</returns>
        [Authorize]
        public ActionResult updateGoodsChecked(String strCartsIds)
        {
            var lstCartsId = StringUtility.DeserializeObject<List<String>>(strCartsIds);
            //var lstSkuNumber = StringUtility.DeserializeObject<List<String>>(strSkuNumber);

            return Json(_ShoppingCartService.updateGoodsChecked(lstCartsId, base.UserID.ToString()).IsValid, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 根据商品id获取商品信息
        /// </summary>
        /// <param name="strGoodsIds"></param>
        /// <param name="strSkuNumber"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult getGoodsInfoByIds(String strGoodsIds, String strSkuNumber)
        {
            var lstGoodsId = StringUtility.DeserializeObject<List<String>>(strGoodsIds);
            var lstSku = StringUtility.DeserializeObject<List<String>>(strSkuNumber);


            List<GoodsInfoModel> prodcutList = _ShoppingCartService.GetGoodsInfo(lstGoodsId, lstSku, CultureHelper.GetLanguageID()).Data;
            var tag = false;
            try
            {
                if (base.UserType == 1)
                {
                    var product = prodcutList.FindAll(o => o.MerchantID == this.UserID);
                    foreach (var p in product)
                    {
                        if (p.MerchantID == this.UserID)
                            tag = true;
                        prodcutList.Remove(p);
                    }
                }
            }
            catch
            {
            }
            //商家不能购买自己的商品
            return Json(new { Data = prodcutList, Msg = (tag ? CultureHelper.GetLangString("SHOPPINGCART_PRODUCTOR_CAN_NOT_BUY_SELF_PRODUCT") : "") }, JsonRequestBehavior.AllowGet);
        }

    }
}