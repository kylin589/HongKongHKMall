using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.AdminModel.Models.Localization
{
    public class MultiLanguageModel
    {
        /// <summary>
        /// 语言Key
        /// </summary>
        public string LangKey { get; set; }
        /// <summary>
        /// 泰语
        /// </summary>
        public string NameTH { get; set; }
        /// <summary>
        /// 英语
        /// </summary>
        public string NameEng { get; set; }
        /// <summary>
        /// 简体中文
        /// </summary>
        public string NameChs { get; set; }
        /// <summary>
        /// 繁体中文
        /// </summary>
        public string NameHK { get; set; }
    }
}
