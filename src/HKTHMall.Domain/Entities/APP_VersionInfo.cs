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
    
    public partial class APP_VersionInfo
    {
        public int ID { get; set; }
        public string APPName { get; set; }
        public int Platform { get; set; }
        public string PackageName { get; set; }
        public string VersionNO { get; set; }
        public string VersionName { get; set; }
        public string FileSize { get; set; }
        public string DownloadURL { get; set; }
        public string UpdateInfo { get; set; }
        public string UpdateInfoEN { get; set; }
        public string UpdateInfoTH { get; set; }
        public Nullable<int> IsForceUpdate { get; set; }
        public string CreateBy { get; set; }
        public Nullable<System.DateTime> CreateDT { get; set; }
        public string UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateDT { get; set; }
    }
}