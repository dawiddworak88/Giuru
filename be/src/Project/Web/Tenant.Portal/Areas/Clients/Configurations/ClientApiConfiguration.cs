using Tenant.Portal.Areas.Clients.Configurations;

namespace Tenant.Portal.Areas.Clients.Controllers.Configurations
{
    public class ClientApiConfiguration
    {
        public string Host { get; set; }
        public ClientEndpointsConfiguration Endpoints { get; set; }
    }
}
