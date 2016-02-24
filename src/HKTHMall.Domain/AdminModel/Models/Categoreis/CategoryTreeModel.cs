using System.Collections.Generic;

namespace HKTHMall.Domain.AdminModel.Models.Categoreis
{
    public class CategoryTreeModel
    {
        public int CategoryId { get; set; }
        public int ParentId { get; set; }
        public string text { get; set; }
        public string CategoryName { get; set; }
        public List<CategoryTreeModel> nodes { get; set; }
    }
}
