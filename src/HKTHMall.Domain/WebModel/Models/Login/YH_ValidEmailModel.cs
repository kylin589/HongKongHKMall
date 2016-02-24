using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.WebModel.Models.Login
{
    public class YH_ValidEmailModel
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public string Email { get; set; }
        public System.DateTime PostDT { get; set; }
        public string SerialNumber { get; set; }
        public int IsValid { get; set; }
        public int EmailType { get; set; }
    }
}
