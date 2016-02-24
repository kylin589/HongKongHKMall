using BrCms.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKTHMall.Domain.AdminModel.Models.Categoreis;
using HKTHMall.Domain.Entities;
using HKTHMall.Domain.Models.Categoreis;
using AutoMapper;


namespace HKTHMall.Domain.AdminModel.Mappers.FloorCategoryMapper
{
    public class FloorFloorCategoryMapper : IStartupTask
    {
        public void Execute()
        {
            Mapper.CreateMap<FloorCategory, FloorCategoryModel>()
                ;
            Mapper.CreateMap<FloorCategoryModel, FloorCategory>()
                ;
        }

        public int Order
        {
            get { return 0; }
        }
    }

    public static class FloorCategoryExtens
    {
        public static T ToModel<T>(this FloorCategory entity) where T : class
        {
            return Mapper.Map<FloorCategory, T>(entity);
        }

        public static IList<T> ToModel<T>(this ICollection<FloorCategory> entities) where T : class
        {
            return Mapper.Map<ICollection<FloorCategory>, IList<T>>(entities);
        }

        public static FloorCategory ToEntity(this FloorCategoryModel model)
        {
            return Mapper.Map<FloorCategory>(model);
        }
    }
}
