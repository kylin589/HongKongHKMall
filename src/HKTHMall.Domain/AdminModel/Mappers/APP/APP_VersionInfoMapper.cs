using AutoMapper;
using BrCms.Core.Infrastructure;
using HKTHMall.Domain.AdminModel.Models.APP;
using HKTHMall.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.AdminModel.Mappers.APP
{
    public class APP_VersionInfoMapper : IStartupTask
    {
        public void Execute()
        {
            Mapper.CreateMap<APP_VersionInfo, APP_VersionInfoModel>();

            Mapper.CreateMap<APP_VersionInfoModel, APP_VersionInfo>();
        }

        public int Order
        {
            get { return 0; }
        }
    }

    public static class APP_VersionInfoExtens
    {
        public static T ToModel<T>(this APP_VersionInfo entity) where T : class
        {
            return Mapper.Map<APP_VersionInfo, T>(entity);
        }


        public static IList<T> ToModel<T>(this ICollection<APP_VersionInfo> entityes) where T : class
        {
            return Mapper.Map<ICollection<APP_VersionInfo>, IList<T>>(entityes);
        }


        public static APP_VersionInfo ToEntity(this APP_VersionInfoModel model)
        {
            return Mapper.Map<APP_VersionInfo>(model);
        }
    }
}
