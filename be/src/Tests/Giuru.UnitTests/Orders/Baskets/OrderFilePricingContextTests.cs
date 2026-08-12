using System;
using System.Collections.Generic;
using SellerClient = Seller.Web.Areas.Clients.DomainModels.Client;
using SellerClientAddress = Seller.Web.Areas.Clients.DomainModels.ClientAddress;
using SellerCountry = Seller.Web.Areas.Global.DomainModels.Country;
using SellerOrderFilePricingContext = Seller.Web.Areas.Orders.Services.Basket.OrderFilePricingContext;

namespace Giuru.UnitTests.Orders.Baskets
{
    public sealed class OrderFilePricingContextTests
    {
        [Fact]
        public void MissingClient_ReturnsNoCountryOrDefaultAddressId()
        {
            var countryName = SellerOrderFilePricingContext.GetClientCountryName(null, null);
            var addressId = SellerOrderFilePricingContext.GetDefaultDeliveryAddressId(null);

            Assert.Null(countryName);
            Assert.Null(addressId);
        }

        [Fact]
        public void ClientWithoutCountryOrAddress_ReturnsNullValues()
        {
            var client = new SellerClient();

            var countryName = SellerOrderFilePricingContext.GetClientCountryName(client, Array.Empty<SellerCountry>());
            var addressId = SellerOrderFilePricingContext.GetDefaultDeliveryAddressId(client);

            Assert.Null(countryName);
            Assert.Null(addressId);
        }

        [Fact]
        public void NullOrEmptyCountries_DoNotThrow()
        {
            var countryId = Guid.NewGuid();
            var client = new SellerClient { CountryId = countryId };
            var address = new SellerClientAddress { CountryId = countryId };

            Assert.Null(SellerOrderFilePricingContext.GetClientCountryName(client, null));
            Assert.Null(SellerOrderFilePricingContext.GetClientCountryName(client, Array.Empty<SellerCountry>()));
            Assert.Null(SellerOrderFilePricingContext.GetDeliveryZipCode(address, null));
            Assert.Null(SellerOrderFilePricingContext.GetDeliveryZipCode(address, Array.Empty<SellerCountry>()));
        }

        [Fact]
        public void NullDeliveryAddress_ReturnsNoDeliveryZipCode()
        {
            var deliveryZipCode = SellerOrderFilePricingContext.GetDeliveryZipCode(null, new List<SellerCountry>());

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

            var countryName = SellerOrderFilePricingContext.GetClientCountryName(client, countries);
            var addressId = SellerOrderFilePricingContext.GetDefaultDeliveryAddressId(client);
            var deliveryZipCode = SellerOrderFilePricingContext.GetDeliveryZipCode(address, countries);

            Assert.Equal("Poland", countryName);
            Assert.Equal(defaultDeliveryAddressId, addressId);
            Assert.Equal("10115 (Berlin, Germany)", deliveryZipCode);
        }
    }
}