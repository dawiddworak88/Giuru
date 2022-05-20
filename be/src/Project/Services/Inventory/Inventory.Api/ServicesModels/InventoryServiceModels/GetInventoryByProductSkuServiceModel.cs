using Foundation.Extensions.Models;

namespace Inventory.Api.ServicesModels.InventoryServiceModels
{
    public class GetInventoryByProductSkuServiceModel : BaseServiceModel
    {
        public string ProductSku { get; set; }
    }
}
