using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.WebModel.Models.Balance
{
    public class JoinAndConsumeList
    {
        public decimal Amount { get; set; }
        public long UserID { get; set; }
        public string Account { get; set; }
        public string NickName { get; set; }
        public DateTime RegisterDate { get; set; }

        public string HeadImageUrl { get; set; }
    }
}
