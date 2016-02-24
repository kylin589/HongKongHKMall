using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.Enum
{
    /// <summary>
    /// 查询列表的类型。综合,销量 ,价格从高到低,价格从低到高
    /// </summary>
    public enum SearchType
    {
        ZongHe = 0,
        Sales = 20,
        PriceDesc = 30,
        PriceAsc=40
    }

    /// <summary>
    /// 排序,升或降
    /// </summary>
    public enum AscOrDescType
    {
        ASC = 0,
        DESC = 10
    }

    /// <summary>
    /// 查询列表的类型。综合,销量 ,价格
    /// </summary>
    public enum SearchField
    {
        ZongHe = 10,
        Sales = 20,
        Price=30    
    }
}
