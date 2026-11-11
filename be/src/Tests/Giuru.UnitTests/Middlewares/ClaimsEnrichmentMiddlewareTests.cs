using Buyer.Web.Shared.Configurations;
using Buyer.Web.Shared.Definitions.Middlewares;
using Buyer.Web.Shared.DomainModels.Clients;
using Buyer.Web.Shared.DomainModels.Global;
using Buyer.Web.Shared.Middlewares;
using Buyer.Web.Shared.Repositories.Clients;
using Buyer.Web.Shared.Repositories.Global;
using Foundation.Account.Definitions;
using Foundation.Extensions.Exceptions;
using Foundation.Extensions.Services.Cache;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BuyerClient = Buyer.Web.Shared.DomainModels.Clients.Client;

namespace Giuru.UnitTests.Middlewares
{
    // A Team Member shares the organisation of the Client they belong to but has an email of their own, so
    // the Client has to be resolved from the organisation. These tests pin that the Client's id and
    // Client-derived claims reach the request for such a user, on a claims-cache miss as well as a hit.
    public class ClaimsEnrichmentMiddlewareTests
    {
        private const string TeamMemberEmail = "team.member@example.com";
        private const string OtherTeamMemberEmail = "other.team.member@example.com";
        private static readonly Guid OrganisationId = Guid.NewGuid();

        [Fact]
        public async Task InvokeAsync_OnClaimsCacheMiss_AddsClaimsOfTheClientResolvedByOrganisation()
        {
            var client = new BuyerClient { Id = Guid.NewGuid(), Name = "Acme" };
            var clientsRepository = CreateClientsRepository(client);
            var middleware = CreateMiddleware(clientsRepository);

            var context = await InvokeAsync(middleware);

            Assert.Equal(client.Id.ToString(), ClaimValue(context.User, ClaimsEnrichmentConstants.ClientIdClaimType));
            Assert.Equal("Acme", ClaimValue(context.User, ClaimsEnrichmentConstants.ClientNameClaimType));
            await clientsRepository.Received(1).GetClientAsync(Arg.Any<string>(), Arg.Any<string>());
        }

        [Fact]
        public async Task InvokeAsync_OnClaimsCacheHit_AddsClientIdFromTheCache()
        {
            var client = new BuyerClient { Id = Guid.NewGuid(), Name = "Acme" };
            var clientsRepository = CreateClientsRepository(client);
            var middleware = CreateMiddleware(clientsRepository);

            await InvokeAsync(middleware);

            // The claims are cached per organisation, so another Team Member of the Client hits the same entry
            var context = await InvokeAsync(middleware, email: OtherTeamMemberEmail);

            Assert.Equal(client.Id.ToString(), ClaimValue(context.User, ClaimsEnrichmentConstants.ClientIdClaimType));
            Assert.Equal("Acme", ClaimValue(context.User, ClaimsEnrichmentConstants.ClientNameClaimType));
            await clientsRepository.Received(1).GetClientAsync(Arg.Any<string>(), Arg.Any<string>());
        }

        [Fact]
        public async Task InvokeAsync_OrganisationHasNoClient_AddsNoClaimsAndContinues()
        {
            var middleware = CreateMiddleware(CreateClientsRepository(null));
            var nextCalled = false;

            var context = await InvokeAsync(middleware, () => nextCalled = true);

            Assert.Null(ClaimValue(context.User, ClaimsEnrichmentConstants.ClientIdClaimType));
            Assert.True(nextCalled);
        }

        [Fact]
        public async Task InvokeAsync_ClientLookupFails_LogsAWarningAddsNoClaimsAndContinues()
        {
            var clientsRepository = Substitute.For<IClientsRepository>();
            clientsRepository.GetClientAsync(Arg.Any<string>(), Arg.Any<string>())
                .Returns<Task<BuyerClient>>(_ => throw new CustomException("Unprocessable", 422));
            var logger = Substitute.For<ILogger<ClaimsEnrichmentMiddleware>>();
            var middleware = CreateMiddleware(clientsRepository, logger);
            var nextCalled = false;

            var context = await InvokeAsync(middleware, () => nextCalled = true);

            Assert.Null(ClaimValue(context.User, ClaimsEnrichmentConstants.ClientIdClaimType));
            Assert.True(nextCalled);
            Assert.Contains(logger.ReceivedCalls(), call =>
                call.GetMethodInfo().Name == nameof(ILogger.Log) && (LogLevel)call.GetArguments()[0] == LogLevel.Warning);
        }

        private static string ClaimValue(ClaimsPrincipal user, string type) => user.Claims.FirstOrDefault(x => x.Type == type)?.Value;

        private static IClientsRepository CreateClientsRepository(BuyerClient client)
        {
            var clientsRepository = Substitute.For<IClientsRepository>();
            clientsRepository.GetClientAsync(Arg.Any<string>(), Arg.Any<string>()).Returns(Task.FromResult(client));

            return clientsRepository;
        }

        private static ClaimsEnrichmentMiddleware CreateMiddleware(
            IClientsRepository clientsRepository,
            ILogger<ClaimsEnrichmentMiddleware> logger = null)
        {
            var fieldValuesRepository = Substitute.For<IClientFieldValuesRepository>();
            fieldValuesRepository.GetAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<Guid?>())
                .Returns(Task.FromResult<IEnumerable<ClientFieldValue>>([]));

            var cacheService = Substitute.For<ICacheService>();
            cacheService.GetOrSetAsync(Arg.Any<string>(), Arg.Any<Func<Task<IEnumerable<Country>>>>(), Arg.Any<TimeSpan?>())
                .Returns(Task.FromResult<IEnumerable<Country>>([]));

            return new ClaimsEnrichmentMiddleware(
                clientsRepository,
                Substitute.For<IClientAddressesRepository>(),
                Substitute.For<IGlobalRepository>(),
                fieldValuesRepository,
                Options.Create(new AppSettings { DefaultCulture = "en-US" }),
                new MemoryDistributedCache(Options.Create(new MemoryDistributedCacheOptions())),
                cacheService,
                logger ?? Substitute.For<ILogger<ClaimsEnrichmentMiddleware>>());
        }

        private static HttpContext CreateHttpContext(string email)
        {
            var identity = new ClaimsIdentity(
                [
                    new Claim(ClaimTypes.Email, email),
                    new Claim(AccountConstants.Claims.OrganisationIdClaim, OrganisationId.ToString())
                ],
                "test");
            var authenticationService = Substitute.For<IAuthenticationService>();
            authenticationService.AuthenticateAsync(Arg.Any<HttpContext>(), Arg.Any<string>())
                .Returns(Task.FromResult(AuthenticateResult.NoResult()));
            var services = new ServiceCollection().AddSingleton(authenticationService).BuildServiceProvider();

            return new DefaultHttpContext { User = new ClaimsPrincipal(identity), RequestServices = services };
        }

        private static async Task<HttpContext> InvokeAsync(ClaimsEnrichmentMiddleware middleware, Action onNext = null, string email = TeamMemberEmail)
        {
            var context = CreateHttpContext(email);

            await middleware.InvokeAsync(context, _ =>
            {
                onNext?.Invoke();

                return Task.CompletedTask;
            });

            return context;
        }
    }
}
