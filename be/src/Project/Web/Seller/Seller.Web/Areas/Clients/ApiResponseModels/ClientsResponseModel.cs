using Foundation.ApiExtensions.Models.Response;
using Foundation.GenericRepository.Paginations;
using System.Collections.Generic;

namespace Seller.Web.Areas.Clients.ApiResponseModels
{
    public class InvsResponseModel : BaseResponseModel
    {
        public PagedResults<IEnumerable<InvResponseModel>> PagedClients { get; set; }
    }
}
