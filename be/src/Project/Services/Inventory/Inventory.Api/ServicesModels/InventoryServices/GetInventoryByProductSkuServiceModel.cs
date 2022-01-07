using Foundation.Extensions.Models;

namespace Inventory.Api.ServicesModels.InventoryServices
{
    public class GetInventoryByProductSkuServiceModel : BaseServiceModel
    {
        public string ProductSku { get; set; }
    }
}
