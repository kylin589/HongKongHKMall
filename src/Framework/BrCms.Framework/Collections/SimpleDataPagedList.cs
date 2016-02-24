using System;
using System.Collections.Generic;
using Simple.Data;

namespace BrCms.Framework.Collections
{
    [Serializable]
    public class SimpleDataPagedList<T> : List<T>, IPagedList<T>
    {

        public SimpleDataPagedList(dynamic source, int pageIndex, int pageSize)
        {
            Promise<int> total;

            this.PageSize = pageSize;
            this.PageIndex = pageIndex;
            this.AddRange(source.WithTotalCount(out total).Skip(pageIndex * pageSize).Take(pageSize).ToList<T>());

            this.TotalCount = total;
            this.TotalPages = this.TotalCount / pageSize;

            if (total % pageSize > 0)
                this.TotalPages++;
        }


        public SimpleDataPagedList(List<T> source, int pageIndex, int pageSize, int total)
        {
            if (source != null && source.Count > 0)
            {
                this.AddRange(source);
            }

            this.PageSize = pageSize;
            this.PageIndex = pageIndex;

            this.TotalCount = total;
            this.TotalPages = this.TotalCount / pageSize;

            if (total % pageSize > 0)
            {
                this.TotalPages++;
            }
        }


        public int PageIndex { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }
        public int TotalPages { get; private set; }

        public bool HasPreviousPage
        {
            get { return (this.PageIndex > 0); }
        }

        public bool HasNextPage
        {
            get { return (this.PageIndex + 1 < this.TotalPages); }
        }
    }
}