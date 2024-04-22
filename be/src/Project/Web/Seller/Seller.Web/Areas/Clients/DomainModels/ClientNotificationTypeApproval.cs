using System;

namespace Seller.Web.Areas.Clients.DomainModels
{
    public class ClientNotificationTypeApproval
    {
        public Guid Id { get; set; }
        public Guid ClientId { get; set; }
        public bool IsApproved { get; set; }
        public DateTime ApprovalDate { get; set; }
        public Guid NotificationTypeId { get; set; }
    }
}
