using Foundation.Extensions.Models;
using System;

namespace Inventory.Api.ServicesModels.InventoryServices
{
    public class UpdateInventoryBasketServiceModel : BaseServiceModel
    {
        public Guid? ProductId { get; set; }
        public int BookedQuantity { get; set; }
    }
}
