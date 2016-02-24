using BrCms.Framework.Collections;
using HKTHMall.Domain.AdminModel.Models.Orders;
using HKTHMall.Domain.Models;
using Simple.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Services.Orders.Impl
{
    public class PurchaseOrderDetailsService : BaseService, IPurchaseOrderDetailsService
    {
        /// <summary>
        /// 供应商采购单明细分布查询
        /// </summary>
        /// <param name="model">输入查询参数对象</param>
        /// <param name="languageID">语言Id</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-9-8</remarks>
        public ResultModel Select(SearchPurchaseOrderDetailsModel model,int languageID)
        {
            var purchaseOrderDetails = _database.Db.PurchaseOrderDetails;
            var orderDetailslang = _database.Db.OrderDetails_lang;
            var purchaseOrder =_database.Db.PurchaseOrder;

            #region 查询参数条件
            //查询参数条件
            var whereParam = new SimpleExpression(1, 1, SimpleExpressionType.Equal);
            //商品名称
            if (!string.IsNullOrEmpty(model.ProductName))
            {
                whereParam = new SimpleExpression(whereParam, orderDetailslang.ProductName.Like("%" + model.ProductName + "%"), SimpleExpressionType.And);
            }

            #endregion

            var query = purchaseOrderDetails.All().
                 Join(orderDetailslang).On(orderDetailslang.OrderDetailsID == purchaseOrderDetails.OrderDetailsID).
                 Select(
                    purchaseOrderDetails.OrderDetailsID,
                    purchaseOrderDetails.PurchaseOrderId,
                    purchaseOrderDetails.ProductId,
                    purchaseOrderDetails.CostPrice,
                    purchaseOrderDetails.SalesPrice,
                    purchaseOrderDetails.Quantity,
                    purchaseOrderDetails.returnedQty,
                    purchaseOrderDetails.RealQty,
                    (purchaseOrderDetails.CostPrice * purchaseOrderDetails.Quantity).As("Subtotal"),
                    purchaseOrderDetails.SkuName,
                    orderDetailslang.ProductName
                 ).Where(whereParam).
                 Where(orderDetailslang.LanguageID == languageID && purchaseOrderDetails.PurchaseOrderId == model.PurchaseOrderId).OrderByOrderDetailsID();


            var result = new ResultModel()
            {
                Data = new SimpleDataPagedList<PurchaseOrderDetailsModel>(query,
                            model.PagedIndex, model.PagedSize)
            };
            return result;
        }
    }
}
