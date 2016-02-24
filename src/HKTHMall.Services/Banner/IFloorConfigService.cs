using BrCms.Core.Infrastructure;
using HKTHMall.Domain.AdminModel.Models.banner;
using HKTHMall.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Services.Banner
{
    public interface IFloorConfigService : IDependency
    {
        /// <summary>
        /// 获取楼层配置表
        /// </summary>
        /// <param name="model">model</param>
        /// <returns>楼层配置表</returns>
        ResultModel GetFloorConfigList(SearchFloorConfigModel model);

        /// <summary>
        /// 添加楼层配置表
        /// </summary>
        /// <param name="model">楼层配置表model</param>
        /// <returns>是否成功</returns>
        ResultModel AddFloorConfig(FloorConfigModel model);

        /// <summary>
        /// 修改楼层配置表
        /// </summary>
        /// <param name="model">楼层配置表model</param>
        /// <returns>是否成功</returns>
        ResultModel UpdateFloorConfig(FloorConfigModel model);

        /// <summary>
        /// 获取单个实体数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ResultModel GetModelById(int id);

    }
}
