using Foundation.Extensions.Models;
using System;

namespace Ordering.Api.ServicesModels.Orders
{
    public class GetOrdersServiceModel : PagedBaseServiceModel
    {
        public DateTime? CreatedDateGreaterThan { get; set; }
        public bool IsSeller { get; set; }
    }
}
