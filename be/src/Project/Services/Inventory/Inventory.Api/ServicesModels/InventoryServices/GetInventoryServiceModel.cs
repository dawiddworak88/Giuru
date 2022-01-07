using Foundation.Extensions.Models;
using System;

namespace Inventory.Api.ServicesModels.InventoryServices
{
    public class GetInventoryServiceModel : BaseServiceModel
    {
        public Guid? Id { get; set; }
    }
}
