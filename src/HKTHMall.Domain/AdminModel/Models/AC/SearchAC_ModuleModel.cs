using BrCms.Framework.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.Models.AC
{
    public class SearchAC_ModuleModel: Paged
    {
        public string ModuleName { get; set; }

        public int ParentID { get; set; }
    }
}
