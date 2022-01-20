using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Foundation.PageContent.Components.Footers.ViewModels;
using Foundation.PageContent.Components.Headers.ViewModels;
using Foundation.PageContent.MenuTiles.ViewModels;
using Seller.Web.Areas.Media.DomainModels;
using Seller.Web.Areas.Media.ViewModel;
using Seller.Web.Shared.ViewModels;
using System.Globalization;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Media.ModelBuilders
{
    public class MediaPageModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, MediaPageViewModel>
    {
        private readonly IModelBuilder<HeaderViewModel> headerModelBuilder;
        private readonly IModelBuilder<MenuTilesViewModel> menuTilesModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, CatalogViewModel<MediaItem>> mediaCatalogModelBuilder;
        private readonly IModelBuilder<FooterViewModel> footerModelBuilder;

        public MediaPageModelBuilder(
            IModelBuilder<HeaderViewModel> headerModelBuilder,
            IModelBuilder<MenuTilesViewModel> menuTilesModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, CatalogViewModel<MediaItem>> mediaCatalogModelBuilder,
            IModelBuilder<FooterViewModel> footerModelBuilder)
        {
            this.headerModelBuilder = headerModelBuilder;
            this.menuTilesModelBuilder = menuTilesModelBuilder;
            this.mediaCatalogModelBuilder = mediaCatalogModelBuilder;
            this.footerModelBuilder = footerModelBuilder;
        }

        public async Task<MediaPageViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new MediaPageViewModel
            {
                Locale = CultureInfo.CurrentUICulture.Name,
                Header = this.headerModelBuilder.BuildModel(),
                MenuTiles = this.menuTilesModelBuilder.BuildModel(),
                Catalog = await this.mediaCatalogModelBuilder.BuildModelAsync(componentModel),
                Footer = footerModelBuilder.BuildModel()
            };

            return viewModel;
        }
    }
}
