using FluentValidation.Attributes;
using HKTHMall.Domain.AdminModel.Validators.New;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.AdminModel.Models.New
{
    [Validator(typeof(BD_NewsInfoValidator))]
    public class APP_NewsInfoModel
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public int TypeID { get; set; }
        public string NaviContent { get; set; }

        public int SendStatus { get; set; }
    }
}
