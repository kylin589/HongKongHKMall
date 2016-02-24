using BrCms.Framework.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.AdminModel.Models.Products
{
    public class SearchSalesRuleModel: Paged
    {
        public string RuleName { get; set; }
    }
}
