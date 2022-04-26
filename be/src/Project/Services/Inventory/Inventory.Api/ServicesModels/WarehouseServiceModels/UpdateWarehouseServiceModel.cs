using Foundation.Extensions.Models;
using System;

namespace Inventory.Api.ServicesModels.WarehouseServiceModels
{
    public class UpdateWarehouseServiceModel : BaseServiceModel
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
    }
}
