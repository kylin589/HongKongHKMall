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
    
    public partial class CategoryType
    {
        public long CategoryTypeId { get; set; }
        public long SkuTypeId { get; set; }
        public int CategoryId { get; set; }
    
        public virtual SKU_ProductTypes SKU_ProductTypes { get; set; }
    }
}
