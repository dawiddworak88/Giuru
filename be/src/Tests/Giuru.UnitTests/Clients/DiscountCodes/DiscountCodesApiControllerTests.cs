using Foundation.Localization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using NSubstitute;
using Seller.Web.Areas.Clients.ApiControllers;
using Seller.Web.Areas.Clients.ApiRequestModels;
using Seller.Web.Areas.Clients.DomainModels;
using Seller.Web.Areas.Clients.Repositories.DiscountCodes;
using Seller.Web.Shared.Configurations;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Giuru.UnitTests.Clients.DiscountCodes
{
    // The Seller portal's Discount Code API is behind the Grula gate: without Grula it answers 404
    // and never reaches Client.Api. The happy paths are covered by the integration tests.
    public class DiscountCodesApiControllerTests
    {
        [Fact]
        public async Task EveryAction_WhenGrulaIsNotConfigured_Returns404WithoutCallingClientApi()
        {
            var repository = Substitute.For<IDiscountCodesRepository>();
            var controller = CreateController(repository, new AppSettings());

            var results = new[]
            {
                await controller.Get("code", 1, 25),
                await controller.GetById(Guid.NewGuid()),
                await controller.Index(new DiscountCodeRequestModel { Code = "SPRING" }),
                await controller.Delete(Guid.NewGuid())
            };

            Assert.All(results, result => Assert.Equal(StatusCodes.Status404NotFound, Assert.IsType<StatusCodeResult>(result).StatusCode));
            Assert.Empty(repository.ReceivedCalls());
        }

        [Fact]
        public async Task GetById_WhenTheCodeDoesNotExist_ReturnsNoContent()
        {
            var repository = Substitute.For<IDiscountCodesRepository>();
            repository.GetAsync("token", Arg.Any<string>(), Arg.Any<Guid?>()).Returns(Task.FromResult<DiscountCode>(null));
            var controller = CreateController(repository, new AppSettings { GrulaAccessToken = "test-token", GrulaEnvironmentId = Guid.NewGuid().ToString() });

            var result = await controller.GetById(Guid.NewGuid());

            Assert.Equal(StatusCodes.Status204NoContent, Assert.IsType<StatusCodeResult>(result).StatusCode);
        }

        private static DiscountCodesApiController CreateController(IDiscountCodesRepository repository, AppSettings settings)
        {
            return new DiscountCodesApiController(
                Substitute.For<IStringLocalizer<ClientResources>>(),
                repository,
                Options.Create(settings))
            {
                ControllerContext = new ControllerContext { HttpContext = CreateHttpContext() }
            };
        }

        private static DefaultHttpContext CreateHttpContext()
        {
            var context = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity())
            };
            var authentication = Substitute.For<IAuthenticationService>();
            var properties = new AuthenticationProperties();
            properties.StoreTokens(new[] { new AuthenticationToken { Name = "access_token", Value = "token" } });
            authentication.AuthenticateAsync(Arg.Any<HttpContext>(), Arg.Any<string>())
                .Returns(Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(context.User, properties, "test"))));
            context.RequestServices = new ServiceCollection().AddSingleton(authentication).BuildServiceProvider();
            return context;
        }
    }
}
