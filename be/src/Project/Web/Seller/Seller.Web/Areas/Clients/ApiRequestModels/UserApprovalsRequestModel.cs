using System;
using System.Collections.Generic;

namespace Seller.Web.Areas.Clients.ApiRequestModels
{
    public class UserApprovalsRequestModel
    {
        public IEnumerable<Guid> ApprovalIds { get; set; }
        public Guid UserId { get; set; }
    }
}
