using BrCms.Framework.Collections;

namespace HKTHMall.Domain.AdminModel.Models.SKU
{
    public class SearchSKU_AttributesModel : Paged
    {
        public string AttributeName { get; set; }

        public int? IsSKU { get; set; }

    }
}
