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
    public class YH_UserBankAccountMapper : IStartupTask
    {
        public void Execute()
        {
            Mapper.CreateMap<YH_UserBankAccount, YH_UserBankAccountModel>();

            Mapper.CreateMap<YH_UserBankAccountModel, YH_UserBankAccount>();
        }

        public int Order
        {
            get { return 0; }
        }

    }

    public static class YH_UserBankAccountExtens
    {
        public static T ToModel<T>(this YH_UserBankAccount entity) where T : class
        {
            return Mapper.Map<YH_UserBankAccount, T>(entity);
        }


        public static IList<T> ToModel<T>(this ICollection<YH_UserBankAccount> entityes) where T : class
        {
            return Mapper.Map<ICollection<YH_UserBankAccount>, IList<T>>(entityes);
        }


        public static YH_UserBankAccount ToEntity(this YH_UserBankAccountModel model)
        {
            return Mapper.Map<YH_UserBankAccount>(model);
        }
    }
}
