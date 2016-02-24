using BrCms.Core.Infrastructure;
using HKTHMall.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKTHMall.Domain.AdminModel.Models.Products;

namespace HKTHMall.Services.Products
{
    public interface ISalesRuleService : IDependency
    {
        /// <summary>
        /// 通过Id查询促销规则对象
        /// </summary>
        /// <param name="id">主键Id</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-10</remarks>
        ResultModel GetSalesRuleById(int id);

        /// <summary>
        ///促销规则分页查询
        /// </summary>
        /// <param name="model">输入查询参数对象</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-10</remarks>
        ResultModel Select(SearchSalesRuleModel model);

        /// <summary>
        /// 添加促销规则
        /// </summary>
        /// <param name="model">促销规则对象</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-10</remarks>
        ResultModel Add(SalesRuleModel model);

        /// <summary>
        /// 通过Id删除促销规则
        /// </summary>
        /// <param name="id">系统参数id</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-10</remarks>
        ResultModel Delete(int id);

        /// <summary>
        /// 更新促销规则
        /// </summary>
        /// <param name="model">促销规则对象</param>
        /// <returns>返回true时,表示更新成功；反之,表示更新失败</returns>
        /// <remarks>added by jimmy,2015-7-10</remarks>
        ResultModel Update(SalesRuleModel model);

        /// <summary>
        /// 查询所有的促销规则信息
        /// </summary>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-10</remarks>
        ResultModel GetSalesRuleList();
    }
}
