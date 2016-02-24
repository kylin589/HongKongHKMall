using AutoMapper;
using BrCms.Core.Infrastructure;
using HKTHMall.Domain.Entities;
using HKTHMall.Domain.Models.AC;
using HKTHMall.Domain.Models.User;

namespace HKTHMall.Domain.Mappers
{
    public class AC_UserMapper : IStartupTask
    {
        public void Execute()
        {
            Mapper.CreateMap<AC_User, AC_UserModel>()
                ;

            Mapper.CreateMap<AC_UserModel, AC_User>()
                ;

        //    Mapper.CreateMap<AC_Role, AC_RoleModel>()
        //       ;

        //    Mapper.CreateMap<AC_RoleModel, AC_Role>()
        //       ;

        //    Mapper.CreateMap<AC_Department, AC_DepartmentModel>()
        //       ;

        //    Mapper.CreateMap<AC_DepartmentModel, AC_Department>()
        //        ;
        }

        public int Order
        {
            get { return 0; }
        }
    }

    public static class AC_User_Extens
    {
        public static T ToModel<T>(this AC_User entity) where T : class
        {
            return Mapper.Map<AC_User, T>(entity);
        }

        public static AC_User ToEntity(this AC_UserModel model)
        {
            return Mapper.Map<AC_User>(model);
        }
    }
}
