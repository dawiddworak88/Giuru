using Foundation.Extensions.Models;
using System;

namespace Ordering.Api.ServicesModels
{
    public class CreateOrderAttributeOptionServiceModel : BaseServiceModel
    {
        public string Name { get; set; }
        public Guid? OrderAttributeId { get; set; }
    }
}
