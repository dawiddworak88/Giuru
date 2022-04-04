using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Foundation.PageContent.Components.Footers.ViewModels;
using Foundation.PageContent.Components.Headers.ViewModels;
using Foundation.PageContent.MenuTiles.ViewModels;
using Seller.Web.Areas.News.ViewModel;
using System.Globalization;
using System.Threading.Tasks;

namespace Seller.Web.Areas.News.ModelBuilders
{
    public class NewsItemPageModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, NewsItemPageViewModel>
    {
        private readonly IModelBuilder<HeaderViewModel> headerModelBuilder;
        private readonly IModelBuilder<MenuTilesViewModel> menuTilesModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, NewsItemFormViewModel> newsItemFormModelBuilder;
        private readonly IModelBuilder<FooterViewModel> footerModelBuilder;

        public NewsItemPageModelBuilder(
            IModelBuilder<HeaderViewModel> headerModelBuilder,
            IModelBuilder<MenuTilesViewModel> menuTilesModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, NewsItemFormViewModel> newsItemFormModelBuilder,
            IModelBuilder<FooterViewModel> footerModelBuilder)
        {
            this.headerModelBuilder = headerModelBuilder;
            this.menuTilesModelBuilder = menuTilesModelBuilder;
            this.newsItemFormModelBuilder = newsItemFormModelBuilder;
            this.footerModelBuilder = footerModelBuilder;
        }

        public async Task<NewsItemPageViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new NewsItemPageViewModel
            {
                Locale = CultureInfo.CurrentUICulture.Name,
                Header = this.headerModelBuilder.BuildModel(),
                MenuTiles = this.menuTilesModelBuilder.BuildModel(),
                NewsItemForm = await this.newsItemFormModelBuilder.BuildModelAsync(componentModel),
                Footer = this.footerModelBuilder.BuildModel()
            };

            return viewModel;
        }
    }
}
