using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Core.Sql
{
    /// <summary>
    /// Sql语句过滤工具类
    /// </summary>
    public static class SqlFilterUtil
    {
        /// <summary>
        /// 替换SQL特殊字符
        /// </summary>
        /// <param name="str">过滤字段</param>
        /// <returns></returns>
        public static string ReplaceSqlChar(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return String.Empty;
            }
            str = str.Replace("'", "''");
            return str;
        }

    }
}
