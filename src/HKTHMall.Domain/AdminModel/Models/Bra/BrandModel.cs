using FluentValidation.Attributes;
using HKTHMall.Domain.Entities;
using HKTHMall.Domain.Validators.Bra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.Models.Bra
{
    /// <summary>
    /// 商品品牌类
    /// </summary>
    [Validator(typeof(BrandValidator))]
    public class BrandModel
    {
        public BrandModel() {
            this.Brand_Category = new List<Brand_Category>();
            this.Brand_Lang = new List<Brand_Lang>();
        }
        public int BrandID { get; set; }
        public string BrandUrl { get; set; }
        public string Remark { get; set; }
        public int BrandState { get; set; }
        public string AddUsers { get; set; }
        public System.DateTime AddTime { get; set; }
        public string UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateDT { get; set; }
        public string FirstPY { get; set; }

        public string BrandName { get; set; }
        public int LanguageID { get; set; }

        public string ZhBrandName { get; set; }

        public string EnBrandName { get; set; }

        public string HongkongBrandName { get; set; }//add by liujc

        public string TaiBrandName { get; set; }
        public List<Brand_Category> Brand_Category { get; set; }
        public List<Brand_Lang> Brand_Lang { get; set; }

        public Brand_Lang Brand_LangModel { get; set; }
    }

    public class BrandAdvertiseModel
    {
        public int BrandID{get;set;}
        public string BrandName{get;set;}
        public string PicUrl { get; set; }
    }
}
