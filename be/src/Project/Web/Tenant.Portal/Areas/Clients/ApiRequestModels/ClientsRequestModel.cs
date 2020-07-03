using Foundation.ApiExtensions.Models.Request;

namespace Tenant.Portal.Areas.Clients.ApiRequestModels
{
    public class ClientsRequestModel : BaseRequestModel
    {
        public string SearchTerm { get; set; }
        public int PageIndex { get; set; }
        public int ItemsPerPage { get; set; }
    }
}
