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
    
    public partial class ProductPic
    {
        public long ProductPicId { get; set; }
        public long ProductID { get; set; }
        public string PicUrl { get; set; }
        public int Flag { get; set; }
        public long sort { get; set; }
    
        public virtual Product Product { get; set; }
    }
}