using Foundation.ApiExtensions.Definitions;
using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Foundation.PageContent.Components.DrawerMenu.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using NSubstitute;
using Seller.Web.Areas.Clients.Controllers;
using Seller.Web.Areas.Clients.DomainModels;
using Seller.Web.Areas.Clients.ModelBuilders;
using Seller.Web.Areas.Clients.Repositories.DiscountCodes;
using Seller.Web.Areas.Clients.ViewModels;
using Seller.Web.Shared.Catalogs.ModelBuilders;
using Seller.Web.Shared.Configurations;
using Seller.Web.Shared.ModelBuilders.DrawerMenu;
using Seller.Web.Shared.ModelBuilders.MenuTiles;
using Seller.Web.Shared.ViewModels;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Giuru.UnitTests.Clients.DiscountCodes
{
    // The Seller portal's Discount Codes list is behind the Grula gate: without Grula the page answers 404
    // and the "Discount codes" entry is absent from the tiles and the drawer. With Grula it sits right after Orders.
    public class DiscountCodesPageTests
    {
        private static readonly AppSettings GrulaConfigured = new AppSettings { GrulaAccessToken = "test-token", GrulaEnvironmentId = Guid.NewGuid().ToString() };

        [Fact]
        public async Task Index_WhenGrulaIsNotConfigured_Returns404WithoutBuildingThePage()
        {
            var pageModelBuilder = Substitute.For<IAsyncComponentModelBuilder<ComponentModelBase, DiscountCodesPageViewModel>>();
            var controller = new DiscountCodesController(pageModelBuilder, Options.Create(new AppSettings()));

            var result = await controller.Index();

            Assert.IsType<NotFoundResult>(result);
            Assert.Empty(pageModelBuilder.ReceivedCalls());
        }

        [Fact]
        public async Task Index_WhenGrulaIsConfigured_ReturnsThePage()
        {
            var pageModelBuilder = Substitute.For<IAsyncComponentModelBuilder<ComponentModelBase, DiscountCodesPageViewModel>>();
            var viewModel = new DiscountCodesPageViewModel();
            pageModelBuilder.BuildModelAsync(Arg.Any<ComponentModelBase>()).Returns(viewModel);
            var controller = new DiscountCodesController(pageModelBuilder, Options.Create(GrulaConfigured))
            {
                ControllerContext = new ControllerContext { HttpContext = CreateHttpContext() },
                TempData = Substitute.For<ITempDataDictionary>()
            };

            var result = await controller.Index();

            Assert.Same(viewModel, Assert.IsType<ViewResult>(result).Model);
        }

        [Fact]
        public void Tiles_WhenGrulaIsConfigured_ListDiscountCodesRightAfterOrders()
        {
            var titles = BuildTileTitles(GrulaConfigured);

            Assert.Equal("Orders", titles[0]);
            Assert.Equal("Discount codes", titles[1]);
        }

        [Fact]
        public void Tiles_WhenGrulaIsNotConfigured_OmitDiscountCodes()
        {
            Assert.DoesNotContain("Discount codes", BuildTileTitles(new AppSettings()));
        }

        [Fact]
        public void Drawer_WhenGrulaIsConfigured_ListsDiscountCodesRightAfterOrders()
        {
            var titles = BuildDrawerOrdersGroupTitles(GrulaConfigured);

            Assert.Equal(new[] { "Orders", "Discount codes" }, titles);
        }

        [Fact]
        public void Drawer_WhenGrulaIsNotConfigured_OmitsDiscountCodes()
        {
            var titles = BuildDrawerOrdersGroupTitles(new AppSettings());

            Assert.Equal(new[] { "Orders" }, titles);
        }

        [Fact]
        public async Task Catalog_DeleteConfirmationNamesTheCode()
        {
            var catalogModelBuilder = Substitute.For<ICatalogModelBuilder>();
            catalogModelBuilder.BuildModel<CatalogViewModel<DiscountCode>, DiscountCode>().Returns(new CatalogViewModel<DiscountCode>());
            var builder = new DiscountCodesPageCatalogModelBuilder(
                catalogModelBuilder,
                GlobalLocalizer(),
                ClientLocalizer(),
                Substitute.For<IDiscountCodesRepository>(),
                Substitute.For<LinkGenerator>());

            var catalog = await builder.BuildModelAsync(new ComponentModelBase());

            Assert.Equal(new[] { "code" }, catalog.ConfirmationDialogDeleteNameProperty);
        }

        private static DefaultHttpContext CreateHttpContext()
        {
            var context = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity())
            };
            var authentication = Substitute.For<IAuthenticationService>();
            var properties = new AuthenticationProperties();
            properties.StoreTokens(new[] { new AuthenticationToken { Name = ApiExtensionsConstants.TokenName, Value = "token" } });
            authentication.AuthenticateAsync(Arg.Any<HttpContext>(), Arg.Any<string>())
                .Returns(Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(context.User, properties, "test"))));
            context.RequestServices = new ServiceCollection().AddSingleton(authentication).BuildServiceProvider();
            return context;
        }

        private static string[] BuildTileTitles(AppSettings settings)
        {
            var builder = new MenuTilesModelBuilder(
                GlobalLocalizer(),
                Substitute.For<IStringLocalizer<DashboardResources>>(),
                ClientLocalizer(),
                Substitute.For<IStringLocalizer<TeamMembersResources>>(),
                Options.Create(settings),
                Substitute.For<LinkGenerator>());

            return builder.BuildModel().Tiles.Select(x => x.Title).ToArray();
        }

        private static string[] BuildDrawerOrdersGroupTitles(AppSettings settings)
        {
            var builder = new DrawerMenuModelBuilder(
                GlobalLocalizer(),
                Substitute.For<IStringLocalizer<DashboardResources>>(),
                ClientLocalizer(),
                Substitute.For<IStringLocalizer<TeamMembersResources>>(),
                Options.Create(settings),
                Substitute.For<LinkGenerator>());

            return builder.BuildModel().First().Items.Select(x => x.Title).ToArray();
        }

        private static IStringLocalizer<GlobalResources> GlobalLocalizer()
        {
            var localizer = Substitute.For<IStringLocalizer<GlobalResources>>();
            localizer["Orders"].Returns(new LocalizedString("Orders", "Orders"));
            return localizer;
        }

        private static IStringLocalizer<ClientResources> ClientLocalizer()
        {
            var localizer = Substitute.For<IStringLocalizer<ClientResources>>();
            localizer["DiscountCodes"].Returns(new LocalizedString("DiscountCodes", "Discount codes"));
            return localizer;
        }
    }
}
