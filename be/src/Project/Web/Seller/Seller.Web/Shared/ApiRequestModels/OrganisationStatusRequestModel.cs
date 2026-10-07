using Foundation.ApiExtensions.Models.Request;

namespace Seller.Web.Shared.ApiRequestModels
{
    public class OrganisationStatusRequestModel : RequestModelBase
    {
        public bool IsDisabled { get; set; }
    }
}
