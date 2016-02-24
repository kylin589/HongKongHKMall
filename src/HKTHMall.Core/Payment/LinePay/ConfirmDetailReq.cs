using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Core.Payment.LinePay
{
    /// <summary>
    /// 确认交易请求实体
    /// </summary>
    public class ConfirmDetailReq
    {

        /// <summary>
        ///  付款金額 必填
        /// </summary>
        public string amount
        {
            get;
            set;
        }
        /// <summary>
        /// 货币 THB 必填
        /// </summary>
        public string currency
        {
            get;
            set;
        }
    }
}
