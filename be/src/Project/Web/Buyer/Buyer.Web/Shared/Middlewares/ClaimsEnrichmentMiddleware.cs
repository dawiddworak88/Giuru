using Buyer.Web.Shared.Configurations;
using Buyer.Web.Shared.Definitions.Middlewares;
using Buyer.Web.Shared.Repositories.Clients;
using Buyer.Web.Shared.Repositories.Global;
using Foundation.ApiExtensions.Definitions;
using Foundation.Extensions.Definitions;
using Foundation.Extensions.ExtensionMethods;
using Foundation.Extensions.Services.Cache;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ClaimsEnrichmentConstants = Buyer.Web.Shared.Definitions.Middlewares.ClaimsEnrichmentConstants;

namespace Buyer.Web.Shared.Middlewares
{
    public class ClaimsEnrichmentMiddleware : IMiddleware
    {
        private readonly IClientsRepository _clientsRepository;
        private readonly IClientAddressesRepository _clientAddressesRepository;
        private readonly IGlobalRepository _globalRepository;
        private readonly IClientFieldValuesRepository _clientFieldValuesRepository;
        private readonly IOptions<AppSettings> _options;
        private readonly IDistributedCache _cache;
        private readonly ICacheService _cacheService;

        public ClaimsEnrichmentMiddleware(
            IClientsRepository clientsRepository,
            IClientAddressesRepository clientAddressesRepository,
            IGlobalRepository globalRepository,
            IClientFieldValuesRepository clientFieldValuesRepository,
            IOptions<AppSettings> options,
            IDistributedCache cache,
            ICacheService cacheService)
        {
            _clientsRepository = clientsRepository;
            _clientAddressesRepository = clientAddressesRepository;
            _globalRepository = globalRepository;
            _clientFieldValuesRepository = clientFieldValuesRepository;
            _options = options;
            _cache = cache;
            _cacheService = cacheService;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (context.User.Identity?.IsAuthenticated is false)
            {
                await next(context);
                return;
            }

            var email = context.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;

            if (string.IsNullOrWhiteSpace(email))
            {
                await next(context);
                return;
            }

            var cacheKey = $"{ClaimsEnrichmentConstants.CacheKey}-{email}";
            var cachedClaims = await _cache.GetStringAsync(cacheKey);

            var claimsIdentity = (ClaimsIdentity)context.User.Identity;

            if (!string.IsNullOrWhiteSpace(cachedClaims))
            {
                var claims = JsonConvert.DeserializeObject<IEnumerable<CachedClaim>>(cachedClaims);

                foreach (var claim in claims)
                {
                    claimsIdentity.AddClaim(new Claim(claim.Key, claim.Value));
                }

                await next(context);
                return;
            }

            var token = await context.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var client = await _clientsRepository.GetClientByEmailAsync(token, _options.Value.DefaultCulture, email);

            if (client is null)
            {
                await next(context);
                return;
            }

            var claimsToCache = new List<CachedClaim>()
            {
                new CachedClaim
                {
                    Key = ClaimsEnrichmentConstants.ClientIdClaimType,
                    Value = client.Id.ToString()
                }
            };

            if (client.PreferedCurrencyId.HasValue)
            {
                var currencies = await _cacheService.GetOrSetAsync(
                    CacheKeysConstants.CurrenciesCacheKey,
                    () => _globalRepository.GetCurrenciesAsync(token, _options.Value.DefaultCulture, null));

                if (currencies.Any())
                {
                    var currency = currencies.FirstOrDefault(c => c.Id == client.PreferedCurrencyId);

                    if (currency is not null)
                    {
                        claimsIdentity.AddClaim(new Claim(ClaimsEnrichmentConstants.CurrencyClaimType, currency.CurrencyCode));

                        var currencyClaim = new CachedClaim
                        {
                            Key = ClaimsEnrichmentConstants.CurrencyClaimType,
                            Value = currency.CurrencyCode
                        };

                        claimsToCache.Add(currencyClaim);
                    }
                }
            }

            var countries = await _cacheService.GetOrSetAsync(
                CacheKeysConstants.CountriesCacheKey, 
                () => _globalRepository.GetCountriesAsync(token, _options.Value.DefaultCulture, null));

            if (countries is not null)
            {
                var clientDeliveryAddress = await _clientAddressesRepository.GetAsync(token, _options.Value.DefaultCulture, client.DefaultDeliveryAddressId);

                if (clientDeliveryAddress is not null)
                {
                    var deliveryCountry = countries.FirstOrDefault(c => c.Id == clientDeliveryAddress.CountryId);

                    if (deliveryCountry is not null)
                    {
                        var deliveryAddress = $"{clientDeliveryAddress.PostCode} ({clientDeliveryAddress.City}, {deliveryCountry.Name})";

                        claimsIdentity.AddClaim(new Claim(ClaimsEnrichmentConstants.ZipCodeClaimType, deliveryAddress));

                        var deliveryZipCodeClaim = new CachedClaim
                        {
                            Key = ClaimsEnrichmentConstants.ZipCodeClaimType,
                            Value = deliveryAddress
                        };

                        claimsToCache.Add(deliveryZipCodeClaim);
                    }
                }

                if (client.CountryId.HasValue)
                {
                    var clientCountry = countries.FirstOrDefault(c => c.Id == client.CountryId);

                    if (clientCountry is not null)
                    {
                        claimsIdentity.AddClaim(new Claim(ClaimsEnrichmentConstants.CountryClaimType, clientCountry.Name));

                        var countryClaim = new CachedClaim
                        {
                            Key = ClaimsEnrichmentConstants.CountryClaimType,
                            Value = clientCountry.Name
                        };

                        claimsToCache.Add(countryClaim);
                    }
                }
            }

            var clientFieldValues = await _clientFieldValuesRepository.GetAsync(token, _options.Value.DefaultCulture, client.Id);

            if (clientFieldValues.Any())
            {
                var extraPackingField = clientFieldValues.FirstOrDefault(x => x.FieldName == ClaimsEnrichmentConstants.ExtraPackingClientFieldName);

                if (extraPackingField is not null)
                {
                    claimsIdentity.AddClaim(new Claim(ClaimsEnrichmentConstants.ExtraPackingClaimType, extraPackingField.FieldValue.ToYesOrNo()));

                    var extraPackingClaim = new CachedClaim
                    {
                        Key = ClaimsEnrichmentConstants.ExtraPackingClaimType,
                        Value = extraPackingField.FieldValue.ToYesOrNo()
                    };

                    claimsToCache.Add(extraPackingClaim);
                }

                var paletteLoading = clientFieldValues.FirstOrDefault(x => x.FieldName == ClaimsEnrichmentConstants.PaletteLoadingClientFieldName);

                if (paletteLoading is not null)
                {
                    claimsIdentity.AddClaim(new Claim(ClaimsEnrichmentConstants.PaletteLoadingClaimType, paletteLoading.FieldValue.ToYesOrNo()));

                    var paletteLoadingClaim = new CachedClaim
                    {
                        Key = ClaimsEnrichmentConstants.PaletteLoadingClaimType,
                        Value = paletteLoading.FieldValue.ToYesOrNo()
                    };

                    claimsToCache.Add(paletteLoadingClaim);
                }

                var methodOfDelivery = clientFieldValues.FirstOrDefault(x => x.FieldName == ClaimsEnrichmentConstants.MethodOfDeliveryClientFieldName);

                if (methodOfDelivery is not null)
                {
                    bool includedTransport = !string.IsNullOrWhiteSpace(methodOfDelivery.FieldValue) && methodOfDelivery.FieldValue != ClaimsEnrichmentConstants.IncludedTransportFieldValue;

                    claimsIdentity.AddClaim(new Claim(ClaimsEnrichmentConstants.IncludedTransportClaimType, includedTransport.ToYesOrNo()));

                    var includedTransportClaim = new CachedClaim
                    {
                        Key = ClaimsEnrichmentConstants.IncludedTransportClaimType,
                        Value = includedTransport.ToYesOrNo()
                    };

                    claimsToCache.Add(includedTransportClaim);
                }
            }

            await _cache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(claimsToCache), new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
            });

            await next(context);
        }

        private class CachedClaim
        {
            public string Key { get; set; }
            public string Value { get; set; }
        }
    }
}
