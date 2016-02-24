using BrCms.Framework.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.AdminModel.Models.User
{
    /// <summary>
    /// 留言搜索条件
    /// <remarks>added by jimmy,2015-7-27</remarks>
    /// </summary>
    public class SearchMessageModel : Paged
    {
        /// <summary>
        /// 留言人
        /// </summary>
        public string MsgPerson { get; set; }

        /// <summary>
        /// 留言主题
        /// </summary>

        public string subject { get; set; }
    }
}
