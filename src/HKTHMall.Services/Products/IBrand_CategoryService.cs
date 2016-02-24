using BrCms.Core.Infrastructure;
using HKTHMall.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrCms.Framework.Collections;
using HKTHMall.Domain.Models.Bra;
using HKTHMall.Domain.Models.Categoreis;
using HKTHMall.Domain.Models.User;

namespace HKTHMall.Services.Products
{
    /// <summary>
    /// 品牌关联接口
    /// zhoub 20150708
    /// </summary>
     public interface IBrand_CategoryService : IDependency
    {
        /// <summary>
        /// 添加品牌分类关联
        /// </summary>
        /// <param name="model">类别模型</param>
        /// <returns>是否成功</returns>
         ResultModel AddBrand_Category(Brand_CategoryModel model);

         /// <summary>
         /// 分页获取品牌关联列表
         /// zhoub 20150709
         /// </summary>
         /// <param name="model"></param>
         /// <returns></returns>
         ResultModel GetPagingBrand_Category(SearchBrandModel model);

         /// <summary>
         /// 品牌关联删除
         /// zhoub 20150709
         /// </summary>
         /// <param name="brand_CategoryId"></param>
         /// <returns></returns>
         ResultModel DeleteBrand_Category(string brand_CategoryId, string brandID);
    }
}
