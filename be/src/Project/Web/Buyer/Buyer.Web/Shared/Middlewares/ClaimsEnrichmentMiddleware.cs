using Buyer.Web.Shared.Configurations;
using Buyer.Web.Shared.Definitions.Middlewares;
using Buyer.Web.Shared.Repositories.Clients;
using Buyer.Web.Shared.Repositories.Global;
using Foundation.ApiExtensions.Definitions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

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

        public ClaimsEnrichmentMiddleware(
            IClientsRepository clientsRepository,
            IClientAddressesRepository clientAddressesRepository,
            IGlobalRepository globalRepository,
            IClientFieldValuesRepository clientFieldValuesRepository,
            IOptions<AppSettings> options,
            IDistributedCache cache)
        {
            _clientsRepository = clientsRepository;
            _clientAddressesRepository = clientAddressesRepository;
            _globalRepository = globalRepository;
            _clientFieldValuesRepository = clientFieldValuesRepository;
            _options = options;
            _cache = cache;
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
                var claims = cachedClaims
                    .Split('|')
                    .Select(x => x.Split(':'))
                    .ToDictionary(x => x[0], x => x[1]);

                foreach (var claim in claims)
                {
                    claimsIdentity.AddClaim(new Claim(claim.Key, claim.Value));
                }

                await next(context);
                return;
            }

            var innerStopwatch = Stopwatch.StartNew();

            var token = await context.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var client = await _clientsRepository.GetClientByEmailAsync(token, _options.Value.DefaultCulture, email);

            if (client is null)
            {
                await next(context);
                return;
            }

            var claimsToCache = new List<Claim>();

            if (client.PreferedCurrencyId.HasValue)
            {
                var currencies = await _globalRepository.GetCurrenciesAsync(token, _options.Value.DefaultCulture, null);

                if (currencies.Any())
                {
                    var currency = currencies.FirstOrDefault(c => c.Id == client.PreferedCurrencyId);

                    if (currency is not null)
                    {
                        var currencyClaim = new Claim(ClaimsEnrichmentConstants.CurrencyClaimType, currency.CurrencyCode);
                        
                        claimsIdentity.AddClaim(currencyClaim);
                        claimsToCache.Add(currencyClaim);
                    }
                }
            }

            var address = await _clientAddressesRepository.GetAsync(token, _options.Value.DefaultCulture, client.DefaultDeliveryAddressId);

            if (address is not null)
            {
                var countries = await _globalRepository.GetCountriesAsync(token, _options.Value.DefaultCulture, null);

                if (countries.Any())
                {
                    var country = countries.FirstOrDefault(c => c.Id == address.CountryId);

                    if (country != null)
                    {
                        var zipClaim = new Claim(ClaimsEnrichmentConstants.ZipCodeClaimType, $"{address.PostCode} ({address.City}, {country.Name})");
                        var countryClaim = new Claim(ClaimsEnrichmentConstants.CountryClaimType, country.Name);

                        claimsIdentity.AddClaim(zipClaim);
                        claimsIdentity.AddClaim(countryClaim);

                        claimsToCache.Add(zipClaim);
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
                    var extraPackingClaim = new Claim(ClaimsEnrichmentConstants.ExtraPackingClaimType, extraPackingField.FieldValue);

                    claimsIdentity.AddClaim(extraPackingClaim);
                    claimsToCache.Add(extraPackingClaim);
                }

                var paletteLoading = clientFieldValues.FirstOrDefault(x => x.FieldName == ClaimsEnrichmentConstants.PaletteLoadingClientFieldName);

                if (paletteLoading is not null)
                {
                    var paletteLoadingClaim = new Claim(ClaimsEnrichmentConstants.PaletteLoadingClaimType, paletteLoading.FieldValue);

                    claimsIdentity.AddClaim(paletteLoadingClaim);
                    claimsToCache.Add(paletteLoadingClaim);
                }
            }

            var claimsToCacheString = string.Join("|", claimsToCache.Select(x => $"{x.Type}:{x.Value}"));

            await _cache.SetStringAsync(cacheKey, claimsToCacheString, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
            });

            await next(context);
        }
    }
}
