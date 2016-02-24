using BrCms.Core.Infrastructure;
using HKTHMall.Domain.AdminModel.Models.Orders;
using HKTHMall.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Services.Orders
{
    /// <summary>
    /// 供应商采购单接口
    /// <remarks>added by jimmy,2015-9-8</remarks>
    /// </summary>
    public interface IPurchaseOrderSerivce : IDependency
    {
        /// <summary>
        /// 供应商采购单分布查询
        /// </summary>
        /// <param name="model">输入查询参数对象</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-9-8</remarks>
        ResultModel Select(SearchPurchaseOrderModel model);

        /// <summary>
        /// 供应商采购单
        /// </summary>
        /// <param name="PurchaseOrderId">供应商采购单ID</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-9-8</remarks>
        ResultModel GetPurchaseOrder(string PurchaseOrderId);

        /// <summary>
        /// 更新收货状态
        /// </summary>
        /// <param name="model">对象</param>
        /// <returns>返回true时,表示更新成功；反之,表示更新失败</returns>
        /// <remarks>added by jimmy,2015-9-8</remarks>
        ResultModel Update(PurchaseOrderModel model);

        /// <summary>
        /// 取消订单时更改其状态和取消时间
        /// zhoub 20150910
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ResultModel UpdateByOrderId(PurchaseOrderModel model);

        #region  供应商后台管理系统传用

        /// <summary>
        /// 采购订单查询
        /// zhoub 20150928
        /// </summary>
        /// <param name="model"></param>
        /// <param name="totalCount">订单条数</param>
        /// <param name="productQuantity">产品个数</param>
        /// <returns></returns>
        ResultModel GetSuppliersPagingPurchaseOrder(SearchPurchaseOrderModel model, out int totalCount, out int productQuantity);

        #endregion

    }
}
