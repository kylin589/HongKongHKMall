using BrCms.Framework.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.AdminModel.Models.User
{
    public class SearchYH_UserBankAccountModel : Paged
    {
        public string Account { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public int IsUse { get; set; }
    }
}
