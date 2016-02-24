using HKTHMall.Domain.Entities;

namespace HKTHMall.Domain.AdminModel.Models.SKU
{
    public partial class SKU_SKUItemsModel
    {
        public long SKU_SKUItemsId { get; set; }
        public long ProductId { get; set; }
        public long AttributeId { get; set; }
        public long ValueId { get; set; }
        public string ValueStr { get; set; }
    
        public virtual Product Product { get; set; }
    }
}
