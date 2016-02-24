using FluentValidation.Attributes;
using HKTHMall.Domain.AdminModel.Validators.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.AdminModel.Models.APP
{
    [Validator(typeof(APP_VersionInfoValidator))]
    public class APP_VersionInfoModel
    {
        /// <summary>
        /// ID
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 应用名称
        /// </summary>
        public string APPName { get; set; }

        /// <summary>
        /// 版本类型1安卓,2IOS
        /// </summary>
        //public int APPTypeID { get; set; }

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
        /// 文件大小
        /// </summary>
        public string FileSize { get; set; }

        /// <summary>
        /// 下载地址
        /// </summary>
        public string DownloadURL { get; set; }

        /// <summary>
        /// 更新内容描述(中)
        /// </summary>
        public string UpdateInfo { get; set; }

        /// <summary>
        /// 更新内容描述(泰)
        /// </summary>
        public string UpdateInfoTH { get; set; }

        /// <summary>
        /// 更新内容描述(英)
        /// </summary>
        public string UpdateInfoEN { get; set; }

        /// <summary>
        /// 是否强制更新
        /// </summary>
        public Nullable<int> IsForceUpdate { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateBy { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public Nullable<System.DateTime> CreateDT { get; set; }

        /// <summary>
        /// 修改人
        /// </summary>
        public string UpdateBy { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public Nullable<System.DateTime> UpdateDT { get; set; }
    }
}
