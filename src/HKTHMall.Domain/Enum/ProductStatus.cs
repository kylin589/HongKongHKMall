using System.ComponentModel;
using BrCms.Framework.Mvc.Extensions;

namespace HKTHMall.Domain.Enum
{
    /// <summary>
    /// 商品状态
    /// </summary>
    public enum ProductStatus
    {
        /// <summary>
        /// 未提交
        /// </summary>
        [Description("Not submitted")]
        [EnumDescription("Not submitted")]
        Uncommitted = 1,

        /// <summary>
        /// 待审核
        /// </summary>
        [Description("Pending audit")]
        [EnumDescription("Pending audit")]
        Submitting = 2,

        /// <summary>
        /// 审核不通过
        /// </summary>
        [Description("Audit does not pass")]
        [EnumDescription("Audit does not pass")]
        ExaminationNotThrough = 3,

        /// <summary>
        /// 已上架
        /// </summary>
        [Description("Has been built")]
        [EnumDescription("Has been built")]
        HasUpShelves = 4,

        /// <summary>
        /// 已下架
        /// </summary>
        [Description("Has been off the shelf")]
        [EnumDescription("Has been off the shelf")]
        HasUnderShelves = 5
    }

    /// <summary>
    /// 评价等级
    /// </summary>
    public enum CommentLevel
    {
        /// <summary>
        /// 好评
        /// </summary>
        [Description("Good comment")]
        HaoPing = 1,

        /// <summary>
        /// 中评
        /// </summary>
        [Description("General comments")]
        ZhongPing = 2,

        /// <summary>
        /// 差评
        /// </summary>
        [Description("Bad comments")]
        ChaPing = 3
    }
}