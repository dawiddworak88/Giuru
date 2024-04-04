using Foundation.Extensions.Models;

namespace Inventory.Api.ServicesModels.InventoryServiceModels
{
    public class GetInventorySuggestionsServiceModel : BaseServiceModel
    {
        public string SearchTerm { get; set; }
        public int Size { get; set; }
    }
}
