using Foundation.Extensions.Models;
using System;

namespace Identity.Api.ServicesModels.Approvals
{
    public class DeleteApprovalServiceModel : BaseServiceModel
    {
        public Guid? Id { get; set; }
    }
}
