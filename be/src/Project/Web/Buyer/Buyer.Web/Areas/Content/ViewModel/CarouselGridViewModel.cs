using System.Collections.Generic;

namespace Buyer.Web.Areas.Content.ViewModel
{
    public class CarouselGridViewModel : StrapiWidgetViewModel
    {
        public string Title { get; set; }
        public IEnumerable<CarouselGridItemViewModel> Items { get; set; }
    }
}
