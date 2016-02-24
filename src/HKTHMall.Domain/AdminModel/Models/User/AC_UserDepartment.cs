using HKTHMall.Domain.AdminModel.Models.AC;
using HKTHMall.Domain.Models.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.AdminModel.Models.User
{
    /// <summary>
    /// 后台用户和部门Model
    /// </summary>
    public class AC_UserDepartment : AC_UserModel
    {
        /// <summary>
        /// 部门名称
        /// </summary>
        [Display(Name = "Department name")]
        public string DeptName { get; set; }

        /// <summary>
        /// 父部门ID
        /// </summary>
        public int ParentID { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        [Display(Name = "Sort")]
        [RegularExpression(@"^[0-9\.]*$", ErrorMessage = "Please enter a number")]
        public int OrderNumber { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        [Display(Name = "Is Activation")]
        public Nullable<byte> IsActive { get; set; }
    }
}
