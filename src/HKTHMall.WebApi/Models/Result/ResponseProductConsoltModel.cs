using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HKTHMall.WebApi.Models.Result
{
    /// <summary>
    /// 产品列表返回参数 zzr
    /// </summary>
    public class ResponseProductConsoltModel
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public long userID { get; set; }
        /// <summary>
        /// 用户昵称
        /// </summary>
        public string nickName { get; set; }
        /// <summary>
        /// 咨询问题
        /// </summary>
        public string question { get; set; }
        /// <summary>
        /// 回复
        /// </summary>
        public string answer { get; set; }
        /// <summary>
        /// 咨询时间戳
        /// </summary>
        public long consultDt { get; set; }        
       
    }
}