﻿using System.Collections.Generic;

namespace Buyer.Web.Shared.ViewModels.Filters
{
    public class FilterViewModel
    {
        public string Label { get; set; }
        public string Key { get; set; }
        public IEnumerable<FilterItemViewModel> Items { get; set; }
    }
}
