using BrCms.Core.Infrastructure;
using HKTHMall.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKTHMall.Domain.Models.AC;
using BrCms.Framework.Collections;
//using HKTHMall.Domain.Models.Sys;

namespace HKTHMall.Services.AC
{
    /// <summary>
    /// 系统角色接口类
    /// <remarks>added by jimmy,2015-7-1</remarks>
    /// </summary>
    public interface IAC_RoleService : IDependency
    {
        /// <summary>
        /// 添加角色
        /// </summary>
        /// <param name="model">角色</param>
        ResultModel Add(AC_RoleModel model);

        /// <summary>
        /// 更新角色
        /// </summary>
        /// <param name="model">角色模型</param>
        /// <returns>是否修改成功</returns>
        ResultModel Update(AC_RoleModel model);

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="id">角色Id</param>
        /// <returns>是否删除成功</returns>
        ResultModel Delete(int id);

         /// <summary>
        /// 根据ID取角色信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ResultModel GetAC_RolesById(int id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model">部门搜索模型</param>
        /// <returns>部门列表数据</returns>
        ResultModel GetPagingList(SearchAC_RoleModel model);

        /// <summary>
        /// 获取角色列表
        /// zhoub 20150707
        /// </summary>
        /// <param name="parentId">父Id</param>
        /// <returns>角色列表</returns>
        ResultModel GetAC_RolesBy();

        /// <summary>
        /// 根据角色模块值取菜单 
        /// </summary>
        /// <param name="Idstr"></param>
        /// <returns></returns>
        /// <remarks>liuhongwen,2015-7-8</remarks>
        ResultModel GetAC_ModuleByIDstr(string Idstr);

        /// <summary>
        /// 根据角色ID取功能点权限
        /// </summary>
        /// <param name="RoleId"></param>
        /// <returns></returns>
        ResultModel GetFunctionList(int RoleId);

         /// <summary>
        /// 根据角色ID取菜单权限
        /// </summary>
        /// <param name="RoleId"></param>
        /// <returns></returns>
        ResultModel GetModuleMenuList(int RoleId);
    }
}
