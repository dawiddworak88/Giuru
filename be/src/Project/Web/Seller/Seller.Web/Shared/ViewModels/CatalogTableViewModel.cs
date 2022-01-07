using System.Collections.Generic;

namespace Seller.Web.Shared.ViewModels
{
    public class CatalogTableViewModel
    {
        public IEnumerable<string> Labels { get; set; }
        public IEnumerable<CatalogActionViewModel> Actions { get; set; }
        public IEnumerable<CatalogPropertyViewModel> Properties { get; set; }
    }
}
