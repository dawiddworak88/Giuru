using Foundation.Extensions.Models;
using System;
using System.Collections.Generic;

namespace Inventory.Api.ServicesModels.InventoryServiceModels
{
    public class GetInventoriesByProductsIdsServiceModel : BaseServiceModel
    {
        public IEnumerable<Guid> Ids { get; set; }
    }
}
