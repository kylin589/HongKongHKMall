using BrCms.Core.Infrastructure;
using HKTHMall.Domain.Entities;
using HKTHMall.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Services.Users
{
    public interface IYh_groupmarkService : IDependency
    {

        ResultModel GetGroupMarkByUserId(long userId);

        ResultModel AddGroupMark(YH_GroupMark model);

        ResultModel DeleteGroupMark(long userId,int groupType);


    }
}
