using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKTHMall.Domain.Models;
using HKTHMall.Domain.AdminModel.Models.New;
using Simple.Data;
using Simple.Data.RawSql;

namespace HKTHMall.Services.New
{
    public class APP_NewsInfoService : BaseService, IAPP_NewsInfoService
    {
        /// <summary>
        /// 根据Id字符串获取新闻基本信息
        /// </summary>
        /// <param name="ids">数组转的字符串 中间以逗号隔开</param>
        /// <returns></returns>
        public  List<APP_NewsInfoModel> GetListById(string ids)
        {
            var result = new ResultModel();
            //查询参数条件
            if (!string.IsNullOrEmpty(ids))
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append(@"select ID,TypeID,Title,NaviContent,SendStatus from bd_newsinfo where ID in (" + ids + ") and IsDelete == 0");

                var data = _database.RunSqlQuery(x => x.ToResultSets(strSql.ToString()));
                result.IsValid = data[0].Count > 0;
                if (result.IsValid)
                {
                    List<APP_NewsInfoModel> source = data[0] as List<APP_NewsInfoModel>;
                    return source;
                }
            }
            return new List<APP_NewsInfoModel>();
        }
        /// <summary>
        /// 执行Sql操作
        /// </summary>
        /// <param name="sql"></param>
        public void ExecuteSql(string sql)
        {
            if (!string.IsNullOrEmpty(sql))
            { var data = _database.RunSqlQuery(x => x.ToResultSets(sql)); }
        }

    }
}
