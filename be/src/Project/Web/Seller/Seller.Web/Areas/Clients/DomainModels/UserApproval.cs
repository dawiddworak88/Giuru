using System;

namespace Seller.Web.Areas.Clients.DomainModels
{
    public class UserApproval
    {
        public Guid ApprovalId { get; set; }
        public Guid UserId { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
