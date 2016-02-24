using BrCms.Framework.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.AdminModel.Products
{
    public class SearchProductConsultModel : Paged
    {
        public long ProductId { get; set; }

        public long UserID { get; set; }

        public long ProductConsultId { get; set; }

        public string Phone { get; set; }
    }
}
