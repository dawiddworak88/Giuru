using System;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Orders.Services.Basket
{
    public interface IBasketService
    {
        Task ValidateStockOutletQuantitiesAsync(string token, string language, Guid? basketId);
    }
}
