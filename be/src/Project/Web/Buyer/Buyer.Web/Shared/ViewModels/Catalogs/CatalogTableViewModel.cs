using System.Collections.Generic;

namespace Buyer.Web.Shared.ViewModels.Catalogs
{
    public class CatalogTableViewModel
    {
        public IEnumerable<string> Labels { get; set; }
        public IEnumerable<CatalogActionViewModel> Actions { get; set; }
        public IEnumerable<CatalogPropertyViewModel> Properties { get; set; }
    }
}
