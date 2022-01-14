using Buyer.Web.Areas.Orders.DomainModels;
using Buyer.Web.Areas.Orders.DomainModels.BasketOrder;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Orders.Repositories.Baskets
{
    public interface IBasketRepository
    {
        Task<BasketOrder> GetBasketByOrganisation(string token, string language);
        Task<BasketOrder> DeleteItemAsync(string token, string language, Guid? id);
        Task DeleteAsync(string token, string language);
        Task<Basket> SaveAsync(string token, string language, Guid? id, IEnumerable<BasketItem> items);
        Task CheckoutBasketAsync(string token, string language, Guid? clientId, string clientName, Guid? basketId, DateTime? expectedDelivery, string moreInfo);
    }
}
