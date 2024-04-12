using System.Collections.Generic;

namespace Buyer.Web.Areas.Content.ViewModel
{
    public class CarouselGridItemWidgetViewModel
    {
        public string Title { get; set; }
        public IEnumerable<CarouselGridCarouselItemWidgetViewModel> CarouselItems { get; set; }
    }
}
