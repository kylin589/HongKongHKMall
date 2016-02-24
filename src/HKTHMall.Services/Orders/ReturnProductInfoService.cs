using BrCms.Framework.Collections;
using HKTHMall.Domain.AdminModel.Models.Orders;
using HKTHMall.Domain.AdminModel.Models.User;
using HKTHMall.Domain.Models;
using HKTHMall.Services.Common.MultiLangKeys;
using HKTHMall.Services.Users;
using Simple.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKTHMall.Domain.CashBack;

namespace HKTHMall.Services.Orders
{
    /// <summary>
    /// Return_Goods退换货记录
    /// </summary>
    public class ReturnProductInfoService : BaseService, IReturnProductInfoService
    {


        /// <summary>
        /// 添加退换货记录
        /// zhoub 20150820
        /// </summary>
        /// <param name="model">退换货记录模型</param>
        /// <returns>是否成功</returns>
        public ResultModel AddReturnProductInfo(ReturnProductInfoModel model, int languageID)
        {
            ResultModel result = new ResultModel();
            var isReturn = _database.Db.OrderDetails.FindAllByOrderDetailsID(model.OrderDetailsID).Select(_database.Db.OrderDetails.IsReturn).ToScalarOrDefault();
            if (isReturn > 0)
            {
                result.IsValid = false;
                result.Messages = new List<string> { CultureHelper.GetAPPLangSgring("ACCOUNT_My_OrderReturnProductInfo_SaveErrorMessage", languageID) + "." };
            }
            else
            {
                isReturn = _database.Db.Order.FindAllByOrderID(model.OrderID).Select(_database.Db.Order.OrderStatus).ToScalarOrDefault();
                if (isReturn == 5)
                {
                    _database.Db.ReturnProductInfo.Insert(model);
                    result.Data = _database.Db.OrderDetails.UpdateByOrderDetailsID(OrderDetailsID: model.OrderDetailsID, IsReturn: 1);
                    if (result.Data > 0)
                    {
                        //修改订单退款状态
                        _database.Db.Order.UpdateByOrderID(OrderID: model.OrderID, RefundFlag: 1);
                        result.Messages = new List<string> { CultureHelper.GetAPPLangSgring("MY_APPLICATION_HAS_BEEN_SUBMITTED", languageID) + "." };
                    }
                    else
                    {
                        result.IsValid = false;
                        result.Messages = new List<string> { CultureHelper.GetAPPLangSgring("ACCOUNT_MY_ORDERRETURNPRODUCT_FAIL", languageID) + "." };
                    }
                }
                else
                {
                    result.IsValid = false;
                    result.Messages = new List<string> { CultureHelper.GetAPPLangSgring("ACCOUNT_MY_ORDERRETURNPRODUCT_FAIL", languageID) + "." };
                }
            }
            return result;
        }

        /// <summary>
        /// 取消退換貨
        /// 黃主霞 2016-01-20
        /// </summary>
        /// <param name="model">退换货记录模型</param>
        /// <param name="languageID">语言ID(默认繁体 4)</param>
        /// <returns>是否成功</returns>
        public ResultModel CancelReturnProductInfo(ReturnProductInfoModel model, int languageID = 4)
        {
            ResultModel result = new ResultModel();
            var isReturn = _database.Db.OrderDetails.FindAllByOrderDetailsID(model.OrderDetailsID).Select(_database.Db.OrderDetails.IsReturn).ToScalarOrDefault();
            if (isReturn != 1) //只有处于退款中才能进行取消退款操作
            {
                result.IsValid = false;
                result.Messages = new List<string> { "订单处于无法取消状态" };
            }
            else
            {
                base._database.Db.ReturnProductInfo.Delete(ReturnOrderID: model.ReturnOrderID);
                result.Data = _database.Db.OrderDetails.UpdateByOrderDetailsID(OrderDetailsID: model.OrderDetailsID, IsReturn: 0);
                if (result.Data > 0)
                {
                    //修改订单退款状态
                    _database.Db.Order.UpdateByOrderID(OrderID: model.OrderID, RefundFlag: 0);
                    result.Messages = new List<string> { CultureHelper.GetAPPLangSgring("MY_APPLICATION_HAS_BEEN_SUBMITTED", languageID) + "." };
                }
                else
                {
                    result.IsValid = false;
                    result.Messages = new List<string> { CultureHelper.GetAPPLangSgring("ACCOUNT_MY_ORDERRETURNPRODUCT_FAIL", languageID) + "." };
                }
            }
            return result;
        }

        /// <summary>
        /// 根据退换货记录id获退换货记录
        /// </summary>
        /// <param name="id">退换货记录id</param>
        /// <returns>退换货记录模型</returns>
        /// wuyf
        public ResultModel GetReturnProductInfoById(string ReturnOrderID)
        {
            var tb = _database.Db.ReturnProductInfo;
            var where = tb.ReturnOrderID.Like("%" + ReturnOrderID + "%");
            var result = new ResultModel
            {
                Data = new SimpleDataPagedList<ReturnProductInfoModel>(tb.FindAll(where),
                        0, 100)
            };

            return result;
        }

        /// <summary>
        /// 获取退换货记录列表
        /// </summary>
        /// <param name="parentId">父Id</param>
        /// <returns>退换货记录列表</returns>
        /// wuyf
        public ResultModel GetReturnProductInfoList(SearchReturnProductInfoModel model)
        {
            var tb = _database.Db.ReturnProductInfo;
            var where = new SimpleExpression(1, 1, SimpleExpressionType.Equal); //

            if (model.ReturnOrderID != null && !string.IsNullOrEmpty(model.ReturnOrderID.Trim()))
            {
                //主键
                where = new SimpleExpression(where, tb.ReturnOrderID == model.ReturnOrderID, SimpleExpressionType.And);
            }

            if (!string.IsNullOrEmpty(model.OrderID))
            {
                //查询订单ID
                where = new SimpleExpression(where, tb.OrderID.Like("%" + model.OrderID.Trim() + "%"), SimpleExpressionType.And);
            }

            if (model.UserID > 0)
            {
                //用户ID
                where = new SimpleExpression(where, tb.UserID == model.UserID, SimpleExpressionType.And);
            }
            if (model.Phone != null && !string.IsNullOrEmpty(model.Phone.Trim()))
            {
                //用户手机
                where = new SimpleExpression(where, _database.Db.YH_User.Phone.Like("%" + model.Phone.Trim() + "%"), SimpleExpressionType.And);
            }
            if (model.Email != null && !string.IsNullOrEmpty(model.Email.Trim()))
            {
                //用户手机
                where = new SimpleExpression(where, _database.Db.YH_User.Email.Like("%" + model.Email.Trim() + "%"), SimpleExpressionType.And);
            }

            if (model.ReturnStatus > 0)
            {
                //状态 （1:退款申请中（会员撤消操作）,2:审核通过（用户发货（系统上不会体现）,后台确认收货操作）,3申请已驳回,4已收货（后台退款操作）,5已退款,6申请已撤消）
                where = new SimpleExpression(where, tb.ReturnStatus == model.ReturnStatus, SimpleExpressionType.And);
            }

            if (model.ReturnType > 0)
            {
                //退换货类型 1退货,2换货,3返修
                where = new SimpleExpression(where, tb.ReturnType == model.ReturnType, SimpleExpressionType.And);
            }

            dynamic cl, pc, mer, pro, detailsLang;

            var query = tb
                .Query()
                .LeftJoin(_database.Db.OrderDetails, out cl).On(_database.Db.OrderDetails.OrderDetailsID == tb.OrderDetailsID)
                .LeftJoin(_database.Db.OrderDetails_lang, out detailsLang).On(detailsLang.OrderDetailsID == cl.OrderDetailsID && detailsLang.LanguageID == model.LanguageID)
                .LeftJoin(_database.Db.YH_User, out pc).On(_database.Db.YH_User.UserID == tb.UserID)
                 .LeftJoin(_database.Db.Product, out pro).On(pro.ProductId == tb.ProductId)
                .LeftJoin(_database.Db.YH_MerchantInfo, out mer).On(mer.MerchantID == pro.MerchantID)
                .Select(
                    tb.ReturnOrderID.Distinct(),
                    tb.OrderID,
                    tb.UserID,
                    tb.ProductId,
                    tb.ProductSnapshotID,
                    tb.ReturnType,
                    tb.ReturnStatus,
                    tb.TradeAmount,
                    tb.RefundAmount,
                    tb.ReturntNumber,
                    tb.ReasonType,
                    tb.Discription,

                    tb.ReturnAddress,
                    tb.ReceiverName,
                    tb.ReceiverMobile,
                    tb.ReceiverTel,

                    tb.MerchantReturnAddress,
                    tb.ReturnText,
                    tb.CreateTime,
                    tb.UpdateTime,
                    tb.AuditUser,

                    tb.Receiver,
                    tb.DeliveryDate,
                    tb.RefundPerson,
                    tb.RefundDate,

                    detailsLang.ProductName,
                    cl.CostPrice,
                    cl.SalesPrice,
                    pc.Account,
                    pc.Phone,
                    pc.NickName,
                    pc.RealName,
                    pc.Email,
                    cl.SkuName,
                    cl.OrderDetailsID,
                    cl.IsReturn,
                    cl.Quantity,
                    mer.ShopName
                )
                .Where(where)
                .OrderByCreateTimeDescending();

            var result = new ResultModel
            {
                Data = new SimpleDataPagedList<ReturnProductInfoModel>(query,
                        model.PagedIndex, model.PagedSize)
            };

            return result;
        }

        /// <summary>
        /// 审核退换货记录（通过）
        /// </summary>
        /// <param name="model">退货记录模型</param>
        /// <returns>是否修改成功</returns>
        /// wuyf
        public ResultModel UpdateReturnProductInfo(ReturnProductInfoModel model)
        {
            var result = new ResultModel()
            {
                Data = base._database.Db.ReturnProductInfo.UpdateByReturnOrderID(ReturnOrderID: model.ReturnOrderID, ReturnText: model.ReturnText, RefundAmount: model.RefundAmount, ReturnStatus: model.ReturnStatus, AuditUser: model.AuditUser, UpdateTime: model.UpdateTime)
            };

            result.IsValid = result.Data > 0 ? true : false;

            return result;
        }

        /// <summary>
        /// 审核退换货记录（申请驳回）
        /// </summary>
        /// <param name="model">退货记录模型</param>
        /// <returns>是否修改成功</returns>
        /// wuyf
        public ResultModel UpdateReturnProductInfoBH(ReturnProductInfoModel model)
        {
            var result = new ResultModel();

            using (var tx1 = _database.Db.BeginTransaction())
            {
                try
                {
                    //退款表 修改状态成(3)
                    tx1.ReturnProductInfo.UpdateByReturnOrderID(ReturnOrderID: model.ReturnOrderID, ReturnText: model.ReturnText, RefundAmount: model.RefundAmount, ReturnStatus: model.ReturnStatus, AuditUser: model.AuditUser, UpdateTime: model.UpdateTime);
                    //订单表 退款标识 修改成 已处理(2) 


                    //订单明细表 退货状态 改成 审核未通过(3)
                    tx1.OrderDetails.UpdateByOrderDetailsID(OrderDetailsID: model.OrderDetailsID, IsReturn: 3);



                    tx1.Commit();

                    IsStust(model);
                }
                catch (Exception ex)
                {
                    tx1.Rollback();

                    result.IsValid = false;
                    result.Messages.Add(ex.Message);
                }
            }

            return result;
        }

        /// <summary>
        /// 修改订单表退款状态 2 已完成
        /// </summary>
        /// <param name="model"></param>
        /// <param name="tx1"></param>
        private void IsStust(ReturnProductInfoModel model)
        {
            //过滤掉自己和相同订单存在退款流程的未退款状态
            var returnCount = _database.Db.ReturnProductInfo.GetCount(_database.Db.ReturnProductInfo.ReturnOrderID != model.ReturnOrderID && _database.Db.ReturnProductInfo.OrderID == model.OrderID && (_database.Db.ReturnProductInfo.ReturnStatus == 1 || _database.Db.ReturnProductInfo.ReturnStatus == 2 || _database.Db.ReturnProductInfo.ReturnStatus == 4));
            //不存在,修改,存在不修改
            if (returnCount == 0)
            {
                _database.Db.Order.UpdateByOrderID(OrderID: model.OrderID, RefundFlag: 2);
            }
        }

        /// <summary>
        /// 收货退换货记录（收货）
        /// </summary>
        /// <param name="model">退货记录模型</param>
        /// <returns>是否修改成功</returns>
        /// wuyf
        public ResultModel UpdateReturnProductInfoSH(ReturnProductInfoModel model)
        {
            var result = new ResultModel()
            {
                Data = base._database.Db.ReturnProductInfo.UpdateByReturnOrderID(ReturnOrderID: model.ReturnOrderID, Receiver: model.Receiver, ReturnStatus: model.ReturnStatus, DeliveryDate: model.DeliveryDate)
            };

            result.IsValid = result.Data > 0 ? true : false;

            return result;
        }

        /// <summary>
        /// 退换货记录(确认退款)
        /// </summary>
        /// <param name="model">退货记录模型</param>
        /// <returns>是否修改成功</returns>
        /// wuyf
        public ResultModel UpdateReturnProductInfoTK(ReturnProductInfoModel model)
        {
            //修改退货表 状态 5已退款
            //订单明细表 退货状态 改成 2已退款
            //订单表 退款标识 修改成 已处理
            //订单表 状态改成8交易关闭（需要判断订单明细表里的相关产品数据都是已经退款状态）
            //给退款用户的账户余额添加退款 费用

            var result = new ResultModel();
            #region 订单表 状态改成8交易关闭（需要判断订单明细表里的相关产品数据都是已经退款状态）
            var bl = true;//是否把当前订单的状态改成 8已关闭
            SearchReturnProductInfoModel srpm = new SearchReturnProductInfoModel();
            srpm.OrderID = model.OrderID;
            srpm.PagedIndex = 0;
            srpm.PagedSize = 100;
            //获取退换货记录和订单明细关联列表
            List<ReturnProductInfoModel> list = GetReturnProductInfoList(srpm).Data;// GetReturnProductInfoOrderDetailsList(srpm).Data;

            //订单明细
            List<OrderDetails> orderDetails = _database.Db.OrderDetails.FindAll(_database.Db.OrderDetails.OrderID == model.OrderID).ToList<OrderDetails>();
            //判断订单详情与退款记录条数是否一致（是一个订单多个商品,还是一个订单一个商品）
            var factoryCount = orderDetails.Count();

            //if (list.Count == factoryCount)
            //{
            //    if (list.Count == 1)
            //    {
            //        ReturnProductInfoModel rpmodel = list[0];//只有一条数据时,订单明细主键和退货数量跟明细数量相等,订单表的状态可以改为 8已关闭

            //        //该订单商品已退完
            //        if (rpmodel.OrderDetailsID == model.OrderDetailsID && rpmodel.ReturntNumber < rpmodel.Quantity)
            //        {
            //            bl = false;
            //        }
            //    }
            //    else
            //    {
            //        for (int i = 0; i < list.Count; i++)
            //        {
            //            ReturnProductInfoModel rpmodel1 = list[i];
            //            //ReturnProductInfoModel returndetail = returnList.Where(x => x.OrderDetailsID == detail.OrderDetailsID).FirstOrDefault();

            //            if ((rpmodel1.OrderDetailsID != model.OrderDetailsID && rpmodel1.IsReturn != 2) || (rpmodel1.OrderDetailsID == model.OrderDetailsID && rpmodel1.ReturntNumber < rpmodel1.Quantity))//状态 2 是已退款
            //            {
            //                //退出循环,不修改订单状态 为 8交易关闭
            //                //该订单还有为退款的商品
            //                bl = false; break;
            //            }
            //        }
            //    }
            //}
            //else
            //{
            //    bl = false;
            //}

            //zhoub 20160129 edit
            if (list.Count > 0)
            {
                //查找当前退款订单产品数量是否全部退完
                ReturnProductInfoModel rpim = list.FindAll(a => a.OrderDetailsID == model.OrderDetailsID).FirstOrDefault();
                if (rpim.ReturntNumber != rpim.Quantity)
                {
                    bl = false;
                }
                //判断之前退款是否有未确认退款或是产品未全部退完的
                for (int i = 0; i < list.Count; i++)
                {
                    ReturnProductInfoModel rpmodel = list[i];
                    if (rpmodel.OrderDetailsID != model.OrderDetailsID)
                    {
                        if (rpmodel.IsReturn != 2 || rpmodel.ReturntNumber != rpmodel.Quantity)
                        {
                            bl = false;
                            break;
                        }
                    }
                }
            }
           

            #endregion

            using (var tx1 = _database.Db.BeginTransaction())
            {
                try
                {
                    //退款表 修改状态成(5)
                    tx1.ReturnProductInfo.UpdateByReturnOrderID(ReturnOrderID: model.ReturnOrderID, RefundPerson: model.RefundPerson, ReturnStatus: model.ReturnStatus, RefundDate: model.RefundDate);
                    //订单明细表 退货状态 改成 2已退款
                    tx1.OrderDetails.UpdateByOrderDetailsID(OrderDetailsID: model.OrderDetailsID, IsReturn: 2);


                    if (bl)
                    {
                        //订单表 订单状态 修改成 8已关闭 （需要判断订单明细表里的相关产品数据都是已经退款状态）
                        tx1.Order.UpdateByOrderID(OrderID: model.OrderID, OrderStatus: 8);
                    }


                    #region 给用户账户充值金额,金额等于退款金额
                    srpm.ReturnOrderID = model.ReturnOrderID;
                    //根据退款主键查询退款单子
                    //List<ReturnProductInfoModel> list1 = GetReturnProductInfoList(srpm).Data;
                    //ReturnProductInfoModel rmodel = list[0];
                    ZJ_UserBalanceModel zjmodel = new ZJ_UserBalanceModel();
                    zjmodel.Account = model.Account;
                    zjmodel.AddOrCutAmount = model.RefundAmount;
                    zjmodel.AddOrCutType = 4;
                    zjmodel.CreateBy = model.RefundPerson;
                    zjmodel.IsDisplay = 1;
                    zjmodel.Phone = model.Phone;
                    zjmodel.RealName = model.RealName;
                    zjmodel.Remark = "退货中的退款金额";
                    zjmodel.UserID = model.UserID;
                    zjmodel.OrderNo = model.OrderID;
                    UpdateZJ_UserBalance(zjmodel, tx1);
                    #endregion


                    tx1.Commit();
                    //订单表 退款标识 修改成 已处理(2) 
                    //tx1.Order.UpdateByOrderID(OrderID: model.OrderID, RefundFlag: 2);
                    IsStust(model);
                }
                catch (Exception ex)
                {
                    tx1.Rollback();

                    result.IsValid = false;
                    result.Messages.Add(ex.Message);
                }
            }

            return result;
        }

        /// <summary>
        /// 组装用户充值参数
        /// </summary>
        /// <param name="model"></param>
        /// <param name="tx"></param>
        internal void UpdateZJ_UserBalance(ZJ_UserBalanceModel model, dynamic tx)
        {

            if (model.UserID != 0 && model.AddOrCutType != 0)
            {


                //ZJ_UserBalanceChangeLog【用户账户金额异动记录表】(资金流水账)
                #region 给流水账添加信息
                ZJ_UserBalanceChangeLogModel ulogModel = new ZJ_UserBalanceChangeLogModel();
                ulogModel.Account = model.Account;
                ulogModel.AddOrCutAmount = model.AddOrCutAmount;
                ulogModel.AddOrCutType = model.AddOrCutType;
                ulogModel.CreateBy = model.CreateBy;
                ulogModel.CreateDT = DateTime.Now;
                if (model.AddOrCutAmount >= 0)
                {
                    ulogModel.IsAddOrCut = 1;
                }
                else
                {
                    ulogModel.IsAddOrCut = 0;
                }
                ulogModel.IsDisplay = model.IsDisplay == 0 ? 0 : model.IsDisplay;
                //ulogModel.NewAmount = model.ConsumeBalance + model.AddOrCutAmount;
                //ulogModel.OldAmount = model.ConsumeBalance;
                ulogModel.OrderNo = model.OrderNo == null ? "" : model.OrderNo;
                ulogModel.Phone = model.Phone;
                ulogModel.RealName = model.RealName;
                ulogModel.Remark = model.Remark;
                ulogModel.UserID = model.UserID;
                #endregion

                //model.ConsumeBalance = model.ConsumeBalance + model.AddOrCutAmount;
                model.UpdateBy = model.CreateBy;
                model.UpdateDT = DateTime.Now;
                ZJ_UserBalanceService _zjUserBalanceService = new ZJ_UserBalanceService();
                _zjUserBalanceService.UpdateZJ_UserBalances(model, ulogModel, tx);
            }


        }

        /// <summary>
        /// 获取退换货记录和订单明细关联列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// wuyf
        public ResultModel GetReturnProductInfoOrderDetailsList(SearchReturnProductInfoModel model)
        {
            var tb = _database.Db.ReturnProductInfo;
            var where = new SimpleExpression(1, 1, SimpleExpressionType.Equal); //



            if (!string.IsNullOrEmpty(model.OrderID))
            {
                //查询订单ID
                where = new SimpleExpression(where, tb.OrderID.Like(model.OrderID.Trim()), SimpleExpressionType.And);
            }



            dynamic cl;

            var query = tb
                .Query()
                .LeftJoin(_database.Db.OrderDetails, out cl).On(_database.Db.OrderDetails.OrderID == tb.OrderID)

                .Select(
                    tb.ReturnOrderID,
                    tb.OrderID,
                    tb.UserID,
                    tb.ProductId,
                    tb.ProductSnapshotID,
                    tb.ReturnType,
                    tb.ReturnStatus,
                    tb.TradeAmount,
                    tb.RefundAmount,
                    tb.ReturntNumber,
                    tb.ReasonType,
                    tb.Discription,

                    tb.ReturnAddress,
                    tb.ReceiverName,
                    tb.ReceiverMobile,
                    tb.ReceiverTel,

                    tb.MerchantReturnAddress,
                    tb.ReturnText,
                    tb.CreateTime,
                    tb.UpdateTime,
                    tb.AuditUser,

                    tb.Receiver,
                    tb.DeliveryDate,
                    tb.RefundPerson,
                    tb.RefundDate,

                    cl.IsReturn,
                    cl.Quantity,
                    cl.OrderDetailsID

                )
                .Where(where)
                .OrderByCreateTimeDescending();

            var result = new ResultModel
            {
                Data = new SimpleDataPagedList<ReturnProductInfoModel>(query,
                        model.PagedIndex, model.PagedSize)
            };

            return result;
        }

        /// <summary>
        /// 删除退换货记录
        /// </summary>
        /// <param name="model">退货记录模型</param>
        /// <returns>是否删除成功</returns>
        /// wuyf
        public ResultModel DeleteReturnProductInfo(ReturnProductInfoModel model)
        {

            

            var result = new ResultModel();
            using (var tx1 = _database.Db.BeginTransaction())
            {
                try
                {
                    tx1.ReturnProductInfo.Delete(ReturnOrderID: model.ReturnOrderID);
                    tx1.OrderDetails.UpdateByOrderDetailsID(OrderDetailsID: model.OrderDetailsID, IsReturn: 0, Iscomment:0);
                    tx1.Commit();
                    result.IsValid = true;
                }
                catch (Exception ex)
                {
                    tx1.Rollback();
                    result.IsValid = false;
                    result.Messages = new List<string> { ex.Message };
                }
            }

            return result;
        }


        public ResultModel GetReturnProductInfo(SearchReturnProductInfoModel model)
        {
            var result = new ResultModel()
            {
                Data = new SimpleDataPagedList<ReturnProductInfoModel>(base._database.Db.ReturnProductInfo.All().Where(_database.Db.ReturnProductInfo.OrderDetailsID == model.OrderDetailsID && _database.Db.ReturnProductInfo.UserID == model.UserID),
                       model.PagedIndex, model.PagedSize)
            };
            return result;
            
        }

        /// <summary>
        /// 用户申请撤消退换货记录
        /// zhoub 20150815
        /// </summary>
        /// <param name="model">退货记录模型</param>
        /// <returns>是否修改成功</returns>
        public ResultModel UndoReturnProductInfoBH(ReturnProductInfoModel model, int languageID)
        {
            var result = new ResultModel();
            var info = _database.Db.ReturnProductInfo.FindAllByReturnOrderID(model.ReturnOrderID).ToList<ReturnProductInfoModel>();
            model.ReturnStatus = 6;
            model.UpdateTime = DateTime.Now;
            model.OrderID = info[0].OrderID;
            model.OrderDetailsID = info[0].OrderDetailsID;
            if (info[0].ReturnStatus == 1 || info[0].ReturnStatus == 3)
            {
                using (var tx1 = _database.Db.BeginTransaction())
                {
                    try
                    {
                        //退款表 修改状态成(6)
                        tx1.ReturnProductInfo.UpdateByReturnOrderID(ReturnOrderID: model.ReturnOrderID, ReturnStatus: model.ReturnStatus, UpdateTime: model.UpdateTime);
                        //订单明细表 退货状态 改成 审核未通过(3)
                        tx1.OrderDetails.UpdateByOrderDetailsID(OrderDetailsID: model.OrderDetailsID, IsReturn: 3);
                        tx1.Commit();
                        IsStust(model);
                        result.Messages.Add(CultureHelper.GetAPPLangSgring("ACCOUNT_RIGHTS_INDEX_UNDOSUCCESS", languageID) + ".");
                    }
                    catch (Exception ex)
                    {
                        tx1.Rollback();
                        result.IsValid = false;
                        result.Messages.Add(CultureHelper.GetAPPLangSgring("ACCOUNT_RIGHTS_INDEX_UNDOERROR", languageID) + ".");
                    }
                }
            }
            else
            {
                result.IsValid = false;
                result.Messages.Add(CultureHelper.GetAPPLangSgring("ACCOUNT_RIGHTS_INDEX_UNDOERROR", languageID) + ".");
            }
            return result;
        }
    }
}
