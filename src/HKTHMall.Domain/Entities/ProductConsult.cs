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
    
    public partial class ProductConsult
    {
        public long ProductConsultId { get; set; }
        public long ProductId { get; set; }
        public long UserID { get; set; }
        public string ConsultContent { get; set; }
        public System.DateTime ConsultDT { get; set; }
        public int IsAnonymous { get; set; }
        public int CheckStatus { get; set; }
        public string CheckBy { get; set; }
        public Nullable<System.DateTime> CheckDT { get; set; }
        public string ReplyBy { get; set; }
        public Nullable<System.DateTime> ReplyDT { get; set; }
        public string ReplyContent { get; set; }
    }
}
