using Foundation.Extensions.Models;

namespace Inventory.Api.ServicesModels.WarehouseServiceModels
{
    public class CreateWarehouseServiceModel : BaseServiceModel
    {
        public string Name { get; set; }
        public string Location { get; set; }
    }
}
