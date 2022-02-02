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
using Buyer.Web.Areas.Orders.Repositories.Baskets;
using Microsoft.AspNetCore.Routing;
using Foundation.Extensions.ExtensionMethods;
using System.Linq;
using Buyer.Web.Areas.Orders.ApiResponseModels;
using System.Globalization;
using System;
using Newtonsoft.Json;

namespace Buyer.Web.Areas.Products.ModelBuilders.SearchProducts
{
    public class SearchProductsCatalogModelBuilder : IAsyncComponentModelBuilder<SearchProductsComponentModel, SearchProductsCatalogViewModel>
    {
        private readonly ICatalogModelBuilder<SearchProductsComponentModel, SearchProductsCatalogViewModel> searchProductsCatalogModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, SidebarViewModel> sidebarModelBuilder;
        private readonly IProductsService productsService;
        private readonly IBasketRepository basketRepository;
        private readonly LinkGenerator linkGenerator;

        public SearchProductsCatalogModelBuilder(
            ICatalogModelBuilder<SearchProductsComponentModel, SearchProductsCatalogViewModel> searchProductsCatalogModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, SidebarViewModel> sidebarModelBuilder,
            IBasketRepository basketRepository,
            LinkGenerator linkGenerator,
            IProductsService productsService)
        {
            this.searchProductsCatalogModelBuilder = searchProductsCatalogModelBuilder;
            this.productsService = productsService;
            this.sidebarModelBuilder = sidebarModelBuilder;
            this.basketRepository = basketRepository;
            this.linkGenerator = linkGenerator;
        }

        public async Task<SearchProductsCatalogViewModel> BuildModelAsync(SearchProductsComponentModel componentModel)
        {
            var viewModel = this.searchProductsCatalogModelBuilder.BuildModel(componentModel);

            viewModel.Title = componentModel.SearchTerm;
            viewModel.Sidebar = await this.sidebarModelBuilder.BuildModelAsync(componentModel);
            viewModel.PagedItems = await this.productsService.GetProductsAsync(
                null,
                null,
                null,
                componentModel.Language,
                componentModel.SearchTerm,
                PaginationConstants.DefaultPageIndex,
                ProductConstants.ProductsCatalogPaginationPageSize,
                componentModel.Token);

            if (componentModel.IsAuthenticated)
            {
                var existingBasket = await this.basketRepository.GetBasketById(componentModel.Token, componentModel.Language, componentModel.BasketId);
                if (existingBasket != null)
                {
                    var productIds = existingBasket.Items.OrEmptyIfNull().Select(x => x.ProductId.Value);
                    if (productIds.OrEmptyIfNull().Any())
                    {
                        var basketResponseModel = existingBasket.Items.OrEmptyIfNull().Select(x => new BasketItemResponseModel
                        {
                            ProductId = x.ProductId,
                            ProductUrl = this.linkGenerator.GetPathByAction("Edit", "Product", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name, Id = x.ProductId }),
                            Name = x.ProductName,
                            Sku = x.ProductSku,
                            Quantity = x.Quantity,
                            ExternalReference = x.ExternalReference,
                            ImageSrc = x.PictureUrl,
                            ImageAlt = x.ProductName,
                            DeliveryFrom = x.DeliveryFrom,
                            DeliveryTo = x.DeliveryTo,
                            MoreInfo = x.MoreInfo
                        });
                        viewModel.OrderItems = basketResponseModel;
                    }
                }
            }

            return viewModel;
        }
    }
}
