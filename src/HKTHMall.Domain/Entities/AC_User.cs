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
    
    public partial class AC_User
    {
        public long UserID { get; set; }
        public int RoleID { get; set; }
        public int ID { get; set; }
        public string UserName { get; set; }
        public string RealName { get; set; }
        public string Password { get; set; }
        public bool Sex { get; set; }
        public string IDNumber { get; set; }
        public int UserMode { get; set; }
        public Nullable<System.DateTime> LastLoginTime { get; set; }
        public decimal LoginTimes { get; set; }
        public string CreateUser { get; set; }
        public System.DateTime CreateDT { get; set; }
        public string UpdateUser { get; set; }
        public Nullable<System.DateTime> UpdateDt { get; set; }
    
        public virtual AC_Department AC_Department { get; set; }
        public virtual AC_Role AC_Role { get; set; }
    }
}