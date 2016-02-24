using System;
using AutoMapper;

namespace HKTHMall.Domain.Mappers.Converters
{
    public class TypeTypeConverter : ITypeConverter<string, Type>
    {
        public Type Convert(ResolutionContext context)
        {
            return context.SourceType;
        }
    }
}
