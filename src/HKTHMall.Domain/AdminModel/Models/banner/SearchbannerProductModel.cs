using BrCms.Framework.Collections;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.Models.banner
{
    public class SearchbannerProductModel : Paged
    {
        /// <summary>
        /// 位置（分类）
        /// </summary>
        [Display(Name = "Position Name")]
        public int PlaceCode { get; set; }

        /// <summary>
        /// 标识ID
        /// </summary>
        [Display(Name = "Identification Name")]
        public int IdentityStatus { get; set; }

        public int LanguageID { get; set; }

        public long productId { get; set; }

        public long bannerProductId { get; set; }

    }
}
