namespace HKTHMall.Domain.AdminModel.Models.SKU
{
    public class SKU_AttributeValuesModel
    {

        public SKU_AttributeValuesModel()
        {
            
        }

        public long ValueId { get; set; }
        public long AttributeId { get; set; }
        public int DisplaySequence { get; set; }
        public string ValueStr { get; set; }
        public string ImageUrl { get; set; }
        public string ValuesGroup { get; set; }

        public int RowStatus { get; set; }

    }
}
