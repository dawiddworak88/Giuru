using System.Collections.Generic;

namespace Buyer.Web.Areas.Content.ViewModel
{
    public class CarouselGridItemViewModel
    {
        public string Title { get; set; }
        public IEnumerable<CarouselGridCarouselItemViewModel> CarouselItems { get; set; }
    }
}
