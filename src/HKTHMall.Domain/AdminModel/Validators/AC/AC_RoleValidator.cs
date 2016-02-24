using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using HKTHMall.Domain.Models.AC;


namespace HKTHMall.Domain.Validators.AC
{
    public class AC_RoleValidator : AbstractValidator<AC_RoleModel>
    {
        public AC_RoleValidator()
        {
            RuleFor(p => p.RoleName).NotEmpty().WithMessage("Please enter the role name").Length(2, 20).WithMessage("Length in 2-20 characters or characters");
            //RuleFor(p => p.RoleModuleValue).NotEmpty().WithMessage("请选择权限");
            RuleFor(p => p.RoleModuleValue).NotEmpty();
        }
        /// <summary>
        /// 检查是否是合法的邮政编码
        /// </summary>
        /// <param name="postcode"></param>
        /// <returns></returns>
        private bool BeAValidPostcode(string postcode)
        {
            if (!string.IsNullOrEmpty(postcode) && postcode.Length == 6)
            {
                return true;
            }
            return false;
        }

    }
}
