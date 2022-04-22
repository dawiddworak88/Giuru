using Foundation.Extensions.Models;
using System;

namespace Inventory.Api.ServicesModels.WarehouseServiceModels
{
    public class GetWarehouseServiceModel : BaseServiceModel
    {
        public Guid? Id { get; set; }
    }
}
