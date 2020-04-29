using Tenant.Portal.Areas.Clients.Controllers.Configurations;
using Tenant.Portal.Areas.Products.Configurations;

namespace Tenant.Portal.Shared.Configurations
{
    public class ServicesEndpointsConfiguration
    {
        public string AccountEndpoint { get; set; }

        public ClientApiConfiguration ClientApi { get; set; }

        public ProductApiConfiguration ProductApi { get; set; }
    }
}
