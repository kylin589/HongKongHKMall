using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.WebModel.Models.Version
{
   public class APP_VersionInfo
    {
		/// <summary>
		/// ID
		/// </summary>
		public int ID{ get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string APPName{ get; set; }
		/// <summary>
		/// APP平台,1:ios; 2: Android
		/// </summary>
		public int Platform{ get; set; }
		/// <summary>
		/// 包名
		/// </summary>
		public string PackageName{ get; set; }
		/// <summary>
		/// 版本编号
		/// </summary>
		public string VersionNO{ get; set; }
		/// <summary>
		/// 版本名称
		/// </summary>
		public string VersionName{ get; set; }
		/// <summary>
		/// 文件大小
		/// </summary>
		public string FileSize{ get; set; }
		/// <summary>
		/// 下载地址
		/// </summary>
		public string DownloadURL{ get; set; }
		/// <summary>
		/// 更新内容描述_中文
		/// </summary>
		public string UpdateInfo{ get; set; }
        /// <summary>
        /// 更新内容描述_英文
        /// </summary>
        public string UpdateInfoEN { get; set; }
        /// <summary>
        /// 更新内容描述_泰文
        /// </summary>
        public string UpdateInfoTH { get; set; }
		/// <summary>
		/// （0:否；1:是）
		/// </summary>
        public int IsForceUpdate { get; set; }
		/// <summary>
		/// 创建人
		/// </summary>
		public string CreateBy{ get; set; }
		/// <summary>
		/// 创建时间
		/// </summary>
        public DateTime CreateDT { get; set; }
		/// <summary>
		/// 更新人
		/// </summary>
		public string UpdateBy{ get; set; }
		/// <summary>
		/// 更新时间
		/// </summary>
        public DateTime UpdateDT { get; set; }
    }
}
