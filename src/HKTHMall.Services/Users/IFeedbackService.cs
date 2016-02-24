using BrCms.Core.Infrastructure;
using HKTHMall.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HKTHMall.Domain.AdminModel.Models.User;
using System.Threading.Tasks;
using HKTHMall.Domain.WebModel.Models.Users;

namespace HKTHMall.Services.Users
{
    /// <summary>
    /// 用户反馈接口
    /// <remarks>added by jimmy,2015-8-12</remarks>
    /// </summary>
    public interface IFeedbackService: IDependency
    {
        /// <summary>
        /// 用户反馈分页查询
        /// </summary>
        /// <param name="model">输入查询参数对象</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-8-12</remarks>
        ResultModel Select(SearchFeedbackModel model);

        
        /// <summary>
        /// 添加一条反馈消息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ResultModel AddFeedback(FeedbackView model);

        ResultModel SelectFeedbackType(int langId);
    }
}

