using Foundation.ApiExtensions.Models.Request;

namespace Buyer.Web.Shared.Catalogs.ApiRequestModels
{
    public class CategoriesRequestModel : PagedRequestModelBase
    {
        public int? Level { get; set; }
    }
}
