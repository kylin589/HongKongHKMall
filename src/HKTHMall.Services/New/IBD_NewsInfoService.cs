using BrCms.Core.Infrastructure;
using HKTHMall.Domain.AdminModel.Models.New;
using HKTHMall.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Services.New
{
    public interface IBD_NewsInfoService : IDependency
    {
        /// <summary>
        /// 通过Id查询新闻信息
        /// </summary>
        /// <param name="id">主键Id</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-8-10</remarks>
        ResultModel GetBD_NewsInfoById(int id);

        /// <summary>
        /// 新闻信息分布查询
        /// </summary>
        /// <param name="model">输入查询参数对象</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-8-10</remarks>
        ResultModel Select(SearchBD_NewsInfoModel model);

        /// <summary>
        /// 添加新闻信息
        /// </summary>
        /// <param name="model">新闻信息</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-8-10</remarks>
        ResultModel Add(BD_NewsInfoModel model);

        /// <summary>
        /// 通过Id删除新闻信息
        /// </summary>
        /// <param name="id">新闻信息id</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-8-10</remarks>
        ResultModel Delete(int id);

        /// <summary>
        /// 更新新闻信息
        /// </summary>
        /// <param name="model">新闻信息对象</param>
        /// <returns>返回true时,表示更新成功；反之,表示更新失败</returns>
        /// <remarks>added by jimmy,2015-8-10</remarks>
        ResultModel Update(BD_NewsInfoModel model);

        /// <summary>
        /// 审核信息
        /// </summary>
        /// <param name="model">新闻信息对象</param>
        /// <returns>返回true时,表示更新成功；反之,表示更新失败</returns>
        /// <remarks>added by jimmy,2015-8-11</remarks>
        ResultModel UpdateState(BD_NewsInfoModel model);

        /// <summary>
        /// 通过语言Id查询新闻类型
        /// </summary>
        /// <param name="languageID">语言ID</param>
        /// <returns></returns>
        /// <remarks>added by jimmmy,2015-8-27</remarks>
        ResultModel GetBD_NewsTypelang(int languageID);
    }
}
