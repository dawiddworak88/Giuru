using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using BuyerClaimsEnrichmentConstants = Buyer.Web.Shared.Definitions.Middlewares.ClaimsEnrichmentConstants;
using ClaimsPriceClientResolver = Buyer.Web.Shared.Services.Prices.ClaimsPriceClientResolver;
using SellerAppSettings = Seller.Web.Shared.Configurations.AppSettings;
using SellerClaimsEnrichmentConstants = Seller.Web.Shared.Definitions.ClaimsEnrichmentConstants;
using SellerClient = Seller.Web.Areas.Clients.DomainModels.Client;
using SellerClientAddress = Seller.Web.Areas.Clients.DomainModels.ClientAddress;
using SellerClientFieldValue = Seller.Web.Areas.Clients.DomainModels.ClientFieldValue;
using SellerCountry = Seller.Web.Areas.Global.DomainModels.Country;
using SellerCurrency = Seller.Web.Areas.Global.DomainModels.Currency;
using IClientAddressesRepository = Seller.Web.Areas.Clients.Repositories.DeliveryAddresses.IClientAddressesRepository;
using IClientFieldValuesRepository = Seller.Web.Areas.Clients.Repositories.FieldValues.IClientFieldValuesRepository;
using ICountriesRepository = Seller.Web.Areas.Global.Repositories.ICountriesRepository;
using ICurrenciesRepository = Seller.Web.Areas.Global.Repositories.ICurrenciesRepository;
using IClientLookupService = Seller.Web.Shared.Services.Clients.IClientLookupService;
using ClientRepositoryPriceClientResolver = Seller.Web.Shared.Services.Prices.ClientRepositoryPriceClientResolver;

namespace Giuru.UnitTests.Services.Prices
{
    // ClaimsPriceClientResolver: every claim maps to the right PriceClient property, and missing
    // claims yield null - not an empty string, which matters because callers forward these values
    // straight into Grula price drivers.
    public class ClaimsPriceClientResolverTests
    {
        [Fact]
        public async Task ResolveAsync_MapsEveryClaimToTheMatchingPriceClientProperty()
        {
            var clientId = Guid.NewGuid();
            var identity = new ClaimsIdentity(new[]
            {
                new Claim(BuyerClaimsEnrichmentConstants.ClientIdClaimType, clientId.ToString()),
                new Claim(ClaimTypes.Name, "Jane Buyer"),
                new Claim(BuyerClaimsEnrichmentConstants.CurrencyClaimType, "EUR"),
                new Claim(BuyerClaimsEnrichmentConstants.ExtraPackingClaimType, "Yes"),
                new Claim(BuyerClaimsEnrichmentConstants.PaletteLoadingClaimType, "No"),
                new Claim(BuyerClaimsEnrichmentConstants.CountryClaimType, "Poland"),
                new Claim(BuyerClaimsEnrichmentConstants.ZipCodeClaimType, "00-001 (Warsaw, Poland)")
            }, "test");
            var resolver = CreateResolver(new ClaimsPrincipal(identity));

            var priceClient = await resolver.ResolveAsync(null, "SUMMER25", "token");

            Assert.Equal(clientId, priceClient.Id);
            Assert.Equal("Jane Buyer", priceClient.Name);
            Assert.Equal("EUR", priceClient.CurrencyCode);
            Assert.Equal("Yes", priceClient.ExtraPacking);
            Assert.Equal("No", priceClient.PaletteLoading);
            Assert.Equal("Poland", priceClient.Country);
            Assert.Equal("00-001 (Warsaw, Poland)", priceClient.DeliveryZipCode);
            Assert.Equal("SUMMER25", priceClient.DiscountCode);
        }

        [Fact]
        public async Task ResolveAsync_WhenClaimsAreMissing_LeavesThePropertiesNullRatherThanEmpty()
        {
            var resolver = CreateResolver(new ClaimsPrincipal(new ClaimsIdentity()));

            var priceClient = await resolver.ResolveAsync(null, null, "token");

            Assert.Null(priceClient.Id);
            Assert.Null(priceClient.Name);
            Assert.Null(priceClient.CurrencyCode);
            Assert.Null(priceClient.ExtraPacking);
            Assert.Null(priceClient.PaletteLoading);
            Assert.Null(priceClient.Country);
            Assert.Null(priceClient.DeliveryZipCode);
            Assert.Null(priceClient.DiscountCode);
        }

        [Fact]
        public async Task ResolveAsync_WhenHttpContextHasNoUser_ReturnsAllNullPropertiesExceptTheSuppliedDiscountCode()
        {
            var httpContextAccessor = Substitute.For<IHttpContextAccessor>();
            httpContextAccessor.HttpContext.Returns((HttpContext)null);
            var resolver = new ClaimsPriceClientResolver(httpContextAccessor);

            var priceClient = await resolver.ResolveAsync(null, "SUMMER25", "token");

            Assert.Null(priceClient.Id);
            Assert.Null(priceClient.Name);
            Assert.Null(priceClient.CurrencyCode);
            Assert.Equal("SUMMER25", priceClient.DiscountCode);
        }

        [Fact]
        public async Task ResolveAsync_WhenNoClientIdIsSupplied_ReadsTheAuthenticatedPrincipal()
        {
            var claimsClientId = Guid.NewGuid();
            var identity = new ClaimsIdentity(new[]
            {
                new Claim(BuyerClaimsEnrichmentConstants.ClientIdClaimType, claimsClientId.ToString())
            }, "test");
            var resolver = CreateResolver(new ClaimsPrincipal(identity));

            var priceClient = await resolver.ResolveAsync(null, null, "token");

            Assert.Equal(claimsClientId, priceClient.Id);
        }

        [Fact]
        public async Task ResolveAsync_WhenAnExplicitClientIdIsSupplied_ThrowsRatherThanPricingForSomebodyElse()
        {
            var identity = new ClaimsIdentity(new[]
            {
                new Claim(BuyerClaimsEnrichmentConstants.ClientIdClaimType, Guid.NewGuid().ToString())
            }, "test");
            var resolver = CreateResolver(new ClaimsPrincipal(identity));

            await Assert.ThrowsAsync<ArgumentException>(
                () => resolver.ResolveAsync(Guid.NewGuid(), null, "token"));
        }

        private static ClaimsPriceClientResolver CreateResolver(ClaimsPrincipal user)
        {
            var httpContext = new DefaultHttpContext { User = user };
            var httpContextAccessor = Substitute.For<IHttpContextAccessor>();
            httpContextAccessor.HttpContext.Returns(httpContext);

            return new ClaimsPriceClientResolver(httpContextAccessor);
        }
    }

    // ClientRepositoryPriceClientResolver pins the null-safe behaviour that the four inlined
    // Seller copies previously disagreed on - a missing client, missing field values, missing
    // default delivery address, missing country and missing currency must all resolve to nulls
    // rather than throwing.
    public class ClientRepositoryPriceClientResolverTests
    {
        private static readonly Guid ClientId = Guid.NewGuid();

        [Fact]
        public async Task ResolveAsync_WhenEverythingResolves_MapsRepositoryDataToPriceClient()
        {
            var countryId = Guid.NewGuid();
            var addressId = Guid.NewGuid();
            var currencyId = Guid.NewGuid();
            var client = new SellerClient
            {
                Id = ClientId,
                Name = "Acme Client",
                CountryId = countryId,
                DefaultDeliveryAddressId = addressId,
                PreferedCurrencyId = currencyId
            };
            var countries = new[] { new SellerCountry { Id = countryId, Name = "Poland" } };
            var address = new SellerClientAddress { CountryId = countryId, PostCode = "00-001", City = "Warsaw" };
            var currency = new SellerCurrency { CurrencyCode = "PLN" };
            var fieldValues = new[]
            {
                new SellerClientFieldValue { FieldName = SellerClaimsEnrichmentConstants.ExtraPackingClientFieldName, FieldValue = "true" },
                new SellerClientFieldValue { FieldName = SellerClaimsEnrichmentConstants.PaletteLoadingClientFieldName, FieldValue = "false" }
            };

            var resolver = CreateResolver(
                out var clientLookupService,
                out var countriesRepository,
                out var clientFieldValuesRepository,
                out var clientAddressesRepository,
                out var currenciesRepository);

            clientLookupService.GetAsync("token", ClientId).Returns(Task.FromResult(client));
            countriesRepository.GetAsync("token", "en-US", Arg.Any<string>()).Returns(Task.FromResult<IEnumerable<SellerCountry>>(countries));
            clientAddressesRepository.GetAsync("token", "en-US", addressId).Returns(Task.FromResult(address));
            clientFieldValuesRepository.GetAsync("token", "en-US", ClientId).Returns(Task.FromResult<IEnumerable<SellerClientFieldValue>>(fieldValues));
            currenciesRepository.GetAsync("token", "en-US", currencyId).Returns(Task.FromResult(currency));

            var priceClient = await resolver.ResolveAsync(ClientId, "SUMMER25", "token");

            Assert.Equal(ClientId, priceClient.Id);
            Assert.Equal("Acme Client", priceClient.Name);
            Assert.Equal("PLN", priceClient.CurrencyCode);
            Assert.Equal("Yes", priceClient.ExtraPacking);
            Assert.Equal("No", priceClient.PaletteLoading);
            Assert.Equal("Poland", priceClient.Country);
            Assert.Equal("00-001 (Warsaw, Poland)", priceClient.DeliveryZipCode);
            Assert.Equal("SUMMER25", priceClient.DiscountCode);
        }

        [Fact]
        public async Task ResolveAsync_WhenClientIsMissing_ReturnsNullFieldsWithoutThrowingAndKeepsTheDiscountCode()
        {
            var resolver = CreateResolver(
                out var clientLookupService,
                out var countriesRepository,
                out var clientFieldValuesRepository,
                out var clientAddressesRepository,
                out _);

            clientLookupService.GetAsync("token", ClientId).Returns(Task.FromResult<SellerClient>(null));
            countriesRepository.GetAsync("token", "en-US", Arg.Any<string>()).Returns(Task.FromResult<IEnumerable<SellerCountry>>(Array.Empty<SellerCountry>()));
            clientFieldValuesRepository.GetAsync("token", "en-US", ClientId).Returns(Task.FromResult<IEnumerable<SellerClientFieldValue>>(null));

            var priceClient = await resolver.ResolveAsync(ClientId, "SUMMER25", "token");

            Assert.Null(priceClient.Id);
            Assert.Null(priceClient.Name);
            Assert.Null(priceClient.Country);
            Assert.Null(priceClient.DeliveryZipCode);
            Assert.Null(priceClient.CurrencyCode);
            Assert.Null(priceClient.ExtraPacking);
            Assert.Null(priceClient.PaletteLoading);
            Assert.Equal("SUMMER25", priceClient.DiscountCode);
            await clientAddressesRepository.DidNotReceive().GetAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<Guid?>());
        }

        [Fact]
        public async Task ResolveAsync_WhenClientFieldValuesIsNull_LeavesExtraPackingAndPaletteLoadingNull()
        {
            var client = new SellerClient { Id = ClientId };
            var resolver = CreateResolver(
                out var clientLookupService,
                out var countriesRepository,
                out var clientFieldValuesRepository,
                out _,
                out _);

            clientLookupService.GetAsync("token", ClientId).Returns(Task.FromResult(client));
            countriesRepository.GetAsync("token", "en-US", Arg.Any<string>()).Returns(Task.FromResult<IEnumerable<SellerCountry>>(Array.Empty<SellerCountry>()));
            clientFieldValuesRepository.GetAsync("token", "en-US", ClientId).Returns(Task.FromResult<IEnumerable<SellerClientFieldValue>>(null));

            var priceClient = await resolver.ResolveAsync(ClientId, null, "token");

            Assert.Null(priceClient.ExtraPacking);
            Assert.Null(priceClient.PaletteLoading);
        }

        [Fact]
        public async Task ResolveAsync_WhenClientHasNoDefaultDeliveryAddress_LeavesDeliveryZipCodeNullAndDoesNotCallTheAddressRepository()
        {
            var client = new SellerClient { Id = ClientId, DefaultDeliveryAddressId = null };
            var resolver = CreateResolver(
                out var clientLookupService,
                out var countriesRepository,
                out var clientFieldValuesRepository,
                out var clientAddressesRepository,
                out _);

            clientLookupService.GetAsync("token", ClientId).Returns(Task.FromResult(client));
            countriesRepository.GetAsync("token", "en-US", Arg.Any<string>()).Returns(Task.FromResult<IEnumerable<SellerCountry>>(Array.Empty<SellerCountry>()));
            clientFieldValuesRepository.GetAsync("token", "en-US", ClientId).Returns(Task.FromResult<IEnumerable<SellerClientFieldValue>>(Array.Empty<SellerClientFieldValue>()));

            var priceClient = await resolver.ResolveAsync(ClientId, null, "token");

            Assert.Null(priceClient.DeliveryZipCode);
            await clientAddressesRepository.DidNotReceive().GetAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<Guid?>());
        }

        [Fact]
        public async Task ResolveAsync_WhenTheClientCountryIsNotAmongTheReturnedCountries_LeavesCountryNull()
        {
            var client = new SellerClient { Id = ClientId, CountryId = Guid.NewGuid() };
            var resolver = CreateResolver(
                out var clientLookupService,
                out var countriesRepository,
                out var clientFieldValuesRepository,
                out _,
                out _);

            clientLookupService.GetAsync("token", ClientId).Returns(Task.FromResult(client));
            countriesRepository.GetAsync("token", "en-US", Arg.Any<string>()).Returns(Task.FromResult<IEnumerable<SellerCountry>>(Array.Empty<SellerCountry>()));
            clientFieldValuesRepository.GetAsync("token", "en-US", ClientId).Returns(Task.FromResult<IEnumerable<SellerClientFieldValue>>(Array.Empty<SellerClientFieldValue>()));

            var priceClient = await resolver.ResolveAsync(ClientId, null, "token");

            Assert.Null(priceClient.Country);
        }

        [Fact]
        public async Task ResolveAsync_WhenTheCurrencyIsMissing_LeavesCurrencyCodeNull()
        {
            var client = new SellerClient { Id = ClientId, PreferedCurrencyId = Guid.NewGuid() };
            var resolver = CreateResolver(
                out var clientLookupService,
                out var countriesRepository,
                out var clientFieldValuesRepository,
                out _,
                out var currenciesRepository);

            clientLookupService.GetAsync("token", ClientId).Returns(Task.FromResult(client));
            countriesRepository.GetAsync("token", "en-US", Arg.Any<string>()).Returns(Task.FromResult<IEnumerable<SellerCountry>>(Array.Empty<SellerCountry>()));
            clientFieldValuesRepository.GetAsync("token", "en-US", ClientId).Returns(Task.FromResult<IEnumerable<SellerClientFieldValue>>(Array.Empty<SellerClientFieldValue>()));
            currenciesRepository.GetAsync("token", "en-US", Arg.Any<Guid?>()).Returns(Task.FromResult<SellerCurrency>(null));

            var priceClient = await resolver.ResolveAsync(ClientId, null, "token");

            Assert.Null(priceClient.CurrencyCode);
        }

        private static ClientRepositoryPriceClientResolver CreateResolver(
            out IClientLookupService clientLookupService,
            out ICountriesRepository countriesRepository,
            out IClientFieldValuesRepository clientFieldValuesRepository,
            out IClientAddressesRepository clientAddressesRepository,
            out ICurrenciesRepository currenciesRepository)
        {
            clientLookupService = Substitute.For<IClientLookupService>();
            countriesRepository = Substitute.For<ICountriesRepository>();
            clientFieldValuesRepository = Substitute.For<IClientFieldValuesRepository>();
            clientAddressesRepository = Substitute.For<IClientAddressesRepository>();
            currenciesRepository = Substitute.For<ICurrenciesRepository>();
            var options = Options.Create(new SellerAppSettings { DefaultCulture = "en-US" });

            return new ClientRepositoryPriceClientResolver(
                clientLookupService,
                countriesRepository,
                clientFieldValuesRepository,
                clientAddressesRepository,
                currenciesRepository,
                options);
        }
    }
}
