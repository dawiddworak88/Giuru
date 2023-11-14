using Foundation.PageContent.Components.Links.ViewModels;
using System.Collections;
using System.Collections.Generic;

namespace Foundation.PageContent.Components.NotificationBar.ViewModels
{
    public class NotificationBarViewModel
    {
        public IEnumerable<NotificationBarItemViewModel> Items { get; set; }
    }
}
