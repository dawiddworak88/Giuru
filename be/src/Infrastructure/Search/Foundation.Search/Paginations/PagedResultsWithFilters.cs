using System.Collections.Generic;
using Foundation.GenericRepository.Paginations;
using Foundation.Search.Models;

namespace Foundation.Search.Paginations
{
    public class PagedResultsWithFilters<T> : PagedResults<T>
    {
        private PagedResultsWithFilters() : base()
        { }

        public PagedResultsWithFilters(long total, int pageSize)
            : base(total, pageSize)
        {
            Filters = new List<Filter>();
        }

/*        public PagedResultsWithFilters(long total, int pageSize, T data, List<Filter> filters)
            : base(total, pageSize)
        {
            Data = data;
            Filters = filters ?? new List<Filter>();
        }*/

        public List<Filter> Filters { get; set; }
    }
}