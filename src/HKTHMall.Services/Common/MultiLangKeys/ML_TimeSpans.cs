using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using HKTHMall.Domain.Entities;
using HKTHMall.Domain.Enum;

namespace HKTHMall.Services.Common.MultiLangKeys
{
    /// <summary>
    /// 多语言时间间隔
    /// </summary>
    public static class ML_TimeSpanTypes
    {
        /// <summary>
        /// 中文时间集合
        /// </summary>
        public static readonly Dictionary<int, string> TimeSpanTypes_zh_CN = new Dictionary<int, string>
        {
            {0,"全部"},
            {1,"近半个月"},
            {7,"近三个月"},
            {11,"更早"}
           
        };
        /// <summary>
        /// 泰语时间集合
        /// </summary>
        public static readonly Dictionary<int, string> TimeSpanTypes_th_TH = new Dictionary<int, string>
        {
            {0,"ทั้งหมด"},
            {1,"สองสัปดาห์ที่ผ่านมา"},
            {7,"สามเดือนที่ผ่านมา"},
            {11,"ก่อนหน้านี้"}
           
        };
        /// <summary>
        /// 英语时间集合
        /// </summary>
        public static readonly Dictionary<int, string> TimeSpanTypes_en_US = new Dictionary<int, string>
        {
            {0,"ALL"},
            {1,"Past 3 months"},
            {7,"Past 3 months"},
            {11,"Earlier"}
           
        };

        /// <summary>
        /// 繁体中文时间集合
        /// </summary>
        public static readonly Dictionary<int, string> TimeSpanTypes_zh_HK = new Dictionary<int, string>
        {
            {0,"全部"},
            {1,"近半個月"},
            {7,"近三個月"},
            {11,"更早"}
           
        };


        /// <summary>
        /// 获取查询订单时间间隔下拉选项
        /// </summary>
        /// <param name="languageType">语言类型</param>
        /// <param name="excludeTimeSpanTypes">需要排除的时间间隔</param>
        /// <param name="defaulType">默认类型</param>
        /// <returns>时间间隔集合</returns>
        public static List<SelectListItem> GetLocalTimeSpanTypesInto(LanguageType languageType = LanguageType.zh_CN, int[] excludeTimeSpanTypes = null, OrderEnums.TimeSpanType defaulType = OrderEnums.TimeSpanType.HalfOfMonth)
        {
            Dictionary<int, string> timeSpanTypes = ML_TimeSpanTypes.GetLocalTimeSpanTypes(languageType);

            if (excludeTimeSpanTypes != null)
            {
                foreach (var status in excludeTimeSpanTypes)
                {
                    if (timeSpanTypes.ContainsKey(status))
                    {
                        timeSpanTypes.Remove(status);
                    }
                }
            }
            return timeSpanTypes.Select(x => new SelectListItem() { Text = x.Value, Value = x.Key.ToString(), Selected = x.Key == (int)defaulType }).ToList();
        }

        /// <summary>
        /// 获取查询订单时间间隔下拉选项
        /// </summary>
        /// <param name="languageId">语言Id</param>
        /// <param name="excludeStatus">需要排除的时间间隔</param>
        /// <param name="defaulType">默认类型</param>
        /// <returns>时间间隔集合</returns>
        public static List<SelectListItem> GetLocalTimeSpanTypesInto(int languageId = (int)LanguageType.zh_CN, int[] excludeStatus = null, OrderEnums.TimeSpanType defaulType = OrderEnums.TimeSpanType.HalfOfMonth)
        {

            LanguageType languageType = (LanguageType)languageId;
            return ML_TimeSpanTypes.GetLocalTimeSpanTypesInto(languageType, excludeStatus, defaulType);
        }

        /// <summary>
        /// 获取本地语言时间间隔集合
        /// </summary>
        /// <param name="languageId">语言Id</param>
        /// <returns>时间间隔集合</returns>
        public static Dictionary<int, string> GetLocalTimeSpanTypes(int languageId = (int) LanguageType.zh_CN)
        {
            LanguageType languageType = (LanguageType)languageId;
            return ML_TimeSpanTypes.GetLocalTimeSpanTypes(languageType);
        }

        /// <summary>
        /// 获取本地语言时间间隔集合
        /// </summary>
        /// <param name="languageType">语言类型</param>
        /// <returns>时间间隔集合</returns>
        public static Dictionary<int, string> GetLocalTimeSpanTypes(LanguageType languageType = LanguageType.zh_CN)
        {
            Dictionary<int, string> timeSpanTypes = new Dictionary<int, string>();
            switch (languageType)
            {
                case LanguageType.zh_CN:
                    timeSpanTypes = TimeSpanTypes_zh_CN;
                    break;
                case LanguageType.en_US:
                    timeSpanTypes = TimeSpanTypes_en_US;
                    break;
                case LanguageType.th_TH:
                    timeSpanTypes = TimeSpanTypes_th_TH;
                    break;
                case LanguageType.zh_HK:
                    timeSpanTypes = TimeSpanTypes_zh_HK;
                    break;
            }
            return timeSpanTypes;
        }
    }
}
