using AutoMapper;
using BrCms.Core.Infrastructure;
using HKTHMall.Domain.AdminModel.Models.Products;
using HKTHMall.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.AdminModel.Mappers.Products
{
    public class SalesProductMapper : IStartupTask
    {
        public void Execute()
        {
            Mapper.CreateMap<SalesProduct, SalesProductModel>();
            Mapper.CreateMap<SalesProductModel, SalesProduct>();
        }

        public int Order
        {
            get { return 0; }
        }


    }
    public static class SalesProduct_Extens
    {
        public static T ToModel<T>(this SalesProduct entity) where T : class
        {
            return Mapper.Map<SalesProduct, T>(entity);
        }

        public static SalesProduct ToEntity(this SalesProductModel model)
        {
            return Mapper.Map<SalesProduct>(model);
        }

        public static IList<T> ToModel<T>(this ICollection<SalesProduct> entities) where T : class
        {
            return Mapper.Map<ICollection<SalesProduct>, IList<T>>(entities);
        }
    }
}
