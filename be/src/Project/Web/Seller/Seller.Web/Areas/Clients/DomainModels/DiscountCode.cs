using System;
using System.Collections.Generic;

namespace Seller.Web.Areas.Clients.DomainModels
{
    public class DiscountCode
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public bool IsDisabled { get; set; }
        public IEnumerable<Guid> ClientIds { get; set; }
        public int ClientsCount { get; set; }
        public string Version { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
