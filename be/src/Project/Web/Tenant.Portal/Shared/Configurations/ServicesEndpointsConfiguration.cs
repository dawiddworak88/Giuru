using Tenant.Portal.Areas.Clients.Controllers.Configurations;

namespace Tenant.Portal.Shared.Configurations
{
    public class ServicesEndpointsConfiguration
    {
        public string AccountEndpoint { get; set; }

        public ClientApiConfiguration ClientApi { get; set; }
    }
}
