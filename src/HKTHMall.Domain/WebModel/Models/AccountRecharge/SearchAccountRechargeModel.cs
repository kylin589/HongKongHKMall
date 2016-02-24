using BrCms.Framework.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.WebModel.Models.AccountRecharge
{
    public class SearchAccountRechargeModel:Paged
    {
        /// <summary>
        /// 订单编号（用户充值订单表ID）
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// 支付编号(订单支付信息表ID)
        /// </summary>
        public string PaymentOrderID { get; set; }
        

    }
}
