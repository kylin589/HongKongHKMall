using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.WebModel.Models.Product
{
    /// <summary>
    /// 商品语言包
    /// </summary>
    public class Product_LangView
    {
        public int Id { get; set; }

        public long ProductId { get; set; }

        public string ProductName { get; set; }

        public int LanguageID { get; set; }
    }
}
