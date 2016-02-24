using BrCms.Core.Infrastructure;
using HKTHMall.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKTHMall.Domain.Models.Sys;
using BrCms.Framework.Collections;

namespace HKTHMall.Services.Sys
{
    /// <summary>
    /// 系统参数设置接口
    /// <remarks>added by jimmy,2015-7-1</remarks>
    /// </summary>
    public interface IParameterSetService : IDependency
    {
        /// <summary>
        /// 通过Id查询系统参数设置对象
        /// </summary>
        /// <param name="id">主键Id</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-1</remarks>
        ResultModel GetParameterSetById(long id);

        /// <summary>
        /// 系统参数对象分布查询
        /// </summary>
        /// <param name="model">输入查询参数对象</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-1</remarks>
        ResultModel Select(SearchParaSetModel model);

        /// <summary>
        /// 添加系统参数设置
        /// </summary>
        /// <param name="model">系统参数设置对象</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-1</remarks>
        ResultModel Add(ParameterSetModel model);

        /// <summary>
        /// 通过Id删除系统参数设置
        /// </summary>
        /// <param name="id">系统参数id</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-1</remarks>
        ResultModel Delete(long id);

        /// <summary>
        /// 更新系统参数
        /// </summary>
        /// <param name="model">系统参数对象</param>
        /// <returns>返回true时,表示更新成功；反之,表示更新失败</returns>
        /// <remarks>added by jimmy,2015-7-1</remarks>
        ResultModel Update(ParameterSetModel model);

        /// <summary>
        /// 查询系统参数设置对象列表
        /// </summary>
        /// <param name="id">主键Id</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-2</remarks>
        ResultModel GetParameterSetList(long? id = null);

        /// <summary>
        /// 通过键名称查询系统参数设置对象列表
        /// </summary>
        /// <param name="id">主键Id</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-2</remarks>
        ResultModel GetParameterSetListByName(ParameterSetModel model);

        /// <summary>
        /// 查询系统参数
        /// </summary>
        /// <param name="key">主键Id</param>
        /// <returns></returns>
        /// <remarks></remarks>
        ResultModel GetParaSetByKeys(string key);

        /// <summary>
        /// 根据ID取系统参数的键值
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <remarks>added by liuhongwen,2015-7-23</remarks>
        ResultModel GetParametePValueById(long id);


        /// <summary>
        /// 根据ids集合获取系统参数
        /// </summary>
        /// <param name="ids">id集合</param>
        /// <returns></returns>
        ResultModel GetParameterSetsBy(long[] ids);

        /// <summary>
        /// 根据ids集合获取系统参数
        /// </summary>
        /// <param name="ids">id集合</param>
        /// <param name="trans">事务对象</param>
        /// <returns></returns>
        ResultModel GetParameterSetsBy(long[] ids, dynamic trans);
    }
}
