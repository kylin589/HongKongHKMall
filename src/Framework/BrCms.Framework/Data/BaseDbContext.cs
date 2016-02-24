using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using Autofac;
using BrCms.Framework.Infrastructure;

namespace BrCms.Framework.Data
{
    /// <summary>
    /// dbcontext基础
    /// </summary>
    public abstract class BaseDbContext : DbContext
    {
        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="nameOrConnectionString">Config数据库连接字符串</param>
        protected BaseDbContext(string nameOrConnectionString) 
            : base(nameOrConnectionString)
        {
            //获取或设置一个值,该值指示是否启用针对公开为导航属性的关系的延迟加载。 延迟加载在默认情况下处于启用状态。
            //this.Configuration.LazyLoadingEnabled = false;

            ////获取或设置一个值,该值指示框架在创建实体类型的实例时是否会创建动态生成的代理类的实例。 请注意,即使使用此标记启用了代理创建,也只会为满足代理设置要求的实体类型创建代理实例。 默认情况下启用代理创建。
            //this.Configuration.ProxyCreationEnabled = false;

            ////获取或设置一个值,该值指示当比较两个操作数,而它们都可能为 null 时,是否展示数据库 null 语义。 默认值为 false。 例如:如果 UseDatabaseNullSemantics 为 true,则 (operand1 == operand2) 将转换为 (operand1 = operand2)；如果 UseDatabaseNullSemantics 为 false,则将转换为 (((operand1 = operand2) AND (NOT (operand1 IS NULL OR operand2 IS NULL))) OR ((operand1 IS NULL) AND (operand2 IS NULL)))。
            ////this.Configuration.UseDatabaseNullSemantics = false;

            ////获取或设置一个值,该值指示在调用 SaveChanges() 时,是否应自动验证所跟踪的实体。 默认值为 true。
            //this.Configuration.ValidateOnSaveEnabled = false;

            ////获取或设置一个值,该值指示是否通过 DbContext 和相关类的方法自动调用 DetectChanges() 方法。 默认值为 true。
            //this.Configuration.AutoDetectChangesEnabled = false;

            ////获取或设置是否用一致 NullReference 行为的“布尔”值。
            //((IObjectContextAdapter)this).ObjectContext.ContextOptions.UseConsistentNullReferenceBehavior = false;

            ////获取或设置是否用 C# NullComparison 行为的“布尔”值。
            //((IObjectContextAdapter)this).ObjectContext.ContextOptions.UseCSharpNullComparisonBehavior = false;

            ////获取或设置确定是否使用旧式 PreserveChanges 行为的布尔值。
            //((IObjectContextAdapter)this).ObjectContext.ContextOptions.UseCSharpNullComparisonBehavior = false;
        }

        /// <summary>
        /// 实体模型数据库映射
        /// </summary>
        /// <param name="modelBuilder">modelBuilder</param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var typeFinder = BrEngineContext.Current.Resolve<ITypeFinder>();

            typeFinder.FindClassesOfType(typeof (EntityTypeConfiguration<>)).ToList()
                .ForEach(type =>
                {
                    var attributes = type.GetCustomAttributes(typeof (DbContextTypeAttribute), false)
                        .Select(a => (DbContextTypeAttribute) a);
                    foreach (var attribute in attributes)
                    {
                        if (attribute.DbType != this.DbType) continue;
                        dynamic configurationInstance = Activator.CreateInstance(type);
                        modelBuilder.Configurations.Add(configurationInstance);
                    }
                });

            base.OnModelCreating(modelBuilder);
        }

        /// <summary>
        /// 数据库名称 byte取值0-255
        /// </summary>
        protected abstract byte DbType { get; }

    }
}
