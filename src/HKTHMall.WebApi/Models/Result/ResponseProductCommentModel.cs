using HKTHMall.Domain.Models.Categoreis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace HKTHMall.WebApi.Models.Result
{
    [DataContract]
    public class ResponseProductCommentModel : ApiResultModel
    {
        /// <summary>
        /// 商品评论列表
        /// </summary>
        [DataMember]
        public List<SP_ProductCommentModel> rs { set; get; }

        /// <summary>
        /// 总条数
        /// </summary>
        [DataMember]
        public int TotalCount { get; set; }

        /// <summary>
        /// goodCount 好评数(返回百分比)
        /// </summary>
        [DataMember]
        public string goodCount { get; set; }

        /// <summary>
        /// midCount 中评数(返回百分比)
        /// </summary>
        [DataMember]
        public string midCount { get; set; }

        /// <summary>
        /// badCount 差评数(返回百分比)
        /// </summary>
        [DataMember]
        public string badCount { get; set; }
        [DataMember]
        public int GCount { get; set; }
        [DataMember]
        public int MCount { get; set; }
        [DataMember]
        public int BCount { get; set; }
        [DataMember]
        public int AllCount { get; set; }
    }


}