using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using HKTHMall.Domain.AdminModel.Models.AC;
using HKTHMall.Domain.Models.AC;

namespace HKTHMall.Domain.Validators.AC
{
    /// <summary>
    /// 部门视图模型验证
    /// </summary>
    public class AC_DepartmentValidator : AbstractValidator<AC_DepartmentModel>
    {
        public AC_DepartmentValidator()
        {
            //RuleFor(x => x.DeptName).NotEmpty().WithMessage("请输入部门名称");
            RuleFor(x => x.OrderNumber)
                .NotEmpty()
                .WithMessage("Please enter the sort field")
                .GreaterThanOrEqualTo(0)
                .WithMessage("Please enter a number greater than or equal to 0.");

            RuleFor(x => x.DeptName).NotEmpty();
                


        }
    }
}
