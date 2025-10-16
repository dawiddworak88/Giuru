using System.Collections.Generic;

namespace Buyer.Web.Shared.ViewModels.Filters
{
    public class FiltersCollectorViewModel
    {
        public string AllFilters { get; set; }
        public string SortLabel { get; set; }
        public string ClearAllFilters { get; set; }
        public string SeeResult { get; set; }
        public string FiltersLabel { get; set; }
        public IEnumerable<SortItemViewModel> SortItems { get; set; }
        public IEnumerable<FilterViewModel> FilterInputs { get; set; }
    }
}
