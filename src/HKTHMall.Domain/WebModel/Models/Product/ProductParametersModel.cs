using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.WebModel.Models.Product
{
    public class ProductParametersModel
    {
        public long? ParametersId { get; set; }
        public long ProductId { get; set; }
        public string PName { get; set; }
        public string PValue { get; set; }
        public string GroupName { get; set; }
        public long Sort { get; set; }
    }
}
