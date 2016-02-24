using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.WebModel.Models.AccountRecharge
{
    public class AccountRechargeModel
    {
        /// <summary>
        /// 异动金额(充值金额)ZJ_UserBalanceChangeLog【用户账户金额异动记录表】(资金流水账)
        /// </summary>
        public decimal AddOrCutAmount { get; set; }

        /// <summary>
        /// 充值通道（(2贝宝支付,3国际信用卡支付)）
        /// </summary>
        public int RechargeChannel { get; set; }

        /// <summary>
        /// 会员ID
        /// </summary>
        public long UserID { get; set; }

        /// <summary>
        /// 用户名（YH_User表,登陆账号）
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 异动类型(充值类型)
        /// </summary>
        public int AddOrCutType { get; set; }

        /// <summary>
        /// 订单编号（用户充值订单表ID）
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        ///  订单来源（0: 网站；1:移动设备）
        /// </summary>
        public byte OrderSource { get; set; }

        /// <summary>
        /// 支付编号(订单支付信息表ID)
        /// </summary>
        public string PaymentOrderID { get; set; }

        /// <summary>
        /// 第三方支付编号
        /// </summary>
        public string outOrderId { get; set; }

    }
}
