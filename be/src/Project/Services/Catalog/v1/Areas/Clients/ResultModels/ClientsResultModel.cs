using Foundation.Extensions.Models;
using Foundation.GenericRepository.Paginations;
using System.Collections.Generic;

namespace Catalog.Api.v1.Areas.Clients.ResultModels
{
    public class ClientsResultModel : BaseServiceResultModel
    {
        public PagedResults<IEnumerable<Foundation.Database.Areas.Clients.Entities.Client>> Clients { get; set; }
    }
}
