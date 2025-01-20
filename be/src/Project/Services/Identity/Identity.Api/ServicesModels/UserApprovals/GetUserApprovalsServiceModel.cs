using Foundation.Extensions.Models;
using System;

namespace Identity.Api.ServicesModels.UserApprovals
{
    public class GetUserApprovalsServiceModel : BaseServiceModel
    {
        public Guid UserId { get; set; }
    }
}
