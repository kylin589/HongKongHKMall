using BrCms.Framework.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.AdminModel.Models.APP
{
     public class SearchAPP_VersionInfoModel:Paged
    {
         public int ID { get; set; }

        /// <summary>
        /// 应用名称
        /// </summary>
        public string APPName { get; set; }

        /// <summary>
        /// 版本类型1安卓,2IOS
        /// </summary>
        public int APPTypeID { get; set; }

        /// <summary>
        /// APP平台,1:ios; 2: Android
        /// </summary>
        public int Platform { get; set; }

        /// <summary>
        /// 包名
        /// </summary>
        public string PackageName { get; set; }

        /// <summary>
        /// 版本编号
        /// </summary>
        public string VersionNO { get; set; }

        /// <summary>
        /// 版本名称
        /// </summary>
        public string VersionName { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateBy { get; set; }

        /// <summary>
        /// 创建开始时间
        /// </summary>
        public Nullable<System.DateTime> BeginCreateDT { get; set; }

        /// <summary>
        /// 创建结束时间
        /// </summary>
        public Nullable<System.DateTime> EndCreateDT { get; set; }
    }
}
