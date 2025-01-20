using System;

namespace Identity.Api.v1.ResponseModels
{
    public class UserApprovalResponseModel
    {
        public Guid ApprovalId { get; set; }
        public Guid UserId { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
