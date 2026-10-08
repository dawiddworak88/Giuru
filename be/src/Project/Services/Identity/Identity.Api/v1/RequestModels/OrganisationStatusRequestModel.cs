using Foundation.ApiExtensions.Models.Request;

namespace Identity.Api.v1.RequestModels
{
    public class OrganisationStatusRequestModel : RequestModelBase
    {
        public bool IsDisabled { get; set; }
    }
}
