using System.Collections.Generic;

namespace Buyer.Web.Shared.DomainModels.GraphQl.NotificationBars
{
    public class NotificationBar
    {
        public IEnumerable<NotificationBarItem> Items { get; set; }
    }
}
