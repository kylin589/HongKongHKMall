using System.Linq;

namespace BrCms.Framework.Collections
{
    public static class PagedExtensions
    {
        public static IPagedList<T> ToPagedList<T>(this IQueryable<T> query, int pagedIndex, int pagedSize)
        {
            return new PagedList<T>(query, pagedIndex, pagedSize);
        } 
    }
}
