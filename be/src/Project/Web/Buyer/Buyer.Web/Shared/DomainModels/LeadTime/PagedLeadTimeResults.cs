using System.Collections.Generic;

namespace Buyer.Web.Shared.DomainModels.LeadTime
{
    public class PagedLeadTimeResults
    {
        public IEnumerable<LeadTimeItem> Items { get; set; }
        public int TotalCount { get; set; }
        public int PageSize { get; set; }
        public int PagedCount { get; set; }
    }
}
