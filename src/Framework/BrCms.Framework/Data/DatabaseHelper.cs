using System;
using System.Configuration;
using System.Data.SqlClient;
using Simple.Data;

namespace BrCms.Framework.Data
{
    public abstract class DatabaseHelper : IDatabaseHelper
    {
        private readonly string _conStr;

        protected DatabaseHelper(string name)
        {
            this._conStr = ConfigurationManager.ConnectionStrings[name].ConnectionString;
            this.Db = Database.Opener.OpenConnection(this._conStr);
        }

        public dynamic Db { get; private set; }

        public dynamic RunQuery(Func<dynamic, dynamic> dbQuery)
        {
            dynamic db = Database.OpenConnection(this._conStr);
            var conn = this.GetOpenConnection();
            db.UseSharedConnection(conn);
            var results = dbQuery(db);
            db.StopUsingSharedConnection();
            conn.Close();
            return results;
        }


        public dynamic RunSqlQuery(Func<Database, dynamic> dbQuery)
        {
            dynamic db = Database.OpenConnection(_conStr);
            SqlConnection conn = GetOpenConnection();
            db.UseSharedConnection(conn);
            var results = dbQuery(db);
            db.StopUsingSharedConnection();
            conn.Close();
            return results;
        }

        private SqlConnection GetOpenConnection()
        {
            var connection = new SqlConnection(this._conStr);
            connection.Open();
            return connection;
        }
    }
}