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
    public class BD_NewsInfoModel
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public int TypeID { get; set; }
        public string NewsContent { get; set; }
        public string Releaser { get; set; }
        public long AcUserID { get; set; }
        public long UserID { get; set; }
        public Nullable<System.DateTime> ReleaseDT { get; set; }
        public int IsCheck { get; set; }
        public int IsDelete { get; set; }
        public Nullable<int> SendStatus { get; set; }
        public int IsPic { get; set; }
        public string PicPath { get; set; }
        public int IsHasNaviContent { get; set; }
        public string NaviContent { get; set; }
        public string CreateBy { get; set; }
        public Nullable<System.DateTime> CreateDT { get; set; }
        public string UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateDT { get; set; }

        public string imagePicth;
    }
}
