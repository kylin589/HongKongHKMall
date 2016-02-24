using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.WebModel.Models.Product
{
    public class ProductSkuItem
    {
        public long arrGoodsId { get; set; }

        public long arrSkuNumber { get; set; }

        public int arrCount { get; set; }

        public decimal arrPrice { get; set; }

    }
}
