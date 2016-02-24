using FluentValidation;
using HKTHMall.Domain.Models.banner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKTHMall.Domain.CustomValidation;

namespace HKTHMall.Domain.Validators.banner
{
    public class bannerValidator : AbstractValidator<bannerModel>
    {
        public bannerValidator()
        {
            //RuleFor(x => x.bannerName).NotEmpty().WithMessage("请输入banner名称");
           
            //RuleFor(x => x.bannerUrl).NotEmpty().WithMessage("请输入网址").Url().WithMessage("网址格式错误");
            //RuleFor(x => x.bannerPic).NotEmpty().WithMessage("请上传banner图片");
            // RuleFor(x => x.bannerUrl).NotEmpty().WithMessage("请输入banner链接");
            //RuleFor(x => x.Sorts)
            //    .NotEmpty()
            //    .WithMessage("请输入排序字段")
            //    .GreaterThanOrEqualTo(0)
            //    .WithMessage("请输入大于或等于0的数字");

            RuleFor(x => x.bannerName).NotEmpty();

            RuleFor(x => x.bannerUrl).NotEmpty().WithMessage("Please enter URL").Url().WithMessage("URL format error");
            RuleFor(x => x.bannerPic).NotEmpty();
        }
    }
}
