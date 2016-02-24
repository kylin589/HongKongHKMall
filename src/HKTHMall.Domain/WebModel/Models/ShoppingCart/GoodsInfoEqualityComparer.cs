using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.WebModel.Models.ShoppingCart
{
    /// <summary>商品Id比较器.</summary>
    /// <remarks></remarks>
    /// <author>PanYun HX1501345 2015-04-20 09:46:32</author>
    public class GoodsInfoEqualityComparer : IEqualityComparer<ShoppingCartModel>
    {
        public bool Equals(ShoppingCartModel x, ShoppingCartModel y)
        {
            return (String.Equals(x.ProductID.ToString(), y.ProductID.ToString(), StringComparison.OrdinalIgnoreCase) && String.Equals(x.SKU_ProducId.ToString(), y.SKU_ProducId.ToString(), StringComparison.OrdinalIgnoreCase));
        }

        public int GetHashCode(ShoppingCartModel obj)
        {
            //return new Random().Next(1, 1000000);
            return obj.ProductID.GetHashCode();
        }
    }
}
