using System;
using System.Collections.Generic;
using SellerClient = Seller.Web.Areas.Clients.DomainModels.Client;
using SellerClientAddress = Seller.Web.Areas.Clients.DomainModels.ClientAddress;
using SellerCountry = Seller.Web.Areas.Global.DomainModels.Country;
using SellerPriceClientContext = Seller.Web.Shared.Services.Prices.PriceClientContext;

namespace Giuru.UnitTests.Orders.Baskets
{
    public sealed class PriceClientContextTests
    {
        [Fact]
        public void MissingClient_ReturnsNoCountryOrDefaultAddressId()
        {
            var countryName = SellerPriceClientContext.GetClientCountryName(null, null);
            var addressId = SellerPriceClientContext.GetDefaultDeliveryAddressId(null);

            Assert.Null(countryName);
            Assert.Null(addressId);
        }

        [Fact]
        public void ClientWithoutCountryOrAddress_ReturnsNullValues()
        {
            var client = new SellerClient();

            var countryName = SellerPriceClientContext.GetClientCountryName(client, Array.Empty<SellerCountry>());
            var addressId = SellerPriceClientContext.GetDefaultDeliveryAddressId(client);

            Assert.Null(countryName);
            Assert.Null(addressId);
        }

        [Fact]
        public void NullOrEmptyCountries_DoNotThrow()
        {
            var countryId = Guid.NewGuid();
            var client = new SellerClient { CountryId = countryId };
            var address = new SellerClientAddress { CountryId = countryId };

            Assert.Null(SellerPriceClientContext.GetClientCountryName(client, null));
            Assert.Null(SellerPriceClientContext.GetClientCountryName(client, Array.Empty<SellerCountry>()));
            Assert.Null(SellerPriceClientContext.GetDeliveryZipCode(address, null));
            Assert.Null(SellerPriceClientContext.GetDeliveryZipCode(address, Array.Empty<SellerCountry>()));
        }

        [Fact]
        public void NullDeliveryAddress_ReturnsNoDeliveryZipCode()
        {
            var deliveryZipCode = SellerPriceClientContext.GetDeliveryZipCode(null, new List<SellerCountry>());

            Assert.Null(deliveryZipCode);
        }

        [Fact]
        public void ValidClientAddressAndCountry_ReturnExistingCountryNameAndFormattedDeliveryZipCode()
        {
            var clientCountryId = Guid.NewGuid();
            var deliveryCountryId = Guid.NewGuid();
            var defaultDeliveryAddressId = Guid.NewGuid();
            var countries = new[]
            {
                new SellerCountry { Id = clientCountryId, Name = "Poland" },
                new SellerCountry { Id = deliveryCountryId, Name = "Germany" }
            };
            var client = new SellerClient
            {
                CountryId = clientCountryId,
                DefaultDeliveryAddressId = defaultDeliveryAddressId
            };
            var address = new SellerClientAddress
            {
                CountryId = deliveryCountryId,
                PostCode = "10115",
                City = "Berlin"
            };

            var countryName = SellerPriceClientContext.GetClientCountryName(client, countries);
            var addressId = SellerPriceClientContext.GetDefaultDeliveryAddressId(client);
            var deliveryZipCode = SellerPriceClientContext.GetDeliveryZipCode(address, countries);

            Assert.Equal("Poland", countryName);
            Assert.Equal(defaultDeliveryAddressId, addressId);
            Assert.Equal("10115 (Berlin, Germany)", deliveryZipCode);
        }
    }
}
