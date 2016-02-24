using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Threading;
using System.Threading.Tasks;

namespace BrCms.Framework.Data
{
    public interface IDbContext
    {
        DbSet<TEntity> Set<TEntity>() where TEntity : class;

        int SaveChanges();

        DbSet Set(Type entityType);

        DbEntityEntry Entry(object entity);

        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;

        Task<int> SaveChangesAsync();

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);

        Database Database { get; }
    }
}
