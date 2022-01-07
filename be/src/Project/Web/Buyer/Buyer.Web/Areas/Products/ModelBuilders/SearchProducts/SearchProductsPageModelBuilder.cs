using Buyer.Web.Areas.Products.ComponentModels;
using Buyer.Web.Areas.Products.ViewModels.SearchProducts;
using Buyer.Web.Shared.ViewModels.Headers;
using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Foundation.PageContent.Components.Footers.ViewModels;
using Foundation.PageContent.Components.MainNavigations.ViewModels;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Products.ModelBuilders.SearchProducts
{
    public class SearchProductsPageModelBuilder : IAsyncComponentModelBuilder<SearchProductsComponentModel, SearchProductsPageViewModel>
    {
        private readonly IModelBuilder<BuyerHeaderViewModel> headerModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, MainNavigationViewModel> mainNavigationModelBuilder;
        private readonly IAsyncComponentModelBuilder<SearchProductsComponentModel, SearchProductsCatalogViewModel> searchProductsCatalogModelBuilder;
        private readonly IModelBuilder<FooterViewModel> footerModelBuilder;

        public SearchProductsPageModelBuilder(
            IModelBuilder<BuyerHeaderViewModel> headerModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, MainNavigationViewModel> mainNavigationModelBuilder,
            IAsyncComponentModelBuilder<SearchProductsComponentModel, SearchProductsCatalogViewModel> searchProductsCatalogModelBuilder,
            IModelBuilder<FooterViewModel> footerModelBuilder)
        {
            this.headerModelBuilder = headerModelBuilder;
            this.mainNavigationModelBuilder = mainNavigationModelBuilder;
            this.searchProductsCatalogModelBuilder = searchProductsCatalogModelBuilder;
            this.footerModelBuilder = footerModelBuilder;
        }

        public async Task<SearchProductsPageViewModel> BuildModelAsync(SearchProductsComponentModel componentModel)
        {
            var viewModel = new SearchProductsPageViewModel
            {
                Header = headerModelBuilder.BuildModel(),
                MainNavigation = await this.mainNavigationModelBuilder.BuildModelAsync(new ComponentModelBase { Id = componentModel.Id, Token = componentModel.Token, IsAuthenticated = componentModel.IsAuthenticated, Language = componentModel.Language }),
                Catalog = await this.searchProductsCatalogModelBuilder.BuildModelAsync(componentModel),
                Footer = footerModelBuilder.BuildModel()
            };

            viewModel.Header.SearchTerm = componentModel.SearchTerm;

            return viewModel;
        }
    }
}
