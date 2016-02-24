using AutoMapper;
using BrCms.Core.Infrastructure;
using HKTHMall.Domain.Entities;
using HKTHMall.Domain.Models.AC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.Mappers.AC
{
    public class AC_FunctionMapper : IStartupTask
    {
        public void Execute()
        {
            Mapper.CreateMap<AC_Function, AC_FunctionModel>()
               ;
            Mapper.CreateMap<AC_FunctionModel, AC_Function>()
                ;
        }

        public int Order
        {
            get { return 0; }
        }
    }
    public static class AC_FunctionExtens
    {
        public static T ToModel<T>(this AC_Function entity) where T : class
        {
            return Mapper.Map<AC_Function, T>(entity);
        }

        public static IList<T> ToModel<T>(this ICollection<AC_Function> entities) where T : class
        {
            return Mapper.Map<ICollection<AC_Function>, IList<T>>(entities);
        }

        public static AC_Function ToEntity(this AC_FunctionModel model)
        {
            return Mapper.Map<AC_Function>(model);
        }
    }
}
