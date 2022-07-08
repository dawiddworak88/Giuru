using Foundation.PageContent.Components.ListItems.ViewModels;
using System.Collections.Generic;

namespace Seller.Web.Areas.DownloadCenter.ViewModel
{
    public class DownloadCenterItemFormViewModel
    {
        public IEnumerable<ListItemViewModel> Categories { get; set; }
    }
}
