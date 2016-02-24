using BrCms.Core.Infrastructure;
using HKTHMall.Domain.AdminModel.Models.New;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Services.New
{
    public interface IAPP_NewsInfoService : IDependency
    {
        /// <summary>
        /// 根据Id字符串获取新闻基本信息
        /// </summary>
        /// <param name="ids">数组转的字符串 中间以逗号隔开</param>
        /// <returns></returns>
        List<APP_NewsInfoModel> GetListById(string ids);
        /// <summary>
        /// 执行Sql操作
        /// </summary>
        /// <param name="sql"></param>
        void ExecuteSql(string sql);
    }
}