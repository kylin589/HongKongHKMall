
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OfficialWeb.common
{
    public class SuppliersLogin
    {
        /// <summary>
        ///     获取登录用户信息
        /// </summary>
        /// <returns></returns>
        public static SuppliersInfoModel GetLoginSuppliersinfo
        {
            get
            {
                if (HttpContext.Current.Session["SuppliersModel"] != null)
                {
                    return (HttpContext.Current.Session["SuppliersModel"] as SuppliersInfoModel);
                }
                return null;
            }
        }

        /// <summary>
        /// 供应商ID
        /// </summary>
        public static long CurrentSuppliersID
        {
            get
            {
                var Info = GetLoginSuppliersinfo;
                if (Info != null)
                {
                    return Info.SupplierId;
                }
                return 0;
            }
        }

        /// <summary>
        /// 供应商登录名
        /// </summary>
        public static string CurrentLoginMobile
        {
            get
            {
                var Info = GetLoginSuppliersinfo;
                if (Info != null)
                {
                    return Info.Mobile;
                }
                return "";
            }
        }

        /// <summary>
        /// 供应商名称
        /// </summary>
        public static string CurrentSupplierName
        {
            get
            {
                var Info = GetLoginSuppliersinfo;
                if (Info != null)
                {
                    return Info.SupplierName;
                }
                return "";
            }
        }
    }

    [Serializable]
    public class SuppliersInfoModel
    {
        /// <summary>
        ///     供应商ID
        /// </summary>
        public long SupplierId { get; set; }

        /// <summary>
        ///     供应商登录名
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 供应商名称
        /// </summary>
        public string SupplierName { get; set; }
    }
}