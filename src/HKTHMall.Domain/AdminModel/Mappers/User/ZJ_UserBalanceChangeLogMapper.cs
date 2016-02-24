using AutoMapper;
using BrCms.Core.Infrastructure;
using HKTHMall.Domain.AdminModel.Models.User;
using HKTHMall.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.AdminModel.Mappers.User
{
    public class ZJ_UserBalanceChangeLogMapper : IStartupTask
    {
        public void Execute()
        {
            Mapper.CreateMap<ZJ_UserBalanceChangeLog, ZJ_UserBalanceChangeLogModel>();

            Mapper.CreateMap<ZJ_UserBalanceChangeLogModel, ZJ_UserBalanceChangeLog>();
        }

        public int Order
        {
            get { return 0; }
        }
    }

    public static class ZJ_UserBalanceChangeLogExtens
    {
        public static T ToModel<T>(this ZJ_UserBalanceChangeLog entity) where T : class
        {
            return Mapper.Map<ZJ_UserBalanceChangeLog, T>(entity);
        }


        public static IList<T> ToModel<T>(this ICollection<ZJ_UserBalanceChangeLog> entityes) where T : class
        {
            return Mapper.Map<ICollection<ZJ_UserBalanceChangeLog>, IList<T>>(entityes);
        }


        public static ZJ_UserBalanceChangeLog ToEntity(this ZJ_UserBalanceChangeLogModel model)
        {
            return Mapper.Map<ZJ_UserBalanceChangeLog>(model);
        }
    }
}
