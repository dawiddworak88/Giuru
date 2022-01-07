using Foundation.Extensions.Models;
using System;

namespace Inventory.Api.ServicesModels.InventoryServices
{
    public class DeleteInventoryServiceModel : BaseServiceModel
    {
        public Guid? Id { get; set; }
    }
}
