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
    public class ZJ_AmountChangeTypeMapper : IStartupTask
    {
        public void Execute()
        {
            Mapper.CreateMap<ZJ_AmountChangeType, ZJ_AmountChangeTypeModel>();

            Mapper.CreateMap<ZJ_AmountChangeTypeModel, ZJ_AmountChangeType>();
        }

        public int Order
        {
            get { return 0; }
        }
    }

    public static class ZJ_AmountChangeTypeExtens
    {
        public static T ToModel<T>(this ZJ_AmountChangeType entity) where T : class
        {
            return Mapper.Map<ZJ_AmountChangeType, T>(entity);
        }


        public static IList<T> ToModel<T>(this ICollection<ZJ_AmountChangeType> entityes) where T : class
        {
            return Mapper.Map<ICollection<ZJ_AmountChangeType>, IList<T>>(entityes);
        }


        public static ZJ_AmountChangeType ToEntity(this ZJ_AmountChangeTypeModel model)
        {
            return Mapper.Map<ZJ_AmountChangeType>(model);
        }
    }
}
