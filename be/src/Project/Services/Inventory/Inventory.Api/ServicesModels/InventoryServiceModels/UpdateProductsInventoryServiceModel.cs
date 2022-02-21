using Foundation.Extensions.Models;
using System.Collections.Generic;

namespace Inventory.Api.ServicesModels.InventoryServiceModels
{
    public class UpdateProductsInventoryServiceModel : BaseServiceModel
    {
        public IEnumerable<UpdateProductInventoryServiceModel> InventoryItems { get; set; }
    }
}
