using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Services.Sys
{
    /// <summary>
    /// 系统参数扩展类
    /// <author>樊利民</author>
    /// </summary>
    public static class ParameterSetExtension
    {
        /// <summary>
        /// 公司虚拟账号
        /// </summary>
        public const long PARAM_VIRTUAL_ACCOUNT_ID = 1215894621;

        /// <summary>
        /// 交易密码错误锁定时间
        /// </summary>
        public const long PARAM_PAYPASSWORD_TIME = 1856770932;

        /// <summary>
        /// 交易密码错误锁定次数
        /// </summary>
        public const long PARAM_PAYPASSWORD_COUNT = 1856771060;

        /// <summary>
        /// PayPal Api类型 1:REST API 2:Classic API (Classic API sandbox 升级，不能用于测试)
        /// </summary>
        public const long PAYPAL_API_TYPE = 7529235516;
    }
}
