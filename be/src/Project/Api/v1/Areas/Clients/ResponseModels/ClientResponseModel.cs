using Foundation.ApiExtensions.Models.Response;

namespace Api.v1.Areas.Clients.ResponseModels
{
    public class ClientResponseModel : BaseResponseModel
    {
        public Foundation.TenantDatabase.Areas.Clients.Entities.Client Client { get; set; }
    }
}
