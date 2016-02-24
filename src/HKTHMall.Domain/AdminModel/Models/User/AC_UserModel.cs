using System;
using System.ComponentModel.DataAnnotations;
using FluentValidation.Attributes;
using HKTHMall.Domain.Validators.AC;
using HKTHMall.Domain.Models.AC;

namespace HKTHMall.Domain.Models.User
{
    /// <summary>
    /// 用户视图模型
    /// </summary>
    [Validator(typeof(AC_UserValidator))]
    public class AC_UserModel
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public long UserID { get; set; }

        /// <summary>
        /// 角色ID
        /// </summary>
        [Display(Name = "Subordinate role")]
        public int RoleID { get; set; }

        /// <summary>
        /// 部门ID
        /// </summary>
        [Display(Name = "Subordinate department")]
        public int ID { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        [Display(Name = "User account")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "Account length can not be less than{2} And to be less than{1}")]
        [RegularExpression(@"^\w+$", ErrorMessage = "The user name can only be made of numbers, letters, or underscores.")]
        public string UserName { get; set; }

        /// <summary>
        /// 真实姓名
        /// </summary>
        [Display(Name = "Real name")]
        public string RealName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Display(Name = "User password")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Password length can not be less than{2} And to be less than{1}")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        /// <summary>
        /// 密码确认
        /// </summary>
        [Display(Name = "Password confirmation")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Same password")]
        public string PasswordTwo { 
            get; set; 
        }

        /// <summary>
        /// 性别
        /// </summary>
        [Display(Name = "User Sex")]
        public bool Sex { get; set; }

        /// <summary>
        /// 身份证号码
        /// </summary>
        [Display(Name = "ID number")]
        [RegularExpression(@"^[0-9\.]*$", ErrorMessage = "Please enter a number or English")]
        public string IDNumber { get; set; }

        /// <summary>
        /// 用户状态
        /// </summary>
        [Display(Name = "User status")]
        public int UserMode { get; set; }

        /// <summary>
        /// 最后登录时间
        /// </summary>
        public Nullable<System.DateTime> LastLoginTime { get; set; }

        /// <summary>
        /// 登录次数
        /// </summary>
        public decimal LoginTimes { get; set; }

        /// <summary>
        /// 添加人
        /// </summary>
        public string CreateUser { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        public System.DateTime CreateDT { get; set; }

        /// <summary>
        /// 修改人
        /// </summary>
        public string UpdateUser { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public Nullable<System.DateTime> UpdateDt { get; set; }

        public string DeptName { get; set; }

        public string RoleName { get; set; }

        
        
        public string PasswordOld
        {
            get;
            set;
        }
    }
}
