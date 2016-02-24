using BrCms.Framework.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.AdminModel.Models.Orders
{
    public class SearchPurchaseOrderDetailsModel : Paged
    {

        /// <summary>
        /// 商品名称 
        /// </summary>
        public string ProductName { get; set; }

        public string PurchaseOrderId { get; set; }
    }
}
