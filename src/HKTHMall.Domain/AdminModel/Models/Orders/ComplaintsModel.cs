using FluentValidation.Attributes;
using HKTHMall.Domain.Entities;
using HKTHMall.Domain.Validators.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.Models.Orders
{
    [Validator(typeof(ComplaintsValidator))]
    public class ComplaintsModel
    {
        public string ComplaintsID { get; set; }
        public string OrderID { get; set; }
        public long MerchantID { get; set; }
        public long UserID { get; set; }

        public string NickName { get; set; }
        public string Phone { get; set; }

        public string Email { get; set; }

        public string ShopName { get; set; }
        public int complainType { get; set; }
        public string Content { get; set; }
        public System.DateTime ComplaintsDate { get; set; }
        public string DealPeople { get; set; }
        public Nullable<System.DateTime> DealDate { get; set; }
        public int Flag { get; set; }
        public string Comments { get; set; }

        public virtual Order Order { get; set; }

        public AC_User User { get; set; }

        public YH_MerchantInfo MerchantInfo { get; set; }
    }
}
