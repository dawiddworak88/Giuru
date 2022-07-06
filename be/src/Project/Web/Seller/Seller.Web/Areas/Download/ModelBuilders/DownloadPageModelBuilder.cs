using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Foundation.PageContent.Components.Footers.ViewModels;
using Foundation.PageContent.Components.Headers.ViewModels;
using Foundation.PageContent.MenuTiles.ViewModels;
using Seller.Web.Areas.Download.ViewModel;
using System.Globalization;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Download.ModelBuilders
{
    public class DownloadPageModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, DownloadPageViewModel>
    {
        private readonly IModelBuilder<HeaderViewModel> headerModelBuilder;
        private readonly IModelBuilder<MenuTilesViewModel> menuTilesModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, DownloadFormViewModel> downloadFormModelBuilder;
        private readonly IModelBuilder<FooterViewModel> footerModelBuilder;

        public DownloadPageModelBuilder(
            IModelBuilder<HeaderViewModel> headerModelBuilder,
            IModelBuilder<MenuTilesViewModel> menuTilesModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, DownloadFormViewModel> downloadFormModelBuilder,
            IModelBuilder<FooterViewModel> footerModelBuilder)
        {
            this.headerModelBuilder = headerModelBuilder;
            this.menuTilesModelBuilder = menuTilesModelBuilder;
            this.downloadFormModelBuilder = downloadFormModelBuilder;
            this.footerModelBuilder = footerModelBuilder;
        }

        public async Task<DownloadPageViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new DownloadPageViewModel
            {
                Locale = CultureInfo.CurrentUICulture.Name,
                Header = this.headerModelBuilder.BuildModel(),
                MenuTiles = this.menuTilesModelBuilder.BuildModel(),
                DownloadForm = await this.downloadFormModelBuilder.BuildModelAsync(componentModel),
                Footer = this.footerModelBuilder.BuildModel()
            };

            return viewModel;
        }
    }
}
