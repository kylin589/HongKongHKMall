using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Core.Sql
{
    /// <summary>
    /// Sql语句失误工具类
    /// </summary>
    public static class SqlTransactionUtil
    {
        /// <summary>
        /// 生成事务Sql语句
        /// </summary>
        /// <param name="sqls">sql语句集合</param>
        /// <returns>Sql语句</returns>
        public static string GenerateTransSql(List<string> sqls)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.AppendLine("DECLARE @errorCount INT=0; BEGIN TRANSACTION ");

            foreach (var sql in sqls)
            {
                sqlBuilder.AppendLine(sql);
                sqlBuilder.AppendLine(SqlTransactionUtil.GenerErrorSql());
            }

            sqlBuilder.AppendLine(" IF @errorCount<>0 BEGIN ROLLBACK TRANSACTION SELECT 0 AS [Count] END  ELSE BEGIN COMMIT TRANSACTION SELECT 1 AS [Count] END ");
            return sqlBuilder.ToString();
        }

        /// <summary>
        /// 生成统计错误Sql语句
        /// </summary>
        /// <returns>Sql语句</returns>
        public static string GenerErrorSql()
        {
            string errorTpl = " SET @errorCount=@errorCount+@@ERROR";
            return errorTpl;
        }
    }
}
