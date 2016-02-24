using FluentValidation.Attributes;
using HKTHMall.Domain.AdminModel.Validators.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.AdminModel.Models.YHUser
{
      [Validator(typeof(ReportValidator))]
    public class ReportModel
    {
        public long ReportId { get; set; }
        public long ReportUserId { get; set; }
        public long BeReporteId { get; set; }
        public int ReportTypeId { get; set; }
        public string ReportContent { get; set; }
        public System.DateTime CreateDT { get; set; }
        public int Status { get; set; }
        public string Result { get; set; }
        public string DealUser { get; set; }
        public Nullable<System.DateTime> DealDate { get; set; }

        /// <summary>
        /// 举报类型
        /// </summary>
        public string ReportTypeName { get; set; }

        public int LanguageID { get; set; }

        /// <summary>
        /// 举报人
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 被举报人
        /// </summary>
        public string ReportedName { get; set; }
    }
}
