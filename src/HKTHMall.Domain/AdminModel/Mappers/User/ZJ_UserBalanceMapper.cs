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
    public class ZJ_UserBalanceMapper : IStartupTask
    {
        public void Execute()
        {
            Mapper.CreateMap<ZJ_UserBalance, ZJ_UserBalanceModel>();

            Mapper.CreateMap<ZJ_UserBalanceModel, ZJ_UserBalance>();
        }

        public int Order
        {
            get { return 0; }
        }

    }

    public static class ZJ_UserBalanceExtens
    {
        public static T ToModel<T>(this ZJ_UserBalance entity) where T : class
        {
            return Mapper.Map<ZJ_UserBalance, T>(entity);
        }


        public static IList<T> ToModel<T>(this ICollection<ZJ_UserBalance> entityes) where T : class
        {
            return Mapper.Map<ICollection<ZJ_UserBalance>, IList<T>>(entityes);
        }


        public static ZJ_UserBalance ToEntity(this ZJ_UserBalanceModel model)
        {
            return Mapper.Map<ZJ_UserBalance>(model);
        }
    }
}
