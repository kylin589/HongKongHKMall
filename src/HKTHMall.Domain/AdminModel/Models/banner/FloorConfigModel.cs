using FluentValidation.Attributes;
using HKTHMall.Domain.AdminModel.Validators.banner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.AdminModel.Models.banner
{
    /// <summary>
    /// FloorConfig【楼层配置表】
    /// </summary>
    [Validator(typeof(FloorConfigValidator))]
    public class FloorConfigModel
    {
        /// <summary>
        /// FloorConfigId 1首页
        /// </summary>
        public int FloorConfigId { get; set; }

        /// <summary>
        /// 配置名称
        /// </summary>
        public string FloorConfigName { get; set; }

        /// <summary>
        /// 所属楼层（分类）
        /// </summary>
        public string CateIdStr { get; set; }
    }
}
