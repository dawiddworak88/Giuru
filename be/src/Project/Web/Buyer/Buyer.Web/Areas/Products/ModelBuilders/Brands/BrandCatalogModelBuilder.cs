using Buyer.Web.Areas.Shared.Definitions.Products;
using Buyer.Web.Areas.Products.Services.Products;
using Buyer.Web.Areas.Products.ViewModels.Brands;
using Buyer.Web.Shared.ModelBuilders.Catalogs;
using Foundation.Extensions.ModelBuilders;
using Foundation.GenericRepository.Paginations;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Microsoft.Extensions.Localization;
using System.Threading.Tasks;
using Buyer.Web.Shared.ViewModels.Sidebar;
using Buyer.Web.Areas.Orders.Repositories.Baskets;
using Foundation.Extensions.ExtensionMethods;
using System.Linq;
using Buyer.Web.Areas.Orders.ApiResponseModels;
using System.Globalization;
using Microsoft.AspNetCore.Routing;
using System;
using Newtonsoft.Json;

namespace Buyer.Web.Areas.Products.ModelBuilders.Brands
{
    public class BrandCatalogModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, BrandCatalogViewModel>
    {
        private readonly ICatalogModelBuilder<ComponentModelBase, BrandCatalogViewModel> catalogModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, SidebarViewModel> sidebarModelBuilder;
        private readonly IProductsService productsService;
        private readonly IStringLocalizer<ProductResources> productLocalizer;
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;
        private readonly IBasketRepository basketRepository;
        private readonly LinkGenerator linkGenerator;

        public BrandCatalogModelBuilder(
            ICatalogModelBuilder<ComponentModelBase, BrandCatalogViewModel> catalogModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, SidebarViewModel> sidebarModelBuilder,
            IProductsService productsService,
            IStringLocalizer<ProductResources> productLocalizer,
            IBasketRepository basketRepository,
            LinkGenerator linkGenerator,
            IStringLocalizer<GlobalResources> globalLocalizer)
        {
            this.catalogModelBuilder = catalogModelBuilder;
            this.productsService = productsService;
            this.productLocalizer = productLocalizer;
            this.sidebarModelBuilder = sidebarModelBuilder;
            this.globalLocalizer = globalLocalizer;
            this.basketRepository = basketRepository;
            this.linkGenerator = linkGenerator;
        }

        public async Task<BrandCatalogViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = this.catalogModelBuilder.BuildModel(componentModel);
            viewModel.Sidebar = await sidebarModelBuilder.BuildModelAsync(componentModel);
            viewModel.BrandId = componentModel.Id;
            viewModel.Title = this.productLocalizer.GetString("Products");
            viewModel.PagedItems = await this.productsService.GetProductsAsync(
                null, null, componentModel.Id, componentModel.Language, null, PaginationConstants.DefaultPageIndex, ProductConstants.ProductsCatalogPaginationPageSize, componentModel.Token);

            if (componentModel.IsAuthenticated && componentModel.BasketId.HasValue)
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
