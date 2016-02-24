using AutoMapper;
using BrCms.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKTHMall.Domain.Models;
using HKTHMall.Domain.Entities;
using HKTHMall.Domain.AdminModel.Models.Keywork;


namespace HKTHMall.Domain.Mappers.Keywork
{
    public class FloorKeywordMapper : IStartupTask
    {

        public void Execute()
        {
            Mapper.CreateMap<FloorKeyword, FloorKeywordModel>()
               ;
            Mapper.CreateMap<FloorKeywordModel, FloorKeyword>()
                ;
        }

        public int Order
        {
            get { return 0; }
        }
    }

    public static class FloorKeyword_Extens
    {
        public static T ToModel<T>(this FloorKeyword entity) where T : class
        {
            return Mapper.Map<FloorKeyword, T>(entity);
        }

        public static IList<T> ToList<T>(this ICollection<FloorKeyword> entities) where T : class
        {
            return Mapper.Map<ICollection<FloorKeyword>, IList<T>>(entities);
        }

        public static FloorKeyword ToEntity(this FloorKeywordModel model)
        {
            return Mapper.Map<FloorKeyword>(model);
        }
    }
}
