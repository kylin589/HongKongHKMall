using BrCms.Core.Infrastructure;
using HKTHMall.Domain.AdminModel.Models;
using HKTHMall.Domain.AdminModel.Models.YHUser;
using HKTHMall.Domain.Entities;
using HKTHMall.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Services.YHUser
{
    public interface IYH_AgentService : IDependency
    {
        /// <summary>
        /// 根据用户ID查询代理商信息
        /// zhoub 20150924
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        ResultModel GetYH_AgentByUserId(long userId);

        /// <summary>
        /// 代理商添加
        /// zhoub 20150924
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ResultModel AddYH_Agent(YH_AgentModel model);

        /// <summary>
        /// 代理商更新
        /// zhoub 20150924
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ResultModel EditYH_Agent(YH_AgentModel model);

        /// <summary>
        /// 代理商列表查询
        /// wuyf 20150924
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ResultModel GetPagingYH_Agent(SearchYH_AgentModel model);

        /// <summary>
        /// 代理商删除
        /// wuyf 20150924 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ResultModel DeleterYH_Agent(YH_AgentModel model);
    }
}
