using System;
using System.Collections.Generic;

namespace Identity.Api.v1.RequestModels
{
    public class UserApprovalRequestModel
    {
        public IEnumerable<Guid> ApprovalIds { get; set; }
        public Guid UserId { get; set; }
    }
}
