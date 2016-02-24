using System;
using HKTHMall.Domain.Entities;

namespace HKTHMall.Domain.AdminModel.Models.SKU
{


    public class SKU_ProductTypeAttributeModel
    {
        public long SKU_ProductTypeAttributeId { get; set; }
        public long AttributeId { get; set; }
        public long SkuTypeId { get; set; }
        public int AttributeType { get; set; }
        public string AttributeGroup { get; set; }
        public long DisplaySequence { get; set; }
        public string CreateBy { get; set; }
        public Nullable<System.DateTime> CreateDT { get; set; }
        public string UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateDT { get; set; }

        public virtual SKU_AttributesModel SKU_AttributesModel { get; set; }
        public virtual SKU_ProductTypesModel SKU_ProductTypesModel { get; set; }
        
        public int RowStatus { get; set; }



    }
}
