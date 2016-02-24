using BrCms.Framework.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.AdminModel.Models.YHUser
{
    public class SearchReportModel:Paged
    {
        public string Account { get; set; }
        public string ReportTypeName { get; set; }

        public int ? LanguageID { get; set; }
    }
}
