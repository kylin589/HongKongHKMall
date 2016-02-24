using BrCms.Core.Infrastructure;
using HKTHMall.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKTHMall.Domain.AdminModel.Models.User;
using BrCms.Framework.Collections;


namespace HKTHMall.Services.Users
{
    /// <summary>
    /// 留言信息接口
    /// <remarks>added by jimmy,2015-7-27</remarks>
    /// </summary>
    public interface IMessageService : IDependency
    {
        /// <summary>
        /// 留言信息分布查询
        /// </summary>
        /// <param name="model">输入查询参数对象</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-27</remarks>
        ResultModel Select(SearchMessageModel model);

        /// <summary>
        /// 添加用户留言
        /// zhoub 20150825
        /// </summary>
        ResultModel AddMessage(MessageModel model);
    }
}
