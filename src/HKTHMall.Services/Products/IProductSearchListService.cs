using BrCms.Core.Infrastructure;
using HKTHMall.Domain.Models;
using HKTHMall.Domain.WebModel.Models.Search;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Services.Products
{
    public interface IProductSearchListService : IDependency
    {
        /// <summary>
        /// 关键字搜素结果
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ResultModel GetProductSearchList(KeyWordsSearch model, out int count);
        /// <summary>
        /// 分类搜索列表
        /// </summary>
        /// <param name="model"></param>
        /// <param name="ilist">分类Id</param>
        /// <returns></returns>
        ResultModel GetProductSearchList(KeyWordsSearch model, int[] ilist, out int count);
        /// <summary>
        /// 我的收藏
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="languageId"></param>
        /// <returns></returns>
        ResultModel GetMyCollectionList(long userId, KeyWordsSearch model,out int count);
         /// <summary>
        /// 我的收藏
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="languageId"></param>
        /// <returns></returns>
        ResultModel GetMyCollectionListForApi(long userId, KeyWordsSearch model, out int count);
        
        /// <summary>
        /// 我的收藏删除
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="collectionId"></param>
        /// <returns></returns>
        ResultModel DeleteMyCollection(long userId, long collectionId);
        /// <summary>
        /// 分类搜索列表
        /// </summary>
        /// <param name="model"></param>
        /// <param name="ilist"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        ResultModel GetAllSearchList(KeyWordsSearch model, int[] ilist, long userId, out int count);
             /// <summary>
        /// 关键字搜素结果
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ResultModel GetProductSearchListNew(KeyWordsSearch model, long userId, out int count);
    }
}
