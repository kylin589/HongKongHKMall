using FluentValidation.Attributes;
using HKTHMall.Domain.AdminModel.Validators.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.AdminModel.Models.User
{
    [Validator(typeof(ZJ_AmountChangeTypeValidator))]
    public class ZJ_AmountChangeTypeModel
    {
        public int ID { get; set; }

        /// <summary>
        /// 异动类型名称
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
