using BrCms.Framework.Data;

namespace HKTHMall.Core.Data
{
    public class BcDbContext : BaseDbContext, IBcDbContext
    {
        public BcDbContext(string nameOrConnectionString) : base(nameOrConnectionString)
        {
            //获取或设置一个值,该值指示是否启用针对公开为导航属性的关系的延迟加载。 延迟加载在默认情况下处于启用状态。
            this.Configuration.LazyLoadingEnabled = false;
        }

        protected override byte DbType
        {
            get { return (byte) BcDbType.Default; }
        }
    }

    public interface IBcDbContext : IDbContext
    {
        
    }
}
