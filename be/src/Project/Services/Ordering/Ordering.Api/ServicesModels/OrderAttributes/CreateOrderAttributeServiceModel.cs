using Foundation.Extensions.Models;

namespace Ordering.Api.ServicesModels.OrderAttributes
{
    public class CreateOrderAttributeServiceModel : BaseServiceModel
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public bool IsRequired { get; set; }
        public bool IsOrderItemAttribute { get; set; }
    }
}
