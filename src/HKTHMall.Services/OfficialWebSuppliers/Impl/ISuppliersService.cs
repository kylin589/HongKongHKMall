using BrCms.Core.Infrastructure;
using HKTHMall.Domain.Models;
using HKTHMall.Domain.OfficialWeb.Models.Suppliers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Services.OfficialWebSuppliers.Impl
{
    public interface ISuppliersService : IDependency
    {
        

        

        /// <summary>
        /// 获取供应商列表（wuyf）
        /// </summary>
        /// <param name="parentId">父Id</param>
        /// <returns>供应商列表</returns>
        ResultModel GetSuppliers(SalesSuppliersModel model);

        /// <summary>
        /// 根据手机号码查询供应商信息（wuyf）,登录用了
        /// </summary>
        /// <param name="Mobile"></param>
        /// <returns></returns>
        ResultModel GetSuppliersMobile(string Mobile, string PassWord);
    }
}
