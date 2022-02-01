using System.Collections.Generic;

namespace Buyer.Web.Shared.ViewModels.Catalogs
{
    public class CatalogItemProductAttributesViewModel
    {
        public string Key { get; set; }
        public IEnumerable<string> Values { get; set; }
    }
}
