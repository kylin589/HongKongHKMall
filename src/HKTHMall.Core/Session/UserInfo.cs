using System;
using System.Web;

namespace HKTHMall.Core
{
    public class UserInfo
    {
        /// <summary>
        ///     获取登录用户信息
        /// </summary>
        /// <returns></returns>
        public static UserInfoModel GetLoginUserInfo
        {
            get
            {
                if (HttpContext.Current.Session["UserInfo"] != null)
                {
                    return (HttpContext.Current.Session["UserInfo"] as UserInfoModel);
                }
                return null;
            }
        }

        public static long CurrentUserID
        {
            get
            {
                var Info = GetLoginUserInfo;
                if (Info != null)
                {
                    return Info.UserID;
                }
                return 0;
            }
        }

        public static string CurrentUserName
        {
            get
            {
                var Info = GetLoginUserInfo;
                if (Info != null)
                {
                    return Info.UserName;
                }
                return "";
            }
        }

        public static int CurrentUserRoleID
        {
            get
            {
                var Info = GetLoginUserInfo;
                if (Info != null)
                {
                    return Info.RoleID;
                }
                return 0;
            }
        }
    }

    [Serializable]
    public class UserInfoModel
    {
        /// <summary>
        ///     后台用户ID
        /// </summary>
        public long UserID { get; set; }

        /// <summary>
        ///     后台用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        ///     角色ID
        /// </summary>
        public int RoleID { get; set; }
    }
}