using Foundation.Extensions.Exceptions;
using Foundation.Extensions.ExtensionMethods;
using Foundation.Pricing.DomainModels;
using Foundation.Pricing.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Seller.Web.Areas.Clients.Repositories.DeliveryAddresses;
using Seller.Web.Areas.Clients.Repositories.FieldValues;
using Seller.Web.Areas.Global.DomainModels;
using Seller.Web.Areas.Global.Repositories;
using Seller.Web.Shared.Configurations;
using Seller.Web.Shared.Definitions;
using Seller.Web.Shared.Services.Clients;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Seller.Web.Shared.Services.Prices
{
    /// <summary>
    /// Sellers price on behalf of a client they pick, so the client is looked up explicitly and
    /// <c>clientId</c> is required. Every read uses the configured default culture rather than the
    /// request culture, because Grula price drivers are matched on the canonical values.
    /// </summary>
    public class ClientRepositoryPriceClientResolver : IPriceClientResolver
    {
        private readonly IClientLookupService _clientLookupService;
        private readonly ICountriesRepository _countriesRepository;
        private readonly IClientFieldValuesRepository _clientFieldValuesRepository;
        private readonly IClientAddressesRepository _clientAddressesRepository;
        private readonly ICurrenciesRepository _currenciesRepository;
        private readonly IOptions<AppSettings> _options;
        private readonly IDiscountCodeEligibilityRepository _discountCodeEligibilityRepository;
        private readonly ILogger<ClientRepositoryPriceClientResolver> _logger;

        public ClientRepositoryPriceClientResolver(
            IClientLookupService clientLookupService,
            ICountriesRepository countriesRepository,
            IClientFieldValuesRepository clientFieldValuesRepository,
            IClientAddressesRepository clientAddressesRepository,
            ICurrenciesRepository currenciesRepository,
            IOptions<AppSettings> options,
            IDiscountCodeEligibilityRepository discountCodeEligibilityRepository,
            ILogger<ClientRepositoryPriceClientResolver> logger)
        {
            _clientLookupService = clientLookupService;
            _countriesRepository = countriesRepository;
            _clientFieldValuesRepository = clientFieldValuesRepository;
            _clientAddressesRepository = clientAddressesRepository;
            _currenciesRepository = currenciesRepository;
            _options = options;
            _discountCodeEligibilityRepository = discountCodeEligibilityRepository;
            _logger = logger;
        }

        public async Task<PriceClient> ResolveAsync(Guid? clientId, string discountCode, string token)
        {
            var culture = _options.Value.DefaultCulture;

            var client = await _clientLookupService.GetAsync(token, clientId);
            var countries = await _countriesRepository.GetAsync(token, culture, $"{nameof(Country.CreatedDate)} desc");
            var clientCountryName = PriceClientContext.GetClientCountryName(client, countries);
            var defaultDeliveryAddressId = PriceClientContext.GetDefaultDeliveryAddressId(client);
            var clientAddress = defaultDeliveryAddressId.HasValue
                ? await _clientAddressesRepository.GetAsync(token, culture, defaultDeliveryAddressId)
                : null;
            var deliveryZipCode = PriceClientContext.GetDeliveryZipCode(clientAddress, countries);
            var clientFieldValues = await _clientFieldValuesRepository.GetAsync(token, culture, clientId);
            var currency = await _currenciesRepository.GetAsync(token, culture, client?.PreferedCurrencyId);

            var validDiscountCode = _options.Value.IsGrulaConfigured
                ? await GetApplicableDiscountCodeAsync(token, culture, clientId, discountCode)
                : null;

            return new PriceClient
            {
                Id = client?.Id,
                Name = client?.Name,
                CurrencyCode = currency?.CurrencyCode,
                ExtraPacking = clientFieldValues.OrEmptyIfNull().FirstOrDefault(x => x.FieldName == ClaimsEnrichmentConstants.ExtraPackingClientFieldName)?.FieldValue.ToYesOrNo(),
                PaletteLoading = clientFieldValues.OrEmptyIfNull().FirstOrDefault(x => x.FieldName == ClaimsEnrichmentConstants.PaletteLoadingClientFieldName)?.FieldValue.ToYesOrNo(),
                Country = clientCountryName,
                DeliveryZipCode = deliveryZipCode,
                DiscountCode = validDiscountCode
            };
        }

        // An invalid or unverifiable code is priced as no code at all - a price lookup never fails on it.
        private async Task<string> GetApplicableDiscountCodeAsync(string token, string culture, Guid? clientId, string discountCode)
        {
            try
            {
                return await _discountCodeEligibilityRepository.GetValidDiscountCodeAsync(token, culture, clientId, discountCode);
            }
            catch (Exception exception) when (exception is CustomException or HttpRequestException or TaskCanceledException)
            {
                _logger.LogWarning(exception, "Couldn't check the Discount Code for Client {ClientId}; pricing without it.", clientId);
                return null;
            }
        }
    }
}
