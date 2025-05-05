using Buyer.Web.Shared.Configurations;
using Buyer.Web.Shared.Definitions.Middlewares;
using Buyer.Web.Shared.Repositories.Clients;
using Buyer.Web.Shared.Repositories.Global;
using Foundation.ApiExtensions.Definitions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
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

        public ClaimsEnrichmentMiddleware(
            IClientsRepository clientsRepository,
            IClientAddressesRepository clientAddressesRepository,
            IGlobalRepository globalRepository,
            IClientFieldValuesRepository clientFieldValuesRepository,
            IOptions<AppSettings> options)
        {
            _clientsRepository = clientsRepository;
            _clientAddressesRepository = clientAddressesRepository;
            _globalRepository = globalRepository;
            _clientFieldValuesRepository = clientFieldValuesRepository;
            _options = options;
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

            var token = await context.GetTokenAsync(ApiExtensionsConstants.TokenName);
            var claimsIdentity = (ClaimsIdentity)context.User.Identity;

            var client = await _clientsRepository.GetClientByEmailAsync(token, _options.Value.DefaultCulture, email);

            if (client is null)
            {
                await next(context);
                return;
            }

            if (client.PreferedCurrencyId.HasValue)
            {
                var currencies = await _globalRepository.GetCurrenciesAsync(token, _options.Value.DefaultCulture, null);

                if (currencies.Any())
                {
                    var currency = currencies.FirstOrDefault(c => c.Id == client.PreferedCurrencyId);

                    if (currency is not null)
                    {
                        claimsIdentity.AddClaim(new Claim(ClaimsEnrichmentConstants.CurrencyClaimType, currency.CurrencyCode));
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
                        claimsIdentity.AddClaim(new Claim(ClaimsEnrichmentConstants.ZipCodeClaimType, $"{address.PostCode} ({address.City}, {country.Name})"));
                        claimsIdentity.AddClaim(new Claim(ClaimsEnrichmentConstants.CountryClaimType, country.Name));
                    }
                }
            }

            var clientFieldValues = await _clientFieldValuesRepository.GetAsync(token, _options.Value.DefaultCulture, client.Id);

            if (clientFieldValues.Any())
            {
                var extraPackingField = clientFieldValues.FirstOrDefault(x => x.FieldName == ClaimsEnrichmentConstants.ExtraPackingClientFieldName);

                if (extraPackingField is not null)
                {
                    claimsIdentity.AddClaim(new Claim(ClaimsEnrichmentConstants.ExtraPackingClaimType, extraPackingField.FieldValue));
                }

                var paletteLoading = clientFieldValues.FirstOrDefault(x => x.FieldName == ClaimsEnrichmentConstants.PaletteLoadingClientFieldName);

                if (paletteLoading is not null)
                {
                    claimsIdentity.AddClaim(new Claim(ClaimsEnrichmentConstants.PaletteLoadingClaimType, paletteLoading.FieldValue));
                }
            }
        }
    }
}
