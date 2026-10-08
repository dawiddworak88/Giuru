using Foundation.Pricing.Configurations;
using Foundation.Pricing.Definitions;
using Grula.PricingIntelligencePlatform.Sdk;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Foundation.Pricing.DiscountCodes
{
    public class GrulaDiscountCodeService : IGrulaDiscountCodeService
    {
        // ASSUMPTIONS, to be confirmed against live Grula (plan, phase 0): paging starts at 1, and the price API matches
        // the Value of a driver item (Name is the fallback when Value is blank). They are kept in one place so that the
        // unit tests pin them and a different answer from the spike is a one-line change.
        public const int FirstPageIndex = 1;
        public const int PageSize = 100;

        // The HTTP client timeout is per call, so the whole paged lookup is bounded separately.
        public const int MaxPages = 100;
        public static readonly TimeSpan TotalLookupTimeout = TimeSpan.FromSeconds(30);

        private readonly GrulaApiClient _grulaApiClient;
        private readonly IPricingSettings _settings;
        private readonly GrulaDriverIdCache _driverIdCache;
        private readonly ILogger<GrulaDiscountCodeService> _logger;

        public GrulaDiscountCodeService(
            GrulaApiClient grulaApiClient,
            IPricingSettings settings,
            GrulaDriverIdCache driverIdCache,
            ILogger<GrulaDiscountCodeService> logger)
        {
            _grulaApiClient = grulaApiClient;
            _settings = settings;
            _driverIdCache = driverIdCache;
            _logger = logger;
        }

        public async Task<GrulaDiscountCodeLookup> FindAsync(string discountCode, CancellationToken cancellationToken = default)
        {
            if (!_settings.IsGrulaConfigured)
            {
                return GrulaDiscountCodeLookup.Of(GrulaDiscountCodeLookupStatus.NotConfigured);
            }

            var code = discountCode?.Trim();

            if (string.IsNullOrEmpty(code))
            {
                return GrulaDiscountCodeLookup.Of(GrulaDiscountCodeLookupStatus.Missing);
            }

            using var timeout = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            timeout.CancelAfter(TotalLookupTimeout);

            try
            {
                var environmentId = Guid.Parse(_settings.GrulaEnvironmentId);

                return await FindAsync(environmentId, code, retryOnStaleDriver: true, timeout.Token);
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (Exception ex)
            {
                // Includes API errors, transport failures and the timeout above: none of them may let a code be saved.
                _logger.LogWarning(ex, "The discount code could not be looked up in Grula.");

                return GrulaDiscountCodeLookup.Of(GrulaDiscountCodeLookupStatus.Unavailable);
            }
        }

        private async Task<GrulaDiscountCodeLookup> FindAsync(Guid environmentId, string code, bool retryOnStaleDriver, CancellationToken cancellationToken)
        {
            var wasCached = _driverIdCache.TryGet(_grulaApiClient.BaseUrl, environmentId, out var driverId);

            if (!wasCached)
            {
                var driver = await FindDriverIdAsync(environmentId, cancellationToken);

                if (driver is null)
                {
                    return GrulaDiscountCodeLookup.Of(GrulaDiscountCodeLookupStatus.Missing);
                }

                driverId = driver.Value;
                _driverIdCache.Set(_grulaApiClient.BaseUrl, environmentId, driverId);
            }

            try
            {
                return await FindItemAsync(driverId, code, cancellationToken);
            }
            catch (ApiException ex) when (ex.StatusCode == (int)HttpStatusCode.NotFound && wasCached && retryOnStaleDriver)
            {
                // The driver was removed or recreated after its id was cached.
                _driverIdCache.Invalidate(_grulaApiClient.BaseUrl, environmentId);

                return await FindAsync(environmentId, code, retryOnStaleDriver: false, cancellationToken);
            }
        }

        private async Task<Guid?> FindDriverIdAsync(Guid environmentId, CancellationToken cancellationToken)
        {
            var seenPages = new HashSet<Guid>();

            for (var page = 0; page < MaxPages; page++)
            {
                var drivers = await _grulaApiClient.GetDriversAsync(environmentId, FirstPageIndex + page, PageSize, cancellationToken);
                var items = drivers?.Items?.ToList() ?? new List<DriverReadModel>();

                var driver = items.FirstOrDefault(x => string.Equals(x.Name, PriceDriversConstants.DiscountCodeDriver, StringComparison.OrdinalIgnoreCase));

                if (driver is not null)
                {
                    return driver.Id;
                }

                if (drivers is null || !drivers.HasNextPage)
                {
                    return null;
                }

                EnsureAdvancing(items.Select(x => x.Id), seenPages, "drivers");
            }

            throw new InvalidOperationException("Grula returned more pages of drivers than expected.");
        }

        private async Task<GrulaDiscountCodeLookup> FindItemAsync(Guid driverId, string code, CancellationToken cancellationToken)
        {
            var seenItems = new HashSet<Guid>();
            var matches = new List<string>();

            for (var page = 0; page < MaxPages; page++)
            {
                // No search term: it is not known to be case-insensitive, so it could hide a case variant of the code.
                var driverItems = await _grulaApiClient.GetDriverItemsAsync(driverId, FirstPageIndex + page, PageSize, null, cancellationToken);
                var items = driverItems?.Items?.ToList() ?? new List<DriverItemReadModel>();

                matches.AddRange(items
                    .Select(GetMatchedValue)
                    .Where(x => string.Equals(x, code, StringComparison.OrdinalIgnoreCase)));

                if (driverItems is null || !driverItems.HasNextPage)
                {
                    return CreateLookup(matches);
                }

                EnsureAdvancing(items.Select(x => x.Id), seenItems, "driver items");
            }

            throw new InvalidOperationException("Grula returned more pages of driver items than expected.");
        }

        private GrulaDiscountCodeLookup CreateLookup(List<string> matches)
        {
            if (matches.Count == 0)
            {
                return GrulaDiscountCodeLookup.Of(GrulaDiscountCodeLookupStatus.Missing);
            }

            if (matches.Count > 1)
            {
                // Two items that only differ by case cannot be told apart by the case-insensitive matching used on every
                // later request, so picking one would be arbitrary.
                _logger.LogWarning("The discount code driver in Grula holds {Count} items that match the code ignoring case. The configuration is unusable for this code.", matches.Count);

                return GrulaDiscountCodeLookup.Of(GrulaDiscountCodeLookupStatus.Unavailable);
            }

            return GrulaDiscountCodeLookup.Exists(matches[0]);
        }

        private static string GetMatchedValue(DriverItemReadModel item)
        {
            return string.IsNullOrWhiteSpace(item.Value) ? item.Name?.Trim() : item.Value.Trim();
        }

        private static void EnsureAdvancing(IEnumerable<Guid> pageIds, HashSet<Guid> seenIds, string description)
        {
            var ids = pageIds.ToList();

            if (ids.Count == 0 || ids.Any(x => !seenIds.Add(x)))
            {
                throw new InvalidOperationException($"Grula returned a page of {description} that does not advance.");
            }
        }
    }
}
