using System.Collections.Generic;
using AutoMapper;
using BrCms.Core.Infrastructure;
using HKTHMall.Domain.AdminModel.Models.SKU;
using HKTHMall.Domain.Entities;
using HKTHMall.Domain.Models.SKU;

namespace HKTHMall.Domain.Mappers.SKU
{

    public class SKU_AttributesMapper : IStartupTask
    {

        public void Execute()
        {
            Mapper.CreateMap<SKU_Attributes, SKU_AttributesModel>()
               ;
            Mapper.CreateMap<SKU_AttributesModel, SKU_Attributes>()
                ;
        }

        public int Order
        {
            get { return 0; }
        }
    }

    public static class SKU_AttributesExtens
    {
        public static T ToModel<T>(this SKU_Attributes entity) where T : class
        {
            return Mapper.Map<SKU_Attributes, T>(entity);
        }

        public static IList<T> ToModel<T>(this ICollection<SKU_Attributes> entities) where T : class
        {
            return Mapper.Map<ICollection<SKU_Attributes>, IList<T>>(entities);
        }

        public static SKU_Attributes ToEntity(this SKU_AttributesModel model)
        {
            return Mapper.Map<SKU_Attributes>(model);
        }
    }
   
}
