using Buyer.Web.Shared.Configurations;
using Buyer.Web.Shared.DomainModels.Clients;
using Buyer.Web.Shared.DomainModels.Global;
using Buyer.Web.Shared.Extensions;
using Buyer.Web.Shared.Middlewares;
using Buyer.Web.Shared.Repositories.Clients;
using Buyer.Web.Shared.Repositories.Global;
using Foundation.Extensions.Services.Cache;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Giuru.UnitTests.Extensions
{
    public class ClaimsEnrichmentMiddlewareTests
    {
        private const string Email = "buyer@test.com";

        private readonly IClientsRepository _clientsRepository = Substitute.For<IClientsRepository>();
        private readonly IDistributedCache _cache = new MemoryDistributedCache(Options.Create(new MemoryDistributedCacheOptions()));

        private ClaimsEnrichmentMiddleware CreateMiddleware()
        {
            var cacheService = Substitute.For<ICacheService>();
            cacheService.GetOrSetAsync(Arg.Any<string>(), Arg.Any<Func<Task<IEnumerable<Country>>>>(), Arg.Any<TimeSpan?>())
                .Returns(Task.FromResult<IEnumerable<Country>>(null));

            var clientFieldValuesRepository = Substitute.For<IClientFieldValuesRepository>();
            clientFieldValuesRepository.GetAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<Guid?>())
                .Returns(Task.FromResult(Enumerable.Empty<ClientFieldValue>()));

            return new ClaimsEnrichmentMiddleware(
                _clientsRepository,
                Substitute.For<IClientAddressesRepository>(),
                Substitute.For<IGlobalRepository>(),
                clientFieldValuesRepository,
                Options.Create(new AppSettings { DefaultCulture = "en" }),
                _cache,
                cacheService);
        }

        private static DefaultHttpContext CreateHttpContext()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Email, Email) }, "test"));
            var context = new DefaultHttpContext { User = user };

            var authentication = Substitute.For<IAuthenticationService>();
            var properties = new AuthenticationProperties();
            properties.StoreTokens(new[] { new AuthenticationToken { Name = "access_token", Value = "token" } });
            authentication.AuthenticateAsync(Arg.Any<HttpContext>(), Arg.Any<string>())
                .Returns(Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(user, properties, "test"))));
            context.RequestServices = new ServiceCollection().AddSingleton(authentication).BuildServiceProvider();

            return context;
        }

        [Fact]
        public async Task InvokeAsync_OnTheRequestThatFillsTheCache_AlreadyCarriesTheClientId()
        {
            // The first request of a buyer - and the first after every cache expiry - must not run without a client:
            // prices would not be the client's and, with enforcement on, a valid discount code would be refused or dropped.
            var clientId = Guid.NewGuid();
            _clientsRepository.GetClientByEmailAsync(Arg.Any<string>(), Arg.Any<string>(), Email)
                .Returns(Task.FromResult(new Client { Id = clientId, Email = Email }));

            var context = CreateHttpContext();
            Guid? clientIdSeenByTheRequest = null;

            await CreateMiddleware().InvokeAsync(context, ctx =>
            {
                clientIdSeenByTheRequest = ctx.User.GetClientId();

                return Task.CompletedTask;
            });

            Assert.Equal(clientId, clientIdSeenByTheRequest);
        }

        [Fact]
        public async Task InvokeAsync_OnALaterRequest_ReadsTheSameClientIdFromTheCache()
        {
            var clientId = Guid.NewGuid();
            _clientsRepository.GetClientByEmailAsync(Arg.Any<string>(), Arg.Any<string>(), Email)
                .Returns(Task.FromResult(new Client { Id = clientId, Email = Email }));

            var middleware = CreateMiddleware();
            await middleware.InvokeAsync(CreateHttpContext(), _ => Task.CompletedTask);

            var laterContext = CreateHttpContext();
            await middleware.InvokeAsync(laterContext, _ => Task.CompletedTask);

            Assert.Equal(clientId, laterContext.User.GetClientId());
            Assert.Single(laterContext.User.FindAll(Buyer.Web.Shared.Definitions.Middlewares.ClaimsEnrichmentConstants.ClientIdClaimType));
            await _clientsRepository.Received(1).GetClientByEmailAsync(Arg.Any<string>(), Arg.Any<string>(), Email);
        }

        [Fact]
        public async Task InvokeAsync_ForAUserWithoutAClient_AddsNoClientId()
        {
            _clientsRepository.GetClientByEmailAsync(Arg.Any<string>(), Arg.Any<string>(), Email)
                .Returns(Task.FromResult<Client>(null));

            var context = CreateHttpContext();

            await CreateMiddleware().InvokeAsync(context, _ => Task.CompletedTask);

            Assert.Null(context.User.GetClientId());
        }
    }
}
