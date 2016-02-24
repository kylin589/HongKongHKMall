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
    public interface IYH_UserUpdateInfoService : IDependency
    {
        ResultModel Add(YH_UserUpdateInfoModel model);
    }
}
