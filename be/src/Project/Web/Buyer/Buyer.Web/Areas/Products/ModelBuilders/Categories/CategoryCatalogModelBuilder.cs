using Buyer.Web.Areas.Products.ComponentModels;
using Buyer.Web.Areas.Shared.Definitions.Products;
using Buyer.Web.Areas.Products.Repositories.Categories;
using Buyer.Web.Areas.Products.Services.Products;
using Buyer.Web.Areas.Products.ViewModels.Categories;
using Buyer.Web.Shared.ModelBuilders.Catalogs;
using Foundation.Extensions.ModelBuilders;
using Foundation.GenericRepository.Paginations;
using System.Threading.Tasks;
using Foundation.PageContent.ComponentModels;
using Buyer.Web.Shared.ViewModels.Sidebar;
using Buyer.Web.Shared.ViewModels.Modals;
using Buyer.Web.Areas.Products.DomainModels;

namespace Buyer.Web.Areas.Products.ModelBuilders.Categories
{
    public class CategoryCatalogModelBuilder : IAsyncComponentModelBuilder<SearchProductsComponentModel, CategoryCatalogViewModel>
    {
        private readonly ICatalogModelBuilder<SearchProductsComponentModel, CategoryCatalogViewModel> _catalogModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, SidebarViewModel> _sidebarModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, ModalViewModel> _modalModelBuilder;
        private readonly IProductsService _productsService;
        private readonly ICategoryRepository _categoryRepository;

        public CategoryCatalogModelBuilder(
            ICatalogModelBuilder<SearchProductsComponentModel, CategoryCatalogViewModel> catalogModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, SidebarViewModel> sidebarModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, ModalViewModel> modalModelBuilder,
            IProductsService productsService,
            ICategoryRepository categoryRepository)
        {
            _catalogModelBuilder = catalogModelBuilder;
            _sidebarModelBuilder = sidebarModelBuilder;
            _productsService = productsService;
            _categoryRepository = categoryRepository;
            _modalModelBuilder = modalModelBuilder;
        }

        public async Task<CategoryCatalogViewModel> BuildModelAsync(SearchProductsComponentModel componentModel)
        {
            var viewModel = _catalogModelBuilder.BuildModel(componentModel);

            var category = await _categoryRepository.GetCategoryAsync(componentModel.Id, componentModel.Token, componentModel.Language);

            if (category != null)
            {
                viewModel.Title = category.Name;
                viewModel.CategoryId = category.Id;
                viewModel.Sidebar = await _sidebarModelBuilder.BuildModelAsync(componentModel);
                viewModel.Modal = await _modalModelBuilder.BuildModelAsync(componentModel);
                viewModel.OrderBy = nameof(Product.Name);
                viewModel.PagedItems = await _productsService.GetProductsAsync(
                    null,
                    componentModel.Id,
                    componentModel.SellerId,
                    componentModel.UserEmail,
                    componentModel.Language,
                    componentModel.SearchTerm,
                    false,
                    PaginationConstants.DefaultPageIndex,
                    ProductConstants.ProductsCatalogPaginationPageSize,
                    componentModel.Token);
            }

            return viewModel;
        }
    }
}
