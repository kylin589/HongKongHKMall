using BrCms.Framework.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.OfficialWeb.Models.Suppliers
{
    public class SalesSuppliersModel : Paged
    {
        /// <summary>
        /// ID
        /// </summary>
        public long SupplierId { get; set; }

        /// <summary>
        /// 供应商名称
        /// </summary>
        public string SupplierName { get; set; }

        /// <summary>
        /// 联系人
        /// </summary>
        public string LinkMan { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public string Telephone { get; set; }

        /// <summary>
        /// 手机
        /// </summary>
        public string Mobile { get; set; }


        /// <summary>
        /// 语言
        /// </summary>
        public int Lang { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
    }
}
