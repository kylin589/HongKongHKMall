using BrCms.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKTHMall.Domain.Entities;
using HKTHMall.Domain.Models.Bra;
using AutoMapper;

namespace HKTHMall.Domain.Mappers.Bra
{
    public class BrandMapper : IStartupTask
    {
        public void Execute()
        {
            Mapper.CreateMap<Brand, BrandModel>()
               ;
            Mapper.CreateMap<BrandModel, BrandModel>()
                ;
        }

        public int Order
        {
            get { return 0; }
        }
    }

    public static class BrandExtens
    {
        public static T ToModel<T>(this Brand entity) where T : class
        {
            return Mapper.Map<Brand, T>(entity);
        }

        public static IList<T> ToList<T>(this ICollection<Brand> entities) where T : class
        {
            return Mapper.Map<ICollection<Brand>, IList<T>>(entities);
        }

        public static Brand ToEntity(this BrandModel model)
        {
            return Mapper.Map<Brand>(model);
        }
    }
}
