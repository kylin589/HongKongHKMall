using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.WebApi.Models.Result
{
    [DataContract]
     public class ApiResultModel
    {
        public ApiResultModel()
        {
            this.flag = 0;
        }
        /// <summary>
        /// 是否执行成功状态标志0:失败1:成功
        /// </summary>
        [DataMember(Name = "flag")]
        public int flag;

        /// <summary>
        /// 提示信息
        /// </summary>
        [DataMember(Name = "msg")]
        public string msg;

        /// <summary>
        /// 集合
        /// </summary>
        [DataMember(Name = "rs")]
        public dynamic rs;
    }

}
