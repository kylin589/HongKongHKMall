using System.Collections.Generic;
using BrCms.Core.Infrastructure;
using HKTHMall.Domain.Models;
using HKTHMall.Domain.WebModel.Models.ShoppingCart;

namespace HKTHMall.Services.ShoppingCart
{
    public interface IShoppingCartService : IDependency
    {
        /// <summary> 加入购物车 </summary>
        /// <param name="model">购物车模型</param>
        ResultModel AddShoppingCart(ShoppingCartModel model);

        /// <summary>
        /// 查询购物车商品数量（吴育富）
        /// </summary>
        /// <param name="UserID">用户ID</param>
        /// <param name="ProductID">商品ID</param>
        /// <param name="SKU_ProducId">商品SKU</param>
        /// <returns></returns>
        ResultModel GetShoppingCartUserID(long UserID, long ProductID, long SKU_ProducId);

        /// <summary>
        /// 根据商品ID和商品SKU_ProductID查询产品数量（wuyf）
        /// </summary>
        /// <param name="ProductId"></param>
        /// <param name="SKU_ProductID"></param>
        /// <returns></returns>
        ResultModel GetSKU_Product(long ProductId, long SKU_ProductID);

        /// <summary>
        /// 查询购物车总数量（吴育富）
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        ResultModel GetShoppingCartUserIDSum(long UserID);

        /// <summary>添加商品到购物车.</summary>
        /// <remarks></remarks>
        /// <param name="lstGoodsIdToAdd">商品ID.</param>
        /// <param name="lstSkuNumber">商品SKUID.</param>
        /// <param name="lstCount">数量.</param>
        /// <param name="userid">用户ID.</param>
        /// <returns>System.Object.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        ResultModel AddToShoppingCart(List<string> lstGoodsIdToAdd, List<string> lstSkuNumber, List<string> lstCount,
            string userid = null);

        /// <summary>删除购物车中的指定商品.</summary>
        /// <remarks></remarks>
        /// <author>Yun 2015-04-19 09:51:04</author>
        /// <param name="lstGoodsId">The LST goods identifier.</param>
        /// <returns>System.Object.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        ResultModel deleteGoodsInShoppingCart(List<string> lstCartsId, string userid = null);

        /// <summary>获取以商家分组的商品信息.</summary>
        /// <remarks></remarks>
        /// <author>PanYun HX1501345 2015-04-21 09:02:19</author>
        /// <param name="getChecked">是否只获取已选中的商品</param>
        /// <param name="userid">用户ID</param>
        /// <returns>List{ComInfo}.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        ResultModel getGoodsGroupByCom(string getChecked, int languageId, string userid = null);

        ResultModel updateGoodsCount(List<GoodsInfoModel> lstGoodsInfo, int languageId, string userid = null);

        /// <summary>根据用户id获取购物车商品</summary>
        /// <param name="userid">用户id</param>
        /// <param name="languageId">语言id</param>
        /// <returns>购物车商品模型</returns>
        ResultModel GetShoppingCartByUserId(string userid, int languageId);

        /// <summary>更新传入的商品为选中状态,购物车中不在入参中的商品都更新为非选中状态</summary>
        /// <remarks></remarks>
        /// <param name="lstGoodsId">The LST goods identifier.</param>
        /// <returns>System.Object.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        ResultModel updateGoodsChecked(List<string> lstCartsId, string userid = null);

        /// <summary>更新传入的商品ID集合为选中状态,购物车中不在入参中的商品都更新为非选中状态(吴育富)</summary>
        /// <remarks></remarks>
        /// <param name="lstCartsId">The LST carts identifier.</param>
        /// <returns>System.Object.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        ResultModel updateGoodsCheckedss(List<string> lstCartsId, string userid = null);

        /// <summary> 生成购物车商品删除Sql</summary>
        /// <param name="view">购物车商品实体</param>
        /// <returns>Sql语句</returns>
        string GenerateDeleteSql(ShoppingCartModel view);

        /// <summary>根据商品Id,sku码获取商品信息</summary>
        /// <param name="lstGoodsId">商品Id集合</param>
        /// <param name="lstSku">sku码集合</param>
        /// <param name="languageId">语言代码</param>
        /// <returns></returns>
        ResultModel GetGoodsInfo(List<string> lstGoodsId, List<string> lstSku, int languageId);

        /// <summary>   
        /// 购物车商品单个商品数量修改 （2015.08.08 戴勇军修改）
        /// </summary> 
        /// <param name="userId	">用户ID</param>
        ///  <param name="ShoppingCartId">购物车ID</param>
        /// <param name="productId">商品编号ID</param>
        /// <param name="quantity">修改后的数量</param>
        /// <returns></returns>
        ResultModel UpdateShoppingCartByCountData(string userid, long ShoppingCartId, int productId, int quantity);


        /// <summary>获取以商家分组的商品信息列表</summary>
        /// <remarks>主要用于APP 接口</remarks>
        /// <author>戴勇军 2018-08-12 修改</author>
        /// <param name="getChecked">是否只获取已选中的商品</param>
        /// <param name="userid">用户ID</param>
        /// <returns>List{ComInfo}.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        List<ComInfo> getGoodsGroupByComList(string getChecked, int languageId, string userid = null);
    }
}