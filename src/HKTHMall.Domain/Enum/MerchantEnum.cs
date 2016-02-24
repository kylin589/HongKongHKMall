using BrCms.Framework.Mvc.Extensions;

namespace HKTHMall.Domain.Enum
{
    public enum MerchantEnum
    {
        /// <summary>
        /// 企业商铺
        /// </summary>
        [EnumDescription("企业商铺")]
        Store = 1,
        /// <summary>
        /// 个体商铺
        /// </summary>
        [EnumDescription("个体商铺")]
        Individual = 2
    }
}