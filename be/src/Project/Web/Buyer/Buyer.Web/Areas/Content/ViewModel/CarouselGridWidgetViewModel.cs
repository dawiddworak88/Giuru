using System.Collections.Generic;

namespace Buyer.Web.Areas.Content.ViewModel
{
    public class CarouselGridWidgetViewModel : WidgetViewModel
    {
        public string Title { get; set; }
        public IEnumerable<CarouselGridItemWidgetViewModel> Items { get; set; }
    }
}
