using Foundation.Extensions.Models;
using System;

namespace Inventory.Api.ServicesModels.InventoryServiceModels
{
    public class GetInventoryByProductIdServiceModel : BaseServiceModel
    {
        public Guid? ProductId { get; set; }
    }
}
