using System;
using AutoMapper;
using BrCms.Framework.Collections;
using BrCms.Framework.Mvc.Extensions;

namespace HKTHMall.Domain.AdminModel.Models.Localization
{
    /// <summary>
    /// 语言
    /// </summary>
    public class LanguageModel
    {
        public LanguageModel()
        {
            Mapper.CreateMap<LanguageModel, AddLanguageModel>();
            Mapper.CreateMap<LanguageModel, UpdateLanguageModel>();
        }

        public T To<T>() where T : class
        {
            return Mapper.Map<LanguageModel, T>(this);
        }

        /// <summary>
        /// 语言Id
        /// </summary>
        public long? ID { get; set; }
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
        /// 香港繁体。add by liujc
        /// </summary>
        public string NameHK { get; set; }
        /// <summary>
        /// 其他
        /// </summary>
        public string NameOther { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDT { get; set; }
        /// <summary>
        /// 更新人
        /// </summary>
        public string UpdateBy { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdateDT { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public int DataType { get; set; }

        /// <summary>
        /// 字典类型
        /// </summary>
        public enum EDataType
        {
            [EnumDescription("Web")]
            Web = 1,
            [EnumDescription("JavaScript")]
            JavaScript = 2
        }

    }

    public class AddLanguageModel
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
        /// 香港繁体.add by liujc
        /// </summary>
        public string NameHK { get; set; }
        /// <summary>
        /// 其他
        /// </summary>
        public string NameOther { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDT { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public int DataType { get; set; }
        ///// <summary>
        ///// 更新人
        ///// </summary>
        //public string UpdateBy { get; set; }
        ///// <summary>
        ///// 更新时间
        ///// </summary>
        //public DateTime? UpdateDT { get; set; }
    }

    public class UpdateLanguageModel
    {
        /// <summary>
        /// 语言ID
        /// </summary>
        public long ID { get; set; }
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
        /// 香港繁体.add by liujc
        /// </summary>
        public string NameHK { get; set; }
        /// <summary>
        /// 其他
        /// </summary>
        public string NameOther { get; set; }
        /// <summary>
        /// 更新人
        /// </summary>
        public string UpdateBy { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdateDT { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public int DataType { get; set; }
    }

    public class SearchLanguageModel : Paged
    {
        /// <summary>
        /// 语言Id
        /// </summary>
        public int? LanguageId { get; set; }
        /// <summary>
        /// 关键词
        /// </summary>
        public string KeyWord { get; set; }

        public int? DataType { get; set; }
    }
}
