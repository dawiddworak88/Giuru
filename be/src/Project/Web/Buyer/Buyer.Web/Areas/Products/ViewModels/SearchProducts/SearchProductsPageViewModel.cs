using Buyer.Web.Shared.ViewModels.Base;
using Buyer.Web.Shared.ViewModels.Headers;
using Foundation.PageContent.Components.MainNavigations.ViewModels;
using Foundation.PageContent.Components.Metadatas.ViewModels;

namespace Buyer.Web.Areas.Products.ViewModels.SearchProducts
{
    public class SearchProductsPageViewModel : BasePageViewModel
    {
        public MetadataViewModel Metadata { get; set; }
        public MainNavigationViewModel MainNavigation { get; set; }
        public SearchProductsCatalogViewModel Catalog { get; set; }
    }
}
