using Buyer.Web.Areas.Products.ComponentModels;
using Buyer.Web.Areas.Shared.Definitions.Products;
using Buyer.Web.Areas.Products.Services.Products;
using Buyer.Web.Areas.Products.ViewModels.SearchProducts;
using Buyer.Web.Shared.ModelBuilders.Catalogs;
using Foundation.Extensions.ModelBuilders;
using Foundation.GenericRepository.Paginations;
using System.Threading.Tasks;
using Buyer.Web.Shared.ViewModels.Sidebar;
using Foundation.PageContent.ComponentModels;
using Buyer.Web.Shared.ViewModels.Modals;

namespace Buyer.Web.Areas.Products.ModelBuilders.SearchProducts
{
    public class SearchProductsCatalogModelBuilder : IAsyncComponentModelBuilder<SearchProductsComponentModel, SearchProductsCatalogViewModel>
    {
        private readonly ICatalogModelBuilder<SearchProductsComponentModel, SearchProductsCatalogViewModel> searchProductsCatalogModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, SidebarViewModel> sidebarModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, ModalViewModel> modalModelBuilder;
        private readonly IProductsService productsService;

        public SearchProductsCatalogModelBuilder(
            ICatalogModelBuilder<SearchProductsComponentModel, SearchProductsCatalogViewModel> searchProductsCatalogModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, ModalViewModel> modalModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, SidebarViewModel> sidebarModelBuilder,
            IProductsService productsService)
        {
            this.searchProductsCatalogModelBuilder = searchProductsCatalogModelBuilder;
            this.productsService = productsService;
            this.sidebarModelBuilder = sidebarModelBuilder;
            this.modalModelBuilder = modalModelBuilder;
        }

        public async Task<SearchProductsCatalogViewModel> BuildModelAsync(SearchProductsComponentModel componentModel)
        {
            var viewModel = this.searchProductsCatalogModelBuilder.BuildModel(componentModel);

            viewModel.Title = componentModel.SearchTerm;
            viewModel.Sidebar = await this.sidebarModelBuilder.BuildModelAsync(componentModel);
            viewModel.Modal = await this.modalModelBuilder.BuildModelAsync(componentModel);
            viewModel.PagedItems = await this.productsService.GetProductsAsync(
                null,
                null,
                null,
                componentModel.Language,
                componentModel.SearchTerm,
                PaginationConstants.DefaultPageIndex,
                ProductConstants.ProductsCatalogPaginationPageSize,
                componentModel.Token);

            return viewModel;
        }
    }
}
