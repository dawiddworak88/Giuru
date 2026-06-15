using Seller.Web.Areas.Inventory.DomainModels;
using Seller.Web.Areas.Orders.DomainModels;
using System.Collections.Generic;

namespace Seller.Web.Areas.Orders.Services.Basket
{
    public interface IBasketService
    {
        void ValidateStockOutletQuantities(IEnumerable<BasketItem> items, IEnumerable<InventoryItem> inventoryItems, IEnumerable<OutletItem> outletItems);
    }
}
