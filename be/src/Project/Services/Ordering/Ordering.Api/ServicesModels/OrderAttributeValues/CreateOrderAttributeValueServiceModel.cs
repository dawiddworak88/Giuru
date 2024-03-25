using Foundation.Extensions.Models;
using System;

namespace Ordering.Api.ServicesModels.OrderAttributeValues
{
    public class CreateOrderAttributeValueServiceModel : BaseServiceModel
    {
        public Guid? AttributeId { get; set; }
        public string Value { get; set; }
    }
}
