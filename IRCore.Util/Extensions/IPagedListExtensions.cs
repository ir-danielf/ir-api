using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace PagedList
{
    public class PagedListConvert<T> : PagedList<T> 
    {

        public PagedListConvert(IPagedList pagedList, IEnumerable<T> superset) : base(superset, 1, pagedList.PageSize)
        {
            this.TotalItemCount = pagedList.TotalItemCount;
            this.PageCount = pagedList.PageCount;
            this.PageNumber = pagedList.PageNumber;
            this.HasNextPage = pagedList.HasNextPage;
            this.HasPreviousPage = pagedList.HasPreviousPage;
            this.IsFirstPage = pagedList.IsFirstPage;
            this.IsLastPage = pagedList.IsLastPage;
        }
        
    }

    public class PagedListManual<T> : PagedListMetaData, IPagedList<T>
    {

        private List<T> _Subset { get; set; }
        public PagedListManual(List<T> subset, int pageNumber, int pageSize, int count)
        {
            this.TotalItemCount = count;
            this.PageSize = pageSize;
            this.PageCount = TotalItemCount > 0 ? (int)Math.Ceiling(TotalItemCount / (double)PageSize) : 0;
            this.PageNumber = pageNumber;
            this._Subset = subset;
            if(PageCount > 0)
            {
                this.HasNextPage = PageCount > PageNumber;
                this.HasPreviousPage = PageNumber > 1;
                this.IsFirstPage = PageNumber == 1;
                this.IsLastPage = PageCount == PageNumber;
            }

            
        }
        /// <summary>
        /// 	Returns an enumerator that iterates through the BasePagedList&lt;T&gt;.
        /// </summary>
        /// <returns>A BasePagedList&lt;T&gt;.Enumerator for the BasePagedList&lt;T&gt;.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            return _Subset.GetEnumerator();
        }

        /// <summary>
        /// 	Returns an enumerator that iterates through the BasePagedList&lt;T&gt;.
        /// </summary>
        /// <returns>A BasePagedList&lt;T&gt;.Enumerator for the BasePagedList&lt;T&gt;.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        ///<summary>
        ///	Gets the element at the specified index.
        ///</summary>
        ///<param name = "index">The zero-based index of the element to get.</param>
        public T this[int index]
        {
            get { return _Subset[index]; }
        }

        /// <summary>
        /// 	Gets the number of elements contained on this page.
        /// </summary>
        public int Count
        {
            get { return _Subset.Count; }
        }

        ///<summary>
        /// Gets a non-enumerable copy of this paged list.
        ///</summary>
        ///<returns>A non-enumerable copy of this paged list.</returns>
        public IPagedList GetMetaData()
        {
            return new PagedListMetaData(this);
        }


        
    }


    public static class IPagedListExtensions 
    {
        public static IPagedList<TResult> SelectPagedList<TSource, TResult>(this IPagedList<TSource> source, Func<TSource, TResult> selector)
        {
            return new PagedListConvert<TResult>(source, source.Select(selector));
        }

        public static IPagedList<T> ToPagedList<T>(this List<T> source, int pageNumber, int pageSize, int count)
        {
            return new PagedListManual<T>(source, pageNumber, pageSize, count);
        }
    }
        
}
