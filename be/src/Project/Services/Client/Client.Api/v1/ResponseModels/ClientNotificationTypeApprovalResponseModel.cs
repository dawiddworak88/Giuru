using System;

namespace Client.Api.v1.ResponseModels
{
    public class ClientNotificationTypeApprovalResponseModel
    {
        public Guid Id { get; set; }
        public Guid ClientId { get; set; }
        public bool IsApproved { get; set; }
        public DateTime ApprovalDate { get; set; }
        public Guid NotificationTypeId { get; set; }
    }
}
