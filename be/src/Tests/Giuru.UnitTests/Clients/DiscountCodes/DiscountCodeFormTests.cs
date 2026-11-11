using Foundation.ApiExtensions.Definitions;
using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
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
using Seller.Web.Shared.Repositories.Clients;
using Seller.Web.Shared.ViewModels;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Giuru.UnitTests.Clients.DiscountCodes
{
    // The Seller portal's Discount Code form is behind the Grula gate, lists every Client for assignment
    // (disabled ones too, they keep their assignments) and loads an existing code with its Assigned Clients.
    public class DiscountCodeFormTests
    {
        private static readonly AppSettings GrulaConfigured = new AppSettings { GrulaAccessToken = "test-token", GrulaEnvironmentId = Guid.NewGuid().ToString() };

        [Fact]
        public async Task Edit_WhenGrulaIsNotConfigured_Returns404WithoutBuildingThePage()
        {
            var pageModelBuilder = Substitute.For<IAsyncComponentModelBuilder<ComponentModelBase, DiscountCodePageViewModel>>();
            var controller = new DiscountCodeController(pageModelBuilder, Options.Create(new AppSettings()));

            var result = await controller.Edit(Guid.NewGuid());

            Assert.IsType<NotFoundResult>(result);
            Assert.Empty(pageModelBuilder.ReceivedCalls());
        }

        [Fact]
        public async Task Edit_WhenGrulaIsConfigured_BuildsThePageForTheRequestedCode()
        {
            var id = Guid.NewGuid();
            var pageModelBuilder = Substitute.For<IAsyncComponentModelBuilder<ComponentModelBase, DiscountCodePageViewModel>>();
            var viewModel = new DiscountCodePageViewModel();
            pageModelBuilder.BuildModelAsync(Arg.Is<ComponentModelBase>(x => x.Id == id)).Returns(viewModel);
            var controller = new DiscountCodeController(pageModelBuilder, Options.Create(GrulaConfigured))
            {
                ControllerContext = new ControllerContext { HttpContext = CreateHttpContext() },
                TempData = Substitute.For<ITempDataDictionary>()
            };

            var result = await controller.Edit(id);

            Assert.Same(viewModel, Assert.IsType<ViewResult>(result).Model);
        }

        [Fact]
        public async Task Form_ForANewCode_IsEmptyAndOffersEveryClientIncludingDisabledOnes()
        {
            var clientsRepository = Substitute.For<IClientsRepository>();
            var active = new Seller.Web.Areas.Clients.DomainModels.Client { Id = Guid.NewGuid(), Name = "Active client" };
            var disabled = new Seller.Web.Areas.Clients.DomainModels.Client { Id = Guid.NewGuid(), Name = "Disabled client", IsDisabled = true };
            clientsRepository.GetAllClientsAsync("token", "en").Returns(new[] { active, disabled });
            var builder = CreateFormBuilder(Substitute.For<IDiscountCodesRepository>(), clientsRepository);

            var form = await builder.BuildModelAsync(new ComponentModelBase { Token = "token", Language = "en" });

            Assert.Null(form.Id);
            Assert.Null(form.Code);
            Assert.False(form.IsDisabled);
            Assert.Empty(form.ClientIds);
            Assert.Equal(new[] { active.Id, disabled.Id }, form.Clients.Select(x => x.Id));
        }

        [Fact]
        public async Task Form_ForAnExistingCode_LoadsItsFieldsAndAssignedClients()
        {
            var id = Guid.NewGuid();
            var clientId = Guid.NewGuid();
            var discountCodesRepository = Substitute.For<IDiscountCodesRepository>();
            discountCodesRepository.GetAsync("token", "en", (Guid?)id).Returns(new DiscountCode
            {
                Id = id,
                Code = "SUMMER10",
                Description = "Summer promotion",
                IsDisabled = true,
                ClientIds = new[] { clientId }
            });
            var builder = CreateFormBuilder(discountCodesRepository, Substitute.For<IClientsRepository>());

            var form = await builder.BuildModelAsync(new ComponentModelBase { Id = id, Token = "token", Language = "en" });

            Assert.Equal(id, form.Id);
            Assert.Equal("SUMMER10", form.Code);
            Assert.Equal("Summer promotion", form.Description);
            Assert.True(form.IsDisabled);
            Assert.Equal(new[] { clientId }, form.ClientIds);
        }

        [Fact]
        public async Task Catalog_LinksToTheFormForCreatingAndEditing()
        {
            var catalogModelBuilder = Substitute.For<ICatalogModelBuilder>();
            catalogModelBuilder.BuildModel<CatalogViewModel<DiscountCode>, DiscountCode>().Returns(new CatalogViewModel<DiscountCode>());
            var linkGenerator = Substitute.For<LinkGenerator>();
            var builder = new DiscountCodesPageCatalogModelBuilder(
                catalogModelBuilder,
                Substitute.For<IStringLocalizer<GlobalResources>>(),
                Substitute.For<IStringLocalizer<ClientResources>>(),
                Substitute.For<IDiscountCodesRepository>(),
                linkGenerator);

            var catalog = await builder.BuildModelAsync(new ComponentModelBase());

            Assert.Contains(catalog.Table.Actions, x => x.IsEdit);
            Assert.Contains(catalog.Table.Actions, x => x.IsDelete);
            Assert.Equal(catalog.EditUrl, catalog.NewUrl);
        }

        private static DiscountCodeFormModelBuilder CreateFormBuilder(IDiscountCodesRepository discountCodesRepository, IClientsRepository clientsRepository)
        {
            return new DiscountCodeFormModelBuilder(
                Substitute.For<IStringLocalizer<GlobalResources>>(),
                Substitute.For<IStringLocalizer<ClientResources>>(),
                discountCodesRepository,
                clientsRepository,
                Substitute.For<LinkGenerator>());
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
    }
}
