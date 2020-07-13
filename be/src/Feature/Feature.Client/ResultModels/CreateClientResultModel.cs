using Foundation.Extensions.Models;

namespace Feature.Client.ResultModels
{
    public class CreateClientResultModel : BaseServiceResultModel
    {
        public Foundation.Database.Areas.Clients.Entities.Client Client { get; set; }
    }
}
