using BrCms.Core.Infrastructure;
using HKTHMall.Domain.AdminModel.Models.SKU;
using HKTHMall.Domain.Models;

namespace HKTHMall.Services.SKU
{
    public interface ISKU_ProductTypesService : IDependency
    {
        /// <summary>
        /// 获取商品类型
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ResultModel GetSKU_ProductTypesById(int id);


        /// <summary>
        ///     分页获取商品类型列表
        /// </summary>
        /// <param name="model">规格商品类型模型</param>
        /// <returns>规格商品类型数据</returns>
        ResultModel GetPagingSKU_ProductTypes(SearchSKU_ProductTypesModel model);

        /// <summary>
        /// 添加商品类型
        /// </summary>
        /// <param name="model">需要添加的商品类型</param>
        /// <returns>操作结果</returns>
        ResultModel AddSKU_ProductTypes(SKU_ProductTypesModel model);

        /// <summary>
        /// 修改商品类型
        /// </summary>
        /// <param name="model">需要修改的商品类型</param>
        /// <returns>操作结果</returns>
        ResultModel UpdateSKU_ProductTypes(SKU_ProductTypesModel model);

        ResultModel GetSku_ProductTypesByCategoryId(int id);
    }
}
