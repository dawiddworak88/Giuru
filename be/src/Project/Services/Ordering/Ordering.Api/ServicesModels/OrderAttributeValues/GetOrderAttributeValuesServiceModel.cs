using Foundation.Extensions.Models;
using System;

namespace Ordering.Api.ServicesModels.OrderAttributeValues
{
    public class GetOrderAttributeValuesServiceModel : PagedBaseServiceModel
    {
        public Guid? OrderId { get; set; }
        public Guid? OrderItemId { get; set; }
    }
}
