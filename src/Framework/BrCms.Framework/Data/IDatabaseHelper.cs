using System;
using Simple.Data;

namespace BrCms.Framework.Data
{
    public interface IDatabaseHelper
    {
        dynamic Db { get; }
        dynamic RunQuery(Func<dynamic, dynamic> dbQuery);
        dynamic RunSqlQuery(Func<Database, dynamic> dbQuery);
    }
}