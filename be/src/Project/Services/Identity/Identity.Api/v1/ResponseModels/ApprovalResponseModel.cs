using System;

namespace Identity.Api.v1.ResponseModels
{
    public class ApprovalResponseModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }
}
