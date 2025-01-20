using System;
using System.Collections.Generic;

namespace Seller.Web.Shared.ApiRequestModels
{
    public class UserApprovalsRequestModel
    {
        public IEnumerable<Guid> ApprovalIds { get; set; }
        public Guid UserId { get; set; }
    }
}
