using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.AdminModel.Models.User
{
    public class MessageModel
    {
        public int MsgId { get; set; }
        public string MsgPerson { get; set; }
        public string Email { get; set; }
        public string subject { get; set; }
        public string MsgContent { get; set; }
        public System.DateTime CreateDT { get; set; }
    }
}
