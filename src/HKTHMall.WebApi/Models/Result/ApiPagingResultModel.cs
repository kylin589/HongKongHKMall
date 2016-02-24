using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace HKTHMall.WebApi.Models.Result
{
    public class ApiPagingResultModel : ApiResultModel
    {
        public ApiPagingResultModel()
        {
            this.totalSize = 0;
        }
        
        /// <summary>
        /// 总页数
        /// </summary>
        [DataMember(Name = "totalSize")]
        public int totalSize;
    }
}