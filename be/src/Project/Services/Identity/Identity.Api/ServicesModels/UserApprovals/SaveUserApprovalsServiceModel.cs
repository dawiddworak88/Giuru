using Foundation.Extensions.Models;
using System;
using System.Collections.Generic;

namespace Identity.Api.ServicesModels.UserApprovals
{
    public class SaveUserApprovalsServiceModel : BaseServiceModel
    {
        public IEnumerable<Guid> ApprvoalIds { get; set; }
        public Guid UserId { get; set; }
    }
}
