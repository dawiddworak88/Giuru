using Foundation.Extensions.Models;

namespace Inventory.Api.ServicesModels.WarehouseServiceModels
{
    public class GetWarehouseByNameServiceModel : BaseServiceModel
    {
        public string Name { get; set; }
    }
}
