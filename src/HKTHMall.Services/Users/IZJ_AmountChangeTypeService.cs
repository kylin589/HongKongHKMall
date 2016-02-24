using BrCms.Core.Infrastructure;
using HKTHMall.Domain.AdminModel.Models.User;
using HKTHMall.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Services.Users
{
    public interface IZJ_AmountChangeTypeService : IDependency
    {
        /// <summary>
        /// 获取账户异动类型
        /// </summary>
        /// <param name="model">model</param>
        /// <returns></returns>
        ResultModel GetZJ_AmountChangeTypeList(SearchZJ_AmountChangeTypeModel model);

        /// <summary>
        /// 添加账户异动类型
        /// </summary>
        /// <param name="model">账户异动类型</param>
        /// <returns>是否成功</returns>
        ResultModel AddZJ_AmountChangeType(ZJ_AmountChangeTypeModel model);

        /// <summary>
        /// 更新账户异动类型
        /// </summary>
        /// <param name="model">账户异动类型</param>
        /// <returns>是否修改成功</returns>
        ResultModel UpdateZJ_AmountChangeType(ZJ_AmountChangeTypeModel model);
    }
}
