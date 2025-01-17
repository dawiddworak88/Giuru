using Foundation.GenericRepository.Entities;
using System;

namespace Identity.Api.Infrastructure.Approvals.Entities
{
    public class UserApproval : Entity
    {
        public Guid UserId { get; set; }
        public Guid ApprovalId { get; set; }
        public virtual Approval Approval { get; set; }
    }
}
