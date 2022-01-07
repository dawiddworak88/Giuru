using Foundation.Extensions.Models;
using System;

namespace Inventory.Api.ServicesModels.WarehouseServices
{
    public class DeleteWarehouseServiceModel : BaseServiceModel
    {
        public Guid? Id { get; set; }
    }
}
