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
    
    public partial class AC_Module
    {
        public AC_Module()
        {
            this.AC_Function = new HashSet<AC_Function>();
        }
    
        public int ModuleID { get; set; }
        public string ModuleName { get; set; }
        public int ParentID { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public long Place { get; set; }
        public string Icon { get; set; }
    
        public virtual ICollection<AC_Function> AC_Function { get; set; }
    }
}
