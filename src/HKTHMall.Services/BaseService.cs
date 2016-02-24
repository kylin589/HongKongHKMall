using Autofac;
using BrCms.Framework.Data;
using BrCms.Framework.Infrastructure;
using BrCms.Framework.Logging;
using HKTH.Framework.Dapper;
using HKTHMall.Core.Data;

namespace HKTHMall.Services
{
    public abstract class BaseService
    {
        protected readonly IDatabaseHelper _database = BrEngineContext.Current.Resolve<IDatabaseHelper>();
        protected readonly IBcDbContext _dbContext = BrEngineContext.Current.Resolve<IBcDbContext>();
        protected readonly ILogger _logger = BrEngineContext.Current.Resolve<ILogger>();
        protected readonly DataUtils _dataDapper = BrEngineContext.Current.Resolve<DataUtils>();
    }
}