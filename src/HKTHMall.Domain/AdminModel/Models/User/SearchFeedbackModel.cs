using BrCms.Framework.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.AdminModel.Models.User
{
    public class SearchFeedbackModel:Paged
    {
        public string FeedbackName { get; set; }

        public int LanguageID { get; set; }

        public string Account { get; set; }

        public int? Source { get; set; }
    }
}
