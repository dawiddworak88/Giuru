using Buyer.Web.Areas.Products.ComponentModels;
using Buyer.Web.Areas.Products.ModelBuilders.SearchProducts;
using Buyer.Web.Areas.Products.Repositories;
using Buyer.Web.Areas.Products.Repositories.Inventories;
using Buyer.Web.Areas.Products.Services.DeliveryMessages;
using Buyer.Web.Areas.Products.Services.Products;
using Buyer.Web.Areas.Products.ViewModels.SearchProducts;
using Buyer.Web.Shared.ModelBuilders.Catalogs;
using Buyer.Web.Shared.Repositories.LeadTime;
using Buyer.Web.Shared.Services.DeliveryDates;
using Buyer.Web.Shared.Services.Prices;
using Buyer.Web.Shared.ViewModels.Catalogs;
using Buyer.Web.Shared.ViewModels.Modals;
using Buyer.Web.Shared.ViewModels.Sidebar;
using Foundation.Extensions.ModelBuilders;
using Foundation.GenericRepository.Paginations;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Foundation.Pricing.DomainModels;
using Foundation.Pricing.Services;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Giuru.UnitTests.Services.Prices
{
    public class SearchProductsCatalogModelBuilderTests
    {
        [Theory]
        [InlineData("SUMMER25", true)]
        [InlineData(null, true)]
        [InlineData("SUMMER25", false)]
        public async Task BuildModelAsync_ResolvesBuyerWithCatalogDiscountOnlyWhenPricingRequestsClient(
            string discountCode, bool requestClient)
        {
            var componentModel = new SearchProductsComponentModel { Token = "buyer-token", Language = "en" };
            var catalogModel = new SearchProductsCatalogViewModel { DiscountCode = discountCode };
            var catalogBuilder = Substitute.For<ICatalogModelBuilder<SearchProductsComponentModel, SearchProductsCatalogViewModel>>();
            catalogBuilder.BuildModel(componentModel).Returns(catalogModel);
            var productsService = Substitute.For<IProductsService>();
            productsService.GetProductsAsync(default, default, default, default, default, default, default, default, default)
                .ReturnsForAnyArgs(new PagedResults<IEnumerable<CatalogItemViewModel>>(0, 20)
                {
                    Data = Array.Empty<CatalogItemViewModel>()
                });

            var resolver = Substitute.For<IPriceClientResolver>();
            var expectedClient = new PriceClient { Id = Guid.NewGuid(), DiscountCode = discountCode };
            resolver.ResolveAsync(null, discountCode, componentModel.Token).Returns(expectedClient);
            var pricingService = Substitute.For<IProductPricingService>();
            PriceClient pricedClient = null;
            pricingService.GetPricesAsync(
                    Arg.Any<Func<Task<IEnumerable<PriceProduct>>>>(),
                    Arg.Any<Func<Task<PriceClient>>>(),
                    Arg.Any<DateTime?>())
                .Returns(async call =>
                {
                    if (requestClient)
                    {
                        pricedClient = await call.Arg<Func<Task<PriceClient>>>()();
                    }

                    return PricedProducts.Empty;
                });

            var builder = new SearchProductsCatalogModelBuilder(
                catalogBuilder,
                Substitute.For<IAsyncComponentModelBuilder<ComponentModelBase, ModalViewModel>>(),
                Substitute.For<IAsyncComponentModelBuilder<ComponentModelBase, SidebarViewModel>>(),
                Substitute.For<IStringLocalizer<GlobalResources>>(),
                productsService,
                Substitute.For<IOutletRepository>(),
                Substitute.For<IInventoryRepository>(),
                Substitute.For<LinkGenerator>(),
                Substitute.For<ILeadTimeRepository>(),
                Substitute.For<IDeliveryMessageHelper>(),
                Substitute.For<IExpectedDeliveryDateService>(),
                Substitute.For<IPriceProductFactory>(),
                pricingService,
                resolver);

            var result = await builder.BuildModelAsync(componentModel);

            Assert.Same(catalogModel, result);
            Assert.Equal(discountCode, result.DiscountCode);
            await pricingService.Received(1).GetPricesAsync(
                Arg.Any<Func<Task<IEnumerable<PriceProduct>>>>(), Arg.Any<Func<Task<PriceClient>>>(), null);
            if (requestClient)
            {
                await resolver.Received(1).ResolveAsync(null, discountCode, componentModel.Token);
                Assert.Same(expectedClient, pricedClient);
            }
            else
            {
                Assert.Empty(resolver.ReceivedCalls());
            }
        }
    }
}
