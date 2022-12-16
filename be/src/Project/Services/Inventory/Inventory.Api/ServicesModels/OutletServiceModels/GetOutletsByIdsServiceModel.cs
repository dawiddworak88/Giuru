using Foundation.Extensions.Models;
using System;
using System.Collections.Generic;

namespace Inventory.Api.ServicesModels.OutletServiceModels
{
    public class GetOutletsByIdsServiceModel : BaseServiceModel
    {
        public IEnumerable<Guid> Ids { get; set; }
        public int? PageIndex { get; set; }
        public int? ItemsPerPage { get; set; }
        public string OrderBy { get; set; }
    }
}
