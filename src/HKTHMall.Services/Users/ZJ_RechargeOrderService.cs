using BrCms.Framework.Collections;
using HKTHMall.Domain.AdminModel.Models.User;
using HKTHMall.Domain.Models;
using Simple.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Services.Users
{
    public class ZJ_RechargeOrderService : BaseService, IZJ_RechargeOrderService
    {
        /// <summary>
        /// 获取用户充值订单表
        /// </summary>
        /// <param name="model">model</param>
        /// <returns>用户充值订单表</returns>
        /// wuyf
        public ResultModel GetZJ_RechargeOrderList(SearchZJ_RechargeOrderModel model)
        {
            var tb = _database.Db.ZJ_RechargeOrder;
            var where = new SimpleExpression(1, 1, SimpleExpressionType.Equal); //

            if (!string.IsNullOrEmpty(model.Email))
            {
                //用户Email
                where = new SimpleExpression(where, _database.Db.YH_User.Email.Like("%" + model.Email.Trim() + "%"), SimpleExpressionType.And);
            }
            if (!string.IsNullOrEmpty(model.RealName))
            {
                //用户真实姓名
                where = new SimpleExpression(where, _database.Db.YH_User.RealName.Like("%" + model.RealName.Trim() + "%"), SimpleExpressionType.And);
            }
            if (!string.IsNullOrEmpty(model.Account))
            {
                //用户登录账户
                where = new SimpleExpression(where, _database.Db.YH_User.Account.Like("%" + model.Account.Trim() + "%"), SimpleExpressionType.And);
            }
            if (model.UserID > 0)
            {
                //用户ID
                where = new SimpleExpression(where, tb.UserID == model.UserID, SimpleExpressionType.And);
            }
            if ( !string.IsNullOrEmpty(model.OrderNO))
            {
                //订单编号
                where = new SimpleExpression(where, tb.OrderNO.Like("%" + model.OrderNO.Trim() + "%"), SimpleExpressionType.And);
            }
            if (model.OrderSource!=10)
            {
                //订单来源
                where = new SimpleExpression(where, tb.OrderSource ==model.OrderSource, SimpleExpressionType.And);
            }
            if (model.BeginCreateDT != null&&model.BeginCreateDT.Year!=0001)
            {
                //订单生成 开始时间 
                where = new SimpleExpression(where, tb.CreateDT >= model.BeginCreateDT, SimpleExpressionType.And);
            }
            if (model.EedCreateDT != null && model.EedCreateDT.Year != 0001)
            {
                //订单生成 结束时间 
                where = new SimpleExpression(where, tb.CreateDT < model.EedCreateDT, SimpleExpressionType.And);
            }
            if (model.BeginRechargeDT != null && model.BeginRechargeDT.Value.Year != 0001)
            {
                //充值 开始时间 
                where = new SimpleExpression(where, tb.RechargeDT >= model.BeginRechargeDT, SimpleExpressionType.And);
            }
            if (model.EndRechargeDT != null && model.EndRechargeDT.Value.Year != 0001)
            {
                //充值  结束时间 
                where = new SimpleExpression(where, tb.RechargeDT < model.EndRechargeDT, SimpleExpressionType.And);
            }

            if (model.RechargeChannel>0)
            {
                //充值  通道
                where = new SimpleExpression(where, tb.RechargeChannel == model.RechargeChannel, SimpleExpressionType.And);
            }

            if ( model.RechargeResult != 10)
            {
                //充值  结果
                where = new SimpleExpression(where, tb.RechargeResult == model.RechargeResult, SimpleExpressionType.And);
            }

            dynamic pc;

            var query = tb
                .Query()

                .LeftJoin(_database.Db.YH_User, out pc)
                .On(_database.Db.YH_User.UserID == tb.UserID)
                .Select(
                    tb.UserID,
                    tb.OrderNO,
                    tb.RechargeChannel,
                    tb.RechargeAmount,
                    tb.RechargeDT,
                    tb.RechargeResult,
                    tb.CreateDT,
                    tb.IsDisplay,
                    tb.OrderSource,
                    

                    pc.Phone,
                    pc.NickName,
                    pc.RealName,
                    pc.Email,
                    pc.Account
                )
                .Where(where)
                .OrderByUserIDDescending();

            var result = new ResultModel
            {
                Data = new SimpleDataPagedList<ZJ_RechargeOrderModel>(query,
                        model.PagedIndex, model.PagedSize)
            };

            return result;
        }

        /// <summary>
        /// 添加用户充值订单表
        /// </summary>
        /// <param name="model">用户充值订单表</param>
        /// <returns>是否成功</returns>
        /// wuyf
        public ResultModel AddZJ_RechargeOrder(ZJ_RechargeOrderModel model)
        {
            var result = new ResultModel
            {
                Data = _database.Db.ZJ_RechargeOrder.Insert(model)
            };
            return result;
        }
    }
}
