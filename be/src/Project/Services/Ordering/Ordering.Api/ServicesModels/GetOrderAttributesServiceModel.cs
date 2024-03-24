using Foundation.Extensions.Models;

namespace Ordering.Api.ServicesModels
{
    public class GetOrderAttributesServiceModel : PagedBaseServiceModel
    {
        public bool? ForOrderItems { get; set; }
    }
}
