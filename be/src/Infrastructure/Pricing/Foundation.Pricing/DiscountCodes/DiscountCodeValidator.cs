using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Foundation.Pricing.DiscountCodes
{
    /// <summary>
    /// Scoped: one instance serves one request. Every failure to reach a verdict fails closed to
    /// <see cref="DiscountCodeValidationStatus.Unavailable"/>, and that verdict is remembered for the rest of the
    /// request too, so every seam sees the same decision. The next request gets a fresh validator and retries.
    /// </summary>
    public sealed class DiscountCodeValidator : IDiscountCodeValidator
    {
        private readonly IDiscountCodeLookup _lookup;
        private readonly ILogger<DiscountCodeValidator> _logger;
        private readonly object _lock = new();
        private readonly Dictionary<CacheKey, Task<DiscountCodeValidation>> _results = new();

        public DiscountCodeValidator(IDiscountCodeLookup lookup, ILogger<DiscountCodeValidator> logger)
        {
            _lookup = lookup;
            _logger = logger;
        }

        public async Task<DiscountCodeValidation> ValidateAsync(Guid? clientId, string discountCode, string token, CancellationToken cancellationToken = default)
        {
            var code = discountCode?.Trim();

            if (string.IsNullOrEmpty(code))
            {
                return DiscountCodeValidation.Invalid(DiscountCodeValidationStatus.NotFound);
            }

            if (!clientId.HasValue || clientId.Value == Guid.Empty)
            {
                return DiscountCodeValidation.Invalid(DiscountCodeValidationStatus.ClientUnknown);
            }

            if (code.Length > DiscountCodeLimits.MaxLength)
            {
                return DiscountCodeValidation.Invalid(DiscountCodeValidationStatus.NotApplicable);
            }

            var key = new CacheKey(token, clientId.Value, code);
            Task<DiscountCodeValidation> result;

            lock (_lock)
            {
                if (!_results.TryGetValue(key, out result))
                {
                    result = ResolveAsync(clientId.Value, code, token, cancellationToken);
                    _results[key] = result;
                }
            }

            try
            {
                return await result;
            }
            catch (OperationCanceledException)
            {
                // A cancelled attempt says nothing about the code, so it must not be remembered.
                lock (_lock)
                {
                    if (_results.TryGetValue(key, out var cached) && ReferenceEquals(cached, result))
                    {
                        _results.Remove(key);
                    }
                }

                throw;
            }
        }

        private async Task<DiscountCodeValidation> ResolveAsync(Guid clientId, string code, string token, CancellationToken cancellationToken)
        {
            try
            {
                var validation = await _lookup.ValidateAsync(clientId, code, token, cancellationToken);

                return Normalize(validation, clientId);
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (Exception ex)
            {
                // The message is logged, never the token.
                _logger.LogWarning(ex, "Discount code verification failed for client {ClientId}. The code is treated as unverified.", clientId);

                return DiscountCodeValidation.Invalid(DiscountCodeValidationStatus.Unavailable);
            }
        }

        private DiscountCodeValidation Normalize(DiscountCodeValidation validation, Guid clientId)
        {
            if (validation is null)
            {
                _logger.LogWarning("Discount code verification for client {ClientId} returned no answer. The code is treated as unverified.", clientId);

                return DiscountCodeValidation.Invalid(DiscountCodeValidationStatus.Unavailable);
            }

            if (!validation.IsValid)
            {
                // Never pass a code through with an inapplicable verdict.
                return DiscountCodeValidation.Invalid(validation.Status);
            }

            var canonical = validation.DiscountCode?.Trim();

            if (string.IsNullOrEmpty(canonical) || canonical.Length > DiscountCodeLimits.MaxLength)
            {
                _logger.LogWarning("Discount code verification for client {ClientId} returned an unusable canonical code. The code is treated as unverified.", clientId);

                return DiscountCodeValidation.Invalid(DiscountCodeValidationStatus.Unavailable);
            }

            return DiscountCodeValidation.Valid(canonical);
        }

        private readonly struct CacheKey : IEquatable<CacheKey>
        {
            private readonly string _token;
            private readonly Guid _clientId;
            private readonly string _code;

            public CacheKey(string token, Guid clientId, string code)
            {
                _token = token;
                _clientId = clientId;
                _code = code;
            }

            // The basket seam asks with the user's spelling and the pricing seam then asks with the canonical one.
            public bool Equals(CacheKey other)
            {
                return string.Equals(_token, other._token, StringComparison.Ordinal)
                    && _clientId == other._clientId
                    && string.Equals(_code, other._code, StringComparison.OrdinalIgnoreCase);
            }

            public override bool Equals(object obj) => obj is CacheKey other && Equals(other);

            public override int GetHashCode()
            {
                return HashCode.Combine(
                    _token is null ? 0 : StringComparer.Ordinal.GetHashCode(_token),
                    _clientId,
                    StringComparer.OrdinalIgnoreCase.GetHashCode(_code));
            }
        }
    }
}
