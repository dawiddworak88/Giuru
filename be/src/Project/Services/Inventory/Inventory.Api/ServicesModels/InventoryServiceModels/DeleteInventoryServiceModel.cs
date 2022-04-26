using Foundation.Extensions.Models;
using System;

namespace Inventory.Api.ServicesModels.InventoryServiceModels
{
    public class DeleteInventoryServiceModel : BaseServiceModel
    {
        public Guid? Id { get; set; }
    }
}
