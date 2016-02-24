using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.Models.Categoreis
{
    public class Brand_CategoryModel
    {
        public long Brand_CategoryId { get; set; }
        public int BrandID { get; set; }
        public int CategoryId { get; set; }

        public string AddUser { get; set; }
        public DateTime AddDate { get; set; }
        public bool IsEnable { get; set; }
    }
}
