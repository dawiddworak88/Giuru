using Foundation.ApiExtensions.Models.Response;

namespace Client.Api.v1.ResponseModels
{
    public class ClientResponseModel : BaseResponseModel
    {
        public Foundation.TenantDatabase.Areas.Clients.Entities.Client Client { get; set; }
    }
}
