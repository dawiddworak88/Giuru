using System;

namespace Client.Api.v1.ResponseModels
{
    public class ClientMarketingApprovalResponseModel
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public bool IsApproved { get; set; }
        public Guid? ClientId { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
