using Foundation.Extensions.Models;
using System;

namespace Inventory.Api.ServicesModels
{
    public class CreateWarehouseServiceModel : BaseServiceModel
    {
        public string Name { get; set; }
        public string Location { get; set; }
    }
}
