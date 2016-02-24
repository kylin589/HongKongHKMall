using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using FluentValidation.Attributes;
using HKTHMall.Domain.Validators.SKU;

namespace HKTHMall.Domain.AdminModel.Models.SKU
{
    /// <summary>
    /// 属性模型
    /// </summary>
    [Validator(typeof(SKU_AttributesValidator))]
    public class SKU_AttributesModel
    {

        public SKU_AttributesModel()
        {
            this.SKU_AttributeValuesModels = new List<SKU_AttributeValuesModel>();

        }

        public long AttributeId { get; set; }

        public string AttributeName { get; set; }
        public int AttributeType { get; set; }
        public int IsSKU { get; set; }
        public int UsageMode { get; set; }
        public int IsSearch { get; set; }
        public string Remark { get; set; }
        public string CreateBy { get; set; }
        public Nullable<System.DateTime> CreateDT { get; set; }
        public string UpdateBy { get; set; }

        public int RowStatus { get; set; }
        public Nullable<System.DateTime> UpdateDT { get; set; }

        public List<SKU_AttributeValuesModel> SKU_AttributeValuesModels { get; set; }

        public string ValuesString { get; set; }



        private List<SelectListItem> _AttributeTypeList;

        public List<SelectListItem> AttributeTypeList
        {
            get
            {
                return new List<SelectListItem>()
                {
                    new SelectListItem() {Text = "Word", Value = "0", Selected = this.AttributeType == 0},
                    new SelectListItem() {Text = "Picture", Value = "1", Selected = this.AttributeType == 1}
                };
            }
        }

    }
}
