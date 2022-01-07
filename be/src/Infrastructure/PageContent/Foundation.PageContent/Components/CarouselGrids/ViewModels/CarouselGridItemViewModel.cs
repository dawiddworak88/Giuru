using System;
using System.Collections.Generic;

namespace Foundation.PageContent.Components.CarouselGrids.ViewModels
{
    public class CarouselGridItemViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public IEnumerable<CarouselGridCarouselItemViewModel> CarouselItems { get; set; }
    }
}
