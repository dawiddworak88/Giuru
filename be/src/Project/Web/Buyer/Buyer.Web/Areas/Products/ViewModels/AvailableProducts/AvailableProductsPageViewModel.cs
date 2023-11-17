using Buyer.Web.Shared.ViewModels.Base;
using Foundation.PageContent.Components.Metadatas.ViewModels;

namespace Buyer.Web.Areas.Products.ViewModels.AvailableProducts
{
    public class AvailableProductsPageViewModel : BasePageViewModel
    {
        public MetadataViewModel Metadata { get; set; }
        public AvailableProductsCatalogViewModel Catalog { get; set; }
    }
}
