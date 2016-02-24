using System;
using AutoMapper;

namespace HKTHMall.Domain.Mappers.Converters
{
    public class DateTimeTypeConverter : ITypeConverter<string, DateTime>
    {
        public DateTime Convert(ResolutionContext context)
        {
            return System.Convert.ToDateTime(context.SourceValue);
        }
    }

    public class DateTimeToStringConverter : ITypeConverter<DateTime, string>
    {
        public string Convert(ResolutionContext context)
        {
            return ((DateTime) context.SourceValue).ToString("yyyy-MM-dd hh:mm:ss");
        }
    }
}
