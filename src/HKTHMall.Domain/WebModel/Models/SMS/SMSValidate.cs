using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.WebModel.Models.SMS
{
    public class SMSValidate
    {
        public long UserID { get; set; }
        public string UserIP { get; set; }
        public string VerifyCode { get; set; }
        public DateTime VerfyTime { get; set; }
        public int IsUse { get; set; }
        public string Phone { get; set; }
        public int VerfyType { get; set; }
        public DateTime SendTime { get; set; }
    }
}
