using Foundation.Extensions.Models;
using System;

namespace Inventory.Api.ServicesModels.WarehouseServiceModels
{
    public class DeleteWarehouseServiceModel : BaseServiceModel
    {
        public Guid? Id { get; set; }
    }
}
