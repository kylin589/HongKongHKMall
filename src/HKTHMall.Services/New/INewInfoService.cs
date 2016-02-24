using BrCms.Core.Infrastructure;
using HKTHMall.Domain.AdminModel.Models.New;
using HKTHMall.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Services.New
{
    public interface INewInfoService : IDependency
    {
        /// <summary>
        /// 获取新闻信息表
        /// </summary>
        /// <param name="model">model</param>
        /// <returns>新闻信息表</returns>
        ResultModel GetNewInfoList(SearchNewInfoModel model);

        /// <summary>
        /// 添加新闻信息表
        /// </summary>
        /// <param name="model">新闻信息表</param>
        /// <returns>是否成功</returns>
        ResultModel AddNewInfo(NewInfoModel model);

        /// <summary>
        /// 更新新闻信息表(更新推荐)
        /// </summary>
        /// <param name="model">新闻信息表模型</param>
        /// <returns>是否修改成功</returns>
        ResultModel UpdateNewInfo(NewInfoModel model);

        /// <summary>
        /// 更新新闻信息表
        /// </summary>
        /// <param name="model">新闻信息表模型</param>
        /// <returns>是否修改成功</returns>
        ResultModel UpdateNewInfos(NewInfoModel model);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="model">新闻信息表模型</param>
        /// <returns>是否删除成功</returns>
        ResultModel DeleteNewInfo(NewInfoModel model);

        /// <summary>
        /// 獲取首頁新聞公告
        /// 黃主霞 2016-01-14
        /// </summary>
        /// <param name="TopCount"></param>
        /// <param name="PageNo">从1开始</param>
        /// <param name="NewsType">null表示获取所有,0表示公告1表示特惠</param>
        /// <param name="IsRecommend">是否推薦(null表示所有)</param>
        /// <returns></returns>
        ResultModel GetIndexNews(int TopCount, int PageNo, int? NewsType, bool? IsRecommend);

        /// <summary>
        /// 获取新闻公告总条数
        /// 黄主霞 2016-01-15
        /// </summary>
        /// <param name="NewsType">null表示获取所有,0表示公告1表示特惠</param>
        /// <param name="IsRecommend">是否推薦(null表示所有)</param>
        /// <returns></returns>
        int GetNewsCount(int? NewsType, bool? IsRecommend);
        /// <summary>
        /// 根据ID获取新闻详细内容
        /// 黄主霞：2016-01-15
        /// </summary>
        /// <param name="id">新闻ID</param>
        /// <returns></returns>
        ResultModel GetNewsById(long id);
    }
}
