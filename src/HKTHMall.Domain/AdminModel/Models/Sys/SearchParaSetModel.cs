using BrCms.Framework.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.Models.Sys
{
    public class SearchParaSetModel:Paged
    {
        /// <summary>
        /// 键名称
        /// </summary>
        public string KeysName { get; set; }
    }
}
