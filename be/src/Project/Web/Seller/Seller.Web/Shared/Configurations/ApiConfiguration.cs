using Seller.Web.Areas.Clients.Configurations;

namespace Seller.Web.Areas.Clients.Controllers.Configurations
{
    public class ApiConfiguration
    {
        public string Host { get; set; }
        public EndpointsConfiguration Endpoints { get; set; }
    }
}
