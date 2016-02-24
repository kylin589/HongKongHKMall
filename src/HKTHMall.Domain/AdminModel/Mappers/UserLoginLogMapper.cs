

using AutoMapper;
using BrCms.Core.Infrastructure;
using HKTHMall.Domain.Entities;
using HKTHMall.Domain.Models.LoginLog;
using System.Collections.Generic;
namespace HKTHMall.Domain.Mappers
{
    public class UserLoginLogMapper : IStartupTask
    {
        public void Execute()
        {
            Mapper.CreateMap<UserLoginLog, UserLoginLogModel>();
            
            Mapper.CreateMap<UserLoginLogModel, UserLoginLog>();
        }

        public int Order
        {
            get { return 0; }
        }

       
    }

    public static class UserLoginLogExtens
    {
        public static T ToModel<T>(this UserLoginLog entity) where T : class
        {
            return Mapper.Map<UserLoginLog, T>(entity);
        }

         
        public static IList<T> ToModel<T>(this ICollection<UserLoginLog> entityes) where T : class
        {
            return Mapper.Map<ICollection<UserLoginLog>, IList<T>>(entityes);
        }

       
        public static UserLoginLog ToEntity(this UserLoginLogModel model)
        {
            return Mapper.Map<UserLoginLog>(model);
        }
    }

}
