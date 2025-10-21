using Foundation.Extensions.Models;
using System;
using System.Collections.Generic;

namespace Ordering.Api.ServicesModels
{
    public class GetOrdersByIdsServiceModel : PagedBaseServiceModel
    {
        public IEnumerable<Guid> Ids { get; set; }
        public Guid? OrderStatusId { get; set; }
        public DateTime? CreatedDateGreaterThan { get; set; }
        public bool IsSeller { get; set; }
    }
}
