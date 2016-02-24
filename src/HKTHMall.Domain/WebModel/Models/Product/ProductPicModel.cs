using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Attributes;
using HKTHMall.Domain.WebModel.Validators.Product;

namespace HKTHMall.Domain.WebModel.Models.Product
{
    /// <summary>
    /// 商品图片模型
    /// </summary>
    [Validator(typeof(ProductPicValidator))]
    public class ProductPicModel
    {
        public long ProductPicId { get; set; }
        public long ProductID { get; set; }
        public string PicUrl { get; set; }
        public int Flag { get; set; }
        public long sort { get; set; }
    }
}
