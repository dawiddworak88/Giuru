using Foundation.Extensions.Models;

namespace Inventory.Api.ServicesModels.WarehouseServices
{
    public class GetWarehouseByNameServiceModel : BaseServiceModel
    {
        public string Name { get; set; }
    }
}
