using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace HKTH.Framework.Dapper
{
    /// <summary>
    ///     数据访问工具类（所有的操作，必须使用参数化语句）
    ///     usage: https://github.com/StackExchange/dapper-dot-net
    /// </summary>
    public class DataUtils
    {
        public DataUtils(string name)
        {
            this.connectionString = ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }

        /// <summary>
        ///     连接地址
        /// </summary>
        private string connectionString { get; set; }

        /// <summary>
        ///     执行参数化SQL
        /// </summary>
        /// <returns>受影响的行数</returns>
        public int Execute(string sql, object param = null, CommandType commandType = CommandType.Text)
        {
            using (IDbConnection cnn = new SqlConnection(this.connectionString))
            {
                return cnn.Execute(sql, param, null, null, commandType);
            }
        }

        /// <summary>
        ///     执行查询, 返回指定的T数据类型
        /// </summary>
        public IList<T> Query<T>(string sql, object param = null, CommandType commandType = CommandType.Text)
        {
            using (IDbConnection cnn = new SqlConnection(this.connectionString))
            {
                return cnn.Query<T>(sql, param, null, true, null, commandType).ToList();

            }
        }

        /// <summary>
        ///     执行查询返回多结果集, 可依次访问 (注意，需要关闭GridReader)
        /// </summary>
        public SqlMapper.GridReader QueryMultiple(string sql, object param = null,
            CommandType commandType = CommandType.Text)
        {
            IDbConnection cnn = new SqlConnection(this.connectionString);
            return cnn.QueryMultiple(sql, param, null, null, commandType);
        }

        /// <summary>
        ///     执行查询，返回一个动态对象列表
        /// </summary>
        public IList<dynamic> Query(string sql, object param = null, CommandType? commandType = CommandType.Text)
        {
            using (IDbConnection cnn = new SqlConnection(this.connectionString))
            {
                return cnn.Query(sql, param, null, true, null, commandType).ToList();
            }
        }

        /// <summary>
        ///     更新实体
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="entity"></param>
        public void Update<T>(T entity) where T : class
        {
            using (IDbConnection cnn = new SqlConnection(this.connectionString))
            {
                cnn.Open();
                cnn.Update(entity);
                cnn.Close();
            }
        }

        /// <summary>
        ///     插入实体
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="entity"></param>
        public void Insert<T>(T entity) where T : class
        {
            using (IDbConnection cnn = new SqlConnection(this.connectionString))
            {
                cnn.Open();
                cnn.Insert(entity);
                cnn.Close();
            }
        }
    }
}