using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.WebModel.Models.Users
{
    public class FeedbackTypeLangView
    {
        public int FeedbackType_langId { get; set; }
        public int FeedbackTypeId { get; set; }
        public string FeedbackName { get; set; }
        public int LanguageID { get; set; }
    }
}
