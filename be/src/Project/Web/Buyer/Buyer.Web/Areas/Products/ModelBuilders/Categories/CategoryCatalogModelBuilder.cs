using Buyer.Web.Areas.Products.ComponentModels;
using Buyer.Web.Areas.Products.DomainModels;
using Buyer.Web.Areas.Products.Repositories.Categories;
using Buyer.Web.Areas.Products.Services.Products;
using Buyer.Web.Areas.Products.ViewModels.Categories;
using Buyer.Web.Areas.Shared.Definitions.Products;
using Buyer.Web.Shared.ModelBuilders.Catalogs;
using Buyer.Web.Shared.ViewModels.Filters;
using Buyer.Web.Shared.ViewModels.Modals;
using Buyer.Web.Shared.ViewModels.Sidebar;
using Foundation.Extensions.ModelBuilders;
using Foundation.GenericRepository.Definitions;
using Foundation.GenericRepository.Paginations;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Products.ModelBuilders.Categories
{
    public class CategoryCatalogModelBuilder : IAsyncComponentModelBuilder<SearchProductsComponentModel, CategoryCatalogViewModel>
    {
        private readonly ICatalogModelBuilder<SearchProductsComponentModel, CategoryCatalogViewModel> _catalogModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, SidebarViewModel> _sidebarModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, ModalViewModel> _modalModelBuilder;
        private readonly IProductsService _productsService;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IStringLocalizer<ProductResources> _productLocalizer;

        public CategoryCatalogModelBuilder(
            ICatalogModelBuilder<SearchProductsComponentModel, CategoryCatalogViewModel> catalogModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, SidebarViewModel> sidebarModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, ModalViewModel> modalModelBuilder,
            IProductsService productsService,
            ICategoryRepository categoryRepository,
            IStringLocalizer<ProductResources> productLocalizer)
        {
            _catalogModelBuilder = catalogModelBuilder;
            _sidebarModelBuilder = sidebarModelBuilder;
            _productsService = productsService;
            _categoryRepository = categoryRepository;
            _modalModelBuilder = modalModelBuilder;
            _productLocalizer = productLocalizer;
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
                    null,
                    componentModel.Language,
                    componentModel.SearchTerm,
                    false,
                    Constants.DefaultPageIndex,
                    Constants.DefaultItemsPerPage,
                    componentModel.Token,
                    SortingConstants.Default);

                viewModel.FilterCollector = new FiltersCollectorViewModel
                {
                    AllFilters = _productLocalizer.GetString("AllFilters"),
                    SortLabel = _productLocalizer.GetString("SortLabel"),
                    ClearAllFilters = _productLocalizer.GetString("ClearAllFilters"),
                    SeeResult = _productLocalizer.GetString("SeeResult"),
                    FiltersLabel = _productLocalizer.GetString("FiltersLabel"),
                    SortItems = new List<SortItemViewModel>
                    {
                        new SortItemViewModel { Label = _productLocalizer.GetString("SortDefault"), Key = SortingConstants.Default },
                        new SortItemViewModel { Label = _productLocalizer.GetString("SortNewest"), Key = SortingConstants.Newest },
                        new SortItemViewModel { Label = _productLocalizer.GetString("SortName"), Key = SortingConstants.Name }
                    }
                };
            }

            return viewModel;
        }
    }
}
