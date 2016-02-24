using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace HKTHMall.Core.Extensions
{
    /// <summary>
    /// 枚举扩展类
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// 获取枚举描述信息
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="enumEntity">枚举值</param>
        /// <returns>描述信息</returns>
        public static string Description<T>(this T enumEntity)
        {
            string summary = string.Empty;

            Type type = typeof(T);

            if (!type.IsEnum)
            {
                throw new Exception("类型T必须为枚举类型");
            }
            FieldInfo field = type.GetField(Enum.GetName(type, enumEntity));
            DescriptionAttribute attribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
            if (attribute != null)
            {
                summary = attribute.Description;
            }
            return summary;
        }
    }
}
