using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.AdminModel.Models.User
{
    public class FeedbackModel
    {
        public int FeedbackId { get; set; }
        public int FeedbackType { get; set; }
        public long UserID { get; set; }
        public string MsgContent { get; set; }
        public System.DateTime FeedbackDate { get; set; }
        public int Source { get; set; }
        public string FeedbackName { get; set; }

        public int LanguageID { get; set; }

        public string Account { get; set; }
    }
}
