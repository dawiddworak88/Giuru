using Buyer.Web.Areas.Products.Services.DeliveryMessages;
using Buyer.Web.Shared.Configurations;
using Buyer.Web.Shared.Definitions.Middlewares;
using Buyer.Web.Shared.DomainModels.Clients;
using Buyer.Web.Shared.Repositories.Clients;
using Buyer.Web.Shared.Repositories.Global;
using Foundation.Account.Definitions;
using Foundation.ApiExtensions.Definitions;
using Foundation.Extensions.Definitions;
using Foundation.Extensions.Exceptions;
using Foundation.Extensions.ExtensionMethods;
using Foundation.Extensions.Services.Cache;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ClaimsEnrichmentConstants = Buyer.Web.Shared.Definitions.Middlewares.ClaimsEnrichmentConstants;
using SharedClaimsEnrichmentConstants = Foundation.Extensions.Definitions.ClaimsEnrichmentConstants;

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
        private readonly ILogger<ClaimsEnrichmentMiddleware> _logger;

        public ClaimsEnrichmentMiddleware(
            IClientsRepository clientsRepository,
            IClientAddressesRepository clientAddressesRepository,
            IGlobalRepository globalRepository,
            IClientFieldValuesRepository clientFieldValuesRepository,
            IOptions<AppSettings> options,
            IDistributedCache cache,
            ICacheService cacheService,
            ILogger<ClaimsEnrichmentMiddleware> logger)
        {
            _clientsRepository = clientsRepository;
            _clientAddressesRepository = clientAddressesRepository;
            _globalRepository = globalRepository;
            _clientFieldValuesRepository = clientFieldValuesRepository;
            _options = options;
            _cache = cache;
            _cacheService = cacheService;
            _logger = logger;
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

            // The client a buyer prices for is the client of their organisation. A team member signs in with an email of
            // their own, so the claims are cached per organisation and email rather than by email alone.
            var organisationId = Guid.TryParse(context.User.FindFirst(AccountConstants.Claims.OrganisationIdClaim)?.Value, out var parsedOrganisationId)
                ? parsedOrganisationId
                : (Guid?)null;
            var cacheKey = SharedClaimsEnrichmentConstants.GetClientClaimsCacheKey(email, organisationId);
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

            if (client is null && organisationId.HasValue)
            {
                // Not the primary contact of a client, so this may be a member of a client's team.
                client = await TryGetClientByOrganisationAsync(context, token);
            }

            if (client is null)
            {
                await next(context);
                return;
            }

            // The claim is added to this request's principal as well as cached: the first request after a cache miss
            // is as much in need of the client as the ones that follow.
            claimsIdentity.AddClaim(new Claim(ClaimsEnrichmentConstants.ClientIdClaimType, client.Id.ToString()));

            var claimsToCache = new List<CachedClaim>()
            {
                new CachedClaim
                {
                    Key = ClaimsEnrichmentConstants.ClientIdClaimType,
                    Value = client.Id.ToString()
                }
            };

            if (!string.IsNullOrWhiteSpace(client.Name))
            {
                claimsIdentity.AddClaim(new Claim(ClaimsEnrichmentConstants.ClientNameClaimType, client.Name));

                claimsToCache.Add(new CachedClaim
                {
                    Key = ClaimsEnrichmentConstants.ClientNameClaimType,
                    Value = client.Name
                });
            }

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

                var deliveryType = clientFieldValues.FirstOrDefault(x => x.FieldName == ClaimsEnrichmentConstants.DeliveryTypeClientFieldName);

                if (deliveryType is not null)
                {
                    var deliveryTypeName = DeliveryTypeResolver.Resolve(deliveryType.FieldValue);

                    claimsIdentity.AddClaim(new Claim(ClaimsEnrichmentConstants.DeliveryTypeClaimType, deliveryTypeName));

                    claimsToCache.Add(new CachedClaim
                    {
                        Key = ClaimsEnrichmentConstants.DeliveryTypeClaimType,
                        Value = deliveryTypeName
                    });
                }
            }

            await _cache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(claimsToCache), new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
            });

            await next(context);
        }

        private async Task<Client> TryGetClientByOrganisationAsync(HttpContext context, string token)
        {
            try
            {
                return await _clientsRepository.GetClientAsync(token, _options.Value.DefaultCulture);
            }
            catch (Exception ex) when (!context.RequestAborted.IsCancellationRequested)
            {
                // Only the status is logged: the token and the user's email stay out of the log. Nothing is cached, so
                // the next request looks the client up again.
                _logger.LogWarning(
                    "The client of the user's organisation could not be looked up ({ExceptionType}, status {StatusCode}). The user is treated as having no client.",
                    ex.GetType().Name,
                    (ex as CustomException)?.StatusCode);

                return null;
            }
        }

        private class CachedClaim
        {
            public string Key { get; set; }
            public string Value { get; set; }
        }
    }
}
