using Buyer.Web.Shared.ViewModels.Headers;
using Foundation.PageContent.Components.Footers.ViewModels;
using Foundation.PageContent.Components.MainNavigations.ViewModels;
using Foundation.PageContent.Components.Metadatas.ViewModels;

namespace Buyer.Web.Areas.News.ViewModel
{
    public class NewsPageViewModel 
    {
        public string Locale { get; set; }
        public MetadataViewModel Metadata { get; set; }
        public BuyerHeaderViewModel Header { get; set; }
        public MainNavigationViewModel MainNavigation { get; set; }
        public NewsCatalogViewModel NewsCatalog { get; set; }
        public FooterViewModel Footer { get; set; }
    }
}
