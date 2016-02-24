using BrCms.Framework.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.AdminModel.Models.Keywork
{
    public class SearchFloorKeywordModel: Paged
    {
        public string KeyWordName { get; set; }

        public int LanguageID { get; set; }
    }
}
