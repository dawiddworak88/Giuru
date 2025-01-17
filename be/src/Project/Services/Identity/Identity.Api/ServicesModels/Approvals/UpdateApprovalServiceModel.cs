using Foundation.Extensions.Models;
using System;

namespace Identity.Api.ServicesModels.Approvals
{
    public class UpdateApprovalServiceModel : BaseServiceModel
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
    }
}
