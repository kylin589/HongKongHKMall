using System.Collections.Generic;
using System.IO;
using Autofac.Extras.DynamicProxy2;
using BrCms.Core.Infrastructure;
using HKTHMall.Domain.AdminModel.Models.Localization;
using HKTHMall.Domain.Models;
using HKTHMall.Domain.Models;

namespace HKTHMall.Services.Localization
{
    /// <summary>
    ///     语言
    /// </summary>
    [Intercept(typeof(ServiceIInterceptor))]
    public interface ILanguageService : IDependency
    {
        /// <summary>
        ///     新增语言
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ResultModel Add(AddLanguageModel model);

        /// <summary>
        ///     更新语言
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ResultModel Update(UpdateLanguageModel model);

        /// <summary>
        ///     删除语言
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        ResultModel Delete(List<long> ids);

        /// <summary>
        ///     查询语言
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ResultModel Search(SearchLanguageModel model);

        /// <summary>
        ///     导入语言
        /// </summary>
        /// <param name="xlsxStream">excel流</param>
        /// <param name="createBy">操作人</param>
        /// <returns></returns>
        ResultModel ImportExcel(Stream xlsxStream, string createBy);

        /// <summary>
        ///     根据Ids导出excel
        /// </summary>
        /// <param name="ids">id集合</param>
        /// <param name="isAll">是否导出全部</param>
        /// <returns></returns>
        ResultModel Export(List<long> ids, bool isAll = false);

        /// <summary>
        /// 根据条件导出数据
        /// </summary>
        /// <param name="model">条件模型</param>
        /// <returns></returns>
        ResultModel Export(SearchLanguageModel model);
    }
}