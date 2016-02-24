using BrCms.Framework.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.AdminModel.Models.Categoreis
{
    public class SearchFloorCategoryModel : Paged
    {
        public int CategoryId { get; set; }
        public int ThreeCategoryId { get; set; }
        public int LanguageID { get; set; }

        public int ParentID { get; set; }
    }
}
