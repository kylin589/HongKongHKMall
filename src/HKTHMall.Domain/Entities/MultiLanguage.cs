//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace HKTHMall.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class MultiLanguage
    {
        public long ID { get; set; }
        public string LangKey { get; set; }
        public string NameTH { get; set; }
        public string NameEng { get; set; }
        public string NameChs { get; set; }
        public string NameOther { get; set; }
        public string CreateBy { get; set; }
        public System.DateTime CreateDT { get; set; }
        public string UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateDT { get; set; }
        public int DataType { get; set; }
    }
}