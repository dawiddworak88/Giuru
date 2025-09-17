using System.Collections.Generic;

namespace Foundation.PageContent.Components.CarouselGrids.ViewModels
{
    public class CarouselGridPriceViewModel
    {
        public decimal Current { get; set; }
        public string Currency { get; set; }
        public IEnumerable<CarouselGridPriceInclusionViewModel> PriceInclusions { get; set; }
    }
}
