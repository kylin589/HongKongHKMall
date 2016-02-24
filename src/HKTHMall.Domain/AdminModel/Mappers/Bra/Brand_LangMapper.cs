using AutoMapper;
using BrCms.Core.Infrastructure;
using HKTHMall.Domain.Entities;
using HKTHMall.Domain.Models.Bra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.Mappers.Bra
{
    public class Brand_LangMapper : IStartupTask
    {
        public void Execute()
        {
            Mapper.CreateMap<Brand_Lang, Brand_LangModel>()
               ;
            Mapper.CreateMap<Brand_LangModel, Brand_LangModel>()
                ;
        }

        public int Order
        {
            get { return 0; }
        }
    }

    public static class Brand_LangExtens
    {
        public static T ToModel<T>(this Brand_Lang entity) where T : class
        {
            return Mapper.Map<Brand_Lang, T>(entity);
        }

        public static IList<T> ToModel<T>(this ICollection<Brand_Lang> entities) where T : class
        {
            return Mapper.Map<ICollection<Brand_Lang>, IList<T>>(entities);
        }

        public static Brand_Lang ToEntity(this Brand_LangModel model)
        {
            return Mapper.Map<Brand_Lang>(model);
        }
    }
}
