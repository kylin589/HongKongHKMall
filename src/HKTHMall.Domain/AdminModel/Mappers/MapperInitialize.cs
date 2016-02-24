using System;
using AutoMapper;
using BrCms.Core.Infrastructure;
using HKTHMall.Domain.Mappers.Converters;

namespace HKTHMall.Domain.Mappers
{
    public class MapperInitialize : IStartupTask
    {
        public void Execute()
        {
            Mapper.CreateMap<string, int>().ConvertUsing(Convert.ToInt32);
            Mapper.CreateMap<string, DateTime>().ConvertUsing(new DateTimeTypeConverter());
            Mapper.CreateMap<DateTime, string>().ConvertUsing(new DateTimeToStringConverter());
            Mapper.CreateMap<string, Type>().ConvertUsing<TypeTypeConverter>();
            Mapper.AssertConfigurationIsValid();
        }

        public int Order
        {
            get { return -1; }
        }
    }
}
