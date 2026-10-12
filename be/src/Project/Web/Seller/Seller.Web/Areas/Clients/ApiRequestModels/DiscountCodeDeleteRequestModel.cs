using Foundation.ApiExtensions.Models.Request;

namespace Seller.Web.Areas.Clients.ApiRequestModels
{
    /// <summary>The query of the Client.Api delete endpoint. Its properties become the query string.</summary>
    public class DiscountCodeDeleteRequestModel : RequestModelBase
    {
        public string Version { get; set; }
    }
}
