using Foundation.Extensions.Models;
using System;
using System.Collections.Generic;

namespace Ordering.Api.ServicesModels.OrderAttributeValues
{
    public class CreateBatchOrderAttributeValuesServiceModel : BaseServiceModel
    {
        public Guid? OrderId { get; set; }
        public Guid? OrderItemId { get; set; }
        public IEnumerable<CreateOrderAttributeValueServiceModel> Values { get; set; }
    }
}
