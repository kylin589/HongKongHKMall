using BrCms.Framework.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.Models.Orders
{
    public class SearchComplaintsModel : Paged
    {
        /// <summary>
        /// 订单Id 
        /// </summary>
        public string OrderID { get; set; }

        /// <summary>
        /// 用户userId
        /// </summary>
        public long UserID { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }
        /// <summary>
        /// 用户手机号
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 用户Email
        /// </summary>
        public string Email { get; set; }
    }
}
