using FluentValidation;
using FluentValidation.Attributes;
using HKTHMall.Core.Security;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HKTHMall.WebApi.Models.Request
{
   
    public class GetFeedbackType
    {      
        
        /// <summary>
        /// 类型编号
        /// </summary>
        public int typeId { get; set; }
        /// <summary>
        ///类型名称
        /// </summary>
        public string typeName { get; set; }
    
        
    }
}