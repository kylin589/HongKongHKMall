using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Management;
using System.Web;

namespace HKTHMall.Core
{
    /// <summary>
    /// 全局的设置类,可在此类中找到大多数的设置参数
    /// @Author yewei 
    /// @Date 2015-01-19
    /// </summary>
    public class Settings
    {


        /// <summary>
        ///  本地发送短信伪验证开关
        /// </summary>
        public static bool IsMessageEM
        {
            get
            {
                string is_open = GetSettingByKey("IsMessageEM");
                if (string.IsNullOrEmpty(is_open))
                {
                    return false;
                }
                return Convert.ToBoolean(is_open);
            }
        }



        /// <summary>
        /// 是否启用帐号系统开关
        /// </summary>
        public static bool IsEnableEM
        {
            get
            {
                string is_open = GetSettingByKey("IsEnableEM");
                if (string.IsNullOrEmpty(is_open))
                {
                    return false;
                }
                return Convert.ToBoolean(is_open);
            }
        }


        /// <summary>
        ///  是否通知大明公司修改用户资料
        /// </summary>
        public static bool IsNoticeEM
        {
            get
            {
                string is_open = GetSettingByKey("IsNoticeEM");
                if (string.IsNullOrEmpty(is_open))
                {
                    return false;
                }
                return Convert.ToBoolean(is_open);
            }
        }


        /// <summary>
        /// 主账号业务系统编号
        /// </summary>
        public static int EmSystemId
        {
            get
            {
                string system_id = GetSettingByKey("Em_System_Id");
                int id = 0;
                int.TryParse(system_id, out id);
                return id;
            }
        }

        /// <summary>
        /// 1=无验证码无手机号
        /// 2=有验证码有手机号
        /// 3=无验证码有手机号
        /// </summary>
        public static int EmMobileVerify
        {
            get
            {
                string mobileVerify = GetSettingByKey("Em_MobileVerify");
                if (string.IsNullOrEmpty(mobileVerify))
                {
                    return 1;
                }
                return Convert.ToInt32(mobileVerify);
            }
        }

        /// <summary>
        /// 注册默认邀请码
        /// </summary>
        public static int InvitationCode
        {
            get
            {
                string InvitationCode = GetSettingByKey("InvitationCode");
                int code = 0;
                int.TryParse(InvitationCode, out code);
                return code;
            }
        }


        #region   By  熊伟
        /// <summary>
        /// 获取mac
        /// </summary>
        /// <returns></returns>
        public static string GetMacAddress()
        {
            try
            {
                string mac = "";
                ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    if ((bool)mo["IPEnabled"] == true)
                    {
                        mac = mo["MacAddress"].ToString();
                        break;
                    }
                }
                moc = null;
                mc = null;
                return mac;
            }
            catch
            {
                return "unknow";
            }
        }

        /// <summary>
        /// 获取客户端IP地址
        /// </summary>
        /// <returns></returns>
        public static string GetIPAddress()
        {
            try
            {
                string user_IP = string.Empty;
                if (System.Web.HttpContext.Current.Request.ServerVariables["HTTP_VIA"] != null)
                {
                    if (System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
                    {
                        user_IP = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
                    }
                    else
                    {
                        user_IP = System.Web.HttpContext.Current.Request.UserHostAddress;
                    }
                }
                else
                {
                    user_IP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
                    if (user_IP.Equals("::1"))
                    {
                        user_IP = "127.0.0.1";
                    }
                }
                return user_IP;
            }
            catch (Exception e)
            {
                return "";
            }

        }

        /// <summary>
        /// 获取短信有效时间  By  熊伟
        /// </summary>
        public static int Life
        {
            get
            {
                string life = GetSettingByKey("life");
                int id = 0;
                int.TryParse(life, out id);
                return id;
            }
        }

        #endregion

        /// <summary>
        /// 注册来源, 1:网站
        /// </summary>
        public static int EmDev
        {
            get
            {
                string system_id = GetSettingByKey("Em_Dev", "1");
                int id = 1;
                bool rtv = int.TryParse(system_id, out id);
                return rtv ? id : 1;
            }
        }


        /// <summary>
        /// 根据key获取配置value
        /// </summary>
        /// <param name="xmlkey"></param>
        /// <returns></returns>
        public static string GetSettingByKey(string xmlkey)
        {
            return GetSettingByKey(xmlkey, string.Empty);
        }

        /// <summary>
        /// 根据key获取配置value
        /// 没有则返回用户defaultVal
        /// </summary>
        /// <param name="xmlkey"></param>
        /// <param name="defaultUrl"></param>
        /// <returns></returns>
        public static string GetSettingByKey(string xmlkey, string defaultVal)
        {
            if (ConfigurationManager.AppSettings[xmlkey] != null)
            {
                string val = ConfigurationManager.AppSettings[xmlkey].ToString();
                return string.IsNullOrEmpty(val) ? defaultVal : val;
            }
            else
            {
                return defaultVal;
            }
        }


    }
}
