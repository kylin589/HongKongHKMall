using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace HKTHMall.WebApi.Models.Result
{
    [DataContract]
    public class ExResult
    {
        /// <summary>
        /// 状态标志,0:失败1:成功
        /// </summary>
        [DataMember(Name = "flag")]
        public int Flag { set; get; }

        /// <summary>
        /// 提示信息
        /// </summary>
        [DataMember(Name = "msg")]
        public string Msg { set; get; }

    }

    [DataContract]
    public class PicExt
    {
        /// <summary>
        /// 图片地址
        /// </summary>
        [DataMember(Name = "imgUrl")]
        public string imgUrl { set; get; }

    }
}