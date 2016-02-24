using BrCms.Framework.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.AdminModel.Models.New
{
    /// <summary>
    /// 新闻条件搜索
    /// </summary>
    public class SearchBD_NewsInfoModel : Paged
    {
        public string Title { get; set; }
        public int ? TypeID { get; set; }
        public string NewsContent { get; set; }
        public string Releaser { get; set; }
        public int? IsCheck { get; set; }
        public int? IsPic { get; set; }
        public int? IsHasNaviContent { get; set; }
        public Nullable<System.DateTime> BeginDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
    }
}
