using System.Collections.Generic;
using AutoMapper;
using BrCms.Core.Infrastructure;
using HKTHMall.Domain.AdminModel.Models.AC;
using HKTHMall.Domain.Entities;
using HKTHMall.Domain.Models.AC;

namespace HKTHMall.Domain.Mappers.AC
{
    public class AC_DepartmentMapper : IStartupTask
    {

        public void Execute()
        {
            Mapper.CreateMap<AC_Department, AC_DepartmentModel>()
               ;
            Mapper.CreateMap<AC_DepartmentModel, AC_Department>()
                ;
        }

        public int Order
        {
            get { return 0; }
        }
    }

    public static class AC_DepartmentExtens
    {
        public static T ToModel<T>(this AC_Department entity) where T : class
        {
            return Mapper.Map<AC_Department, T>(entity);
        }

        public static IList<T> ToModel<T>(this ICollection<AC_Department> entities) where T : class
        {
            return Mapper.Map<ICollection<AC_Department>, IList<T>>(entities);
        }

        public static AC_Department ToEntity(this AC_DepartmentModel model)
        {
            return Mapper.Map<AC_Department>(model);
        }
    }
}
