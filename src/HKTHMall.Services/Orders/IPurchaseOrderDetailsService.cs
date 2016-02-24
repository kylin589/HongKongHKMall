using BrCms.Core.Infrastructure;
using HKTHMall.Domain.AdminModel.Models.Orders;
using HKTHMall.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Services.Orders
{
    public interface IPurchaseOrderDetailsService : IDependency
    {
        /// <summary>
        /// 供应商采购单明细分布查询
        /// </summary>
        /// <param name="model">输入查询参数对象</param>
        /// <param name="languageID">语言Id</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-9-8</remarks>
        ResultModel Select(SearchPurchaseOrderDetailsModel model, int languageID);
    }
}
