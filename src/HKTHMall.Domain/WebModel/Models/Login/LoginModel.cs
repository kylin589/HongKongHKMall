using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.Attributes;
using HKTHMall.Domain.WebModel.Validators.Login;

namespace HKTHMall.Domain.WebModel.Models.Login
{
    [Validator(typeof(LoginValidator))]
    public class LoginModel
    {
        [Display(Name = "账号")]
        public string account { get; set; }
        [Display(Name = "密码")]
        public string passWord { get; set; }    
        public bool IsJz { get; set; }//记住我
        public string code { get; set; }
    }
}
