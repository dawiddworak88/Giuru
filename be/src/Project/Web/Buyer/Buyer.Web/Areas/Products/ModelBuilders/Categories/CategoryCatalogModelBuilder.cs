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
using Buyer.Web.Shared.DomainModels.Prices;
using System.Linq;
using Buyer.Web.Shared.Services.Prices;
using Microsoft.Extensions.Options;
using Buyer.Web.Shared.Configurations;
using System;
using Buyer.Web.Areas.Products.ViewModels.Products;
using Buyer.Web.Shared.ViewModels.Catalogs;
using System.Collections.Generic;

namespace Buyer.Web.Areas.Products.ModelBuilders.Categories
{
    public class CategoryCatalogModelBuilder : IAsyncComponentModelBuilder<SearchProductsComponentModel, CategoryCatalogViewModel>
    {
        private readonly ICatalogModelBuilder<SearchProductsComponentModel, CategoryCatalogViewModel> catalogModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, SidebarViewModel> sidebarModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, ModalViewModel> modalModelBuilder;
        private readonly IProductsService productsService;
        private readonly ICategoryRepository categoryRepository;
        private readonly IOptions<AppSettings> _options;
        private readonly IPriceService _priceService;

        public CategoryCatalogModelBuilder(
            ICatalogModelBuilder<SearchProductsComponentModel, CategoryCatalogViewModel> catalogModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, SidebarViewModel> sidebarModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, ModalViewModel> modalModelBuilder,
            IProductsService productsService,
            ICategoryRepository categoryRepository,
            IOptions<AppSettings> options,
            IPriceService priceService)
        {
            this.catalogModelBuilder = catalogModelBuilder;
            this.sidebarModelBuilder = sidebarModelBuilder;
            this.productsService = productsService;
            this.categoryRepository = categoryRepository;
            this.modalModelBuilder = modalModelBuilder;
            _options = options;
            _priceService = priceService;
        }

        public async Task<CategoryCatalogViewModel> BuildModelAsync(SearchProductsComponentModel componentModel)
        {
            var viewModel = this.catalogModelBuilder.BuildModel(componentModel);

            var category = await this.categoryRepository.GetCategoryAsync(componentModel.Id, componentModel.Token, componentModel.Language);

            if (category != null)
            {
                viewModel.Title = category.Name;
                viewModel.CategoryId = category.Id;
                viewModel.Sidebar = await this.sidebarModelBuilder.BuildModelAsync(componentModel);
                viewModel.Modal = await this.modalModelBuilder.BuildModelAsync(componentModel);
                viewModel.OrderBy = nameof(Product.Name);

                var products = await this.productsService.GetProductsAsync(
                    null,
                    componentModel.Id,
                    null,
                    componentModel.Language,
                    componentModel.SearchTerm,
                    false,
                    PaginationConstants.DefaultPageIndex,
                    ProductConstants.ProductsCatalogPaginationPageSize,
                    componentModel.Token);

                if (products is not null)
                {
                    var prices = Enumerable.Empty<Price>();

                    if (string.IsNullOrWhiteSpace(_options.Value.GrulaAccessToken) is false)
                    {
                        prices = await _priceService.GetPrices(
                            _options.Value.GrulaAccessToken,
                            DateTime.UtcNow,
                            products.Data.Select(x => new PriceProduct
                            {
                                PrimarySku = x.PrimaryProductSku,
                                FabricsGroup = x.FabricsGroup,
                                SleepAreaSize = x.SleepAreaSize,
                                ExtraPacking = x.ExtraPacking
                            }),
                            new PriceClient
                            {
                                Name = componentModel.Name,
                                CurrencyCode = componentModel.CurrencyCode,
                                ExtraPacking = componentModel.ExtraPacking,
                                PaletteLoading = componentModel.PaletteLoading,
                                Country = componentModel.Country,
                                DeliveryZipCode = componentModel.DeliveryZipCode
                            });
                    }

                    for (int i = 0; i < products.Data.Count(); i++)
                    {
                        var product = products.Data.ElementAtOrDefault(i);

                        if (product is null)
                        {
                            continue;
                        }

                        if (prices.Any())
                        {
                            var price = prices.ElementAtOrDefault(i);

                            if (price is not null)
                            {
                                product.Price = new ProductPriceViewModel
                                {
                                    Current = price.Amount,
                                    Currency = price.CurrencyCode
                                };
                            }
                        }
                    }

                    viewModel.PagedItems = new PagedResults<IEnumerable<CatalogItemViewModel>>(products.Total, products.PageSize)
                    {
                        Data = products.Data.OrderBy(x => x.Title)
                    };
                }
            }

            return viewModel;
        }
    }
}
