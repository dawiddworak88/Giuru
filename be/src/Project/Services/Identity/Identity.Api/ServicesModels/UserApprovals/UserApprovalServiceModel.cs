using Foundation.Extensions.Models;
using System;

namespace Identity.Api.ServicesModels.UserApprovals
{
    public class UserApprovalServiceModel : BaseServiceModel
    {
        public Guid ApprovalId { get; set; }
        public Guid UserId { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
