using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.WebModel.Models.Bank
{
    public class BankModel
    {
        public int BankID { get; set; }
        public string BankName { get; set; }
        public string BankLogoURL { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDT { get; set; }
        public string UpdateBy { get; set; }
        public DateTime UpdateDT { get; set; }
    }
}
