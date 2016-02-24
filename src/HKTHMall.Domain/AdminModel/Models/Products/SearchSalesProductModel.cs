using BrCms.Framework.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.AdminModel.Models.Products
{
    public class SearchSalesProductModel : Paged
    {
        public string ProductName { get; set; }

        public int LanguageID { get; set; }
    }
}
