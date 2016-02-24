using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.WebModel.Models.Users
{
    public class FeedbackView
    {
        public int FeedbackId { get; set; }
        public int FeedbackType { get; set; }
        public long UserID { get; set; }
        public string MsgContent { get; set; }
        public DateTime FeedbackDate { get; set; }
        public int Source { get; set; }
    }
}
