using Foundation.Extensions.Models;

namespace Feature.Client.Models
{
    public class GetClientsModel : BaseServiceModel
    {
        public int PageIndex { get; set; }
        public int ItemsPerPage { get; set; }
        public string SearchTerm { get; set; }
    }
}
