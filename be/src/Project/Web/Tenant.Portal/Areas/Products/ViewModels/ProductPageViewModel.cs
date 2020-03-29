using Feature.PageContent.Components.Footers.ViewModels;
using Feature.PageContent.Components.Headers.ViewModels;
using Feature.PageContent.MenuTiles.ViewModels;

namespace Tenant.Portal.Areas.Products.ViewModels
{
    public class ProductPageViewModel
    {
        public HeaderViewModel Header { get; set; }
        public MenuTilesViewModel MenuTiles { get; set; }
        public ProductCatalogViewModel Catalog { get; set; }
        public FooterViewModel Footer { get; set; }
    }
}
