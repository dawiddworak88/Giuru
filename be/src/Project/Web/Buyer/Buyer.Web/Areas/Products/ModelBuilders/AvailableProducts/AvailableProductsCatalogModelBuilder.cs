using Buyer.Web.Areas.Shared.Definitions.Products;
using Buyer.Web.Areas.Products.Services.Products;
using Buyer.Web.Areas.Products.ViewModels.AvailableProducts;
using Buyer.Web.Shared.ModelBuilders.Catalogs;
using Foundation.Extensions.ModelBuilders;
using Foundation.GenericRepository.Paginations;
using System.Threading.Tasks;
using Foundation.PageContent.ComponentModels;
using Microsoft.Extensions.Localization;
using Foundation.Localization;
using Buyer.Web.Areas.Products.Repositories.Inventories;
using System.Linq;
using Microsoft.AspNetCore.Routing;
using Buyer.Web.Shared.ViewModels.Catalogs;
using System.Collections.Generic;

namespace Buyer.Web.Areas.Products.ModelBuilders.AvailableProducts
{
    public class AvailableProductsCatalogModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, AvailableProductsCatalogViewModel>
    {
        private readonly IStringLocalizer globalLocalizer;
        private readonly ICatalogModelBuilder<ComponentModelBase, AvailableProductsCatalogViewModel> availableProductsCatalogModelBuilder;
        private readonly IProductsService productsService;
        private readonly IInventoryRepository inventoryRepository;
        private readonly LinkGenerator linkGenerator;

        public AvailableProductsCatalogModelBuilder(
            IStringLocalizer<GlobalResources> globalLocalizer,
            ICatalogModelBuilder<ComponentModelBase, AvailableProductsCatalogViewModel> availableProductsCatalogModelBuilder,
            IProductsService productsService,
            IInventoryRepository inventoryRepository,
            LinkGenerator linkGenerator)
        {
            this.globalLocalizer = globalLocalizer;
            this.availableProductsCatalogModelBuilder = availableProductsCatalogModelBuilder;
            this.productsService = productsService;
            this.inventoryRepository = inventoryRepository;
            this.linkGenerator = linkGenerator;
        }

        public async Task<AvailableProductsCatalogViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = this.availableProductsCatalogModelBuilder.BuildModel(componentModel);

            viewModel.Title = this.globalLocalizer.GetString("AvailableProducts");
            viewModel.ProductsApiUrl = this.linkGenerator.GetPathByAction("Get", "AvailableProductsApi", new { Area = "Products" });
            viewModel.PagedItems = new PagedResults<IEnumerable<CatalogItemViewModel>>(PaginationConstants.EmptyTotal, ProductConstants.ProductsCatalogPaginationPageSize);

            var inventories = await this.inventoryRepository.GetAvailbleProductsInventory(
                componentModel.Language,
                PaginationConstants.DefaultPageIndex,
                ProductConstants.ProductsCatalogPaginationPageSize,
                componentModel.Token);

            if (inventories?.Data is not null && inventories.Data.Any())
            {
                var products = await this.productsService.GetProductsAsync(
                    inventories.Data.Select(x => x.ProductId),
                    null,
                    null,
                    componentModel.Language,
                    null,
                    PaginationConstants.DefaultPageIndex,
                    ProductConstants.ProductsCatalogPaginationPageSize,
                    componentModel.Token);

                if (products is not null)
                {
                    foreach (var product in products.Data)
                    {
                        product.InStock = true;
                        product.AvailableQuantity = inventories.Data.FirstOrDefault(x => x.ProductId == product.Id)?.AvailableQuantity;
                    }

                    viewModel.PagedItems = new PagedResults<IEnumerable<CatalogItemViewModel>>(inventories.Total, ProductConstants.ProductsCatalogPaginationPageSize)
                    {
                        Data = products.Data.OrderByDescending(x => x.AvailableQuantity)
                    };
                }
            }

            return viewModel;
        }
    }
}
