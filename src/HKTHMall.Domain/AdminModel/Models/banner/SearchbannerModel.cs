using BrCms.Framework.Collections;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.Models.banner
{
    public class SearchbannerModel : Paged
    {
        /// <summary>
        /// 位置（分类）
        /// </summary>
        [Display(Name = "Position")]
        public int PlaceCode { get; set; }

        /// <summary>
        /// 标识ID
        /// </summary>
        [Display(Name = "Identification Name")]
        public int IdentityStatus { get; set; }

        /// <summary>
        /// 商品ID
        /// </summary>
        public long ProductId { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        [Display(Name = "Sorts")]
        public long Sorts { get; set; }
    }
}
