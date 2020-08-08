namespace Foundation.GenericRepository.Paginations
{
    public class PagedResults<T>
    {
        public T Data { get; set; }

        public int Total { get; set; }

        public int PageCount { get; set; }
    }
}
