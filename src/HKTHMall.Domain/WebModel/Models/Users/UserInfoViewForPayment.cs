using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.WebModel.Models.Users
{
    /// <summary>
    /// 用户信息（用户支付）
    /// </summary>
    public class UserInfoViewForPayment
    {

        public int LanguageId { get; set; }

        public long UserID { get; set; }

        public string Account { get; set; }

        public string PayPassWord { get; set; }

        public int IsLock { get; set; }

        public int IsDelete { get; set; }

        public decimal ConsumeBalance { get; set; }

        public string Phone { get; set; }
    }
}
