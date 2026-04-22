using Buyer.Web.Areas.Orders.DomainModels;
using Buyer.Web.Shared.DomainModels.Clients;
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
            string clientEmail,
            Guid? basketId,
            ClientAddress billingAddress,
            ClientAddress shippingAddress,
            string moreInfo, bool hasCustomOrder,
            bool hasApprovalToSendEmail,
            IEnumerable<Guid> attachments);
    }
}
