using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using BrCms.Core.Infrastructure;
using HKTHMall.Domain.AdminModel.Models.Categoreis;
using HKTHMall.Domain.Entities;
using HKTHMall.Domain.Models.Categoreis;

namespace HKTHMall.Domain.Mappers
{
    public class CategoryMapper : IStartupTask
    {
        public void Execute()
        {
            Mapper.CreateMap<Category, CategoryModel>()
                ;
            Mapper.CreateMap<CategoryModel, Category>()
                ;
            //Mapper.CreateMap<Category, CategoryTreeModel>()
            //    .ForMember(dest => dest.text,
            //        mo => mo.MapFrom(dto => dto.Category_Lang.FirstOrDefault(m => m.CategoryId == 1).Category))
            //    ;
        }

        public int Order
        {
            get { return 0; }
        }
    }

    public static class CategoryExtens
    {
        public static T ToModel<T>(this Category entity) where T : class
        {
            return Mapper.Map<Category, T>(entity);
        }

        public static IList<T> ToModel<T>(this ICollection<Category> entities) where T : class
        {
            return Mapper.Map<ICollection<Category>, IList<T>>(entities);
        }

        public static Category ToEntity(this AddCategoryModel model)
        {
            return Mapper.Map<Category>(model);
        }
    }
}
