using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.Attributes;
using HKTHMall.Domain.Validators.banner;
using System.ComponentModel.DataAnnotations;

namespace HKTHMall.Domain.Models.banner
{
    [Validator(typeof(bannerProductValidator))]
    public class bannerProductModel
    {
        /// <summary>
        /// 广告商品
        /// </summary>
        [Display(Name = "bannerProductID")]
        public long bannerProductId { get; set; }

        /// <summary>
        /// 商品ID 
        /// </summary>
        //[Display(Name = "商品ID")]
        public long productId { get; set; }

        /// <summary>
        /// 位置（分类）
        /// </summary>
        
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

        /// <summary>
        /// 商品广告图片地址
        /// </summary>
        [Display(Name = "PicAddress")]
        public string PicAddress { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 商品价格
        /// </summary>
        public decimal HKPrice { get; set; }

        /// <summary>
        /// 语言种类,1中文
        /// </summary>
        public int LanguageID { get; set; }

        /// <summary>
        /// 分类ID
        /// </summary>
        public long CategoryId { get; set; }

        /// <summary>
        /// 商品类型
        /// </summary>
        public int Status { get; set; }
    }
}
