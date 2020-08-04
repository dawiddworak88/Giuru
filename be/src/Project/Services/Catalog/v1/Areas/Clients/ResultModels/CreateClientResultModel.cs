using Foundation.Extensions.Models;

namespace Catalog.Api.v1.Areas.Clients.ResultModels
{
    public class CreateClientResultModel : BaseServiceResultModel
    {
        public Foundation.Database.Areas.Clients.Entities.Client Client { get; set; }
    }
}
