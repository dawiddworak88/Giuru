using System.Collections.Generic;

namespace Buyer.Web.Shared.ViewModels.Filters
{
    public class NestedFilterViewModel : FilterViewModel
    {
        public IEnumerable<NestedFilterItemViewModel> Items { get; set; }
    }
}
