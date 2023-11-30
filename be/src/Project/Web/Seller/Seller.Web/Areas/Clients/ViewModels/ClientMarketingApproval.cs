using System;

namespace Seller.Web.Areas.Clients.ViewModels
{
    public class ClientMarketingApproval
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public bool IsApproved { get; set; }
        public Guid ClientId { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
