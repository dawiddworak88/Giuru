using Buyer.Web.Shared.Configurations;
using Buyer.Web.Shared.Definitions.Middlewares;
using Buyer.Web.Shared.DomainModels.Clients;
using Buyer.Web.Shared.DomainModels.Global;
using Buyer.Web.Shared.Middlewares;
using Buyer.Web.Shared.Repositories.Clients;
using Buyer.Web.Shared.Repositories.Global;
using Foundation.Extensions.Exceptions;
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
using BuyerClient = Buyer.Web.Shared.DomainModels.Clients.Client;
using System.Threading.Tasks;

namespace Giuru.UnitTests.Middlewares
{
    // A Team Member shares the organisation of the Client they belong to but has an email of their own, so
    // the Client has to be resolved from the organisation. These tests pin that the Client's id and
    // Client-derived claims reach the request for such a user, on a claims-cache miss as well as a hit.
    public class ClaimsEnrichmentMiddlewareTests
    {
        private const string TeamMemberEmail = "team.member@example.com";

        [Fact]
        public async Task InvokeAsync_TeamMemberWhoseEmailMatchesNoClient_GetsTheirClientsClaims()
        {
            var client = new BuyerClient { Id = Guid.NewGuid(), Name = "Acme" };
            var fixture = new Fixture(client);

            var user = await fixture.InvokeAsync(TeamMemberEmail);

            Assert.Equal(client.Id.ToString(), ClaimValue(user, ClaimsEnrichmentConstants.ClientIdClaimType));
            Assert.Equal("Acme", ClaimValue(user, ClaimsEnrichmentConstants.ClientNameClaimType));
            await fixture.ClientsRepository.Received(1).GetClientAsync(Arg.Any<string>(), Arg.Any<string>());
        }

        [Fact]
        public async Task InvokeAsync_OnClaimsCacheMiss_AddsClientIdToTheCurrentRequest()
        {
            var client = new BuyerClient { Id = Guid.NewGuid() };
            var fixture = new Fixture(client);

            var user = await fixture.InvokeAsync(TeamMemberEmail);

            Assert.Equal(client.Id.ToString(), ClaimValue(user, ClaimsEnrichmentConstants.ClientIdClaimType));
        }

        [Fact]
        public async Task InvokeAsync_OnClaimsCacheHit_AddsClientIdFromTheCache()
        {
            var client = new BuyerClient { Id = Guid.NewGuid(), Name = "Acme" };
            var fixture = new Fixture(client);
            await fixture.InvokeAsync(TeamMemberEmail);

            var user = await fixture.InvokeAsync(TeamMemberEmail);

            Assert.Equal(client.Id.ToString(), ClaimValue(user, ClaimsEnrichmentConstants.ClientIdClaimType));
            Assert.Equal("Acme", ClaimValue(user, ClaimsEnrichmentConstants.ClientNameClaimType));
            await fixture.ClientsRepository.Received(1).GetClientAsync(Arg.Any<string>(), Arg.Any<string>());
        }

        [Fact]
        public async Task InvokeAsync_OrganisationHasNoClient_AddsNoClaimsAndContinues()
        {
            var fixture = new Fixture(null);

            var user = await fixture.InvokeAsync(TeamMemberEmail);

            Assert.Null(ClaimValue(user, ClaimsEnrichmentConstants.ClientIdClaimType));
            Assert.True(fixture.NextCalled);
        }

        [Fact]
        public async Task InvokeAsync_ClientLookupFails_AddsNoClaimsAndContinues()
        {
            var fixture = new Fixture(null);
            fixture.ClientsRepository.GetClientAsync(Arg.Any<string>(), Arg.Any<string>())
                .Returns<Task<BuyerClient>>(_ => throw new CustomException("Unprocessable", 422));

            var user = await fixture.InvokeAsync(TeamMemberEmail);

            Assert.Null(ClaimValue(user, ClaimsEnrichmentConstants.ClientIdClaimType));
            Assert.True(fixture.NextCalled);
        }

        private static string ClaimValue(ClaimsPrincipal user, string type) => user.Claims.FirstOrDefault(x => x.Type == type)?.Value;

        private class Fixture
        {
            private readonly ClaimsEnrichmentMiddleware _middleware;

            public Fixture(BuyerClient client)
            {
                ClientsRepository = Substitute.For<IClientsRepository>();
                ClientsRepository.GetClientAsync(Arg.Any<string>(), Arg.Any<string>()).Returns(Task.FromResult(client));

                var addressesRepository = Substitute.For<IClientAddressesRepository>();
                var globalRepository = Substitute.For<IGlobalRepository>();
                var fieldValuesRepository = Substitute.For<IClientFieldValuesRepository>();
                fieldValuesRepository.GetAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<Guid?>())
                    .Returns(Task.FromResult<IEnumerable<ClientFieldValue>>([]));

                var cacheService = Substitute.For<ICacheService>();
                cacheService.GetOrSetAsync(Arg.Any<string>(), Arg.Any<Func<Task<IEnumerable<Country>>>>(), Arg.Any<TimeSpan?>())
                    .Returns(Task.FromResult<IEnumerable<Country>>([]));

                _middleware = new ClaimsEnrichmentMiddleware(
                    ClientsRepository,
                    addressesRepository,
                    globalRepository,
                    fieldValuesRepository,
                    Options.Create(new AppSettings { DefaultCulture = "en-US" }),
                    new MemoryDistributedCache(Options.Create(new MemoryDistributedCacheOptions())),
                    cacheService);
            }

            public IClientsRepository ClientsRepository { get; }

            public bool NextCalled { get; private set; }

            public async Task<ClaimsPrincipal> InvokeAsync(string email)
            {
                var identity = new ClaimsIdentity([new Claim(ClaimTypes.Email, email)], "test");
                var authenticationService = Substitute.For<IAuthenticationService>();
                authenticationService.AuthenticateAsync(Arg.Any<HttpContext>(), Arg.Any<string>())
                    .Returns(Task.FromResult(AuthenticateResult.NoResult()));
                var services = new ServiceCollection().AddSingleton(authenticationService).BuildServiceProvider();
                var context = new DefaultHttpContext { User = new ClaimsPrincipal(identity), RequestServices = services };

                await _middleware.InvokeAsync(context, _ =>
                {
                    NextCalled = true;
                    return Task.CompletedTask;
                });

                return context.User;
            }
        }
    }
}
