using BrCms.Framework.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.Models.Bra
{
    public class SearchBrandModel : Paged
    {
        public string BrandName { get; set; }

        public int BrandID { get; set; }

        public int LanguageID { get; set; }

        public int BrandState { get; set; }
    }
}
