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
    public class PurchaseOrderSerivce : BaseService, IPurchaseOrderSerivce
    {
        /// <summary>
        /// 供应商采购单分布查询
        /// </summary>
        /// <param name="model">输入查询参数对象</param>
        /// <param name="languageID">语言Id</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-9-8</remarks>
        public ResultModel Select(SearchPurchaseOrderModel model)
        {
            var purchaseOrder = _database.Db.PurchaseOrder;
            var suppliers = _database.Db.Suppliers;

            #region 查询参数条件
            //查询参数条件
            var whereParam = new SimpleExpression(1, 1, SimpleExpressionType.Equal);

            //PurchaseOrderId
            if (!string.IsNullOrEmpty(model.PurchaseOrderId))
            {
                whereParam = new SimpleExpression(whereParam, purchaseOrder.PurchaseOrderId.Like("%" + model.PurchaseOrderId + "%"), SimpleExpressionType.And);
            }
            //订单号
            if (!string.IsNullOrEmpty(model.OrderID))
            {
                whereParam = new SimpleExpression(whereParam, purchaseOrder.OrderID.Like("%" + model.OrderID + "%"), SimpleExpressionType.And);
            }
            //供应商名称
            if (!string.IsNullOrEmpty(model.SupplierName))
            {
                whereParam = new SimpleExpression(whereParam, suppliers.SupplierName.Like("%" + model.SupplierName + "%"), SimpleExpressionType.And);
            }
            //手机号码
            if (!string.IsNullOrEmpty(model.Mobile))
            {
                whereParam = new SimpleExpression(whereParam, suppliers.Mobile.Like("%" + model.Mobile + "%"), SimpleExpressionType.And);
            }
            //状态
            if (model.status != null)
            {
                whereParam = new SimpleExpression(whereParam, purchaseOrder.status == model.status.Value, SimpleExpressionType.And);
            }
            #endregion

            var query = purchaseOrder.All().
                 Join(suppliers).On(suppliers.SupplierId == purchaseOrder.SupplierId).
                 Select(
                    purchaseOrder.PurchaseOrderId,
                    purchaseOrder.OrderID,
                    purchaseOrder.SupplierId,
                    purchaseOrder.PurchaseAmount,
                    purchaseOrder.RealPurchaseAmount,
                    purchaseOrder.status,
                    purchaseOrder.CreateTime,
                    purchaseOrder.CreateBy,
                    purchaseOrder.Deliveryer,
                    purchaseOrder.DeliveryDate,
                    suppliers.SupplierName,
                    suppliers.Mobile
                 ).Where(whereParam).OrderByCreateTimeDescending();


            var result = new ResultModel()
            {
                Data = new SimpleDataPagedList<PurchaseOrderModel>(query,
                            model.PagedIndex, model.PagedSize)
            };
            return result;
        }

        /// <summary>
        /// 更新收货状态
        /// </summary>
        /// <param name="model">对象</param>
        /// <returns>返回true时,表示更新成功；反之,表示更新失败</returns>
        /// <remarks>added by jimmy,2015-9-8</remarks>
        public ResultModel Update(PurchaseOrderModel model)
        {
            var result = new ResultModel
            {
                Data = _database.Db.PurchaseOrder.UpdateByPurchaseOrderId(PurchaseOrderId: model.PurchaseOrderId,
                    DeliveryDate: model.DeliveryDate, status: model.status, Deliveryer: model.Deliveryer)
            };
            return result;
        }

        /// <summary>
        /// 取消订单时更改其状态和取消时间
        /// zhoub 20150910
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ResultModel UpdateByOrderId(PurchaseOrderModel model)
        {
            var result = new ResultModel
            {
                Data = _database.Db.PurchaseOrder.UpdateByOrderID(OrderID: model.OrderID,
                    CancelDate: model.CancelDate, status: model.status, CancelUser: model.CancelUser)
            };
            return result;
        }

        /// <summary>
        /// 供应商采购单
        /// </summary>
        /// <param name="PurchaseOrderId">供应商采购单ID</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-9-8</remarks>
        public ResultModel GetPurchaseOrder(string PurchaseOrderId)
        {
            var result = new ResultModel { Data = _database.Db.PurchaseOrder.FindByPurchaseOrderId(PurchaseOrderId) };
            return result;
        }


        #region  供应商后台管理系统传用

         /// <summary>
        /// 采购订单查询
        /// zhoub 20150928
        /// </summary>
        /// <param name="model"></param>
        /// <param name="totalCount">订单条数</param>
        /// <param name="productQuantity">产品个数</param>
        /// <returns></returns>
        public ResultModel GetSuppliersPagingPurchaseOrder(SearchPurchaseOrderModel model, out int totalCount, out int productQuantity)
        {
            List<PurchaseOrderModel> list = new List<PurchaseOrderModel>();
            PurchaseOrderModel pom = null;
            var po = _database.Db.PurchaseOrder;
            var pod = _database.Db.PurchaseOrderDetails;
            var pro = _database.Db.Product;
            var prol = _database.Db.Product_Lang;
            var pic = _database.Db.ProductPic;
            var whereExpr = po.SupplierId == model.SupplierId;
            totalCount = 0;
            productQuantity = 0;

            if (!string.IsNullOrEmpty(model.PurchaseOrderId))
            {
                whereExpr = new SimpleExpression(whereExpr, pod.PurchaseOrderId == model.PurchaseOrderId.Trim(), SimpleExpressionType.And);
            }

            if (model.ProductId > 0)
            {
                whereExpr = new SimpleExpression(whereExpr, pod.ProductId == model.ProductId, SimpleExpressionType.And);
            }

            if (model.status > 0)
            {
                whereExpr = new SimpleExpression(whereExpr, po.status == model.status, SimpleExpressionType.And);
            }

            if (model.StartTime != null)
            {
                whereExpr = new SimpleExpression(whereExpr, po.CreateTime >= model.StartTime, SimpleExpressionType.And);
            }
            if (model.EndTime != null)
            {
                whereExpr = new SimpleExpression(whereExpr, po.CreateTime < Convert.ToDateTime(model.EndTime).AddDays(1), SimpleExpressionType.And);
            }

            var result = new ResultModel
            {
                Data = new SimpleDataPagedList<PurchaseOrderModel>(
                        po.All()
                        .LeftJoin(pod).On(pod.PurchaseOrderId == po.PurchaseOrderId)
                        .Select(po.PurchaseOrderId.Distinct(), po.CreateTime, po.status)
                        .Where(whereExpr).OrderBy(po.CreateTime), model.PagedIndex, model.PagedSize)
            };

            if (result.Data != null)
            {
                totalCount = result.Data.TotalCount;

                productQuantity = po.All()
                .LeftJoin(pod).On(pod.PurchaseOrderId == po.PurchaseOrderId)
                .Select(pod.Quantity.Sum())
                .Where(whereExpr).ToScalarOrDefault();

                foreach (PurchaseOrderModel m in result.Data)
                {
                    pom = new PurchaseOrderModel();
                    pom.PurchaseOrderId = m.PurchaseOrderId;
                    pom.CreateTime = m.CreateTime;
                    pom.status = m.status;
                    pom.OrderDetailViews = pod.All()
                    .LeftJoin(pro).On(pro.ProductId == pod.ProductId)
                    .LeftJoin(prol).On(prol.ProductId == pod.ProductId && prol.LanguageID == model.LanguageID)
                    .LeftJoin(pic).On(pic.ProductId == pod.ProductId && pic.Flag == 1)
                    .Select(pod.ProductId, prol.ProductName, pic.PicUrl, pod.SkuName, pod.Quantity)
                    .Where(pod.PurchaseOrderId == m.PurchaseOrderId).ToList<OrderDetailsModel>();
                    list.Add(pom); 
                }
            }
            result.Data = list;
            return result;
        }

        #endregion
    }
}
