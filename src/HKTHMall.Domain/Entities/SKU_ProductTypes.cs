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
    
    public partial class SKU_ProductTypes
    {
        public SKU_ProductTypes()
        {
            this.CategoryType = new HashSet<CategoryType>();
            this.SKU_ProductTypeAttribute = new HashSet<SKU_ProductTypeAttribute>();
        }
    
        public long SkuTypeId { get; set; }
        public string Name { get; set; }
        public Nullable<int> IsGoods { get; set; }
        public Nullable<int> UseExtend { get; set; }
        public Nullable<int> UseParameter { get; set; }
        public string Remark { get; set; }
        public int IsUse { get; set; }
        public string CreateBy { get; set; }
        public Nullable<System.DateTime> CreateDT { get; set; }
        public string UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateDT { get; set; }
    
        public virtual ICollection<CategoryType> CategoryType { get; set; }
        public virtual ICollection<SKU_ProductTypeAttribute> SKU_ProductTypeAttribute { get; set; }
    }
}