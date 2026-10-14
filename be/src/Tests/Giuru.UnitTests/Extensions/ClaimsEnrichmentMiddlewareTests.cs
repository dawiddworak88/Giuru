using Buyer.Web.Shared.Configurations;
using Buyer.Web.Shared.DomainModels.Clients;
using Buyer.Web.Shared.DomainModels.Global;
using Buyer.Web.Shared.Extensions;
using Buyer.Web.Shared.Middlewares;
using Buyer.Web.Shared.Repositories.Clients;
using Buyer.Web.Shared.Repositories.Global;
using Foundation.Account.Definitions;
using Foundation.Extensions.Definitions;
using Foundation.Extensions.Exceptions;
using Foundation.Extensions.Services.Cache;
using Foundation.Extensions.Services.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BuyerClaimsEnrichmentConstants = Buyer.Web.Shared.Definitions.Middlewares.ClaimsEnrichmentConstants;
using BuyerClient = Buyer.Web.Shared.DomainModels.Clients.Client;

namespace Giuru.UnitTests.Extensions
{
    public class ClaimsEnrichmentMiddlewareTests
    {
        private const string Email = "buyer@test.com";
        private const string TeamMemberEmail = "team.member@test.com";

        private static readonly Guid OrganisationId = Guid.NewGuid();

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
                cacheService,
                Substitute.For<ILogger<ClaimsEnrichmentMiddleware>>());
        }

        private static DefaultHttpContext CreateHttpContext(string email = Email, Guid? organisationId = null, bool withOrganisation = true)
        {
            var claims = new List<Claim> { new(ClaimTypes.Email, email) };

            if (withOrganisation)
            {
                claims.Add(new Claim(AccountConstants.Claims.OrganisationIdClaim, (organisationId ?? OrganisationId).ToString()));
            }

            var user = new ClaimsPrincipal(new ClaimsIdentity(claims, "test"));
            var context = new DefaultHttpContext { User = user };

            var authentication = Substitute.For<IAuthenticationService>();
            var properties = new AuthenticationProperties();
            properties.StoreTokens(new[] { new AuthenticationToken { Name = "access_token", Value = "token" } });
            authentication.AuthenticateAsync(Arg.Any<HttpContext>(), Arg.Any<string>())
                .Returns(Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(user, properties, "test"))));
            context.RequestServices = new ServiceCollection().AddSingleton(authentication).BuildServiceProvider();

            return context;
        }

        private async Task<string> GetCachedClaimsAsync(string email, Guid organisationId)
        {
            var version = await _cache.GetStringAsync(ClaimsEnrichmentConstants.GetClientClaimsVersionCacheKey(organisationId));

            return version is null
                ? null
                : await _cache.GetStringAsync(ClaimsEnrichmentConstants.GetClientClaimsCacheKey(email, organisationId, version));
        }

        private static async Task<Guid?> RunAsync(ClaimsEnrichmentMiddleware middleware, DefaultHttpContext context)
        {
            Guid? clientIdSeenByTheRequest = null;

            await middleware.InvokeAsync(context, ctx =>
            {
                clientIdSeenByTheRequest = ctx.User.GetClientId();

                return Task.CompletedTask;
            });

            return clientIdSeenByTheRequest;
        }

        [Fact]
        public async Task InvokeAsync_ForAPrimaryContactOnTheRequestThatFillsTheCache_AlreadyCarriesTheClientId()
        {
            // The first request of a buyer - and the first after every cache expiry - must not run without a client:
            // prices would not be the client's and a valid discount code would be refused.
            var clientId = Guid.NewGuid();
            _clientsRepository.GetClientByEmailAsync(Arg.Any<string>(), Arg.Any<string>(), Email)
                .Returns(Task.FromResult(new BuyerClient { Id = clientId, Email = Email }));

            var seen = await RunAsync(CreateMiddleware(), CreateHttpContext());

            Assert.Equal(clientId, seen);
        }

        [Fact]
        public async Task InvokeAsync_ForAPrimaryContactOnALaterRequest_ReadsTheSameSingleClientIdFromTheCache()
        {
            var clientId = Guid.NewGuid();
            _clientsRepository.GetClientByEmailAsync(Arg.Any<string>(), Arg.Any<string>(), Email)
                .Returns(Task.FromResult(new BuyerClient { Id = clientId, Email = Email }));

            var middleware = CreateMiddleware();
            await RunAsync(middleware, CreateHttpContext());

            var laterContext = CreateHttpContext();
            var seen = await RunAsync(middleware, laterContext);

            Assert.Equal(clientId, seen);
            Assert.Single(laterContext.User.FindAll(BuyerClaimsEnrichmentConstants.ClientIdClaimType));
            await _clientsRepository.Received(1).GetClientByEmailAsync(Arg.Any<string>(), Arg.Any<string>(), Email);
            await _clientsRepository.DidNotReceiveWithAnyArgs().GetClientAsync(default, default);
        }

        [Fact]
        public async Task InvokeAsync_ForATeamMemberOnTheRequestThatFillsTheCache_UsesTheClientOfTheirOrganisation()
        {
            var clientId = Guid.NewGuid();
            _clientsRepository.GetClientByEmailAsync(Arg.Any<string>(), Arg.Any<string>(), TeamMemberEmail)
                .Returns(Task.FromResult<BuyerClient>(null));
            _clientsRepository.GetClientAsync(Arg.Any<string>(), Arg.Any<string>())
                .Returns(Task.FromResult(new BuyerClient { Id = clientId, Email = Email }));

            var seen = await RunAsync(CreateMiddleware(), CreateHttpContext(TeamMemberEmail));

            Assert.Equal(clientId, seen);
        }

        [Fact]
        public async Task InvokeAsync_ForATeamMemberOnALaterRequest_ReadsTheClientIdFromTheCacheWithoutLookingItUpAgain()
        {
            var clientId = Guid.NewGuid();
            _clientsRepository.GetClientByEmailAsync(Arg.Any<string>(), Arg.Any<string>(), TeamMemberEmail)
                .Returns(Task.FromResult<BuyerClient>(null));
            _clientsRepository.GetClientAsync(Arg.Any<string>(), Arg.Any<string>())
                .Returns(Task.FromResult(new BuyerClient { Id = clientId, Email = Email }));

            var middleware = CreateMiddleware();
            await RunAsync(middleware, CreateHttpContext(TeamMemberEmail));

            var laterContext = CreateHttpContext(TeamMemberEmail);
            var seen = await RunAsync(middleware, laterContext);

            Assert.Equal(clientId, seen);
            Assert.Single(laterContext.User.FindAll(BuyerClaimsEnrichmentConstants.ClientIdClaimType));
            await _clientsRepository.Received(1).GetClientByEmailAsync(Arg.Any<string>(), Arg.Any<string>(), TeamMemberEmail);
            await _clientsRepository.Received(1).GetClientAsync(Arg.Any<string>(), Arg.Any<string>());
        }

        [Fact]
        public async Task InvokeAsync_WhenTheEmailMatchesAClient_DoesNotFallBackToTheOrganisation()
        {
            _clientsRepository.GetClientByEmailAsync(Arg.Any<string>(), Arg.Any<string>(), Email)
                .Returns(Task.FromResult(new BuyerClient { Id = Guid.NewGuid(), Email = Email }));

            await RunAsync(CreateMiddleware(), CreateHttpContext());

            await _clientsRepository.DidNotReceiveWithAnyArgs().GetClientAsync(default, default);
        }

        [Fact]
        public async Task InvokeAsync_ForAUserWithoutAClientByEmailOrOrganisation_AddsNoClientIdAndCachesNothing()
        {
            _clientsRepository.GetClientByEmailAsync(Arg.Any<string>(), Arg.Any<string>(), Email).Returns(Task.FromResult<BuyerClient>(null));
            _clientsRepository.GetClientAsync(Arg.Any<string>(), Arg.Any<string>()).Returns(Task.FromResult<BuyerClient>(null));

            var middleware = CreateMiddleware();
            var seen = await RunAsync(middleware, CreateHttpContext());
            await RunAsync(middleware, CreateHttpContext());

            Assert.Null(seen);
            await _clientsRepository.Received(2).GetClientAsync(Arg.Any<string>(), Arg.Any<string>());
            Assert.Null(await GetCachedClaimsAsync(Email, OrganisationId));
        }

        [Fact]
        public async Task InvokeAsync_WhenTheOrganisationFallbackFails_TreatsTheUserAsHavingNoClientAndCachesNothing()
        {
            _clientsRepository.GetClientByEmailAsync(Arg.Any<string>(), Arg.Any<string>(), TeamMemberEmail).Returns(Task.FromResult<BuyerClient>(null));
            _clientsRepository.GetClientAsync(Arg.Any<string>(), Arg.Any<string>()).ThrowsAsync(new CustomException("boom", 500));

            var middleware = CreateMiddleware();
            var seen = await RunAsync(middleware, CreateHttpContext(TeamMemberEmail));

            Assert.Null(seen);
            Assert.Null(await GetCachedClaimsAsync(TeamMemberEmail, OrganisationId));

            // The next request retries instead of reading a remembered failure.
            _clientsRepository.GetClientAsync(Arg.Any<string>(), Arg.Any<string>()).Returns(Task.FromResult(new BuyerClient { Id = Guid.NewGuid() }));

            Assert.NotNull(await RunAsync(middleware, CreateHttpContext(TeamMemberEmail)));
        }

        [Fact]
        public async Task InvokeAsync_WhenTheOrganisationFallbackThrowsAnythingElse_TreatsTheUserAsHavingNoClient()
        {
            _clientsRepository.GetClientByEmailAsync(Arg.Any<string>(), Arg.Any<string>(), TeamMemberEmail).Returns(Task.FromResult<BuyerClient>(null));
            _clientsRepository.GetClientAsync(Arg.Any<string>(), Arg.Any<string>()).ThrowsAsync(new System.Net.Http.HttpRequestException("unreachable"));

            var seen = await RunAsync(CreateMiddleware(), CreateHttpContext(TeamMemberEmail));

            Assert.Null(seen);
        }

        [Fact]
        public async Task InvokeAsync_WithoutAnOrganisationClaim_DoesNotTryTheOrganisationFallback()
        {
            _clientsRepository.GetClientByEmailAsync(Arg.Any<string>(), Arg.Any<string>(), TeamMemberEmail).Returns(Task.FromResult<BuyerClient>(null));

            var seen = await RunAsync(CreateMiddleware(), CreateHttpContext(TeamMemberEmail, withOrganisation: false));

            Assert.Null(seen);
            await _clientsRepository.DidNotReceiveWithAnyArgs().GetClientAsync(default, default);
        }

        [Fact]
        public async Task InvokeAsync_KeepsTheClaimsOfTheSameEmailInDifferentOrganisationsApart()
        {
            var firstClientId = Guid.NewGuid();
            var secondClientId = Guid.NewGuid();
            var otherOrganisationId = Guid.NewGuid();
            _clientsRepository.GetClientByEmailAsync(Arg.Any<string>(), Arg.Any<string>(), TeamMemberEmail).Returns(Task.FromResult<BuyerClient>(null));
            _clientsRepository.GetClientAsync(Arg.Any<string>(), Arg.Any<string>())
                .Returns(Task.FromResult(new BuyerClient { Id = firstClientId }), Task.FromResult(new BuyerClient { Id = secondClientId }));

            var middleware = CreateMiddleware();
            var first = await RunAsync(middleware, CreateHttpContext(TeamMemberEmail, OrganisationId));
            var second = await RunAsync(middleware, CreateHttpContext(TeamMemberEmail, otherOrganisationId));
            var firstAgain = await RunAsync(middleware, CreateHttpContext(TeamMemberEmail, OrganisationId));

            Assert.Equal(firstClientId, first);
            Assert.Equal(secondClientId, second);
            Assert.Equal(firstClientId, firstAgain);
        }

        [Fact]
        public async Task InvokeAsync_NeverReadsAnOldEmailOnlyCacheEntry()
        {
            var staleClientId = Guid.NewGuid();
            var clientId = Guid.NewGuid();
            await _cache.SetStringAsync(
                $"{ClaimsEnrichmentConstants.CacheKey}-{Email}",
                $"[{{\"Key\":\"{BuyerClaimsEnrichmentConstants.ClientIdClaimType}\",\"Value\":\"{staleClientId}\"}}]");
            _clientsRepository.GetClientByEmailAsync(Arg.Any<string>(), Arg.Any<string>(), Email)
                .Returns(Task.FromResult(new BuyerClient { Id = clientId, Email = Email }));

            var seen = await RunAsync(CreateMiddleware(), CreateHttpContext());

            Assert.Equal(clientId, seen);
        }

        [Fact]
        public async Task InvalidateAsync_MakesThePrimaryContactLookTheClientUpAgain()
        {
            var firstClientId = Guid.NewGuid();
            var secondClientId = Guid.NewGuid();
            _clientsRepository.GetClientByEmailAsync(Arg.Any<string>(), Arg.Any<string>(), Email)
                .Returns(Task.FromResult(new BuyerClient { Id = firstClientId, Email = Email }), Task.FromResult(new BuyerClient { Id = secondClientId, Email = Email }));

            var middleware = CreateMiddleware();
            await RunAsync(middleware, CreateHttpContext());

            await new ClaimsCacheInvalidatorService(_cache).InvalidateAsync(OrganisationId);

            Assert.Equal(secondClientId, await RunAsync(middleware, CreateHttpContext()));
        }

        [Fact]
        public async Task InvalidateAsync_AlsoMakesEveryTeamMemberOfTheOrganisationLookTheClientUpAgain()
        {
            // An edit of the client - its currency, say - must not stay hidden from the team for the life of their entries.
            var firstClientId = Guid.NewGuid();
            var secondClientId = Guid.NewGuid();
            const string otherTeamMemberEmail = "other.member@test.com";
            _clientsRepository.GetClientByEmailAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>())
                .Returns(Task.FromResult<BuyerClient>(null));
            _clientsRepository.GetClientAsync(Arg.Any<string>(), Arg.Any<string>())
                .Returns(Task.FromResult(new BuyerClient { Id = firstClientId }), Task.FromResult(new BuyerClient { Id = firstClientId }), Task.FromResult(new BuyerClient { Id = secondClientId }));

            var middleware = CreateMiddleware();
            await RunAsync(middleware, CreateHttpContext(TeamMemberEmail));
            await RunAsync(middleware, CreateHttpContext(otherTeamMemberEmail));

            await new ClaimsCacheInvalidatorService(_cache).InvalidateAsync(OrganisationId);

            Assert.Equal(secondClientId, await RunAsync(middleware, CreateHttpContext(TeamMemberEmail)));
            Assert.Equal(secondClientId, await RunAsync(middleware, CreateHttpContext(otherTeamMemberEmail)));
            await _clientsRepository.Received(4).GetClientAsync(Arg.Any<string>(), Arg.Any<string>());
        }

        [Fact]
        public async Task InvalidateAsync_LeavesTheClaimsOfOtherOrganisationsCached()
        {
            var otherOrganisationId = Guid.NewGuid();
            _clientsRepository.GetClientByEmailAsync(Arg.Any<string>(), Arg.Any<string>(), TeamMemberEmail)
                .Returns(Task.FromResult<BuyerClient>(null));
            _clientsRepository.GetClientAsync(Arg.Any<string>(), Arg.Any<string>())
                .Returns(Task.FromResult(new BuyerClient { Id = Guid.NewGuid() }));

            var middleware = CreateMiddleware();
            await RunAsync(middleware, CreateHttpContext(TeamMemberEmail, otherOrganisationId));

            await new ClaimsCacheInvalidatorService(_cache).InvalidateAsync(OrganisationId);
            await RunAsync(middleware, CreateHttpContext(TeamMemberEmail, otherOrganisationId));

            await _clientsRepository.Received(1).GetClientAsync(Arg.Any<string>(), Arg.Any<string>());
        }

        [Fact]
        public async Task InvokeAsync_WhenTheOrganisationsVersionIsGone_NeverReadsTheEntriesCachedUnderAnEarlierOne()
        {
            var firstClientId = Guid.NewGuid();
            var secondClientId = Guid.NewGuid();
            _clientsRepository.GetClientByEmailAsync(Arg.Any<string>(), Arg.Any<string>(), Email)
                .Returns(Task.FromResult(new BuyerClient { Id = firstClientId, Email = Email }), Task.FromResult(new BuyerClient { Id = secondClientId, Email = Email }));

            var middleware = CreateMiddleware();
            await RunAsync(middleware, CreateHttpContext());

            // Evicted, or expired, while the entries it versioned are still cached.
            await _cache.RemoveAsync(ClaimsEnrichmentConstants.GetClientClaimsVersionCacheKey(OrganisationId));

            Assert.Equal(secondClientId, await RunAsync(middleware, CreateHttpContext()));
        }

        [Fact]
        public async Task InvokeAsync_ForAnUnauthenticatedUser_DoesNotLookAnythingUp()
        {
            var context = new DefaultHttpContext { User = new ClaimsPrincipal(new ClaimsIdentity()) };
            var nextCalled = false;

            await CreateMiddleware().InvokeAsync(context, _ =>
            {
                nextCalled = true;

                return Task.CompletedTask;
            });

            Assert.True(nextCalled);
            await _clientsRepository.DidNotReceiveWithAnyArgs().GetClientByEmailAsync(default, default, default);
        }
    }
}
