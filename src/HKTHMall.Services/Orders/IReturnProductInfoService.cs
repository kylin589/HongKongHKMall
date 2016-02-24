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
    /// Return_Goods退换货记录
    /// </summary>
    public interface IReturnProductInfoService : IDependency
    {
        /// <summary>
        /// 添加退换货记录
        /// </summary>
        /// <param name="model">退换货记录模型</param>
        /// <returns>是否成功</returns>
        ResultModel AddReturnProductInfo(ReturnProductInfoModel model, int languageID);

        /// <summary>
        /// 根据退换货记录id获退换货记录
        /// </summary>
        /// <param name="id">退换货记录id</param>
        /// <returns>退换货记录模型</returns>
        ResultModel GetReturnProductInfoById(string Return_GoodsId);

        /// <summary>
        /// 获取退换货记录列表
        /// </summary>
        /// <param name="parentId">父Id</param>
        /// <returns>退换货记录列表</returns>
        ResultModel GetReturnProductInfoList(SearchReturnProductInfoModel model);

        /// <summary>
        /// 获取退换货记录和订单明细关联列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// wuyf
        ResultModel GetReturnProductInfoOrderDetailsList(SearchReturnProductInfoModel model);

        ResultModel GetReturnProductInfo(SearchReturnProductInfoModel model);

        /// <summary>
        /// 更新退换货记录(审核)
        /// </summary>
        /// <param name="model">退换货记录模型</param>
        /// <returns>是否修改成功</returns>
        ResultModel UpdateReturnProductInfo(ReturnProductInfoModel model);

        /// <summary>
        /// 审核退换货记录（申请驳回）
        /// </summary>
        /// <param name="model">退货记录模型</param>
        /// <returns>是否修改成功</returns>
        /// wuyf
        ResultModel UpdateReturnProductInfoBH(ReturnProductInfoModel model);

        /// <summary>
        /// 更新退换货记录(收货)
        /// </summary>
        /// <param name="model">退换货记录模型</param>
        /// <returns>是否修改成功</returns>
        ResultModel UpdateReturnProductInfoSH(ReturnProductInfoModel model);

        /// <summary>
        /// 更新退换货记录(退款)
        /// </summary>
        /// <param name="model">退换货记录模型</param>
        /// <returns>是否修改成功</returns>
        ResultModel UpdateReturnProductInfoTK(ReturnProductInfoModel model);

        /// <summary>
        /// 删除退换货记录
        /// </summary>
        /// <param name="model">退换货记录模型</param>
        /// <returns>是否删除成功</returns>
        ResultModel DeleteReturnProductInfo(ReturnProductInfoModel model);

        /// <summary>
        /// 用户申请撤消退换货记录
        /// zhoub 20150815
        /// </summary>
        /// <param name="model">退货记录模型</param>
        /// <returns>是否修改成功</returns>
        ResultModel UndoReturnProductInfoBH(ReturnProductInfoModel model, int languageID);

        /// <summary>
        /// 取消退換貨
        /// 黃主霞 2016-01-20
        /// </summary>
        /// <param name="model">退换货记录模型</param>
        /// <param name="languageID">语言ID(默认繁体 4)</param>
        /// <returns>是否成功</returns>
        ResultModel CancelReturnProductInfo(ReturnProductInfoModel model, int languageID = 4);
    }
}
