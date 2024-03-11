using Foundation.Extensions.Models;
using System;
using System.Collections.Generic;

namespace Ordering.Api.ServicesModels
{
    public class CreateBatchOrderAttributeValuesServiceModel : BaseServiceModel
    {
        public Guid? OrderId { get; set; }
        public IEnumerable<CreateOrderAttributeValueServiceModel> Values { get; set; }
    }
}
