using FluentValidation.Attributes;
using HKTHMall.Domain.Validators.AC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.Models.AC
{
    /// <summary>
    /// 系统功能
    /// </summary>

    [Validator(typeof(AC_FunctionValidator))]
    public class AC_FunctionModel
    {
        public int FunctionID { get; set; }
        public string FunctionName { get; set; }

        public string ModuleName { get; set; }
        public int ModuleID { get; set; }
        public int FirstModuleID { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
    }
}
