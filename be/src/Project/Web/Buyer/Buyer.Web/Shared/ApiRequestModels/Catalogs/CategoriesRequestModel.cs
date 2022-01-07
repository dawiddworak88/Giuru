using Foundation.ApiExtensions.Models.Request;

namespace Buyer.Web.Shared.ApiRequestModels.Catalogs
{
    public class CategoriesRequestModel : PagedRequestModelBase
    {
        public int? Level { get; set; }
    }
}
