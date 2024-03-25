using Foundation.Extensions.Models;

namespace Ordering.Api.ServicesModels.OrderAttributes
{
    public class GetOrderAttributesServiceModel : PagedBaseServiceModel
    {
        public bool? ForOrderItems { get; set; }
    }
}
