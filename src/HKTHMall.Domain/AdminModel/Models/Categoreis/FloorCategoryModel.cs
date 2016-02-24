using FluentValidation.Attributes;
using HKTHMall.Domain.AdminModel.Validators.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.AdminModel.Models.Categoreis
{
    [Validator(typeof(FloorCategoryValidator))]
    public class FloorCategoryModel
    {
        public long FloorCategoryId { get; set; }
        public int CategoryId { get; set; }

        public int CategoryIdSecond { get; set; }
        public int DCategoryId { get; set; }

        public string CategoryNameFirst { get; set; }
        public string CategoryNameThree { get; set; }
        public long Place { get; set; }
        public string AddUsers { get; set; }
        public System.DateTime AddTime { get; set; }

        public string navigationName { get; set; }
    }
}
