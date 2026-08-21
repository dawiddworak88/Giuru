using Foundation.Extensions.ExtensionMethods;
using Foundation.Pricing.DomainModels;
using Foundation.Pricing.Services;
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

        public ClientRepositoryPriceClientResolver(
            IClientLookupService clientLookupService,
            ICountriesRepository countriesRepository,
            IClientFieldValuesRepository clientFieldValuesRepository,
            IClientAddressesRepository clientAddressesRepository,
            ICurrenciesRepository currenciesRepository,
            IOptions<AppSettings> options)
        {
            _clientLookupService = clientLookupService;
            _countriesRepository = countriesRepository;
            _clientFieldValuesRepository = clientFieldValuesRepository;
            _clientAddressesRepository = clientAddressesRepository;
            _currenciesRepository = currenciesRepository;
            _options = options;
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

            return new PriceClient
            {
                Id = client?.Id,
                Name = client?.Name,
                CurrencyCode = currency?.CurrencyCode,
                ExtraPacking = clientFieldValues.OrEmptyIfNull().FirstOrDefault(x => x.FieldName == ClaimsEnrichmentConstants.ExtraPackingClientFieldName)?.FieldValue.ToYesOrNo(),
                PaletteLoading = clientFieldValues.OrEmptyIfNull().FirstOrDefault(x => x.FieldName == ClaimsEnrichmentConstants.PaletteLoadingClientFieldName)?.FieldValue.ToYesOrNo(),
                Country = clientCountryName,
                DeliveryZipCode = deliveryZipCode,
                DiscountCode = discountCode
            };
        }
    }
}
