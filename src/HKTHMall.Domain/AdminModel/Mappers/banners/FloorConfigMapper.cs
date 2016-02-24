using AutoMapper;
using BrCms.Core.Infrastructure;
using HKTHMall.Domain.AdminModel.Models.banner;
using HKTHMall.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.AdminModel.Mappers.banners
{
    public class FloorConfigMapper : IStartupTask
    {
        public void Execute()
        {
            Mapper.CreateMap<FloorConfig, FloorConfigModel>()
               ;
            Mapper.CreateMap<FloorConfigModel, FloorConfig>()
                ;
        }

        public int Order
        {
            get { return 0; }
        }
    }

    public static class FloorConfigExtens
    {
        public static T ToModel<T>(this FloorConfig entity) where T : class
        {
            return Mapper.Map<FloorConfig, T>(entity);
        }

        public static IList<T> ToModel<T>(this ICollection<FloorConfig> entities) where T : class
        {
            return Mapper.Map<ICollection<FloorConfig>, IList<T>>(entities);
        }

        public static FloorConfig ToEntity(this FloorConfigModel model)
        {
            return Mapper.Map<FloorConfig>(model);
        }
    }
}
