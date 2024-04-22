using System;
using System.Collections.Generic;

namespace Identity.Api.Areas.Accounts.Models
{
    public class ClientNotificationTypeApprovals
    {
        public Guid ClientId { get; set; }
        public IEnumerable<Guid> ClientApprovals { get; set; }
    }
}
