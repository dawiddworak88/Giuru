using Buyer.Web.Areas.Orders.ModelBuilders;
using Buyer.Web.Shared.Configurations;
using Buyer.Web.Shared.Repositories.Clients;
using Buyer.Web.Shared.Services.Baskets;
using Buyer.Web.Shared.Services.Prices;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using NSubstitute;
using System;
using System.Threading.Tasks;
using BuyerBasket = Buyer.Web.Shared.DomainModels.Baskets.Basket;

namespace Giuru.UnitTests.Orders.Baskets
{
    // The buyer's order form keeps showing a stored code that stopped being valid: checkout refuses the
    // Basket until the code is removed, and the form only offers removal for a code it shows.
    public class OrderFormDiscountCodeTests
    {
        private static readonly Guid BasketId = Guid.NewGuid();

        [Fact]
        public async Task OrderForm_WhenTheStoredCodeIsValid_ShowsItsStoredSpelling()
        {
            var builder = CreateBuilder("summer25", DiscountCodeCheck.Valid("SUMMER25"));

            var form = await builder.BuildModelAsync(CreateComponentModel());

            Assert.Equal("SUMMER25", form.DiscountCode);
            Assert.False(form.IsDiscountCodeNotValid);
        }

        [Fact]
        public async Task OrderForm_WhenTheStoredCodeIsNoLongerValid_ShowsItMarkedNotValidSoItCanBeRemoved()
        {
            var builder = CreateBuilder("SUMMER25", DiscountCodeCheck.NotValid("SUMMER25"));

            var form = await builder.BuildModelAsync(CreateComponentModel());

            Assert.Equal("SUMMER25", form.DiscountCode);
            Assert.True(form.IsDiscountCodeNotValid);
        }

        [Fact]
        public async Task OrderForm_WhenTheStoredCodeCannotBeChecked_ShowsItWithoutCallingItNotValid()
        {
            var builder = CreateBuilder("SUMMER25", DiscountCodeCheck.Unchecked("SUMMER25"));

            var form = await builder.BuildModelAsync(CreateComponentModel());

            Assert.Equal("SUMMER25", form.DiscountCode);
            Assert.False(form.IsDiscountCodeNotValid);
        }

        private static ComponentModelBase CreateComponentModel()
        {
            return new ComponentModelBase { BasketId = BasketId, Token = "token", Language = "en" };
        }

        private static OrderFormModelBuilder CreateBuilder(string storedDiscountCode, DiscountCodeCheck check)
        {
            var basketService = Substitute.For<IBasketService>();
            basketService.GetBasketAsync(BasketId, "token", "en").Returns(new BuyerBasket { Id = BasketId, DiscountCode = storedDiscountCode });

            var discountCodeService = Substitute.For<IApplicableDiscountCodeService>();
            discountCodeService.CheckAsync("token", storedDiscountCode).Returns(check);

            return new OrderFormModelBuilder(
                Substitute.For<IStringLocalizer<GlobalResources>>(),
                Substitute.For<IStringLocalizer<OrderResources>>(),
                Substitute.For<IStringLocalizer<ClientResources>>(),
                Substitute.For<IClientsRepository>(),
                Substitute.For<IClientAddressesRepository>(),
                basketService,
                Substitute.For<LinkGenerator>(),
                Options.Create(new AppSettings { GrulaAccessToken = "test-token", GrulaEnvironmentId = Guid.NewGuid().ToString() }),
                discountCodeService);
        }
    }
}
