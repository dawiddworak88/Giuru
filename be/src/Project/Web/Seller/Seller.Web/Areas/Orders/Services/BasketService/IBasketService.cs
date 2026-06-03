using System;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Orders.Services.BasketService
{
    public interface IBasketService
    {
        Task ValidateStockOutletQuantitiesAsync(string token, string language, Guid? basketId);
    }
}
