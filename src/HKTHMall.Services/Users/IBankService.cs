using BrCms.Core.Infrastructure;
using HKTHMall.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Services.Users
{
    public interface IBankService : IDependency
    {
        /// <summary>
        /// 获取银行卡列表
        /// </summary>
        /// <returns></returns>
        ResultModel GetBankList();
    }
}
