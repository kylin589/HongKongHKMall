using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.WebModel.Models.Login
{
    public class YH_PasswordErrorModel
    {
        public int ID { get; set; }
        public long UserID { get; set; }
        public string Account { get; set; }
        public System.DateTime VerifyTime { get; set; }
        public int FailVerifyTimes { get; set; }
        public int PassWordType { get; set; }
    }
}
