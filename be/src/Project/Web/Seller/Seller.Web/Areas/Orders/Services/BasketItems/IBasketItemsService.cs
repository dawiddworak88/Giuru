using Seller.Web.Areas.Orders.ApiRequestModels;
using Seller.Web.Areas.Orders.DomainModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Orders.Services.BasketItems
{
    public interface IBasketItemsService
    {
        Task<IEnumerable<BasketItem>> GetBasketItemsAsync(string token, string language, IEnumerable<BasketItemRequestModel> items);
    }
}
