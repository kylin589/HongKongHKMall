using AutoMapper;
using BrCms.Core.Infrastructure;
using HKTHMall.Domain.Entities;
using HKTHMall.Domain.Models.LoginLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.Mappers
{
    public class YH_UserLoginLogMapper : IStartupTask
    {
        public void Execute()
        {
            Mapper.CreateMap<YH_UserLoginLog, YH_UserLoginLogModel>()
                ;
            Mapper.CreateMap<YH_UserLoginLogModel, YH_UserLoginLog>()
                ;
        }

        public int Order
        {
            get { return 0; }
        }
    }

    public static class YH_UserLoginLogExtens
    {
        public static T ToModel<T>(this YH_UserLoginLog entity) where T : class
        {
            return Mapper.Map<YH_UserLoginLog, T>(entity);
        }

        public static IList<T> ToModel<T>(this ICollection<YH_UserLoginLog> entities) where T : class
        {
            return Mapper.Map<ICollection<YH_UserLoginLog>, IList<T>>(entities);
        }

        public static YH_UserLoginLog ToEntity(this YH_UserLoginLogModel model)
        {
            return Mapper.Map<YH_UserLoginLog>(model);
        }
    }
}
