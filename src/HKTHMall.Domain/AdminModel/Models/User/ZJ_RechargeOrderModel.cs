using FluentValidation.Attributes;
using HKTHMall.Domain.AdminModel.Validators.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.AdminModel.Models.User
{
    [Validator(typeof(ZJ_RechargeOrderValidator))]
    public class ZJ_RechargeOrderModel
    {
        /// <summary>
        /// 订单编号
        /// </summary>
        public string OrderNO { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public long UserID { get; set; }

        /// <summary>
        /// 充值通道（(2贝宝支付,3国际信用卡支付)）
        /// </summary>
        public int RechargeChannel { get; set; }

        /// <summary>
        /// 充值金额
        /// </summary>
        public decimal RechargeAmount { get; set; }

        /// <summary>
        /// 充值时间
        /// </summary>
        public Nullable<System.DateTime> RechargeDT { get; set; }

        /// <summary>
        /// 充值结果,0:失败；1:成功
        /// </summary>
        public byte RechargeResult { get; set; }

        /// <summary>
        /// 订单生成时间
        /// </summary>
        public System.DateTime CreateDT { get; set; }

        /// <summary>
        /// 是否可见,0:隐藏,1显示
        /// </summary>
        public int IsDisplay { get; set; }

        /// <summary>
        ///  订单来源（0: 网站；1:移动设备）
        /// </summary>
        public byte OrderSource { get; set; }

        /// <summary>
        /// 用户名（YH_User表,登陆账号）
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 用户手机 （YH_User表）
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 用户Email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 用户真实姓名（YH_User表）
        /// </summary>
        public string RealName { get; set; }

        /// <summary>
        /// 用户昵称（YH_User表）
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 支付编号
        /// </summary>
        public string PaymentOrderID { get; set; }

    }
}
