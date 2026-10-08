using Foundation.Localization;
using Foundation.Pricing.DiscountCodes;
using Giuru.UnitTests.Helpers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NSubstitute;
using Seller.Web.Areas.Clients.ApiControllers;
using Seller.Web.Areas.Clients.ApiRequestModels;
using Seller.Web.Areas.Clients.Repositories.DiscountCodes;
using Seller.Web.Shared.Configurations;
using Seller.Web.Shared.Filters;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Giuru.UnitTests.Services.DiscountCodes
{
    public class SellerDiscountCodesApiControllerTests
    {
        private static readonly Guid SavedId = Guid.NewGuid();

        private sealed class Fixture
        {
            public IDiscountCodesRepository Repository { get; } = Substitute.For<IDiscountCodesRepository>();
            public IGrulaDiscountCodeService Grula { get; } = Substitute.For<IGrulaDiscountCodeService>();
            public DiscountCodesApiController Controller { get; }

            public Fixture(GrulaDiscountCodeLookup lookup = null)
            {
                Repository.SaveAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<Guid?>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<bool>())
                    .Returns(Task.FromResult(SavedId));
                Grula.FindAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
                    .Returns(Task.FromResult(lookup ?? GrulaDiscountCodeLookup.Exists("SUMMER25")));

                Controller = new DiscountCodesApiController(TestLocalizer.Create<ClientResources>(), Repository, Grula)
                {
                    ControllerContext = new ControllerContext { HttpContext = CreateHttpContext() }
                };
            }

            public Task AssertNothingSavedAsync()
            {
                return Repository.DidNotReceiveWithAnyArgs().SaveAsync(default, default, default, default, default, default);
            }
        }

        private static int StatusOf(IActionResult result) => ((ObjectResult)result).StatusCode.Value;

        private static string MessageOf(IActionResult result)
        {
            var value = ((ObjectResult)result).Value;

            return (string)value.GetType().GetProperty("Message").GetValue(value);
        }

        [Fact]
        public async Task Index_ForANewCodeThatExistsInGrula_SavesGrulasSpelling()
        {
            var fixture = new Fixture(GrulaDiscountCodeLookup.Exists("SUMMER25"));

            var result = await fixture.Controller.Index(new DiscountCodeRequestModel { Code = "  summer25 ", Description = "Summer" });

            Assert.Equal((int)HttpStatusCode.OK, StatusOf(result));
            Assert.Equal("DiscountCodeSavedSuccessfully", MessageOf(result));
            await fixture.Grula.Received(1).FindAsync("summer25", Arg.Any<CancellationToken>());
            await fixture.Repository.Received(1).SaveAsync("token", Arg.Any<string>(), null, "SUMMER25", "Summer", false);
        }

        [Fact]
        public async Task Index_WhenTheCodeIsMissingInGrula_Returns422AndSavesNothing()
        {
            var fixture = new Fixture(GrulaDiscountCodeLookup.Of(GrulaDiscountCodeLookupStatus.Missing));

            var result = await fixture.Controller.Index(new DiscountCodeRequestModel { Code = "SUMMER25" });

            Assert.Equal((int)HttpStatusCode.UnprocessableEntity, StatusOf(result));
            Assert.Equal("DiscountCodeNotFoundInGrula", MessageOf(result));
            await fixture.AssertNothingSavedAsync();
        }

        [Fact]
        public async Task Index_WhenGrulaIsUnavailable_Returns503AndSavesNothing()
        {
            var fixture = new Fixture(GrulaDiscountCodeLookup.Of(GrulaDiscountCodeLookupStatus.Unavailable));

            var result = await fixture.Controller.Index(new DiscountCodeRequestModel { Code = "SUMMER25" });

            Assert.Equal((int)HttpStatusCode.ServiceUnavailable, StatusOf(result));
            Assert.Equal("DiscountCodeGrulaUnavailable", MessageOf(result));
            await fixture.AssertNothingSavedAsync();
        }

        [Fact]
        public async Task Index_WhenGrulaIsNotConfigured_Returns404AndSavesNothing()
        {
            var fixture = new Fixture(GrulaDiscountCodeLookup.Of(GrulaDiscountCodeLookupStatus.NotConfigured));

            var result = await fixture.Controller.Index(new DiscountCodeRequestModel { Code = "SUMMER25" });

            Assert.IsType<NotFoundResult>(result);
            await fixture.AssertNothingSavedAsync();
        }

        [Fact]
        public async Task Index_WhenGrulaReturnsAnOversizedCanonicalCode_IsRejectedAndSavesNothing()
        {
            var fixture = new Fixture(GrulaDiscountCodeLookup.Exists(new string('A', DiscountCodeLimits.MaxLength + 1)));

            var result = await fixture.Controller.Index(new DiscountCodeRequestModel { Code = "SUMMER25" });

            Assert.Equal((int)HttpStatusCode.UnprocessableEntity, StatusOf(result));
            await fixture.AssertNothingSavedAsync();
        }

        [Fact]
        public async Task Index_WhenGrulaReturnsAnExistingCodeWithoutAText_IsRejectedAndSavesNothing()
        {
            var fixture = new Fixture(GrulaDiscountCodeLookup.Exists("  "));

            var result = await fixture.Controller.Index(new DiscountCodeRequestModel { Code = "SUMMER25" });

            Assert.Equal((int)HttpStatusCode.UnprocessableEntity, StatusOf(result));
            await fixture.AssertNothingSavedAsync();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task Index_ForABlankNewCode_Returns422WithoutAskingGrula(string code)
        {
            var fixture = new Fixture();

            var result = await fixture.Controller.Index(new DiscountCodeRequestModel { Code = code });

            Assert.Equal((int)HttpStatusCode.UnprocessableEntity, StatusOf(result));
            Assert.Equal("DiscountCodeRequiredErrorMessage", MessageOf(result));
            await fixture.Grula.DidNotReceiveWithAnyArgs().FindAsync(default, default);
            await fixture.AssertNothingSavedAsync();
        }

        [Fact]
        public async Task Index_ForAnOversizedNewCode_Returns422WithoutAskingGrula()
        {
            var fixture = new Fixture();

            var result = await fixture.Controller.Index(new DiscountCodeRequestModel { Code = new string('A', DiscountCodeLimits.MaxLength + 1) });

            Assert.Equal((int)HttpStatusCode.UnprocessableEntity, StatusOf(result));
            await fixture.Grula.DidNotReceiveWithAnyArgs().FindAsync(default, default);
            await fixture.AssertNothingSavedAsync();
        }

        [Fact]
        public async Task Index_ForAnUpdate_DoesNotCallGrulaAndNeverSendsTheCodeText()
        {
            var fixture = new Fixture();
            var id = Guid.NewGuid();

            var result = await fixture.Controller.Index(new DiscountCodeRequestModel { Id = id, Code = "RENAMED", Description = "New description", IsDisabled = true });

            Assert.Equal((int)HttpStatusCode.OK, StatusOf(result));
            await fixture.Grula.DidNotReceiveWithAnyArgs().FindAsync(default, default);

            // The code text is immutable after creation, so an update carries none.
            await fixture.Repository.Received(1).SaveAsync(Arg.Is<string>(x => x == "token"), Arg.Any<string>(), Arg.Is<Guid?>(x => x == id), Arg.Is<string>(x => x == null), Arg.Is<string>(x => x == "New description"), Arg.Is<bool>(x => x));
        }

        [Fact]
        public async Task Delete_DeletesTheCodeThroughTheRepository()
        {
            var fixture = new Fixture();
            var id = Guid.NewGuid();

            var result = await fixture.Controller.Delete(id);

            Assert.Equal((int)HttpStatusCode.OK, StatusOf(result));
            Assert.Equal("DiscountCodeDeletedSuccessfully", MessageOf(result));
            await fixture.Repository.Received(1).DeleteAsync("token", Arg.Any<string>(), id);
        }

        [Fact]
        public void TheController_IsGatedByGrulaAndRestrictedToSellers()
        {
            var type = typeof(DiscountCodesApiController);

            Assert.NotEmpty(type.GetCustomAttributes(typeof(RequireGrulaAttribute), inherit: true));

            var authorize = Assert.Single(type.GetCustomAttributes(typeof(Microsoft.AspNetCore.Authorization.AuthorizeAttribute), inherit: true));
            Assert.Equal("SellerOnly", ((Microsoft.AspNetCore.Authorization.AuthorizeAttribute)authorize).Policy);
        }

        [Theory]
        [InlineData(typeof(Seller.Web.Areas.Clients.Controllers.DiscountCodesController))]
        [InlineData(typeof(Seller.Web.Areas.Clients.Controllers.DiscountCodeController))]
        public void ThePageControllers_AreGatedByGrulaAndRestrictedToSellers(Type controller)
        {
            Assert.NotEmpty(controller.GetCustomAttributes(typeof(RequireGrulaAttribute), inherit: true));

            var authorize = Assert.Single(controller.GetCustomAttributes(typeof(Microsoft.AspNetCore.Authorization.AuthorizeAttribute), inherit: true));
            Assert.Equal("SellerOnly", ((Microsoft.AspNetCore.Authorization.AuthorizeAttribute)authorize).Policy);
        }

        private static DefaultHttpContext CreateHttpContext()
        {
            var context = new DefaultHttpContext();
            var authentication = Substitute.For<IAuthenticationService>();
            var properties = new AuthenticationProperties();
            properties.StoreTokens(new[] { new AuthenticationToken { Name = "access_token", Value = "token" } });
            authentication.AuthenticateAsync(Arg.Any<HttpContext>(), Arg.Any<string>())
                .Returns(Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(context.User, properties, "test"))));
            context.RequestServices = new ServiceCollection().AddSingleton(authentication).BuildServiceProvider();

            return context;
        }
    }

    public class RequireGrulaAttributeTests
    {
        private static ResourceExecutingContext CreateContext(bool grulaConfigured)
        {
            var settings = Options.Create(new AppSettings
            {
                GrulaAccessToken = grulaConfigured ? "token" : null,
                GrulaEnvironmentId = grulaConfigured ? Guid.NewGuid().ToString() : null
            });
            var httpContext = new DefaultHttpContext
            {
                RequestServices = new ServiceCollection().AddSingleton(settings).BuildServiceProvider()
            };
            var actionContext = new ActionContext(httpContext, new RouteData(), new ActionDescriptor());

            return new ResourceExecutingContext(actionContext, new List<IFilterMetadata>(), new List<IValueProviderFactory>());
        }

        [Fact]
        public void OnResourceExecuting_WhenGrulaIsNotConfigured_ShortCircuitsWith404()
        {
            var context = CreateContext(grulaConfigured: false);

            new RequireGrulaAttribute().OnResourceExecuting(context);

            Assert.IsType<NotFoundResult>(context.Result);
        }

        [Fact]
        public void OnResourceExecuting_WhenGrulaIsConfigured_LetsTheRequestThrough()
        {
            var context = CreateContext(grulaConfigured: true);

            new RequireGrulaAttribute().OnResourceExecuting(context);

            Assert.Null(context.Result);
        }
    }
}
