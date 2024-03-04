using Buyer.Web.Shared.ViewModels.Base;
using Foundation.PageContent.Components.Metadatas.ViewModels;

namespace Buyer.Web.Areas.Products.ViewModels.SearchProducts
{
    public class SearchProductsPageViewModel : BasePageViewModel
    {
        public MetadataViewModel Metadata { get; set; }
        public SearchProductsCatalogViewModel Catalog { get; set; }
    }
}
