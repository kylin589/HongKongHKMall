using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrCms.Core.Infrastructure;
using System.Threading.Tasks;
using HKTHMall.Domain.Entities;
using HKTHMall.Domain.Models.Sys;
using AutoMapper;


namespace HKTHMall.Domain.Mappers
{
    public class ParameterSetMapper : IStartupTask
    {
        public void Execute()
        {
            Mapper.CreateMap<ParameterSet, ParameterSetModel>();
            Mapper.CreateMap<ParameterSetModel, ParameterSet>();
        }

        public int Order
        {
            get { return 0; }
        }


    }
    public static class ParameterSet_Extens
    {
        public static T ToModel<T>(this ParameterSet entity) where T : class
        {
            return Mapper.Map<ParameterSet, T>(entity);
        }

        public static ParameterSet ToEntity(this ParameterSetModel model)
        {
            return Mapper.Map<ParameterSet>(model);
        }

        public static IList<T> ToModel<T>(this ICollection<ParameterSet> entities) where T : class
        {
            return Mapper.Map<ICollection<ParameterSet>, IList<T>>(entities);
        }
    }
}
