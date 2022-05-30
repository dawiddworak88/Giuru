using System;

namespace Seller.Web.Areas.Clients.DomainModels
{
    public class ClientApplication
    {
        public Guid Id { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
