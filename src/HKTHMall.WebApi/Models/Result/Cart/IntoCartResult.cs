
﻿using HKTHMall.WebApi.Common;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.WebApi.Models.Result.Cart
{
   public class IntoCartResult: ExResult
    {
       /// <summary>
       ///返回结果
       /// </summary>
       [DataMember(Name = "rs")]
       public ResultM Rs { set; get; }
    }

   [DataContract]
   /// <summary>
   /// 返回结果
   /// </summary>
   public class ResultM
   {
       /// <summary>
       /// 购物车数量
       /// </summary>
       [DataMember]
       public int number { get; set; }
       /// <summary>
       /// 限购数量
       /// </summary>
       [DataMember]
       public int limit { get; set; }
   }
}
