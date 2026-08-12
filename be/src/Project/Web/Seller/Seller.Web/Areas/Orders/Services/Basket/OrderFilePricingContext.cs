using Foundation.Extensions.ExtensionMethods;
using Seller.Web.Areas.Clients.DomainModels;
using Seller.Web.Areas.Global.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Seller.Web.Areas.Orders.Services.Basket
{
    internal static class OrderFilePricingContext
    {
        internal static string GetClientCountryName(Client client, IEnumerable<Country> countries)
        {
            return client?.CountryId.HasValue is true
                ? countries.OrEmptyIfNull().FirstOrDefault(x => x.Id == client.CountryId)?.Name
                : null;
        }

        internal static Guid? GetDefaultDeliveryAddressId(Client client)
        {
            return client?.DefaultDeliveryAddressId;
        }

        internal static string GetDeliveryZipCode(ClientAddress clientAddress, IEnumerable<Country> countries)
        {
            var deliveryCountry = countries.OrEmptyIfNull().FirstOrDefault(x => x.Id == clientAddress?.CountryId);

            return clientAddress is not null && deliveryCountry is not null
                ? $"{clientAddress.PostCode} ({clientAddress.City}, {deliveryCountry.Name})"
                : null;
        }
    }
}