using System;

namespace Foundation.GenericRepository.Paginations
{
    public class PagedResults<T>
    { 
        protected PagedResults()
        { }

        public PagedResults(long total, int pageSize)
        {
            this.Total = total;
            this.PageSize = pageSize;
        }

        public T Data { get; set; }

        public long Total { get; }

        public int PageSize { get; }

        public int PageCount 
        {
            get
            {
                return (int)Math.Ceiling((decimal)this.Total / this.PageSize);
            }  
        }
    }
}
