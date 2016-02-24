using System;
using System.ComponentModel.DataAnnotations;
using FluentValidation.Attributes;
using HKTHMall.Domain.Validators.AC;

namespace HKTHMall.Domain.AdminModel.Models.AC
{
    /// <summary>
    /// 部门视图模型
    /// </summary>
    [Validator(typeof(AC_DepartmentValidator))]
    public class AC_DepartmentModel
    {


        public int ID { get; set; }

        [Display(Name = "Department name")]
        public string DeptName { get; set; }
        public int ParentID { get; set; }

        [Display(Name = "Sort")]
        [RegularExpression(@"^[0-9\.]*$", ErrorMessage = "Please enter a number")]
        public int OrderNumber { get; set; }

        [Display(Name = "Is Activation")]
        public Nullable<byte> IsActive { get; set; }
        public string CreateBy { get; set; }
        public Nullable<DateTime> CreateDT { get; set; }
        public string UpdateBy { get; set; }
        public Nullable<DateTime> UpdateDT { get; set; }

    }
}
