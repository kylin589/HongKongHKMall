using BrCms.Core.Infrastructure;
using HKTHMall.Domain.AdminModel;
using HKTHMall.Domain.AdminModel.Products;
using HKTHMall.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Services.Products
{
    public interface IProductConsultService : IDependency
    {
        /// <summary>
        /// 分页获取商品咨询信息
        /// zhoub 20150827
        /// </summary>
        /// <param name="model">用户搜索模型</param>
        /// <returns>用户列表数据</returns>
        ResultModel GetPagingProductConsult(SearchProductConsultModel model, int languageID);      
      

        /// <summary>
        /// 商品咨询回复
        /// zhoub 20150827
        /// </summary>
        /// <param name="model"></param>
        /// <param name="languageID"></param>
        /// <returns></returns>
        ResultModel ReplyProductConsult(ProductConsultModel model, int languageID);

        /// <summary>
        /// 商品咨询删除
        /// zhoub 20150827
        /// </summary>
        /// <param name="productConsultId"></param>
        /// <param name="languageID"></param>
        /// <returns></returns>
        ResultModel DeleteProductConsult(long productConsultId, int languageID);

        
        /// <summary>
        /// 添加商品咨询
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ResultModel AddConsult(ProductConsult model);
        /// <summary>
        /// 商品问题咨询列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ResultModel GetConsulList(SearchConsle model, out int count);


        /// <summary>
        /// 获取咨询总数
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        ResultModel GetConsultCount(long productId);

        /// <summary>
        /// 获取咨询总数,add by liujc
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        ResultModel GetConsultCountGroup(long productId);

        /// <summary>
        /// 商品问题咨询列表.add by liujc
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ResultModel GetConsulList(SearchConsle model);

        /// <summary>
        /// 添加商品咨询点赞,add by liujc
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ResultModel AddUserConsult(UserConsult model);
    }
}
