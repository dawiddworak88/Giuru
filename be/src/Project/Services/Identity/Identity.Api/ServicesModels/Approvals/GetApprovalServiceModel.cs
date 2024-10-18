using Foundation.Extensions.Models;
using System;

namespace Identity.Api.ServicesModels.Approvals
{
    public class GetApprovalServiceModel : BaseServiceModel
    {
        public Guid? Id { get; set; }
    }
}
