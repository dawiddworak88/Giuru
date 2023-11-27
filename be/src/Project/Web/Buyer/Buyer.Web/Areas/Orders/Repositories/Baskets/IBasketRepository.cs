using Buyer.Web.Areas.Orders.DomainModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Orders.Repositories.Baskets
{
    public interface IBasketRepository
    {
        Task<Basket> GetBasketById(string token, string language, Guid? id);
        Task DeleteAsync(string token, string language, Guid? id);
        Task<Basket> SaveAsync(string token, string language, Guid? id, IEnumerable<BasketItem> items);
        Task CheckoutBasketAsync(
            string token, 
            string language, 
            Guid? clientId, 
            string clientName, 
            Guid? basketId,
            Guid? billingAddressId,
            string billingCompany,
            string billingFirstName,
            string billingLastName,
            string billingRegion,
            string billingPostCode,
            string billingCity,
            string billingStreet,
            string billingPhoneNumber,
            Guid? billingCountryId,
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
            string moreInfo, bool hasCustomOrder, 
            IEnumerable<Guid> attachments);
    }
}
