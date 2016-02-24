using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.Attributes;
using HKTHMall.Domain.Validators.AC;

namespace HKTHMall.Domain.Models.AC
{

    /// <summary>
    /// 模块视图模型
    /// </summary>
    [Validator(typeof(AC_ModuleValidator))]
    public class AC_ModuleModel
    {
        public int ModuleID { get; set; }
        public string ModuleName { get; set; }
        public int ParentID { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public long Place { get; set; }
        public string Icon { get; set; }

    }
}
