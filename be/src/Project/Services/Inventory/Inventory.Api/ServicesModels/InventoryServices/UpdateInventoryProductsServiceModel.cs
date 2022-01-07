using Foundation.Extensions.Models;
using Inventory.Api.v1.RequestModels;
using System.Collections.Generic;

namespace Inventory.Api.ServicesModels.InventoryServices
{
    public class UpdateInventoryProductsServiceModel : BaseServiceModel
    {
        public IEnumerable<UpdateInventoryProductRequestModel> InventoryItems { get; set; }
    }
}
