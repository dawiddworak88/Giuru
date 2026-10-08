using System;
using System.Collections.Concurrent;

namespace Foundation.Pricing.DiscountCodes
{
    /// <summary>
    /// Shared (singleton) memory of the discount code driver id, kept apart from <see cref="GrulaDiscountCodeService"/>
    /// because that service holds the typed HTTP client and so cannot be a singleton itself. Only a successful
    /// driver lookup is stored, for a bounded time.
    /// </summary>
    public class GrulaDriverIdCache
    {
        public static readonly TimeSpan DefaultDuration = TimeSpan.FromMinutes(10);

        private readonly ConcurrentDictionary<string, Entry> _entries = new();
        private readonly TimeProvider _timeProvider;
        private readonly TimeSpan _duration;

        public GrulaDriverIdCache()
            : this(TimeProvider.System, DefaultDuration)
        {
        }

        public GrulaDriverIdCache(TimeProvider timeProvider, TimeSpan duration)
        {
            _timeProvider = timeProvider;
            _duration = duration;
        }

        public bool TryGet(string apiUrl, Guid environmentId, out Guid driverId)
        {
            var key = CreateKey(apiUrl, environmentId);

            if (_entries.TryGetValue(key, out var entry))
            {
                if (entry.ExpiresAt > _timeProvider.GetUtcNow())
                {
                    driverId = entry.DriverId;

                    return true;
                }

                _entries.TryRemove(key, out _);
            }

            driverId = Guid.Empty;

            return false;
        }

        public void Set(string apiUrl, Guid environmentId, Guid driverId)
        {
            _entries[CreateKey(apiUrl, environmentId)] = new Entry(driverId, _timeProvider.GetUtcNow().Add(_duration));
        }

        public void Invalidate(string apiUrl, Guid environmentId)
        {
            _entries.TryRemove(CreateKey(apiUrl, environmentId), out _);
        }

        private static string CreateKey(string apiUrl, Guid environmentId)
        {
            return $"{apiUrl?.TrimEnd('/').ToLowerInvariant()}|{environmentId:N}";
        }

        private sealed record Entry(Guid DriverId, DateTimeOffset ExpiresAt);
    }
}
