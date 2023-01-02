using Foundation.Extensions.Models;
using System;
using System.Collections.Generic;

namespace Inventory.Api.ServicesModels.WarehouseServiceModels
{
    public class GetWarehousesByIdsServiceModel : BaseServiceModel
    {
        public IEnumerable<Guid> Ids { get; set; }
        public int? PageIndex { get; set; }
        public int? ItemsPerPage { get; set; }
        public string OrderBy { get; set; }
    }
}
