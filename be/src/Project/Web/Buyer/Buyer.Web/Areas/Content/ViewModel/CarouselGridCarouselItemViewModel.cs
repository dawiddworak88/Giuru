using Foundation.PageContent.Components.Images;
using System.Collections.Generic;

namespace Buyer.Web.Areas.Content.ViewModel
{
    public class CarouselGridCarouselItemViewModel
    {
        public string Url { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public IEnumerable<SourceViewModel> Sources { get; set; }
    }
}
