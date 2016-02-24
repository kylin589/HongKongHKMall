using BrCms.Framework.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.AdminModel.Models.User
{
    public class SearchZJ_AmountChangeTypeModel:Paged
    {

        public int ID { get; set; }

        /// <summary>
        /// 异动类型名称
        /// </summary>
        public string TypeName { get; set; }

    }
}
