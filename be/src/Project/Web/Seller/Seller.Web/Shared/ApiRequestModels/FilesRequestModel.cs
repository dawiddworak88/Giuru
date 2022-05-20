using Foundation.ApiExtensions.Models.Request;

namespace Seller.Web.Shared.ApiRequestModels
{
    public class FilesRequestModel : RequestModelBase
    {
        public string Ids { get; set; }
        public int PageIndex { get; set; }
        public int ItemsPerPage { get; set; }
    }
}
