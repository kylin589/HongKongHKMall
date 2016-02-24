using System.Collections.Generic;
using AutoMapper;
using BrCms.Core.Infrastructure;
using HKTHMall.Domain.AdminModel.Models.SKU;
using HKTHMall.Domain.Entities;
using HKTHMall.Domain.Models.SKU;

namespace HKTHMall.Domain.Mappers.SKU
{

    public class SKU_AttributeValuesMapper : IStartupTask
    {

        public void Execute()
        {
            Mapper.CreateMap<SKU_AttributeValues, SKU_AttributeValuesModel>()
               ;
            Mapper.CreateMap<SKU_AttributeValuesModel, SKU_AttributeValues>()
                ;
        }

        public int Order
        {
            get { return 0; }
        }
    }

    public static class SSKU_AttributeValuesExtens
    {
        public static T ToModel<T>(this SKU_AttributeValues entity) where T : class
        {
            return Mapper.Map<SKU_AttributeValues, T>(entity);
        }

        public static IList<T> ToModel<T>(this ICollection<SKU_AttributeValues> entities) where T : class
        {
            return Mapper.Map<ICollection<SKU_AttributeValues>, IList<T>>(entities);
        }

        public static SKU_AttributeValues ToEntity(this SKU_AttributeValuesModel model)
        {
            return Mapper.Map<SKU_AttributeValues>(model);
        }
    }
   
}
