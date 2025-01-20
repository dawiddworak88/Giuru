using System;

namespace Identity.Api.ServicesModels.Approvals
{
    public class ApprovalServiceModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }
}
