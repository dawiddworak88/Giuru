using Foundation.GenericRepository.Entities;
using System;

namespace Client.Api.Infrastructure.Notifications.Entities
{
    public class ClientNotificationTypeApproval : Entity
    {
        public Guid ClientId { get; set; }
        public bool IsApproved { get; set; }
        public DateTime ApprovalDate { get; set; }
        public Guid ClientNotificationTypeId { get; set; }
        public virtual ClientNotificationType ClientNotificationType { get; set; }
    }
}
