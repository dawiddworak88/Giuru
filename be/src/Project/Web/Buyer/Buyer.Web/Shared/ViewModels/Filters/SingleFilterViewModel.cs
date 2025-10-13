using System.Collections.Generic;

namespace Buyer.Web.Shared.ViewModels.Filters
{
    public class SingleFilterViewModel : FilterViewModel
    {
        public IEnumerable<FilterItemViewModel> Items { get; set; }
    }
}
