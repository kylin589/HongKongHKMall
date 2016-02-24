using System;
using System.Collections.Generic;
using FluentValidation.Attributes;
using HKTHMall.Domain.AdminModel.Validators.Categories;

namespace HKTHMall.Domain.AdminModel.Models.Categoreis
{
    //[Bind(Exclude = "parentId")]
    [Validator(typeof (UpdateCategoryModelValidator))]
    public class UpdateCategoryModel : CategoryModel
    {
        /// <summary>
        ///     更新人
        /// </summary>
        public string UpdateBy { get; set; }

        /// <summary>
        ///     更新时间
        /// </summary>
        public DateTime UpdateDT { get; set; }
    }

    public class CategoryModel
    {
        /// <summary>
        ///     构造器
        /// </summary>
        public CategoryModel()
        {
            this.Category_Lang = new List<CategoryLanguageModel>();
        }

        /// <summary>
        ///     商品分类Id
        /// </summary>
        public virtual int? CategoryId { get; set; }

        /// <summary>
        ///     icon
        /// </summary>
        public virtual string IconCode { get; set; }

        /// <summary>
        ///     是否启用
        /// </summary>
        public virtual bool AuditState { get; set; }

        /// <summary>
        ///     排序
        /// </summary>
        public virtual long Place { get; set; }

        /// <summary>
        ///     父Id
        /// </summary>
        public virtual int parentId { get; set; }

        /// <summary>
        ///     分类级别
        /// </summary>
        public virtual int Grade { get; set; }

        /// <summary>
        /// 父类级别
        /// </summary>
        public virtual int ParentGrade { get; set; }
        /// <summary>
        ///     分类名称
        /// </summary>
        public virtual string CategoryName { get; set; }

        /// <summary>
        /// 背景颜色
        /// </summary>
        public string BackColor { get; set; }
        /// <summary>
        ///     语言列表
        /// </summary>
        public virtual IList<CategoryLanguageModel> Category_Lang { get; set; }

        //update by liujc
        private CategoryLanguageModel _CategoryLanguageModel;
        /// <summary>
        /// 语言
        /// </summary>
        public CategoryLanguageModel CategoryLanguageModel
        {
            get
            {
                if(_CategoryLanguageModel ==null)
                {
                    return new CategoryLanguageModel();
                }
                return _CategoryLanguageModel;
            }
            set
            {
                _CategoryLanguageModel = value;
            }
        }
        public CategoryLanguageModel GetCategoryLanguageModel(int lang)
        {
            foreach(CategoryLanguageModel m in Category_Lang)
            {
                if (m.LanguageID == lang)
                    return m;
            }
            return this.CategoryLanguageModel;
        }

        public CategoryTypeModel CategoryTypeModel { get; set; }

        public SKU_ProductTypesModel SKU_ProductTypesModel { get; set; }
    }

    public class CategoryTypeModel
    {
        public long CategoryTypeId { get; set; }
        public long SkuTypeId { get; set; }
        public int CategoryId { get; set; }

        public IList<SKU_ProductTypesModel> SKU_ProductTypesModels { get; set; }
    }

    public class SKU_ProductTypesModel
    {
        public long SkuTypeId { get; set; }
        public string Name { get; set; }
        public int? IsGoods { get; set; }
        public int? UseExtend { get; set; }
        public int? UseParameter { get; set; }
        public string Remark { get; set; }
        public int IsUse { get; set; }
        public string CreateBy { get; set; }
        public DateTime? CreateDT { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateDT { get; set; }
    }

    [Validator(typeof (AddCategoryModelValidator))]
    public class AddCategoryModel : CategoryModel
    {
        /// <summary>
        ///     添加人
        /// </summary>
        public string AddUser { get; set; }

        /// <summary>
        ///     添加时间
        /// </summary>
        public DateTime AddDate { get; set; }
    }

    [Validator(typeof (CategoryLanguageValidator))]
    public class CategoryLanguageModel
    {
        /// <summary>
        ///     主键
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///     语言
        /// </summary>
        public int LanguageID { get; set; }

        /// <summary>
        ///     商品分类名称
        /// </summary>
        public string CategoryName { get; set; }

        /// <summary>
        ///     分类Id
        /// </summary>
        public int CategoryId { get; set; }
    }

    public class ChlidernCategoryModel
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }

        public int parentId { get; set; }
        public int ParentCategoryId { get; set; }
        public string ParentCategoryName { get; set; }
        public int ToCategoryId { get; set; }
    }
}