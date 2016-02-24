using BrCms.Core.Infrastructure;
using HKTHMall.Domain.Models;
using HKTHMall.Domain.WebModel.Models.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Services.WebLogin
{
    public interface IZJ_UserBalanceService : IDependency
    {
        /// <summary>
        /// 资金表插入
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ResultModel AddBalance(ZJ_UserBalanceModel model);
    }
}
