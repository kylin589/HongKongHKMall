using FluentValidation.Attributes;
using HKTHMall.Domain.AdminModel.Validators.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.AdminModel.Models.Products
{
    /// <summary>
    /// 产品图模型
    /// </summary>
    [Validator(typeof(ProductImageValidator))]
    public class ProductImageModel
    {
        public long ProductImageId { get; set; }
        public string ProductName { get; set; }
        public string ImageUrl { get; set; }
        public int ImageType { get; set; }
        public string linkUrl { get; set; }
        public long place { get; set; }
        public string CreateBy { get; set; }
        public System.DateTime CreateDT { get; set; }
        public string UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateDT { get; set; }
    }
}
