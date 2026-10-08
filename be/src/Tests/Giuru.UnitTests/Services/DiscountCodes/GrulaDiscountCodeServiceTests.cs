using Foundation.Pricing.Definitions;
using Foundation.Pricing.DiscountCodes;
using Giuru.UnitTests.Helpers;
using Grula.PricingIntelligencePlatform.Sdk;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Giuru.UnitTests.Services.DiscountCodes
{
    public class GrulaDiscountCodeServiceTests
    {
        private static readonly Guid EnvironmentId = Guid.Parse("00000000-0000-0000-0000-000000000001");
        private static readonly Guid DriverId = Guid.NewGuid();

        // Substituting the (non-sealed) client with its constructor arguments keeps an unconfigured call from reaching the network.
        private static GrulaApiClient CreateClient(string baseUrl = "http://localhost")
        {
            return Substitute.For<GrulaApiClient>(baseUrl, new HttpClient());
        }

        private static GrulaDiscountCodeService CreateService(
            GrulaApiClient client,
            GrulaDriverIdCache cache = null,
            bool configured = true)
        {
            var settings = new TestPricingSettings
            {
                GrulaAccessToken = configured ? "token" : null,
                GrulaEnvironmentId = EnvironmentId.ToString()
            };

            return new GrulaDiscountCodeService(client, settings, cache ?? new GrulaDriverIdCache(), Substitute.For<ILogger<GrulaDiscountCodeService>>());
        }

        private static DriverReadModelIPaged Drivers(bool hasNextPage, params DriverReadModel[] drivers)
        {
            return new DriverReadModelIPaged { HasNextPage = hasNextPage, Items = drivers.ToList() };
        }

        private static DriverReadModel Driver(string name, Guid? id = null)
        {
            return new DriverReadModel { Id = id ?? Guid.NewGuid(), Name = name };
        }

        private static DriverItemReadModelIPaged Items(bool hasNextPage, params DriverItemReadModel[] items)
        {
            return new DriverItemReadModelIPaged { HasNextPage = hasNextPage, Items = items.ToList() };
        }

        private static DriverItemReadModel Item(string value, string name = null, Guid? id = null)
        {
            return new DriverItemReadModel { Id = id ?? Guid.NewGuid(), Name = name ?? value, Value = value, DriverId = DriverId };
        }

        private static void ReturnsDiscountCodeDriver(GrulaApiClient client)
        {
            client.GetDriversAsync(EnvironmentId, Arg.Any<int>(), Arg.Any<int>(), Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(Drivers(false, Driver("Other"), Driver(PriceDriversConstants.DiscountCodeDriver, DriverId))));
        }

        private static void ReturnsItems(GrulaApiClient client, DriverItemReadModelIPaged page)
        {
            client.GetDriverItemsAsync(DriverId, Arg.Any<int>(), Arg.Any<int>(), Arg.Any<string>(), Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(page));
        }

        [Fact]
        public async Task FindAsync_WhenGrulaIsNotConfigured_ReturnsNotConfiguredWithoutACall()
        {
            var client = CreateClient();

            var lookup = await CreateService(client, configured: false).FindAsync("SUMMER25");

            Assert.Equal(GrulaDiscountCodeLookupStatus.NotConfigured, lookup.Status);
            Assert.Null(lookup.DiscountCode);
            await client.DidNotReceiveWithAnyArgs().GetDriversAsync(default, default, default, default);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task FindAsync_ForABlankCode_IsMissingWithoutACall(string code)
        {
            var client = CreateClient();

            var lookup = await CreateService(client).FindAsync(code);

            Assert.Equal(GrulaDiscountCodeLookupStatus.Missing, lookup.Status);
            await client.DidNotReceiveWithAnyArgs().GetDriversAsync(default, default, default, default);
        }

        [Fact]
        public async Task FindAsync_WhenTheItemExistsIgnoringCase_ReturnsGrulasSpelling()
        {
            var client = CreateClient();
            ReturnsDiscountCodeDriver(client);
            ReturnsItems(client, Items(false, Item("WINTER25"), Item("SUMMER25")));

            var lookup = await CreateService(client).FindAsync("  summer25 ");

            Assert.Equal(GrulaDiscountCodeLookupStatus.Exists, lookup.Status);
            Assert.Equal("SUMMER25", lookup.DiscountCode);
        }

        [Fact]
        public async Task FindAsync_WhenTheValueIsBlank_FallsBackToTheItemName()
        {
            var client = CreateClient();
            ReturnsDiscountCodeDriver(client);
            ReturnsItems(client, Items(false, new DriverItemReadModel { Id = Guid.NewGuid(), Name = "SUMMER25", Value = " " }));

            var lookup = await CreateService(client).FindAsync("summer25");

            Assert.Equal("SUMMER25", lookup.DiscountCode);
        }

        [Fact]
        public async Task FindAsync_WhenTheDriverIsOnALaterPage_PagesUntilItIsFound()
        {
            var client = CreateClient();
            client.GetDriversAsync(EnvironmentId, 1, Arg.Any<int>(), Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(Drivers(true, Driver("First"))));
            client.GetDriversAsync(EnvironmentId, 2, Arg.Any<int>(), Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(Drivers(false, Driver(PriceDriversConstants.DiscountCodeDriver, DriverId))));
            ReturnsItems(client, Items(false, Item("SUMMER25")));

            var lookup = await CreateService(client).FindAsync("SUMMER25");

            Assert.Equal(GrulaDiscountCodeLookupStatus.Exists, lookup.Status);
        }

        [Fact]
        public async Task FindAsync_WhenTheItemIsOnALaterPage_PagesUntilItIsFound()
        {
            var client = CreateClient();
            ReturnsDiscountCodeDriver(client);
            client.GetDriverItemsAsync(DriverId, 1, Arg.Any<int>(), Arg.Any<string>(), Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(Items(true, Item("A"))));
            client.GetDriverItemsAsync(DriverId, 2, Arg.Any<int>(), Arg.Any<string>(), Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(Items(false, Item("SUMMER25"))));

            var lookup = await CreateService(client).FindAsync("summer25");

            Assert.Equal("SUMMER25", lookup.DiscountCode);
        }

        [Fact]
        public async Task FindAsync_DoesNotUseASearchTerm_SoACaseVariantCannotBeHidden()
        {
            var client = CreateClient();
            ReturnsDiscountCodeDriver(client);
            ReturnsItems(client, Items(false, Item("SUMMER25")));

            await CreateService(client).FindAsync("summer25");

            await client.Received().GetDriverItemsAsync(DriverId, Arg.Any<int>(), Arg.Any<int>(), null, Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task FindAsync_WhenTwoItemsMatchIgnoringCase_IsUnavailableInsteadOfPickingOne()
        {
            var client = CreateClient();
            ReturnsDiscountCodeDriver(client);
            ReturnsItems(client, Items(false, Item("SUMMER25"), Item("Summer25")));

            var lookup = await CreateService(client).FindAsync("summer25");

            Assert.Equal(GrulaDiscountCodeLookupStatus.Unavailable, lookup.Status);
            Assert.Null(lookup.DiscountCode);
        }

        [Fact]
        public async Task FindAsync_WhenThereIsNoDiscountCodeDriver_IsMissing()
        {
            var client = CreateClient();
            client.GetDriversAsync(EnvironmentId, Arg.Any<int>(), Arg.Any<int>(), Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(Drivers(false, Driver("Other"))));

            var lookup = await CreateService(client).FindAsync("SUMMER25");

            Assert.Equal(GrulaDiscountCodeLookupStatus.Missing, lookup.Status);
        }

        [Fact]
        public async Task FindAsync_WhenNoItemMatches_IsMissing()
        {
            var client = CreateClient();
            ReturnsDiscountCodeDriver(client);
            ReturnsItems(client, Items(false, Item("WINTER25")));

            var lookup = await CreateService(client).FindAsync("SUMMER25");

            Assert.Equal(GrulaDiscountCodeLookupStatus.Missing, lookup.Status);
        }

        [Fact]
        public async Task FindAsync_WhenTheApiFails_IsUnavailable()
        {
            var client = CreateClient();
            client.GetDriversAsync(EnvironmentId, Arg.Any<int>(), Arg.Any<int>(), Arg.Any<CancellationToken>())
                .ThrowsAsync(new ApiException("forbidden", 403, null, new Dictionary<string, IEnumerable<string>>(), null));

            var lookup = await CreateService(client).FindAsync("SUMMER25");

            Assert.Equal(GrulaDiscountCodeLookupStatus.Unavailable, lookup.Status);
        }

        [Fact]
        public async Task FindAsync_WhenTheTransportFails_IsUnavailable()
        {
            var client = CreateClient();
            client.GetDriversAsync(EnvironmentId, Arg.Any<int>(), Arg.Any<int>(), Arg.Any<CancellationToken>())
                .ThrowsAsync(new HttpRequestException("down"));

            var lookup = await CreateService(client).FindAsync("SUMMER25");

            Assert.Equal(GrulaDiscountCodeLookupStatus.Unavailable, lookup.Status);
        }

        [Fact]
        public async Task FindAsync_WhenTheHttpClientTimesOut_IsUnavailable()
        {
            var client = CreateClient();
            client.GetDriversAsync(EnvironmentId, Arg.Any<int>(), Arg.Any<int>(), Arg.Any<CancellationToken>())
                .ThrowsAsync(new TaskCanceledException("timeout"));

            var lookup = await CreateService(client).FindAsync("SUMMER25", CancellationToken.None);

            Assert.Equal(GrulaDiscountCodeLookupStatus.Unavailable, lookup.Status);
        }

        [Fact]
        public async Task FindAsync_WhenTheCallerCancels_Propagates()
        {
            using var cancellation = new CancellationTokenSource();
            var client = CreateClient();
            client.GetDriversAsync(EnvironmentId, Arg.Any<int>(), Arg.Any<int>(), Arg.Any<CancellationToken>())
                .Returns<Task<DriverReadModelIPaged>>(call =>
                {
                    cancellation.Cancel();
                    call.ArgAt<CancellationToken>(3).ThrowIfCancellationRequested();

                    return Task.FromResult(Drivers(false));
                });

            await Assert.ThrowsAnyAsync<OperationCanceledException>(() => CreateService(client).FindAsync("SUMMER25", cancellation.Token));
        }

        [Fact]
        public async Task FindAsync_WhenAPageDoesNotAdvance_IsUnavailable()
        {
            var client = CreateClient();
            var repeated = Item("A", id: Guid.NewGuid());
            ReturnsDiscountCodeDriver(client);

            // Grula keeps claiming a next page but serves the same item again.
            ReturnsItems(client, Items(true, repeated));

            var lookup = await CreateService(client).FindAsync("SUMMER25");

            Assert.Equal(GrulaDiscountCodeLookupStatus.Unavailable, lookup.Status);
        }

        [Fact]
        public async Task FindAsync_WhenANextPageIsEmpty_IsUnavailable()
        {
            var client = CreateClient();
            ReturnsDiscountCodeDriver(client);
            ReturnsItems(client, Items(true));

            var lookup = await CreateService(client).FindAsync("SUMMER25");

            Assert.Equal(GrulaDiscountCodeLookupStatus.Unavailable, lookup.Status);
        }

        [Fact]
        public async Task FindAsync_WhenDriverPagingDoesNotAdvance_IsUnavailable()
        {
            var client = CreateClient();
            var sameDriver = Driver("Other");
            client.GetDriversAsync(EnvironmentId, Arg.Any<int>(), Arg.Any<int>(), Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(Drivers(true, sameDriver)));

            var lookup = await CreateService(client).FindAsync("SUMMER25");

            Assert.Equal(GrulaDiscountCodeLookupStatus.Unavailable, lookup.Status);
        }

        [Fact]
        public async Task FindAsync_AfterASuccessfulDriverLookup_ReusesTheCachedDriverId()
        {
            var client = CreateClient();
            ReturnsDiscountCodeDriver(client);
            ReturnsItems(client, Items(false, Item("SUMMER25")));
            var service = CreateService(client);

            await service.FindAsync("SUMMER25");
            await service.FindAsync("SUMMER25");

            await client.Received(1).GetDriversAsync(EnvironmentId, Arg.Any<int>(), Arg.Any<int>(), Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task FindAsync_DoesNotCacheAMissingDriverOrAnUnavailableAnswer()
        {
            var client = CreateClient();
            client.GetDriversAsync(EnvironmentId, Arg.Any<int>(), Arg.Any<int>(), Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(Drivers(false, Driver("Other"))));
            var service = CreateService(client);

            await service.FindAsync("SUMMER25");
            await service.FindAsync("SUMMER25");

            await client.Received(2).GetDriversAsync(EnvironmentId, Arg.Any<int>(), Arg.Any<int>(), Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task FindAsync_WhenTheCachedDriverIsGone_InvalidatesItAndLooksTheDriverUpAgain()
        {
            var staleDriverId = Guid.NewGuid();
            var cache = new GrulaDriverIdCache();
            cache.Set("http://localhost", EnvironmentId, staleDriverId);

            var client = CreateClient();
            ReturnsDiscountCodeDriver(client);
            client.GetDriverItemsAsync(staleDriverId, Arg.Any<int>(), Arg.Any<int>(), Arg.Any<string>(), Arg.Any<CancellationToken>())
                .ThrowsAsync(new ApiException("not found", 404, null, new Dictionary<string, IEnumerable<string>>(), null));
            ReturnsItems(client, Items(false, Item("SUMMER25")));

            var lookup = await CreateService(client, cache).FindAsync("SUMMER25");

            Assert.Equal("SUMMER25", lookup.DiscountCode);
            Assert.True(cache.TryGet("http://localhost", EnvironmentId, out var cached));
            Assert.Equal(DriverId, cached);
        }

        [Fact]
        public async Task FindAsync_WhenAFreshDriverAnswersNotFound_DoesNotRetryForever()
        {
            var client = CreateClient();
            ReturnsDiscountCodeDriver(client);
            client.GetDriverItemsAsync(DriverId, Arg.Any<int>(), Arg.Any<int>(), Arg.Any<string>(), Arg.Any<CancellationToken>())
                .ThrowsAsync(new ApiException("not found", 404, null, new Dictionary<string, IEnumerable<string>>(), null));

            var lookup = await CreateService(client).FindAsync("SUMMER25");

            Assert.Equal(GrulaDiscountCodeLookupStatus.Unavailable, lookup.Status);
            await client.Received(1).GetDriversAsync(EnvironmentId, Arg.Any<int>(), Arg.Any<int>(), Arg.Any<CancellationToken>());
        }
    }

    public class GrulaDriverIdCacheTests
    {
        private sealed class ManualTimeProvider : TimeProvider
        {
            public DateTimeOffset Now { get; set; } = new(2026, 10, 6, 12, 0, 0, TimeSpan.Zero);

            public override DateTimeOffset GetUtcNow() => Now;
        }

        private static readonly Guid EnvironmentId = Guid.NewGuid();

        [Fact]
        public void TryGet_ReturnsTheStoredIdUntilTheDurationHasPassed()
        {
            var time = new ManualTimeProvider();
            var cache = new GrulaDriverIdCache(time, TimeSpan.FromMinutes(10));
            var driverId = Guid.NewGuid();

            cache.Set("http://grula", EnvironmentId, driverId);

            time.Now = time.Now.AddMinutes(9);
            Assert.True(cache.TryGet("http://grula", EnvironmentId, out var cached));
            Assert.Equal(driverId, cached);

            time.Now = time.Now.AddMinutes(2);
            Assert.False(cache.TryGet("http://grula", EnvironmentId, out _));
        }

        [Fact]
        public void TryGet_IsIsolatedByApiUrlAndEnvironment()
        {
            var cache = new GrulaDriverIdCache();
            cache.Set("http://grula", EnvironmentId, Guid.NewGuid());

            Assert.False(cache.TryGet("http://other-grula", EnvironmentId, out _));
            Assert.False(cache.TryGet("http://grula", Guid.NewGuid(), out _));
        }

        [Fact]
        public void TryGet_IgnoresACaseOrTrailingSlashDifferenceInTheApiUrl()
        {
            var cache = new GrulaDriverIdCache();
            cache.Set("http://Grula/", EnvironmentId, Guid.NewGuid());

            Assert.True(cache.TryGet("http://grula", EnvironmentId, out _));
        }

        [Fact]
        public void Invalidate_RemovesTheEntry()
        {
            var cache = new GrulaDriverIdCache();
            cache.Set("http://grula", EnvironmentId, Guid.NewGuid());

            cache.Invalidate("http://grula", EnvironmentId);

            Assert.False(cache.TryGet("http://grula", EnvironmentId, out _));
        }
    }
}
