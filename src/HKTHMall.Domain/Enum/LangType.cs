using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.Enum
{
    /// <summary>
    /// 语言枚举
    /// </summary>
    public enum LanguageType
    {
        /// <summary>
        /// 默认语言
        /// </summary>
        defaultLang = 3,

        /// <summary>
        /// 中文
        /// </summary>
        zh_CN = 1,

        /// <summary>
        /// 英语
        /// </summary>
        en_US = 2,

        /// <summary>
        /// 泰语
        /// </summary>
        th_TH = 3,

        /// <summary>
        /// （中文）香港地区
        /// </summary>
        zh_HK = 4 //add by liujc
    }
}
