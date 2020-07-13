using Seller.Portal.Areas.Clients.Configurations;

namespace Seller.Portal.Areas.Clients.Controllers.Configurations
{
    public class ApiConfiguration
    {
        public string Host { get; set; }
        public EndpointsConfiguration Endpoints { get; set; }
    }
}
