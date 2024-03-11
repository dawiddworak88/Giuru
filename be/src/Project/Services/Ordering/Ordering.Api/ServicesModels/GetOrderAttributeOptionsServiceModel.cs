using Foundation.Extensions.Models;
using System;

namespace Ordering.Api.ServicesModels
{
    public class GetOrderAttributeOptionsServiceModel : PagedBaseServiceModel
    {
        public Guid? AttributeId { get; set; }
    }
}
