using AutoMapper;
using BrCms.Core.Infrastructure;
using HKTHMall.Domain.Entities;
using HKTHMall.Domain.Models.Categoreis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.Mappers.Converters
{
    public class SP_ProductCommentMappers : IStartupTask
    {
        public void Execute()
        {
            Mapper.CreateMap<SP_ProductComment, SP_ProductCommentModel>();

            Mapper.CreateMap<SP_ProductCommentModel, SP_ProductComment>();
        }

        public int Order
        {
            get { return 0; }
        }
    }
    public static class SP_ProductCommentExtens
    {
        public static T ToModel<T>(this SP_ProductComment entity) where T : class
        {
            return Mapper.Map<SP_ProductComment, T>(entity);
        }


        public static IList<T> ToModel<T>(this ICollection<SP_ProductComment> entityes) where T : class
        {
            return Mapper.Map<ICollection<SP_ProductComment>, IList<T>>(entityes);
        }


        public static SP_ProductComment ToEntity(this SP_ProductCommentModel model)
        {
            return Mapper.Map<SP_ProductComment>(model);
        }
    }
}
