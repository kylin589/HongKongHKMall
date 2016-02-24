using FluentValidation.Attributes;
using HKTHMall.Domain.AdminModel.Validators.Keywork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.AdminModel.Models.Keywork
{
    /// <summary>
    /// 关键字模型
    /// </summary>
    [Validator(typeof(FloorKeyValidator))]
    public class FloorKeywordModel
    {
        public long FloorKeywordId { get; set; }
        public int PlaceCode { get; set; }
        public string KeyWordName { get; set; }
        public long Sorts { get; set; }
        public string CreateBy { get; set; }
        public Nullable<System.DateTime> CreateDT { get; set; }
        public string UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateDT { get; set; }
        public int LanguageID { get; set; }
        public string LanguageName { get; set; }//update by liujc

    }
}
