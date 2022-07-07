using Foundation.PageContent.Components.ListItems.ViewModels;
using System.Collections.Generic;

namespace Seller.Web.Areas.Download.ViewModel
{
    public class DownloadFormViewModel
    {
        public IEnumerable<ListItemViewModel> Categories { get; set; }
    }
}
