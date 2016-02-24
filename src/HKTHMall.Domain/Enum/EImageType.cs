using BrCms.Framework.Mvc.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.Enum
{
    /// <summary>
    /// 图片类型:1)图片；2）视频
    /// <remarks>added by jimmy,2015-7-28</remarks>
    /// </summary>
    public enum EImageType
    {
        /// <summary>
        /// 图片
        /// </summary>
        [EnumDescription("Picture", 1)]
        Image = 1,

        /// <summary>
        /// 视频
        /// </summary>
        [EnumDescription("Video", 2)]
        Video = 2
    }
}
