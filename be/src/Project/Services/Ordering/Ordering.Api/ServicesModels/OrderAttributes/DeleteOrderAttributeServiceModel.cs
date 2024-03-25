using Foundation.Extensions.Models;
using System;

namespace Ordering.Api.ServicesModels.OrderAttributes
{
    public class DeleteOrderAttributeServiceModel : BaseServiceModel
    {
        public Guid? Id { get; set; }
    }
}
