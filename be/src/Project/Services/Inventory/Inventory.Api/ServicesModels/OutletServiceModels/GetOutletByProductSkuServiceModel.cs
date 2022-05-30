using Foundation.Extensions.Models;

namespace Inventory.Api.ServicesModels.OutletServiceModels
{
    public class GetOutletByProductSkuServiceModel : BaseServiceModel
    {
        public string ProductSku { get; set; }
    }
}
