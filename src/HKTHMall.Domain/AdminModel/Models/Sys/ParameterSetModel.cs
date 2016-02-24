using FluentValidation.Attributes;
using HKTHMall.Domain.Entities;
using HKTHMall.Domain.Validators.Sys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.Models.Sys
{
    /// <summary>
    /// 系统参数设置类
    /// <remarks>added by jimmy,2015-7-1</remarks>
    /// </summary>
    [Validator(typeof(ParameterSetValidator))]
    public class ParameterSetModel
    {
        public long ParamenterID { get; set; }
        public string keys { get; set; }
        public string PValue { get; set; }
        public string Remark { get; set; }
        public string CreateBy { get; set; }
        public Nullable<System.DateTime> CreateDT { get; set; }
        public string UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateDT { get; set; }
    }
}
