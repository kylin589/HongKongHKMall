using System.Collections.Generic;
using AutoMapper;
using HKTHMall.Domain.AdminModel.Models.SKU;
using HKTHMall.Domain.Entities;

namespace HKTHMall.Domain.Models.SKU
{
    public class SKU_SKUItemsMapper
    {

        public void Execute()
        {
            Mapper.CreateMap<SKU_SKUItems, SKU_SKUItemsModel>()
               ;
            Mapper.CreateMap<SKU_SKUItemsModel, SKU_SKUItems>()
                ;
        }

        public int Order
        {
            get { return 0; }
        }
    }

    public static class SKU_SKUItemsExtens
    {
        public static T ToModel<T>(this SKU_SKUItems entity) where T : class
        {
            return Mapper.Map<SKU_SKUItems, T>(entity);
        }

        public static IList<T> ToModel<T>(this ICollection<SKU_SKUItems> entities) where T : class
        {
            return Mapper.Map<ICollection<SKU_SKUItems>, IList<T>>(entities);
        }

        public static SKU_SKUItems ToEntity(this SKU_SKUItemsModel model)
        {
            return Mapper.Map<SKU_SKUItems>(model);
        }
    }
}
