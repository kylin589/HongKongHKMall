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
    
    public partial class GroupTestDetail
    {
        public int Id { get; set; }
        public System.DateTime Date { get; set; }
        public int Number { get; set; }
        public int MasterId { get; set; }
    
        public virtual GroupTestMaster GroupTestMaster { get; set; }
    }
}
