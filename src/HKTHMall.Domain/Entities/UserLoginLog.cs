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
    
    public partial class UserLoginLog
    {
        public int ID { get; set; }
        public Nullable<long> UserID { get; set; }
        public string UserName { get; set; }
        public Nullable<int> LoginSource { get; set; }
        public string IP { get; set; }
        public string LoginAddress { get; set; }
        public Nullable<System.DateTime> LoginTime { get; set; }
    }
}
