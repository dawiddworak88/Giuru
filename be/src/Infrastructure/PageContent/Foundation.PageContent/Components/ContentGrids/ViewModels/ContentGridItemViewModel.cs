using System;
using System.Collections.Generic;

namespace Foundation.PageContent.Components.ContentGrids.ViewModels
{
    public class ContentGridItemViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public IEnumerable<ContentGridCarouselItemViewModel> CarouselItems { get; set; }
    }
}
