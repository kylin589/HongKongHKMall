using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Validators;
using HKTHMall.Domain.AdminModel.Models.SKU;
using HKTHMall.Domain.Entities;

namespace HKTHMall.Domain.Validators.SKU
{
    /// <summary>
    /// 属性模型验证
    /// </summary>
    public class SKU_AttributesValidator : AbstractValidator<SKU_AttributesModel>
    {
        public SKU_AttributesValidator()
        {
            RuleFor(x => x.AttributeName).NotEmpty().WithMessage("Please enter specification name").SetValidator(new GBLengthValidator(40, "Specifications Name character length limit of 40 characters！"));
            //RuleFor(x => x.SKU_AttributeValuesModels).SetValidator(new ListItemsValidator<SKU_AttributeValuesModel>(1, "请输入规格值"));
            //RuleFor(x => x.AttributeName).Matches("^(X|Y)?$").WithMessage()

        }

        /// <summary>
        /// 集合验证器
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public class ListItemsValidator<T> : PropertyValidator
        {
            public int MinCount { get; set; }

            public ListItemsValidator(int minCount, string errorMessage)
                : base(errorMessage)
            {
                this.MinCount = minCount;
            }

            protected override bool IsValid(PropertyValidatorContext context)
            {
                var list = context.PropertyValue as IList<T>;
                if (list == null || list.Count < this.MinCount)
                {
                    return false;
                }
                return true;
            }
        }

        /// <summary>
        /// 中文长度验证器
        /// </summary>
        public class GBLengthValidator : PropertyValidator
        {
            public int MaxLength { get; set; }
            public GBLengthValidator(int maxLength, string errorMessage)
                : base(errorMessage)
            {
                this.MaxLength = maxLength;
            }

            /// <summary>
            /// 是否通过验证
            /// </summary>
            /// <param name="context"></param>
            /// <returns></returns>
            protected override bool IsValid(PropertyValidatorContext context)
            {

                string value = context.PropertyValue == null ? "" : context.PropertyValue.ToString();
                int length = GBLengthValidator.GetLength(value);
                return length <= MaxLength;

            }

            /// <summary>
            /// 获取字符串长度
            /// </summary>
            /// <param name="str"></param>
            /// <returns></returns>
            public static int GetLength(string str)
            {
                if (str.Length == 0)
                {
                    return 0;
                }
                ASCIIEncoding ascii = new ASCIIEncoding();
                int tempLen = 0; byte[] s = ascii.GetBytes(str);
                for (int i = 0; i < s.Length; i++)
                {
                    if ((int)s[i] == 63)
                    {
                        tempLen += 2;
                    }
                    else
                    {
                        tempLen += 1;
                    }
                }
                return tempLen;
            }

        }


    }
}
