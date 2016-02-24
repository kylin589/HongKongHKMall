using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace HKTHMall.WebApi.Models.Result
{
    [DataContract]
    public class ResponseGetGroupList
    {
        /// <summary>
        /// 用户名称     
        /// </summary>
        [DataMember(Name = "userId")]
        public long UserId { set; get; }
        /// <summary>
        /// 加盟时间     
        /// </summary>
        [DataMember(Name = "addTime")]
        public long AddTime { set; get; }
        /// <summary>
        /// 用户头像     
        /// </summary>
        [DataMember(Name = "imageUrl")]
        public string ImageUrl { set; get; }
        /// <summary>
        /// 用户昵称     
        /// </summary>
        [DataMember(Name = "nickName")]
        public string NickName { set; get; }

        /// <summary>
        /// 账号     
        /// </summary>
        [DataMember(Name = "account")]
        public string Account { set; get; }
        /// <summary>
        /// 电话号码     
        /// </summary>
        [DataMember(Name = "phone")]
        public string Phone { set; get; }
        /// <summary>
        /// 真实姓名     
        /// </summary>
        [DataMember(Name = "realName")]
        public string RealName { set; get; }
        /// <summary>
        /// 感恩     
        /// </summary>
        [DataMember(Name = "gnNumber")]
        public string GnNumber { set; get; }
        /// <summary>
        /// 感动     
        /// </summary>
        [DataMember(Name = "gdNumber")]
        public string GdNumber { set; get; }
        /// <summary>
        /// 感谢     
        /// </summary>
        [DataMember(Name = "gxNumber")]
        public string GxNumber { set; get; }
    }
}