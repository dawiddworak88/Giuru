using Foundation.PageContent.Components.ListItems.ViewModels;
using System.Collections.Generic;

namespace Buyer.Web.Areas.News.ViewModel
{
    public class NewsCatalogViewModel
    {
        public string NewsApiUrl { get; set; }
        public IEnumerable<ListItemViewModel> Categories { get; set; }
    }
}
