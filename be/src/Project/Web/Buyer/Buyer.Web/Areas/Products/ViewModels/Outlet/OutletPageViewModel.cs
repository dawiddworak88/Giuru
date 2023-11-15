using Buyer.Web.Shared.ViewModels.Base;
using Foundation.PageContent.Components.MainNavigations.ViewModels;
using Foundation.PageContent.Components.Metadatas.ViewModels;

namespace Buyer.Web.Areas.Products.ViewModels
{
    public class OutletPageViewModel : BasePageViewModel
    {
        public MetadataViewModel Metadata { get; set; }
        public MainNavigationViewModel MainNavigation { get; set; }
        public OutletPageCatalogViewModel Catalog { get; set; }
    }
}
