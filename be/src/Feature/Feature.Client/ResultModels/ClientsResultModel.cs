using Foundation.Extensions.Models;
using Foundation.GenericRepository.Paginations;
using System.Collections.Generic;

namespace Feature.Client.ResultModels
{
    public class ClientsResultModel : BaseServiceResultModel
    {
        public PagedResults<IEnumerable<Foundation.TenantDatabase.Areas.Clients.Entities.Client>> Clients { get; set; }
    }
}
