using FluentValidation;
using HKTHMall.Domain.AdminModel.Models.APP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.AdminModel.Validators.App
{
    public class APP_VersionInfoValidator: AbstractValidator<APP_VersionInfoModel>
    {

        public APP_VersionInfoValidator()
        {
            //RuleFor(x => x.APPName).NotEmpty().WithMessage("请输应用名称");
            //RuleFor(x => x.DownloadURL).NotEmpty().WithMessage("请上传文件");
            //RuleFor(x => x.PackageName).NotEmpty().WithMessage("请输入包名");
            //RuleFor(x => x.Platform).NotEmpty().WithMessage("请输入APP平台");
            //RuleFor(x => x.UpdateInfo).NotEmpty().WithMessage("请输入更新描述内容");
            //RuleFor(x => x.VersionName).NotEmpty().WithMessage("请输入版本名称");
            //RuleFor(x => x.VersionNO).NotEmpty().WithMessage("请输入版本编号");

            RuleFor(x => x.APPName).NotEmpty();
            RuleFor(x => x.DownloadURL).NotEmpty();
            RuleFor(x => x.PackageName).NotEmpty();
            RuleFor(x => x.Platform).NotEmpty();
            RuleFor(x => x.UpdateInfo).NotEmpty();
            RuleFor(x => x.VersionName).NotEmpty();
            RuleFor(x => x.VersionNO).NotEmpty();
        }
    }
}
