using System;

namespace Seller.Web.Shared.DomainModels.UserApproval
{
    public class UserApproval
    {
        public Guid ApprovalId { get; set; }
        public Guid UserId { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
