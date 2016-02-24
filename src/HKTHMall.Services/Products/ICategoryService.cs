using System.Collections.Generic;
using Autofac.Extras.DynamicProxy2;
using BrCms.Core.Infrastructure;
using HKTHMall.Domain.AdminModel.Models.Categoreis;
using HKTHMall.Domain.Models;

namespace HKTHMall.Services.Products
{
    /// <summary>
    ///     类别
    /// </summary>
    [Intercept(typeof (ServiceIInterceptor))]
    public interface ICategoryService : IDependency
    {
        /// <summary>
        /// 获取类别详细信息
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        ResultModel GetCategoryInfoById(int categoryId);
         /// <summary>
        /// 隐藏分类
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        ResultModel HideCategoryById(int categoryId);
        
        /// <summary>
        ///     添加类别
        /// </summary>
        /// <param name="model">类别模型</param>
        /// <returns>是否成功</returns>
        ResultModel AddCategory(AddCategoryModel model);

        /// <summary>
        /// 获取全部分类
        /// </summary>
        /// <param name="languageId">语言</param>
        /// <returns></returns>
        ResultModel GetAll(int languageId);

        /// <summary>
        ///     根据类别id获取类别
        /// </summary>
        /// <param name="id">类别id</param>
        /// <returns>类别模型</returns>
        ResultModel GetCategoryById(int id);

        /// <summary>
        ///     获取类别列表
        /// </summary>
        /// <returns>类别列表</returns>
        ResultModel GetCategoriesByCategoryToTree(int languageId, int parentId=0);

        /// <summary>
        ///     更新类别
        /// </summary>
        /// <param name="model">类别模型</param>
        /// <returns>是否修改成功</returns>
        ResultModel UpdateCategory(UpdateCategoryModel model);

        /// <summary>
        ///     根据父ID获取商品分类
        ///     zhoub 20150708
        /// </summary>
        /// <param name="languageId">语言ID</param>
        /// <param name="parentId">父ID</param>
        /// <returns></returns>
        List<CategoryModel> GetCategoriesByParentId(int languageId, int parentId);

        /// <summary>
        ///     根据父ID获取(启用)商品分类
        ///     wuyf 20150708
        /// </summary>
        /// <param name="languageId">语言ID</param>
        /// <param name="parentId">父ID</param>
        /// <returns></returns>
        ResultModel GetCategoriesByParentIdAuditState(int languageId, int parentId);


        /// <summary>
        /// 根据语言类型获取全部的可以使用的数据
        /// </summary>
        /// <param name="languageId"></param>
        /// <returns></returns>
        ResultModel GetCategoriesByALL(int languageId, int dCategoryId);

        /// <summary>
        /// 根据分类Id和语言Id获取类别
        /// </summary>
        /// <param name="cateId"></param>
        /// <param name="languageId"></param>
        /// <returns></returns>
        ResultModel GetCateById(int cateId, int languageId);
         /// <summary>
        /// 根据分类Id和语言Id获取类别
        /// </summary>
        /// <param name="cateId"></param>
        /// <param name="languageId"></param>
        /// <returns></returns>
        ResultModel GetCateByIdForApi(int cateId, int languageId);
        /// <summary>
        /// 根据父Id和语言Id获取类别
        /// </summary>
        /// <param name="cateId"></param>
        /// <param name="languageId"></param>
        /// <returns></returns>
        ResultModel GetCateByPid(int pid, int languageId);

        /// <summary>
        ///  首页获取全部分类
        ///  zhoub 20150831
        /// </summary>
        /// <param name="languageId">语言</param>
        /// <returns></returns>
        ResultModel GetWebAll(int languageId);

        /// <summary>
        /// 根据分类级别获取分类列表
        /// </summary>
        /// <param name="grade">分类级别</param>
        /// <param name="languangeId">语言Id</param>
        /// <returns>类别列表</returns>
        ResultModel GetCategoryByGrade(int grade, int languangeId);

        /// <summary>
        /// 根据级id获取子列表
        /// </summary>
        /// <param name="parentId">父Id</param>
        /// <param name="languageId">语言</param>
        /// <returns>列表</returns>
        ResultModel GetCategoryByParentId(int parentId, int languageId);


        ResultModel GetParentCategoryListByChildernCategoryId(int id,int languageId);

        /// <summary>
        /// 根据语言ID和分类父ID获取到父ID下的所有子分类
        /// </summary>
        /// <param name="languageId"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        ResultModel GetCategoriesByParentIds(int languageId, int parentId);
    }
}