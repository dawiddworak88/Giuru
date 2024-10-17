using System;

namespace Identity.Api.Areas.Accounts.Models
{
    public class ClientNotificationTypeApproval
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsApproved { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
