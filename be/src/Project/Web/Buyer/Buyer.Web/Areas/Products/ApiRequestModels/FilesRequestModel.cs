using Foundation.ApiExtensions.Models.Request;

namespace Buyer.Web.Areas.Products.ApiRequestModels
{
    public class FilesRequestModel : RequestModelBase
    {
        public string Ids { get; set; }
        public int PageIndex { get; set; }
        public int ItemsPerPage { get; set; }
    }
}
