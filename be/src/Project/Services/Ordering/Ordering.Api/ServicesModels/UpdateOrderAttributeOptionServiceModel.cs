using Foundation.Extensions.Models;
using System;

namespace Ordering.Api.ServicesModels
{
    public class UpdateOrderAttributeOptionServiceModel : BaseServiceModel
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public Guid? OrderAttributeId { get; set; }
    }
}
