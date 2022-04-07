using Foundation.Extensions.Models;
using System.Collections.Generic;

namespace Inventory.Api.ServicesModels.InventoryServices
{
    public class UpdateProductsInventoryServiceModel : BaseServiceModel
    {
        public IEnumerable<UpdateProductInventoryServiceModel> InventoryItems { get; set; }
    }
}
