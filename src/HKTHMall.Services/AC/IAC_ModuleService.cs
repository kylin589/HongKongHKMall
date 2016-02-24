using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrCms.Core.Infrastructure;
using HKTHMall.Domain.Models;
using HKTHMall.Domain.Models.AC;

namespace HKTHMall.Services.AC
{
    public interface IAC_ModuleService : IDependency
    {
        /// <summary>
        /// 通过Id查询系统模块对象
        /// </summary>
        /// <param name="id">主键Id</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-6</remarks>
        ResultModel GetAC_ModuleById(int id);

        /// <summary>
        ///系统模块分页查询
        /// </summary>
        /// <param name="model">输入查询参数对象</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-6</remarks>
        ResultModel Select(SearchAC_ModuleModel model);

        /// <summary>
        /// 添加系统模块
        /// </summary>
        /// <param name="model">系统模块对象</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-6</remarks>
        ResultModel Add(AC_ModuleModel model);

        /// <summary>
        /// 通过Id删除系统模块
        /// </summary>
        /// <param name="id">系统参数id</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-6</remarks>
        ResultModel Delete(int id);

        /// <summary>
        /// 更新系统模块
        /// </summary>
        /// <param name="model">系统模块对象</param>
        /// <returns>返回true时,表示更新成功；反之,表示更新失败</returns>
        /// <remarks>added by jimmy,2015-7-6</remarks>
        ResultModel Update(AC_ModuleModel model);

        /// <summary>
        ///查询系统模块列表
        /// </summary>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-7</remarks>
        ResultModel GetAC_ModuleList();

        /// <summary>
        ///查询系统模块列表
        /// </summary>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-7</remarks>
        ResultModel GetAC_ModuleList(int parentId);

        ///// <summary>
        ///// 根据角色模块值取菜单 
        ///// </summary>
        ///// <param name="Idstr"></param>
        ///// <returns></returns>
        ///// <remarks>liuhongwen,2015-7-8</remarks>
        //Result GetAC_ModuleByIDstr(string Idstr);

        /// <summary>
        /// 获取菜单数据
        /// zhoub 20150709
        /// </summary>
        /// <returns></returns>
        ResultModel GetAC_ModuleToTree();

        /// <summary>
        /// 更新菜单排序
        /// zhoub 20150713
        /// </summary>
        /// <param name="modeleId"></param>
        /// <param name="place"></param>
        /// <returns></returns>
        ResultModel UpdatePlace(int modeleId,long place);
    }
}
