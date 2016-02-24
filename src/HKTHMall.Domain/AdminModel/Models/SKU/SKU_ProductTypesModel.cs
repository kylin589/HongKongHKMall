using System;
using System.Collections.Generic;
using FluentValidation.Attributes;
using HKTHMall.Domain.AdminModel.Validators.SKU;
using HKTHMall.Domain.Entities;

namespace HKTHMall.Domain.AdminModel.Models.SKU
{
    [Validator(typeof(SKU_ProductTypesValidator))]
    public class SKU_ProductTypesModel
    {
        public SKU_ProductTypesModel()
        {
            this.SKU_ProductTypeAttributeModel = new List<SKU_ProductTypeAttributeModel>();

            this.StandardAttributeModels = new List<SKU_ProductTypeAttributeModel>();

            this.UseExtendAttributeModels = new List<SKU_ProductTypeAttributeModel>();

            this.UseParamAttributeModels = new List<SKU_ProductTypeAttributeModel>();

            this.SKU_AttributesModels = new List<SKU_AttributesModel>();

            this.SKU_AttributeValuesModels = new List<SKU_AttributeValuesModel>();
        }

        public long SkuTypeId { get; set; }
        public string Name { get; set; }
        public Nullable<int> IsGoods { get; set; }
        public Nullable<int> UseExtend { get; set; }
        public Nullable<int> UseParameter { get; set; }
        public string Remark { get; set; }
        public int IsUse { get; set; }
        public string CreateBy { get; set; }
        public Nullable<System.DateTime> CreateDT { get; set; }
        public string UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateDT { get; set; }

        #region 扩展属性

        public int RowStatus { get; set; }


        public virtual List<SKU_ProductTypeAttributeModel> SKU_ProductTypeAttributeModel { get; set; }


        /// <summary>
        /// 规格属性
        /// </summary>
        public virtual List<SKU_ProductTypeAttributeModel> StandardAttributeModels { get; set; }

        /// <summary>
        /// 扩展参数
        /// </summary>
        public virtual List<SKU_ProductTypeAttributeModel> UseExtendAttributeModels { get; set; }

        /// <summary>
        /// 详细参数
        /// </summary>
        public virtual List<SKU_ProductTypeAttributeModel> UseParamAttributeModels { get; set; }

        public virtual List<SKU_AttributesModel> SKU_AttributesModels { get; set; }

        public virtual List<SKU_AttributeValuesModel> SKU_AttributeValuesModels { get; set; }

        #endregion
    }
}
