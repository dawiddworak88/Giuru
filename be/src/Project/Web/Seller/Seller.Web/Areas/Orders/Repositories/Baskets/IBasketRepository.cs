using Seller.Web.Areas.Orders.DomainModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Orders.Repositories.Baskets
{
    public interface IBasketRepository
    {
        Task<Basket> SaveAsync(string token, string language, Guid? id, IEnumerable<BasketItem> items);
        Task CheckoutBasketAsync(
            string token,
            string language,
            Guid? clientId,
            string clientName,
            Guid? basketId,
            Guid? shippingAddressId,
            string shippingCompany,
            string shippingFirstName,
            string shippingLastName,
            string shippingRegion,
            string shippingPostCode,
            string shippingCity,
            string shippingStreet,
            string shippingPhoneNumber,
            Guid? shippingCountryId,
            string moreInfo);
    }
}
