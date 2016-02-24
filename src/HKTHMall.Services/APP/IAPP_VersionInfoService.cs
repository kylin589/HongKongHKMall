using BrCms.Core.Infrastructure;
using HKTHMall.Domain.AdminModel.Models.APP;
using HKTHMall.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Services.APP
{
    public interface IAPP_VersionInfoService : IDependency
    {
        /// <summary>
        /// 添加APP版本信息表
        /// </summary>
        /// <param name="model">用APP版本信息表模型</param>
        /// <returns>是否成功</returns>

        ResultModel AddAPP_VersionInfo(APP_VersionInfoModel model);

        /// <summary>
        /// 获取APP版本信息表
        /// </summary>
        /// <param name="model">model</param>
        /// <returns></returns>
        ResultModel GetAPP_VersionInfoList(SearchAPP_VersionInfoModel model);

        /// <summary>
        /// 更新APP_VersionInfo
        /// </summary>
        /// <param name="model">APP_VersionInfo模型</param>
        /// <returns>是否修改成功</returns>
        ResultModel UpdateAPP_VersionInfo(APP_VersionInfoModel model);

        /// <summary>
        /// 删除APP_VersionInfo
        /// </summary>
        /// <param name="model">APP_VersionInfo模型</param>
        /// <returns>是否删除成功</returns>
        ResultModel DeleteAPP_VersionInfo(APP_VersionInfoModel model);

    }
}
