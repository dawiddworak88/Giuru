using Foundation.Extensions.Models;
using System;
using System.Collections.Generic;

namespace Ordering.Api.ServicesModels.Orders
{
    public class GetOrdersByIdsServiceModel : PagedBaseServiceModel
    {
        public IEnumerable<Guid> Ids { get; set; }
        public DateTime? CreatedDateGreaterThan { get; set; }
        public bool IsSeller { get; set; }
    }
}
