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
    public class ZJ_RechargeOrderMapper : IStartupTask
    {
        public void Execute()
        {
            Mapper.CreateMap<ZJ_RechargeOrder, ZJ_RechargeOrderModel>();

            Mapper.CreateMap<ZJ_RechargeOrderModel, ZJ_RechargeOrder>();
        }

        public int Order
        {
            get { return 0; }
        }
    }

    public static class ZJ_RechargeOrderExtens
    {
        public static T ToModel<T>(this ZJ_RechargeOrder entity) where T : class
        {
            return Mapper.Map<ZJ_RechargeOrder, T>(entity);
        }


        public static IList<T> ToModel<T>(this ICollection<ZJ_RechargeOrder> entityes) where T : class
        {
            return Mapper.Map<ICollection<ZJ_RechargeOrder>, IList<T>>(entityes);
        }


        public static ZJ_RechargeOrder ToEntity(this ZJ_RechargeOrderModel model)
        {
            return Mapper.Map<ZJ_RechargeOrder>(model);
        }
    }
}
