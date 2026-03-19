using System;

namespace Buyer.Web.Shared.DomainModels.LeadTime
{
    public class LeadTimeItem
    {
        public Guid CustomerId { get; set; }
        public string Sku { get; set; }
        public int LeadTimeDays { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
