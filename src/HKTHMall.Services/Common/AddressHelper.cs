using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKTHMall.Domain.Enum;

namespace HKTHMall.Services.Common
{
    /// <summary>
    /// 地址帮助类
    /// </summary>
    public class AddressHelper
    {
        /// <summary>
        /// 展示用户地址
        /// </summary>
        /// <param name="sheng">省份</param>
        /// <param name="shi">市</param>
        /// <param name="qu">区</param>
        /// <param name="detailAddress">详细地址</param>
        /// <param name="lang">语言标识</param>
        /// <returns></returns>
        public static string ShowUserAddress(string country,string sheng, string shi, string qu, string detailAddress, int lang)
        {
            string userAddress = string.Empty;
            LanguageType type = (LanguageType)lang;
            switch (type)
            {
                default:
                case LanguageType.en_US:
                case LanguageType.th_TH:
                    userAddress = detailAddress + qu + shi + sheng+country;
                    break;
                case LanguageType.zh_CN:
                    userAddress =country+ sheng + shi + qu + detailAddress;
                    break;

                case LanguageType.zh_HK:
                    userAddress = country + sheng + shi + qu + detailAddress;
                    break;
            }

            return userAddress;
        }
    }
}
