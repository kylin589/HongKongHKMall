using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrCms.Core.Infrastructure;
using BrCms.Framework.Collections;
using HKTHMall.Domain.AdminModel.Models.AC;
using HKTHMall.Domain.Models;
using HKTHMall.Domain.Models.AC;

namespace HKTHMall.Services.AC
{
    /// <summary>
    /// 部门接口
    /// </summary>
    public interface IAC_DepartmentService : IDependency
    {
        /// <summary>
        /// 添加部门
        /// </summary>
        /// <param name="model">部门模型</param>
        ResultModel AddAC_Department(AC_DepartmentModel model);

        /// <summary>
        /// 根据部门id获取部门
        /// </summary>
        /// <param name="id">部门id</param>
        /// <returns>部门模型</returns>
        ResultModel GetAC_DepartmentById(int id);

        /// <summary>
        /// 获取部门列表
        /// </summary>
        /// <param name="parentId">父Id</param>
        /// <returns>部门列表</returns>
        ResultModel GetAC_DepartmentsBy();

        /// <summary>
        /// 更新部门
        /// </summary>
        /// <param name="model">部门模型</param>
        /// <returns>是否修改成功</returns>
        ResultModel UpdateAC_Department(AC_DepartmentModel model);

        /// <summary>
        /// 删除部门
        /// </summary>
        /// <param name="id">部门Id</param>
        /// <returns>是否删除成功</returns>
        ResultModel DeleteAC_DepartmentById(int id);

        /// <summary>
        /// 分页获取部门列表
        /// </summary>
        /// <param name="model">部门搜索模型</param>
        /// <returns>部门列表数据</returns>
        ResultModel GetPagingAC_Departments(SearchAC_DepartmentModel model);
    }
}
