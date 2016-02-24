using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.Attributes;
using HKTHMall.Domain.Validators.banner;

namespace HKTHMall.Domain.Models.banner
{
   
    public class bannerMidModel
    {
        /// <summary>
        /// bannerId
        /// </summary>
        public long bannerId { get; set; }

        /// <summary>
        /// 商品ID
        /// </summary>
        [Display(Name = "ProductId")]
        public long ProductId { get; set; }

        /// <summary>
        /// banner名称
        /// </summary>
       [Display(Name = "bannerName")]
        public string bannerName { get; set; }

        /// <summary>
       /// banner链接
        /// </summary>
        [Display(Name = "bannerUrl")]
        public string bannerUrl { get; set; }

        /// <summary>
        /// banner图片地址
        /// </summary>
        [Display(Name = "bannerPic")]
        public string bannerPic { get; set; }

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
        /// 排序
        /// </summary>
        [Display(Name = "Sorts")]
        public long Sorts { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        [Display(Name = "Create Name")]
        public string CreateBy { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Display(Name = "Create Time")]
        public Nullable<System.DateTime> CreateDT { get; set; }

        /// <summary>
        /// 修改人
        /// </summary>
        [Display(Name = "Update Name")]
        public string UpdateBy { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        [Display(Name = "Update Time")]
        public Nullable<System.DateTime> UpdateDT { get; set; }

        public int Status { get; set; }

        
    }
}
