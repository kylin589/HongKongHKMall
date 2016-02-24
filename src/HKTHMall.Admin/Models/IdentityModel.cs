using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HKTHMall.Admin.Models
{
    public class IdentityModel
    {
        /// <summary>
        /// 位置所属页面（标识ID）
        /// </summary>
        public int IdentityStatus { get; set; }

        /// <summary>
        /// 位置所属页面名称（标识名称）
        /// </summary>
        public string IdentityStatusName { get; set; }
    }
}