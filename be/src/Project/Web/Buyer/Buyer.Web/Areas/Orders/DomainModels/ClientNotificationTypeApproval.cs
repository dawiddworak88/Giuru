using System;

namespace Buyer.Web.Areas.Orders.DomainModels
{
    public class ClientNotificationTypeApproval
    {
        public Guid Id { get; set; }
        public Guid ClientId { get; set; }
        public Guid NotificationTypeId { get; set; }
        public bool IsApproved { get; set; }
    }
}
