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
    public class SalesRuleMapper : IStartupTask
    {
        public void Execute()
        {
            Mapper.CreateMap<SalesRule, SalesRuleModel>();
            Mapper.CreateMap<SalesRuleModel, SalesRule>();
        }

        public int Order
        {
            get { return 0; }
        }


    }
    public static class SalesRule_Extens
    {
        public static T ToModel<T>(this SalesRule entity) where T : class
        {
            return Mapper.Map<SalesRule, T>(entity);
        }

        public static SalesRule ToEntity(this SalesRuleModel model)
        {
            return Mapper.Map<SalesRule>(model);
        }

        public static IList<T> ToModel<T>(this ICollection<SalesRule> entities) where T : class
        {
            return Mapper.Map<ICollection<SalesRule>, IList<T>>(entities);
        }
    }
}
