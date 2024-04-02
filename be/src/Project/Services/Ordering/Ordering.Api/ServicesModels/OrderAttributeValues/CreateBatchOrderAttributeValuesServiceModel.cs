using Foundation.Extensions.Models;
using System;
using System.Collections.Generic;

namespace Ordering.Api.ServicesModels.OrderAttributeValues
{
    public class CreateBatchOrderAttributeValuesServiceModel : BaseServiceModel
    {
        public IEnumerable<CreateOrderAttributeValueServiceModel> Values { get; set; }
    }
}
