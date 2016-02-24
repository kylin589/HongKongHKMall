using BrCms.Framework.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.AdminModel.Models.AC
{
    public class SearchExceptionLogModel : Paged
    {
        public long ElId { get; set; }
        public string ServiceName { get; set; }
        public DateTime? BeginOperateTime { get; set; }
        public DateTime? EndOperateTime { get; set; }
        public string HandleId { get; set; }
        public int Status { get; set; }
        public int ResultType { get; set; }
    }
}
