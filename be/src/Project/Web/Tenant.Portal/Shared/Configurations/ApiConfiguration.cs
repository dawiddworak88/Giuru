using Tenant.Portal.Areas.Clients.Configurations;

namespace Tenant.Portal.Areas.Clients.Controllers.Configurations
{
    public class ApiConfiguration
    {
        public string Host { get; set; }
        public EndpointsConfiguration Endpoints { get; set; }
    }
}
