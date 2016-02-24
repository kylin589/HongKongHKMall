using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrCms.Core.Infrastructure;
using BrCms.Framework.Collections;
using HKTHMall.Domain.AdminModel.Models.SKU;
using HKTHMall.Domain.Models;

namespace HKTHMall.Services.SKU
{
    /// <summary>
    /// 商品规格服务接口
    /// </summary>
    public interface ISKU_AttributesService : IDependency
    {
        /// <summary>
        /// 添加规格
        /// </summary>
        /// <param name="model">规格模型</param>
        ResultModel AddStandardSKU_Attributes(SKU_AttributesModel model);

        /// <summary>
        /// 根据规格id获取规格
        /// </summary>
        /// <param name="id">规格id</param>
        /// <returns>规格模型</returns>
        ResultModel GetSKU_AttributesById(long id);


        /// <summary>
        /// 判断规格值是否已经被使用
        /// </summary>
        /// <param name="valueId">规格值Id</param>
        /// <returns>是否已经存在</returns>
        ResultModel CheckValueIsUsed(int valueId);


        /// <summary>
        /// 更新规格
        /// </summary>
        /// <param name="model">规格模型</param>
        /// <returns>是否修改成功</returns>
        ResultModel UpdateStandardSKU_Attributes(SKU_AttributesModel model);

        /// <summary>
        /// 删除规格
        /// </summary>
        /// <param name="id">规格Id</param>
        /// <returns>是否删除成功</returns>
        ResultModel DeleteSKU_AttributesById(long id);

        /// <summary>
        /// 分页获取规格列表
        /// </summary>
        /// <param name="model">规格搜索模型</param>
        /// <returns>规格列表数据</returns>
        ResultModel GetPagingSKU_Attributess(SearchSKU_AttributesModel model);

        /// <summary>
        /// 获取所有属性名称列表
        /// </summary>
        /// <param name="isSKU">是否SKU规格</param>
        /// <returns>属性列表</returns>
        ResultModel GetAllSKU_AttributesBy(bool? isSKU);

        /// <summary>
        /// 根据属性名Id获取属性值列表
        /// </summary>
        /// <param name="id">属性名Id</param>
        /// <returns>属性值列表</returns>
        ResultModel GetAttributeValuesById(long id);

        /// <summary>
        /// 查询商品规格参数
        /// </summary>
        /// <param name="productId">商品ID</param>
        /// <returns></returns>
        ResultModel GetSKU_ProductSpecificationParameterById(long productId,int lang);

    }
}
