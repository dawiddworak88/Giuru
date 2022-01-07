using Inventory.Api.ServicesModels.InventoryServices;
using System.Collections.Generic;

namespace Inventory.Api.v1.RequestModels
{
    public class SaveInventoriesBySkusRequestModel
    {
        public IEnumerable<UpdateInventoryProductRequestModel> InventoryItems { get; set; }
    }
}
