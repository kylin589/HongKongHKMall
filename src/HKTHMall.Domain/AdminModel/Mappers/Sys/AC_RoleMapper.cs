using AutoMapper;
using BrCms.Core.Infrastructure;
using HKTHMall.Domain.Entities;
using HKTHMall.Domain.Models.AC;
using HKTHMall.Domain.Models.User;

namespace HKTHMall.Domain.Mappers
{
    public class AC_RoleMapper : IStartupTask
    {
        public void Execute()
        {
            Mapper.CreateMap<AC_Role, AC_RoleModel>()
               ;

            Mapper.CreateMap<AC_RoleModel, AC_Role>()
                ;
        }

        public int Order
        {
            get { return 0; }
        }
    }
    public static class AC_Role_Extens
    {
        public static T ToModel<T>(this AC_Role entity) where T : class
        {
            return Mapper.Map<AC_Role, T>(entity);
        }

        public static AC_Role ToEntity(this AC_RoleModel model)
        {
            return Mapper.Map<AC_Role>(model);
        }
    }
}
