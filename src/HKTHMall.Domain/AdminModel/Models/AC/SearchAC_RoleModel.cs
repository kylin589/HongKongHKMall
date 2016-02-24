using BrCms.Framework.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.Models.AC
{
    public class SearchAC_RoleModel : Paged
    {
        /// <summary>
        /// 角色名称
        /// </summary>
        public string RoleName { get; set; }
    }
}
