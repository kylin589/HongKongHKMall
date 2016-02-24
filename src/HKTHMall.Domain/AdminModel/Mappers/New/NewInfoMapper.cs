using AutoMapper;
using HKTHMall.Domain.AdminModel.Models.New;
using HKTHMall.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.AdminModel.Mappers.New
{
    public class NewInfoMapper
    {
        public void Execute()
        {
            Mapper.CreateMap<NewInfo, NewInfoModel>();

            Mapper.CreateMap<NewInfoModel, NewInfo>();
        }

        public int Order
        {
            get { return 0; }
        }
    }

    public static class NewInfoExtens
    {
        public static T ToModel<T>(this NewInfo entity) where T : class
        {
            return Mapper.Map<NewInfo, T>(entity);
        }


        public static IList<T> ToModel<T>(this ICollection<NewInfo> entityes) where T : class
        {
            return Mapper.Map<ICollection<NewInfo>, IList<T>>(entityes);
        }


        public static NewInfo ToEntity(this NewInfoModel model)
        {
            return Mapper.Map<NewInfo>(model);
        }
    }
}
