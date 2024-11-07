using System;

namespace Buyer.Web.Areas.Orders.DomainModels
{
    public class UserApproval
    {
        public Guid ApprovalId { get; set; }
        public Guid UserId { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
