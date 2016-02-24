using System;
using System.Collections.Generic;
using System.Linq;
using HKTHMall.Domain.AdminModel.Models.Categoreis;
using HKTHMall.Domain.Models;
using HKTHMall.Domain.WebModel.Models.Product;
using HKTHMall.Core;

namespace HKTHMall.Services.Products
{
    /// <summary>
    ///     产品类别
    /// </summary>
    public class CategoryService : BaseService, ICategoryService
    {
        /// <summary>
        ///     获取类型详细信息
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public ResultModel GetCategoryInfoById(int categoryId)
        {
            var result = new ResultModel
            {
                Data = this._database.Db.Category.Get(categoryId)
            };

            return result;
        }

        /// <summary>
        ///     添加类别
        /// </summary>
        /// <param name="model">类别模型</param>
        public ResultModel AddCategory(AddCategoryModel model)
        {
            var result = new ResultModel();
            using (var tx = this._database.Db.BeginTransaction())
            {
                try
                {
                    var category = tx.Category.Insert(model);
                    if (model.Category_Lang != null && model.Category_Lang.Count != 0)
                    {
                        foreach (var lang in model.Category_Lang)
                        {
                            lang.CategoryId = category.CategoryId;
                        }
                        tx.Category_Lang.Insert(model.Category_Lang);
                    }

                    if (model.Grade == 3)
                    {
                        if (model.SKU_ProductTypesModel != null)
                        {
                            model.CategoryTypeModel.CategoryId = category.CategoryId;
                            model.CategoryTypeModel.SkuTypeId = model.SKU_ProductTypesModel.SkuTypeId;
                            tx.CategoryType.Insert(model.CategoryTypeModel);
                        }
                    }

                    tx.Commit();
                }
                catch
                {
                    tx.Rollback();
                    throw;
                }
            }

            return result;
        }

        /// <summary>
        ///     根据类别id获取类别
        /// </summary>
        /// <param name="id">类别id</param>
        /// <returns>类别模型</returns>
        public ResultModel GetCategoryById(int id)
        {
            var result = new ResultModel();

            result.Data = this._database.RunQuery(db =>
            {
                dynamic cl, ct, spt;
                var data = db.Category
                    .FindAllByCategoryId(id)
                    .LeftJoin(db.Category_Lang, out cl)
                    .On(cl.CategoryId == db.Category.CategoryId)
                    .LeftJoin(db.CategoryType, out ct)
                    .On(ct.CategoryId == db.Category.CategoryId)
                    .LeftJoin(db.SKU_ProductTypes.As("SKU_ProductTypesModel"), out spt)
                    .On(spt.SkuTypeId == ct.SkuTypeId)
                    .WithMany(cl)
                    .WithMany(ct)
                    .WithOne(spt)
                    .ToList<CategoryModel>();
                return data;
            });

            return result;
        }

        /// <summary>
        ///     根据分类级别获取分类列表
        /// </summary>
        /// <param name="grade">分类级别</param>
        /// <param name="languageId">语言Id</param>
        /// <returns>类别列表</returns>
        public ResultModel GetCategoryByGrade(int grade, int languageId)
        {
            var result = new ResultModel();

            dynamic cl;

            result.Data = this._database.Db.Category
                .Query()
                .LeftJoin(this._database.Db.Category_Lang.As("CategoryLanguageModel"), out cl)
                .On(cl.CategoryId == this._database.Db.Category.CategoryId && cl.LanguageId == languageId)
                .WithOne(cl)
                .Where(this._database.Db.Category.grade == grade)
                .ToList<CategoryModel>();

            return result;
        }

        /// <summary>
        ///     根据级id获取子列表
        /// </summary>
        /// <param name="parentId">父Id</param>
        /// <param name="languageId">语言</param>
        /// <returns>列表</returns>
        public ResultModel GetCategoryByParentId(int parentId, int languageId)
        {
            var result = new ResultModel();

            dynamic cl;

            result.Data = this._database.Db.Category
                .Query()
                .LeftJoin(this._database.Db.Category_Lang.As("CategoryLanguageModel"), out cl)
                .On(cl.CategoryId == this._database.Db.Category.CategoryId && cl.LanguageId == languageId)
                .WithOne(cl)
                .Where(this._database.Db.Category.parentId == parentId).Where(this._database.Db.Category.AuditState == 1)
                .ToList<CategoryModel>();

            return result;
        }

        /// <summary>
        ///     获取类别列表
        /// </summary>
        /// <returns>类别列表</returns>
        public ResultModel GetCategoriesByCategoryToTree(int languageId, int parentId=0)
        {
            dynamic cl;

            var data = this._database.Db.Category
                .Query()
                .LeftJoin(this._database.Db.Category_Lang, out cl)
                .On(cl.CategoryId == this._database.Db.Category.CategoryId && cl.languageId == languageId)
                .Select(
                    this._database.Db.Category.CategoryId.As("id")
                    , cl.CategoryName.As("text")
                    , cl.languageId
                    , this._database.Db.Category.parentId
                    , this._database.Db.Category.Grade
                ).Where(this._database.Db.Category.AuditState==1)
                .ToList();

            return new ResultModel { Data = CreateTree(data, parentId) };
        }      
        /// <summary>
        ///     更新类别
        /// </summary>
        /// <param name="model">类别模型</param>
        /// <returns>是否修改成功</returns>
        public ResultModel UpdateCategory(UpdateCategoryModel model)
        {
            var result = new ResultModel();

            using (var tx = this._database.Db.BeginTransaction())
            {
                try
                {
                    tx.Category.Update(model);
                    foreach (var languageModel in model.Category_Lang)
                    {
                        languageModel.CategoryId = model.CategoryId.Value;
                        if (languageModel.Id != 0)
                        {
                            tx.Category_Lang.Update(languageModel);
                        }
                        else
                        {
                            tx.Category_Lang.Insert(languageModel);
                        }
                    }

                    tx.Commit();
                }
                catch (Exception)
                {
                    tx.Rollback();
                    throw;
                }
            }


            return result;
        }

        /// <summary>
        ///     根据父ID获取商品分类
        ///     zhoub 20150708 
        /// </summary>
        /// <param name="languageId">语言ID</param>
        /// <param name="parentId">父ID</param>
        /// <returns></returns>
        /// <remarks>modified by jimmy,2015-8-13</remarks>
        public List<CategoryModel> GetCategoriesByParentId(int languageId, int parentId)
        {
            string key = "GetCategoriesByParentId" + parentId;
            List<CategoryModel> list = this._database.Db.Category.All()
                .LeftJoin(this._database.Db.Category_Lang)
                .On(this._database.Db.Category.CategoryId == this._database.Db.Category_Lang.CategoryId)
                .Where(this._database.Db.Category.parentId == parentId &&
                       this._database.Db.Category_Lang.LanguageID == languageId&&
                       this._database.Db.Category.AuditState == 1)
                .Select(this._database.Db.Category.CategoryId, this._database.Db.Category_Lang.CategoryName)
                .OrderByPlace()
                .ToList<CategoryModel>();
            MemCacheFactory.GetCurrentMemCache().AddCache(key, list, 2);
            return list;
        }

        /// <summary>
        ///     根据父ID获取(启用)商品分类
        ///     wuyf 20150708
        /// </summary>
        /// <param name="languageId">语言ID</param>
        /// <param name="parentId">父ID</param>
        /// <returns></returns>
        public ResultModel GetCategoriesByParentIdAuditState(int languageId, int parentId)
        {
            var result = new ResultModel();
            result.Data  = this._database.Db.Category.All()
                .LeftJoin(this._database.Db.Category_Lang)
                .On(this._database.Db.Category.CategoryId == this._database.Db.Category_Lang.CategoryId)
                .Where(this._database.Db.Category.parentId == parentId &&
                       this._database.Db.Category_Lang.LanguageID == languageId && this._database.Db.Category.AuditState == 1)
                .Select(this._database.Db.Category.CategoryId, this._database.Db.Category_Lang.CategoryName)
                .OrderByPlace()
                .ToList<CategoryModel>();
            return result;
        }

        /// <summary>
        ///     根据语言类型获取全部的可以使用的数据
        /// </summary>
        /// <param name="languageId"></param>
        /// <param name="dCategoryId"></param>
        /// <returns></returns>
        public ResultModel GetCategoriesByALL(int languageId, int dCategoryId)
        {
            var result = new ResultModel();
            result.Data = this._database.Db.FloorCategory.All()
                .Select(this._database.Db.Category_Lang.CategoryName, this._database.Db.Category_Lang.CategoryId)
                .LeftJoin(this._database.Db.Category_Lang, CategoryId: this._database.Db.FloorCategory.CategoryId)
                .Where(this._database.Db.Category_Lang.LanguageID == languageId &&
                       this._database.Db.FloorCategory.DCategoryId == dCategoryId)
                .OrderBy(this._database.Db.FloorCategory.Place)
                .ToList<CategoryModel>();
            return result;
        }

        /// <summary>
        ///     获取全部分类
        /// </summary>
        /// <param name="languageId">语言</param>
        /// <returns></returns>
        public ResultModel GetAll(int languageId)
        {
            var result = new ResultModel();
            result.Data = this._database.RunQuery(db =>
            {
                dynamic lang;
                var data = db.Category.Query()
                    .LeftJoin(db.Category_Lang.As("CategoryLanguageModel"), out lang)
                    .On(lang.CategoryId == db.Category.CategoryId && lang.LanguageID == languageId)
                    .WithOne(lang)
                    .OrderByDescending(db.Category.Place)
                    .ToList<ResultCategoryModel>();
                return data;
            });
            return result;
        }

        /// <summary>
        ///     根据分类Id和语言Id获取类别
        /// </summary>
        /// <param name="cateId"></param>
        /// <param name="languageId"></param>
        /// <returns></returns>
        public ResultModel GetCateById(int cateId, int languageId)
        {
            var result = new ResultModel();
            var z = this._database.Db.Category
                .Query().LeftJoin(this._database.Db.Category_Lang, CategoryId: this._database.Db.Category.CategoryId);
            z = z.Select(
                this._database.Db.Category.CategoryId, this._database.Db.Category.parentId, this._database.Db.Category.Place
                , z.Category_Lang.CategoryName, z.Category_Lang.LanguageID);
            z = z.Where(z.Category_Lang.LanguageID == languageId).Where(this._database.Db.Category.CategoryId == cateId);
            result.Data = z.ToList<CategorysModel>();
            return result;
        }
        /// <summary>
        ///     根据分类Id和语言Id获取类别
        /// </summary>
        /// <param name="cateId"></param>
        /// <param name="languageId"></param>
        /// <returns></returns>
        public ResultModel GetCateByIdForApi(int cateId, int languageId)
        {
            var result = new ResultModel();
            var z = this._database.Db.Category
                .Query().LeftJoin(this._database.Db.Category_Lang, CategoryId: this._database.Db.Category.CategoryId);
            z = z.Select(
                this._database.Db.Category.CategoryId, this._database.Db.Category.parentId, this._database.Db.Category.Place
                , z.Category_Lang.CategoryName, z.Category_Lang.LanguageID);
            z = z.Where(z.Category_Lang.LanguageID == languageId).Where(this._database.Db.Category.CategoryId == cateId).Where(this._database.Db.Category.AuditState == 1);
            result.Data = z.ToList<CategorysModel>();
            return result;
        }

        private void CreateTree1(List<ResultCategoryModel> categories, int parentId, List<ResultCategoryModel> categories1)
        {
            var list = categories.FindAll(m => m.parentId == parentId) ;
            categories1.AddRange(list);
            for (int i = 0; i < list.Count; i++)
			{
                CategoryModel model=list[i];
                CreateTree1(categories,Convert.ToInt32(model.CategoryId), categories1);
			}
            

            
        }


        /// <summary>
        /// 根据语言ID和分类父ID获取到父ID下的所有子分类
        /// </summary>
        /// <param name="languageId"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public ResultModel GetCategoriesByParentIds(int languageId, int parentId)
        {
            var data = GetAll(languageId).Data;

            List<ResultCategoryModel> list = new List<ResultCategoryModel>();
            CreateTree1(data, parentId, list);
            return new ResultModel { Data = list };
            
        } 


        /// <summary>
        /// 根据分类Id和语言Id获取类别
        /// </summary>
        /// <param name="cateId"></param>
        /// <param name="languageId"></param>
        /// <returns></returns>
        public ResultModel GetCateByPid(int pid, int languageId)
        {
            var result = new ResultModel();
            var z = this._database.Db.Category
                .Query().LeftJoin(this._database.Db.Category_Lang, CategoryId: this._database.Db.Category.CategoryId);
            z = z.Select(
                this._database.Db.Category.CategoryId, this._database.Db.Category.Place
                , z.Category_Lang.CategoryName, z.Category_Lang.LanguageID);
            z = z.Where(z.Category_Lang.LanguageID == languageId).Where(this._database.Db.Category.parentId == pid).
                Where(this._database.Db.Category.AuditState == 1).OrderBy(this._database.Db.Category.Place);
            result.Data = z.ToList<CategorysModel>();
            return result;
        }

        /// <summary>
        ///  首页获取全部分类
        ///  zhoub 20150831
        /// </summary>
        /// <param name="languageId">语言</param>
        /// <returns></returns>
        public ResultModel GetWebAll(int languageId)
        {
            var result = new ResultModel();
            result.Data = this._database.RunQuery(db =>
            {
                dynamic lang;
                var data = db.Category.Query()
                    .LeftJoin(db.Category_Lang.As("CategoryLanguageModel"), out lang)
                    .On(lang.CategoryId == db.Category.CategoryId && lang.LanguageID == languageId)
                    .Select(db.Category.CategoryId, lang.CategoryName, db.Category.parentId)
                    .Where(db.Category.AuditState==1)
                    .OrderBy(db.Category.Place)
                    .ToList<CategorysModel>();
                return data;
            });
            return result;
        }

        /// <summary>
        ///     根据父id获取子类列表
        /// </summary>
        /// <param name="id">父id</param>
        /// <param name="languageId">语言</param>
        /// <returns></returns>
        public ResultModel GetParentCategoryListByChildernCategoryId(int id, int languageId)
        {
            var result = new ResultModel();
            result.Data = this._database.RunQuery(db =>
            {
                dynamic category;
                dynamic pcategory;
                dynamic pcategorys;
                dynamic newcsategorys;
                dynamic clang, pclang;

                return db.Category
                    .Query()
                    .LeftJoin(db.Category.As("cs"), out category).On(category.parentId == db.Category.parentId)
                    .LeftJoin(db.Category.As("pc"), out pcategory).On(pcategory.CategoryId == category.parentId)
                    .LeftJoin(db.Category.As("pcs"), out pcategorys).On(pcategorys.parentId == pcategory.parentId)
                    .LeftJoin(db.Category.As("newcs"), out newcsategorys)
                    .On(newcsategorys.parentId == pcategorys.CategoryId)
                    .LeftJoin(db.Category_Lang.As("clang"), out clang)
                    .On(clang.CategoryId == newcsategorys.CategoryId && clang.LanguageID == languageId)
                    .LeftJoin(db.Category_Lang.As("pclang"), out pclang)
                    .On(pclang.CategoryId == pcategorys.CategoryId && pclang.LanguageID == languageId)
                    .Select(
                        newcsategorys.CategoryId.Distinct(),
                        clang.CategoryName,
                        db.Category.parentId,
                        pcategorys.CategoryId.As("ParentCategoryId"),
                        pclang.CategoryName.As("ParentCategoryName"),
                        pcategory.parentId.As("ToCategoryId")
                    )
                    .Where(db.Category.CategoryId == id)
                    .ToList<ChlidernCategoryModel>()
                    ;
            });
            return result;
        }
        /// <summary>
        /// 隐藏分类
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public ResultModel HideCategoryById(int categoryId)
        {
            var result = new ResultModel();
            dynamic db = this._database.Db;
            #region 获取该分类和所有子分类
            var model = new CategoryModel();
            model = db.Category.get(categoryId);
            List<CategoryModel> list = GetAllChild(model);
            int astate = model.AuditState ? 0 : 1;
            #endregion
            #region 判断分类下是否有商品
            if (IsHaveProduct(list))//存在商品 不能删除
            {
                result.IsValid = false;
                result.Messages.Add("this ia having product,does not hide");
                return result;
            }
            #endregion
            #region 隐藏分类及其子分类
            using (var tx = this._database.Db.BeginTransaction())
            {
                try
                {
                    foreach (var cModel in list)
                    {
                        tx.Category.UpdateByCategoryId(CategoryId: cModel.CategoryId, AuditState: astate,
                                UpdateDT: DateTime.Now);
                    }
                    tx.Commit();
                    result.IsValid = true;
                }
                catch (Exception ex)
                {
                    tx.Rollback();
                    result.IsValid = false;
                    result.Messages.Add(string.Format("hide category {0} fails", categoryId));         
                }
            }
            #endregion
            return result;
        }
        /// <summary>
        /// 判断分类中是否包含商品
        /// </summary>
        /// <param name="cList"></param>
        /// <returns></returns>
        private bool IsHaveProduct(List<CategoryModel> cList)
        {
            cList= cList.FindAll(m=>m.Grade==3);
            if (cList.Count > 0)
            {
                foreach (var model in cList)
                {
                    int count = this._database.Db.Product.GetCount(this._database.Db.Product.CategoryId == model.CategoryId);
                    if (count > 0)
                    {
                        return true;
                    }                   
                }
            }
            return false;
        }
        /// <summary>
        /// 获取所有子分类（包括当前分类）
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public List<CategoryModel> GetAllChild(CategoryModel model)
        {
            List<CategoryModel> rList = new List<CategoryModel>();
            rList.Add(model);
            List<CategoryModel> cList = this._database.Db.Category.All().ToList<CategoryModel>();
            var list = cList.FindAll(m => m.parentId == model.CategoryId);
            if (list != null && list.Count > 0)//判断是否有子类
            {               
                foreach (var l in list)
                {
                    rList.AddRange(GetAllChild(l));
                }
            }
            return rList;
        }
        /// <summary>
        ///     递归创建树
        /// </summary>
        /// <param name="categories"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        private dynamic CreateTree(List<dynamic> categories, int parentId)
        {
            var list = categories.FindAll(m => m.parentId == parentId);

            dynamic nodes = null;

            if (list.Any())
            {
                nodes = list.Select(m => new
                {
                    m.id,
                    m.text,
                    m.languageId,
                    m.parentId,
                    m.Grade,
                    nodes = CreateTree(categories, m.id)
                });
            }

            return nodes;
        }

    }
}