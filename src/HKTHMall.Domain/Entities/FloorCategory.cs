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
    
    public partial class FloorCategory
    {
        public long FloorCategoryId { get; set; }
        public int CategoryId { get; set; }
        public int DCategoryId { get; set; }
        public long Place { get; set; }
        public string AddUsers { get; set; }
        public System.DateTime AddTime { get; set; }
    }
}