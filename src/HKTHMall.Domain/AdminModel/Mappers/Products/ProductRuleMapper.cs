using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BrCms.Core.Infrastructure;
using HKTHMall.Domain.AdminModel.Models.Products;
using HKTHMall.Domain.Entities;


namespace HKTHMall.Domain.AdminModel.Mappers.Products
{
    public class ProductRuleMapper : IStartupTask
    {
        public void Execute()
        {
            Mapper.CreateMap<ProductRule, ProductRuleModel>();
            Mapper.CreateMap<ProductRuleModel, ProductRule>();
        }

        public int Order
        {
            get { return 0; }
        }


    }
    public static class ProductRule_Extens
    {
        public static T ToModel<T>(this ProductRule entity) where T : class
        {
            return Mapper.Map<ProductRule, T>(entity);
        }

        public static ProductRule ToEntity(this ProductRuleModel model)
        {
            return Mapper.Map<ProductRule>(model);
        }

        public static IList<T> ToModel<T>(this ICollection<ProductRule> entities) where T : class
        {
            return Mapper.Map<ICollection<ProductRule>, IList<T>>(entities);
        }
    }
}
