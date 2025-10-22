using Foundation.Extensions.Models;
using System;

namespace Ordering.Api.ServicesModels
{
    public class GetOrdersServiceModel : PagedBaseServiceModel
    {
        public Guid? OrderStatusId { get; set; }
        public DateTime? CreatedDateGreaterThan { get; set; }
        public bool IsSeller { get; set; }
    }
}
