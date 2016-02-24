using AutoMapper;
using BrCms.Core.Infrastructure;
using HKTHMall.Domain.Entities;
using HKTHMall.Domain.Models.banner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.Mappers.banners
{
    public class bannerMapper : IStartupTask
    {
        public void Execute()
        {
            Mapper.CreateMap<banner, bannerModel>()
               ;
            Mapper.CreateMap<bannerModel, banner>()
                ;
        }

        public int Order
        {
            get { return 0; }
        }
    }

    public static class bannerExtens
    {
        public static T ToModel<T>(this banner entity) where T : class
        {
            return Mapper.Map<banner, T>(entity);
        }

        public static IList<T> ToModel<T>(this ICollection<banner> entities) where T : class
        {
            return Mapper.Map<ICollection<banner>, IList<T>>(entities);
        }

        public static banner ToEntity(this bannerModel model)
        {
            return Mapper.Map<banner>(model);
        }
    }
}
