using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.AdminModel.Models.AC
{
    /// <summary>
    /// 服务异常信息表
    /// zhoub 20150902
    /// </summary>
    public class ExceptionLogModel
    {
        public int ElId { get; set; }

        public string HandleId { get; set; }
        public string Message { get; set; }
        public string ServiceName { get; set; }
        public int Status { get; set; }
        public string CreateBy { get; set; }
        public DateTime? CreateDT { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateDT { get; set; }
        public string UpdateResult { get; set; }
        public int ResultType { get; set; }
    }
}
