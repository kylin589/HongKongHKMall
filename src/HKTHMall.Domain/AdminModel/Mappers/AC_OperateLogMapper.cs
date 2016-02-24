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
    public class AC_OperateLogMapper : IStartupTask
    {
        public void Execute()
        {
            Mapper.CreateMap<AC_OperateLog, AC_OperateLogModel>();

            Mapper.CreateMap<AC_OperateLogModel, AC_OperateLog>();
        }

        public int Order
        {
            get { return 0; }
        }

    }

    public static class AC_OperateLogExtens
    {
        public static T ToModel<T>(this AC_OperateLog entity) where T : class
        {
            return Mapper.Map<AC_OperateLog, T>(entity);
        }


        public static IList<T> ToModel<T>(this ICollection<AC_OperateLog> entityes) where T : class
        {
            return Mapper.Map<ICollection<AC_OperateLog>, IList<T>>(entityes);
        }


        public static AC_OperateLog ToEntity(this AC_OperateLogModel model)
        {
            return Mapper.Map<AC_OperateLog>(model);
        }
    }
}
