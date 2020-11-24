namespace Foundation.ApiExtensions.Models.Request
{
    public class PagedRequestModelBase : RequestModelBase
    {
        public string SearchTerm { get; set; }
        public int PageIndex { get; set; }
        public int ItemsPerPage { get; set; }
    }
}
