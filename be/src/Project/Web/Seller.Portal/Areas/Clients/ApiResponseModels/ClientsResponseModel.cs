using Foundation.ApiExtensions.Models.Response;
using Foundation.GenericRepository.Paginations;
using System.Collections.Generic;

namespace Seller.Portal.Areas.Clients.ApiResponseModels
{
    public class ClientsResponseModel : BaseResponseModel
    {
        public PagedResults<IEnumerable<ClientResponseModel>> PagedClients { get; set; }
    }
}
