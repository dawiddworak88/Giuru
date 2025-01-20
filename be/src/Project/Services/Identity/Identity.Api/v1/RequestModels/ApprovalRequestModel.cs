using System;

namespace Identity.Api.v1.RequestModels
{
    public class ApprovalRequestModel
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
