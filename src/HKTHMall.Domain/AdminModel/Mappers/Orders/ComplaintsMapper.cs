using AutoMapper;
using BrCms.Core.Infrastructure;
using HKTHMall.Domain.Entities;
using HKTHMall.Domain.Models.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.Mappers.Orders
{
    public class ComplaintsMapper : IStartupTask
    {
         public void Execute()
        {
            Mapper.CreateMap<Complaints, ComplaintsModel>();
            Mapper.CreateMap<ComplaintsModel, Complaints>();
        }

        public int Order
        {
            get { return 0; }
        }


    }
    public static class Complaints_Extens
    {
        public static T ToModel<T>(this Complaints entity) where T : class
        {
            return Mapper.Map<Complaints, T>(entity);
        }

        public static Complaints ToEntity(this ComplaintsModel model)
        {
            return Mapper.Map<Complaints>(model);
        }

        public static IList<T> ToModel<T>(this ICollection<Complaints> entities) where T : class
        {
            return Mapper.Map<ICollection<Complaints>, IList<T>>(entities);
        }
    }
}
