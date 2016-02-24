using HKTHMall.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace HKTHMall.Admin.common
{
    public class ACultureHelper
    {
        /// <summary>
        /// 获取默认的语言区域,暂时默认返回中文（1中文,2英语,3泰语）
        /// </summary>
        /// <returns></returns>
        public static int GetLanguageID
        {
            get { return (int)LanguageType.zh_HK; } // return Default culture,update by liujc(默认返回中文香港)
        }
        /// <summary>
        /// 获取语言显示名称,add by liujc
        /// </summary>
        /// <param name="LanguageID"></param>
        /// <returns></returns>
        public static string GetLanguageName(LanguageType LanguageID)
        {
            switch (LanguageID)
            {
                case LanguageType.zh_CN:
                    return "Chinese";//中文
                case LanguageType.en_US:
                    return "English";//英文
                case LanguageType.th_TH:
                    return "Thai";//泰文
                case LanguageType.zh_HK:
                    return "Chinese(HK)";//中文，香港地区 add by liujc
                default:
                    return "Chinese(HK)";
            }
        }
        public static string GetLanguageName(int LanguageID)
        {
            return GetLanguageName((LanguageType)LanguageID);
        }

        private static readonly Dictionary<int, string> _langlist = new Dictionary<int, string>();
        private static readonly DataTable _langdt = new DataTable();
        static ACultureHelper()
        {
            //_langlist.Add(3, "Thai");//泰文
            _langlist.Add(1, "Chinese");//中文
            _langlist.Add(2, "English");//英文
            _langlist.Add(4, "Chinese(HK)");//香港 add by liujc

            _langdt.Columns.Add("SortId", typeof(Int32));
            _langdt.Columns.Add("LanguageId", typeof(Int32));
            _langdt.Columns.Add("LanguageName", typeof(String));

            int count = 1;
            foreach (KeyValuePair<int, string> _pair in _langlist)
            {
                _langdt.Rows.Add(new object[] { count, _pair.Key, _pair.Value });
                count++;
            }
        }

        /// <summary>
        /// 返回语言列表，键值对
        /// </summary>
        /// <returns></returns>
        public static Dictionary<int, string> GetLanguageList()
        {
            return _langlist;
        }
        public static DataTable GetLanguageTable()
        {
            return _langdt;
        }
    }
}