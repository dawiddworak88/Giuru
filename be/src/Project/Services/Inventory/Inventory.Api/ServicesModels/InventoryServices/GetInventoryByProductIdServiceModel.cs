using Foundation.Extensions.Models;
using System;

namespace Inventory.Api.ServicesModels.InventoryServices
{
    public class GetInventoryByProductIdServiceModel : BaseServiceModel
    {
        public Guid? ProductId { get; set; }
    }
}
