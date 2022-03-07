using Buyer.Web.Areas.News.DomainModels;
using Foundation.GenericRepository.Paginations;
using Foundation.PageContent.Components.ListItems.ViewModels;
using System.Collections.Generic;

namespace Buyer.Web.Areas.News.ViewModel
{
    public class NewsCatalogViewModel
    {
        public string NewsApiUrl { get; set; }
        public string AllCategoryLabel { get; set; }
        public string NoResultsLabel { get; set; }
        public IEnumerable<ListItemViewModel> Categories { get; set; }
        public PagedResults<IEnumerable<NewsItem>> PagedItems { get; set; }
    }
}
