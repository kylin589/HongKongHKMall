
using System;
namespace HKTHMall.Domain.AdminModel.Models.Categoreis
{
    public class ResultCategoryModel : CategoryModel
    {
        /// <summary>
        ///     添加人
        /// </summary>
        public string AddUser { get; set; }

        /// <summary>
        ///     添加时间
        /// </summary>
        public DateTime AddDate { get; set; }

        public string UpdateBy { get; set; }
        public DateTime UpdateDT { get; set; }
    }

    public class SearchCategoryModel
    {
        /// <summary>
        ///     语言
        /// </summary>
        public int LanguageId { get; set; }

        /// <summary>
        ///     分类级别
        /// </summary>
        public int Grade { get; set; }
    }
}