using System;

namespace HKTHMall.Core.Extensions
{
    public static class HkExtensions
    {
        public static bool ToBoolean(this int? val)
        {
            if (val == null)
            {
                return false;
            }
            return Convert.ToBoolean(val);
        }
        public static bool ToBoolean(this int val)
        {
            return Convert.ToBoolean(val);
        }
    }
}
