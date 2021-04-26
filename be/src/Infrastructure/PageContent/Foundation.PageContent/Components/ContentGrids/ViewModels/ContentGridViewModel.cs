using System.Collections.Generic;

namespace Foundation.PageContent.Components.ContentGrids.ViewModels
{
    public class ContentGridViewModel
    {
        public string Title { get; set; }
        public IEnumerable<ContentGridItemViewModel> Items { get; set; }
    }
}
