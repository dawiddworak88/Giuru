using Buyer.Web.Areas.Orders.ApiRequestModels;
using Buyer.Web.Areas.Orders.DomainModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Orders.Services.BasketItmes
{
    public interface IBasketItemsService
    {
        Task<IEnumerable<BasketItem>> GetBasketItemsAsync(string token, string language, IEnumerable<BasketItemRequestModel> items);
    }
}
