using System;

namespace Foundation.ApiExtensions.Models.Request
{
    public class PagedRequestModelBase : RequestModelBase
    {
        public string Ids { get; set; }
        public Guid? SellerId { get; set; }
        public string SearchTerm { get; set; }
        public int PageIndex { get; set; }
        public int ItemsPerPage { get; set; }
        public string OrderBy { get; set; }
    }
}
