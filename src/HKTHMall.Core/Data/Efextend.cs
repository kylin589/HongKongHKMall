using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;

namespace HKTHMall.Core.Data
{
    public static class Efextend
    {
        public static string GetPropertyName<T>(this T t, Expression<Func<T, object>> expr) where T : class
        {
            var rtn = "";
            if (expr.Body is UnaryExpression)
            {
                rtn = ((MemberExpression)((UnaryExpression)expr.Body).Operand).Member.Name;
            }
            else if (expr.Body is MemberExpression)
            {
                rtn = ((MemberExpression)expr.Body).Member.Name;
            }
            else if (expr.Body is ParameterExpression)
            {
                rtn = ((ParameterExpression)expr.Body).Type.Name;
            }
            return rtn;
        }

        public static void Delete<TEntity>(this IBcDbContext dbContext, TEntity entity) where TEntity : class
        {
            dbContext.Set<TEntity>().Attach(entity);
            dbContext.Set<TEntity>().Remove(entity);
        }

        public static void Update<TEntity>(this IBcDbContext dbContext, TEntity entity,
            params Expression<Func<TEntity, object>>[] exprs) where TEntity : class
        {
            dbContext.Update(entity,exprs.Select(entity.GetPropertyName).ToArray());
        }


        public static void Update<TEntity>(this IBcDbContext dbContext, TEntity entity,
           params string[] pnames) where TEntity : class
        {
            dbContext.Set<TEntity>().Attach(entity);
            var stateEntry = ((IObjectContextAdapter)dbContext).ObjectContext.
                ObjectStateManager.GetObjectStateEntry(entity);
            foreach (var pname in pnames)
            {
                stateEntry.SetModifiedProperty(pname);
            }
        }
    }
}
