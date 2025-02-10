using Foundation.Extensions.Models;
using System;
using System.Collections.Generic;

namespace Inventory.Api.ServicesModels.OutletServiceModels
{
    public class GetOutletsByProductsIdsServiceModel : BaseServiceModel
    {
        public IEnumerable<Guid> Ids { get; set; }
    }
}
