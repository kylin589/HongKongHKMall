using BrCms.Core.Infrastructure;
using HKTHMall.Domain.Models;
using HKTHMall.Domain.Models.AC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Services.AC
{
    /// <summary>
    /// 系统功能接口
    /// <remarks>added by jimmy,2015-7-1</remarks>
    /// </summary>
    public interface IAC_FunctionService : IDependency
    {
        /// <summary>
        /// 通过Id查询系统功能对象
        /// </summary>
        /// <param name="id">主键Id</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-1</remarks>
        ResultModel GetAC_FunctionById(int id);

        /// <summary>
        ///系统功能分页查询
        /// </summary>
        /// <param name="model">输入查询参数对象</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-1</remarks>
        ResultModel Select(SearchAC_FunctionModel model);

        /// <summary>
        /// 添加系统功能
        /// </summary>
        /// <param name="model">系统功能对象</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-1</remarks>
        ResultModel Add(AC_FunctionModel model);

        /// <summary>
        /// 通过Id删除系统功能
        /// </summary>
        /// <param name="id">系统参数id</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-1</remarks>
        ResultModel Delete(int id);

        /// <summary>
        /// 更新系统功能
        /// </summary>
        /// <param name="model">系统功能对象</param>
        /// <returns>返回true时,表示更新成功；反之,表示更新失败</returns>
        /// <remarks>added by jimmy,2015-7-1</remarks>
        ResultModel Update(AC_FunctionModel model);

         /// <summary>
        ///查询功能点列表
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
         ResultModel GetAC_FunList();

    }
}
